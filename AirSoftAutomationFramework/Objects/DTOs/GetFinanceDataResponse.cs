using System;

namespace AirSoftAutomationFramework.Objects.DTOs
{
    public class GetFinanceDataResponse
    {


        public FinanceData[] Property1 { get; set; }


        public class FinanceData
        {
            public int id { get; set; }
            public int is_deleted { get; set; }
            public string status { get; set; }
            public object delete_date { get; set; }
            public string last_digits { get; set; }
            public string delete_erp_user_id { get; set; }
            public string delete_reason { get; set; }
            public string reject_reason { get; set; }
            public DateTime? created_at { get; set; }
            public string erp_user_id_assigned { get; set; }
            public string currency { get; set; }
            public int original_amount { get; set; }
            public int amount { get; set; }
            public string original_currency { get; set; }
            public string method { get; set; }
            public string manual_erp_user_id { get; set; }
            public string type { get; set; }
            public string psp_transaction_id { get; set; }
            public string order_id { get; set; }
            public string withdrawal_data { get; set; }
            public object title { get; set; }
            public object kyc_proof_of_identity_status { get; set; }
            public object kyc_proof_of_residency_status { get; set; }
            public object kyc_credit_debit_card_documentation_status { get; set; }
            public object dod_url { get; set; }
            public int deleted_amount { get; set; }
            public double pnl { get; set; }
            public string erp_username { get; set; }
            public string delete_erp_user_username { get; set; }
        }
    }
}
