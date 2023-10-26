using System;

namespace AirSoftAutomationFramework.Objects.DTOs
{
    public class GetUserLastLogin
    {
        public string _id { get; set; }
        public DateTime created_at { get; set; }
        public string user_id { get; set; }
        public string platform { get; set; }
        public string user_full_name { get; set; }
        public DateTime logout_date { get; set; }
        public string ip { get; set; }
        public string erp_user_id { get; set; }
        public string erp_username { get; set; }
        public string type { get; set; }
        public string from_os { get; set; }
        public string from_device { get; set; }
        public string from_browser { get; set; }
    }
}
