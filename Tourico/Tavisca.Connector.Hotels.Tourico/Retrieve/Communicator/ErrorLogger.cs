using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tavisca.Connector.Hotels.Common;
using Tavisca.Connector.Hotels.ErrorHandling.Exceptions;
using Tavisca.Connector.Hotels.Model.Retrieve;
using Tavisca.Platform.Common;
using static Tavisca.Connector.Hotels.Tourico.Common.Proxy.SupplierProxy;
using static Tavisca.Connector.Hotels.Tourico.Common.SupplierConstants;

namespace Tavisca.Connector.Hotels.Tourico.Retrieve.Communicator
{
    public class ErrorLogger
    {
        internal void LogSupplierError(ResponseOrFault<SupplierRetrieveRs, ErrorTypes> supplierResponse, HttpResponse httpResponse, RetrieveRequest request)
        {
            if (supplierResponse.IsFaulted)
            {
                //Added Supplier Error Handling
                var error = supplierResponse.Fault.Errors.First();
                LogHelper.WriteSupplierErrorLog(httpResponse.LogData[Logging.SupplierErrorCode].ToString(), httpResponse.LogData[Logging.SupplierErrorMessage].ToString(), error.Code, error.Text,
                      RetrieveConstants.TraceLogMethodName, request.Supplier?.Name, request.Supplier?.Id,
                      RetrieveConstants.Api, RetrieveConstants.Verb, HeadersHelper.GetHeaders(), httpResponse.Headers);
                throw new SupplierException(error.Code, error.Text, httpResponse.Status);
            }
        }
    }
}
