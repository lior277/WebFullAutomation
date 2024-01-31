using static AirSoftAutomationFramework.Objects.DTOs.CreateOfficeRequest;

namespace AirSoftAutomationFramework.Objects.DTOs
{
    public class PutOfficeRequest
    {
        public string city { get; set; }
        public string[] allowed_ip_addresses { get; set; }
        public string gmt_timezone { get; set; }
        public Sales_Dashboard sales_dashboard { get; set; }
        public Working_Hours working_hours { get; set; }

        //public class Sales_Dashboard
        //{
        //    public bool active { get; set; }
        //    public int price { get; set; }
        //}

        //public class Working_Hours
        //{
        //    public bool active { get; set; }
        //    public bool block_user { get; set; }
        //    public string[] send_alert_to { get; set; }
        //    public Monday monday { get; set; }
        //    public Tuesday tuesday { get; set; }
        //    public Wednesday wednesday { get; set; }
        //    public Thursday thursday { get; set; }
        //    public Friday friday { get; set; }
        //    public Saturday saturday { get; set; }
        //    public Sunday sunday { get; set; }
        //}

        //public class Monday
        //{
        //    public string from { get; set; }
        //    public string to { get; set; }
        //}

        //public class Tuesday
        //{
        //    public string from { get; set; }
        //    public string to { get; set; }
        //}

        //public class Wednesday
        //{
        //    public string from { get; set; }
        //    public string to { get; set; }
        //}

        //public class Thursday
        //{
        //    public string from { get; set; }
        //    public string to { get; set; }
        //}

        //public class Friday
        //{
        //    public string from { get; set; }
        //    public string to { get; set; }
        //}

        //public class Saturday
        //{
        //    public string from { get; set; }
        //    public string to { get; set; }
        //}

        //public class Sunday
        //{
        //    public string from { get; set; }
        //    public string to { get; set; }
        //}

    }
}
