using System.Collections.Generic;
using AirSoftAutomationFramework.Objects.DTOs;
using OpenQA.Selenium;

namespace AirSoftAutomationFramework.Objects.ForexObjects.ApiGeneral
{
    public interface IGlobalEventsApi
    {
        T ChangeContext<T>(IWebDriver driver) where T : class;
        List<GetGlobalEventsResponse> GetGlobalEventsByUserRequest(string url,
              string userId, string apiKey = null);
        string GetGlobalEventsRequest(string url, string apiKey, bool checkStatusCode = true);
    }
}