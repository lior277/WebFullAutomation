using Newtonsoft.Json;

namespace AirSoftAutomationFramework.Objects.DTOs
{
    public class ExportOpenTradesTablesResponse
    {
        [JsonProperty("ID")]
        public string id { get; set; }

        [JsonProperty("Client ID")]
        public string Client_ID { get; set; }

        [JsonProperty("Client Name")]
        public string Client_Name { get; set; }

        [JsonProperty("Sales Agent")]
        public string Sales_Agent { get; set; }

        [JsonProperty("Open Time | GMT")]
        public string Ope_Time_GMT { get; set; }

#pragma warning disable CA1507 // Use nameof to express symbol names
        [JsonProperty("Type")]
#pragma warning restore CA1507 // Use nameof to express symbol names
        public string Type { get; set; }

#pragma warning disable CA1507 // Use nameof to express symbol names
        [JsonProperty("Asset")]
#pragma warning restore CA1507 // Use nameof to express symbol names
        public string Asset { get; set; }

#pragma warning disable CA1507 // Use nameof to express symbol names
        [JsonProperty("Amount")]
#pragma warning restore CA1507 // Use nameof to express symbol names
        public string Amount { get; set; }

        [JsonProperty("Open Price")]
        public string Open_Price { get; set; }

        [JsonProperty("Required Margin")]
        public string Required_Margin { get; set; }

        [JsonProperty("Close Price")]
        public string Close_Price { get; set; }

        [JsonProperty("Close Time | GMT")]
        public string Close_Time_GMT { get; set; }

#pragma warning disable CA1507 // Use nameof to express symbol names
        [JsonProperty("PNL")]
#pragma warning restore CA1507 // Use nameof to express symbol names
        public string PNL { get; set; }

#pragma warning disable CA1507 // Use nameof to express symbol names
        [JsonProperty("SL")]
#pragma warning restore CA1507 // Use nameof to express symbol names
        public string SL { get; set; }

#pragma warning disable CA1507 // Use nameof to express symbol names
        [JsonProperty("TP")]
#pragma warning restore CA1507 // Use nameof to express symbol names
        public string TP { get; set; }

#pragma warning disable CA1507 // Use nameof to express symbol names
        [JsonProperty("Commission")]
#pragma warning restore CA1507 // Use nameof to express symbol names
        public string Commission { get; set; }

#pragma warning disable CA1507 // Use nameof to express symbol names
        [JsonProperty("Swap")]
#pragma warning restore CA1507 // Use nameof to express symbol names
        public string Swap { get; set; }       
    }
}
