using System;

namespace AirSoftAutomationFramework.Objects.DTOs
{
    public class CloseTrade
    {
        public string id { get; set; }
        public string transaction_type { get; set; }
        public DateTime? trade_time_start { get; set; }
        public string asset_symbol { get; set; }
        public int amount { get; set; }
        public double rate { get; set; }
        public double current_rate { get; set; }
        public double? profit_loss { get; set; }
        public double close_at_profit { get; set; }
        public double close_at_loss { get; set; }
        public int commision { get; set; }
        public int swap_commission { get; set; }
        public string original_currency { get; set; }
        public string currency { get; set; }
        public string status { get; set; }
        public double? min_margin { get; set; }
        public double? investment { get; set; }
        public DateTime? trade_close_time { get; set; }
        public string close_reason { get; set; }
        public double? swap_short { get; set; }
        public double? swap_long { get; set; }
        public double? trade_volume { get; set; }
    }
}
