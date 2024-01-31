using Newtonsoft.Json;

namespace AirSoftAutomationFramework.Objects.DTOs
{
    public class ImportClientRequest
    {
        [JsonProperty("Last Name")]
        public string LastName { get; set; }

        [JsonProperty("First name")]
        public string FirstName { get; set; }

        [JsonProperty("Phone number")]
        public string PhoneNumber { get; set; }

        [JsonProperty("E-Mail")]
        public string EMail { get; set; }

        [JsonProperty("Country/ISO Code")]
        public string CountryIsoCodeId { get; set; }

        [JsonProperty("Currency(optional)")]
        public string Currency { get; set; }

        [JsonProperty("Note(optional)")]
        public string Note { get; set; }
    }
}
