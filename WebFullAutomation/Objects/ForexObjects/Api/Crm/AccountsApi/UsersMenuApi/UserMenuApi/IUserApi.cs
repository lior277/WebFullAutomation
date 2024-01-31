// Ignore Spelling: Forex Api

using AirSoftAutomationFramework.Objects.DTOs;
using OpenQA.Selenium;
using System.Collections.Generic;

namespace AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.AccountsApi.UsersMenuApi.UserMenuApi
{
    public interface IUserApi
    {
        T ChangeContext<T>(IWebDriver driver) where T : class;
        string CreateUserForUiPipe(string url, string userName, string phone = null, string country = null, string emailPrefix = null, string password = null, string role = "admin", string pbxName = null, bool affiliate = false, bool active = true, bool addAsChild = false, string[] subUsers = null, string salesType = "retention", string salesAgentSalaryId = null, GetOfficeResponse officeData = null, string apiKey = null);
        IUserApi CreateUserIfNotExistByNamePipe(string url, string userName, string mailPrefix = null);
        string GetAllowedFingerPrintFromMongo(string email);
        string GetPendingFingerPrintFromMongo(string email);
        GetUsersResponse GetUsersRequest(string url, bool checkStatusCode = true);
        IUserApi PatchAddFingerPrintPipe(string url, string email);
        string PostCreateWooCommerceUserRequest(string url);

        IUserApi PatchDeleteOrRestoreUserRequest(string url, string userId, string apiKey = null);
        string PostCreateApiKeyRequest(string uri, string userId, string apiKey = null, bool checkStatusCode = true);
        List<string> PostCreateUserRequest(string url, List<string> userNames, string phone = null, string country = null, string emailPrefix = null, string password = null, string role = "admin", string pbxName = null, bool affiliate = false, bool active = true, bool addAsChild = false, string[] subUsers = null, string salesType = "retention", string salesAgentSalaryId = null, GetOfficeResponse officeData = null);
        string PostCreateUserRequest(string url, string userName, string phone = null, string country = null, string emailPrefix = null, string password = null, string role = "admin", string pbxName = null, bool affiliate = false, bool active = true, bool addAsChild = true, string[] subUsers = null, string salesType = "retention", string salesAgentSalaryId = null, GetOfficeResponse officeData = null, string apiKey = null, bool checkStatusCode = true);
        IUserApi PutEditUserOfficeRequest(string url, GetUserResponse getUserResponse);
        string PutEditUserRequest(string url, GetUserResponse getUserResponse, string apiKey = null, bool checkStatusCode = true);
        IUserApi PutEditUserRoleRequest(string url, string userId, string roleName);
    }
}