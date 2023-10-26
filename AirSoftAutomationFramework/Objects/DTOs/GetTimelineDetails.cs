using System;

namespace AirSoftAutomationFramework.Objects.DTOs
{
    public class GetTimelineDetails
    {
        public string _id { get; set; }
        public DateTime created_at { get; set; }
        public string user_id { get; set; }
        public string user_full_name { get; set; }
        public double amount { get; set; }
        public string currency { get; set; }
        public string ip { get; set; }
        public string erp_user_id { get; set; }
        public string erp_username { get; set; }
        public string type { get; set; }
        public string action_made_by { get; set; }
        public string action_made_by_user_id { get; set; }
        public string from_os { get; set; }
        public string from_device { get; set; }
        public string from_browser { get; set; }
        public int bonus_amount { get; set; }
        public int transaction_id { get; set; }
        public string email_type { get; set; }
        public string email_content { get; set; }
        public string email_title { get; set; }
        public string email_sent_from { get; set; }
        public double original_amount { get; set; }
        public string original_currency { get; set; }
        public string erp_user_id_assigned { get; set; }
        public string transaction_type { get; set; }
        public string status { get; set; }
        public string registered_from { get; set; }
        public string platform { get; set; }
        public DateTime? logout_date { get; set; }
    }
}
