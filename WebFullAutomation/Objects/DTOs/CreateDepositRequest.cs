namespace AirSoftAutomationFramework.Objects.DTOs
{
    public class CreateDepositRequest
    {
        public string user_id { get; set; }
        public object psp_instance_id { get; set; }
        public string psp_transaction_id { get; set; }
        public string last_digits { get; set; }
        public string transaction_type { get; set; }
        public string method { get; set; }
        public string original_currency { get; set; }
        public int amount { get; set; }
        public string name_for_method { get; set; }
    }
}
