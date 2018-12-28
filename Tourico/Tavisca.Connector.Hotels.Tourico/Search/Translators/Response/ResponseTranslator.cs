using System;
using Tavisca.Connector.Hotels.Tourico.Common;
using Tavisca.Connector.Hotels.Model.Search;
using Tavisca.Connector.Hotels.ErrorHandling.Exceptions;
using Tavisca.Connector.Hotels.ErrorHandling;
using System.Net;
using Info = Tavisca.Connector.Hotels.Model.Common;
using ConnectorSearch = Tavisca.Connector.Hotels.Model.Search;
using Tavisca.Connector.Hotels.Model.Common;
using System.Collections.Generic;
using System.Linq;
using Rates = Tavisca.Connector.Hotels.Model.Search;
using System.Globalization;
using Cassandra.Mapping;

namespace Tavisca.Connector.Hotels.Tourico.Search.Translators.Response
{
    internal class ResponseTranslator
    {
        private  Criteria _criteria;
        internal SearchResponse ParseResponse(SearchResult supplierResponse,string sessionId, ConnectorSearch.SearchRequest searchRequest, SupplierConfiguration supplierConfigurations)
        {
            try
            {
                List<Itinerary> Itineries = GetItineries(supplierResponse, searchRequest, supplierConfigurations);
                var result = new SearchResponse(sessionId,Itineries , new List<Warning>());
               return result;
            }
            catch (Exception ex)
            {
                throw new ParsingException(FaultCodes.ParsingFailure, FaultMessages.ParsingFailure, HttpStatusCode.InternalServerError, ex);
            }
        }
        private List<Itinerary> GetItineries(SearchResult searchResult, ConnectorSearch.SearchRequest request, SupplierConfiguration supplier)
        {
            var itineries = new List<Itinerary>();
            _criteria = request.Criteria;
            var optionalDataRequired = request.OptionalFields?.Any(x => x.Equals(OptionalField.All)) ?? false;
            foreach (var hotel in searchResult.HotelList)
            {
                try
                {
                    Info.HotelInfo hotelInfo = ParsingHotelInfo(hotel);
                    List<ResponseOccupancy> occupancies = null;
                    List<RoomOption> roomOptions =null;
                    Rates.RoomRates roomRates = RoomRatesParser(hotel, request, supplier,out occupancies,out roomOptions);
                    var numberOfNights = request.Criteria.CheckOut.Date.Subtract(request.Criteria.CheckIn.Date).Days;
                    var parseHotelRates = ParseHotelRates(hotel, request.Criteria.Occupancies, roomRates, occupancies, numberOfNights);
                    var hotelItinerary = new Itinerary(hotelInfo, parseHotelRates, optionalDataRequired ? roomOptions : null, optionalDataRequired ? occupancies : null, optionalDataRequired ? roomRates : null);
                    itineries.Add(hotelItinerary);
                }
                catch (Exception)
                {
                    throw new Exception("Unavailable to get response");
                }
            }
           return itineries;
        }
       
