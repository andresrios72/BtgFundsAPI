using Microsoft.AspNetCore.Mvc;
using BtgFundsApi.Services;
using BtgFundsApi.Models;
using Microsoft.AspNetCore.Authorization;

namespace BtgFundsApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class TransactionsController : ControllerBase
    {
        private readonly ITransactionService _transactionService;

        public TransactionsController(ITransactionService transactionService)
        {
            _transactionService = transactionService;
        }

        // GET: api/transactions/{userId}
        [HttpGet("{userId}")]
        public async Task<ActionResult<List<Transaction>>> GetTransactionsByUser(string userId)
        {
            var transactions = await _transactionService.GetByUserIdAsync(userId);
            if (transactions.Count == 0)
                return NotFound($"No se encontraron transacciones para el usuario con ID {userId}.");

            return Ok(transactions);
        }
    }
}
