using Newtonsoft.Json;

namespace AirSoftAutomationFramework.Objects.DTOs
{
    public class ExportFinancesTablesResponse
    {
        [JsonProperty("Client ID")]
        public string ClientId { get; set; }

#pragma warning disable CA1507 // Use nameof to express symbol names
        [JsonProperty("Name")]
#pragma warning restore CA1507 // Use nameof to express symbol names
        public string Name { get; set; }

        [JsonProperty("Client Name")]
        public string ClientName { get; set; }

        [JsonProperty("Open Time | GMT")]
        public string OpenTimeGMT { get; set; }

        [JsonProperty("Creation date")]
        public string CreationDate { get; set; }

#pragma warning disable CA1507 // Use nameof to express symbol names
        [JsonProperty("Deal")]
#pragma warning restore CA1507 // Use nameof to express symbol names
        public string Deal { get; set; }

        [JsonProperty(PropertyName = "Leads#")]
        public int Leads { get; set; }

        [JsonProperty(PropertyName = "FTD#")]
        public int FTD { get; set; }

        [JsonProperty("User ID")]
        public string UserId { get; set; }

#pragma warning disable CA1507 // Use nameof to express symbol names
        [JsonProperty("Other")]
#pragma warning restore CA1507 // Use nameof to express symbol names
        public string Other { get; set; }

        [JsonProperty("updated_at")]
        public string UpdatedAt { get; set; }

        [JsonProperty("Open Price")]
        public string OpenPrice { get; set; }

        [JsonProperty("Current Price")]
        public string CurrentPrice { get; set; }

        [JsonProperty("PNL")]
        public string Pnl { get; set; }

        [JsonProperty("PNL Close trades")]
        public string PnlCloseTrades { get; set; }

        [JsonProperty("PNL Open trades")]
        public string PnlOpenTrades { get; set; }

        [JsonProperty("Close Time | GMT")]
        public string CloseTimeGMT { get; set; }

        [JsonProperty("SL TP")]
        public string SlTp { get; set; }

#pragma warning disable CA1507 // Use nameof to express symbol names
        [JsonProperty("Commission")]
#pragma warning restore CA1507 // Use nameof to express symbol names
        public string Commission { get; set; }

#pragma warning disable CA1507 // Use nameof to express symbol names
        [JsonProperty("Swap")]
#pragma warning restore CA1507 // Use nameof to express symbol names

        public string Swap { get; set; }

#pragma warning disable CA1507 // Use nameof to express symbol names
        [JsonProperty("ID")]
#pragma warning restore CA1507 // Use nameof to express symbol names
        public string ID { get; set; }

#pragma warning disable CA1507 // Use nameof to express symbol names
        [JsonProperty("Id")]
#pragma warning restore CA1507 // Use nameof to express symbol names
        public int Id { get; set; }

        [JsonProperty("Full Name")]
        public string FullName { get; set; }

        [JsonProperty("Client Full Name")]
        public string ClientFullName { get; set; }

#pragma warning disable CA1507 // Use nameof to express symbol names
        [JsonProperty("Email")]
#pragma warning restore CA1507 // Use nameof to express symbol names
        public string Email { get; set; }

#pragma warning disable CA1507 // Use nameof to express symbol names
        [JsonProperty("Currency")]
#pragma warning restore CA1507 // Use nameof to express symbol names
        public string Currency { get; set; }

#pragma warning disable CA1507 // Use nameof to express symbol names
        [JsonProperty("Country")]
#pragma warning restore CA1507 // Use nameof to express symbol names
        public string Country { get; set; }

        [JsonProperty("Registration | GMT")]
        public string RegistrationGMT { get; set; }      

        [JsonProperty("Transaction ID")]
        public string TransactionId { get; set; }

        [JsonProperty("Withdrawal Amount")]
        public string WithdrawalAmount { get; set; }

        [JsonProperty("Total Withdrawal")]
        public string TotalWithdrawal { get; set; }

        [JsonProperty("Amount of Chargeback")]
        public string AmountOfChargeback { get; set; }

        [JsonProperty("Amount of Bonus")]
        public string AmountOfBonus { get; set; }

        [JsonProperty("Proceed Date | GMT")]
        public string ProceedDateGMT { get; set; }

        [JsonProperty("Date of Deposit | GMT")]
        public string DateOfDepositGMT { get; set; }

        [JsonProperty("Date of Chargeback | GMT")]
        public string DateOfChargebackGMT { get; set; }

        [JsonProperty("Date of Bonus | GMT")]
        public string DateOfBonusGMT { get; set; }

#pragma warning disable CA1507 // Use nameof to express symbol names
        [JsonProperty("Amount")]
#pragma warning restore CA1507 // Use nameof to express symbol names
        public long Amount { get; set; }

#pragma warning disable CA1507 // Use nameof to express symbol names
        [JsonProperty("Type")]
#pragma warning restore CA1507 // Use nameof to express symbol names
        public string Type { get; set; }

#pragma warning disable CA1507 // Use nameof to express symbol names
        [JsonProperty("Asset")]
#pragma warning restore CA1507 // Use nameof to express symbol names
        public string Asset { get; set; }

#pragma warning disable CA1507 // Use nameof to express symbol names
        [JsonProperty("Balance")]
#pragma warning restore CA1507 // Use nameof to express symbol names
        public string Balance { get; set; }

#pragma warning disable CA1507 // Use nameof to express symbol names
        [JsonProperty("Available")]
#pragma warning restore CA1507 // Use nameof to express symbol names
        public string Available { get; set; }

#pragma warning disable CA1507 // Use nameof to express symbol names
        [JsonProperty("Equity")]
#pragma warning restore CA1507 // Use nameof to express symbol names
        public string Equity { get; set; }

