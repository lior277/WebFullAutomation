using System;

namespace AirSoftAutomationFramework.Objects.DTOs
{
    public class GetUserResponse
    {
        public UsersApi user { get; set; }
        public Fields fields { get; set; }
    }

    public class UsersApi
    {
        public string _id { get; set; }
        public string first_name { get; set; }
        public string last_name { get; set; }
        public string username { get; set; }
        public string phone { get; set; }
        public object last_login { get; set; }
        public Extension[] extensions { get; set; }
        public string country { get; set; }
        public string email { get; set; }
        public string role { get; set; }
        public string[] allowed_ip_addresses { get; set; }
        public string office { get; set; }
        public bool affiliate { get; set; }
        public bool active { get; set; }
        public string sales_type { get; set; }
        public object[] sub_users { get; set; }
        public string gmt_timezone { get; set; }
        public object salary_id { get; set; }
        public DateTime last_update_password { get; set; }
        public string[] allowed_fingerprints { get; set; }
    }

    public class Fields
    {
        public string[] roles { get; set; }
        public Subuser[] subUsers { get; set; }
        public Country[] countries { get; set; }
        public Currencies currencies { get; set; }
        public object[] salaries { get; set; }
    }

    public class Subuser
    {
        public string _id { get; set; }
        public string first_name { get; set; }
        public string last_name { get; set; }
    }

    public class Country
    {
        public string name { get; set; }
        public string[] timezones { get; set; }
        public string iso2 { get; set; }
    }
}
