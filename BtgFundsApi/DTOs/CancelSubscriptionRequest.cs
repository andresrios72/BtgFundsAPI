namespace BtgFundsApi.DTOs
{
    public class CancelSubscriptionRequest
    {
        public string UserId { get; set; } = null!;
        public int FundId { get; set; }
    }
}
