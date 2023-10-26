using Newtonsoft.Json;

namespace AirSoftAutomationFramework.Objects.DTOs
{
    public class GetDepositResponse
    {
        [JsonProperty("data")]
        public DepositDataResponse[] depositDataResponse { get; set; }       
    }

    public class DepositDataResponse
    {
        [JsonProperty("transaction_id")]
        public long? TransactionId { get; set; }

        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("order_id")]
        public object OrderId { get; set; }

        [JsonProperty("reject_reason")]
        public object RejectReason { get; set; }

        [JsonProperty("user_id")]
        public string UserId { get; set; }

        [JsonProperty("client_full_name")]
        public string ClientFullName { get; set; }

        [JsonProperty("phone")]
        public long Phone { get; set; }

        [JsonProperty("country")]
        public string Country { get; set; }

        [JsonProperty("email")]
        public string Email { get; set; }

        [JsonProperty("comments")]
        public long Comments { get; set; }

        [JsonProperty("ftd")]
        public long Ftd { get; set; }

        [JsonProperty("amount")]
        public string Amount { get; set; }

        [JsonProperty("currency")]
        public string Currency { get; set; }

        [JsonProperty("original_amount")]
        public long OriginalAmount { get; set; }

        [JsonProperty("original_currency")]
        public string OriginalCurrency { get; set; }

        [JsonProperty("last_digits")]
        public long? LastDigits { get; set; }

        [JsonProperty("created_at")]
        public string CreatedAt { get; set; }

        [JsonProperty("updated_at")]
        public object UpdatedAt { get; set; }

        [JsonProperty("campaign")]
        public string Campaign { get; set; }

        [JsonProperty("erp_username")]
        public string ErpUsername { get; set; }

        [JsonProperty("erp_full_name")]
        public string ErpFullName { get; set; }

        [JsonProperty("erp_assigned")]
        public string ErpAssigned { get; set; }

        [JsonProperty("erp_user_id_assigned")]
        public object ErpUserIdAssigned { get; set; }

        [JsonProperty("psp")]
        public string Psp { get; set; }

        [JsonProperty("method")]
        public string Method { get; set; }

        [JsonProperty("psp_name")]
        public string PspName { get; set; }

        [JsonProperty("display_name")]
        public string DisplayName { get; set; }

        [JsonProperty("office_id")]
        public string OfficeId { get; set; }

        [JsonProperty("office_city")]
        public string OfficeCity { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("note")]
        public object Note { get; set; }

        [JsonProperty("status")]
        public string Status { get; set; }

        [JsonProperty("balance")]
        public string Balance { get; set; }

        [JsonProperty("total_deposit")]
        public string TotalDeposit { get; set; }

        [JsonProperty("eur_fixed_amount")]
        public double EurFixedAmount { get; set; }

        [JsonProperty("withdrawal_proceed_date")]
        public string WithdrawalProceedDate { get; set; }

        [JsonProperty("deposit_details")]
        public string DepositDetails { get; set; }
    }
}
