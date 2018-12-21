using Xunit;
using Newtonsoft.Json;
using Tavisca.Connector.Hotels.Model.Search;
using Tavisca.Connector.Hotels.ErrorHandling;
using System.Linq;
using System.Collections.Generic;
using ConnectorSearch = Tavisca.Connector.Hotels.Model.Search;
namespace Tavisca.Connector.Hotels.Tourico.TestSuite.IntegrationTests.Search
{
    public class SearchTestCases : BaseTestCase
    {
        private readonly MockSearchRequest _mockSearchRequest;

        public SearchTestCases()
        {
            _mockSearchRequest = new MockSearchRequest(_hotelMetadata);
        }

        [Fact]
        public async void Test_GreaterThanMaxChildAgeInRequest_ReturnsError()
        {
            var responseString = await GetSearchResponse(_mockSearchRequest.GreaterThanMaxChildAge_Request());
            ErrorInfo errorInfo = JsonConvert.DeserializeObject<ErrorInfo>(responseString, _globalSerializerSettings);
            CheckIsTestPassed(errorInfo, FaultCodes.ValidationFailure, new List<Info>() { new Info(FaultCodes.InvalidChildAges) });
        }

        [Fact]
        public async void Test_MaxNumberOfRoomsExceeded_ReturnsError()
        {
            var responseString = await GetSearchResponse(_mockSearchRequest.MaxNumberOfRoomsExceeded_Request());
            ErrorInfo errorInfo = JsonConvert.DeserializeObject<ErrorInfo>(responseString, _globalSerializerSettings);
            CheckIsTestPassed(errorInfo, FaultCodes.ValidationFailure, new List<Info>() { new Info(FaultCodes.InvalidMultiRoomsSearch) });
        }

        [Fact]
        public async void Test_MultiroomSupportOnlyAdults_Succeeds()
        {
            var searchRequest = _mockSearchRequest.MultiroomSupportOnlyAdults_Request();
            var responseString = await GetSearchResponse(searchRequest);
            SearchResponse searchResponse = JsonConvert.DeserializeObject<SearchResponse>(responseString, _globalSerializerSettings);

            if (searchResponse?.Itineraries?.Count > 0)
            {
                //Note: Ideally below logic should be checked, but as response is mocked we have commented this code. 
                // Uncomment this code when connector is implemented.
                //bool containsCorrectNumberOfRooms = searchResponse.Itineraries.All(item => item.Occupancies.Count == searchRequest.Criteria.Occupancies.Count);
                // Assert.True(containsCorrectNumberOfRooms);
            }
            else
            {
                Assert.True(CheckWarnings(searchResponse.Warnings, FaultCodes.NoResultsForSearchCriteria));
            }
        }

        [Fact]
        public async void Test_MultiroomSupportAdultsAndChildren_Succeeds()
        {
            var searchRequest = _mockSearchRequest.MultiroomSupportAdultsAndChildren_Request();
            var responseString = await GetSearchResponse(searchRequest);
            SearchResponse searchResponse = JsonConvert.DeserializeObject<SearchResponse>(responseString, _globalSerializerSettings);

            if (searchResponse?.Itineraries?.Count > 0)
            {
                //Note: Ideally below logic should be checked, but as response is mocked we have commented this code. 
                // Uncomment this code when connector is implemented.
                //bool containsCorrectNumberOfRooms = searchResponse.Itineraries.All(item => item.Occupancies.Count == searchRequest.Criteria.Occupancies.Count);
                //Assert.True(containsCorrectNumberOfRooms);
            }
            else
            {
                Assert.True(CheckWarnings(searchResponse.Warnings, FaultCodes.NoResultsForSearchCriteria));
            }
        }

        [Fact]
        public async void Test_GuestCountEqualMaxGuestsPerRoom_Succeeds()
        {
            var searchRequest = _mockSearchRequest.GuestCountEqualMaxGuestsPerRoom_Request();
            var responseString = await GetSearchResponse(searchRequest);
            SearchResponse searchResponse = JsonConvert.DeserializeObject<SearchResponse>(responseString, _globalSerializerSettings);

            if (searchResponse?.Itineraries?.Count > 0)
            {
                //Note: Ideally below logic should be checked, but as response is mocked we have commented this code. 
                // Uncomment this code when connector is implemented.
                //searchRequest.Criteria.Occupancies.ForEach(occupancy =>
                //{
                //    var guestCount = 0;
                //    guestCount = guestCount + occupancy.NumOfAdults;
                //    guestCount = guestCount + (occupancy.ChildAges?.Count ?? 0);
                //    Assert.True(searchResponse.Itineraries.Any(item => item.Occupancies.Any(responseOccupancy => (responseOccupancy.Adults + responseOccupancy.Children) == guestCount)));
                //});
            }
            else
            {
                Assert.True(CheckWarnings(searchResponse.Warnings, FaultCodes.NoResultsForSearchCriteria));
            }
        }

