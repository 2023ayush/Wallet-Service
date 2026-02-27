namespace WalletService.Models
{
    public class Wallet
    {
        public int Id { get; set; }
        public string UserId { get; set; } = null!; // FK to Identity User
        public decimal Balance { get; set; } = 0;

        // Optional: timestamps
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        // Navigation property
        public User User { get; set; } = null!;
    }
}
