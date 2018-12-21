using Newtonsoft.Json;
using Tavisca.Connector.Hotels.Model.RateRules;
using Tavisca.Connector.Hotels.Model.RoomRates;
using Tavisca.Connector.Hotels.Model.Search;
using Xunit;

namespace Tavisca.Connector.Hotels.Tourico.TestSuite.IntegrationTests.RateRules
{
    public class RateRulesTestCases  : BaseTestCase
    {

        [Fact]
        public async void Test_GauranteeRequiredInResponse_Succeeds()
        {
            bool guaranteeRequired = _hotelMetadata?.Verbs?.RateRules?.GuaranteeRequired ?? false;
            if (guaranteeRequired)
            {
                // call Search API
                var searchResponseString = await GetSearchResponse(MockRateRulesRequest.BasicSearch_Request());
                SearchResponse searchResponse = JsonConvert.DeserializeObject<SearchResponse>(searchResponseString);

                // call RoomRates API
                var roomRatesResponseString = await GetRoomRatesResponse(MockRateRulesRequest.BasicRoomRates_Request(searchResponse));
                Response roomRatesResponse = JsonConvert.DeserializeObject<Response>(roomRatesResponseString);

                // call RateRules API
                var responseString = await GetRateRulesResponse(MockRateRulesRequest.RateRules_PerBooking_Request(roomRatesResponse));
                RateRulesResponse rateRulesResponse = JsonConvert.DeserializeObject<RateRulesResponse>(responseString, _globalSerializerSettings);

                //Assert
                Assert.NotNull(rateRulesResponse?.RoomRate?.PerBookingRate?.BookingRequirement?.GuaranteeRequired);
            }

        }

        [Fact]
        public async void Test_DepositRequiredInResponse_Succeeds()
        {
            bool depositRequired = _hotelMetadata?.Verbs?.RateRules?.DepositRequired ?? false;
            if (depositRequired)
            {
                // call Search API
                var searchResponseString = await GetSearchResponse(MockRateRulesRequest.BasicSearch_Request());
                SearchResponse searchResponse = JsonConvert.DeserializeObject<SearchResponse>(searchResponseString);

                // call RoomRates API
                var roomRatesResponseString = await GetRoomRatesResponse(MockRateRulesRequest.BasicRoomRates_Request(searchResponse));
                Response roomRatesResponse = JsonConvert.DeserializeObject<Response>(roomRatesResponseString);

                // call RateRules API
                var responseString = await GetRateRulesResponse(MockRateRulesRequest.RateRules_PerBooking_Request(roomRatesResponse));
                RateRulesResponse rateRulesResponse = JsonConvert.DeserializeObject<RateRulesResponse>(responseString, _globalSerializerSettings);

                //Assert
                Assert.True(rateRulesResponse?.RoomRate?.PerBookingRate?.BookingRequirement?.DepositRequired);
            }
        }

        [Fact]
        public async void Test_RefundableRequiredInResponse_Succeeds()
        {
            bool refundablity = _hotelMetadata?.Verbs?.RateRules?.Refundablity ?? false;
            if (refundablity)
            {
                var searchResponseString = await GetSearchResponse(MockRateRulesRequest.BasicSearch_Request());
                SearchResponse searchResponse = JsonConvert.DeserializeObject<SearchResponse>(searchResponseString);

                // call RoomRates API
                var roomRatesResponseString = await GetRoomRatesResponse(MockRateRulesRequest.BasicRoomRates_Request(searchResponse));
                Response roomRatesResponse = JsonConvert.DeserializeObject<Response>(roomRatesResponseString);

                // call RateRules API
                var responseString = await GetRateRulesResponse(MockRateRulesRequest.RateRules_PerBooking_Request(roomRatesResponse));
                RateRulesResponse rateRulesResponse = JsonConvert.DeserializeObject<RateRulesResponse>(responseString, _globalSerializerSettings);

                //Assert
                Assert.NotNull(rateRulesResponse?.RoomRate?.PerBookingRate?.Refundable);
            }
        }

        [Fact]
        public async void Test_AllowedCreditCardsRequiredInResponse_Succeeds()
        {

            var searchResponseString = await GetSearchResponse(MockRateRulesRequest.BasicSearch_Request());
            SearchResponse searchResponse = JsonConvert.DeserializeObject<SearchResponse>(searchResponseString);

            // call RoomRates API
            var roomRatesResponseString = await GetRoomRatesResponse(MockRateRulesRequest.BasicRoomRates_Request(searchResponse));
            Response roomRatesResponse = JsonConvert.DeserializeObject<Response>(roomRatesResponseString);

            // call RateRules API
            var responseString = await GetRateRulesResponse(MockRateRulesRequest.RateRules_PerBooking_Request(roomRatesResponse));
            RateRulesResponse rateRulesResponse = JsonConvert.DeserializeObject<RateRulesResponse>(responseString, _globalSerializerSettings);

            bool rateSpecific = (_hotelMetadata?.Verbs?.RateRules?.CreditCardInfo?.RateSpecific != null) ? _hotelMetadata.Verbs.RateRules.CreditCardInfo.RateSpecific : false;

            if (rateSpecific)
            {
                Assert.True(rateRulesResponse?.RoomRate?.PerBookingRate?.BookingRequirement?.AllowedCreditCards?.Count > 0);
            }
        }      
    }
}
