using System;

namespace AirSoftAutomationFramework.Models;

public partial class MassTrade
{
    public int Id { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? ClosedAt { get; set; }

    public int? TotalUsers { get; set; }

    public float? Exposure { get; set; }

    public string AssetSymbol { get; set; }

    public string TransactionType { get; set; }

    public bool? IsPending { get; set; }

    public double? Rate { get; set; }

    public double? StopLoss { get; set; }

    public double? TakeProfit { get; set; }
}
