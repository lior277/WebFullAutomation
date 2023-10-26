using Newtonsoft.Json;

namespace AirSoftAutomationFramework.Objects.DTOs
{
    public class PostFilterRequest
    {
        public string name { get; set; }
        public bool @default { get; set; }
        public string time_filter { get; set; }
        public string period_range { get; set; }

        [JsonProperty("filters")]
        public FilterBody[] filterBody { get; set; }
    }

    public class FilterBody
    {
        public string id { get; set; }
        public string itemName { get; set; }
        public string label { get; set; }
        public string inputName { get; set; }
    }
}
