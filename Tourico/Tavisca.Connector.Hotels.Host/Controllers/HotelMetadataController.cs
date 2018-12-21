using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Tavisca.Connector.Hotels.Model.Metadata;
using static Tavisca.Connector.Hotels.Host.Constants;

namespace Tavisca.Connector.Hotels.Host.Controllers
{
    [Route(WebApiRoute.BaseRoute)]
    public class HotelMetadataController : Controller
    {
        private readonly IHotelMetadata _metadata;

        public HotelMetadataController(IHotelMetadata metadata)
        {
            _metadata = metadata;
        }

        [HttpGet, Route(WebApiRoute.MetaDataRoute)]
        public async Task<IActionResult> MetadataHotel()
        {
            var response = await _metadata.GetMetadataAsync();
            return Ok(response);
        }
    }
}
