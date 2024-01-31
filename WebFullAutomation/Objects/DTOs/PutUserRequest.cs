using Newtonsoft.Json;

namespace AirSoftAutomationFramework.Objects.DTOs
{
    public class PutUserRequest
    {
        [JsonProperty("first_name")]
        public string FirstName { get; set; }

        [JsonProperty("last_name")]
        public string LastName { get; set; }

        [JsonProperty("username")]
        public string Username { get; set; }

        [JsonProperty("email")]
        public string Email { get; set; }

        [JsonProperty("phone")]
        public string Phone { get; set; }

        public Extension[] extensions { get; set; }

        [JsonProperty("office")]
        public string Office { get; set; }

        [JsonProperty("allowed_ip_addresses")]
        public string[] AllowedIpAddresses { get; set; }

        [JsonProperty("country")]
        public string Country { get; set; }

        [JsonProperty("gmt_timezone")]
        public string GmtTimezone { get; set; }

        [JsonProperty("role")]
        public string Role { get; set; }

        [JsonProperty("salary_id")]
        public object SalaryId { get; set; }

        [JsonProperty("sub_users")]
        public object[] SubUsers { get; set; }

        [JsonProperty("sales_type")]
        public string SalesType { get; set; }
    }
}