        private Rates.RoomRates RoomRatesParser(Hotel hotel, ConnectorSearch.SearchRequest request, SupplierConfiguration supplier,out List<ResponseOccupancy> occupancies,out List<RoomOption> roomOptions)
        {
            var roomRates = new Rates.RoomRates(new List<PerRoomRate>());
            var perRoomRates = new List<PerRoomRate>();
            roomOptions = new List<RoomOption>();
            occupancies = new List<ResponseOccupancy>();
            foreach (var roomTypes in hotel.RoomTypes.Where(roomAvailability => roomAvailability.isAvailable))
            {
                foreach (var occupancy in roomTypes.Occupancies)
                {
                    var rooms = new List<Room>();
                    if (occupancy.Rooms.Length > 0)
                    {
                        foreach (var room in occupancy.Rooms)
                        {
                            rooms.Add(room);
                        }
                    }
                    else
                        rooms.Add(rooms[0]);

                    //parsing room options
                    var roomOption = ParseRoomOption(roomTypes, occupancy);
                    roomOptions.Add(roomOption);

                    //parsing boardbases
                    List<Boardbase> freeBoardbases = new List<Boardbase>();
                    List<Boardbase> paidBoardbases = new List<Boardbase>();
                    if (occupancy.BoardBases.Length>0 && occupancy.BoardBases!=null)
                    {
                      foreach(var boardbase in occupancy.BoardBases)
                        {
                            if (boardbase.bbPrice.Equals(0))
                            {
                                freeBoardbases.Add(boardbase);
                            }
                            else
                                paidBoardbases.Add(boardbase);
                        }  
                    }
                    //supplements
                    var supplementsRequired=occupancy.SelctedSupplements!=null?occupancy.SelctedSupplements.Where(supplements => supplements.suppIsMandatory).ToList():new List<Supplement>();

                    if (rooms.Count > 0)
                    {
                        foreach(var room in rooms)
                        {
                            var occupanciesRefId = ParseOccupancy(room, occupancies);
                            var parseRates = ParseRates(hotel,supplier,occupanciesRefId, roomOption.RefId, roomTypes, occupancy, freeBoardbases, paidBoardbases, supplementsRequired);
                            roomRates.PerRoomRates.AddRange(parseRates);
                        }
                    }
                    else
                    {
                        var occupanciesRefId = ParseOccupancy(rooms[0], occupancies);
                        var parseRates = ParseRates(hotel,supplier,occupanciesRefId, roomOption.RefId, roomTypes, occupancy, freeBoardbases, paidBoardbases, supplementsRequired);
                        roomRates.PerRoomRates.AddRange(parseRates);
                    }
                    
                }
            }

            return roomRates;
        }
        private List<PerRoomRate> ParseRates(Hotel hotel,SupplierConfiguration configurations,string occupancyRefId, string roomRefId,
            RoomType roomType, Occupancy occupancy, List<Boardbase> freeBoardBases, List<Boardbase> paidBoardBases,List<Supplement> mandatorySupplements)
        {
            var perRoomRates = new List<PerRoomRate>();
            var rateOccupancies = ParseRateOccupancy(roomRefId, occupancyRefId);

            if (paidBoardBases.Count > 0)
            {
                perRoomRates.AddRange(from boardBasis in paidBoardBases
                                      select ParsePerRoomRate(hotel,configurations,boardBasis, roomType, occupancy, rateOccupancies, mandatorySupplements));
            }
            if (freeBoardBases.Count > 0)
            {
                perRoomRates.AddRange(from boardBasis in freeBoardBases
                                      select ParsePerRoomRate(hotel,configurations,boardBasis, roomType, occupancy, rateOccupancies, mandatorySupplements));
            }
            else
                perRoomRates.Add(ParsePerRoomRate(hotel, configurations,null, roomType, occupancy, rateOccupancies, mandatorySupplements));
        
            return perRoomRates;
        }
        private PerRoomRate ParsePerRoomRate(Hotel hotel,SupplierConfiguration configurations,Boardbase boardBase, RoomType roomType, Occupancy occupancy, RateOccupancy rateOccupancy, List<Supplement> mandatorySupplements)
        {
            var code = occupancy.occupId;
            var rateType = configurations.IsPublished ? RateType.Published : RateType.Negotiated;
            var refundable = roomType.isNonRefundableSpecified ? !roomType.isNonRefundable : (bool?)null;
            var boardBasis = boardBase != null ? ParseBoardBasis(boardBase) : null;
            var supplementPrice = ParseSupplementPrice(mandatorySupplements);
            var totalPrice = occupancy.occupPrice + (boardBase?.bbPrice ?? 0) + supplementPrice;
            var baseRate = totalPrice - occupancy.tax;
            var taxesAndFees = ParseTaxesAndFees(occupancy);
            var discount = ParseDiscount(roomType);
            var inclusions = ParseInclusions(mandatorySupplements);
            var additionalCharges = GetAdditionalCharges(hotel,mandatorySupplements);
            var additionalChargesInfo = GetAdditionalChargesInfo(hotel,mandatorySupplements);
            var offerParsing = ParseOffer(hotel, roomType);
            var bookingRequirementParsing = ParseBookingRequirement();
            var rackRateParsing = ParseRackRate(boardBase, occupancy, supplementPrice);
            return new PerRoomRate(rateOccupancy, code, null, rateType,
                InventoryType.Prepaid, hotel.currency,
                totalPrice, new RateBreakup(baseRate, taxesAndFees, discount),
                ParseDailyRoomRate(occupancy),
                bookingRequirementParsing, refundable, false, null,
                boardBasis, additionalCharges, additionalChargesInfo, null, inclusions,offerParsing , null,rackRateParsing );
        }
            private RateOccupancy ParseRateOccupancy(string roomRefId,string occupancyrefId)
        {
            return new RateOccupancy(Guid.NewGuid().ToString(),roomRefId,occupancyrefId);
        }
        private string ParseOccupancy(Room room, List<ResponseOccupancy> occupancies)
        {
            var matchingOccupancy = occupancies.FirstOrDefault(occupancyCheck => occupancyCheck.Adults == room.AdultNum && occupancyCheck.Children==room.ChildNum);
            if (matchingOccupancy != null)
            {
                return matchingOccupancy.RefId;
            }
            var occupancy = new ResponseOccupancy(Guid.NewGuid().ToString(),room.AdultNum,room.ChildNum);
            occupancies.Add(occupancy);
            return occupancy.RefId;
        }
        private RoomOption ParseRoomOption(RoomType roomTypes,Occupancy occupancy)
        {
            List<BedDetail> bedDetails = ParseBedDetails(occupancy.bedding);
            return new RoomOption(Guid.NewGuid().ToString(),roomTypes.hotelRoomTypeId.ToString(),roomTypes.roomTypeCategoryId.ToString(),roomTypes.name,null,occupancy.Rooms.Length,bedDetails,occupancy.maxGuests,SmokingPreference.Unknown);
        }
        static List<BedDetail> ParseBedDetails(string bedding)
        {
            List<BedDetail> details = new List<BedDetail>();
            int bedCount=0;
            string description = null;
            if (!string.IsNullOrEmpty(bedding))
            {
                var bedOccupancy = bedding.Split(',');

                if (bedOccupancy.Length > 1)
                {
                    bedCount = Convert.ToInt32(bedOccupancy[1]);
                    description = $"{bedOccupancy[1]} bed for {bedOccupancy[0]} guest";
                }

                details.Add(new BedDetail(null, description, bedCount));
            }
            return details;
        }
        private ItineraryRate ParseHotelRates(Hotel hotel, List<RequestOccupancy> requestedOccupancies, Rates.RoomRates roomRates, List<ResponseOccupancy> responseOccupancies, int numberOfNights)
        {
            decimal minDailyRates = GetMinDailyRate(requestedOccupancies, roomRates, responseOccupancies, numberOfNights);
            var itineraryRate = new ItineraryRate(hotel.currency, minDailyRates, new List<InventoryType> { InventoryType.Prepaid });

            return itineraryRate;
        }

