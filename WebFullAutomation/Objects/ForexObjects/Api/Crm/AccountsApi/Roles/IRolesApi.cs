using AirSoftAutomationFramework.Objects.DTOs;
using OpenQA.Selenium;
using System.Collections.Generic;

namespace AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.AccountsApi.Roles
{
    public interface IRolesApi
    {
        T ChangeContext<T>(IWebDriver driver) where T : class;
        IRolesApi CreateAdminRoleWithDialerPipe(string uri);
        IRolesApi CreateAdminRoleWithUsersOnlyPipe(string uri);
        List<GetRoleByNameResponse> GetAffiliateRolesRequest(string uri);
        IRolesApi DeleteRoleRequest(string uri, string roleName);
        string DeleteRoleRequest(string uri, string roleName, string apiKey = null, bool checkStatusCode = true);
        GetRoleByNameResponse GetRoleByNameRequest(string uri, string roleName);
        List<GetRoleByNameResponse> GetRolesRequest(string uri);
        IRolesApi PostCreateAdminUsersOnlyRoleViewTradesApiRequest(string uri);
        IRolesApi PostCreateCostomRoleRequest(string uri, string roleName, Dictionary<List<string>, string> permissionsToUpdate = null, Dictionary<List<string>, string> notificationsToUpdate = null);
        IRolesApi PostCreateRoleRequest(string uri, GetRoleByNameResponse getRoleByNameResponse, string apiKey = null);
        IRolesApi PutEditRoleRequest(string uri, GetRoleByNameResponse getRoleByNameResponse, string apiKey = null);
        IRolesApi WaitForRollToCreate(string uri, string rollName);
    }
}