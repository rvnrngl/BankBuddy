using BankBuddy.Application.DTOs.Auth;
using BankBuddy.Application.DTOs.BankAccount;
using BankBuddy.Application.Interfaces.IServices;
using BankBuddy.Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BankBuddy.API.Controllers
{
    [Authorize]
    [Route("api/bank-accounts")]
    [ApiController]
    public class BankAccountController(IBankAccountService _bankAccountService) : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> CreateAccount([FromBody] AccountType type)
        {
            Guid userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value!);
            BankAccountResponseDTO response = await _bankAccountService.CreateAccountAsync(userId, type);
            return Ok(response);
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetUserBankAccount(Guid id)
        {
            Guid userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value!);
            BankAccountResponseDTO response = await _bankAccountService.GetUserBankAccountAsync(userId, id);
            return Ok(response);
        }

        [HttpGet]
        public async Task<IActionResult> GetUserBankAccounts([FromQuery] AccountType? type, AccountStatus? status)
        {
            Guid userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value!);
            List<BankAccountResponseDTO> response = await _bankAccountService.GetUserBankAccountsAsync(userId, type, status);
            return Ok(response);
        }

        #region PublicEndpoint

        [AllowAnonymous]
        [HttpGet("account-types")]
        public IActionResult GetAccountTypes()
        {
            var types = Enum.GetNames(typeof(AccountType));
            return Ok(types);
        }

        [AllowAnonymous]
        [HttpGet("account-statuses")]
        public IActionResult GetAccountStatuses()
        {
            var statuses = Enum.GetNames(typeof(AccountStatus));
            return Ok(statuses);
        }

        #endregion
    }
}
