using System.Collections.Generic;
using AirSoftAutomationFramework.Objects.DTOs;
using OpenQA.Selenium;

namespace AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.Settings
{
    public interface IPspTabApi
    {
        T ChangeContext<T>(IWebDriver driver) where T : class;
        string GetPspIdByNameFromMongo(string pspName);
        List<GetPspInstanceResponse> GetPspInstancesByNameRequest(string pspName);
        List<GetPspInstanceResponse> PostCreateAirsoftSandboxPspRequest(string url);
        IPspTabApi PostCreateAuthorizePspRequest(string url);
        IPspTabApi PostCreateBankTransferPspRequest(string url);
        IPspTabApi PostPaymentNotificationRequestPipe(string url, string cientId, double amount, string currency, string status = "APPROVED");
    }
}