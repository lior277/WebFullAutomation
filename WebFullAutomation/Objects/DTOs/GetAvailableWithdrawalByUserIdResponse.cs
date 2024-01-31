using Newtonsoft.Json;

namespace AirSoftAutomationFramework.Objects.DTOs
{
    public class GetAvailableWithdrawalByUserIdResponse
    {
        [JsonProperty("user_id")]
        public string UserId { get; set; }

        [JsonProperty("available_withdrawal")]
        public double AvailableWithdrawal { get; set; }
    }
}
