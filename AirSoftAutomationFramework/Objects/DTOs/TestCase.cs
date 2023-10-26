using System.Collections.Generic;

namespace AirSoftAutomationFramework.Objects.DTOs
{
    //[BsonIgnoreExtraElements]
    public class TestCase
    {
        public string _id { get; set; }
        public int case_id { get; set; }
        public string test_name { get; set; }
        public List<Steps> steps { get; set; }
        public ExpectedFinanceData expected_finance_data { get; set; }
        public List<double> account_value_values { get; set; }
        public List<int> is_deleted_values { get; set; }
        public List<int> is_chargeback_values { get; set; }
        public List<double> balance { get; set; }
        public List<string> type { get; set; }
        public List<string> status { get; set; }
        public List<double> amount { get; set; }    

        public class Steps
        {
            public int case_id { get; set; }
            public string test_name { get; set; }
            //public int case_order_id { get; set; }
            public string action { get; set; }
            public int deposit_amount { get; set; }
            public int delete_deposit_amount { get; set; }
            public int bonus_amount { get; set; }
            public int delete_bonus_amount { get; set; }
            public int withdrawal_amount { get; set; }
            public int withdrawal_bonus_amount { get; set; }
            public int withdrawal_split_amount { get; set; }
            public string withdrawal_status { get; set; }
            public List<string> statuses_ofsplit_withdrawals { get; set; }
            public int chargeback_amount { get; set; }
            public int transfer_sa_amount { get; set; }
            public int transfer_balance_amount { get; set; }
            public int trade_amount { get; set; }
            public int pnl_amount { get; set; }
            public string asset_symbol { get; set; }
            public float spred { get; set; }
            public string transaction_type { get; set; }
            public string trade_status { get; set; }
            public double current_rate { get; set; }
            public ExpectedFinanceData expected_finance_data { get; set; }
        }

        public class ExpectedFinanceData
        {
            public double? balance { get; set; }
            public double? equity { get; set; }
            public double? available { get; set; }
            public double? min_margin { get; set; }
            public double? bonus { get; set; }
        }
    }
}