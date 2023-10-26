using AirSoftAutomationFramework.Objects.DTOs;
using OpenQA.Selenium;

namespace AirSoftAutomationFramework.Objects.ForexObjects.ApiGeneral
{
    public interface IBroadcastMessageApi
    {
        T ChangeContext<T>(IWebDriver driver) where T : class;
        string CrmPostCreateBroadcastMessageRequest(string uri, string apiKey = null, bool checkStatusCode = true);
        IBroadcastMessageApi MgmPostCreateBroadcastMessageRequest(string uri, GetLoginResponse loginData);
        IBroadcastMessageApi PatchAllMessagesStatusRequest(string uri);
    }
}