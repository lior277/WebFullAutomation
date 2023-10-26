using Newtonsoft.Json;

namespace AirSoftAutomationFramework.Objects.DTOs
{
    public class GetBrandsActivitiesResponse
    {
        [JsonProperty("crm-qa-dev-auto")]
        public CrmQaDevAuto crmQaDevAuto { get; set; }

        public class CrmQaDevAuto
        {
            public int net_deposits { get; set; }
            public int deposits { get; set; }
            public int ftds { get; set; }
            public int deposits_count { get; set; }
            public int withdrawals { get; set; }
            public int other_withdrawals { get; set; }
            public int chargebacks { get; set; }
            public int bonuses { get; set; }
            public double pnl { get; set; }
            public int open_pnl { get; set; }
            public string group { get; set; }
            public string platform { get; set; }
        }
    }
}
