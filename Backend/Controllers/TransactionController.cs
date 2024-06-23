using Backend.Data;
using Backend.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionController : ControllerBase
    {
        private readonly MyDbContext _context;

        public TransactionController(MyDbContext context)
        {
            _context = context;
        }

        // Endpoint to check if the recipient email exists and is active
        [HttpGet("check-email")]
        public async Task<IActionResult> CheckEmail(string email)
        {
            var recipient = await _context.Users.FirstOrDefaultAsync(u => u.Email == email && u.Status == "Active");

            if (recipient == null)
            {
                return NotFound(new { message = "Odbiorca nie istnieje lub konto nie jest aktywne." });
            }

            return Ok(new { userId = recipient.Id });
        }

        // Endpoint to handle money transfers
        [HttpPost("transfer")]
        [Authorize]
        public async Task<IActionResult> Transfer([FromBody] TransferRequest request)
        {
            var sender = await _context.Users.Include(u => u.BankAccount).FirstOrDefaultAsync(u => u.Id == request.UserId);
            if (sender == null || sender.BankAccount == null)
            {
                return BadRequest(new { message = "Nieprawidłowy identyfikator użytkownika." });
            }

            var recipient = await _context.Users.Include(u => u.BankAccount).FirstOrDefaultAsync(u => u.Email == request.RecipientEmail && u.Status == "Active");
            if (recipient == null)
            {
                return BadRequest(new { message = "Odbiorca nie istnieje lub konto nie jest aktywne." });
            }

            if (sender.Id == recipient.Id)
            {
                return BadRequest(new { message = "Nie można wykonać przelewu do samego siebie." });
            }

            if (sender.BankAccount.Balance < request.Amount)
            {
                return BadRequest(new { message = "Niewystarczające środki na koncie." });
            }

            // Przeprowadzenie transakcji
            var transaction = new Transaction
            {
                UserId = sender.Id,
                Amount = request.Amount,
                Type = "Outgoing",
                Date = DateTime.UtcNow,
                Description = request.Description,
                RecipientEmail = request.RecipientEmail // ustawienie adresu e-mail odbiorcy
            };

            _context.Transactions.Add(transaction);
            sender.BankAccount.Balance -= request.Amount;

            var recipientTransaction = new Transaction
            {
                UserId = recipient.Id,
                Amount = request.Amount,
                Type = "Incoming",
                Date = DateTime.UtcNow,
                Description = request.Description,
                RecipientEmail = sender.Email // ustawienie adresu e-mail nadawcy
            };

            _context.Transactions.Add(recipientTransaction);
            recipient.BankAccount.Balance += request.Amount;

            await _context.SaveChangesAsync();

            return Ok(new { message = "Przelew został wysłany." });
        }

        // Endpoint to get the recent transactions for a user
        [HttpGet("recent-transactions/{userId}")]
        [Authorize]
        public async Task<IActionResult> GetRecentTransactions(int userId)
        {
            var transactions = await _context.Transactions
                .Where(t => t.UserId == userId)
                .OrderByDescending(t => t.Date)
                .Take(10)
                .ToListAsync();

            if (transactions == null || transactions.Count == 0)
            {
                return NotFound(new { message = "Brak transakcji." });
            }

            return Ok(transactions);
        }
    }

    public class TransferRequest
    {
        [Required]
        public string RecipientEmail { get; set; }

        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "Kwota musi być większa niż zero.")]
        public decimal Amount { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public int UserId { get; set; }
    }
}
