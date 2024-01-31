using Newtonsoft.Json;

namespace AirSoftAutomationFramework.Objects.DTOs
{
    public class PutCurrenciesRequest
    {
#pragma warning disable CA1507 // Use nameof to express symbol names
        [JsonProperty("currencies")]
#pragma warning restore CA1507 // Use nameof to express symbol names
        public PutCurrencies currencies { get; set; }
    }

    public class PutCurrencies
    {
#pragma warning disable CA1507 // Use nameof to express symbol names
        [JsonProperty("USD")]
#pragma warning restore CA1507 // Use nameof to express symbol names
        public UsdCurrencie USD { get; set; }

#pragma warning disable CA1507 // Use nameof to express symbol names
        [JsonProperty("EUR")]
#pragma warning restore CA1507 // Use nameof to express symbol names
        public EurCurrencie EUR { get; set; }

#pragma warning disable CA1507 // Use nameof to express symbol names
        [JsonProperty("CAD")]
#pragma warning restore CA1507 // Use nameof to express symbol names
        public CadCurrencie CAD { get; set; }
    }

    public class UsdCurrencie
    {
        public int max_initial_bonus { get; set; }
        public int max_bonus_after_deposit { get; set; }
        [JsonProperty("default")]
        public bool Default { get; set; }
    }

    public class EurCurrencie
    {
        public int max_initial_bonus { get; set; }
        public int max_bonus_after_deposit { get; set; }
        [JsonProperty("default")]
        public bool Default { get; set; }
    }

    public class CadCurrencie
    {
        public int max_initial_bonus { get; set; }
        public int max_bonus_after_deposit { get; set; }
        [JsonProperty("default")]
        public bool Default { get; set; }
    }
}
