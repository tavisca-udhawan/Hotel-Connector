using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Tavisca.Connector.Hotels.Common;
using Tavisca.Connector.Hotels.ErrorHandling;
using Tavisca.Connector.Hotels.Model.Book;
using Tavisca.Connector.Hotels.WebAPI;
using Tavisca.Platform.Common.Profiling;
using static Tavisca.Connector.Hotels.Host.Constants;

namespace Tavisca.Connector.Hotels.Host.Controllers
{
    [Route(WebApiRoute.BaseRoute)]
    public class HotelBookController : Controller
    {
        private readonly IHotelBook _hotelBook;

        public HotelBookController(IHotelBook hotelBook)
        {
            _hotelBook = hotelBook;
        }

        [HttpPost, Route(WebApiRoute.BookRoute)]
        public async Task<IActionResult> BookHotel([FromBody] BookRequest request)
        {
            using (var profileScope = new ProfileContext("HotelBookController.BookHotel", false))
            {
                var validationErrors = ModalValidator.GetRequestFormatErrors(ModelState);
                if (validationErrors != null && validationErrors.Count > 0)
                {
                    var error = new ErrorInfo(FaultCodes.ValidationFailure, ErrorMessages.ValidationFailure(), System.Net.HttpStatusCode.BadRequest, validationErrors);
                    return BadRequest(error);
                }
                CallContext.SetSessionId(request?.SessionId);
                CallContext.SetSupplierId(request?.Supplier?.Id);
                CallContext.SetSupplierName(request?.Supplier?.Name);
                var response = await _hotelBook.BookAsync(request);
                return Ok(response);
            }
        }
    }
}
