using OpenQA.Selenium;
using System.Collections.Generic;

namespace AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.Banking
{
    public interface IBonusPageApi
    {
        T ChangeContext<T>(IWebDriver driver) where T : class;
        string ExportBonusTablePipe(string url, string clientEmail, string userEmail, string apiKey = null, bool checkStatusCode = true);
    }
}