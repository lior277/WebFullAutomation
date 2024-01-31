using AirSoftAutomationFramework.Objects.DTOs;
using OpenQA.Selenium;
using System.Collections.Generic;

namespace AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.Settings
{
    public interface ISecurityTubApi
    {
        T ChangeContext<T>(IWebDriver driver = null) where T : class;
        ISecurityTubApi DeleteReleaseBlockIpUserRequest(string url, string userId);
        List<GetBlockedIpsResponse> GetBlockIpsRequest(string url);
        List<GetBlockUsersResponse> GetBlockUsersRequest(string url);
        ISecurityTubApi WaitForUserToGetBlocked(string url, string userId);
        GetLoginSectionInSecurity GetLoginSectionRequest(string url);
        ISecurityTubApi PatchReleaseBlockUserRequest(string url, string userId, string apiKey = null);
        ISecurityTubApi PutLoginSectionRequest(string url, GetLoginSectionInSecurity getLoginSectionInSecurity);
        SecurityTubApi PutRecaptchaRequest(string url);
    }
}