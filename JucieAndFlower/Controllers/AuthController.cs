using JucieAndFlower.Data.Enities.Login;
using JucieAndFlower.Data.Enities.User;
using JucieAndFlower.Service.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace JucieAndFlower.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : Controller
    {
        private readonly IUserService _userService;

        public AuthController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] UserRegisterDto dto)
        {
            var result = await _userService.RegisterAsync(dto);
            if (result == "Vui lòng kiểm tra email để xác nhận tài khoản.")
                return Ok(new { message = result });

            return BadRequest(new { error = result });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            var token = await _userService.LoginAsync(request.Email, request.Password);

            if (token == null)
            {
                return Unauthorized(new { message = "Invalid email or password" });
            }
            if (token is not null && token.GetType().GetProperty("error") != null)
            {
                return BadRequest(token);
            }

            return Ok(token); ;
        }

        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            var email = User.FindFirst(ClaimTypes.Email)?.Value;
            if (string.IsNullOrEmpty(email))
                return Unauthorized("Invalid token.");

            var result = await _userService.LogoutAsync(email);
            if (!result)
                return NotFound("User not found.");

            return Ok("Logged out successfully.");
        }

        [HttpPost("refresh-token")]
        public async Task<IActionResult> Refresh([FromBody] TokenModel model)
        {
            var result = await _userService.RefreshTokenAsync(model.AccessToken!, model.RefreshToken!);
            if (result == null)
                return Unauthorized("Invalid token.");

            return Ok(result);
        }

        [Authorize]
        [HttpPut("update-profile")]
        public async Task<IActionResult> UpdateProfile([FromBody] UserUpdateDto dto)
        {
            var email = User.FindFirst(ClaimTypes.Email)?.Value;
            if (string.IsNullOrEmpty(email))
                return Unauthorized("Invalid token.");

            var result = await _userService.UpdateUserProfileAsync(email, dto);
            if (!result.IsSuccess)
                return BadRequest(result.Message);

            return Ok("Update successful.");
        }

        [Authorize]
        [HttpGet("profile")]
        public async Task<IActionResult> GetProfile()
        {
            var email = User.FindFirst(ClaimTypes.Email)?.Value;
            if (string.IsNullOrEmpty(email))
                return Unauthorized("Invalid token.");

            var profile = await _userService.GetUserProfileAsync(email);
            if (profile == null)
                return NotFound("User not found.");

            return Ok(profile);
        }

        [HttpGet("verify-email")]
        public async Task<IActionResult> VerifyEmail(string email, string token)
        {
            var result = await _userService.VerifyEmailAsync(email, token);
            if (!result)
                return BadRequest("Invalid token or email");

            return Ok("Email verified successfully");
        }

    }
}
