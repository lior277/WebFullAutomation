using System;

namespace AirSoftAutomationFramework.Objects.DTOs
{
    public class UsersMongoTableDto
    {
        public string first_name { get; set; }
        public string last_name { get; set; }
        public string username { get; set; }
        public string phone { get; set; }
        public string country { get; set; }
        public string email { get; set; }
        public string password { get; set; }
        public string role { get; set; }
        public string[] allowed_ip_addresses { get; set; }
        public string office { get; set; }
        public bool affiliate { get; set; }
        public bool active { get; set; }
        public object[] sub_users { get; set; }
        public string gmt_timezone { get; set; }
        public string platform { get; set; }
        public DateTime created_at { get; set; }
        public object last_login { get; set; }
        public object last_logout { get; set; }
        public string lang { get; set; }
        public object[] allowed_fingerprints { get; set; }
        public int login_attempts { get; set; }
        public Pending_Fingerprints[] pending_fingerprints { get; set; }
        public object[] signature_reset_password { get; set; }
        public object[] erp_permissions { get; set; }
        public object sales_type { get; set; }
        public DateTime last_update_password { get; set; }
        public DateTime last_timeline_added { get; set; }
        public DateTime last_update { get; set; }
        public bool lock_side_bar { get; set; }
        public object mgm_role { get; set; }
        public object api_key { get; set; }
        public object api_key_generated_by_id { get; set; }
        public object api_key_generated_by_username { get; set; }
        public object api_key_generated_date { get; set; }
        public object last_api_token_set { get; set; }

        public class Pending_Fingerprints
        {
            public string fingerprint { get; set; }
            public DateTime expires { get; set; }
            public string signature { get; set; }
        }
    }
}
