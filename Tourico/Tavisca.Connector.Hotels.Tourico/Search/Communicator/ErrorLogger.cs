using System.Linq;
using Tavisca.Connector.Hotels.Common;
using Tavisca.Connector.Hotels.ErrorHandling.Exceptions;
using Tavisca.Connector.Hotels.Model.Search;
using Tavisca.Platform.Common;
using static Tavisca.Connector.Hotels.Tourico.Common.Proxy.SupplierProxy;
using static Tavisca.Connector.Hotels.Tourico.Common.SupplierConstants;
using ConnectorSearch = Tavisca.Connector.Hotels.Model.Search;
namespace Tavisca.Connector.Hotels.Tourico.Search.Communicator
{
    public class ErrorLogger
    {
        internal void LogSupplierError( SearchResult supplierResponse, ConnectorSearch.SearchRequest request)
        {
            if (supplierResponse!=null)
            {
                //Added Supplier Error Handling
             //   var error = supplierResponse.Fault.Errors.First();
                //LogHelper.WriteSupplierErrorLog(supplierResponse.LogData[Logging.SupplierErrorCode].ToString(), supplierResponse.LogData[Logging.SupplierErrorMessage].ToString(), error.Code, error.Text,
                //        SearchConstants.MethodName, request.Supplier?.Name, request.Supplier?.Id,
                //        SearchConstants.Api, SearchConstants.Verb, HeadersHelper.GetHeaders(), httpResponse.Headers);
             //   throw new SupplierException(error.Code, error.Text, supplierResponse.Status);
            }
        }
    }
}
