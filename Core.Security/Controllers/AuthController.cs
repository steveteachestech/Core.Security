using Core.Security.Models;
using Core.Security.Services;
using Microsoft.AspNetCore.Mvc;

namespace Core.Security.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase {
        private readonly AuthenticationService _authenticationService;

        public AuthController(AuthenticationService authenticationService) {
            _authenticationService = authenticationService;
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginModel model) {
            var token = _authenticationService.Authenticate(model.Username, model.Password);
            if (token == null)
                return Unauthorized();

            return Ok(new { Token = token });
        }
    }
}