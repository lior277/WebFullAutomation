

namespace AirSoftAutomationFramework.Objects.DTOs
{
    public class GetRiskFilterResult
    {
        public Datum[] data { get; set; }
    }

    public class Datum
    {
        public string user_id { get; set; }
        public string currency { get; set; }
        public string full_name { get; set; }
        public string group_id { get; set; }
        public string total_deposit { get; set; }
        public string total_bonus { get; set; }
        public string balance { get; set; }
        public string closed_profit_loss { get; set; }
        public string open_profit_loss { get; set; }
        public string total_swap_commission { get; set; }
        public string available { get; set; }
        public string equity { get; set; }
        public string min_margin { get; set; }
        public int margin_usage { get; set; }
        public string erp_user_id { get; set; }
        public int count_deposits { get; set; }
        public string sales_agent_name { get; set; }
        public string group { get; set; }
        public string volume { get; set; }
    }
}
