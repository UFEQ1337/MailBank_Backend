using System.ComponentModel.DataAnnotations;

namespace Backend.Models
{
    public class BankAccount
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int UserId { get; set; }

        [Required]
        [Range(0, double.MaxValue, ErrorMessage = "Balance must be a positive number")]
        public decimal Balance { get; set; }

        public User User { get; set; } = null!;
    }
}