using System;

namespace AirSoftAutomationFramework.Models;

public partial class UserAccount
{
    public string UserId { get; set; }

    public string Currency { get; set; }

    public double FundsTransactions { get; set; }

    public double ClosedProfitLoss { get; set; }

    public double OpenProfitLoss { get; set; }

    public double OpenInvestments { get; set; }

    public double? Balance { get; set; }

    public double? Available { get; set; }

    public double MinMargin { get; set; }

    public double DemoFundsTransactions { get; set; }

    public double DemoClosedProfitLoss { get; set; }

    public double DemoOpenProfitLoss { get; set; }

    public double DemoOpenInvestments { get; set; }

    public double? DemoBalance { get; set; }

    public double? DemoAvailable { get; set; }

    public double? DemoEquity { get; set; }

    public double DemoMinMargin { get; set; }

    public double Bonus { get; set; }

    public double SavingAccountDeposits { get; set; }

    public double SavingAccountWithdrawals { get; set; }

    public double SavingAccountProfit { get; set; }

    public DateTime SavingAccountProfitLastUpdate { get; set; }

    public double? SavingAccountCurrentAmount { get; set; }

    public double SavingAccountStashedProfit { get; set; }

    public string ErpUserId { get; set; }

    public DateTime? BalanceUpdate { get; set; }

    public double TotalDeposit { get; set; }

    public DateTime CreatedAt { get; set; }

    public bool NeedToUpdateBalance { get; set; }

    public double? Equity { get; set; }

    public sbyte? MarginCall { get; set; }

    public DateTime? MarginCallNotifiedAt { get; set; }

    public bool SuspiciousProfitSent { get; set; }

    public bool PauseTrades { get; set; }
}
