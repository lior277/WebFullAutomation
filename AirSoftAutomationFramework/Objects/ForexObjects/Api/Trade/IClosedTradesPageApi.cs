using OpenQA.Selenium;

namespace AirSoftAutomationFramework.Objects.ForexObjects.Api.Trade
{
    public interface IClosedTradesPageApi
    {
        T ChangeContext<T>(IWebDriver driver) where T : class;
        string ExportCloseTradeTablePipe(string url, string userEmail, string apiKey = null, bool checkStatusCode = true);
        string GetClosedTradesRequest(string url, string apiKey, bool checkStatusCode = true);
    }
}