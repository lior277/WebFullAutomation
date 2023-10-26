namespace AirSoftAutomationFramework.Objects.DTOs
{
    public class CreatePaymentRequest
    {
        public string _id { get; set; }
        public string instance_id { get; set; }
        public Buyer buyer { get; set; }
        public double amount { get; set; }
    }

    public class Buyer
    {
        public string firstname { get; set; }
        public string lastname { get; set; }
        public string birthday { get; set; }
        public string address { get; set; }
        public string city { get; set; }
        public string zip { get; set; }
        public string country { get; set; }
        public string state { get; set; }
        public string phone { get; set; }
        public string email { get; set; }
        public bool update_profile { get; set; }
    }
}
