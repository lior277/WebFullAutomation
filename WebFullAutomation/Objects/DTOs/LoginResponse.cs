using System;

namespace AirSoftAutomationFramework.Objects.DTOs
{
    public class LoginResponse
    {
        public string token_time_in_minutes { get; set; }
        public string token { get; set; }
        public string id { get; set; }
        public string role { get; set; }
        public string first_name { get; set; }
        public string last_name { get; set; }
        public string platform { get; set; }
        public string lang { get; set; }
        public bool chat_enabled { get; set; }
        public string gmt_timezone { get; set; }
        public string sales_agent { get; set; }
        public string crypto_group_id { get; set; }
        public string activationStatus { get; set; }
        public string currency_code { get; set; }
        public string email { get; set; }
        public string phone { get; set; }
        public Platformgroup platformGroup { get; set; }
        public string shsec { get; set; }
    }

    public class Platformgroup
    {
        public string _id { get; set; }
        public Assets assets { get; set; }
        public string name { get; set; }
        public Default_Attr default_attr { get; set; }
        public Stk stk { get; set; }
        public Cash cash { get; set; }
        public Cmdty cmdty { get; set; }
        public Ind ind { get; set; }
        public bool _default { get; set; }
        public DateTime created_at { get; set; }
        public Forex forex { get; set; }
        public Commodities commodities { get; set; }
        public Stock stock { get; set; }
        public Crypto crypto { get; set; }
        public Futures futures { get; set; }
        public DateTime last_update { get; set; }
    }
}
