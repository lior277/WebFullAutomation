using System;

namespace AirSoftAutomationFramework.Models;

public partial class FundsSnapshot
{
    public int Id { get; set; }

    public int? FundsTransactionId { get; set; }

    public sbyte? IsDeleted { get; set; }

    public sbyte? IsChargeback { get; set; }

    public float? AccountValue { get; set; }

    public DateTime? SnapDate { get; set; }

    public string UserId { get; set; }

    public double? Balance { get; set; }

    public string Type { get; set; }

    public string Status { get; set; }

    public double? Amount { get; set; }
}
