using System;
using Newtonsoft.Json;

namespace AirSoftAutomationFramework.Objects.DTOs
{
    public class GetPspInstanceResponse
    {
        [JsonProperty("_id")]
        public string Id { get; set; }

        [JsonProperty("psp_name")]
        public string PspName { get; set; }

        [JsonProperty("instances")]
        public Instances Instances { get; set; }

        [JsonProperty("last_update")]
        public DateTimeOffset LastUpdate { get; set; }
    }

    public partial class Instances
    {
        [JsonProperty("display_name")]
        public string DisplayName { get; set; }

        [JsonProperty("free_text")]
        public string FreeText { get; set; }

        [JsonProperty("logo")]
        public object Logo { get; set; }

        [JsonProperty("test_mode")]
        public bool TestMode { get; set; }

        [JsonProperty("hide_min_amount")]
        public bool HideMinAmount { get; set; }

        [JsonProperty("show_in_iframe")]
        public bool ShowInIframe { get; set; }

        [JsonProperty("have_dod")]
        public bool HaveDod { get; set; }

        [JsonProperty("subscription")]
        public bool Subscription { get; set; }

        [JsonProperty("integration_type")]
        public string IntegrationType { get; set; }

        [JsonProperty("countries")]
        public object[] Countries { get; set; }

        [JsonProperty("active")]
        public bool Active { get; set; }

        [JsonProperty("credentials")]
        public Credentials Credentials { get; set; }

        [JsonProperty("position")]
        public long Position { get; set; }

        [JsonProperty("currencies")]
        public object[] Currencies { get; set; }

        [JsonProperty("hasAdditionalDetails")]
        public bool HasAdditionalDetails { get; set; }

        [JsonProperty("_id")]
        public string Id { get; set; }
    }

    public partial class Credentials
    {
        [JsonProperty("private___apiKey")]
        public string PrivateApiKey { get; set; }

        [JsonProperty("private___tokenizationKey")]
        public string PrivateTokenizationKey { get; set; }

        [JsonProperty("selectedPSP")]
        public string SelectedPsp { get; set; }
    }
}

