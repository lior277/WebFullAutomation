using System;
using Newtonsoft.Json;

namespace AirSoftAutomationFramework.Objects.DTOs
{
    public class GetOrdersByClientIdResponse
    {
        public string _id { get; set; }
        public DateTime created_at { get; set; }

        [JsonProperty("billing")]
        public OrderBilling orderBilling { get; set; }

        [JsonProperty("shipping")]
        public OrderShipping orderShipping { get; set; }
        public double total_price { get; set; }
        public string status { get; set; }
    }

    public class OrderBilling
    {
        public string first_name { get; set; }
        public string last_name { get; set; }
    }

    public class OrderShipping
    {
        public string first_name { get; set; }
        public string last_name { get; set; }
    }
}
