using AirSoftAutomationFramework.Objects.DTOs;
using OpenQA.Selenium;
using System.Collections.Generic;

namespace AirSoftAutomationFramework.Objects.ForexObjects.Api.Trade
{
    public interface IBulkTradePageApi
    {
        T ChangeContext<T>(IWebDriver driver) where T : class;
        GeneralResult<SearchResultBulkTradeHistory> GetBulkTradesRequest(string url, string apiKey = null, bool checkStatusCode = true);
        List<GetMassTradeByIdRequest> GetMassTradeByIdRequest(string url, string MassTradeId);
        string PatchCloseBulkTradeRequest(string url, string bulkTradeId, string apiKey = null, bool checkStatusCode = true);
        string PatchEditBulkTradeRequest(string url, string bulkTradeId, string apiKey = null, bool checkStatusCode = true);
        string PostCreateBulkTradeRequest(string url, string[] user_ids, string assetSymbol = "ETHUSD", string transactionType = "buy", string exposure = "100", int takeProfit = 0, int stopLoss = 0, double rate = 0, string marketLimit = "", string apiKey = null, bool checkStatusCode = true);
    }
}