namespace AirSoftAutomationFramework.Objects.DTOs
{
    public class GetBrandsResponse
    {
        public string _id { get; set; }
        public string domain { get; set; }
        public string _namespace { get; set; }
        public string platform { get; set; }
        public string subdomain_erp { get; set; }
        public string subdomain_trade { get; set; }
        public bool is_dev { get; set; }
        public General_Info general_info { get; set; }
        public string group { get; set; }
        public string crmUrl { get; set; }

        public class General_Info
        {
            public Director director { get; set; }
            public Contact[] contacts { get; set; }
            public Company company { get; set; }
        }

        public class Director
        {
            public string first_name { get; set; }
            public string last_name { get; set; }
            public string passport { get; set; }
            public string email { get; set; }
            public string phone { get; set; }
        }

        public class Company
        {
            public string name { get; set; }
            public string registration_number { get; set; }
            public string[] address { get; set; }
            public string country { get; set; }
            public string account_manager { get; set; }
            public string note { get; set; }
        }

        public class Contact
        {
            public string first_name { get; set; }
            public string last_name { get; set; }
            public string email { get; set; }
            public string phone { get; set; }
        }

    }
}
