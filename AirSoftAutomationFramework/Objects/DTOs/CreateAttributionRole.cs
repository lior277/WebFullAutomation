namespace AirSoftAutomationFramework.Objects.DTOs
{
    public class CreateAttributionRole
    {
        public string name { get; set; }
        public string type { get; set; }
        public string country { get; set; }
        public string campaign_id { get; set; }
        public object[] ftd_agent_id { get; set; }
        public string[] retention_agent_id { get; set; }
        public string retention_type { get; set; }
    }
}
