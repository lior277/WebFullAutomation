using OpenQA.Selenium;

namespace AirSoftAutomationFramework.Objects.ForexObjects.Api.Trade
{
    public interface IAssetsOnFrontPageApi
    {
        T ChangeContext<T>(IWebDriver driver) where T : class;
        string GetAssetsOnFrontRequest(string url, string apiKey, bool checkStatusCode = true);
    }
}