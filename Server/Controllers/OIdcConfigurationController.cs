using Microsoft.AspNetCore.ApiAuthorization.IdentityServer;
using Microsoft.AspNetCore.Mvc;

namespace BlazorMovies.Server.Controllers
{
    public class OIdcConfigurationController : Controller
    {
        private readonly ILogger<OIdcConfigurationController> _logger;

        public OIdcConfigurationController(IClientRequestParametersProvider clientRequestParametersProvider,
            ILogger<OIdcConfigurationController> logger)
        {
            ClientRequestParametersProvider = clientRequestParametersProvider;
            _logger = logger;
        }

        public IClientRequestParametersProvider ClientRequestParametersProvider { get; }

        [HttpGet("_configuration/{clientId}")]
        public IActionResult GetClientRequestParameters([FromRoute] string clientId)
        {
            var parameters = ClientRequestParametersProvider.GetClientParameters(HttpContext, clientId);
            return Ok(parameters);
        }
    }
}
