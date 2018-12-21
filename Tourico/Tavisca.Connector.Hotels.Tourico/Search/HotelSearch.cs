using System;
using System.Threading.Tasks;
using Tavisca.Connector.Hotels.Tourico.Common;
using Tavisca.Connector.Hotels.Tourico.Search.Communicator;
using Tavisca.Connector.Hotels.Tourico.Search.Translators.Request;
using Tavisca.Connector.Hotels.Tourico.Search.Translators.Response;
using Tavisca.Connector.Hotels.Tourico.Search.Validation;
using Tavisca.Connector.Hotels.ErrorHandling;
using Tavisca.Connector.Hotels.ErrorHandling.Exceptions;
using Tavisca.Connector.Hotels.Model.Metadata;
using Tavisca.Connector.Hotels.Model.Search;
using Tavisca.Platform.Common;
using Tavisca.Platform.Common.Profiling;
using ConnectorSearch = Tavisca.Connector.Hotels.Model.Search;
using static Tavisca.Connector.Hotels.Tourico.Common.Proxy.SupplierProxy;

namespace Tavisca.Connector.Hotels.Tourico.Search
{
    public class HotelSearch : IHotelSearch
    {
        private readonly IHotelMetadata _metadata;
        private readonly IHttpConnector _connector;

        public HotelSearch(IHotelMetadata metadata, IHttpConnector connector)
        {
            _metadata = metadata;
            _connector = connector;
        }

        public async Task<SearchResponse> SearchAsync(ConnectorSearch.SearchRequest request)
        {
            var sessionId = Guid.NewGuid().ToString();
            SearchResponse response;
            try
            {
                using (var profileScope = new ProfileContext("Tourico-SearchAsync", false))
                {
                    //1.Request Validation
                    var errors = new SearchRequestValidator(_metadata).Validate(request);
                    if (errors != null && errors.Count > 0)
                        throw Errors.ClientSide.ValidationFailure(errors);

                    //2.Create Supplier Request
                    var supplierConfigurations = request.Supplier.GetConfigurations();
                    var supplierRequest = new RequestTranslator().CreateRequest(request, supplierConfigurations);

                    //3.Supplier Call
                    var httpResponse = await new SearchCommunicator(_connector).GetHotelsAsync(supplierRequest, request.Supplier, supplierConfigurations);
                    //TODO: If supplier provide error as different object then use GetResponseOrFaultAsync else use GetResponseAsync
                    //var response = await httpResponse.GetResponseAsync<SupplierSearchRs>();
                    var supplierResponse = await httpResponse.GetResponseOrFaultAsync<SupplierSearchRs, ErrorTypes>();

                    //4.Handle Supplier Error Exception (if response is failure)
                    new ErrorLogger().LogSupplierError(supplierResponse, httpResponse, request);

                    //5.Parse Supplier Response
                    response = new ResponseTranslator().ParseResponse(supplierResponse.Response, supplierConfigurations);
                }
            }
            catch (Exception exception)
            {
                var bas = exception as BaseApplicationException;
                if (bas != null)
                    throw bas;
                throw new ErrorHandling.Exceptions.SystemException(FaultCodes.System, FaultMessages.System, System.Net.HttpStatusCode.InternalServerError, exception);
            }
            return response;
        }
    }
}