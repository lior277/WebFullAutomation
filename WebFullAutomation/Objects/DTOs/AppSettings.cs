


// Ignore Spelling: Api Mongo App Sql Crm

using Newtonsoft.Json;

namespace AirSoftAutomationFramework.Objects.DTOs
{
    public class AppSettings
    {
        //[JsonProperty("brandName")]
        //public string BrandName { get; set; }     

        //[JsonProperty("namespace")]
        //public string NameSpace { get; set; }

        //[JsonProperty("type")]
        //public string Type { get; set; }

        //[JsonProperty("clusterId")]
        //public string ClusterId { get; set; }

#pragma warning disable CA1507 // Use nameof to express symbol names
        [JsonProperty("tradingPlatformUrl")]
#pragma warning restore CA1507 // Use nameof to express symbol names
        public string tradingPlatformUrl { get; set; }

        [JsonProperty("crmUrl")]
        public string CrmUrl { get; set; }

        [JsonProperty("apiKey")]
        public string ApiKey { get; set; }

        //[JsonProperty("status")]
        //public string Status { get; set; }

        //[JsonProperty("isJira")]
        //public bool IsJira { get; set; }

        //[JsonProperty("dbName")]
        //public string DbName { get; set; }

        [JsonProperty("mongoDbConnectionString")]
        public string MongoConnectionString { get; set; }

        [JsonProperty("sqlDbConnectionString")]
        public string SqlConnectionString { get; set; }

        [JsonProperty("automationDbConnectionString")]
        public string AutomationDbConnectionString { get; set; }
    }
}
