
namespace AirSoftAutomationFramework.Objects.DTOs
{
    public class SiteTableDto
    {
        public string type { get; set; }
        public string name { get; set; }
        public string dbName { get; set; }
        public string _namespace { get; set; }
        public string clusterId { get; set; }
        public string tradingPlatformUrl { get; set; }
        public string crmUrl { get; set; }
        public string ApiKey { get; set; }
        public string status { get; set; }
        public bool isJira { get; set; }
        public string mongoPassword { get; set; }
        public string mongoUser { get; set; }
        public string sqlPassword { get; set; }
        public string sqlUser { get; set; }
    }
}
