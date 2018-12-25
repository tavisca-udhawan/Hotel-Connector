using System;
using Tavisca.Connector.Hotels.Tourico.Common;
using Tavisca.Connector.Hotels.Model.Search;
using static Tavisca.Connector.Hotels.Tourico.Common.Proxy.SupplierProxy;
using ConnectorSearch = Tavisca.Connector.Hotels.Model.Search;
using System.Collections.Generic;
using System.Linq;
using Tavisca.Connector.Hotels.Model.Common;
namespace Tavisca.Connector.Hotels.Tourico.Search.Translators.Request
{
    public class RequestTranslator
    {
        internal SearchHotelsByIdRequest1 CreateRequest(ConnectorSearch.SearchRequest request, SupplierConfiguration supplierConfiguration)
        {
            return new SearchHotelsByIdRequest1
            {
                AuthenticationHeader= HeaderAuthentication(request),
                request= SearchRequest(request),
                features= GetFeatures(request)
            };
        }
      
        private AuthenticationHeader HeaderAuthentication(ConnectorSearch.SearchRequest request)
        {
            return new AuthenticationHeader
            {
                LoginName=request.Supplier.Configurations.Find(userName=>userName.Name=="userName").Value,
                Password=request.Supplier.Configurations.Find(userPassword=>userPassword.Name=="password").Value,
                Version=request.Supplier.Configurations.Find(version => version.Name == "version").Value,
                Culture=Culture.en_US
            };
        }
        private SearchHotelsByIdRequest SearchRequest(ConnectorSearch.SearchRequest request)
        {
            return new SearchHotelsByIdRequest
            {
                CheckIn=request.Criteria.CheckIn,
                CheckOut=request.Criteria.CheckOut,
                RoomsInformation = GetRoomsInformation(request.Criteria.Occupancies),
                HotelIdsInfo = GetHotelIds(request.Criteria.HotelIds,request.Supplier),
                AvailableOnly =true,
                MaxPrice=0,
                StarLevel=0

            };
        }
        private HotelIdInfo[] GetHotelIds(List<string> hotelIds, Supplier supplier)
        {
            List<string> invalidHotelIds = new List<string>();
            var hotelIdInfos = new List<HotelIdInfo>();
            foreach (var hotelIdString in hotelIds)
            {
                int hotelId;
                if (int.TryParse(hotelIdString, out hotelId))
                {
                    var hotelIdInfo = new HotelIdInfo { id = Convert.ToInt32(hotelId) };
                    hotelIdInfos.Add(hotelIdInfo);
                }
                else
                {
                    invalidHotelIds.Add(hotelIdString);
                }
            }
           
            return hotelIdInfos.ToArray();
        }
        private RoomInfo[] GetRoomsInformation(List<RequestOccupancy> occupancies)
        {
            return occupancies.Select(occupancy => new RoomInfo
            {
                AdultNum = occupancy.NumOfAdults,
                ChildAges = GetChildAges(occupancy.ChildAges),
                ChildNum = occupancy.ChildAges.Count
            }).ToArray();
        }

        private ChildAge[] GetChildAges(List<int> childAges)
        {
            if (childAges == null || childAges.Count == 0)
                return null;

            return childAges.Select(childAge => new ChildAge
            {
                age = childAge
            }).ToArray();
        }
        private Feature[] GetFeatures(ConnectorSearch.SearchRequest request)
        {
            if (request.Filters.Equals(null))
                return null;
            return request.Filters.ToString().Select(filters => new Feature
            {
            }).ToArray();
        }
    }
}
