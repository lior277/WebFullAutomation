using System;

namespace AirSoftAutomationFramework.Objects.DTOs
{
    public class GetClientsRespose
    {

        public Datum[] data { get; set; }
        public int recordsFiltered { get; set; }
        public int recordsTotal { get; set; }

        public class Datum
        {
            public string _id { get; set; }
            public string country { get; set; }
            public string note { get; set; }
            public string free_text { get; set; }
            public string free_text_2 { get; set; }
            public string free_text_3 { get; set; }
            public string free_text_4 { get; set; }
            public string free_text_5 { get; set; }
            public string phone { get; set; }
            public string phone_2 { get; set; }
            public string email { get; set; }
            public string created_at { get; set; }
            public string attribution_date { get; set; }
            public string balance { get; set; }
            public string sa_balance { get; set; }
            public object last_login { get; set; }
            public object last_logout { get; set; }
            public string total_deposit { get; set; }
            public string role { get; set; }
            public string cfd_group_id { get; set; }
            public object crypto_group_id { get; set; }
            public string kyc_status { get; set; }
            public string kyc_proof_of_identity_status { get; set; }
            public string kyc_proof_of_residency_status { get; set; }
            public string kyc_credit_debit_card_documentation_status { get; set; }
            public string dod_status { get; set; }
            public string other_kyc_status { get; set; }
            public string kyc_credit_debit_card_back_documentation_status { get; set; }
            public string sales_status { get; set; }
            public string sales_status2 { get; set; }
            public string saving_account_id { get; set; }
            public int bonus { get; set; }
            public string activation_status { get; set; }
            public string currency_code { get; set; }
            public object last_trade { get; set; }
            public string last_comment { get; set; }
            public object last_call { get; set; }
            public string ftd_date { get; set; }
            public Deposit[] deposits { get; set; }
            public string last_deposit_date { get; set; }
            public bool active { get; set; }
            public string margin_call_notified_date { get; set; }
            public object vpn_status { get; set; }
            public string full_name { get; set; }
            public string campaign { get; set; }
            public Comment[] comments { get; set; }
            public string sales_agent { get; set; }
            public string office { get; set; }
            public string total_bonus { get; set; }
            public string last_modified { get; set; }
            public int total_deposit_count { get; set; }
            public bool online { get; set; }
            public string group { get; set; }
        }

        public class Deposit
        {
            public string last_digits { get; set; }
            public int transaction_id { get; set; }
            public string date { get; set; }
        }

        public class Comment
        {
            public string _id { get; set; }
            public string user_id { get; set; }
            public string date { get; set; }
            public string comment { get; set; }
            public string created_at { get; set; }
            public string erp_user_id { get; set; }
            public string scheduler_id { get; set; }
            public string last_update { get; set; }
            public string erp_username { get; set; }
        }
    }
}
