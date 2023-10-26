using Newtonsoft.Json;

namespace AirSoftAutomationFramework.Objects.DTOs
{
    public class GetCryptoVsUsdtResponse
    {
        public ADAUSD ADAUSD { get; set; }
        public BCHUSDT BCHUSDT { get; set; }
        public BTCUSDT BTCUSDT { get; set; }
        public BTGUSDT BTGUSDT { get; set; }
        public DASHUSDT DASHUSDT { get; set; }
        public EOSUSDT EOSUSDT { get; set; }
        public ETHUSDT ETHUSDT { get; set; }
        public ETCUSDT ETCUSDT { get; set; }
        public IOTUSDT IOTUSDT { get; set; }
        public LTCUSDT LTCUSDT { get; set; }
        public NEOUSDT NEOUSDT { get; set; }
        public QTUMUSDT QTUMUSDT { get; set; }
        public TRXUSDT TRXUSDT { get; set; }
        public XLMUSDT XLMUSDT { get; set; }
        public XMRUSDT XMRUSDT { get; set; }
        public XRPUSDT XRPUSDT { get; set; }
        public ZECUSDT ZECUSDT { get; set; }
    }

    public class ADAUSD
    {
        public string _id { get; set; }
        public string symbol { get; set; }
        public string label { get; set; }
        public string currency { get; set; }
        public double decimal_digits { get; set; }
        public string yahoo_symbol { get; set; }
        public string description { get; set; }
        public bool asset_is_future { get; set; }
        public string category { get; set; }
        public string source { get; set; }
#pragma warning disable CA1507 // Use nameof to express symbol names
        [JsonProperty("times")]
#pragma warning restore CA1507 // Use nameof to express symbol names
        public TimesAllAssets times { get; set; }
        public string icon_secondary { get; set; }
        public string icon_main { get; set; }
        public object dividend_date { get; set; }
        public object expiry_date { get; set; }
        public object roll_over_date { get; set; }
        public string menu_category { get; set; }
        public object menu_sub_category { get; set; }
        public Market_Data market_data { get; set; }
        public Platform_Attr platform_attr { get; set; }
        public string original_currency { get; set; }
    }

    public class TimesAllAssets
    {
#pragma warning disable CA1507 // Use nameof to express symbol names
        [JsonProperty("open")]
#pragma warning restore CA1507 // Use nameof to express symbol names
        public OpenAllAssets[] open { get; set; }
    }

    public class OpenAllAssets
    {
        public string from { get; set; }
        public string to { get; set; }
    }

    public class Market_Data
    {
        public double bid { get; set; }
        public double ask { get; set; }
        public double low { get; set; }
        public double high { get; set; }
        public double open { get; set; }
        public double close { get; set; }
        public string time { get; set; }
        public double last_price { get; set; }
    }

    public class Platform_Attr
    {
        public double commision { get; set; }
        public string swap_time { get; set; }
        public double swap_long { get; set; }
        public double swap_short { get; set; }
        public double spread { get; set; }
        public double minimum_step { get; set; }
        public double minimum_amount { get; set; }
        public double leverage { get; set; }
        public double madoubleenance { get; set; }
        public object margin_call { get; set; }
    }

    public class BCHUSDT
    {
        public string _id { get; set; }
        public string symbol { get; set; }
        public string label { get; set; }
        public string currency { get; set; }
        public double decimal_digits { get; set; }
        public string yahoo_symbol { get; set; }
        public string description { get; set; }
        public bool asset_is_future { get; set; }
        public string category { get; set; }
        public string source { get; set; }
        public Times1 times { get; set; }
        public string icon_secondary { get; set; }
        public string icon_main { get; set; }
        public string menu_category { get; set; }
        public object menu_sub_category { get; set; }
        public Market_Data1 market_data { get; set; }
        public Platform_Attr1 platform_attr { get; set; }
        public string original_currency { get; set; }
    }

    public class Times1
    {
        public Open1[] open { get; set; }
    }

    public class Open1
    {
        public string from { get; set; }
        public string to { get; set; }
    }

