namespace AirSoftAutomationFramework.Objects.DTOs
{
    public class PostTradeRequest
    {
        public string asset_symbol { get; set; }
        public string transaction_type { get; set; }
        public int amount { get; set; }
        public string status { get; set; }
        public double rate { get; set; }
        public double close_at_loss { get; set; }
        public double close_at_profit { get; set; }
    }
}
