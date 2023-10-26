using System;
using Newtonsoft.Json;

namespace AirSoftAutomationFramework.Objects.DTOs
{
    public class GetExportClientActivitiesResponse
    {
#pragma warning disable CA1507 // Use nameof to express symbol names
        [JsonProperty("Sheets")]
#pragma warning restore CA1507 // Use nameof to express symbol names
        public Sheets Sheets { get; set; }
    }

    public class Sheets
    {
#pragma warning disable CA1507 // Use nameof to express symbol names
        [JsonProperty("Trades")]
#pragma warning restore CA1507 // Use nameof to express symbol names
        public Trades[] Trades { get; set; }

#pragma warning disable CA1507 // Use nameof to express symbol names
        [JsonProperty("Finance")]
#pragma warning restore CA1507 // Use nameof to express symbol names
        public Finance[] Finance { get; set; }

        [JsonProperty("Saving Account")]
        public Finance[] SavingAccount { get; set; }
    }

    public class SavingAccount
    {
        [JsonProperty("ID")]
        public string Id { get; set; }

#pragma warning disable CA1507 // Use nameof to express symbol names
        [JsonProperty("Type")]
#pragma warning restore CA1507 // Use nameof to express symbol names
        public string Type { get; set; }

#pragma warning disable CA1507 // Use nameof to express symbol names
        [JsonProperty("Date")]
#pragma warning restore CA1507 // Use nameof to express symbol names
        public string Date { get; set; }

#pragma warning disable CA1507 // Use nameof to express symbol names
        [JsonProperty("Amount")]
#pragma warning restore CA1507 // Use nameof to express symbol names
        public long Amount { get; set; }

#pragma warning disable CA1507 // Use nameof to express symbol names
        [JsonProperty("Percentage")]
#pragma warning restore CA1507 // Use nameof to express symbol names
        public string Percentage { get; set; }

#pragma warning disable CA1507 // Use nameof to express symbol names
        [JsonProperty("Balance")]
#pragma warning restore CA1507 // Use nameof to express symbol names
        public string Balance { get; set; }
    }

    public class Finance
    {
        [JsonProperty("Transaction ID")]
        public string TransactionId { get; set; }

#pragma warning disable CA1507 // Use nameof to express symbol names
        [JsonProperty("Date")]
#pragma warning restore CA1507 // Use nameof to express symbol names
        public string Date { get; set; }

#pragma warning disable CA1507 // Use nameof to express symbol names
        [JsonProperty("Amount")]
#pragma warning restore CA1507 // Use nameof to express symbol names
        public long Amount { get; set; }

#pragma warning disable CA1507 // Use nameof to express symbol names
        [JsonProperty("Currency")]
#pragma warning restore CA1507 // Use nameof to express symbol names
        public string Currency { get; set; }

#pragma warning disable CA1507 // Use nameof to express symbol names
        [JsonProperty("Type")]
#pragma warning restore CA1507 // Use nameof to express symbol names
        public string Type { get; set; }

#pragma warning disable CA1507 // Use nameof to express symbol names
        [JsonProperty("Status")]
#pragma warning restore CA1507 // Use nameof to express symbol names
        public string Status { get; set; }
    }

    public class Trades
    {
        [JsonProperty("Order ID")]
        public string OrderId { get; set; }

        [JsonProperty("By Type")]
        public string ByType { get; set; }

        [JsonProperty("By Position")]
        public string ByPosition { get; set; }

#pragma warning disable CA1507 // Use nameof to express symbol names
        [JsonProperty("Instrument")]
#pragma warning restore CA1507 // Use nameof to express symbol names
        public string Instrument { get; set; }

#pragma warning disable CA1507 // Use nameof to express symbol names
        [JsonProperty("Amount")]
#pragma warning restore CA1507 // Use nameof to express symbol names
        public long Amount { get; set; }

#pragma warning disable CA1507 // Use nameof to express symbol names
        [JsonProperty("Status")]
#pragma warning restore CA1507 // Use nameof to express symbol names
        public string Status { get; set; }

        [JsonProperty("Open Time")]
        public DateTimeOffset OpenTime { get; set; }

        [JsonProperty("Open Price")]
        public double OpenPrice { get; set; }

        [JsonProperty("Minimum Margin")]
        public double MinimumMargin { get; set; }

        [JsonProperty("Stop Loss")]
        public string StopLoss { get; set; }

        [JsonProperty("Take Profit")]
        public string TakeProfit { get; set; }

#pragma warning disable CA1507 // Use nameof to express symbol names
        [JsonProperty("Commission")]
#pragma warning restore CA1507 // Use nameof to express symbol names
        public string Commission { get; set; }

#pragma warning disable CA1507 // Use nameof to express symbol names
        [JsonProperty("Swap")]
#pragma warning restore CA1507 // Use nameof to express symbol names
        public string Swap { get; set; }

        [JsonProperty("P/L")]
        public double PL { get; set; }

        [JsonProperty("Close Price")]
        public string ClosePrice { get; set; }

        [JsonProperty("Close Time")]
        public string CloseTime { get; set; }

        [JsonProperty("Close reason")]
        public string CloseReason { get; set; }
    }
}