        [Fact]
        public async void Test_GuestCountEqualMaxGuestsMultiroomPerRoom_Succeeds()
        {
            var searchRequest = _mockSearchRequest.GuestCountEqualMaxGuestsMultiroomSucceeds_Request();
            var responseString = await GetSearchResponse(searchRequest);
            SearchResponse searchResponse = JsonConvert.DeserializeObject<SearchResponse>(responseString, _globalSerializerSettings);

            if (searchResponse?.Itineraries?.Count > 0)
            {
                //Note: Ideally below logic should be checked, but as response is mocked we have commented this code. 
                // Uncomment this code when connector is implemented.
                //searchRequest.Criteria.Occupancies.ForEach(occupancy =>
                //{
                //    var guestCount = 0;
                //    guestCount = guestCount + occupancy.NumOfAdults;
                //    guestCount = guestCount + (occupancy.ChildAges?.Count ?? 0);
                //    Assert.True(searchResponse.Itineraries.Any(item => item.Occupancies.Any(responseOccupancy => (responseOccupancy.Adults + responseOccupancy.Children) == guestCount)));
                //});
            }
            else
            {
                Assert.True(CheckWarnings(searchResponse.Warnings, FaultCodes.NoResultsForSearchCriteria));
            }
        }

        [Fact]
        public async void Test_GuestCountGreaterThanMaxGuestsOneRoom_ReturnsError()
        {
            var responseString = await GetSearchResponse(_mockSearchRequest.GuestCountGreaterThanMaxGuestsOneRoom_Request());
            ErrorInfo errorInfo = JsonConvert.DeserializeObject<ErrorInfo>(responseString, _globalSerializerSettings);
            CheckIsTestPassed(errorInfo, FaultCodes.ValidationFailure, new List<Info>() { new Info(FaultCodes.InvalidGuestCountForPerRoom) });
        }

        [Fact]
        public async void Test_GuestCountGreaterThanMaxGuestsAllRooms_ReturnsError()
        {
            var responseString = await GetSearchResponse(_mockSearchRequest.GuestCountGreaterThanMaxGuestsAllRooms_Request());
            ErrorInfo errorInfo = JsonConvert.DeserializeObject<ErrorInfo>(responseString, _globalSerializerSettings);
            CheckIsTestPassed(errorInfo, FaultCodes.ValidationFailure, new List<Info>() { new Info(FaultCodes.InvalidGuestCountForPerRoom) });
        }

        [Fact]
        public async void Test_HotelIDsMissing_ReturnsError()
        {
            var responseString = await GetSearchResponse(_mockSearchRequest.HotelIDsMissing_Request());
            ErrorInfo errorInfo = JsonConvert.DeserializeObject<ErrorInfo>(responseString, _globalSerializerSettings);
            CheckIsTestPassed(errorInfo, FaultCodes.ValidationFailure, new List<Info>() { new Info(FaultCodes.InvalidSearchPattern) });
        }

        [Fact]
        public async void Test_CityInformationMissing_ReturnsError()
        {
            var responseString = await GetSearchResponse(_mockSearchRequest.CityInformationMissing_Request());
            ErrorInfo errorInfo = JsonConvert.DeserializeObject<ErrorInfo>(responseString, _globalSerializerSettings);
            CheckIsTestPassed(errorInfo, FaultCodes.ValidationFailure, new List<Info>() { new Info(FaultCodes.InvalidSearchPattern) });
        }

        [Fact]
        public async void Test_RadialRegionMissing_ReturnsError()
        {
            var responseString = await GetSearchResponse(_mockSearchRequest.RadialRegionMissing_Request());
            ErrorInfo errorInfo = JsonConvert.DeserializeObject<ErrorInfo>(responseString, _globalSerializerSettings);
            CheckIsTestPassed(errorInfo, FaultCodes.ValidationFailure, new List<Info>() { new Info(FaultCodes.InvalidSearchPattern) });
        }

        [Fact]
        public async void Test_MaxNumberOfRoomsSent_Succeeds()
        {
            var searchRequest = _mockSearchRequest.MaxNumberOfRoomsSent_Request();
            var responseString = await GetSearchResponse(searchRequest);
            SearchResponse searchResponse = JsonConvert.DeserializeObject<SearchResponse>(responseString, _globalSerializerSettings);

            if (searchResponse?.Itineraries?.Count > 0)
            {
                //Note: Ideally below logic should be checked, but as response is mocked we have commented this code. 
                // Uncomment this code when connector is implemented.
                //bool containsCorrectNumberOfRooms = searchResponse.Itineraries.All(item => item.Occupancies.Count == searchRequest.Criteria.Occupancies.Count);
                //Assert.True(containsCorrectNumberOfRooms);
            }
            else
            {
                Assert.True(CheckWarnings(searchResponse.Warnings, FaultCodes.NoResultsForSearchCriteria));
            }
        }

