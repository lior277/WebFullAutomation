using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using static AirSoftAutomationFramework.Objects.DTOs.CreateCampaignRequest;

namespace AirSoftAutomationFramework.Objects.DTOs
{
    public class GetCampaignByIdResponse
    {
        [JsonProperty("_id")]
        public string Id { get; set; }

        [JsonProperty("erp_user_id")]
        public string ErpUserId { get; set; }

        [JsonProperty("deal")]
        public string Deal { get; set; }
        public Cap cap { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("currency")]
        public string Currency { get; set; }

        [JsonProperty("accepting_leads_hours_active")]
        public bool AcceptingLeadsHoursActive { get; set; }

        [JsonProperty("accepting_leads_hours_from")]
        public string AcceptingLeadsHoursFrom { get; set; }

        [JsonProperty("accepting_leads_hours_to")]
        public string AcceptingLeadsHoursTo { get; set; }

        [JsonProperty("blocked_countries")]
        public object[] BlockedCountries { get; set; }

        [JsonProperty("account_type_id")]
        public object AccountTypeId { get; set; }

        [JsonProperty("payment")]
        public long Payment { get; set; }

        [JsonProperty("code")]
        public string Code { get; set; }

        [JsonProperty("compliance_status")]
        public string ComplianceStatus { get; set; }

        [JsonProperty("min_deposit")]
        public Dictionary<string, long> MinDeposit { get; set; }

        [JsonProperty("created_at")]
        public DateTimeOffset CreatedAt { get; set; }

        [JsonProperty("register_pixel")]
        public string RegisterPixel { get; set; }

        [JsonProperty("accepted_deposit_pixel")]
        public string AcceptedDepositPixel { get; set; }

        [JsonProperty("last_update")]
        public DateTimeOffset LastUpdate { get; set; }

        [JsonProperty("registrations")]
        public Registrations Registrations { get; set; }

        [JsonProperty("active")]
        public bool Active { get; set; }
    }

    public class Cap
    {
        public bool stop_traffic { get; set; }
        public bool send_email { get; set; }
        public List<Limitation> limitations { get; set; }
        public string email { get; set; }
    }

    public class Limitation
    {
        public string country { get; set; }
        public string timeframe { get; set; }
        public int leads_num { get; set; }
    }

    public partial class Registrations
    {
        [JsonProperty("2021-07-22")]
        public long The20210722 { get; set; }
    }
}
