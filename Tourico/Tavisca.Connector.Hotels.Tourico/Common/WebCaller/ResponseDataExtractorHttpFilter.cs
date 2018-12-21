using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Tavisca.Platform.Common;

namespace Tavisca.Connector.Hotels.Tourico.Common.WebCaller
{
    public class ResponseDataExtractorHttpFilter<SupplierRes, ErrorRes> : HttpFilter
    {
        private Func<SupplierRes, ErrorRes, IDictionary<string, object>> _responseDataExtractor;
        public ResponseDataExtractorHttpFilter(Func<SupplierRes, ErrorRes, IDictionary<string, object>> responseDataExtractor)
        {
            _responseDataExtractor = responseDataExtractor;
        }

        public async override Task<HttpResponse> ApplyAsync(HttpRequest request, CancellationToken cancellationToken = default(CancellationToken))
        {
            var httpResponse = await base.ApplyAsync(request, cancellationToken);
            if (_responseDataExtractor != null && httpResponse != null)
            {
                var responseObj = await httpResponse.GetResponseOrFaultAsync<SupplierRes, ErrorRes>();
                var dataExtractedFromResponse = _responseDataExtractor(responseObj.Response, responseObj.Fault);
                if (dataExtractedFromResponse != null)
                {
                    foreach (var data in dataExtractedFromResponse)
                    {
                        httpResponse.LogData[data.Key] = data.Value;
                    }
                }
            }
            return httpResponse;
        }
    }
}