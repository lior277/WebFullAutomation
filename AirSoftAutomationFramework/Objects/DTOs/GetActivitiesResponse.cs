using System;

namespace AirSoftAutomationFramework.Objects.DTOs
{
    public class GetActivitiesResponse
    {
        public string _id { get; set; }
        public DateTime created_at { get; set; }
        public string user_id { get; set; }
        public string erp_user_id { get; set; }
        public string erp_username { get; set; }
        public string new_status { get; set; }
        public string old_status { get; set; }
        public string old_trade_id { get; set; }
        public int new_trade_id { get; set; }
        public string user_full_name { get; set; }
        public string erp_user_id_assigned { get; set; }
        public string transaction_type { get; set; }
        public string status { get; set; }
        public int amount { get; set; }
        public string withdrawal_id { get; set; }
        public int bonus_amount { get; set; }
        public string currency { get; set; }
        public string original_currency { get; set; }
        public string trade_id { get; set; }
        public string transaction_id { get; set; }
        public string asset_symbol { get; set; }
        public int original_amount { get; set; }
        public double? current_rate { get; set; }
        public double? open_on_rate { get; set; }
        public string platform { get; set; }
        public string close_reason { get; set; }
        public string asset_label { get; set; }
        public string ip { get; set; }
        public string type { get; set; }
        public string action_made_by { get; set; }
        public string system_type { get; set; }
        public string action_made_by_user_id { get; set; }
        public string from_os { get; set; }
        public string from_device { get; set; }
        public string from_browser { get; set; }
    }
}
