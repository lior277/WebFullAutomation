// Ignore Spelling: Kyc Ftd Por

using Newtonsoft.Json;

namespace AirSoftAutomationFramework.Objects.DTOs
{
    public class ExportClientTableResponse
    {
        [JsonProperty("ID")]
        public string Id { get; set; }

        [JsonProperty("Full Name")]
        public string FullName { get; set; }

#pragma warning disable CA1507 // Use nameof to express symbol names
        [JsonProperty("Country")]
#pragma warning restore CA1507 // Use nameof to express symbol names
        public string Country { get; set; }

        [JsonProperty("SA Balance")]
        public string SABalance { get; set; }    

        [JsonProperty("Sales Agent")]
        public string SalesAgent { get; set; }

#pragma warning disable CA1507 // Use nameof to express symbol names
        [JsonProperty("Campaign")]
#pragma warning restore CA1507 // Use nameof to express symbol names
        public string Campaign { get; set; }

        public string Answer { get; set; }

#pragma warning disable CA1507 // Use nameof to express symbol names
        [JsonProperty("Email")]
#pragma warning restore CA1507 // Use nameof to express symbol names
        public string Email { get; set; }

#pragma warning disable CA1507 // Use nameof to express symbol names
        [JsonProperty("Phone")]
#pragma warning restore CA1507 // Use nameof to express symbol names
        public string Phone { get; set; }

        [JsonProperty("Last Attribution | GMT")]
        public string LastAttributionDateGMT { get; set; }

        [JsonProperty("Free Text")]
        public string FreeText { get; set; }

        [JsonProperty("Trading Group")]
        public string TradingGroup { get; set; }
        
        [JsonProperty("Total Deposit")]
        public string TotalDeposit { get; set; }

        [JsonProperty("#Deposit")]
        public string NumOfDeposit { get; set; }

#pragma warning disable CA1507 // Use nameof to express symbol names
        [JsonProperty("Balance")]
#pragma warning restore CA1507 // Use nameof to express symbol names
        public string Balance { get; set; }

        [JsonProperty("Bonus %")]
        public string Bonus { get; set; }

        [JsonProperty("Status 2")]
        public string Status2 { get; set; }

        [JsonProperty("KYC POI")]
        public string KycPoi { get; set; }

        [JsonProperty("KYC POR")]
        public string KycPor { get; set; }

        [JsonProperty("KYC CC FRONT")]
        public string KycCcFront { get; set; }

        [JsonProperty("KYC CC BACK")]
        public string KycCcBack { get; set; }

        [JsonProperty("KYC Status")]
        public string KycStatus { get; set; }

        [JsonProperty("Total Bonus")]
        public string TotalBonus { get; set; }

        [JsonProperty("Registration | GMT")]
        public string RegistrationGMT { get; set; }

#pragma warning disable CA1507 // Use nameof to express symbol names
        [JsonProperty("Status")]
#pragma warning restore CA1507 // Use nameof to express symbol names
        public string Status { get; set; }

#pragma warning disable CA1507 // Use nameof to express symbol names
        [JsonProperty("Office")]
#pragma warning restore CA1507 // Use nameof to express symbol names
        public string Office { get; set; }

        [JsonProperty("Last Digits")]
        public string LastDigits { get; set; }

        [JsonProperty("Last Login | GMT")]
        public string LastLoginGMT { get; set; }

        [JsonProperty("Last Comment")]
        public string LastComment { get; set; }

        [JsonProperty("Last Comment | GMT")]
        public string LastCommentGMT { get; set; }

        [JsonProperty("sub Campaign")]
        public string subCampaign { get; set; }

        [JsonProperty("Last Trade")]
        public string LastTrade { get; set; }

        [JsonProperty("Last Call")]
        public string LastCall { get; set; }

#pragma warning disable CA1507 // Use nameof to express symbol names
        [JsonProperty("Note")]
#pragma warning restore CA1507 // Use nameof to express symbol names
        public string Note { get; set; }

        [JsonProperty("Last Deposit | GMT")]
        public string LastDepositGMT { get; set; }

        [JsonProperty("FTD | GMT")]
        public string FtdGMT { get; set; }
    }
}
