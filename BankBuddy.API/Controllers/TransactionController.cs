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
    }
}
