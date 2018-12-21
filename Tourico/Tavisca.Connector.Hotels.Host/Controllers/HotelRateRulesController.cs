using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Tavisca.Connector.Hotels.Common;
using Tavisca.Connector.Hotels.ErrorHandling;
using Tavisca.Connector.Hotels.Model.RateRules;
using Tavisca.Connector.Hotels.WebAPI;
using Tavisca.Platform.Common.Profiling;
using static Tavisca.Connector.Hotels.Host.Constants;

namespace Tavisca.Connector.Hotels.Host.Controllers
{
    [Route(WebApiRoute.BaseRoute)]
    public class HotelRateRulesController : Controller
    {
        private readonly IHotelRateRules _hotelRateRules;

        public HotelRateRulesController(IHotelRateRules hotelRateRules)
        {
            _hotelRateRules = hotelRateRules;
        }

        [HttpPost, Route(WebApiRoute.RateRulesRoute)]
        public async Task<IActionResult> RateRulesHotel([FromBody] RateRulesRequest rateRulesRequest)
        {
            using (var profileScope = new ProfileContext("HotelRateRulesController.RateRulesHotel", false))
            {
                var validationErrors = ModalValidator.GetRequestFormatErrors(ModelState);
                if (validationErrors != null && validationErrors.Count > 0)
                {
                    var error = new ErrorInfo(FaultCodes.ValidationFailure, ErrorMessages.ValidationFailure(), System.Net.HttpStatusCode.BadRequest, validationErrors);
                    return BadRequest(error);
                }
                CallContext.SetSessionId(rateRulesRequest?.SessionId);
                CallContext.SetSupplierId(rateRulesRequest?.Supplier?.Id);
                CallContext.SetSupplierName(rateRulesRequest?.Supplier?.Name);
                var rateRulesResponse = await _hotelRateRules.GetRateRulesAsync(rateRulesRequest);
                return Ok(rateRulesResponse);
            }
        }
    }
}
