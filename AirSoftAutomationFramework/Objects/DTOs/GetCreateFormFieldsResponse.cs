using System;
using Newtonsoft.Json;

namespace AirSoftAutomationFramework.Objects.DTOs
{
    public class GetCreateClientFormFieldsResponse
    {
#pragma warning disable CA1507 // Use nameof to express symbol names
        [JsonProperty("currencies")]
#pragma warning restore CA1507 // Use nameof to express symbol names
        public ClientCurrencies currencies { get; set; }
        public string _id { get; set; }
        public DateTime last_update { get; set; }
    }

    public class ClientCurrencies
    {
#pragma warning disable CA1507 // Use nameof to express symbol names
        [JsonProperty("USD")]
#pragma warning restore CA1507 // Use nameof to express symbol names
        public ClientUSDCurrency USD { get; set; }

#pragma warning disable CA1507 // Use nameof to express symbol names
        [JsonProperty("EUR")]
#pragma warning restore CA1507 // Use nameof to express symbol names
        public ClientEURCurrency EUR { get; set; }
    }

    public class ClientUSDCurrency
    {
        public int max_initial_bonus { get; set; }
        public int max_bonus_after_deposit { get; set; }
        public bool _default { get; set; }
        public object[] countries { get; set; }
    }

    public class ClientEURCurrency
    {
        public int max_initial_bonus { get; set; }
        public int max_bonus_after_deposit { get; set; }
        public bool _default { get; set; }
        public object[] countries { get; set; }
    }

}

