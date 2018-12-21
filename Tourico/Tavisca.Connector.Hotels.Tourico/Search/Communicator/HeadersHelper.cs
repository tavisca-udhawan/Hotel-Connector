using System.Collections.Generic;
using System.Collections.Specialized;
using Tavisca.Platform.Common;
using static Tavisca.Connector.Hotels.Tourico.Common.SupplierConstants;

namespace Tavisca.Connector.Hotels.Tourico.Search.Communicator
{

    public static class HeadersHelper
    {
        public static NameValueCollection GetHeaders()
        {
            //TODO: Add headers here which required by supplier
            var headers = new NameValueCollection
            {
                {CommonConstants.AcceptEncoding, CommonConstants.AcceptEncodingValue}
            };
            return headers;
        }

        public static HttpRequest WithHeaders(this HttpRequest httpRequest, NameValueCollection headers)
        {
            if (headers != null)
            {
                foreach (var headerName in headers.AllKeys)
                {
                    httpRequest.WithHeader(headerName, headers[headerName]);
                }
            }
            return httpRequest;
        }
    }
}
