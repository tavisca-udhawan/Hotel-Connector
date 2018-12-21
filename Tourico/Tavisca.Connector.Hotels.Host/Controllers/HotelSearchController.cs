using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Tavisca.Connector.Hotels.Common;
using Tavisca.Connector.Hotels.ErrorHandling;
using Tavisca.Connector.Hotels.Model.Search;
using Tavisca.Connector.Hotels.WebAPI;
using Tavisca.Platform.Common.Profiling;
using ConnectorSearch = Tavisca.Connector.Hotels.Model.Search;
using static Tavisca.Connector.Hotels.Host.Constants;

namespace Tavisca.Connector.Hotels.Host.Controllers
{
    [Route(WebApiRoute.BaseRoute)]
    public class HotelSearchController  : Controller
    {
        private readonly IHotelSearch _hotelSearch;
        
        public HotelSearchController(IHotelSearch hotelSearch)
        {
            _hotelSearch = hotelSearch;
        }

        [HttpPost, Route(WebApiRoute.SearchRoute)]
        public async Task<IActionResult> SearchHotels([FromBody] ConnectorSearch.SearchRequest request)
        {
            using (var profileScope = new ProfileContext("HotelSearchController.SearchHotels", false))
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
                var response = await _hotelSearch.SearchAsync(request);
                return Ok(response);
            }
        }
    }
}
