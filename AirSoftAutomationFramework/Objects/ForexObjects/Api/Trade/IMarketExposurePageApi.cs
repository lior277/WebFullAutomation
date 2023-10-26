using OpenQA.Selenium;

namespace AirSoftAutomationFramework.Objects.ForexObjects.Api.Trade
{
    public interface IMarketExposurePageApi
    {
        T ChangeContext<T>(IWebDriver driver) where T : class;
        string GetMarketExposureRequest(string url, string apiKey, bool checkStatusCode = true);
    }
}