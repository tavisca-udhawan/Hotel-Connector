using System.Linq;
using Tavisca.Connector.Hotels.Common;
using Tavisca.Connector.Hotels.ErrorHandling.Exceptions;
using Tavisca.Connector.Hotels.Model.RoomRates;
using Tavisca.Platform.Common;
using static Tavisca.Connector.Hotels.Tourico.Common.Proxy.SupplierProxy;
using static Tavisca.Connector.Hotels.Tourico.Common.SupplierConstants;

namespace Tavisca.Connector.Hotels.Tourico.RoomRates.Communicator
{
    public class ErrorLogger
    {
        internal void LogSupplierError(ResponseOrFault<SupplierRoomRatesRs, ErrorTypes> supplierResponse, HttpResponse httpResponse, Request request)
        {
            if (supplierResponse.IsFaulted)
            {
                //Added Supplier Error Handling
                var error = supplierResponse.Fault.Errors.First();
                LogHelper.WriteSupplierErrorLog(httpResponse.LogData[Logging.SupplierErrorCode].ToString(), httpResponse.LogData[Logging.SupplierErrorMessage].ToString(), error.Code, error.Text,
                      RoomRatesConstants.MethodName, request.Supplier?.Name, request.Supplier?.Id,
                      RoomRatesConstants.Api, RoomRatesConstants.Verb, HeadersHelper.GetHeaders(), httpResponse.Headers);
                throw new SupplierException(error.Code, error.Text, httpResponse.Status);

            }
        }
    }
}
