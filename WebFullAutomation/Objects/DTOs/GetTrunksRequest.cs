using Newtonsoft.Json;

namespace AirSoftAutomationFramework.Objects.DTOs
{
    public class GetTrunksRequest
    {
        [JsonProperty("Class1")]
        public TrunkData[] trunkData { get; set; }

        public class TrunkData
        {
            public string _id { get; set; }
            public string office { get; set; }
            public string pbx_name { get; set; }
            public Trunk[] trunks { get; set; }
            public string city { get; set; }
        }

        public class Trunk
        {
            public string name { get; set; }
            public string number { get; set; }
        }

    }
}
