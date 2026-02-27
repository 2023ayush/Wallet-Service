namespace WalletService.DTOs
{
    public class WalletResponse
    {
        public decimal Balance { get; set; }
    }

    public class WalletTransactionRequest
    {
        public decimal Amount { get; set; }
    }
}