

using Newtonsoft.Json;

namespace AirSoftAutomationFramework.Objects.DTOs
{
    public class GetSalesStatusResponse
    {
        public string _id { get; set; }

        [JsonProperty("Sales_Status_Text")]
        public SalesStatusText salesStatusText { get; set; }

        public class SalesStatusText
        {
            public string New { get; set; }

            [JsonProperty("Call Back")]
            public string CallBack { get; set; }
            public string Interested { get; set; }
            public string Deposit { get; set; }

            [JsonProperty("No Answer")]
            public string NoAnswer { get; set; }

            [JsonProperty("Wrong Number")]
            public string WrongNumber { get; set; }

            [JsonProperty("Voice Mail")]
            public string VoiceMail { get; set; }

            [JsonProperty("Not Interested")]
            public string NotInterested { get; set; }

            [JsonProperty("No Age")]
            public string NoAge { get; set; }

            [JsonProperty("No Language")]
            public string NoLanguage { get; set; }

            [JsonProperty("Hot Lead")]
            public string HotLead { get; set; }

            [JsonProperty("Double Phone Number")]
            public string DoublePhoneNumber { get; set; }
        }

    }
}
