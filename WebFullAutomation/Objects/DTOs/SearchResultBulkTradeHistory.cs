using Newtonsoft.Json;

namespace AirSoftAutomationFramework.Objects.DTOs
{
    public class SearchResultBulkTradeHistory
    {

        public BulkTradeHistory[] data { get; set; }
    }

    public class BulkTradeHistory
    {
        public string id { get; set; }
        public string exposure { get; set; }

        [JsonProperty("#trades")]
        public string numOfTradestrades { get; set; }
        public string asset { get; set; }
        public string equity { get; set; }

        [JsonProperty("Buy/Sell")]
        public string buysell { get; set; }

        [JsonProperty("Market/Limit")]
        public string marketlimit { get; set; }
        public string rate { get; set; }

        [JsonProperty("Stop Loss")]
        public string stoploss { get; set; }

        [JsonProperty("Take Profit")]
        public string takeprofit { get; set; }

        [JsonProperty("Open Trades")]
        public string opentrades { get; set; }

        [JsonProperty("Close Trades")]
        public string closetrades { get; set; }

        [JsonProperty("Pending Trades")]
        public string pendingtrades { get; set; }
    }
}

