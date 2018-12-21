using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Tavisca.Connector.Hotels.WebAPI.HealthChecks;
using Tavisca.Platform.Common.Profiling;
using static Tavisca.Connector.Hotels.Host.Constants;
using System.Collections.Generic;

namespace Tavisca.Connector.Hotels.Host.Controllers
{
    [Route(WebApiRoute.BaseRoute)]
    public class HealthCheckController : Controller
    {
        private readonly HealthCheck _healthCheck;
        public HealthCheckController(HealthCheck healthCheck)
        {
            _healthCheck = healthCheck;
        }

        [HttpGet, Route(WebApiRoute.HealthCheckRoute)]
        public async Task<IActionResult> HealthCheck()
        {
            using (var profileScope = new ProfileContext("HealthCheckController.HealthCheck"))
            {
                var response = new List<string>();
                var configurationHealthStatus = await _healthCheck.GetConfigurationStatusAsync();
                var sessionHealthStatus = await _healthCheck.GetSessionStatusAsync();
                response.AddRange(configurationHealthStatus.Messages);
                response.AddRange(sessionHealthStatus.Messages);
                if (configurationHealthStatus.IsHealthy && sessionHealthStatus.IsHealthy)
                    return Ok(response);
                return BadRequest(response);
            }
        }
    }
}
