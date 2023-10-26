using System;
using System.Collections.Generic;

namespace AirSoftAutomationFramework.Objects.DTOs
{
    public class GetGlobalEventsResponse
    {
        public string _id { get; set; }
        public DateTime created_at { get; set; }
        public string erp_user_id { get; set; }
        public string erp_username { get; set; }
        public string erp_user_full_name { get; set; }
        public string user_full_name { get; set; }
        public string table { get; set; }
        public List<string> usernames { get; set; }
         
        public string ip { get; set; }
        public string type { get; set; }
        public string action_made_by { get; set; }
        public string action_made_by_user_id { get; set; }
        public string from_os { get; set; }
        public string from_device { get; set; }
        public string from_browser { get; set; }
        public string user_id { get; set; }
        public string trade_group_id { get; set; }
        public string trade_group_name { get; set; }
        public string saving_account_id { get; set; }
        public string chat_message_id { get; set; }
        public string chat_message_type { get; set; }
        public string saving_account_name { get; set; }
        public string account_type_id { get; set; }
        public string account_type_name { get; set; }
        public string campaign_id { get; set; }
        public string campaign_name { get; set; }
        public string sales_status_name { get; set; }
        public string role_id { get; set; }
        public string role_name { get; set; }
        public string banner_id { get; set; }
        public string banner_name { get; set; }
        public string office_id { get; set; }
        public string office_name { get; set; }
        public string email_name { get; set; }
        public bool global { get; set; }
    }
}
