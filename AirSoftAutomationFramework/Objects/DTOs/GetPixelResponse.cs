using Newtonsoft.Json;

namespace AirSoftAutomationFramework.Objects.DTOs
{
    public class GetPixelResponse
    {
        [JsonProperty("CODELANG")]
        public string Codelang { get; set; }

        [JsonProperty("COUNTRY")]
        public string Country { get; set; }

        [JsonProperty("ID")]
        public string Id { get; set; }

        [JsonProperty("MAIL")]
        public string Mail { get; set; }

        [JsonProperty("PHONE")]
        public string Phone { get; set; }

        [JsonProperty("referance_id")]
        public string ReferanceId { get; set; }

        [JsonProperty("DEPOSIT", NullValueHandling = NullValueHandling.Ignore)]
        public int Deposit { get; set; }
    }
}
