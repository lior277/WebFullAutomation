namespace AirSoftAutomationFramework.Objects.DTOs
{
    public class CreateAffiliateRequest
    {
        public string first_name { get; set; }
        public string last_name { get; set; }
        public string username { get; set; }
        public string email { get; set; }
        public string phone { get; set; }
        public string[] allowed_ip_addresses { get; set; }
        public string role { get; set; }
        public string affiliate_manager { get; set; }
        public string office { get; set; }
        public string password { get; set; }
    }
}
