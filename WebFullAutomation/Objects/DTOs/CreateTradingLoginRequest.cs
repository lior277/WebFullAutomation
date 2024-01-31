namespace AirSoftAutomationFramework.Objects.DTOs
{
    public class CreateTradingLoginRequest
    {
        public string email { get; set; }
        public string password { get; set; }
        public string platform { get; set; }
        public string fingerprint { get; set; }
    }
}
