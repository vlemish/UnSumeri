using Microsoft.AspNetCore.Mvc;

namespace Authentication.Service.Controllers
{
    [ApiController]
    [Route("api/authorization")]
    public class AuthorizationController : ControllerBase
    {
        private readonly ILogger<AuthorizationController> _logger;

        public AuthorizationController(ILogger<AuthorizationController> logger)
        {
            _logger = logger;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(string username, string password)
        {
            _logger.LogInformation("Hello!");
            await Task.Delay(200);
            return Ok();
        }
    }
}