        private decimal GetMinDailyRate(List<RequestOccupancy> requestedOccupancies, Rates.RoomRates roomRates, List<ResponseOccupancy> responseOccupancies, int numberOfNights)
        {
            var minDailyRate = 0m;
            var occupancyWiseMinRates = new Dictionary<string, decimal>();
            foreach (var roomRate in roomRates.PerRoomRates)
            {
                decimal min;
                if (!occupancyWiseMinRates.TryGetValue(roomRate.RateOccupancy.OccupancyRefId, out min) || min > roomRate.Total)
                    occupancyWiseMinRates[roomRate.RateOccupancy.OccupancyRefId] = roomRate.Total;
            }
            foreach (var requestedOccupancy in requestedOccupancies)
            {
                var correspondingResponseOccupancy = responseOccupancies.Find(x => x.Adults == requestedOccupancy.NumOfAdults && x.Children == (requestedOccupancy.ChildAges.Count>0? requestedOccupancy.ChildAges.Count:0));
                decimal min;
                if (correspondingResponseOccupancy != null && occupancyWiseMinRates.TryGetValue(correspondingResponseOccupancy.RefId, out min))
                    minDailyRate += min;
            }
            return minDailyRate / numberOfNights;
        }
        static Info.HotelInfo ParsingHotelInfo(Hotel hotel)
        {
            return new Info.HotelInfo(hotel.hotelId.ToString(CultureInfo.InvariantCulture), hotel.name);
        }
        private static bool isRequestedRoomsAvailable(Hotel hotel, int roomCount)
        {
            var roomSeqNumbers = new List<int>();
            foreach (var roomType in hotel.RoomTypes.Where(x => x != null && x.isAvailable))
            {
                foreach (var occupancy in roomType.Occupancies)
                {
                    foreach (var room in occupancy.Rooms)
                    {
                        if (!roomSeqNumbers.Contains(room.seqNum))
                            roomSeqNumbers.Add(room.seqNum);
                    }
                }
            }
               return true;
        }

        private List<string> GetAdditionalCharges(Hotel hotel,List<Supplement> supplements)
        {
            return (from supplement in supplements
             where supplement.suppChargeType == ChargeType.AtProperty
             select string.Format(supplement.suppName, hotel.currency, supplement.price)).ToList();
}

        private List<AdditionalChargeInfo> GetAdditionalChargesInfo(Hotel hotel,List<Supplement> supplements)
        {
            var additionalChargeInformation = new List<AdditionalChargeInfo>();
            foreach (var supplement in supplements.Where(supplementType => supplementType.suppChargeType == ChargeType.AtProperty))
            {
                if (supplement != null)
                {
                    var charge = new AdditionalCharge
                    {
                        Description = supplement.suppName,
                        Amount = supplement.price,
                        Currency = hotel.currency,
                        Type = AdditionalChargeType.ResortFee,
                        Unit = AdditionalChargeUnit.PerRoom,
                        Frequency = AdditionalChargeFrequency.PerNight
                    };
                    additionalChargeInformation.Add(new AdditionalChargeInfo { Charge = charge,
                         Text=null});
                }
            }
            return additionalChargeInformation;
        }

