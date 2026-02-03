using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PasswordManager.Domain.Service;
using PasswordManager.Model.Builder;
using PasswordManager.Model.Dto;

namespace PasswordManager.Web.Client.Controllers.Account
{
    [ApiController]
    [Route("auth")]
    public class AuthController : Controller
    {
        private readonly IUserService _userService;
        private readonly IAuthTokenService _authTokenService;
        private readonly IUserModelBuilder _userModelBuilder;

        public AuthController(
            IUserService userService,
            IAuthTokenService authTokenService,
            IUserModelBuilder userModelBuilder)
        {
            _userService = userService;
            _authTokenService = authTokenService;
            _userModelBuilder = userModelBuilder;
        }

        // POST /login
        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDto loginRequest)
        {
            var user = await _userService.AuthenticateAsync(loginRequest.Username, loginRequest.Password);
            if (user == null)
                return Unauthorized(new { message = "Invalid credentials" });

            var authTokenViewModel = await _authTokenService.GenerateJwtTokenAsync(user, Request.Headers["User-Agent"].ToString());
            return Ok(new { authToken = authTokenViewModel });
        }

        // POST /revoke
        [Authorize]
        [HttpPost("revoke")]
        public async Task<IActionResult> RevokeToken([FromBody] string token)
        {
            var result = await _authTokenService.RevokeAsync(token);
            if (!result)
                return NotFound();
            return Ok();
        }

        // POST /refresh
        [AllowAnonymous]
        [HttpPost("refresh")]
        public async Task<IActionResult> RefreshToken([FromBody] string refreshToken)
        {
            var authTokenViewModel = await _authTokenService.RefreshAsync(refreshToken, Request.Headers["User-Agent"].ToString());
            if (authTokenViewModel == null)
                return Unauthorized();
            return Ok(new { authToken = authTokenViewModel });
        }

        // POST /logout
        [Authorize]
        [HttpPost("logout")]
        public async Task<IActionResult> Logout([FromBody] string token)
        {
            var result = await _authTokenService.RevokeAsync(token);
            if (!result)
                return NotFound();
            return Ok();
        }

        // POST /verify
        [AllowAnonymous]
        [HttpPost("verify")]
        public async Task<IActionResult> VerifyToken([FromBody] string token)
        {
            var authToken = await _authTokenService.GetByTokenAsync(token);
            if (authToken == null || authToken.RevokedAt != null || authToken.ExpiresAt < DateTime.UtcNow)
            {
                return Unauthorized();
            }
            return Ok(new { valid = true });
        }

        // POST /register
        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] UserCreateDto userCreateDto)
        {
            if (userCreateDto == null)
            {
                return BadRequest(new { message = "User data is required." });
            }
            var user = _userModelBuilder.Build(userCreateDto);
            await _userService.CreateAsync(user);
            if (!user.ValidationResult.IsValid)
            {
                var errors = user.ValidationResult.Errors.Select(e => e.ErrorMessage).ToList();
                return BadRequest(new { message = "Failed to register user.", errors });
            }
            return Ok();
        }
    }
}
