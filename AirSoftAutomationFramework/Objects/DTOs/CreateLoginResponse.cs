using System.Collections.Generic;
using Newtonsoft.Json;

namespace AirSoftAutomationFramework.Objects.DTOs
{
    public class CreateLoginResponse
    {
        [JsonProperty("token_time_in_minutes")]
        public long TokenTimeInMinutes { get; set; }

        [JsonProperty("token")]
        public string Token { get; set; }

        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("role")]
        public string Role { get; set; }

        [JsonProperty("first_name")]
        public string FirstName { get; set; }

        [JsonProperty("last_name")]
        public string LastName { get; set; }

        [JsonProperty("platform")]
        public string Platform { get; set; }

        [JsonProperty("username")]
        public string Username { get; set; }

        [JsonProperty("lang")]
        public string Lang { get; set; }

        [JsonProperty("chat_enabled")]
        public bool ChatEnabled { get; set; }

        [JsonProperty("office_timezone")]
        public string OfficeTimezone { get; set; }

        [JsonProperty("gmt_timezone")]
        public string GmtTimezone { get; set; }

        [JsonProperty("erp_permissions")]
        public List<string> ErpPermissions { get; set; }

        [JsonProperty("notifications")]
        public List<string> Notifications { get; set; }

        [JsonProperty("type_affiliate")]
        public bool TypeAffiliate { get; set; }

        [JsonProperty("dialer_api")]
        public bool DialerApi { get; set; }

        [JsonProperty("redirectUrl")]
        public string RedirectUrl { get; set; }

        [JsonProperty("message")]
        public string Message { get; set; }

        [JsonProperty("email")]
        public string Email { get; set; }

        [JsonProperty("phone")]
        public string Phone { get; set; }

        [JsonProperty("shsec")]
        public string Shsec { get; set; }
    }
}
