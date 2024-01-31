using AirSoftAutomationFramework.Objects.DTOs;
using OpenQA.Selenium;
using System.Collections.Generic;

namespace AirSoftAutomationFramework.Objects.ForexObjects.Api.Trade
{
    public interface IRiskPageApi
    {
        T ChangeContext<T>(IWebDriver driver) where T : class;
        GeneralResult<GetRiskFilterResult> GetRisksByFilterRequest(string url, Dictionary<string, string> filtersValues, string apiKey = null, bool checkStatusCode = true);
        string GetRisksRequest(string url, string apiKey, bool checkStatusCode = true);
        string PostExportRisksTablePipeRequest(string url, string clientEmail, string userEmail, string apiKey = null, bool checkStatusCode = true);
    }
}