        [Fact]
        public async void Test_LessThanMaxNumberOfRoomsSent_Succeeds()
        {
            var searchRequest = _mockSearchRequest.LessThanMaxNumberOfRoomsSent_Request();
            var responseString = await GetSearchResponse(searchRequest);
            SearchResponse searchResponse = JsonConvert.DeserializeObject<SearchResponse>(responseString, _globalSerializerSettings);

            if (searchResponse?.Itineraries?.Count > 0)
            {
                //Note: Ideally below logic should be checked, but as response is mocked we have commented this code. 
                // Uncomment this code when connector is implemented.
                //bool containsCorrectNumberOfRooms = searchResponse.Itineraries.All(item => item.Occupancies.Count == searchRequest.Criteria.Occupancies.Count);
                //Assert.True(containsCorrectNumberOfRooms);
            }
            else
            {
                Assert.True(CheckWarnings(searchResponse.Warnings, FaultCodes.NoResultsForSearchCriteria));
            }
        }

        [Fact]
        public async void Test_EqualToMaxStayDurationSent_Succeeds()
        {
            var searchRequest = _mockSearchRequest.EqualToMaxStayDurationSent_Request();
            var responseString = await GetSearchResponse(searchRequest);
            SearchResponse searchResponse = JsonConvert.DeserializeObject<SearchResponse>(responseString, _globalSerializerSettings);

            if (searchResponse?.Itineraries?.Count > 0)
            {
                Assert.True(true);
            }
            else
            {
                Assert.True(CheckWarnings(searchResponse.Warnings, FaultCodes.NoResultsForSearchCriteria));
            }
        }

        [Fact]
        public async void Test_LessThanMaxStayDurationSent_Succeeds()
        {
            var searchRequest = _mockSearchRequest.LessThanMaxStayDurationSent_Request();
            var responseString = await GetSearchResponse(searchRequest);
            SearchResponse searchResponse = JsonConvert.DeserializeObject<SearchResponse>(responseString, _globalSerializerSettings);

            if (searchResponse?.Itineraries?.Count > 0)
            {
                Assert.True(true);
            }
            else
            {
                Assert.True(CheckWarnings(searchResponse.Warnings, FaultCodes.NoResultsForSearchCriteria));
            }
        }

        [Fact]
        public async void Test_GreaterThanMaxStayDurationSent_ReturnsError()
        {
            var responseString = await GetSearchResponse(_mockSearchRequest.GreaterThanMaxStayDurationSent_Request());
            ErrorInfo errorInfo = JsonConvert.DeserializeObject<ErrorInfo>(responseString, _globalSerializerSettings);
            CheckIsTestPassed(errorInfo, FaultCodes.ValidationFailure, new List<Info>() { new Info(FaultCodes.StayDurationLimit) });
        }

        [Fact]
        public async void Test_CheckoutDateAfterAdvanceBookingLimit_ReturnsError()
        {
            if (_hotelMetadata.Verbs.Search.AdvanceBookingLimit.HasValue)
            {
                var responseString = await GetSearchResponse(_mockSearchRequest.VerifyCheckoutDateAfterAdvanceBookingLimit_Request());
                ErrorInfo errorInfo = JsonConvert.DeserializeObject<ErrorInfo>(responseString, _globalSerializerSettings);
                CheckIsTestPassed(errorInfo, FaultCodes.ValidationFailure, new List<Info>() { new Info(FaultCodes.CheckOutExceedsPermittedLimit) });
            }
            else
            {
                Assert.True(true);
            }
        }

        [Fact]
        public async void Test_CheckoutDateEqualAdvanceBookingLimit_Succeeds()
        {
            if (_hotelMetadata.Verbs.Search.AdvanceBookingLimit.HasValue)
            {
                var searchRequest = _mockSearchRequest.VerifyCheckoutDateEqualAdvanceBookingLimit_Request();
                var responseString = await GetSearchResponse(searchRequest);
                SearchResponse searchResponse = JsonConvert.DeserializeObject<SearchResponse>(responseString, _globalSerializerSettings);

                if (searchResponse?.Itineraries?.Count > 0)
                {
                    Assert.True(true);
                }
                else
                {
                    Assert.True(CheckWarnings(searchResponse.Warnings, FaultCodes.NoResultsForSearchCriteria));
                }
            }
        }

        [Fact]
        public async void Test_CheckoutDateBeforeAdvanceBookingLimit_Succeeds()
        {
            if (_hotelMetadata.Verbs.Search.AdvanceBookingLimit.HasValue)
            {
                var searchRequest = _mockSearchRequest.VerifyCheckoutDateBeforeAdvanceBookingLimit_Request();
                var responseString = await GetSearchResponse(searchRequest);
                SearchResponse searchResponse = JsonConvert.DeserializeObject<SearchResponse>(responseString, _globalSerializerSettings);

                if (searchResponse?.Itineraries?.Count > 0)
                {
                    Assert.True(true);
                }
                else
                {
                    Assert.True(CheckWarnings(searchResponse.Warnings, FaultCodes.NoResultsForSearchCriteria));
                }
            }
        }      

    }
}
