using AirSoftAutomationFramework.Objects.ForexObjects.TradingPlatform.TradeObjects.TradeApi;
using OpenQA.Selenium;

namespace AirSoftAutomationFramework.Objects.ForexObjects.Api.Trade
{
    public interface IPendingTradesPageApi
    {
        T ChangeContext<T>(IWebDriver driver) where T : class;
        IPendingTradesPageApi ExportPendingTradeTablePipe(string url, string userEmail, string apiKey = null);
        string GetPendingTradesRequest(string url, string apiKey, bool checkStatusCode = true);
    }
}