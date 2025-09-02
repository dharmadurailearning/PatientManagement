using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PatientManagement.Api.Auth;


namespace PatientManagement.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly JwtTokenService _jwt;
        public AuthController(JwtTokenService jwt) { _jwt = jwt; }


        public record LoginRequest(string Username, string Password);


        [HttpPost("token")]
        [AllowAnonymous]
        public ActionResult<string> Token([FromBody] LoginRequest req)
        {
            if (req.Username == "admin" && req.Password == "P@ssw0rd!")
            {
                var token = _jwt.GenerateToken(req.Username, roles: new[] { "Admin" });
                return Ok(token);
            }
            return Unauthorized();
        }
    }
}