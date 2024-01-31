using AirSoftAutomationFramework.Objects.ForexObjects.TradingPlatform.TradeObjects.TradeApi;
using OpenQA.Selenium;
using System.Collections.Generic;

namespace AirSoftAutomationFramework.Objects.ForexObjects.Api.Trade
{
    public interface IOpenTradesPageApi
    {
        T ChangeContext<T>(IWebDriver driver) where T : class;
        IOpenTradesPageApi ExportOpenTradeTablePipe(string url, string userEmail, string apiKey = null);
        string GetOpenTradesRequest(string url, string apiKey, bool checkStatusCode = true);
        IOpenTradesPageApi PatchCloseTradeRequest(string url, List<string> tradeIds, string apiKey = null);
        IOpenTradesPageApi PatchCloseTradeRequest(string url, string tradeId, string apiKey = null);
    }
}