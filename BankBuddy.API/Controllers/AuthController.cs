using BankBuddy.Application.DTOs.Auth;
using BankBuddy.Application.Interfaces.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using System.Security.Claims;

namespace BankBuddy.API.Controllers
{
    [Authorize]
    [Route("api/auth")]
    [ApiController]
    public class AuthController(IAuthService _authService) : ControllerBase
    {
        [EnableRateLimiting("register-policy")]
        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDTO dto)
        {
            AuthResponseDTO response = await _authService.RegisterAsync(dto);
            return Ok(response);
        }

        [EnableRateLimiting("login-policy")]
        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDTO dto)
        {
            AuthResponseDTO response = await _authService.LoginAsync(dto);
            return Ok(response);
        }

        [AllowAnonymous]
        [HttpPost("logout")]
        public async Task<IActionResult> Logout([FromBody] string refreshToken)
        {
            string response = await _authService.LogoutAsync(refreshToken);
            return Ok(response);
        }

        [AllowAnonymous]
        [HttpPost("refresh-token")]
        public async Task<IActionResult> RefreshToken([FromBody] string token)
        {
            AuthResponseDTO response = await _authService.RefreshTokenAsync(token);
            return Ok(response);
        }

        [EnableRateLimiting("password-policy")]
        [HttpPost("change-password")]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordDTO dto)
        {
            string userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value!;
            string response = await _authService.ChangePasswordAsync(Guid.Parse(userId), dto);
            return Ok(response);
        }

        [HttpPost("info")]
        public async Task<IActionResult> GetUserInfo()
        {
            string userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value!;
            UserInfoDTO response = await _authService.GetUserInfoAsync(Guid.Parse(userId));
            return Ok(response);
        }

        [HttpPost("send-verification")]
        public async Task<IActionResult> SendVerificationEmail()
        {
            Guid userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value!);
            string email = User.FindFirst(ClaimTypes.Email)?.Value!;
            string response = await _authService.SendVerificationEmailAsync(userId, email);
            return Ok(response);
        }

        [HttpGet("verify")]
        public async Task<IActionResult> VerifyEmail([FromQuery] string token)
        {
            string result = await _authService.VerifyEmailAsync(token);
            return Ok(result);
        }
    }
}
