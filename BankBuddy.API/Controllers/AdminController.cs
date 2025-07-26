using BankBuddy.Application.DTOs.Auth;
using BankBuddy.Application.DTOs.BankAccount;
using BankBuddy.Application.Interfaces.IServices;
using BankBuddy.Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BankBuddy.API.Controllers
{
    [Route("api/admin")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class AdminController(IAdminService _adminService) : ControllerBase
    {
        #region UserModule

        [HttpPost("register")]
        public async Task<IActionResult> CreateAdmin([FromBody] RegisterDTO dto)
        {
            string response = await _adminService.CreateAdminAsync(dto);
            return Ok(response);
        }

        [HttpGet("users")]
        public async Task<IActionResult> GetUsers([FromQuery] string? role, bool? isVerified)
        {
            List<UserDetailsDTO> response = await _adminService.GetUsersAsync(role, isVerified);
            return Ok(response);
        }

        #endregion

        #region BankAccountModule

        [HttpGet("bank-accounts")]
        public async Task<IActionResult> GetBankAccounts([FromQuery] Guid? userId, AccountType? type, AccountStatus? status)
        {
            List<BankAccountResponseDTO> response = await _adminService.GetBankAccountsAsync(userId, type, status);
            return Ok(response);
        }

        [HttpPatch("bank-accounts/{id:guid}/status")]
        public async Task<IActionResult> UpdateAccountStatus(Guid id, [FromBody] AccountStatus status)
        {
            string response = await _adminService.UpdateAccountStatusAsync(id, status);
            return Ok(response);
        }

        #endregion

    }
}