    public class Market_Data1
    {
        public double bid { get; set; }
        public double ask { get; set; }
        public double low { get; set; }
        public double high { get; set; }
        public double open { get; set; }
        public double close { get; set; }
        public string time { get; set; }
        public double last_price { get; set; }
    }

    public class Platform_Attr1
    {
        public double commision { get; set; }
        public string swap_time { get; set; }
        public double swap_long { get; set; }
        public double swap_short { get; set; }
        public double spread { get; set; }
        public double minimum_step { get; set; }
        public double minimum_amount { get; set; }
        public double leverage { get; set; }
        public double madoubleenance { get; set; }
        public object margin_call { get; set; }
    }

    public class BTCUSDT
    {
        public string _id { get; set; }
        public string symbol { get; set; }
        public string label { get; set; }
        public string currency { get; set; }
        public double decimal_digits { get; set; }
        public string yahoo_symbol { get; set; }
        public string description { get; set; }
        public bool asset_is_future { get; set; }
        public string category { get; set; }
        public string source { get; set; }
        public Times2 times { get; set; }
        public string icon_secondary { get; set; }
        public string icon_main { get; set; }
        public string menu_category { get; set; }
        public object menu_sub_category { get; set; }
        public Market_Data2 market_data { get; set; }
        public Platform_Attr2 platform_attr { get; set; }
        public string original_currency { get; set; }
    }

    public class Times2
    {
        public Open2[] open { get; set; }
    }

    public class Open2
    {
        public string from { get; set; }
        public string to { get; set; }
    }

    public class Market_Data2
    {
        public double bid { get; set; }
        public double ask { get; set; }
        public double low { get; set; }
        public double high { get; set; }
        public double open { get; set; }
        public double close { get; set; }
        public string time { get; set; }
        public double last_price { get; set; }
    }

    public class Platform_Attr2
    {
        public double commision { get; set; }
        public string swap_time { get; set; }
        public double swap_long { get; set; }
        public double swap_short { get; set; }
        public double spread { get; set; }
        public double minimum_step { get; set; }
        public double minimum_amount { get; set; }
        public double leverage { get; set; }
        public double madoubleenance { get; set; }
        public object margin_call { get; set; }
    }

    public class BTGUSDT
    {
        public string _id { get; set; }
        public string symbol { get; set; }
        public string label { get; set; }
        public string currency { get; set; }
        public double decimal_digits { get; set; }
        public string yahoo_symbol { get; set; }
        public string description { get; set; }
        public bool asset_is_future { get; set; }
        public string category { get; set; }
        public string source { get; set; }
        public Times3 times { get; set; }
        public string icon_secondary { get; set; }
        public string icon_main { get; set; }
        public string menu_category { get; set; }
        public object menu_sub_category { get; set; }
        public Market_Data3 market_data { get; set; }
        public Platform_Attr3 platform_attr { get; set; }
        public string original_currency { get; set; }
    }

    public class Times3
    {
        public Open3[] open { get; set; }
    }

    public class Open3
    {
        public string from { get; set; }
        public string to { get; set; }
    }

    public class Market_Data3
    {
        public double bid { get; set; }
        public double ask { get; set; }
        public double low { get; set; }
        public double high { get; set; }
        public double open { get; set; }
        public double close { get; set; }
        public string time { get; set; }
        public double last_price { get; set; }
    }

    public class Platform_Attr3
    {
        public double commision { get; set; }
        public string swap_time { get; set; }
        public double swap_long { get; set; }
        public double swap_short { get; set; }
        public double spread { get; set; }
        public double minimum_step { get; set; }
        public double minimum_amount { get; set; }
        public double leverage { get; set; }
        public double madoubleenance { get; set; }
        public object margin_call { get; set; }
    }

