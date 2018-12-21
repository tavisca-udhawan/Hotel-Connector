using Newtonsoft.Json;
using System.Collections.Generic;
using Tavisca.Connector.Hotels.ErrorHandling;
using Tavisca.Connector.Hotels.Model.Book;
using Tavisca.Connector.Hotels.Model.RateRules;
using Tavisca.Connector.Hotels.Model.RoomRates;
using Tavisca.Connector.Hotels.Model.Search;
using Xunit;

namespace Tavisca.Connector.Hotels.Tourico.TestSuite.IntegrationTests.Book
{
    public class BookTestCases : BaseTestCase
    {
        public BookTestCases()
        {
            
        }

        [Fact]
        public async void Test_InValidCreditCardInRequest_ReturnsError()
        {
            //Note: The test case fails in shell, as response from supplier is mocked and supplier is never hit.
            //This should pass when actual connector is implemented.
            var bookResponseString = await GetBookResponse(MockBookRequest.InValidCreditCardInRequest());

            ErrorInfo errorInfo = JsonConvert.DeserializeObject<ErrorInfo>(bookResponseString, _globalSerializerSettings);

            //Assert
            CheckIsTestPassed(errorInfo, FaultCodes.ValidationFailure, new List<Info>() { new Info(FaultCodes.InvalidCreditCardDetails)});
        }

        [Fact]
        public async void Test_FirstNameLongerThanMaxLengthInRequest_ReturnsError()
        {
            var bookResponseString = await GetBookResponse(MockBookRequest.FirstNameLongerThanMaxLengthInRequest());

            if (_hotelMetadata?.Verbs?.Book?.Restriction?.FirstNameMaxLength != null)
            {
                ErrorInfo errorInfo = JsonConvert.DeserializeObject<ErrorInfo>(bookResponseString, _globalSerializerSettings);

                //Assert
                CheckIsTestPassed(errorInfo, FaultCodes.ValidationFailure, new List<Info>() { new Info(FaultCodes.FirstNameLengthExceeded) });
            }
        }

        [Fact]
        public async void Test_LastNameLongerThanMaxLengthInRequest_ReturnsError()
        {
            var bookResponseString = await GetBookResponse(MockBookRequest.LastNameLongerThanMaxLengthInRequest());
            
            if (_hotelMetadata?.Verbs?.Book?.Restriction?.LastNameMaxLength != null)
            {
                ErrorInfo errorInfo = JsonConvert.DeserializeObject<ErrorInfo>(bookResponseString, _globalSerializerSettings);

                //Assert
                CheckIsTestPassed(errorInfo, FaultCodes.ValidationFailure, new List<Info>() { new Info(FaultCodes.LastNameLengthExceeded) });
            }
        }

        [Fact]
        public async void Test_FirstAndLastNameLongerThanMaxLengthInRequest_ReturnsError()
        {
            var bookResponseString = await GetBookResponse(MockBookRequest.FirstAndLastNameLongerThanMaxLengthInRequest());
                        
            if (_hotelMetadata?.Verbs?.Book?.Restriction?.FirstNameMaxLength != null && _hotelMetadata?.Verbs?.Book?.Restriction?.LastNameMaxLength != null)
            {
                ErrorInfo errorInfo = JsonConvert.DeserializeObject<ErrorInfo>(bookResponseString, _globalSerializerSettings);

                //Assert
                CheckIsTestPassed(errorInfo, FaultCodes.ValidationFailure, new List<Info>() { new Info(FaultCodes.FirstNameLengthExceeded), new Info(FaultCodes.LastNameLengthExceeded) });
            }
        }

        [Fact]
        public async void Test_FirstAndLastNameEqualToMaxLengthInRequest_Succeeds()
        {
            // call Search API
            var searchResponseString = await GetSearchResponse(MockBookRequest.BasicSearch_Request());
            SearchResponse searchResponse = JsonConvert.DeserializeObject<SearchResponse>(searchResponseString);

            // call RoomRates API
            var roomRatesResponseString = await GetRoomRatesResponse(MockBookRequest.BasicRoomRates_Request(searchResponse));
            Response roomRatesResponse = JsonConvert.DeserializeObject<Response>(roomRatesResponseString);

            // call RateRules API
            var responseString = await GetRateRulesResponse(MockBookRequest.BasicRateRules_PerBooking_Request(roomRatesResponse));
            RateRulesResponse rateRulesResponse = JsonConvert.DeserializeObject<RateRulesResponse>(responseString, _globalSerializerSettings);

            var bookResponseString = await GetBookResponse(MockBookRequest.FirstAndLastNameEqualToMaxLengthInRequest(rateRulesResponse));
            BookResponse bookingResponse = JsonConvert.DeserializeObject<BookResponse>(bookResponseString, _globalSerializerSettings);

            // Act
            bool isCheckFailed = false;

            if (bookingResponse?.Status != null)
                isCheckFailed = true;

            //Assert
            Assert.True(isCheckFailed);
        }

        [Fact]
        public async void Test_FirstAndLastNameLessThanMaxLengthInRequest_Succeeds()
        {
            var searchResponseString = await GetSearchResponse(MockBookRequest.BasicSearch_Request());
            SearchResponse searchResponse = JsonConvert.DeserializeObject<SearchResponse>(searchResponseString);

            // call RoomRates API
            var roomRatesResponseString = await GetRoomRatesResponse(MockBookRequest.BasicRoomRates_Request(searchResponse));
            Response roomRatesResponse = JsonConvert.DeserializeObject<Response>(roomRatesResponseString);

            // call RateRules API
            var responseString = await GetRateRulesResponse(MockBookRequest.BasicRateRules_PerBooking_Request(roomRatesResponse));
            RateRulesResponse rateRulesResponse = JsonConvert.DeserializeObject<RateRulesResponse>(responseString, _globalSerializerSettings);

            var bookResponseString = await GetBookResponse(MockBookRequest.FirstAndLastNameLessThanMaxLengthInRequest(rateRulesResponse));
            BookResponse bookingResponse = JsonConvert.DeserializeObject<BookResponse>(bookResponseString, _globalSerializerSettings);

            // Act
            bool isCheckFailed = false;

            if (bookingResponse?.Status != null)
                isCheckFailed = true;

            //Assert
            Assert.True(isCheckFailed);
        }     
    }
}
