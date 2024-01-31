using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace AirSoftAutomationFramework.Objects.DTOs
{
    public class GetProductsResponse
    {
#pragma warning disable CA1507 // Use nameof to express symbol names
        [JsonProperty("data")]
#pragma warning restore CA1507 // Use nameof to express symbol names
        public List<ProductDetails> data { get; set; }
        public int recordsFiltered { get; set; }
        public int recordsTotal { get; set; }

        public class ProductDetails
        {
            public string _id { get; set; }
            public string global_product_id { get; set; }
            public string supplier_id { get; set; }
            public bool available { get; set; }
            public string category { get; set; }
            public DateTime created_at { get; set; }
            public int discount { get; set; }
            public bool enable { get; set; }
            public bool featured { get; set; }
            public double final_price { get; set; }
            public string image_1 { get; set; }
            public int margin { get; set; }
            public string name { get; set; }
            public double original_price { get; set; }
            public string supplier_name { get; set; }
        }
    }
}