    public class DASHUSDT
    {
        public string _id { get; set; }
        public string symbol { get; set; }
        public string label { get; set; }
        public string currency { get; set; }
        public double decimal_digits { get; set; }
        public string yahoo_symbol { get; set; }
        public string description { get; set; }
        public bool asset_is_future { get; set; }
        public string category { get; set; }
        public string source { get; set; }
        public Times4 times { get; set; }
        public string icon_secondary { get; set; }
        public string icon_main { get; set; }
        public string menu_category { get; set; }
        public object menu_sub_category { get; set; }
        public Market_Data4 market_data { get; set; }
        public Platform_Attr4 platform_attr { get; set; }
        public string original_currency { get; set; }
    }

    public class Times4
    {
        public Open4[] open { get; set; }
    }

    public class Open4
    {
        public string from { get; set; }
        public string to { get; set; }
    }

    public class Market_Data4
    {
        public double bid { get; set; }
        public double ask { get; set; }
        public double low { get; set; }
        public double high { get; set; }
        public double open { get; set; }
        public double close { get; set; }
        public string time { get; set; }
        public double last_price { get; set; }
    }

    public class Platform_Attr4
    {
        public double commision { get; set; }
        public string swap_time { get; set; }
        public double swap_long { get; set; }
        public double swap_short { get; set; }
        public double spread { get; set; }
        public double minimum_step { get; set; }
        public double minimum_amount { get; set; }
        public double leverage { get; set; }
        public double madoubleenance { get; set; }
        public object margin_call { get; set; }
    }

    public class EOSUSDT
    {
        public string _id { get; set; }
        public string symbol { get; set; }
        public string label { get; set; }
        public string currency { get; set; }
        public double decimal_digits { get; set; }
        public string yahoo_symbol { get; set; }
        public string description { get; set; }
        public bool asset_is_future { get; set; }
        public string category { get; set; }
        public string source { get; set; }
        public Times5 times { get; set; }
        public string icon_secondary { get; set; }
        public string icon_main { get; set; }
        public string menu_category { get; set; }
        public object menu_sub_category { get; set; }
        public Market_Data5 market_data { get; set; }
        public Platform_Attr5 platform_attr { get; set; }
        public string original_currency { get; set; }
    }

    public class Times5
    {
        public Open5[] open { get; set; }
    }

    public class Open5
    {
        public string from { get; set; }
        public string to { get; set; }
    }

    public class Market_Data5
    {
        public double bid { get; set; }
        public double ask { get; set; }
        public double low { get; set; }
        public double high { get; set; }
        public double open { get; set; }
        public double close { get; set; }
        public string time { get; set; }
        public double last_price { get; set; }
    }

    public class Platform_Attr5
    {
        public double commision { get; set; }
        public string swap_time { get; set; }
        public double swap_long { get; set; }
        public double swap_short { get; set; }
        public double spread { get; set; }
        public double minimum_step { get; set; }
        public double minimum_amount { get; set; }
        public double leverage { get; set; }
        public double madoubleenance { get; set; }
        public object margin_call { get; set; }
    }

    public class ETHUSDT
    {
        public string _id { get; set; }
        public string symbol { get; set; }
        public string label { get; set; }
        public string currency { get; set; }
        public double decimal_digits { get; set; }
        public string yahoo_symbol { get; set; }
        public string description { get; set; }
        public bool asset_is_future { get; set; }
        public string category { get; set; }
        public string source { get; set; }
        public Times6 times { get; set; }
        public string icon_secondary { get; set; }
        public string icon_main { get; set; }
        public string menu_category { get; set; }
        public object menu_sub_category { get; set; }
        public Market_Data6 market_data { get; set; }
        public Platform_Attr6 platform_attr { get; set; }
        public string original_currency { get; set; }
    }

    public class Times6
    {
        public Open6[] open { get; set; }
    }

    public class Open6
    {
        public string from { get; set; }
        public string to { get; set; }
    }

    public class Market_Data6
    {
        public double bid { get; set; }
        public double ask { get; set; }
        public double low { get; set; }
        public double high { get; set; }
        public double open { get; set; }
        public double close { get; set; }
        public string time { get; set; }
        public double last_price { get; set; }
    }

