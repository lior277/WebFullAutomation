using Newtonsoft.Json;

namespace AirSoftAutomationFramework.Objects.DTOs
{
    public class GetAccountTypesResponse
    {
        [JsonProperty("data")]
        public AccountTypeData[] AccountTypeData { get; set; }

        [JsonProperty("recordsFiltered")]
        public long RecordsFiltered { get; set; }      
       
        [JsonProperty("recordsTotal")]
        public long RecordsTotal { get; set; }
    }

    public class AccountTypeData
    {
        [JsonProperty("_id")]
        public string AccountTypeId { get; set; }

        [JsonProperty("name")]
        public string AccountTypeName { get; set; }

        [JsonProperty("chat_enabled")]
        public bool? ChatEnabled { get; set; }

        [JsonProperty("initial_bonus")]
        public int? InitialBonus { get; set; }

        [JsonProperty("demo_initial_demo")]
        public long? DemoInitialDemo { get; set; }

        [JsonProperty("show_on_register")]
        public bool? ShowOnRegister { get; set; }

        [JsonProperty("enable_demo")]
        public bool? EnableDemo { get; set; }

        [JsonProperty("demo_reinitialize_balance_on_0")]
        public bool? DemoReinitializeBalanceOn0 { get; set; }

        [JsonProperty("default")]
        public bool? AccountTypeDefault { get; set; }

        [JsonProperty("saving_account_id")]
        public string SavingAccountId { get; set; }

        [JsonProperty("chrono_trading")]
        public bool? ChronoTrading { get; set; }

        [JsonProperty("deposit_range_id")]
        public object[] DepositRangeId { get; set; }

        [JsonProperty("trading_tabs")]
        public object[] TradingTabs { get; set; }

        [JsonProperty("deposit_obj")]
        public object[] DepositObj { get; set; }

        [JsonProperty("deposit_name")]
        public object[] DepositName { get; set; }

        [JsonProperty("saving_account_name")]
        public object[] SavingAccountName { get; set; }

        [JsonProperty("trading_tabs_name")]
        public object[] TradingTabsName { get; set; }

        [JsonProperty("nft_trading")]
        public bool? NftTrading { get; set; }
    }
}
