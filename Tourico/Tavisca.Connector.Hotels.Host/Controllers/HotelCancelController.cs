using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Tavisca.Connector.Hotels.Common;
using Tavisca.Connector.Hotels.ErrorHandling;
using Tavisca.Connector.Hotels.Model.Cancel;
using Tavisca.Connector.Hotels.WebAPI;
using Tavisca.Platform.Common.Profiling;
using static Tavisca.Connector.Hotels.Host.Constants;

namespace Tavisca.Connector.Hotels.Host.Controllers
{
    [Route(WebApiRoute.BaseRoute)]
    public class HotelCancelController : Controller
    {
        private readonly IHotelCancel _hotelCancel;

        public HotelCancelController(IHotelCancel hotelCancel)
        {
            _hotelCancel = hotelCancel;
        }

        [HttpPost, Route(WebApiRoute.CancelRoute)]
        public async Task<IActionResult> CancelHotel([FromBody] CancelRequest request)
        {
            using (var profileScope = new ProfileContext("HotelCancelController.CancelHotel", false))
            {
                var validationErrors = ModalValidator.GetRequestFormatErrors(ModelState);
                if (validationErrors != null && validationErrors.Count > 0)
                {
                    var error = new ErrorInfo(FaultCodes.ValidationFailure, ErrorMessages.ValidationFailure(), System.Net.HttpStatusCode.BadRequest, validationErrors);
                    return BadRequest(error);
                }
                CallContext.SetSupplierId(request?.Supplier?.Id);
                CallContext.SetSupplierName(request?.Supplier?.Name);
                var response = await _hotelCancel.CancelAsync(request);
                return Ok(response);
            }
        }
    }
}
