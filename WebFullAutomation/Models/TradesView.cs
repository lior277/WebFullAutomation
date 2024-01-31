using System;

namespace AirSoftAutomationFramework.Models;

public partial class TradesView
{
    public int Id { get; set; }

    public DateTime CreatedAt { get; set; }

    /// <summary>
    /// binary,cfd,crypto
    /// </summary>
    public string Platform { get; set; }

    public string UserId { get; set; }

    public bool ChronoTrade { get; set; }

    /// <summary>
    /// buy,sell
    /// </summary>
    public string TransactionType { get; set; }

    /// <summary>
    /// pending,open,close
    /// </summary>
    public string Status { get; set; }

    public string Currency { get; set; }

    public string OriginalCurrency { get; set; }

    public double OriginalConversionRate { get; set; }

    public float Amount { get; set; }

    /// <summary>
    /// the real amount in the user currency
    /// </summary>
    public double Investment { get; set; }

    /// <summary>
    /// the rate price it was bought or wanted to buy on pending
    /// </summary>
    public double Rate { get; set; }

    public string AssetSymbol { get; set; }

    public DateTime? TradeTimeStart { get; set; }

    public DateTime? TradeTimeEnd { get; set; }

    public float? ChronoLeverage { get; set; }

    public float StopLoss { get; set; }

    public bool WinningStatus { get; set; }

    public float WinningProfitPercent { get; set; }

    public float LossingProfitPercent { get; set; }

    public double? CloseAtProfit { get; set; }

    public double? CloseAtLoss { get; set; }

    public DateTime? LastSwapDateCharge { get; set; }

    public TimeOnly? SwapTime { get; set; }

    public float? SwapLong { get; set; }

    public float? SwapShort { get; set; }

    public float? Spread { get; set; }

    public float? SpreadOnOpen { get; set; }

    public float? MinimumStep { get; set; }

    public float? MinimumAmount { get; set; }

    public float? Leverage { get; set; }

    public float? Maintenance { get; set; }

    public float? Commision { get; set; }

    public double MinMargin { get; set; }

    public bool Demo { get; set; }

    public int? MassTradeId { get; set; }

    public string SchedulerId { get; set; }

    public double? ClosedProfitLoss { get; set; }

    public float SwapCommission { get; set; }

    public float ClosedRate { get; set; }

    public string CloseReason { get; set; }

    public DateTime? TradeCloseTime { get; set; }

    /// <summary>
    /// on pending trades: indicates whether the order rate was higher than the current rate
    /// </summary>
    public bool PendingRateHigher { get; set; }

    public byte IsNft { get; set; }

    public double? CurrentRate { get; set; }

    public double? ProfitLoss { get; set; }

    public string Error { get; set; }

    public bool OnAssetOpen { get; set; }

    public bool RollOverSent { get; set; }

    public bool ExpireSent { get; set; }
}
