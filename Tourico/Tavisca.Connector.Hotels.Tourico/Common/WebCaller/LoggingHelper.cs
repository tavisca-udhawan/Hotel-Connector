using System;
using System.Collections.Generic;
using System.Text;
using Tavisca.Connector.Hotels.Model.Common;
using Tavisca.Platform.Common;

namespace Tavisca.Connector.Hotels.Tourico.Common.WebCaller
{
    public static class LoggingHelper
    {
        public static HttpRequest WithApiLogData(this HttpRequest httpRequest, string api, string verb, string methodName, SupplierConfiguration supplierConfiguration)
        {
            httpRequest.WithLogData(SupplierConstants.Keys.Api, api);
            httpRequest.WithLogData(SupplierConstants.Keys.Verb, verb);
            httpRequest.WithLogData(SupplierConstants.Keys.MethodName, methodName);
            return httpRequest;
        }

        public static HttpRequest WithSupplierLogData(this HttpRequest httpRequest, Supplier supplier)
        {
            httpRequest.WithLogData(SupplierConstants.Keys.IsTestEnvironment, supplier.IsTestEnvironment);
            httpRequest.WithLogData(SupplierConstants.Keys.SupplierId, supplier.Id);
            httpRequest.WithLogData(SupplierConstants.Keys.SupplierName, supplier.Name);
            return httpRequest;
        }
    }
}
