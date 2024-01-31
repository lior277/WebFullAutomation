
using System;

namespace AirSoftAutomationFramework.Objects.DTOs
{
    public class GetCustomEmailsResponse
    {
        public string _id { get; set; }
        public string type { get; set; }
        public string name { get; set; }
        public string language { get; set; }
        public string subject { get; set; }
        public bool active { get; set; }
        public DateTime? last_update { get; set; }
        public bool can_deactivate { get; set; }
        public string body { get; set; }
        public string user_id { get; set; }
    }
}
