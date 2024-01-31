namespace AirSoftAutomationFramework.Objects.DTOs
{
    public class CreateClientWithCampaignRequest
    {
        public string country { get; set; }
        public string currency_code { get; set; }
        public string sub_campaign_key { get; set; }
        public string free_text { get; set; }
        public string free_text_2 { get; set; }
        public string free_text_3 { get; set; }
        public string free_text_4 { get; set; }
        public string free_text_5 { get; set; }
        public string email { get; set; }
        public string first_name { get; set; }
        public string last_name { get; set; }
        public string phone { get; set; }
        public string gmt_timezone { get; set; }
        public string password { get; set; }
        public string campaign_id { get; set; }
        public string note { get; set; }
    }
}