    public class Platform_Attr6
    {
        public double commision { get; set; }
        public string swap_time { get; set; }
        public double swap_long { get; set; }
        public double swap_short { get; set; }
        public double spread { get; set; }
        public double minimum_step { get; set; }
        public double minimum_amount { get; set; }
        public double leverage { get; set; }
        public double madoubleenance { get; set; }
        public object margin_call { get; set; }
    }

    public class ETCUSDT
    {
        public string _id { get; set; }
        public string symbol { get; set; }
        public string label { get; set; }
        public string currency { get; set; }
        public double decimal_digits { get; set; }
        public string yahoo_symbol { get; set; }
        public string description { get; set; }
        public bool asset_is_future { get; set; }
        public string category { get; set; }
        public string source { get; set; }
        public Times7 times { get; set; }
        public string icon_secondary { get; set; }
        public string icon_main { get; set; }
        public object dividend_date { get; set; }
        public object expiry_date { get; set; }
        public object roll_over_date { get; set; }
        public string menu_category { get; set; }
        public object menu_sub_category { get; set; }
        public Market_Data7 market_data { get; set; }
        public Platform_Attr7 platform_attr { get; set; }
        public string original_currency { get; set; }
    }

    public class Times7
    {
        public Open7[] open { get; set; }
    }

    public class Open7
    {
        public string from { get; set; }
        public string to { get; set; }
    }

    public class Market_Data7
    {
        public double bid { get; set; }
        public double ask { get; set; }
        public double low { get; set; }
        public double high { get; set; }
        public double open { get; set; }
        public double close { get; set; }
        public string time { get; set; }
        public double last_price { get; set; }
    }

    public class Platform_Attr7
    {
        public double commision { get; set; }
        public string swap_time { get; set; }
        public double swap_long { get; set; }
        public double swap_short { get; set; }
        public double spread { get; set; }
        public double minimum_step { get; set; }
        public double minimum_amount { get; set; }
        public double leverage { get; set; }
        public double madoubleenance { get; set; }
        public object margin_call { get; set; }
    }

    public class IOTUSDT
    {
        public string _id { get; set; }
        public string symbol { get; set; }
        public string label { get; set; }
        public string currency { get; set; }
        public double decimal_digits { get; set; }
        public string yahoo_symbol { get; set; }
        public string description { get; set; }
        public bool asset_is_future { get; set; }
        public string category { get; set; }
        public string source { get; set; }
        public Times8 times { get; set; }
        public string icon_secondary { get; set; }
        public string icon_main { get; set; }
        public object dividend_date { get; set; }
        public object expiry_date { get; set; }
        public object roll_over_date { get; set; }
        public string menu_category { get; set; }
        public object menu_sub_category { get; set; }
        public Market_Data8 market_data { get; set; }
        public Platform_Attr8 platform_attr { get; set; }
        public string original_currency { get; set; }
    }

    public class Times8
    {
        public Open8[] open { get; set; }
    }

    public class Open8
    {
        public string from { get; set; }
        public string to { get; set; }
    }

    public class Market_Data8
    {
        public double bid { get; set; }
        public double ask { get; set; }
        public double low { get; set; }
        public double high { get; set; }
        public double open { get; set; }
        public double close { get; set; }
        public string time { get; set; }
        public double last_price { get; set; }
    }

    public class Platform_Attr8
    {
        public double commision { get; set; }
        public string swap_time { get; set; }
        public double swap_long { get; set; }
        public double swap_short { get; set; }
        public double spread { get; set; }
        public double minimum_step { get; set; }
        public double minimum_amount { get; set; }
        public double leverage { get; set; }
        public double madoubleenance { get; set; }
        public object margin_call { get; set; }
    }

    public class LTCUSDT
    {
        public string _id { get; set; }
        public string symbol { get; set; }
        public string label { get; set; }
        public string currency { get; set; }
        public double decimal_digits { get; set; }
        public string yahoo_symbol { get; set; }
        public string description { get; set; }
        public bool asset_is_future { get; set; }
        public string category { get; set; }
        public string source { get; set; }
        public Times9 times { get; set; }
        public string icon_secondary { get; set; }
        public string icon_main { get; set; }
        public string menu_category { get; set; }
        public object menu_sub_category { get; set; }
        public Market_Data9 market_data { get; set; }
        public Platform_Attr9 platform_attr { get; set; }
        public string original_currency { get; set; }
    }

