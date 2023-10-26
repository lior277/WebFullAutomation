using Newtonsoft.Json;

namespace AirSoftAutomationFramework.Objects.DTOs
{
    public class GetSavingAccountResponse
    {
        [JsonProperty("data")]
        public SavingAccountData[] SavingAccountData { get; set; }
    }

    public class SavingAccountData
    {
        [JsonProperty("_id")]
        public string Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("platform_name")]
        public string PlatformName { get; set; }

        [JsonProperty("percent_per_month")]
        public long PercentPerMonth { get; set; }

        [JsonProperty("client_transfer")]
        public bool ClientTransfer { get; set; }

        [JsonProperty("daily")]
        public bool Daily { get; set; }

        [JsonProperty("default")]
        public bool Default { get; set; }

        [JsonProperty("revision", NullValueHandling = NullValueHandling.Ignore)]
        public long? Revision { get; set; }
    }
}
