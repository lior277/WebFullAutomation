using Newtonsoft.Json;

namespace AirSoftAutomationFramework.Objects.DTOs
{
    public class GetAgentProfileInfoResponse
    {
        [JsonProperty("period")]
        public string Period { get; set; }

        [JsonProperty("customers")]
        public ClickToCall Customers { get; set; }

        [JsonProperty("deposits")]
        public ClickToCall Deposits { get; set; }

        [JsonProperty("ftd")]
        public ClickToCall Ftd { get; set; }

        [JsonProperty("conversion")]
        public ConversionForSalesAgent conversionForSalesAgent { get; set; }

        [JsonProperty("online")]
        public ClickToCall Online { get; set; }

        [JsonProperty("click_to_call")]
        public ClickToCall ClickToCall { get; set; }

        [JsonProperty("top_10_deposits")]
        public object[] Top10_Deposits { get; set; }

        [JsonProperty("top_10_pnl")]
        public object[] Top10_Pnl { get; set; }

        [JsonProperty("performance_by_goals")]
        public PerformanceByGoal[] PerformanceByGoals { get; set; }
    }

    public partial class ClickToCall
    {
        [JsonProperty("total")]
        public int Total { get; set; }

        [JsonProperty("diff")]
        public long Diff { get; set; }
    }

    public partial class ConversionForSalesAgent
    {
        [JsonProperty("total")]
        public long? Total { get; set; }

        [JsonProperty("diff")]
        public long Diff { get; set; }

        [JsonProperty("conversion_period")]
        public ConversionPeriod ConversionPeriod { get; set; }
    }

    public partial class ConversionPeriod
    {
        [JsonProperty("start_year")]
        public long StartYear { get; set; }

        [JsonProperty("end_year")]
        public long EndYear { get; set; }

        [JsonProperty("start_month")]
        public long StartMonth { get; set; }

        [JsonProperty("end_month")]
        public long EndMonth { get; set; }
    }

    public partial class PerformanceByGoal
    {
        [JsonProperty("year")]
        public long Year { get; set; }

        [JsonProperty("month")]
        public long Month { get; set; }

        [JsonProperty("performance")]
        public long Performance { get; set; }

        [JsonProperty("target")]
        public long Target { get; set; }
    }
}