    public class Times9
    {
        public Open9[] open { get; set; }
    }

    public class Open9
    {
        public string from { get; set; }
        public string to { get; set; }
    }

    public class Market_Data9
    {
        public double bid { get; set; }
        public double ask { get; set; }
        public double low { get; set; }
        public double high { get; set; }
        public double open { get; set; }
        public double close { get; set; }
        public string time { get; set; }
        public double last_price { get; set; }
    }

    public class Platform_Attr9
    {
        public double commision { get; set; }
        public string swap_time { get; set; }
        public double swap_long { get; set; }
        public double swap_short { get; set; }
        public double spread { get; set; }
        public double minimum_step { get; set; }
        public double minimum_amount { get; set; }
        public double leverage { get; set; }
        public double madoubleenance { get; set; }
        public object margin_call { get; set; }
    }

    public class NEOUSDT
    {
        public string _id { get; set; }
        public string symbol { get; set; }
        public string label { get; set; }
        public string currency { get; set; }
        public double decimal_digits { get; set; }
        public string yahoo_symbol { get; set; }
        public string description { get; set; }
        public bool asset_is_future { get; set; }
        public string category { get; set; }
        public string source { get; set; }
        public Times10 times { get; set; }
        public string icon_secondary { get; set; }
        public string icon_main { get; set; }
        public string menu_category { get; set; }
        public object menu_sub_category { get; set; }
        public Market_Data10 market_data { get; set; }
        public Platform_Attr10 platform_attr { get; set; }
        public string original_currency { get; set; }
    }

    public class Times10
    {
        public Open10[] open { get; set; }
    }

    public class Open10
    {
        public string from { get; set; }
        public string to { get; set; }
    }

    public class Market_Data10
    {
        public double bid { get; set; }
        public double ask { get; set; }
        public double low { get; set; }
        public double high { get; set; }
        public double open { get; set; }
        public double close { get; set; }
        public string time { get; set; }
        public double last_price { get; set; }
    }

    public class Platform_Attr10
    {
        public double commision { get; set; }
        public string swap_time { get; set; }
        public double swap_long { get; set; }
        public double swap_short { get; set; }
        public double spread { get; set; }
        public double minimum_step { get; set; }
        public double minimum_amount { get; set; }
        public double leverage { get; set; }
        public double madoubleenance { get; set; }
        public object margin_call { get; set; }
    }

    public class QTUMUSDT
    {
        public string _id { get; set; }
        public string symbol { get; set; }
        public string label { get; set; }
        public string currency { get; set; }
        public double decimal_digits { get; set; }
        public string yahoo_symbol { get; set; }
        public string description { get; set; }
        public bool asset_is_future { get; set; }
        public string category { get; set; }
        public string source { get; set; }
        public Times11 times { get; set; }
        public string icon_secondary { get; set; }
        public string icon_main { get; set; }
        public object dividend_date { get; set; }
        public object expiry_date { get; set; }
        public object roll_over_date { get; set; }
        public string menu_category { get; set; }
        public object menu_sub_category { get; set; }
        public Market_Data11 market_data { get; set; }
        public Platform_Attr11 platform_attr { get; set; }
        public string original_currency { get; set; }
    }

    public class Times11
    {
        public Open11[] open { get; set; }
    }

    public class Open11
    {
        public string from { get; set; }
        public string to { get; set; }
    }

    public class Market_Data11
    {
        public double bid { get; set; }
        public double ask { get; set; }
        public double low { get; set; }
        public double high { get; set; }
        public double open { get; set; }
        public double close { get; set; }
        public string time { get; set; }
        public double last_price { get; set; }
    }

