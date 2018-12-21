using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Tavisca.Connector.Hotels.Model.Metadata;
using static Tavisca.Connector.Hotels.Host.Constants;

namespace Tavisca.Connector.Hotels.Host.Controllers
{
    [Route(WebApiRoute.BaseRoute)]
    public class HotelConfigSpecController : Controller
    {
        private readonly IHotelMetadata _metadata;

        public HotelConfigSpecController(IHotelMetadata metadata)
        {
            _metadata = metadata;
        }

        [HttpGet, Route(WebApiRoute.ConfigsSpecRoute)]
        public async Task<IActionResult> ConfigSpecHotel()
        {
            var response = await _metadata.GetConfigurationSpecAsync();
            return Ok(response);
        }
    }
}
