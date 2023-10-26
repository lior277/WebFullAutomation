using System;
using Newtonsoft.Json;
using static AirSoftAutomationFramework.Objects.DTOs.GetSalesStatusResponse;

namespace AirSoftAutomationFramework.Objects.DTOs
{
    public class GetClientCardByIdResponse
    {
        public Campaign[] campaigns { get; set; }
        public Agent[] agents { get; set; }
        public SalesStatusText salesStatusText { get; set; }
        public Tradegroup[] tradeGroups { get; set; }
        public Accounttype[] accountTypes { get; set; }
        public object[] savingAccounts { get; set; }

#pragma warning disable CA1507 // Use nameof to express symbol names
        [JsonProperty("user")]
#pragma warning restore CA1507 // Use nameof to express symbol names
        public User user { get; set; }
        public string group { get; set; }
    }  

    public class User
    {
        public string _id { get; set; }
        public string country { get; set; }
        public string currency_code { get; set; }
        public string email { get; set; }
        public string first_name { get; set; }
        public string last_name { get; set; }
        public string phone { get; set; }
        public string phone_2 { get; set; }
        public string gmt_timezone { get; set; }
        public string role { get; set; }
        public string note { get; set; }
        public DateTime created_at { get; set; }
        public string sales_status { get; set; }
        public object saving_account_id { get; set; }
        public string account_type_id { get; set; }
        public object last_login { get; set; }
        public object last_logout { get; set; }
        public bool active { get; set; }
        public DateTime? attribution_date { get; set; }
        public double balance { get; set; }
        public double sa_balance { get; set; }
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
        public string sales_agent { get; set; }
        public string cfd_group_id { get; set; }
        public string campaign_id { get; set; }
        public string available { get; set; }
        public int bonus { get; set; }
        public string demo_balance { get; set; }
        public double equity { get; set; }
        public string open_pnl { get; set; }
        public string min_margin { get; set; }
        public object margin_usage { get; set; }
        public string pnl { get; set; }
        public string pnl_real { get; set; }
        public string pnl_bonus { get; set; }
        public bool online { get; set; }
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
    }
}
