namespace AirSoftAutomationFramework.Objects.DTOs
{
    public class CreateBroadcastRequest
    {
        public Brand[] brands { get; set; }
        public string[] list_of_all_roles { get; set; }
        public string[] target_roles { get; set; }
        public Message message { get; set; }


        public class Message
        {
            public string subject { get; set; }
            public string body { get; set; }
        }

        public class Brand
        {
            public string _id { get; set; }
            public string name { get; set; }
        }

    }
}
