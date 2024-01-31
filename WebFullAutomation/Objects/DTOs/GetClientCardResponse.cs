using Newtonsoft.Json;
using System;

namespace AirSoftAutomationFramework.Objects.DTOs
{
    public class GetClientCardResponse
    {
            public Campaign[] campaigns { get; set; }
            public Agent[] agents { get; set; }
            public Sales_Status_Text sales_status_text { get; set; }
            public Sales_Status_Text2 sales_status_text2 { get; set; }
            public Tradegroup[] tradeGroups { get; set; }
            public Accounttype[] accountTypes { get; set; }
            public Savingaccount[] savingAccounts { get; set; }
            public User user { get; set; }

        public class Sales_Status_Text
        {
            public New New { get; set; }
            public CallBack CallBack { get; set; }
            public Interested Interested { get; set; }
            public Deposit Deposit { get; set; }
            public NoAnswer NoAnswer { get; set; }
            public WrongNumber WrongNumber { get; set; }
            public VoiceMail VoiceMail { get; set; }
            public NotInterested NotInterested { get; set; }
            public NoAge NoAge { get; set; }
            public NoLanguage NoLanguage { get; set; }
            public HotLead HotLead { get; set; }
            public DoublePhoneNumber DoublePhoneNumber { get; set; }
        }

        public class New
        {
            public string color { get; set; }
            public bool answer { get; set; }
        }

        public class CallBack
        {
            public string color { get; set; }
            public bool answer { get; set; }
        }

        public class Interested
        {
            public string color { get; set; }
            public bool answer { get; set; }
        }

        public class Deposit
        {
            public string color { get; set; }
            public bool answer { get; set; }
        }

        public class NoAnswer
        {
            public string color { get; set; }
            public bool answer { get; set; }
        }

        public class WrongNumber
        {
            public string color { get; set; }
            public bool answer { get; set; }
        }

        public class VoiceMail
        {
            public string color { get; set; }
            public bool answer { get; set; }
        }

        public class NotInterested
        {
            public string color { get; set; }
            public bool answer { get; set; }
        }

        public class NoAge
        {
            public string color { get; set; }
            public bool answer { get; set; }
        }

        public class NoLanguage
        {
            public string color { get; set; }
            public bool answer { get; set; }
        }

        public class HotLead
        {
            public string color { get; set; }
            public bool answer { get; set; }
        }

        public class DoublePhoneNumber
        {
            public string color { get; set; }
            public bool answer { get; set; }
        }

        public class Sales_Status_Text2
        {
            public string _id { get; set; }
            public bool active { get; set; }

            [JsonProperty("Sales_Status_Text")]
            public Sales_Status_Text_ForTwo sales_status_text { get; set; }
            public DateTime last_update { get; set; }
        }
      
        public class Sales_Status_Text_ForTwo
        {
            public string _1 { get; set; }
            public string _2 { get; set; }
            public string _3 { get; set; }
            public string _4 { get; set; }
            public string _5 { get; set; }
        }

        public class User
        {
            public string _id { get; set; }
            public string country { get; set; }
            public string currency_code { get; set; }
            public string free_text { get; set; }
            public string email { get; set; }
            public string first_name { get; set; }
            public string last_name { get; set; }
            public string phone { get; set; }
            public string phone_2 { get; set; }
            public string gmt_timezone { get; set; }
            public string campaign_id { get; set; }
            public string note { get; set; }
            public string role { get; set; }
            public object sub_campaign_key { get; set; }
            public DateTime created_at { get; set; }
            public string sales_status { get; set; }
            public string saving_account_id { get; set; }
            public string account_type_id { get; set; }
            public object last_login { get; set; }
            public object last_logout { get; set; }
            public bool active { get; set; }
            public object attribution_date { get; set; }
            public double balance { get; set; }
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
            public string other_kyc { get; set; }
            public string other_kyc_status { get; set; }
            public object sales_status2 { get; set; }
            public object sales_agent { get; set; }
            public object mass_trade { get; set; }
            public string cfd_group_id { get; set; }
            public string available { get; set; }
            public int bonus { get; set; }
            public string demo_balance { get; set; }
            public string equity { get; set; }
            public string open_pnl { get; set; }
            public string min_margin { get; set; }
            public object margin_usage { get; set; }
            public string pnl { get; set; }
            public string pnl_real { get; set; }
            public string pnl_bonus { get; set; }
            public bool online { get; set; }
            public bool chat_enabled { get; set; }
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
