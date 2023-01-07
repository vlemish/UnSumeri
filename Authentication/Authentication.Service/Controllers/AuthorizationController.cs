using Microsoft.AspNetCore.Mvc;

namespace Authentication.Service.Controllers
{
    [ApiController]
    [Route("api/authorization")]
    public class AuthorizationController : ControllerBase
    {
        public AuthorizationController()
        {

        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(string username, string password)
        {
            await Task.Delay(200);
            return Ok();
        }
    }
}
