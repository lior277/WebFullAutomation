using System;

namespace AirSoftAutomationFramework.Objects.DTOs
{
    public class GetCouponsResponse
    {
        public string _id { get; set; }
        public string name { get; set; }
        public string type { get; set; }
        public int amount { get; set; }
        public bool active { get; set; }
        public bool allow_double_discounts { get; set; }
        public int minimum_spend { get; set; }
        public int limit_per_coupon { get; set; }
        public int limit_per_user { get; set; }
        public string code { get; set; }
        public DateTime created_at { get; set; }
        public object start_date { get; set; }
        public object end_date { get; set; }
        public int used_coupons { get; set; }
        public DateTime last_update { get; set; }
        public int used { get; set; }
        public int total_orders { get; set; }
    }
}
