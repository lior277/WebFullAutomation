namespace AirSoftAutomationFramework.Objects.DTOs
{
    public class ClientEmails
    {
        public Email[] email { get; set; }
    }

    public class Email
    {
        public string Body { get; set; }
        public string Date { get; set; }
        public string From { get; set; }
        public string Subject { get; set; }
        public string To { get; set; }
    }
}
