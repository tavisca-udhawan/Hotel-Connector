using System;
using System.Linq;
using System.Threading.Tasks;
using Tavisca.Connector.Hotels.Common;
using Tavisca.Connector.Hotels.Tourico.Book.Communicator;
using Tavisca.Connector.Hotels.Tourico.Book.Translators.Request;
using Tavisca.Connector.Hotels.Tourico.Book.Translators.Response;
using Tavisca.Connector.Hotels.Tourico.Book.Validation;
using Tavisca.Connector.Hotels.Tourico.Common;
using Tavisca.Connector.Hotels.ErrorHandling;
using Tavisca.Connector.Hotels.ErrorHandling.Exceptions;
using Tavisca.Connector.Hotels.Model.Book;
using Tavisca.Connector.Hotels.Model.Metadata;
using Tavisca.Platform.Common;
using Tavisca.Platform.Common.Profiling;
using Tavisca.Platform.Common.Serialization;
using static Tavisca.Connector.Hotels.Tourico.Common.Proxy.SupplierProxy;
using static Tavisca.Connector.Hotels.Tourico.Common.SupplierConstants;

namespace Tavisca.Connector.Hotels.Tourico.Book
{
    public class HotelBook : IHotelBook
    {
        private readonly IHotelMetadata _metadata;
        private readonly IHttpConnector _connector;

        public HotelBook(IHotelMetadata metadata, IHttpConnector connector)
        {
            _metadata = metadata;
            _connector = connector;
        }

        public async Task<BookResponse> BookAsync(BookRequest request)
        {
            BookResponse response = null;
            try
            {
                using (var profileScope = new ProfileContext("HotelBook.BookAsync", false))
                {
                    //1.Request Validation
                    var errors = new BookRequestValidator(_metadata).Validate(request);
                    if (errors != null && errors.Count > 0)
                        throw Errors.ClientSide.ValidationFailure(errors);

                    //2.Create Supplier Request
                    var supplierConfigurations = request.Supplier.GetConfigurations();
                    var supplierRequest = new RequestTranslator().CreateRequest(request, supplierConfigurations);

                    //3.Supplier Call
                    var httpResponse = await new BookCommunicator(_connector).BookAsync(supplierRequest, request.Supplier, supplierConfigurations);
                    //TODO: If supplier provide error as different object then use GetResponseOrFaultAsync else use GetResponseAsync
                    //var response = await httpResponse.GetResponseAsync<SupplierSearchRs>();
                    var supplierResponse = await httpResponse.GetResponseOrFaultAsync<SupplierBookingRs, ErrorTypes>();

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
