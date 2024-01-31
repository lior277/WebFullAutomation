using System;

namespace AirSoftAutomationFramework.Objects.DTOs
{
    public class GetBlockUsersResponse
    {
            public string _id { get; set; }
            public string first_name { get; set; }
            public string last_name { get; set; }
            public string platform { get; set; }
            public int login_attempts { get; set; }
            public DateTime block_date { get; set; }
    }
}
