using System;
using System.Collections.Generic;
using Tavisca.Connector.Hotels.Model.Metadata;
using Tavisca.Connector.Hotels.Model.Search;
using Tavisca.Connector.Hotels.Model.Common;
using ConnectorSearch = Tavisca.Connector.Hotels.Model.Search;
namespace Tavisca.Connector.Hotels.Tourico.TestSuite.IntegrationTests.Search
{
    public class MockSearchRequest : BaseMockRequest
    {
        private readonly HotelMetadata _metadata;
        private readonly DateTime _checkInDate;
        private readonly DateTime _checkOutDate;
        private readonly DateTime _bookingDate;
        public MockSearchRequest(HotelMetadata metadata)
        {
            _metadata = metadata;
            _checkInDate = DateTime.Now.AddDays(90);
            _checkOutDate = DateTime.Now.AddDays(100);
            _bookingDate = DateTime.Now;
        }

        public ConnectorSearch.SearchRequest GreaterThanMaxChildAge_Request()
        {
            List<RequestOccupancy> requestOccupancies = new List<RequestOccupancy>()
            {
                new RequestOccupancy(2, new List<int>() {2, (_metadata.Verbs.Search.MaxAllowedChildAge ?? 17) + 1 }),
                new RequestOccupancy(1, new List<int>() {5, (_metadata.Verbs.Search.MaxAllowedChildAge ?? 17) })
            };

            Criteria criteria = new Criteria(_checkInDate, _checkOutDate, GetSearchRequestHotelIDs(), requestOccupancies);
            return new ConnectorSearch.SearchRequest(GetSessionId(), criteria, CreateSupplierObject());
        }

        public ConnectorSearch.SearchRequest MaxNumberOfRoomsExceeded_Request()
        {

            List<RequestOccupancy> requestOccupancies = new List<RequestOccupancy>();
            for (int i = 0; i < (_metadata.MultiRoom.MaxRoomsPerBooking + 1); i++)
            {
                requestOccupancies.Add(new RequestOccupancy(13, new List<int>() { 2, 12 }));
            }

            Criteria criteria = new Criteria(_checkInDate, _checkOutDate, GetSearchRequestHotelIDs(), requestOccupancies);
            return new ConnectorSearch.SearchRequest(GetSessionId(), criteria, CreateSupplierObject());
        }

        public ConnectorSearch.SearchRequest MultiroomSupportOnlyAdults_Request()
        {
            var childAges = new List<int>();
            List<RequestOccupancy> requestOccupancies = new List<RequestOccupancy>()
            {
                new RequestOccupancy(2, childAges),
                new RequestOccupancy(8, childAges),
                new RequestOccupancy(2, childAges)
            };

            Criteria criteria = new Criteria(_checkInDate, _checkOutDate, GetSearchRequestHotelIDs(), requestOccupancies);
            return new ConnectorSearch.SearchRequest(GetSessionId(), criteria, CreateSupplierObject());
        }

        public ConnectorSearch.SearchRequest MultiroomSupportAdultsAndChildren_Request()
        {
            List<RequestOccupancy> requestOccupancies = new List<RequestOccupancy>()
            {
                new RequestOccupancy(1, new List<int> {5, 11 }),
                new RequestOccupancy(1, new List<int> {2, 7 })
            };

            Criteria criteria = new Criteria(_checkInDate, _checkOutDate, GetSearchRequestHotelIDs(), requestOccupancies);
            return new ConnectorSearch.SearchRequest(GetSessionId(), criteria, CreateSupplierObject());
        }

        public ConnectorSearch.SearchRequest GuestCountEqualMaxGuestsPerRoom_Request()
        {
            List<RequestOccupancy> requestOccupancies = new List<RequestOccupancy>()
            {
                new RequestOccupancy(_metadata.MultiRoom.MaxGuestsPerRoom ?? 15, new List<int>())
            };

            Criteria criteria = new Criteria(_checkInDate, _checkOutDate, GetSearchRequestHotelIDs(), requestOccupancies);
            return new ConnectorSearch.SearchRequest(GetSessionId(), criteria, CreateSupplierObject());
        }

