using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace AirSoftAutomationFramework.Objects.DTOs
{
    public class GetChronoTabResponse
    {
        public Chrono chrono { get; set; }
    }

    public class Chrono
    {
        [JsonProperty("_id")]
        public string Id { get; set; }

        [JsonProperty("assets")]
        public string[] Assets { get; set; }

        [JsonProperty("boosts")]
        public long[] Boosts { get; set; }

        [JsonProperty("last_update")]
        public DateTimeOffset LastUpdate { get; set; }

        [JsonProperty("min_amounts")]
        public Dictionary<string, long> MinAmounts { get; set; }

        [JsonProperty("stop_loss_fee")]
        public long StopLossFee { get; set; }

        [JsonProperty("take_profit_fee")]
        public long TakeProfitFee { get; set; }

        [JsonProperty("time_periods")]
        public string[] TimePeriods { get; set; }
    }
}
