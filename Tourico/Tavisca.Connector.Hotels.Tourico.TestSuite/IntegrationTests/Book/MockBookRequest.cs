using System.Collections.Generic;
using Tavisca.Connector.Hotels.Model.Book;
using Tavisca.Connector.Hotels.Model.Common;
using Tavisca.Connector.Hotels.Model.RateRules;
using Tavisca.Connector.Hotels.Model.RoomRates;
using Tavisca.Connector.Hotels.Model.Search;
using ConnectorSearch = Tavisca.Connector.Hotels.Model.Search;
namespace Tavisca.Connector.Hotels.Tourico.TestSuite.IntegrationTests.Book
{
    public class MockBookRequest : BaseMockRequest
    {
        private static string _bookingToken = "";

        public static BookRequest InValidCreditCardInRequest()
        {
            List<RoomPaxDistribution> roomPaxDistributionList = new List<RoomPaxDistribution>()
            {
               new RoomPaxDistribution("RoomCode1", new List<Guest>()
               {
                        new Guest(PassengerType.Adult, new Name("Testfirstname1", "TestLastname1", "TestLastName1", Title.Mr, string.Empty), 35),
                        new Guest(PassengerType.Child, new Name("Testfirstname2", "TestLastname2", "TestLastName2", Title.Master, string.Empty), 7)
               })
            };

            Model.Book.Criteria criteria = new Model.Book.Criteria(GetHotelID(), _checkinDate, _checkoutDate, _bookingToken, new Model.Book.RoomRate(new Model.Book.PerBookingRateRequest("PerBookingRequest1", "ratecode313", roomPaxDistributionList)));

            return new BookRequest(GetSessionId(), criteria, GetBookingContact(), CreateSupplierObject(), new BookRequestOptions("early check in"), GetInValidCardDetails());
        }

        public static BookRequest FirstNameLongerThanMaxLengthInRequest()
        {
            List<RoomPaxDistribution> roomPaxDistributionList = new List<RoomPaxDistribution>()
            {
               new RoomPaxDistribution("RoomCode1", new List<Guest>()
               {
                        new Guest(PassengerType.Adult, new Name("Testfirstname1", "TestLastname1", "TestLastName1", Title.Mr, string.Empty), 35),
                        new Guest(PassengerType.Child, new Name("Testfirstname2", "TestLastname2", "TestLastName2", Title.Master, string.Empty), 7)
               })
            };

            Model.Book.Criteria criteria = new Model.Book.Criteria(GetHotelID(), _checkinDate, _checkoutDate, _bookingToken, new Model.Book.RoomRate(new Model.Book.PerBookingRateRequest("PerBookingRequest1", "ratecode313", roomPaxDistributionList)));

            ContactInfo contactInfo = new ContactInfo(new Phone(PhoneType.Mobile, "1234567890", "91", "4564", "4324"), new Address("pune", "sds", new Model.Common.City("Pune1", "Pune"), new State("MH", "MH"), "IN", "411"), "testmail@testemail.com");
            BookingContact bookingContact = new BookingContact(new Name("Testfirstname11111111111111111111111111111111111111", "TestLastName1", string.Empty, Title.Mr, string.Empty), 35, contactInfo, null);

            return new BookRequest(GetSessionId(), criteria, bookingContact, CreateSupplierObject(), new BookRequestOptions("early check in"), GetCardDetails());
        }