        public ConnectorSearch.SearchRequest GuestCountEqualMaxGuestsMultiroomSucceeds_Request()
        {
            List<RequestOccupancy> requestOccupancies = new List<RequestOccupancy>()
            {
                new RequestOccupancy(_metadata.MultiRoom.MaxGuestsPerRoom ?? 15, new List<int>()),
                new RequestOccupancy(_metadata.MultiRoom.MaxGuestsPerRoom ?? 15, new List<int>())
            };

            Criteria criteria = new Criteria(_checkInDate, _checkOutDate, GetSearchRequestHotelIDs(), requestOccupancies);
            return new ConnectorSearch.SearchRequest(GetSessionId(), criteria, CreateSupplierObject());
        }

        public ConnectorSearch.SearchRequest GuestCountGreaterThanMaxGuestsOneRoom_Request()
        {
            List<RequestOccupancy> requestOccupancies = new List<RequestOccupancy>()
            {
                new RequestOccupancy((_metadata.MultiRoom.MaxGuestsPerRoom ?? 15) + 1, new List<int>()),
                new RequestOccupancy(_metadata.MultiRoom.MaxGuestsPerRoom ?? 15, new List<int>())
            };

            Criteria criteria = new Criteria(_checkInDate, _checkOutDate, GetSearchRequestHotelIDs(), requestOccupancies);
            return new ConnectorSearch.SearchRequest(GetSessionId(), criteria, CreateSupplierObject());
        }

        public ConnectorSearch.SearchRequest GuestCountGreaterThanMaxGuestsAllRooms_Request()
        {
            List<RequestOccupancy> requestOccupancies = new List<RequestOccupancy>()
            {
                new RequestOccupancy((_metadata.MultiRoom.MaxGuestsPerRoom ?? 15) + 1, new List<int>()),
                new RequestOccupancy((_metadata.MultiRoom.MaxGuestsPerRoom ?? 15) + 1, new List<int>()),
            };

            Criteria criteria = new Criteria(_checkInDate, _checkOutDate, GetSearchRequestHotelIDs(), requestOccupancies);
            return new ConnectorSearch.SearchRequest(GetSessionId(), criteria, CreateSupplierObject());
        }

        public ConnectorSearch.SearchRequest HotelIDsMissing_Request()
        {
            Criteria criteria = new Criteria(_checkInDate, _checkOutDate, new List<string>(), GetRequestOccupancies());
            return new ConnectorSearch.SearchRequest(GetSessionId(), criteria, CreateSupplierObject());
        }

        public ConnectorSearch.SearchRequest CityInformationMissing_Request()
        {
            Model.Search.City city = null;
            Criteria criteria = new Criteria(_checkInDate, _checkOutDate, city, GetRequestOccupancies());
            return new ConnectorSearch.SearchRequest(GetSessionId(), criteria, CreateSupplierObject());
        }

        public ConnectorSearch.SearchRequest RadialRegionMissing_Request()
        {
            RadialRegion radionRegion = null;
            Criteria criteria = new Criteria(_checkInDate, _checkOutDate, radionRegion, GetRequestOccupancies());
            return new ConnectorSearch.SearchRequest(GetSessionId(), criteria, CreateSupplierObject());
        }

        public ConnectorSearch.SearchRequest MaxNumberOfRoomsSent_Request()
        {
            List<RequestOccupancy> requestOccupancies = new List<RequestOccupancy>();
            for (int i = 0; i < _metadata.MultiRoom.MaxRoomsPerBooking; i++)
            {
                requestOccupancies.Add(new RequestOccupancy(2, new List<int>() { 2, 12 }));
            }

            Criteria criteria = new Criteria(_checkInDate, _checkOutDate, GetSearchRequestHotelIDs(), requestOccupancies);
            return new ConnectorSearch.SearchRequest(GetSessionId(), criteria, CreateSupplierObject());
        }

        public ConnectorSearch.SearchRequest LessThanMaxNumberOfRoomsSent_Request()
        {
            List<RequestOccupancy> requestOccupancies = new List<RequestOccupancy>();
            for (int i = 0; i < (_metadata.MultiRoom.MaxRoomsPerBooking - 1); i++)
            {
                requestOccupancies.Add(new RequestOccupancy(2, new List<int>() { 2, 12 }));
            }

            Criteria criteria = new Criteria(_checkInDate, _checkOutDate, GetSearchRequestHotelIDs(), requestOccupancies);
            return new ConnectorSearch.SearchRequest(GetSessionId(), criteria, CreateSupplierObject());
        }