        [JsonProperty("Close Profit Loss")]
        public string CloseProfitLoss { get; set; }

        [JsonProperty("Open Profit Loss")]
        public string OpenProfitLoss { get; set; }

#pragma warning disable CA1507 // Use nameof to express symbol names
        [JsonProperty("Profit")]
#pragma warning restore CA1507 // Use nameof to express symbol names
        public decimal Profit { get; set; }

        [JsonProperty("Player Value")]
        public decimal PlayerValue { get; set; }

        [JsonProperty("Total Swap Commission")]
        public string TotalSwapCommission { get; set; }

        [JsonProperty("Min Margin")]
        public string MinMargin { get; set; }

        [JsonProperty("Margin Usage")]
        public int MarginUsage { get; set; }

#pragma warning disable CA1507 // Use nameof to express symbol names
        [JsonProperty("Volume")]
#pragma warning restore CA1507 // Use nameof to express symbol names
        public string Volume { get; set; }

        [JsonProperty("Euro Amount")]
        public decimal EuroAmount { get; set; }

        [JsonProperty("Original Amount")]
        public string OriginalAmount { get; set; }

        [JsonProperty("Order ID")]
        public string OrderId { get; set; }

        [JsonProperty("Original Currency")]
        public string OriginalCurrency { get; set; }

        [JsonProperty("Erp User Id")]
        public string ErpUserId { get; set; }

        [JsonProperty("Transaction Status")]
        public string TransactionStatus { get; set; }

#pragma warning disable CA1507 // Use nameof to express symbol names
        [JsonProperty("Status")]
#pragma warning restore CA1507 // Use nameof to express symbol names
        public string Status { get; set; }

        [JsonProperty("Request Date | GMT")]
        public string RequestDateGMT { get; set; }

        [JsonProperty("PSP")]
        public string Psp { get; set; }

        [JsonProperty("Display Name")]
        public string DisplayName { get; set; }

        [JsonProperty("Assigned To")]
        public string AssignedTo { get; set; }

#pragma warning disable CA1507 // Use nameof to express symbol names
        [JsonProperty("Office")]
#pragma warning restore CA1507 // Use nameof to express symbol names
        public string Office { get; set; }

        [JsonProperty("Free Text")]
        public string FreeText { get; set; }

#pragma warning disable CA1507 // Use nameof to express symbol names
        [JsonProperty("FTD")]
#pragma warning restore CA1507 // Use nameof to express symbol names
        public long Ftd { get; set; }

        [JsonProperty("Total Deposits")]
        public string TotalDeposits { get; set; }

        [JsonProperty("Deposit amount")]
        public string DepositAmount { get; set; }

#pragma warning disable CA1507 // Use nameof to express symbol names
        [JsonProperty("Title")]
#pragma warning restore CA1507 // Use nameof to express symbol names
        public string Title { get; set; }

#pragma warning disable CA1507 // Use nameof to express symbol names
        [JsonProperty("Deposits")]
#pragma warning restore CA1507 // Use nameof to express symbol names
        public string Deposits { get; set; }

#pragma warning disable CA1507 // Use nameof to express symbol names
        [JsonProperty("Bonus")]
#pragma warning restore CA1507 // Use nameof to express symbol names
        public string Bonus { get; set; }

        [JsonProperty(PropertyName = "Deposit#")]
        public int NumOfDeposit { get; set; }

        [JsonProperty(PropertyName = "Conversion%")]
        public double Conversion { get; set; }

#pragma warning disable CA1507 // Use nameof to express symbol names
        [JsonProperty("Cost")]
#pragma warning restore CA1507 // Use nameof to express symbol names
        public double Cost { get; set; }

        [JsonProperty("Total Deposit")]
        public string TotalDeposit { get; set; }

        [JsonProperty(PropertyName = "% of Σ Deposit")]
        public object  PrecentageOfAllDeposits { get; set; }

        [JsonProperty("Total Bonus")]
        public string TotalBonus { get; set; }

#pragma warning disable CA1507 // Use nameof to express symbol names
        [JsonProperty("Campaign")]
#pragma warning restore CA1507 // Use nameof to express symbol names
        public string Campaign { get; set; }

        [JsonProperty("Sales Agent")]
        public string SalesAgent { get; set; }

        [JsonProperty("Sales Agent Name")]
        public string SalesAgentName { get; set; }

        [JsonProperty("method")]
        public string Method { get; set; }

#pragma warning disable CA1507 // Use nameof to express symbol names
        [JsonProperty("Phone")]
#pragma warning restore CA1507 // Use nameof to express symbol names
        public string Phone { get; set; }

        [JsonProperty("Reject Reason")]
        public string RejectReason { get; set; }

#pragma warning disable CA1507 // Use nameof to express symbol names
        [JsonProperty("Note")]
#pragma warning restore CA1507 // Use nameof to express symbol names
        public string Note { get; set; }
    }
}
