using System.Collections.Generic;
using AirSoftAutomationFramework.Objects.DTOs;
using OpenQA.Selenium;

namespace AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.Clients.ClientCard
{
    public interface ITimeLineTabApi
    {
        T ChangeContext<T>(IWebDriver driver) where T : class;
        List<GetTimelineDetails>
            GetTimelineRequest(string url, string clientId, string apiKey = null);
    }
}