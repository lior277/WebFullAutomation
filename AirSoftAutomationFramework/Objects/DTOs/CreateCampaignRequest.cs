namespace AirSoftAutomationFramework.Objects.DTOs
{
    public class CreateCampaignRequest
    {
        public string erp_user_id { get; set; }
        public Cap cap { get; set; }
        public string deal { get; set; }
        public string name { get; set; }
        public string currency { get; set; }
        public object[] blocked_countries { get; set; }
        public object account_type_id { get; set; }
        public int payment { get; set; }
        public bool accepting_leads_hours_active { get; set; }
        public string accepting_leads_hours_from { get; set; }
        public string accepting_leads_hours_to { get; set; }
        public string code { get; set; }
        public string compliance_status { get; set; }
        public Min_Deposit min_deposit { get; set; }

        public class Cap
        {
            public bool stop_traffic { get; set; }
            public bool send_email { get; set; }
            public Limitation[] limitations { get; set; }
            public string email { get; set; }
        }

        public class Limitation
        {
            public string country { get; set; }
            public string timeframe { get; set; }
            public int leads_num { get; set; }
        }

        public class Min_Deposit
        {
            public int USD { get; set; }
            public int EUR { get; set; }
            public int GBP { get; set; }
            public int CAD { get; set; }
            public int JPY { get; set; }
            public int CHF { get; set; }
            public int CNY { get; set; }
            public int RUB { get; set; }
            public int BTC { get; set; }
            public int USDT { get; set; }
        }

    }
}
