using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace AirSoftAutomationFramework.Objects.DTOs
{
    public class GetAllClientsResponse
    {
        public Campaign[] campaigns { get; set; }
        public Agent[] agents { get; set; }
        public Sales_Status_Text sales_status_text { get; set; }
        public Sales_Status_Text2 sales_status_text2 { get; set; }
        public Tradegroup[] tradeGroups { get; set; }
        public Accounttype[] accountTypes { get; set; }
        public Savingaccount[] savingAccounts { get; set; }

        [JsonProperty("data")]
        public List<ClientData> clientData { get; set; }
        public string draw { get; set; }
        public int recordsFiltered { get; set; }
        public int recordsTotal { get; set; }

        public class Sales_Status_Text
        {
            public string New { get; set; }
            public string CallBack { get; set; }
            public string Interested { get; set; }
            public string Deposit { get; set; }
            public string NoAnswer { get; set; }
            public string WrongNumber { get; set; }
            public string VoiceMail { get; set; }
            public string NotInterested { get; set; }
            public string NoAge { get; set; }
            public string NoLanguage { get; set; }
            public string HotLead { get; set; }
            public string DoublePhoneNumber { get; set; }
        }

        public class Sales_Status_Text2
        {
            public string _id { get; set; }
            public bool active { get; set; }
            public Sales_Status_Text1 sales_status_text { get; set; }
            public DateTime last_update { get; set; }
        }

        public class Sales_Status_Text1
        {
            public string _1 { get; set; }
            public string _2 { get; set; }
            public string _3 { get; set; }
            public string _4 { get; set; }
            public string _5 { get; set; }
        }

        public class ClientData
        {
            public string _id { get; set; }
            public string country { get; set; }
            public string currency_code { get; set; }
            public string free_text { get; set; }
            public string email { get; set; }
            public string first_name { get; set; }
            public string last_name { get; set; }
            public string phone { get; set; }
            public string gmt_timezone { get; set; }
            public string campaign_id { get; set; }
            public string note { get; set; }
            public string role { get; set; }
            public DateTime created_at { get; set; }
            public string sales_status { get; set; }
            public string saving_account_id { get; set; }
            public string account_type_id { get; set; }
            public object last_login { get; set; }
            public object last_logout { get; set; }
            public bool active { get; set; }
            public object attribution_date { get; set; }
            public string balance { get; set; }
            public string activation_status { get; set; }
            public string kyc_status { get; set; }
            public string kyc_proof_of_identity_status { get; set; }
            public string kyc_proof_of_identity { get; set; }
            public string kyc_proof_of_residency_status { get; set; }
            public string kyc_proof_of_residency { get; set; }
            public string kyc_credit_debit_card_documentation_status { get; set; }
            public string kyc_credit_debit_card_documentation { get; set; }
            public string kyc_credit_debit_card_back_documentation { get; set; }
            public string kyc_credit_debit_card_back_documentation_status { get; set; }
            public string kyc_details_for_client { get; set; }
            public string dod { get; set; }
            public string dod_status { get; set; }
            public object sales_status2 { get; set; }
            public string sales_agent { get; set; }
            public string cfd_group_id { get; set; }
            public string sub_campaign_key { get; set; }
            public string phone_2 { get; set; }
            public string available { get; set; }
            public double bonus { get; set; }
            public string demo_balance { get; set; }
            public string equity { get; set; }
            public string open_pnl { get; set; }
            public string min_margin { get; set; }
            public int margin_usage { get; set; }
            public string pnl { get; set; }
            public string pnl_real { get; set; }
            public string pnl_bonus { get; set; }
            public bool online { get; set; }
            public bool chat_enabled { get; set; }
            public string other_kyc_status { get; set; }
            public bool sales_status_answer { get; set; }
        }

        public class Campaign
        {
            public string _id { get; set; }
            public string name { get; set; }
        }

        public class Agent
        {
            public string _id { get; set; }
            public string username { get; set; }
            public string sales_type { get; set; }
        }

        public class Tradegroup
        {
            public string _id { get; set; }
            public string name { get; set; }
        }

        public class Accounttype
        {
            public string _id { get; set; }
            public string name { get; set; }
            public bool enable_demo { get; set; }
            public bool chat_enabled { get; set; }
        }

        public class Savingaccount
        {
            public string _id { get; set; }
            public string name { get; set; }
        }
    }
}
