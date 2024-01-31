using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace AirSoftAutomationFramework.Objects.DTOs
{
    public class GetUsersResponse
    {
        [JsonProperty(PropertyName = "data")]
        public List<UserData> userData { get; set; }
    }

    public class UserData
    {
        public string _id { get; set; }
        public string first_name { get; set; }
        public string last_name { get; set; }
        public string username { get; set; }
        public object last_login { get; set; }
        public string email { get; set; }
        public string role { get; set; }
        public bool affiliate { get; set; }
        public string gmt_timezone { get; set; }
        public DateTime created_at { get; set; }
        public string office { get; set; }
    }
}
