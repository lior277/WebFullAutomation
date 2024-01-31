using System;

namespace AirSoftAutomationFramework.Objects.DTOs
{
    public class GetBlockedIpsResponse
    {
        public string _id { get; set; }
        public string ip { get; set; }
        public DateTime expires { get; set; }
        public DateTime last_update { get; set; }
        public int login_attempts { get; set; }
        public string[] username_or_email { get; set; }
        public string[] users { get; set; }
        public DateTime block_date { get; set; }
        public bool blocked { get; set; }
    }
}
