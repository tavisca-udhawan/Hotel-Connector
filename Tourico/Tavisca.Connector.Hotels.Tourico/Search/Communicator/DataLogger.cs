﻿using System;
using System.Collections.Generic;
using Tavisca.Connector.Hotels.Tourico.Common;
using Tavisca.Connector.Hotels.ErrorHandling;
using Tavisca.Connector.Hotels.ErrorHandling.ErrorMapping;
using static Tavisca.Connector.Hotels.Tourico.Common.Proxy.SupplierProxy;

namespace Tavisca.Connector.Hotels.Tourico.Search.Communicator
{
    public class DataLogger
    {
        public Dictionary<string, object> ExtractDataFromSearchResponse(SupplierSearchRs response, ErrorTypes errorTypes)
        {
            var additionalInfo = new Dictionary<string, object>();
            if (errorTypes != null)
            {
                foreach (var error in errorTypes.Errors)
                {
                    additionalInfo.Add(SupplierConstants.Logging.SupplierErrorCode, error.Code);
                    additionalInfo.Add(SupplierConstants.Logging.SupplierErrorMessage, error?.Text);

                    //TODO: Warning Handling is not supported in ErrorMap. Need to revisit this code.
                    var errorInfo = new ErrorMapping().GetSupplierMappedError(error.Code, error.Text);
                    if (errorInfo.Code.Equals(FaultCodes.UnMappedSupplier, StringComparison.CurrentCultureIgnoreCase))
                    {
                        additionalInfo.Add(SupplierConstants.Logging.IsUnMapped, true);
                    }
                    else
                    {
                        additionalInfo.Add(SupplierConstants.Logging.IsUnMapped, false);
                        additionalInfo.Add(SupplierConstants.Logging.ErrorCode, errorInfo?.Code);
                        additionalInfo.Add(SupplierConstants.Logging.ErrorMessage, errorInfo?.Message);
                    }
                }
            }
            //ToDo : Add supplier specific logic to get hotel count from supplier response object.
            if (response?.Itineraries != null & response.Itineraries.Count > 0)
                additionalInfo.Add("result_count", response?.Itineraries.Count);
            throw new NotImplementedException();

            return additionalInfo;
        }
    }
}