        private BookingRequirement ParseBookingRequirement()
        {
            return new BookingRequirement(false, false);
        }

        private List<FareComponent> ParseTaxesAndFees(Occupancy occupancy)
        {
            var taxes = new List<FareComponent>();

            if (occupancy.TaxBreakdown != null && occupancy.TaxBreakdown.Length > 0)
            {
               
                taxes.AddRange(from tax in occupancy.TaxBreakdown
                               select new FareComponent(tax.value, false, null, tax.type.ToString()));
            }
            else
            {
                taxes.Add(new FareComponent(occupancy.tax, false, "TotalTax"));
            }

            return taxes;
        }

        private decimal ParseRackRate(Boardbase boardBase, Occupancy occupancy, decimal supplementPrice)
        {
            return occupancy.occupPublishPrice + (boardBase?.bbPublishPrice ?? 0) +
                  supplementPrice;
            // return occupancy.occupPublishPrice + boardBase.bbPublishPrice +supplementPrice;
        }

        private DailyRoomRate ParseDailyRoomRate(Occupancy occupancy)
        {
            var dailyRoomRateBreakup = new List<DailyRoomRateBreakup>();
            if (occupancy.PriceBreakdown != null && occupancy.PriceBreakdown.Length > 0)
            {
                var prices = occupancy.PriceBreakdown;
                dailyRoomRateBreakup.AddRange(from price in prices
                                               select new DailyRoomRateBreakup(_criteria.CheckIn.AddDays(price.offset), price.value,null));
            }

            return dailyRoomRateBreakup.Count > 0 ? new DailyRoomRate(dailyRoomRateBreakup, true) : null;
        }

        private Offer ParseOffer(Hotel hotel,RoomType roomType)
        {
            if (roomType.Discount != null)
            {
                if((_criteria.CheckIn>=roomType.Discount.from &&_criteria.CheckIn<=roomType.Discount.to)&&
                    (_criteria.CheckOut >= roomType.Discount.from && _criteria.CheckOut <= roomType.Discount.to))
                {
                    var progressivePromotion = roomType.Discount as ProgressivePromotion;
                    var payStayPromotion = roomType.Discount as PayStayPromotion;
                    if (progressivePromotion != null)
                    {
                        if (progressivePromotion.type.Equals(ProgressiveTypes.Amount))
                        {
                            return new Offer (string.IsNullOrWhiteSpace(progressivePromotion.name) ? progressivePromotion.name:null,null,progressivePromotion.value);
                        }
                        if (progressivePromotion.type.Equals(ProgressiveTypes.Percent))
                        {
                            var percentageDiscountOffer = new PercentageDiscountOffer(progressivePromotion.value,AppliedOn.TotalRate);
                            return new Offer(string.IsNullOrWhiteSpace(progressivePromotion.name) ? progressivePromotion.name : null, null, percentageDiscountOffer);
                        }
                    }
                    else if (payStayPromotion != null)
                    {
                        var payStayOffer = new PayStayOffer(payStayPromotion.stay, (payStayPromotion.stay - payStayPromotion.pay));
                        return new Offer(null, null, payStayOffer);
                    }
                }
            }
            return null;
        }

        private Discount ParseDiscount(RoomType roomType)
        {
           if(roomType.Discount==null)
            return null;
           if((roomType.Discount.from<=_criteria.CheckIn && roomType.Discount.to>=_criteria.CheckIn) &&
                (roomType.Discount.from <= _criteria.CheckOut && roomType.Discount.to >= _criteria.CheckOut))
            {
                var progressivePromotion = roomType.Discount as ProgressivePromotion;
                var payStaypromotion = roomType.Discount as PayStayPromotion;    
                if (progressivePromotion != null)
                {
                    if (progressivePromotion.type.Equals(ProgressiveTypes.Amount))
                    {
                        var desc = progressivePromotion.name != null ?progressivePromotion.name: null;
                        return new Discount(progressivePromotion.value,true,desc);
                    }
                }
            }
            return null;
        }

        private BoardBasis ParseBoardBasis(Boardbase boardBase)
        {
            return new BoardBasis(boardBase.bbId.ToString(), boardBase.bbName,BoardBasisType.RoomOnly, boardBase.bbPrice, true);
        }

        private decimal ParseSupplementPrice(List<Supplement> supplements)
        {
            return supplements.Where(supplement => supplement.suppChargeType == ChargeType.Addition)
               .Aggregate(0m, (current, supplement) => current + supplement.price);
        }

        private List<string> ParseInclusions(List<Supplement> supplements)
        {
            return (from supplement in supplements  where supplement.suppChargeType != ChargeType.AtProperty
                    select supplement.suppName).ToList();
        }
    }
}


