using System;

namespace AirSoftAutomationFramework.Objects.DTOs
{
    public class GetAllLeadsResponse
    {
        public Leads[] leads { get; set; }
    }

    public class Leads
    {
        public string country { get; set; }
        public string first_name { get; set; }
        public string last_name { get; set; }
        public string phone { get; set; }
        public string campaign_id { get; set; }
        public string client_id { get; set; }
        public string owner { get; set; }
        public string language_code { get; set; }
        public string status { get; set; }
        public bool is_lead { get; set; }
        public DateTime? registration_time { get; set; }
        public DateTime? last_date_connected { get; set; }
        public bool has_ftd { get; set; }
        public DateTime? modified_time { get; set; }
    }
}
