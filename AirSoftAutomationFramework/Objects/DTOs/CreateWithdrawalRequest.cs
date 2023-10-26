namespace AirSoftAutomationFramework.Objects.DTOs
{
    public class CreateWithdrawalRequest
    {
        public int amount { get; set; }
        public string credit_card_owner { get; set; }
        public string last_digits { get; set; }
        public string exp_date { get; set; }
    }
}
