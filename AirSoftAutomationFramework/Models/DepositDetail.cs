using System;
using System.Collections.Generic;

namespace ConsoleApp;

public class DepositDetail
{
    public int Id { get; set; }

    public int FundsTransactionsId { get; set; }

    public string BuyerFirstname { get; set; }

    public string BuyerLastname { get; set; }

    public string BuyerEmail { get; set; }

    public DateOnly? BuyerBirthday { get; set; }

    public string BuyerPhone { get; set; }

    public string BuyerAddress { get; set; }

    public string BuyerCity { get; set; }

    public string BuyerZip { get; set; }

    public string BuyerCountry { get; set; }

    public string BuyerState { get; set; }

    public string BuyerIpAddress { get; set; }

    public string CustomField { get; set; }

    public string KycProofOfIdentity { get; set; }

    public string KycProofOfIdentityStatus { get; set; }

    public string KycProofOfResidency { get; set; }

    public string KycProofOfResidencyStatus { get; set; }

    public string KycCreditDebitCardDocumentation { get; set; }

    public string KycCreditDebitCardDocumentationStatus { get; set; }

    public sbyte DefaultKyc { get; set; }

    public string DodUrl { get; set; }

    public string DodId { get; set; }

    public string DodType { get; set; }
}
