// Ignore Spelling: Dto erp Psp ips username

using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

namespace AirSoftAutomationFramework.Objects.DTOs
{
    public class GeneralDto
    {
        [JsonProperty("trade_id")]
        public string TradeId { get; set; }

        [JsonProperty("client_id")]
        public string ClientId { get; set; }
        public Trading trading { get; set; }
        public Erp erp { get; set; }

        public List<TransactionByCampaignResponse> TransactionByCampaignResponse { get; set; }

        [JsonProperty("failed_to_mass_assign_to_campaign")]
        public string[] FailedToMassAssignToCampaign { get; set; }

        [JsonProperty("nModified")]
        public int NModified { get; set; }

        [JsonProperty("psp_name")]
        public string PspName { get; set; }

        [JsonProperty("massTradeId")]
        public string MassTradeId { get; set; }

        [JsonProperty("label")]
        public List<string> Label { get; set; }

        [JsonProperty("converted")]
        public string Converted { get; set; }

        [JsonProperty("path")]
        public string Path { get; set; }

        [JsonProperty("redirectUrl")]
        public string RedirectUrl { get; set; }

        [JsonProperty("rate")]
        public double TradeRate { get; set; }

        [JsonProperty("link")]
        public string LoginLink { get; set; }

        [JsonProperty("answer")]
        public string Answer { get; set; }

        [JsonProperty("question")]
        public string Question { get; set; }

        [JsonProperty("feed_message_id")]
        public string FeedMessageId { get; set; }

        [JsonProperty("message")]
        public string Message { get; set; }

        [JsonProperty("affectedRows")]
        public long AffectedRows { get; set; }

        [JsonProperty("columnName")]
        public string ColumnName { get; set; }

        [JsonProperty("attempts")]
        public int Attempts { get; set; }

        [JsonProperty("trade_attempts")]
        public int TradeAttempts { get; set; }

        [JsonProperty("visibility")]
        public bool Visibility { get; set; }

        [JsonProperty("insertId")]
        public string InsertId { get; set; }

        public List<CommentResponse> commentResponse { get; set; }

        [JsonProperty("order_id")]
        public string OrderId { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }

        public List<string> first_name { get; set; }

        public List<string> last_name { get; set; }

        public List<string> email { get; set; }

        public List<string> phone { get; set; }

        public List<string> extension { get; set; }

        public List<string> username { get; set; }

        [JsonProperty("amount")]
        public string[] Amount { get; set; }

        [JsonProperty("language")]
        public string Language { get; set; }

        [JsonProperty("active")]
        public bool Active { get; set; }

        [JsonProperty("token")]
        public string Token { get; set; }

        [JsonProperty("_id")]
        public string Id { get; set; }


        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("names")]
        public string[] Names { get; set; }


        [JsonProperty("last_update")]
        public string LastUpdate { get; set; }
    }

    public class Erp
    {
        public Countries[] countries { get; set; }
        public object[] ips { get; set; }
        public string url { get; set; }
    }

    public class GeneralResult<T>
    {
        public T GeneralResponse { get; set; }
        public List<T> GeneralResponseCollection { get; set; }
        public string Message { get; set; }
        public Stream Stream { get; set; }
        public int Value { get; set; }
    }

    public class Trading
    {
        public Countries[] countries { get; set; }
        public object[] ips { get; set; }
        public string url { get; set; }
    }

    public class Countries
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("itemName")]
        public string ItemName { get; set; }

        [JsonProperty("selected")]
        public object Selected { get; set; }
    }

    public class TransactionByCampaignResponse
    {
        [JsonProperty("user_id")]
        public string UserId { get; set; }

        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("original_currency")]
        public string OriginalCurrency { get; set; }

        [JsonProperty("psp_transaction_id")]
        public string PspTransactionId { get; set; }

        [JsonProperty("original_amount")]
        public string OriginalAmount { get; set; }

        [JsonProperty("free_text")]
        public string FreeText { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("action")]
        public string Action { get; set; }

        [JsonProperty("country")]
        public string Country { get; set; }
    }
    public class CommentResponse
    {
        [JsonProperty("user_id")]
        public string UserId { get; set; }

        [JsonProperty("comment")]
        public string Comment { get; set; }

        [JsonProperty("created_at")]
        public string CreatedAt { get; set; }

        [JsonProperty("erp_user_id")]
        public string ErpUserId { get; set; }

        [JsonProperty("date")]
        public object Date { get; set; }

        [JsonProperty("erp_username")]
        public string ErpUserName { get; set; }
    }
}
