using Newtonsoft.Json;

namespace AirSoftAutomationFramework.Objects.DTOs
{
    public class GetFiltersReponse
    {
        [JsonProperty("filters")]
        public Filters Filters { get; set; }
    }

    public partial class Filters
    {
        [JsonProperty("clients")]
        public Clients[] Clients { get; set; }
    }

    public class Clients
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("default")]
        public bool Default { get; set; }

        [JsonProperty("time_filter")]
        public string TimeFilter { get; set; }

        [JsonProperty("period_range")]
        public object PeriodRange { get; set; }

        [JsonProperty("data")]
        public ClientsData[] ClientsData { get; set; }
    }

    public class ClientsData
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("itemName")]
        public string ItemName { get; set; }

        [JsonProperty("label")]
        public string Label { get; set; }

        [JsonProperty("inputName")]
        public string InputName { get; set; }
    }
}

