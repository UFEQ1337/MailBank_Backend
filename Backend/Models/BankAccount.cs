using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

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

        [JsonIgnore]
        public User User { get; set; } = null!;
    }
}