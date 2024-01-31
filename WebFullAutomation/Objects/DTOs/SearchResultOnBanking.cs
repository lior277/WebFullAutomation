namespace AirSoftAutomationFramework.Objects.DTOs
{
    public class SearchResultOnBanking
    {
        public Data[] data { get; set; }
        public int recordsFiltered { get; set; }
        public int recordsTotal { get; set; }
        public int totalFtds { get; set; }
        public string totalEuro { get; set; }
        public int otherEuro { get; set; }
    }

    public class Data
    {
        public string title { get; set; }
        public object transaction_id { get; set; }
        public string id { get; set; }
        public object order_id { get; set; }
        public object reject_reason { get; set; }
        public string user_id { get; set; }
        public string client_full_name { get; set; }
        public string phone { get; set; }
        public string country { get; set; }
        public string email { get; set; }
        public string comments { get; set; }
        public string ftd { get; set; }
        public string amount { get; set; }
        public string currency { get; set; }
        public string original_amount { get; set; }
        public string original_currency { get; set; }
        public object last_digits { get; set; }
        public string created_at { get; set; }
        public string updated_at { get; set; }
        public string campaign { get; set; }
        public string erp_username { get; set; }
        public string erp_full_name { get; set; }
        public string erp_assigned { get; set; }
        public string erp_user_id_assigned { get; set; }
        public string psp { get; set; }
        public string office_id { get; set; }
        public string office_city { get; set; }
        public string type { get; set; }
        public object note { get; set; }
        public string status { get; set; }
        public string balance { get; set; }
        public string total_deposit { get; set; }
        public string eur_fixed_amount { get; set; }
        public string withdrawal_proceed_date { get; set; }
        public string deposit_details { get; set; }
    }
}
