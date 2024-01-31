using System;

namespace AirSoftAutomationFramework.Objects.DTOs
{
    public class GetChatMessagesResponse
    {
        public string _id { get; set; }
        public string type { get; set; }
        public string language { get; set; }
        public bool active { get; set; }
        public string content { get; set; }
        public DateTime last_update { get; set; }
    }
}
