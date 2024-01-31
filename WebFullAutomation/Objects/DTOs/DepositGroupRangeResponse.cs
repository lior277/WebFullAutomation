using Newtonsoft.Json;

namespace AirSoftAutomationFramework.Objects.DTOs
{
    public class DepositGroupRangeResponse
    {
        [JsonProperty("data")]
        public DepositGroupRange[] depositGroupRange { get; set; }
    }

    public class DepositGroupRange
    {
        public string _id { get; set; }
        public string name { get; set; }
        public int from_sum { get; set; }
        public int to_sum { get; set; }
        public int bonus_on_deposit { get; set; }
    }
}
