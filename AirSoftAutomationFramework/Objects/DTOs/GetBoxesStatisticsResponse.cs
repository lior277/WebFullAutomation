namespace AirSoftAutomationFramework.Objects.DTOs
{
    public class GetBoxesStatisticsResponse
    {
        public double? eur_total_deposits { get; set; }
        public double? net_deposit { get; set; }
        public double? pending_total_withdrawals { get; set; }
        public double? approved_total_withdrawals { get; set; }
        public double? approved_total_other_withdrawals { get; set; }
        public double? total_chargeback { get; set; }
        public double? sales_performance { get; set; }
        public double? ftd { get; set; }
        public double? not_ftd { get; set; }
        public double? total_open_pnl { get; set; }
        public double? total_close_pnl { get; set; }
        public Top_Deposits[] top_deposits { get; set; }
        public Deposits_By_Currency[] deposits_by_currency { get; set; }
        public Top_Pnl[] top_pnl { get; set; }
        public int? total_users { get; set; }
        public int? online_users { get; set; }
    }

    public class Top_Deposits
    {
        public string user_id { get; set; }
        public string user_name { get; set; }
        public double? deposit_eur { get; set; }
    }

    public class Deposits_By_Currency
    {
        public double? amount { get; set; }
        public string deposit_currency { get; set; }
    }

    public class Top_Pnl
    {
        public double? pnl { get; set; }
        public string user_id { get; set; }
        public string user_name { get; set; }
    }
}
