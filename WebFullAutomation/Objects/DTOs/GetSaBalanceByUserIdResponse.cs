using Newtonsoft.Json;

namespace AirSoftAutomationFramework.Objects.DTOs
{
    public class GetSaBalanceByUserIdResponse
    {
        [JsonProperty("sa_balance")]
        public long SaBalance { get; set; }

        [JsonProperty("available_deposit")]
        public double AvailableDeposit { get; set; }
    }
}
