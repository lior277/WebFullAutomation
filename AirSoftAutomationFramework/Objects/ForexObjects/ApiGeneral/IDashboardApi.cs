// Ignore Spelling: Api Forex

using AirSoftAutomationFramework.Objects.DTOs;
using OpenQA.Selenium;
using System.Collections.Generic;
using static AirSoftAutomationFramework.Objects.DTOs.GetUserTimeLine;

namespace AirSoftAutomationFramework.Objects.ForexObjects.ApiGeneral
{
    public interface IDashboardApi
    {
        T ChangeContext<T>(IWebDriver driver) where T : class;
        string GetActiveCampaignsRequest(string uri, string apiKey, bool checkStatusCode = true);
        GetBoxesStatisticsResponse GetBoxesStatisticsRequest(string uri, string apiKey = null);
        string GetBoxesStatisticsRequest(string uri, string apiKey, bool checkStatusCode = true);
        string GetCalendarRequest(string uri, string apiKey, bool checkStatusCode = true);
        string GetDepositsRequest(string uri, string apiKey, bool checkStatusCode = true);
        string GetLastRegistrationRequest(string uri, string apiKey, bool checkStatusCode = true);
        string GetSalesPerformanceRequest(string uri, string apiKey, bool checkStatusCode = true);
        List<UserTimeLine> GetUserTimeLineRequest(string uri, Dictionary<string, object> urlParams, string apiKey, bool checkStatusCode = true);
    }
}