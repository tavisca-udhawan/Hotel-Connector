using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tavisca.Connector.Hotels.Model.Book;
using Tavisca.Connector.Hotels.Model.Common;
using Tavisca.Connector.Hotels.Model.RateRules;
using Tavisca.Connector.Hotels.Model.RoomRates;
using Tavisca.Connector.Hotels.Model.Search;

namespace Tavisca.Connector.Hotels.Tourico.TestSuite.IntegrationTests
{
    public class BaseMockRequest
    {
        protected static DateTime _checkinDate = DateTime.Now.AddMonths(1);
        protected static DateTime _checkoutDate = _checkinDate.AddDays(2);

        protected static string GetSessionId()
        {
            return new Random().Next(1001, 9999).ToString();
        }

        protected static TokenizedCard GetCardDetails()
        {
            List<Phone> phoneNumerList = new List<Phone>()
            {
                new Phone(PhoneType.Home, "13213123131","91","341", "123")
            };
            TokenizedCard card = new TokenizedCard("4231********1234", "Test name", CardIssuer.DS, new Expiry(1, 2020), "123", new Address("pune", "pune", 
                new Model.Common.City("pune1", "pune"), new State("state1", "state"), "IN", "411"), "~1234-1234-1234-1234#", phoneNumerList);
            return card;
        }

        protected static TokenizedCard GetInValidCardDetails()
        {
            List<Phone> phoneNumerList = new List<Phone>()
            {
                new Phone(PhoneType.Home, "13213123131","91","341", "123")
            };
            TokenizedCard card = new TokenizedCard("4231*******1234", "Test name", CardIssuer.R, new Expiry(1, 2020), "123", new Address("pune", "pune", 
                new Model.Common.City("pune1", "pune"), new State("state1", "state"), "IN", "411"), "~1234-1234-1234-1234#", phoneNumerList);
            return card;
        }

        protected static BookingContact GetBookingContact()
        {

            ContactInfo contactInfo = new ContactInfo(new Phone(PhoneType.Mobile, "1234567890", "91", "4564", "4324"), new Address("pune", "sds", 
                new Model.Common.City("Pune1", "Pune"), new State("MH", "MH"), "IN", "411"), "testmail@testemail.com");
            return new BookingContact(new Name("Testfirstname", "TestFirstName1", string.Empty, Title.Mr, string.Empty), 35, contactInfo, null);
        }

        protected static string GetHotelID()
        {
            return "d4b5bc60-1c9c-4e10-b8bd-cc6f6a1a08a8";
        }

        protected static List<string> GetSearchRequestHotelIDs()
        {
            return new List<string>()
            {
                "d4b5bc60-1c9c-4e10-b8bd-cc6f6a1a08a8",
                "480a6e89-4ba2-5677-b1b6-121fbdc5cd2b"
            };
        }


        protected static Supplier CreateSupplierObject()
       
        {
            Configuration keyConfig = new Configuration("api_key", "1234");
            Configuration authTokenConfig = new Configuration("auth_token", "123!@#");
            Configuration token = new Configuration("token", "1234");
            Configuration istestbooking = new Configuration("istestbooking", "123!@#");

            return new Supplier("123", "HotelsCom", true, new List<Configuration> { keyConfig, authTokenConfig, token, istestbooking });
        }

        protected static string GetHotelIDFromSearchResponse(SearchResponse searchResponse)
        {
            string hotelId = string.Empty;
            if (searchResponse?.Itineraries != null)
            {
                if (searchResponse.Itineraries.Count > 0 && searchResponse.Itineraries[0].HotelInfo != null)
                {
                    hotelId = searchResponse.Itineraries[0].HotelInfo.Id;
                }
            }

            return hotelId;
        }

        protected static string GetHotelIDFromRoomRatesResponse(Response roomRatesResponse)
        {
            string hotelId = string.Empty;
            if (roomRatesResponse?.HotelInfo != null)
            {
                if (!string.IsNullOrWhiteSpace(roomRatesResponse.HotelInfo.Id))
                {
                    hotelId = roomRatesResponse.HotelInfo.Id;
                }
            }

            return hotelId;
        }

        protected static string GetSessionIdFromSearchResponse(SearchResponse searchResponse)
        {
            string sessionId = string.Empty;
            if (searchResponse?.SessionId != null)
            {
                if (!string.IsNullOrWhiteSpace(searchResponse.SessionId))
                {
                    sessionId = searchResponse.SessionId;
                }
            }
            return sessionId;
        }

        protected static string GetSessionIdFromRoomRatesRespons(Response roomRatesResponse)
        {
            string sessionId = string.Empty;
            if (roomRatesResponse?.SessionId != null)
            {
                if (!string.IsNullOrWhiteSpace(roomRatesResponse.SessionId))
                {
                    sessionId = roomRatesResponse.SessionId;
                }
            }
            return sessionId;
        }

        protected static string GetSessionIdFromRateRulesResponse(RateRulesResponse rateRulesResponse)
        {
            string sessionId = string.Empty;
            if (rateRulesResponse?.SessionId != null)
            {
                if (!string.IsNullOrWhiteSpace(rateRulesResponse.SessionId))
                {
                    sessionId = rateRulesResponse.SessionId;
                }
            }
            return sessionId;
        }

    }
}
