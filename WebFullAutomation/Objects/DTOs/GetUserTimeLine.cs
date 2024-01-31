using Newtonsoft.Json;
using System;

namespace AirSoftAutomationFramework.Objects.DTOs
{
    public class GetUserTimeLine
    {
        [JsonProperty("Class1")]
        public UserTimeLine[] Property1 { get; set; }

        public class UserTimeLine
        {
            public string _id { get; set; }
            public string old_sales_agent_name { get; set; }
            public string new_sales_agent_name { get; set; }
            public string old_sales_agent_id { get; set; }
            public string new_sales_agent_id { get; set; }
            public string created_at { get; set; }
            public string user_id { get; set; }
            public string user_full_name { get; set; }
            public string erp_user_id { get; set; }
            public string erp_username { get; set; }
            public string ip { get; set; }
            public string type { get; set; }
            public string action_made_by { get; set; }
            public string action_made_by_user_id { get; set; }
            public string from_os { get; set; }
            public string from_device { get; set; }
            public string from_browser { get; set; }
            public string platform { get; set; }
            public DateTime logout_date { get; set; }
            public DateTime last_update { get; set; }
            public string email_type { get; set; }
            public string email_content { get; set; }
            public string email_title { get; set; }
            public string email_sent_from { get; set; }
            public string registered_from { get; set; }
        }

    }
}
