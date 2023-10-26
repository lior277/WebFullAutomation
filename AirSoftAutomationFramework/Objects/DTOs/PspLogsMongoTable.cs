

using System;

namespace AirSoftAutomationFramework.Objects.DTOs
{
    public class PspLogsMongoTable
    {
        public DateTime creation_date { get; set; }
        public string order_id { get; set; }
        public string customer_id { get; set; }
        public string status { get; set; }
        public string psp_name { get; set; }
        public Create_Payment create_payment { get; set; }

        public class Create_Payment
        {
            public string date { get; set; }
            public bool test_mode { get; set; }
            public Request request { get; set; }
            public Response response { get; set; }
            public Response_To_Frontend response_to_frontend { get; set; }
        }

        public class Request
        {
            public string type { get; set; }
            public string url { get; set; }
            public Headers headers { get; set; }
            public Body body { get; set; }
            public Query_Params query_params { get; set; }
        }

        public class Headers
        {
            public string Authorization { get; set; }
        }

        public class Body
        {
            public string firstname { get; set; }
            public string lastname { get; set; }
            public string birthday { get; set; }
            public string address { get; set; }
            public string city { get; set; }
            public string zip { get; set; }
            public string country { get; set; }
            public string state { get; set; }
            public string phone { get; set; }
            public string email { get; set; }
            public string order_id { get; set; }
            public string currency { get; set; }
            public double amount { get; set; }
            public string buyer_ip_address { get; set; }
            public string user_id { get; set; }
            public string lang { get; set; }
        }

        public class Query_Params
        {
        }

        public class Response
        {
            public string redirect_url { get; set; }
        }

        public class Response_To_Frontend
        {
            public string type { get; set; }
            public string method { get; set; }
            public string action { get; set; }
            public string target { get; set; }
        }
    }
}