        public ConnectorSearch.SearchRequest EqualToMaxStayDurationSent_Request()
        {
            Criteria criteria = new Criteria(_bookingDate, _bookingDate.AddDays(_metadata?.Verbs?.Search?.MaxStay ?? 1), GetSearchRequestHotelIDs(), GetRequestOccupancies());
            return new ConnectorSearch.SearchRequest(GetSessionId(), criteria, CreateSupplierObject());
        }

        public ConnectorSearch.SearchRequest LessThanMaxStayDurationSent_Request()
        {
            Criteria criteria = new Criteria(_bookingDate, _bookingDate.AddDays((_metadata?.Verbs?.Search?.MaxStay ?? 2) - 1), GetSearchRequestHotelIDs(), GetRequestOccupancies());
            return new ConnectorSearch.SearchRequest(GetSessionId(), criteria, CreateSupplierObject());
        }

        public ConnectorSearch.SearchRequest GreaterThanMaxStayDurationSent_Request()
        {
            Criteria criteria = new Criteria(_bookingDate, _bookingDate.AddDays((_metadata?.Verbs?.Search?.MaxStay ?? 1) + 1), GetSearchRequestHotelIDs(), GetRequestOccupancies());
            return new ConnectorSearch.SearchRequest(GetSessionId(), criteria, CreateSupplierObject());
        }

        public ConnectorSearch.SearchRequest VerifyCheckoutDateAfterAdvanceBookingLimit_Request()
        {
            var advanceBookingLimit = 0;

            if (_metadata?.Verbs?.Search?.AdvanceBookingLimit != null)
            {
                advanceBookingLimit = Convert.ToInt32(_metadata.Verbs.Search.AdvanceBookingLimit);
            }

            Criteria criteria = new Criteria(_bookingDate.AddDays(advanceBookingLimit - 5), _bookingDate.AddDays(advanceBookingLimit + 1), GetSearchRequestHotelIDs(), GetRequestOccupancies());
            return new ConnectorSearch.SearchRequest(GetSessionId(), criteria, CreateSupplierObject());
        }

        public ConnectorSearch.SearchRequest VerifyCheckoutDateEqualAdvanceBookingLimit_Request()
        {
            var advanceBookingLimit = 1;

            if (_metadata?.Verbs?.Search?.AdvanceBookingLimit != null)
            {
                advanceBookingLimit = Convert.ToInt32(_metadata.Verbs.Search.AdvanceBookingLimit);
            }
            Criteria criteria = new Criteria(_bookingDate.AddDays(advanceBookingLimit - 5), _bookingDate.AddDays(advanceBookingLimit), GetSearchRequestHotelIDs(), GetRequestOccupancies());
            return new ConnectorSearch.SearchRequest(GetSessionId(), criteria, CreateSupplierObject());
        }

        public ConnectorSearch.SearchRequest VerifyCheckoutDateBeforeAdvanceBookingLimit_Request()
        {
            var advanceBookingLimit = 2;

            if (_metadata?.Verbs?.Search?.AdvanceBookingLimit != null)
            {
                advanceBookingLimit = Convert.ToInt32(_metadata.Verbs.Search.AdvanceBookingLimit);
            }

            Criteria criteria = new Criteria(_bookingDate.AddDays(advanceBookingLimit - 10), _bookingDate.AddDays(advanceBookingLimit - 5), GetSearchRequestHotelIDs(), GetRequestOccupancies());
            return new ConnectorSearch.SearchRequest(GetSessionId(), criteria, CreateSupplierObject());
        }

        private List<RequestOccupancy> GetRequestOccupancies()
        {
            return new List<RequestOccupancy>()
            {
                new RequestOccupancy(14, new List<int> {5}),
                new RequestOccupancy(13, new List<int> {2}),
                new RequestOccupancy(11, new List<int> {1}),
                new RequestOccupancy(2, new List<int> {5})
            };
        }
    }
}