    public class Platform_Attr11
    {
        public double commision { get; set; }
        public string swap_time { get; set; }
        public double swap_long { get; set; }
        public double swap_short { get; set; }
        public double spread { get; set; }
        public double minimum_step { get; set; }
        public double minimum_amount { get; set; }
        public double leverage { get; set; }
        public double madoubleenance { get; set; }
        public object margin_call { get; set; }
    }

    public class TRXUSDT
    {
        public string _id { get; set; }
        public string symbol { get; set; }
        public string label { get; set; }
        public string currency { get; set; }
        public double decimal_digits { get; set; }
        public string yahoo_symbol { get; set; }
        public string description { get; set; }
        public bool asset_is_future { get; set; }
        public string category { get; set; }
        public string source { get; set; }
        public Times12 times { get; set; }
        public string icon_secondary { get; set; }
        public string icon_main { get; set; }
        public object dividend_date { get; set; }
        public object expiry_date { get; set; }
        public object roll_over_date { get; set; }
        public string menu_category { get; set; }
        public object menu_sub_category { get; set; }
        public Market_Data12 market_data { get; set; }
        public Platform_Attr12 platform_attr { get; set; }
        public string original_currency { get; set; }
    }

    public class Times12
    {
        public Open12[] open { get; set; }
    }

    public class Open12
    {
        public string from { get; set; }
        public string to { get; set; }
    }

    public class Market_Data12
    {
        public double bid { get; set; }
        public double ask { get; set; }
        public double low { get; set; }
        public double high { get; set; }
        public double open { get; set; }
        public double close { get; set; }
        public string time { get; set; }
        public double last_price { get; set; }
    }

    public class Platform_Attr12
    {
        public double commision { get; set; }
        public string swap_time { get; set; }
        public double swap_long { get; set; }
        public double swap_short { get; set; }
        public double spread { get; set; }
        public double minimum_step { get; set; }
        public double minimum_amount { get; set; }
        public double leverage { get; set; }
        public double madoubleenance { get; set; }
        public object margin_call { get; set; }
    }

    public class XLMUSDT
    {
        public string _id { get; set; }
        public string symbol { get; set; }
        public string label { get; set; }
        public string currency { get; set; }
        public double decimal_digits { get; set; }
        public string yahoo_symbol { get; set; }
        public string description { get; set; }
        public bool asset_is_future { get; set; }
        public string category { get; set; }
        public string source { get; set; }
        public Times13 times { get; set; }
        public string icon_secondary { get; set; }
        public string icon_main { get; set; }
        public string menu_category { get; set; }
        public object menu_sub_category { get; set; }
        public Market_Data13 market_data { get; set; }
        public Platform_Attr13 platform_attr { get; set; }
        public string original_currency { get; set; }
    }

    public class Times13
    {
        public Open13[] open { get; set; }
    }

    public class Open13
    {
        public string from { get; set; }
        public string to { get; set; }
    }

    public class Market_Data13
    {
        public double bid { get; set; }
        public double ask { get; set; }
        public double low { get; set; }
        public double high { get; set; }
        public double open { get; set; }
        public double close { get; set; }
        public string time { get; set; }
        public double last_price { get; set; }
    }

    public class Platform_Attr13
    {
        public double commision { get; set; }
        public string swap_time { get; set; }
        public double swap_long { get; set; }
        public double swap_short { get; set; }
        public double spread { get; set; }
        public double minimum_step { get; set; }
        public double minimum_amount { get; set; }
        public double leverage { get; set; }
        public double madoubleenance { get; set; }
        public object margin_call { get; set; }
    }

    public class XMRUSDT
    {
        public string _id { get; set; }
        public string symbol { get; set; }
        public string label { get; set; }
        public string currency { get; set; }
        public double decimal_digits { get; set; }
        public string yahoo_symbol { get; set; }
        public string description { get; set; }
        public bool asset_is_future { get; set; }
        public string category { get; set; }
        public string source { get; set; }
        public Times14 times { get; set; }
        public string icon_secondary { get; set; }
        public string icon_main { get; set; }
        public string menu_category { get; set; }
        public object menu_sub_category { get; set; }
        public Market_Data14 market_data { get; set; }
        public Platform_Attr14 platform_attr { get; set; }
        public string original_currency { get; set; }
    }

