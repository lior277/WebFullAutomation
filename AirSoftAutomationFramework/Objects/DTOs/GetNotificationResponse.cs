using System;

namespace AirSoftAutomationFramework.Objects.DTOs
{
    public class GetNotificationResponse
    {

        public string _id { get; set; }
        public string type { get; set; }
        public Body body { get; set; }
        public string seller_id { get; set; }
        public DateTime created_at { get; set; }
        public bool responded { get; set; }
        public bool read { get; set; }
    }

    public class Body
    {
        public string id { get; set; }
        public string fullName { get; set; }
        public int amount { get; set; }
        public string currency { get; set; }
        public string platform { get; set; }
        public int tradeId { get; set; }
        public string erpUsername { get; set; }
        public string newStatus { get; set; }
        public DateTime date { get; set; }
        public string username { get; set; }
    }
}
