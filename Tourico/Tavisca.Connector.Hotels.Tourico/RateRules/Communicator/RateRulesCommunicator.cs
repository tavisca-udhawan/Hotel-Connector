using System;
using System.Net;
using System.Threading.Tasks;
using Tavisca.Connector.Hotels.Common.Serialization;
using Tavisca.Connector.Hotels.Tourico.Common;
using Tavisca.Connector.Hotels.Tourico.Common.WebCaller;
using Tavisca.Connector.Hotels.ErrorHandling;
using Tavisca.Connector.Hotels.ErrorHandling.Exceptions;
using Tavisca.Connector.Hotels.Model.Common;
using Tavisca.Platform.Common;
using Tavisca.Platform.Common.Profiling;
using Tavisca.Platform.Common.Serialization;
using static Tavisca.Connector.Hotels.Tourico.Common.Proxy.SupplierProxy;

namespace Tavisca.Connector.Hotels.Tourico.RateRules.Communicator
{
    public class RateRulesCommunicator
    {
        private readonly IHttpConnector _connector;

        public RateRulesCommunicator(IHttpConnector connector)
        {
            _connector = connector;
        }

        internal async Task<HttpResponse> GetRateRulesAsync(SupplierRateRulesRq supplierRequest, Supplier supplier, SupplierConfiguration supplierConfiguration)
        {
            using (var profileScope = new ProfileContext("Tourico-ConnectorCall", false))
            {
                throw new NotImplementedException("Change serializer as per supplier request / response supports");
                //ToDo : Change serializer as per supplier request / response supports.
                var settings = new HttpSettings()
                                .WithSerializer(new XmlSerializer())
                                .WithConnector(_connector);

                var httpRequest = Http.NewPostRequest(new Uri(supplierConfiguration.RateRulesUrl), settings)
                    .WithBody(supplierRequest)
                    .WithTimeout(TimeSpan.FromSeconds(supplierConfiguration.TimeOutInSeconds))
                    .WithHeaders(HeadersHelper.GetHeaders())
                    .WithFaultPolicy(new TouricoFaultPolicy())
                    .WithHttpFilter(new Common.WebCaller.LoggingHttpFilter())
                    .WithHttpFilter(new ResponseDataExtractorHttpFilter<SupplierRateRulesRs, ErrorTypes>(new DataLogger().ExtractDataFromResponse))
                    .WithApiLogData(SupplierConstants.RateRulesConstants.Api, SupplierConstants.RateRulesConstants.Verb, SupplierConstants.RateRulesConstants.MethodName, supplierConfiguration)
                    .WithSupplierLogData(supplier);

                HttpResponse httpResponse = null;
                try
                {
                    httpResponse = await httpRequest.SendAsync();
                }
                catch (BaseApplicationException bae)
                {
                    throw bae;
                }
                catch (TimeoutException timeoutException)
                {
                    throw new SupplierTimeoutException(FaultCodes.TimeOut, FaultMessages.TimeOut, HttpStatusCode.RequestTimeout, timeoutException);
                }
                catch (Exception exception)
                {
                    throw new SupplierException(FaultCodes.SupplierException, FaultMessages.SupplierException, HttpStatusCode.InternalServerError, exception);
                }
                return httpResponse;
            }
        }
    }

    public class TouricoFaultPolicy : IFaultPolicy
    {
        public static IFaultPolicy Instance
        {
            get { return new TouricoFaultPolicy(); }
        }
        public Task<bool> IsFaultedAsync(HttpRequest req, HttpResponse res)
        {
            throw new NotImplementedException();

            //TODO: Add logic here to log - supplier call is success or failure. 
            //Here is a Sample logic which checks 
            //    if http status is 200 then success
            //    else failure

            //var statusCode = (int)(res?.Status);
            //return Task.FromResult(statusCode / 100 == 2);
        }
    }
}
