using System;
using System.Collections.Generic;

namespace ConsoleApp;

public partial class FundsTransaction
{
    public int Id { get; set; }

    public string UserId { get; set; }

    /// <summary>
    /// deposit,delete_deposit,deposit_demo,delete_deposit_bonus,deposit_bonus,withdrawal,chargeback
    /// </summary>
    public string Type { get; set; }

    public string OrderId { get; set; }

    public string PspTransactionId { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    /// <summary>
    /// describe the action that made (psp name / chargeback / title on manual deposits etc.)
    /// </summary>
    public string Method { get; set; }

    public string ManualErpUserId { get; set; }

    public string ErpUserId { get; set; }

    public string ErpUserIdAssigned { get; set; }

    public double Amount { get; set; }

    public string Currency { get; set; }

    public double OriginalAmount { get; set; }

    public string OriginalCurrency { get; set; }

    public double? EurFixedAmount { get; set; }

    /// <summary>
    /// &apos;approved&apos;,&apos;rejected&apos;,&apos;pending&apos;,&apos;in_process&apos;,&apos;canceled&apos;,&apos;error&apos;
    /// </summary>
    public string Status { get; set; }

    public string PspInstanceId { get; set; }

    public bool Ftd { get; set; }

    public string RejectReason { get; set; }

    public string CampaignId { get; set; }

    public bool IsDeleted { get; set; }

    public DateTime? DeleteDate { get; set; }

    public string DeleteErpUserId { get; set; }

    public string DeleteReason { get; set; }

    public string Cctype { get; set; }

    public int? FirstDigits { get; set; }

    public string LastDigits { get; set; }

    public float AccountValue { get; set; }

    public DateTime? ExpiryDate { get; set; }

    /// <summary>
    /// used for knowing delete and chargebcaks  original ids
    /// </summary>
    public int? TransactionRefId { get; set; }

    public string WithdrawalData { get; set; }

    /// <summary>
    /// psp | bank_transfer | wallet_to_wallet | demo | bonus
    /// </summary>
    public string TransactionType { get; set; }

    public DateTime? WithdrawalProceedDate { get; set; }

    public string Title { get; set; }

    public string Note { get; set; }

    public sbyte CantBeDeleted { get; set; }

    public sbyte IsChargeback { get; set; }
}
