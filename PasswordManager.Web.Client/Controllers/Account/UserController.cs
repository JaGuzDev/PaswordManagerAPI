using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PasswordManager.Domain.Service;
using PasswordManager.Model.Builder;
using PasswordManager.Model.Dto;
using PasswordManager.Model.ViewModel;
using System.Security.Claims;

namespace PasswordManager.Web.Client.Controllers.Account
{
    [Authorize]
    [ApiController]
    [Route("users")]
    public class UserController : Controller
    {
        private readonly IUserService _userService;
        private readonly IUserModelBuilder _userModelBuilder;

        public UserController(IUserService userService, IUserModelBuilder userModelBuilder)
        {
            _userService = userService;
            _userModelBuilder = userModelBuilder;
        }        

        // GET /users
        [HttpGet]
        public async Task<IActionResult> GetUsersAsync()
        {
            var userId = Convert.ToInt64(HttpContext.User.Claims.First(c => c.Type.Equals(ClaimTypes.NameIdentifier)).Value);
            var users = await _userService.GetManyAsync();
            if(users == null)
            {
                return NotFound();
            }
            var userResponseViewModel = _userModelBuilder.Build(users);
            return Ok(userResponseViewModel);
        }

        // GET /users/current
        [HttpGet("current")]
        public async Task<IActionResult> GetCurrentUserAsync()
        {
            var userId = Convert.ToInt64(HttpContext.User.Claims.First(c => c.Type.Equals(ClaimTypes.NameIdentifier)).Value);
            var user = await _userService.GetByIdAsync(userId);
            if (user == null)
            {
                return NotFound();
            }
            var userResponseViewModel = _userModelBuilder.Build(user);
            return Ok(userResponseViewModel);
        }

        // GET /users/{userId}
        [HttpGet("{userId:long}")]
        public async Task<IActionResult> GetByIdAsync(long userId)
        {
            var user = await _userService.GetByIdAsync(userId);
            if (user == null)
            {
                return NotFound();
            }
            var userResponseViewModel = _userModelBuilder.Build(user);
            return Ok(userResponseViewModel);
        }

        // POST /users
        [HttpPost]
        public async Task<IActionResult> CreateAsync(UserCreateDto userCreateDto)
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
                return BadRequest(new { message = "Failed to update user data.", errors });
            }
            return Ok();
        }

        // PUT /users/{userId}
        [HttpPut("{userId:long}")]
        public async Task<IActionResult> UpdateAsync(long userId, [FromBody] UserViewModel userViewModel)
        {
            if (userViewModel == null)
            {
                return BadRequest(new { message = "User data is required." });
            }
            var user = _userModelBuilder.Build(userViewModel);
            user.Id = userId;
            var result = await _userService.UpdateAsync(user);
            if (!result)
            {
                var errors = user.ValidationResult.Errors.Select(e => e.ErrorMessage).ToList();
                return BadRequest(new { message = "Failed to update user data.", errors });
            }
            return Ok();
        }

        // DELETE /users/{entryId}
        [HttpDelete("{userId:long}")]
        public async Task<IActionResult> DeleteAsync(long userId)
        {
            var result = await _userService.DeleteAsync(userId);
            if (!result)
            {
                return NotFound();
            }
            return Ok();
        }
    }
}
