using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Tavisca.Connector.Hotels.Common;
using Tavisca.Connector.Hotels.ErrorHandling;
using Tavisca.Connector.Hotels.Model.Retrieve;
using Tavisca.Connector.Hotels.WebAPI;
using Tavisca.Platform.Common.Profiling;
using static Tavisca.Connector.Hotels.Host.Constants;

namespace Tavisca.Connector.Hotels.Host.Controllers
{
    [Route(WebApiRoute.BaseRoute)]
    public class HotelRetrieveController : Controller
    {
        private readonly IRetrieve _retrieveService;

        public HotelRetrieveController(IRetrieve retrieveService)
        {
            _retrieveService = retrieveService;
        }

        [HttpPost, Route(WebApiRoute.RetrieveRoute)]
        public async Task<IActionResult> RetrieveBookings([FromBody] RetrieveRequest request)
        {
            using (var profileScope = new ProfileContext("HotelRetrieveController.RetrieveBookings", false))
            {
                var validationErrors = ModalValidator.GetRequestFormatErrors(ModelState);
                if (validationErrors != null && validationErrors.Count > 0)
                {
                    var error = new ErrorInfo(FaultCodes.ValidationFailure, ErrorMessages.ValidationFailure(), System.Net.HttpStatusCode.BadRequest, validationErrors);
                    return BadRequest(error);
                }
                CallContext.SetSupplierId(request?.Supplier?.Id);
                CallContext.SetSupplierName(request?.Supplier?.Name);
                var response = await _retrieveService.RetrieveAsync(request);
                return Ok(response);
            }
        }
    }
}
