using System;
using Tavisca.Connector.Hotels.Tourico.Common;
using Tavisca.Connector.Hotels.Model.Search;
using static Tavisca.Connector.Hotels.Tourico.Common.Proxy.SupplierProxy;
using ConnectorSearch = Tavisca.Connector.Hotels.Model.Search;
namespace Tavisca.Connector.Hotels.Tourico.Search.Translators.Request
{
    public class RequestTranslator
    {
        internal SupplierSearchRq CreateRequest(ConnectorSearch.SearchRequest request, SupplierConfiguration supplierConfiguration)
        {
            return new SupplierSearchRq
            {
                HotelIdInfo = request.Criteria.HotelIds,
                CheckIn = request.Criteria.CheckIn,
                CheckOut = request.Criteria.CheckOut,
                AdultNum = request.Criteria.Occupancies[0].NumOfAdults,
                ChildNum = request.Criteria.Occupancies[0].ChildAges.Count,
                ChildAge= request.Criteria.Occupancies[0].ChildAges
               // MaxPrice
                //StarLevel
                //AvailableOnly,
                //Features
            };
        }
    }
}
