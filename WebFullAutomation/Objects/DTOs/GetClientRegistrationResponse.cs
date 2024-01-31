using System;

namespace AirSoftAutomationFramework.Objects.DTOs
{
    public class GetClientRegistrationResponse
    {

        public string _id { get; set; }
        public string country { get; set; }
        public string currency_code { get; set; }
        public string email { get; set; }
        public string first_name { get; set; }
        public string last_name { get; set; }
        public string phone { get; set; }
        public string gmt_timezone { get; set; }
        public DateTime created_at { get; set; }
        public string sales_status { get; set; }
        public string sales_status2 { get; set; }
        public bool active { get; set; }
        public object attribution_date { get; set; }
        public int balance { get; set; }
        public string activation_status { get; set; }
        public string campaign_id { get; set; }
        public bool has_ftd { get; set; }
        public bool has_fto { get; set; }
    }
}
