using Newtonsoft.Json;

namespace AirSoftAutomationFramework.Objects.DTOs
{
    public class CreateRoleRequest
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("erp_permissions")]
        public string[] ErpPermissions { get; set; }

        [JsonProperty("notifications")]
        public string[] Notifications { get; set; }

        [JsonProperty("children")]
        public string[] Children { get; set; }

        [JsonProperty("show")]
        public bool Show { get; set; }

        [JsonProperty("users_only")]
        public bool UsersOnly { get; set; }

        [JsonProperty("affiliate")]
        public bool Affiliate { get; set; }

        [JsonProperty("dialer_api")]
        public bool DialerApi { get; set; }

        [JsonProperty("show_all_deposits")]
        public bool ShowAllDeposits { get; set; }

        [JsonProperty("show_deposits_amount")]
        public bool ShowDepositsAmount { get; set; }

        [JsonProperty("see_single_office")]
        public bool SeeSingleOffice { get; set; }

        [JsonProperty("show_attribution_date")]
        public bool ShowAttributionDate { get; set; }

        [JsonProperty("show_sales_status")]
        public bool ShowSalesStatus { get; set; }
    }
}

