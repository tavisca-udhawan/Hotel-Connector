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
using Tavisca.Connector.Hotels.Tourico.Search.Proxy;

namespace Tavisca.Connector.Hotels.Tourico.Search.Communicator
{
    public class SearchCommunicator
    {
        private readonly IHttpConnector _connector;

        public SearchCommunicator(IHttpConnector connector)
        {
            _connector = connector;
        }

        internal async Task<SearchResult> GetHotelsAsync(SearchHotelsByIdRequest1 supplierRequest, SupplierConfiguration supplierConfiguration)
        {
            using (var profileScope = new ProfileContext("Tourico-ConnectorCall", false))
            {
           //  var httpRequest = CreateHttpRequest(supplierRequest, supplierConfiguration);
                 HttpResponse httpResponse = null;
                try
                {
                    var touricoClient = new TouricoClient(supplierConfiguration);
                      var hotelList =  touricoClient.GetHotels(supplierRequest);
                     return hotelList;
                    //httpResponse = await httpRequest.SendAsync();
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
                
            }
        }

     /*   private HttpRequest CreateHttpRequest(SearchHotelsByIdRequest1 supplierRequest, SupplierConfiguration supplierConfiguration)
               {
            var settings = new HttpSettings()
                                .WithSerializer(new XmlSerializer())
                                .WithConnector(_connector);



            var httpRequest = Http.NewGetRequest(new Uri(supplierConfiguration.Url), settings)
                .WithHeaders(HeadersHelper.GetHeaders())
                .WithFaultPolicy(new TouricoFaultPolicy().IsFaultedAsync)
                .WithHttpFilter(new Common.WebCaller.LoggingHttpFilter())
                .WithHttpFilter(new ResponseDataExtractorHttpFilter<SupplierSearchRs, ErrorTypes>(new DataLogger().ExtractDataFromSearchResponse))
                .WithApiLogData(SupplierConstants.SearchConstants.Api, SupplierConstants.SearchConstants.Verb, SupplierConstants.SearchConstants.MethodName, supplierConfiguration);
               // .WithSupplierLogData(supplier);
            return httpRequest;
        }*/
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
