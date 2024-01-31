using static AirSoftAutomationFramework.Objects.DTOs.CreateOfficeRequest;

namespace AirSoftAutomationFramework.Objects.DTOs
{
    public class GetOfficeResponse
    {
        public string city { get; set; }
        public string _id { get; set; }
        public string[] allowed_ip_addresses { get; set; }
        public string gmt_timezone { get; set; }
        public Sales_Dashboard sales_dashboard { get; set; }
        public Dialer[] dialers { get; set; }
        public Working_Hours working_hours { get; set; }
    }
}