        public static BookRequest LastNameLongerThanMaxLengthInRequest()
        {
            List<RoomPaxDistribution> roomPaxDistributionList = new List<RoomPaxDistribution>()
            {
               new RoomPaxDistribution("RoomCode1", new List<Guest>()
               {
                        new Guest(PassengerType.Adult, new Name("Testfirstname1", "TestLastname1", "TestLastName1", Title.Mr, string.Empty), 35),
                        new Guest(PassengerType.Child, new Name("Testfirstname2", "TestLastname2", "TestLastName2", Title.Master, string.Empty), 7)
               })
            };

            Model.Book.Criteria criteria = new Model.Book.Criteria(GetHotelID(), _checkinDate, _checkoutDate, _bookingToken, new Model.Book.RoomRate(new Model.Book.PerBookingRateRequest("PerBookingRequest1", "ratecode313", roomPaxDistributionList)));

            ContactInfo contactInfo = new ContactInfo(new Phone(PhoneType.Mobile, "1234567890", "91", "4564", "4324"), new Address("pune", "sds", new Model.Common.City("Pune1", "Pune"), new State("MH", "MH"), "IN", "411"), "testmail@testemail.com");
            BookingContact bookingContact = new BookingContact(new Name("Testfirstname1", "TestLast11111111111111111111111111111111111111111111", string.Empty, Title.Mr, string.Empty), 35, contactInfo, null);

            return new BookRequest(GetSessionId(), criteria, bookingContact, CreateSupplierObject(), new BookRequestOptions("early check in"), GetCardDetails());
        }

        public static BookRequest FirstAndLastNameLongerThanMaxLengthInRequest()
        {
            List<RoomPaxDistribution> roomPaxDistributionList = new List<RoomPaxDistribution>()
            {
               new RoomPaxDistribution("RoomCode1", new List<Guest>()
               {
                        new Guest(PassengerType.Adult, new Name("Testfirstname1", "TestLastname1", "TestLastName1", Title.Mr, string.Empty), 35),
                        new Guest(PassengerType.Child, new Name("Testfirstname2", "TestLastname2", "TestLastName2", Title.Master, string.Empty), 7)
               })
            };

            Model.Book.Criteria criteria = new Model.Book.Criteria(GetHotelID(), _checkinDate, _checkoutDate, _bookingToken, new Model.Book.RoomRate(new Model.Book.PerBookingRateRequest("PerBookingRequest1", "ratecode313", roomPaxDistributionList)));

            ContactInfo contactInfo = new ContactInfo(new Phone(PhoneType.Mobile, "1234567890", "91", "4564", "4324"), new Address("pune", "sds", new Model.Common.City("Pune1", "Pune"), new State("MH", "MH"), "IN", "411"), "testmail@testemail.com");
            BookingContact bookingContact = new BookingContact(new Name("Testfirstname11111111111111111111111111111111111111", "TestLastname1111111111111111111111111111111111111111", string.Empty, Title.Mr, string.Empty), 35, contactInfo, null);

            return new BookRequest(GetSessionId(), criteria, bookingContact, CreateSupplierObject(), new BookRequestOptions("early check in"), GetCardDetails());
        }

        public static BookRequest FirstAndLastNameEqualToMaxLengthInRequest(RateRulesResponse rateRulesResponse)
        {
            List<RoomPaxDistribution> roomPaxDistributionList = new List<RoomPaxDistribution>()
            {
               new RoomPaxDistribution("RoomCode1", new List<Guest>()
               {
                        new Guest(PassengerType.Adult, new Name("Testfirstname1", "TestLastname1", "TestLastName1", Title.Mr, string.Empty), 35),
                        new Guest(PassengerType.Child, new Name("Testfirstname2", "TestLastname2", "TestLastName2", Title.Master, string.Empty), 7)
               })
            };

            Model.Book.Criteria criteria = new Model.Book.Criteria(GetHotelID(), _checkinDate, _checkoutDate, _bookingToken, new Model.Book.RoomRate(new Model.Book.PerBookingRateRequest("PerBookingRequest1", "ratecode313", roomPaxDistributionList)));

            ContactInfo contactInfo = new ContactInfo(new Phone(PhoneType.Mobile, "1234567890", "91", "4564", "4324"), new Address("pune", "sds", new Model.Common.City("Pune1", "Pune"), new State("MH", "MH"), "IN", "411"), "testmail@testemail.com");
            BookingContact bookingContact = new BookingContact(new Name("TestFirstName1111111111111111111111111111111111111", "TestLastName11111111111111111111111111111111111111", string.Empty, Title.Mr, string.Empty), 35, contactInfo, null);

            return new BookRequest(GetSessionIdFromRateRulesResponse(rateRulesResponse), criteria, bookingContact, CreateSupplierObject(), new BookRequestOptions("early check in"), GetCardDetails());
        }

