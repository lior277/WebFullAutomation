namespace AirSoftAutomationFramework.Objects.DTOs
{
    public class GetSuspiciousProfit
    {
        public string _id { get; set; }
        public string[] admin_emails { get; set; }
        public bool alert { get; set; }
        public bool block_user { get; set; }
        public int percentage { get; set; }
        public int block_user_percentage { get; set; }
    }
}
