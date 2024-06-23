using System;
using System.ComponentModel.DataAnnotations;

namespace Backend.Models
{
    public class Transaction
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int UserId { get; set; }

        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "Amount must be greater than zero")]
        public decimal Amount { get; set; }

        [Required]
        [MaxLength(10)]
        public string Type { get; set; } = string.Empty; // Incoming or Outgoing

        [Required]
        public DateTime Date { get; set; }

        public User User { get; set; } = null!;

        [Required]
        [EmailAddress]
        public string RecipientEmail { get; set; } = string.Empty; // Nowa właściwość

        [Required]
        [MaxLength(255)]
        public string Description { get; set; } = string.Empty; // Nowa właściwość
    }
}