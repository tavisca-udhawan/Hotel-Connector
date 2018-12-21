using System;
using System.Threading.Tasks;
using Tavisca.Connector.Hotels.Tourico.Common;
using Tavisca.Connector.Hotels.Tourico.RoomRates.Communicator;
using Tavisca.Connector.Hotels.Tourico.RoomRates.Translators.Request;
using Tavisca.Connector.Hotels.Tourico.RoomRates.Translators.Response;
using Tavisca.Connector.Hotels.Tourico.RoomRates.Validation;
using Tavisca.Connector.Hotels.ErrorHandling;
using Tavisca.Connector.Hotels.ErrorHandling.Exceptions;
using Tavisca.Connector.Hotels.Model.Metadata;
using Tavisca.Connector.Hotels.Model.RoomRates;
using Tavisca.Platform.Common;
using Tavisca.Platform.Common.Profiling;
using static Tavisca.Connector.Hotels.Tourico.Common.Proxy.SupplierProxy;

namespace Tavisca.Connector.Hotels.Tourico.RoomRates
{
    public class HotelRoomRates : IHotelRoomRates
    {
        private readonly IHotelMetadata _metadata;
        private readonly IHttpConnector _connector;

        public HotelRoomRates(IHotelMetadata metadata, IHttpConnector connector)
        {
            _metadata = metadata;
            _connector = connector;
        }

        public async Task<Response> GetRoomRatesAsync(Request request)
        {
            Response response = null;
            try
            {
                using (var profileScope = new ProfileContext("HotelRoomRates.GetRoomRatesAsync", false))
                {
                    //1.Request Validation
                    var errors = new RoomRatesRequestValidator(_metadata).Validate(request);
                    if (errors != null && errors.Count > 0)
                        throw Errors.ClientSide.ValidationFailure(errors);

                    //2.Create Supplier Request
                    var supplierConfigurations = request.Supplier.GetConfigurations();
                    var supplierRequest = new RequestTranslator().CreateRequest(request, supplierConfigurations);

                    //3.Supplier Call
                    var httpResponse = await new RoomRatesCommunicator(_connector).GetRoomRatesAsync(supplierRequest, request.Supplier, supplierConfigurations);
                    //TODO: If supplier provide error as different object then use GetResponseOrFaultAsync else use GetResponseAsync
                    //var response = await httpResponse.GetResponseAsync<SupplierSearchRs>();
                    var supplierResponse = await httpResponse.GetResponseOrFaultAsync<SupplierRoomRatesRs, ErrorTypes>();

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
