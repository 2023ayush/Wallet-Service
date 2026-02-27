using Microsoft.AspNetCore.Identity;

namespace WalletService.Models
{
    public class User : IdentityUser
    {
        // Additional fields if needed
        public string FullName { get; set; }
        public bool IsActive { get; set; } = true;
    }
}