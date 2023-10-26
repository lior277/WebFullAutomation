using System.Collections.Generic;
using OpenQA.Selenium;

namespace AirSoftAutomationFramework.Objects.ForexObjects.ApiGeneral
{
    public interface INotificationsApi
    {
        T ChangeContext<T>(IWebDriver driver) where T : class;
        List<string> GetNotificationRequest(string url, string apiKey, int expectedNumOfNotification);
    }
}