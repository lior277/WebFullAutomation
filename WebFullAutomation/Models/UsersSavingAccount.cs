using System;

namespace AirSoftAutomationFramework.Models;

public partial class UsersSavingAccount
{
    public int Id { get; set; }

    public string ActionType { get; set; }

    public string UserId { get; set; }

    public DateTime CreatedAt { get; set; }

    public double Amount { get; set; }

    public double? Balance { get; set; }

    public string SaId { get; set; }

    public double? SaPercentage { get; set; }
}
