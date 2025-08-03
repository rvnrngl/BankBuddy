using BankBuddy.Application.Commons.Utils;
using BankBuddy.Application.DTOs.Transaction;
using BankBuddy.Application.Interfaces.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BankBuddy.API.Controllers
{
    [Authorize]
    [Route("api/transaction")]
    [ApiController]
    public class TransactionController(ITransactionService _transactionService) : ControllerBase
    {
        [HttpPost("transfer")]
        public async Task<IActionResult> Transfer([FromBody] TransactionRequestDTO dto)
        {
            Guid userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value!);
            TransferReceiptDTO response = await _transactionService.TransferAsync(userId, dto);
            return Ok(response);
        }

        [HttpPost("deposit")]
        public async Task<IActionResult> Deposit([FromBody] DepositRequestDTO dto)
        {
            Guid userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value!);
            DepositReceiptDTO response = await _transactionService.DepositAsync(userId, dto);
            return Ok(response);
        }

        [HttpPost("withdraw")]
        public async Task<IActionResult> Withdraw([FromBody] WithdrawRequestDTO dto)
        {
            Guid userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value!);
            WithdrawReceiptDTO response = await _transactionService.WithdrawAsync(userId, dto);
            return Ok(response);
        }

        [HttpGet("historry")]
        public async Task<IActionResult> GetMyTransactionHistory([FromQuery] TransactionHistoryQueryDTO dto)
        {
            Guid userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value!);
            PaginatedResult<TransactionHistoryDTO> response = await _transactionService.GetMyTransactionHistoryAsync(userId, dto);
            return Ok(response);
        }
    }
}
