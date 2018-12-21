using Newtonsoft.Json;
using System.Linq;
using System.Collections.Generic;
using Tavisca.Connector.Hotels.ErrorHandling;
using Xunit;
using Tavisca.Connector.Hotels.Model.Retrieve;

namespace Tavisca.Connector.Hotels.Tourico.TestSuite.IntegrationTests.Retrieve
{
    public class RetrieveTestCases : BaseTestCase
    {
        [Fact]
        public async void Test_CriteriaIsMissing_ReturnsError()
        {
            var responseString = await GetRetrieveResponse(MockRetrieveRequest.CriteriaMissing_Request());
            ErrorInfo errorInfo = JsonConvert.DeserializeObject<ErrorInfo>(responseString, _globalSerializerSettings);
            CheckIsTestPassed(errorInfo, FaultCodes.ValidationFailure, new List<Info>() { new Info(FaultCodes.InvalidCriteria) });
        }

        [Fact]
        public async void Test_FromDateGreaterThanToDate_ReturnsError()
        {
            var responseString = await GetRetrieveResponse(MockRetrieveRequest.FromDateGreaterThanToDate_Request());
            ErrorInfo errorInfo = JsonConvert.DeserializeObject<ErrorInfo>(responseString, _globalSerializerSettings);
            CheckIsTestPassed(errorInfo, FaultCodes.ValidationFailure, new List<Info>() { new Info(FaultCodes.InvalidRetrieveDateInterval) });
        }

        [Fact]
        public async void Test_BookingStatusIsNoneInResponse_Succeeds()
        {
            var retrieveResponseString = await GetRetrieveResponse(MockRetrieveRequest.BasicRetrieve_Request());
            RetrieveResponse retrieveResponse = JsonConvert.DeserializeObject<RetrieveResponse>(retrieveResponseString, _globalSerializerSettings);
            if (retrieveResponse?.RetrieveBookingResponse != null)
            {
                List<RetrieveBookingResponse> bookingStatusIsNone = retrieveResponse.RetrieveBookingResponse.Where(booking => booking.BookingDetails.Status == Model.Common.BookingStatus.None).ToList();
                Assert.True(bookingStatusIsNone.Count == 0);
            }            
        }

        [Fact]
        public async void Test_BookingCreationDateIsInRangeInResponse_Succeeds()
        {
            RetrieveRequest request = MockRetrieveRequest.RetrieveByDate_Request();
            var retrieveResponseString = await GetRetrieveResponse(request);

            RetrieveResponse retrieveResponse = JsonConvert.DeserializeObject<RetrieveResponse>(retrieveResponseString, _globalSerializerSettings);

            if (request?.BookingDuration != null && retrieveResponse?.RetrieveBookingResponse != null)
            {
                List<RetrieveBookingResponse> bookingCreationDateOutOfRange =
                 retrieveResponse.RetrieveBookingResponse.Where(booking => booking.BookingDetails.BookingCreationDate < request.BookingDuration.FromDate || booking.BookingDetails.BookingCreationDate > request.BookingDuration.ToDate).ToList();
                Assert.True(bookingCreationDateOutOfRange.Count == 0);
            }
        }

        [Fact]
        public async void Test_BookingIdInResponse_Succeeds()
        {
            RetrieveRequest request = MockRetrieveRequest.BasicRetrieve_Request();
            var retrieveResponseString = await GetRetrieveResponse(request);

            RetrieveResponse retrieveResponse = JsonConvert.DeserializeObject<RetrieveResponse>(retrieveResponseString, _globalSerializerSettings);

            if (!string.IsNullOrEmpty(request.SupplierConfirmationNum) && retrieveResponse?.RetrieveBookingResponse != null)
            {
                List<RetrieveBookingResponse> validBookingId =
                 retrieveResponse.RetrieveBookingResponse.Where(booking => booking.BookingDetails.SupplierConfirmationNum == request.SupplierConfirmationNum).ToList();
                Assert.True(validBookingId.Count > 0);
            }
        }

        [Fact]
        public async void Test_CurrencyNotReturnedInResponse_ReturnError()
        {
            RetrieveRequest request = MockRetrieveRequest.BasicRetrieve_Request();
            var retrieveResponseString = await GetRetrieveResponse(request);

            RetrieveResponse retrieveResponse = JsonConvert.DeserializeObject<RetrieveResponse>(retrieveResponseString, _globalSerializerSettings);

            if (retrieveResponse?.RetrieveBookingResponse != null)
            {
                List<RetrieveBookingResponse> currencyNotFoundBookings =
                 retrieveResponse.RetrieveBookingResponse.Where(booking => (string.IsNullOrWhiteSpace(booking.Rate?.Currency))).ToList();
                Assert.False(currencyNotFoundBookings.Count > 0);
            }
        }

        [Fact]
        public async void Test_TotalNotReturnedInResponse_ReturnError()
        {
            RetrieveRequest request = MockRetrieveRequest.BasicRetrieve_Request();
            var retrieveResponseString = await GetRetrieveResponse(request);

            RetrieveResponse retrieveResponse = JsonConvert.DeserializeObject<RetrieveResponse>(retrieveResponseString, _globalSerializerSettings);

            if (retrieveResponse?.RetrieveBookingResponse != null)
            {
                List<RetrieveBookingResponse> totalNotFoundBookings =
                 retrieveResponse.RetrieveBookingResponse.Where(booking => (booking.Rate?.Total <= 0)).ToList();
                Assert.False(totalNotFoundBookings.Count > 0);
            }
        }
    }
}
