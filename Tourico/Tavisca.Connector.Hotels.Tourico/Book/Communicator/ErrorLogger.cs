using System.Linq;
using Tavisca.Connector.Hotels.Common;
using Tavisca.Connector.Hotels.ErrorHandling.Exceptions;
using Tavisca.Connector.Hotels.Model.Book;
using Tavisca.Platform.Common;
using static Tavisca.Connector.Hotels.Tourico.Common.Proxy.SupplierProxy;
using static Tavisca.Connector.Hotels.Tourico.Common.SupplierConstants;

namespace Tavisca.Connector.Hotels.Tourico.Book.Communicator
{
    public class ErrorLogger
    {
        internal void LogSupplierError(ResponseOrFault<SupplierBookingRs, ErrorTypes> supplierResponse, HttpResponse httpResponse, BookRequest request)
        {
            if (supplierResponse.IsFaulted)
            {
                //Added Supplier Error Handling
                var error = supplierResponse.Fault.Errors.First();
                LogHelper.WriteSupplierErrorLog(httpResponse.LogData[Logging.SupplierErrorCode].ToString(), httpResponse.LogData[Logging.SupplierErrorMessage].ToString(), error.Code, error.Text,
                     BookConstants.MethodName, request.Supplier?.Name, request.Supplier?.Id,
                     BookConstants.Api, BookConstants.Verb, HeadersHelper.GetHeaders(), httpResponse.Headers);
                throw new SupplierException(error.Code, error.Text, httpResponse.Status);
            }
        }
    }
}
