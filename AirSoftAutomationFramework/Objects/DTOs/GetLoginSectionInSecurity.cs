
namespace AirSoftAutomationFramework.Objects.DTOs
{
    public class GetLoginSectionInSecurity
    {
        public string _id { get; set; }
        public bool two_factor { get; set; }
        public int attempts { get; set; }
        public bool disable_ip_verification { get; set; }
        public int trade_attempts { get; set; }
    }
}
