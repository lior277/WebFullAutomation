

using System.Net;

namespace AirSoftAutomationFramework.Objects.DTOs
{
    public class GetLoginResponse
    {
        public Cookie token { get; set; }
        public Cookie token2 { get; set; }
        public Cookie token3 { get; set; }
        public string shsec { get; set; }
    }
}
