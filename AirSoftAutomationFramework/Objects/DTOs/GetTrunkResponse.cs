using System;
using Newtonsoft.Json;

namespace AirSoftAutomationFramework.Objects.DTOs
{
    public class GetTrunkResponse
    {
        [JsonProperty("_id")]
        public string Id { get; set; }

        [JsonProperty("office")]
        public string Office { get; set; }

        [JsonProperty("trunks")]
        public Trunk[] Trunks { get; set; }

        [JsonProperty("last_update")]
        public DateTimeOffset LastUpdate { get; set; }

        [JsonProperty("city")]
        public string City { get; set; }
    }

    public class Trunk
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("number")]
        public string Number { get; set; }
    }
}

