// Ignore Spelling: Forex Api Usd Crm

using AirSoftAutomationFramework.Objects.DTOs;
using OpenQA.Selenium;
using System.Collections.Generic;
using System.Net.Http;

namespace AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.Settings
{
    public interface IGeneralTabApi
    {
        T ChangeContext<T>(IWebDriver driver = null) where T : class;
        string GetCountriesRequest(string url, string apiKey = null);
        HttpResponseMessage GetDownloadMainLogoFileRequest(string url, string mainLogoUrl);
        GetEditCompanyInformationResponse GetEditCompanyInformationRequest(string url);
        Dictionary<string, string> GetSalesStatus2Request(string url, string apiKey = null);
        List<string> GetSalesStatusFromApiDocRequest(string url, string apiKey = null);
        Dictionary<string, SaleStatusValues> GetSalesStatusRequest(string url, string apiKey = null);
        Dictionary<string, string> GetSettingBySectionNameRequest(string url, List<string> sectionNames);
        string GetSettingRequest(string url, string apiKey, bool checkStatusCode = true);
        GetSuspiciousProfit GetSuspiciousPnlRequest(string url);
        IGeneralTabApi PutEditCompanyInformationRequest(string url);
        IGeneralTabApi PutGeneralDodRequest(string url);
        IGeneralTabApi PutGeneralSettingsRequest(string url, GetRegulationResponse getRegulationResponse);
        IGeneralTabApi PutMarginCallRequest(string url);
        IGeneralTabApi PutMaximumDepositRequest(string url, int maxDepositUsd = 1000, int maxDepositEur = 1000, int maxDepositUsdT = 10000, string apiKey = null);
        IGeneralTabApi PutMinimumDepositRequest(string url, int minDepositUsd = 0, int minDepositEur = 0, int minDepositUsdT = 0, string apiKey = null);
        IGeneralTabApi PutRemindAboutDepositRequest(string url, double remindAboutDeposit = 0.001);
        string PutSalesStatus2Request(string url, Dictionary<string, string> salesStatusText, string newStatusName = null, string oldStatusName = null, bool active = true, string apiKey = null, bool checkStatusCode = true);
        string PutSalesStatusRequest(string url, Dictionary<string, SaleStatusValues> salesStatusText, string newStatusName = null, string oldStatusName = null, string apiKey = null, bool checkStatusCode = true);
        Dictionary<string, string> PutSettingBySectionNameRequest(string url, Dictionary<string, string> sectionNamesAndBodies, string apiKey, bool checkStatusCode = true);
        IGeneralTabApi PutSuspiciousPnlRequest(string url, int suspiciousPercentage = 4000, int blockUserPercentage = 1, string[] exportEmails = null, bool blockUser = false);
        IGeneralTabApi PutTermAndConditionRequest(string url, bool TermAndCondition = false);
    }
}