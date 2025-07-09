namespace BtgFundsApi.DTOs
{
    public class SubscribeFundRequest
    {
        public string UserId { get; set; } = null!;
        public int FundId { get; set; }
        public int Amount { get; set; }
    }
}
