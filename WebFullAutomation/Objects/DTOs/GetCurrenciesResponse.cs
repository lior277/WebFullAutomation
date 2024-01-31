using System;
using Newtonsoft.Json;

namespace AirSoftAutomationFramework.Objects.DTOs
{
    public class GetCurrenciesResponse
    {
        [JsonProperty("_id")]
        public string Id { get; set; }

        [JsonProperty("currencies")]
        public Currencies Currencies { get; set; }

        [JsonProperty("last_update")]
        public DateTimeOffset LastUpdate { get; set; }
    }

    public class Currencies
    {
        [JsonProperty("USD")]
        public Eur Usd { get; set; }

        [JsonProperty("EUR")]
        public Eur Eur { get; set; }
    }

    public partial class Eur
    {
        [JsonProperty("max_initial_bonus")]
        public long MaxInitialBonus { get; set; }

        [JsonProperty("max_bonus_after_deposit")]
        public long MaxBonusAfterDeposit { get; set; }

        [JsonProperty("default")]
        public bool Default { get; set; }

        [JsonProperty("countries")]
        public object[] Countries { get; set; }
    }
}   
