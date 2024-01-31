namespace AirSoftAutomationFramework.Objects.DTOs
{
    public class CreateUserRequest
    {
        public string first_name { get; set; }
        public string last_name { get; set; }
        public string username { get; set; }
        public string phone { get; set; }
        public string country { get; set; }
        public string email { get; set; }
        public Extension[] extensions { get; set; }
        public string password { get; set; }
        public string role { get; set; }
        public string[] allowed_ip_addresses { get; set; }
        public string office { get; set; }
        public bool affiliate { get; set; }
        public bool active { get; set; }
        public bool add_as_child { get; set; }
        public string sales_type { get; set; }
        public object[] sub_users { get; set; }
        public string gmt_timezone { get; set; }
        public object salary_id { get; set; }
    }

    public class Extension
    {
        public string pbx_name { get; set; }
        public string ext_num { get; set; }
    }
}
