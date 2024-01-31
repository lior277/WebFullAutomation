using System;

namespace AirSoftAutomationFramework.Objects.DTOs
{
    public class GetAssestResponse
    {
        public string _id { get; set; }
        public string old_system_id { get; set; }
        public string symbol { get; set; }
        public string label { get; set; }
        public string currency { get; set; }
        public int decimal_digits { get; set; }
        public long ib_id { get; set; }
        public bool asset_is_contract { get; set; }
        public string ib_sec_type { get; set; }
        public string exchange { get; set; }
        public string yahoo_symbol { get; set; }
        public string icon_main { get; set; }
        public string description { get; set; }
        public string bloomberg_key { get; set; }
        public string asset_timeout_email { get; set; }
        public string asset_timeout_close { get; set; }
        public string asset_timeout_email_after_open { get; set; }
        public bool asset_is_future { get; set; }
        public string category { get; set; }
        public string source { get; set; }
        public DateTime last_update { get; set; }
    }
}
