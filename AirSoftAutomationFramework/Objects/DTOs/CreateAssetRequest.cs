// Ignore Spelling: ib ka Cfd crypto bloomberg Chrono

using Newtonsoft.Json;

namespace AirSoftAutomationFramework.Objects.DTOs
{
    public class CreateAssetRequest
    {
        public string symbol { get; set; }
        public string label { get; set; }
        public string category { get; set; }
        public object expiry_date { get; set; }
        public object roll_over_date { get; set; }
        public object dividend_date { get; set; }
        public string description { get; set; }
        public int ib_id { get; set; }
        public string ib_sec_type { get; set; }
        public bool asset_is_future { get; set; }
        public string exchange { get; set; }
        public string currency { get; set; }
        public int decimal_digits { get; set; }
        public string yahoo_symbol { get; set; }
        public string bloomberg_key { get; set; }
        public string source { get; set; }
        public string ms_symbol { get; set; }
        public string ms_exchange { get; set; }
        public string ms_type { get; set; }
        public string ka_exchange { get; set; }
        public string ka_type { get; set; }
        public object ka_symbol { get; set; }
        public string icon_main { get; set; }
        public object icon_secondary { get; set; }
        public Cfd cfd { get; set; }

#pragma warning disable CA1507 // Use nameof to express symbol names
        [JsonProperty("crypto")]
#pragma warning restore CA1507 // Use nameof to express symbol names
        public CreateAssetCrypto crypto { get; set; }
#pragma warning disable CA1507 // Use nameof to express symbol names
        [JsonProperty("chrono")]
#pragma warning restore CA1507 // Use nameof to express symbol names
        public CreateAssetChrono chrono { get; set; }
    }

    public class Cfd
    {
        public bool include { get; set; }
    }

    public class CreateAssetCrypto
    {
        public bool include { get; set; }
    }

    public class CreateAssetChrono
    {
        public bool include { get; set; }
    }
}
