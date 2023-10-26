namespace AirSoftAutomationFramework.Objects.DTOs
{
    public class CreateClosedTradeResponse
    {
        public int page { get; set; }
        public int trades_amount { get; set; }
        public Trade[] trades { get; set; }
    }

    public class Trade
    {
        public string id { get; set; }
        public double rate { get; set; }
        public int chrono_trade { get; set; }
        public double current_rate { get; set; }
        public string sltp { get; set; }
#nullable enable
        public string? trade_time_start { get; set; }
#nullable disable
#nullable enable
        public string? trade_close_time { get; set; }
#nullable disable
        public int? sort_trade_time_start { get; set; }
        public int? sort_trade_close_time { get; set; }
        public double profit_loss { get; set; }
        public double swap_commission { get; set; }
        public double commision { get; set; }
        public string platform { get; set; }
        public string user_id { get; set; }
        public string transaction_type { get; set; }
        public string status { get; set; }
        public int amount { get; set; }
        public string asset_symbol { get; set; }
        public int demo { get; set; }
        public double? spread { get; set; }
        public double? spread_on_open { get; set; }
        public double? leverage { get; set; }
        public string currency { get; set; }
    }
}
