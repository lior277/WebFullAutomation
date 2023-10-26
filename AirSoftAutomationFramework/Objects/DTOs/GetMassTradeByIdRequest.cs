namespace AirSoftAutomationFramework.Objects.DTOs
{
    public class GetMassTradeByIdRequest
    {
        public int id { get; set; }
        public int mass_trade_id { get; set; }
        public string error { get; set; }
        public int success { get; set; }
        public string user_id { get; set; }
        public string full_name { get; set; }
    }
}
