using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace AirSoftAutomationFramework.Objects.DTOs
{
    public class GetClientByMailResponse
    {
#pragma warning disable CA1507 // Use nameof to express symbol names
        [JsonProperty("data")]
#pragma warning restore CA1507 // Use nameof to express symbol names
        public List<ClientByMail> data { get; set; }
        public string draw { get; set; }
        public int recordsFiltered { get; set; }
        public int recordsTotal { get; set; }
    }

    public class ClientByMail
    {
        public string _id { get; set; }
        public string country { get; set; }
        public string phone { get; set; }
        public string email { get; set; }
        public DateTime? created_at { get; set; }
        public string attribution_date { get; set; }
        public string balance { get; set; }
        public double sa_balance { get; set; }
        public DateTime? last_login { get; set; }
        public DateTime? last_logout { get; set; }
        public string total_deposit { get; set; }
        public string role { get; set; }
        public string margin_call_notified_date { get; set; }
        public string kyc_status { get; set; }
        public string kyc_proof_of_identity_status { get; set; }
        public string kyc_proof_of_residency_status { get; set; }
        public string kyc_credit_debit_card_documentation_status { get; set; }
        public string sales_status { get; set; }
        public object saving_account_id { get; set; }
        public double bonus { get; set; }
        public string activation_status { get; set; }
        public string currency_code { get; set; }
        public DateTime? last_trade { get; set; }
        public object last_comment { get; set; }
        public object last_call { get; set; }
        public DateTime? ftd_date { get; set; }
        public string full_name { get; set; }
        public string campaign { get; set; }
        public object[] comments { get; set; }
        public string sales_agent { get; set; }
        public string office { get; set; }
        public string total_bonus { get; set; }
        public bool online { get; set; }
    }
}
