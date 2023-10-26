using AirSoftAutomationFramework.Objects.DTOs;
using OpenQA.Selenium;

namespace AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.Settings
{
    public interface ISuperAdminTubApi
    {
        T ChangeContext<T>(IWebDriver driver = null) where T : class;
        ISuperAdminTubApi DeleteGroupNamesRequest(string url);
        GetRegulationResponse GetBrandRegulationRequest(string url);
        GeneralDto GetBrandRestrictionRequest(string url);
        GetRegulationResponse GetRegulationRequest(string url);
        GetCurrenciesResponse GetCurrenciesRequest(string url);
        GetGlobalSettingsResponse GetGlobalSettingsRequest(string url);
        GeneralDto GetGroupNamesRequest(string url);
        GroupRestrictionsForex GetRestrictionsRequest(string uri);
        ISuperAdminTubApi PutBrandRestrictionRequest(string url, string redirectedUrl, int? countryId = null, string countryName = null);
        ISuperAdminTubApi PutChatRequest(string url);
        ISuperAdminTubApi PutCurrenciesRequest(string url);
        ISuperAdminTubApi PutGroupNamesRequest(string url, string[] groupNames);
        ISuperAdminTubApi PutGroupRestrictionsRequest(string url, object group = null, int actualMinus = -1, int actualPlus = 1);
        ISuperAdminTubApi PutPlatformRequest(string url);
        ISuperAdminTubApi PutRegulationsRequest(string url, GetRegulationResponse getRegulationResponse);
        ISuperAdminTubApi PutRiskRestrictionsRequest(string url, int maxGroupNumber = 1000, bool allowGroupTitleChange = true, bool allowSpreadChange = true);
        ISuperAdminTubApi UpdateGroupRestrictionsTable(string ConfigName, int minus, int plus);
    }
}