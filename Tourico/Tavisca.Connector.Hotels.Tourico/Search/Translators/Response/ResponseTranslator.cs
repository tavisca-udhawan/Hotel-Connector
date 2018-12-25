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

namespace Tavisca.Connector.Hotels.Tourico.Search.Translators.Response
{
    internal class ResponseTranslator
    {
        internal SearchResponse ParseResponse(SearchResult supplierResponse,string sessionId, ConnectorSearch.SearchRequest searchRequest, SupplierConfiguration supplierConfigurations)
        {
            try
            {
                var result = new SearchResponse(sessionId, GetItineries(supplierResponse, searchRequest, supplierConfigurations), new List<Warning>());
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

            var optionalDataRequired = request.OptionalFields?.Any(x => x.Equals(OptionalField.All)) ?? false;
            foreach (var hotel in searchResult.HotelList)
            {
                try
                {
                    if (!isRequestedRoomsAvailable(hotel, request.Criteria.Occupancies.Count))
                        continue;
                    Info.HotelInfo hotelInfo = ParsingHotelInfo(hotel);
                    List<ResponseOccupancy> occupancies = null;
                    List<RoomOption> roomOptions = null;
                    Rates.RoomRates roomRates = RoomRatesParser(hotel, request, supplier);
                    var numberOfNights = request.Criteria.CheckOut.Date.Subtract(request.Criteria.CheckIn.Date).Days;
              
                    var hotelItinerary = new Itinerary(hotelInfo, ParseHotelRates(hotel, request.Criteria.Occupancies, roomRates, occupancies, numberOfNights), optionalDataRequired ? roomOptions : null, optionalDataRequired ? occupancies : null, optionalDataRequired ? roomRates : null);
                    itineries.Add(hotelItinerary);
                }
                catch (Exception)
                {
                    throw new Exception("Unavailable to get response");
                }
            }

            return itineries;
        }
        private Rates.RoomRates RoomRatesParser(Hotel hotel, ConnectorSearch.SearchRequest request, SupplierConfiguration supplier)
        {
            List<PerBookingRate> bookingRate = new List<PerBookingRate>();
            return new Rates.RoomRates(bookingRate);
        }
        private ItineraryRate ParseHotelRates(Hotel hotel, List<RequestOccupancy> requestedOccupancies, Rates.RoomRates roomRates, List<ResponseOccupancy> responseOccupancies, int numberOfNights)
        {
            var itineraryRate = new ItineraryRate(hotel.currency, GetMinDailyRate(requestedOccupancies, roomRates, responseOccupancies, numberOfNights), new List<InventoryType> { InventoryType.Prepaid });

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
                var correspondingResponseOccupancy = responseOccupancies.Find(x => x.Adults == requestedOccupancy.NumOfAdults && x.Children == (requestedOccupancy.ChildAges?.Count ?? 0));
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
    }
}
