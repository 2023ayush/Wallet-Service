using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using WalletService.Models;

namespace WalletService.Data
{
    public class ApplicationDbContext : IdentityDbContext<User>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<Wallet> Wallets { get; set; }
        // Add other tables here later (Wallets, Ledger, Transfers)
        // public DbSet<Wallet> Wallets { get; set; }
    }
}