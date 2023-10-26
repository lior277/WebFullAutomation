﻿namespace AirSoftAutomationFramework.Objects.DTOs
{
    public class GetRegulationResponse
    {
        public string _id { get; set; }
        public int chargeback_months_limit { get; set; }
        public bool delete_bonus { get; set; }
        public bool edit_free_text { get; set; }
        public bool edit_free_text2 { get; set; }
        public bool edit_free_text3 { get; set; }
        public bool edit_free_text4 { get; set; }
        public bool edit_free_text5 { get; set; }
        public bool delete_trades { get; set; }
        public bool edit_swap { get; set; }
        public bool mass_trading { get; set; }      
        public bool export_data { get; set; }
        public string[] export_data_email_url { get; set; }
        public bool show_closed_pnl { get; set; }
        public bool terms_conditions { get; set; }
        public string terms_conditions_url { get; set; }
        public bool withdrawal_by_wallet { get; set; }
        public bool withdrawal_title { get; set; }
        public Edit_Client_Profile edit_client_profile { get; set; }
        public bool reopen_trade { get; set; }
        public string[] admin_email_for_deposit { get; set; }

        public class Edit_Client_Profile
        {
            public bool first_name { get; set; }
            public bool last_name { get; set; }
            public bool client_export_activity { get; set; }
            public bool country { get; set; }
            public bool phone { get; set; }
            public bool show_available_to_withdrawal { get; set; }
            public bool show_client_name { get; set; }
            public bool show_doc_section { get; set; }
            public Edit_Doc_Parts edit_doc_parts { get; set; }
        }

        public class Edit_Doc_Parts
        {
            public bool proof_of_identity { get; set; }
            public bool proof_of_residency { get; set; }
            public bool credit_debit_card_documentation { get; set; }
            public bool general_dod { get; set; }
        }
    }
}
