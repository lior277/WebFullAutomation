using System.Collections.Generic;
using Newtonsoft.Json;

namespace AirSoftAutomationFramework.Objects.DTOs
{
    public class PatchBoostOptionRequest
    {
        [JsonProperty("boosts")]
        public long[] Boosts { get; set; }

        [JsonProperty("min_amounts")]
        public Dictionary<string, long> MinAmounts { get; set; }
    }
}
