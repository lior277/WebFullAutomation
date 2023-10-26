using AirSoftAutomationFramework.Objects.DTOs;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.AccountsApi.UsersMenuApi.UserMenuApi;
using OpenQA.Selenium;
using System.Collections.Generic;

namespace AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.AccountsApi.UsersMenuApi
{
    public interface IUsersApi
    {
        T ChangeContext<T>(IWebDriver driver) where T : class;
        IUsersApi DeleteMassUserRequest(string url,
            string[] usersIds, string apiKey = null);
        GetUsersResponse GetActiveUsersRequest(string url);
        GetUsersResponse GetDeletedUsersRequest(string url, string apiKey = null);
        List<GetUserLastLogin> GetUserLastLoginsTimelineRequest(string url,
            string userId, string apiKey);

        (object, string) GetAllSalesAgentsByAgentRequest(string url, bool checkStatusCode = true);
        List<GetAllSalesAgentsByDialerResponse> GetAllSalesAgentsByDialerRequest(string url, string apiKey);
        GeneralResult<GetUserResponse> GetUserByIdRequest(string url, string userId, string apiKey = null, bool checkStatusCode = true);
        IUsersApi PostForgotPasswordRequest(string url, string userEmail);
    }
}