using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tavisca.Connector.Hotels.Model.Common;
using Tavisca.Connector.Hotels.Model.RateRules;
using Tavisca.Connector.Hotels.Model.RoomRates;
using Tavisca.Connector.Hotels.Model.Search;

using ConnectorSearch = Tavisca.Connector.Hotels.Model.Search;
namespace Tavisca.Connector.Hotels.Tourico.TestSuite.IntegrationTests.RateRules
{
    public class MockRateRulesRequest : BaseMockRequest
    {
        public static RateRulesRequest RateRules_PerBooking_Request(Response roomRatesResponse)
        {
            RequestOccupancy requestOccupancy = new RequestOccupancy(1, new List<int>() { 4, 5 });

            List<RoomOccupancyDetail> listRoomOccupancyDetail = new List<RoomOccupancyDetail>()
            {
                new RoomOccupancyDetail("roomcode1", requestOccupancy)
            };

            Model.RateRules.Criteria criteria = new Model.RateRules.Criteria(GetHotelIDFromRoomRatesResponse(roomRatesResponse), _checkinDate, _checkoutDate, new RequestRoomRate(new PerBookingRateRequest("id", "code", listRoomOccupancyDetail)));

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
