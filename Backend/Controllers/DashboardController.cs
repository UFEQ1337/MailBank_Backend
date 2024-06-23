using Microsoft.AspNetCore.Mvc;
using Backend.Services;
using Backend.Models;
using System.Linq;
using Backend.Data;

namespace Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DashboardController : ControllerBase
    {
        private readonly MyDbContext _context;

        public DashboardController(MyDbContext context)
        {
            _context = context;
        }

        [HttpGet("{userId}")]
        public IActionResult GetDashboardData(int userId)
        {
            var user = _context.Users.Find(userId);
            if (user == null)
            {
                return NotFound(new { message = "User not found" });
            }

            var bankAccount = _context.BankAccounts.FirstOrDefault(b => b.UserId == userId);
            var transactions = _context.Transactions
                .Where(t => t.UserId == userId)
                .OrderByDescending(t => t.Date)
                .Take(5)
                .ToList();

            var dashboardData = new
            {
                User = user,
                BankAccount = bankAccount,
                Transactions = transactions
            };

            return Ok(dashboardData);
        }
    }
}