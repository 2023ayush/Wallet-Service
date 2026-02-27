using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using WalletService.Data;
using WalletService.DTOs;
using WalletService.Models;
using System.IdentityModel.Tokens.Jwt;

namespace WalletService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class WalletController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public WalletController(ApplicationDbContext context)
        {
            _context = context;
        }

        private string? GetCurrentUserId()
        {
            return User.FindFirstValue(JwtRegisteredClaimNames.Sub);
        }

        [HttpGet("balance")]
        public async Task<IActionResult> GetBalance()
        {
            var userId = GetCurrentUserId();
            if (string.IsNullOrEmpty(userId))
                return Unauthorized("User not found");

            var wallet = await _context.Wallets.FirstOrDefaultAsync(w => w.UserId == userId);
            if (wallet == null)
                return Ok(new { balance = 0 });

            return Ok(new { balance = wallet.Balance });
        }

        [HttpPost("topup")]
        public async Task<IActionResult> TopUp([FromBody] WalletRequest request)
        {
            if (request.Amount <= 0) return BadRequest("Amount must be positive");

            var userId = GetCurrentUserId();
            if (string.IsNullOrEmpty(userId)) return Unauthorized("User not found");

            var wallet = await _context.Wallets.FirstOrDefaultAsync(w => w.UserId == userId);

            if (wallet == null)
            {
                wallet = new Wallet
                {
                    UserId = userId,
                    Balance = request.Amount,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                };
                _context.Wallets.Add(wallet);
            }
            else
            {
                wallet.Balance += request.Amount;
                wallet.UpdatedAt = DateTime.UtcNow;
            }

            await _context.SaveChangesAsync();
            return Ok(new { balance = wallet.Balance });
        }

        [HttpPost("withdraw")]
        public async Task<IActionResult> Withdraw([FromBody] WalletRequest request)
        {
            if (request.Amount <= 0) return BadRequest("Amount must be positive");

            var userId = GetCurrentUserId();
            if (string.IsNullOrEmpty(userId)) return Unauthorized("User not found");

            var wallet = await _context.Wallets.FirstOrDefaultAsync(w => w.UserId == userId);
            if (wallet == null || wallet.Balance < request.Amount)
                return BadRequest("Insufficient balance");

            wallet.Balance -= request.Amount;
            wallet.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return Ok(new { balance = wallet.Balance });
        }
    }
}