    public class Times14
    {
        public Open14[] open { get; set; }
    }

    public class Open14
    {
        public string from { get; set; }
        public string to { get; set; }
    }

    public class Market_Data14
    {
        public double bid { get; set; }
        public double ask { get; set; }
        public double low { get; set; }
        public double high { get; set; }
        public double open { get; set; }
        public double close { get; set; }
        public string time { get; set; }
        public double last_price { get; set; }
    }

    public class Platform_Attr14
    {
        public double commision { get; set; }
        public string swap_time { get; set; }
        public double swap_long { get; set; }
        public double swap_short { get; set; }
        public double spread { get; set; }
        public double minimum_step { get; set; }
        public double minimum_amount { get; set; }
        public double leverage { get; set; }
        public double madoubleenance { get; set; }
        public object margin_call { get; set; }
    }

    public class XRPUSDT
    {
        public string _id { get; set; }
        public string symbol { get; set; }
        public string label { get; set; }
        public string currency { get; set; }
        public double decimal_digits { get; set; }
        public string yahoo_symbol { get; set; }
        public string description { get; set; }
        public bool asset_is_future { get; set; }
        public string category { get; set; }
        public string source { get; set; }
        public Times15 times { get; set; }
        public string icon_secondary { get; set; }
        public string icon_main { get; set; }
        public string menu_category { get; set; }
        public object menu_sub_category { get; set; }
        public Market_Data15 market_data { get; set; }
        public Platform_Attr15 platform_attr { get; set; }
        public string original_currency { get; set; }
    }

    public class Times15
    {
        public Open15[] open { get; set; }
    }

    public class Open15
    {
        public string from { get; set; }
        public string to { get; set; }
    }

    public class Market_Data15
    {
        public double bid { get; set; }
        public double ask { get; set; }
        public double low { get; set; }
        public double high { get; set; }
        public double open { get; set; }
        public double close { get; set; }
        public string time { get; set; }
        public double last_price { get; set; }
    }

    public class Platform_Attr15
    {
        public double commision { get; set; }
        public string swap_time { get; set; }
        public double swap_long { get; set; }
        public double swap_short { get; set; }
        public double spread { get; set; }
        public double minimum_step { get; set; }
        public double minimum_amount { get; set; }
        public double leverage { get; set; }
        public double madoubleenance { get; set; }
        public object margin_call { get; set; }
    }

    public class ZECUSDT
    {
        public string _id { get; set; }
        public string symbol { get; set; }
        public string label { get; set; }
        public string currency { get; set; }
        public double decimal_digits { get; set; }
        public string yahoo_symbol { get; set; }
        public string description { get; set; }
        public bool asset_is_future { get; set; }
        public string category { get; set; }
        public string source { get; set; }
        public Times16 times { get; set; }
        public string icon_secondary { get; set; }
        public string icon_main { get; set; }
        public string menu_category { get; set; }
        public object menu_sub_category { get; set; }
        public Market_Data16 market_data { get; set; }
        public Platform_Attr16 platform_attr { get; set; }
        public string original_currency { get; set; }
    }

    public class Times16
    {
        public Open16[] open { get; set; }
    }

    public class Open16
    {
        public string from { get; set; }
        public string to { get; set; }
    }

    public class Market_Data16
    {
        public double bid { get; set; }
        public double ask { get; set; }
        public double low { get; set; }
        public double high { get; set; }
        public double open { get; set; }
        public double close { get; set; }
        public string time { get; set; }
        public double last_price { get; set; }
    }

    public class Platform_Attr16
    {
        public double commision { get; set; }
        public string swap_time { get; set; }
        public double swap_long { get; set; }
        public double swap_short { get; set; }
        public double spread { get; set; }
        public double minimum_step { get; set; }
        public double minimum_amount { get; set; }
        public double leverage { get; set; }
        public double madoubleenance { get; set; }
        public object margin_call { get; set; }
    }
}
