using OpenQA.Selenium;

namespace AirSoftAutomationFramework.Objects.ForexObjects.Api.Trade
{
    public interface IHourlyPnlPageApi
    {
        T ChangeContext<T>(IWebDriver driver) where T : class;
        string GetHourlyPnlRequest(string url, string apiKey, bool checkStatusCode = true);
    }
}