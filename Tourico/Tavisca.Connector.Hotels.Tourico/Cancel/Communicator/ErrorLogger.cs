using System.Linq;
using Tavisca.Connector.Hotels.Common;
using Tavisca.Connector.Hotels.ErrorHandling.Exceptions;
using Tavisca.Connector.Hotels.Model.Cancel;
using Tavisca.Platform.Common;
using static Tavisca.Connector.Hotels.Tourico.Common.Proxy.SupplierProxy;
using static Tavisca.Connector.Hotels.Tourico.Common.SupplierConstants;

namespace Tavisca.Connector.Hotels.Tourico.Cancel.Communicator
{
    public class ErrorLogger
    {
        internal void LogSupplierError(ResponseOrFault<SupplierCancelRs, ErrorTypes> supplierResponse, HttpResponse httpResponse, CancelRequest request)
        {
            if (supplierResponse.IsFaulted)
            {
                //Added Supplier Error Handling
                var error = supplierResponse.Fault.Errors.First();
                LogHelper.WriteSupplierErrorLog(httpResponse.LogData[Logging.SupplierErrorCode].ToString(), httpResponse.LogData[Logging.SupplierErrorMessage].ToString(), error.Code, error.Text,
                     CancelConstants.MethodName, request.Supplier?.Name, request.Supplier?.Id,
                     CancelConstants.Api, CancelConstants.Verb, HeadersHelper.GetHeaders(), httpResponse.Headers);
                throw new SupplierException(error.Code, error.Text, httpResponse.Status);
            }
        }
    }
}
