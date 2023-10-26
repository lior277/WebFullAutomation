using System;

namespace AirSoftAutomationFramework.Objects.DTOs
{
    public class GetUsersSavingAccounts
    {
        public Account[] account { get; set; }
        public string availableDeposit { get; set; }

        public class Account
        {
            public int id { get; set; }
            public string action_type { get; set; }
            public string user_id { get; set; }
            public DateTime created_at { get; set; }
            public int amount { get; set; }
            public int balance { get; set; }
            public string sa_id { get; set; }
            public int sa_percentage { get; set; }
        }

    }
}
