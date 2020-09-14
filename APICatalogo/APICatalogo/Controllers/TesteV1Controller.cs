using Microsoft.AspNetCore.Mvc;

namespace APICatalogo.Controllers
{
    [ApiVersion("1.0")]
    [ApiVersion("1.1")]
    [Route("api/v{v:apiVersion}/teste")]
    [ApiController]
    public class TesteV1Controller : ControllerBase
    {
        [HttpGet]
        public IActionResult Get()
        {
            return Content("<html><body><h2>TesteV1Controller - V 1.0 </h2></body></html>", "text/html");
        }

        [HttpGet, MapToApiVersion("1.1")]
        public IActionResult GetVersao11()
        {
            return Content("<html><body><h2>TesteV1Controller - V 1.1 </h2></body></html>", "text/html");
        }
    }
}
