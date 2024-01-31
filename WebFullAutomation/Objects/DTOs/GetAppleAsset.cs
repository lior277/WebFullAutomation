using System;
using Newtonsoft.Json;

namespace AirSoftAutomationFramework.Objects.DTOs
{
    public class GetAppleAsset
    {
        [JsonProperty("APPLE")]
        public AppleAsset appleAsset { get; set; }
    }

    public class AppleAsset
    {
        [JsonProperty("_id")]
        public string Id { get; set; }

        [JsonProperty("symbol")]
        public string Symbol { get; set; }

        [JsonProperty("label")]
        public string Label { get; set; }

        [JsonProperty("currency")]
        public string Currency { get; set; }

        [JsonProperty("decimal_digits")]
        public long DecimalDigits { get; set; }

        [JsonProperty("yahoo_symbol")]
        public string YahooSymbol { get; set; }

        [JsonProperty("icon_main")]
        public string IconMain { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("asset_is_future")]
        public bool AssetIsFuture { get; set; }

        [JsonProperty("category")]
        public string Category { get; set; }

        [JsonProperty("source")]
        public string Source { get; set; }

        [JsonProperty("times")]
        public Times Times { get; set; }

        [JsonProperty("dividend_date")]
        public object DividendDate { get; set; }

        [JsonProperty("expiry_date")]
        public object ExpiryDate { get; set; }

        [JsonProperty("icon_secondary")]
        public object IconSecondary { get; set; }

        [JsonProperty("roll_over_date")]
        public object RollOverDate { get; set; }

        [JsonProperty("market_data")]
        public MarketData MarketData { get; set; }

        [JsonProperty("menu_category")]
        public string MenuCategory { get; set; }

        [JsonProperty("menu_sub_category")]
        public string MenuSubCategory { get; set; }

        [JsonProperty("platform_attr")]
        public PlatformAttr PlatformAttr { get; set; }
    }

    public partial class MarketData
    {
        [JsonProperty("bid")]
        public double Bid { get; set; }

        [JsonProperty("ask")]
        public double Ask { get; set; }

        [JsonProperty("high")]
        public double High { get; set; }

        [JsonProperty("low")]
        public double Low { get; set; }

        [JsonProperty("close")]
        public double Close { get; set; }

        [JsonProperty("open")]
        public long Open { get; set; }

        [JsonProperty("last_price")]
        public double LastPrice { get; set; }

        [JsonProperty("change")]
        public long Change { get; set; }
    }

    public partial class PlatformAttr
    {
        [JsonProperty("commision")]
        public long Commision { get; set; }

        [JsonProperty("leverage")]
        public long Leverage { get; set; }

        [JsonProperty("maintenance")]
        public double Maintenance { get; set; }

        [JsonProperty("minimum_amount")]
        public long MinimumAmount { get; set; }

        [JsonProperty("minimum_step")]
        public long MinimumStep { get; set; }

        [JsonProperty("spread")]
        public double Spread { get; set; }

        [JsonProperty("swap_long")]
        public double SwapLong { get; set; }

        [JsonProperty("swap_short")]
        public double SwapShort { get; set; }

        [JsonProperty("swap_time")]
        public DateTimeOffset SwapTime { get; set; }

        [JsonProperty("margin_call")]
        public object MarginCall { get; set; }
    }

    public partial class Times
    {
        [JsonProperty("open")]
        public Open[] Open { get; set; }

        [JsonProperty("custom_open")]
        public CustomOpen[] CustomOpen { get; set; }
    }

    public partial class CustomOpen
    {
        [JsonProperty("from")]
        public TimeSpan From { get; set; }

        [JsonProperty("to")]
        public TimeSpan To { get; set; }

        [JsonProperty("date")]
        public DateTimeOffset Date { get; set; }
    }

    public partial class Open
    {
        [JsonProperty("from")]
        public TimeSpan From { get; set; }

        [JsonProperty("to")]
        public TimeSpan To { get; set; }
    }
}
