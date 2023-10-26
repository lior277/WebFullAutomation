namespace AirSoftAutomationFramework.Objects.DTOs
{
    public class GetAttributionRoleResponse
    {
        public AttributionRoleResponse[] attributionRoleResponse { get; set; }
    }

    public class AttributionRoleResponse
    {
        public string _id { get; set; }
        public string name { get; set; }
        public string type { get; set; }
        public string[] campaign_id { get; set; }
        public string[] ftd_agent_id { get; set; }
        public string[] retention_agent_id { get; set; }
        public string retention_type { get; set; }
        public string[] country { get; set; }
    }
}
