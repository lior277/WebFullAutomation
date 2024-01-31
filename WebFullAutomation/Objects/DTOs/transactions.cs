using Newtonsoft.Json;
using System;

namespace AirSoftAutomationFramework.Objects.DTOs
{
    public class Transactions
    {

        [JsonProperty("transactions")]
        public Transaction[] transactionsList { get; set; }
        public bool withdrawal_title { get; set; }

        public class Transaction
        {
            public int id { get; set; }
            public int amount { get; set; }
            public string currency { get; set; }
            public int original_amount { get; set; }
            public string original_currency { get; set; }
            public string type { get; set; }
            public object title { get; set; }
            public DateTime created_at { get; set; }
            public string status { get; set; }
            public string method { get; set; }
            public string dod_url { get; set; }
            public string dod_id { get; set; }
            public string dod_type { get; set; }
        }

    }
}