        public static BookRequest FirstAndLastNameLessThanMaxLengthInRequest(RateRulesResponse rateRulesResponse)
        {
            List<RoomPaxDistribution> roomPaxDistributionList = new List<RoomPaxDistribution>()
            {
               new RoomPaxDistribution("RoomCode1", new List<Guest>()
               {
                        new Guest(PassengerType.Adult, new Name("Testfirstname1", "TestLastname1", "TestLastName1", Title.Mr, string.Empty), 35),
                        new Guest(PassengerType.Child, new Name("Testfirstname2", "TestLastname2", "TestLastName2", Title.Master, string.Empty), 7)
               })
            };

            Model.Book.Criteria criteria = new Model.Book.Criteria(GetHotelID(), _checkinDate, _checkoutDate, _bookingToken, new Model.Book.RoomRate(new Model.Book.PerBookingRateRequest("PerBookingRequest1", "ratecode313", roomPaxDistributionList)));

            ContactInfo contactInfo = new ContactInfo(new Phone(PhoneType.Mobile, "1234567890", "91", "4564", "4324"), new Address("pune", "sds", new Model.Common.City("Pune1", "Pune"), new State("MH", "MH"), "IN", "411"), "testmail@testemail.com");
            BookingContact bookingContact = new BookingContact(new Name("TestName11TestName11", "TestName22TestName22", string.Empty, Title.Mr, string.Empty), 35, contactInfo, null);

            return new BookRequest(GetSessionIdFromRateRulesResponse(rateRulesResponse), criteria, bookingContact, CreateSupplierObject(), new BookRequestOptions("early check in"), GetCardDetails());
        }

        public static RateRulesRequest BasicRateRules_PerBooking_Request(Response roomRatesResponse)
        {
            RequestOccupancy requestOccupancy = new RequestOccupancy(1, new List<int>() { 4, 5 });

            List<RoomOccupancyDetail> listRoomOccupancyDetail = new List<RoomOccupancyDetail>()
            {
                new RoomOccupancyDetail("roomcode1", requestOccupancy)
            };

            Model.RateRules.Criteria criteria = new Model.RateRules.Criteria(GetHotelIDFromRoomRatesResponse(roomRatesResponse), _checkinDate, _checkoutDate, new RequestRoomRate(new Model.RateRules.PerBookingRateRequest("id", "code", listRoomOccupancyDetail)));

            return new RateRulesRequest(GetSessionIdFromRoomRatesRespons(roomRatesResponse), criteria, CreateSupplierObject());
        }

        public static ConnectorSearch.SearchRequest BasicSearch_Request()
        {
            List<RequestOccupancy> requestOccupancies = new List<RequestOccupancy>()
            {
                new RequestOccupancy(2, new List<int>() {2, 10 }),
                new RequestOccupancy(1, new List<int>() {3, 7 })
            };

            Model.Search.Criteria criteria = new Model.Search.Criteria(_checkinDate, _checkoutDate, GetSearchRequestHotelIDs(), requestOccupancies);
            return new ConnectorSearch.SearchRequest(GetSessionId(), criteria, CreateSupplierObject());
        }

        public static Request BasicRoomRates_Request(SearchResponse searchResponse)
        {
            List<RequestOccupancy> requestOccupancies = new List<RequestOccupancy>()
            {
                new RequestOccupancy(2, new List<int>() {2, 10 }),
                new RequestOccupancy(1, new List<int>() {3, 7 })
            };

            Model.RoomRates.Criteria criteria = new Model.RoomRates.Criteria(GetHotelIDFromSearchResponse(searchResponse), _checkinDate, _checkoutDate, requestOccupancies);
            return new Request(GetSessionIdFromSearchResponse(searchResponse), criteria, CreateSupplierObject());
        }
        
    }
}
