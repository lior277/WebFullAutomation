
using System;

namespace AirSoftAutomationFramework.Objects.DTOs
{
    public class GetClientProfileResponse
    {
        public string _id { get; set; }
        public string country { get; set; }
        public string currency_code { get; set; }
        public string first_name { get; set; }
        public string last_name { get; set; }
        public string lang { get; set; }
        public string saving_account_id { get; set; }
        public string account_type_id { get; set; }
        public bool terms_conditions { get; set; }
        public DateTime last_login { get; set; }
        public string profile_image { get; set; }
        public int balance { get; set; }
        public object[] favorite_assets { get; set; }
        public object[] alerts { get; set; }
        public string kyc_status { get; set; }
        public string kyc_proof_of_identity_status { get; set; }
        public string kyc_proof_of_identity { get; set; }
        public string kyc_proof_of_residency_status { get; set; }
        public string kyc_proof_of_residency { get; set; }
        public string kyc_credit_debit_card_documentation_status { get; set; }
        public string kyc_credit_debit_card_documentation { get; set; }
        public string kyc_credit_debit_card_back_documentation { get; set; }
        public string kyc_credit_debit_card_back_documentation_status { get; set; }
        public string dod { get; set; }
        public string dod_status { get; set; }
        public string sales_agent { get; set; }
        public string account_type { get; set; }
        public bool chat_enabled { get; set; }
        public string platform_name { get; set; }
        public string platform { get; set; }
        public Financeaccount financeAccount { get; set; }
    }

    public class Financeaccount
    {
        public string user_id { get; set; }
        public string currency { get; set; }
        public int funds_transactions { get; set; }
        public int closed_profit_loss { get; set; }
        public float open_profit_loss { get; set; }
        public float open_investments { get; set; }
        public int balance { get; set; }
        public float available { get; set; }
        public float min_margin { get; set; }
        public int demo_funds_transactions { get; set; }
        public int demo_closed_profit_loss { get; set; }
        public int demo_open_profit_loss { get; set; }
        public int demo_open_investments { get; set; }
        public int demo_balance { get; set; }
        public int demo_available { get; set; }
        public int demo_equity { get; set; }
        public int demo_min_margin { get; set; }
        public int bonus { get; set; }
        public int saving_account_deposits { get; set; }
        public int saving_account_withdrawals { get; set; }
        public int saving_account_profit { get; set; }
        public DateTime saving_account_profit_last_update { get; set; }
        public int saving_account_current_amount { get; set; }
        public int saving_account_stashed_profit { get; set; }
        public string erp_user_id { get; set; }
        public object balance_update { get; set; }
        public int total_deposit { get; set; }
        public DateTime created_at { get; set; }
        public int need_to_update_balance { get; set; }
        public float equity { get; set; }
        public object margin_call { get; set; }
        public object margin_call_notified_at { get; set; }
        public int suspicious_profit_sent { get; set; }
        public int pause_trades { get; set; }
    }

}

