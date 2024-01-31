using AirSoftAutomationFramework.Internals.DAL.ConfigurationManagement;
using AirSoftAutomationFramework.Internals.ExtensionsMethods;
using AirSoftAutomationFramework.Internals.Helpers;
using AirSoftAutomationFramework.Objects.DTOs;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using AirSoftAutomationFramework.Internals.DAL.ApiAccsess;
using AirSoftAutomationFramework.Internals.DAL.ApiAccsess.ApiRoutes;
using AirSoftAutomationFramework.Internals.Factorys;

namespace AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.AccountsApi.Roles
{
    public class RolesApi : IRolesApi
    {
        #region Members       
        private IWebDriver _driver;
        private IApiAccess _apiAccess;
        private string _apiKey = Config.appSettings.ApiKey;
        private readonly IApplicationFactory _apiFactory;
        #endregion Members

        public RolesApi(IApplicationFactory apiFactory, IWebDriver driver, IApiAccess apiAccess)
        {
            _apiFactory = apiFactory;
            _driver = driver;
            _apiAccess = apiAccess;
        }

        public GetRoleByNameResponse GetRoleByNameRequest(string uri, string roleName)
        {
            var route = $"{uri}{ApiRouteAggregate.GetRoleByName()}{roleName}?api_key={_apiKey}";
            var response = _apiAccess.ExecuteGetEntry(route);
            _apiAccess.CheckStatusCode(route, response);
            var json = response.Content.ReadAsStringAsync().Result;
            var roleData = JsonConvert.DeserializeObject<GetRoleByNameResponse>(json);

            return roleData;

            //for (var i = 0; i < 10; i++)
            //{
            //    try
            //    {
            //        if (roleData.Name == roleName)
            //        {
            //            break;
            //        }

            //    }
            //    catch(Exception ex)
            //    {
            //        throw ex.InnerException;
            //    }
            //}

            //return roleData;
        }

        public IRolesApi DeleteRoleRequest(string uri, string roleName)
        {
            var role = GetRoleByNameRequest(uri, roleName);
            var route = $"{uri}{ApiRouteAggregate.DeleteRole()}{role.Id}/{role.Name}?api_key={_apiKey}";
            var response = _apiAccess.ExecuteDeleteEntry(route);
            _apiAccess.CheckStatusCode(route, response);

            return this;
        }

        public string DeleteRoleRequest(string uri, string roleName,
            string apiKey = null, bool checkStatusCode = true)
        {
            _apiKey = apiKey ?? _apiKey;
            var role = GetRoleByNameRequest(uri, roleName);
            var route = $"{uri}{ApiRouteAggregate.DeleteRole()}{role.Id}/{role.Name}?api_key={apiKey}";
            var response = _apiAccess.ExecuteDeleteEntry(route);

            var json = response
               .Content
               .ReadAsStringAsync()
               .Result;

            if (checkStatusCode)
            {
                _apiAccess.CheckStatusCode(route, response);
            }

            return json;
        }

        public List<GetRoleByNameResponse> GetRolesRequest(string uri)
        {
            var route = $"{uri}{ApiRouteAggregate.GetRoles()}&api_key={_apiKey}";
            var response = _apiAccess.ExecuteGetEntry(route);
            _apiAccess.CheckStatusCode(route, response);
            var json = response.Content.ReadAsStringAsync().Result;

            return JsonConvert.DeserializeObject<List<GetRoleByNameResponse>>(json);
        }

        public List<GetRoleByNameResponse> GetAffiliateRolesRequest(string uri)
        {
            var route = $"{uri}{ApiRouteAggregate.GetAffiliateRoles()}?api_key={_apiKey}";
            var response = _apiAccess.ExecuteGetEntry(route);
            _apiAccess.CheckStatusCode(route, response);
            var json = response.Content.ReadAsStringAsync().Result;

            return JsonConvert.DeserializeObject<List<GetRoleByNameResponse>>(json);
        }

        public IRolesApi CreateAdminRoleWithUsersOnlyPipe(string uri)
        {
            var roleExist = _apiFactory
                .ChangeContext<IRolesApi>(_driver)
                .GetRolesRequest(uri)
                .Any(p => p.Name == DataRep.AdminWithUsersOnlyRoleName);

            if (!roleExist)
            {
                // get role by name
                var roleData = _apiFactory
                    .ChangeContext<IRolesApi>()
                    .GetRoleByNameRequest(uri, DataRep.AdminRole);

                roleData.Name = DataRep.AdminWithUsersOnlyRoleName;
                roleData.UsersOnly = true;

                // create  role 
                _apiFactory
                    .ChangeContext<IRolesApi>()
                    .PostCreateRoleRequest(uri, roleData);
            }

            return this;
        }

        public IRolesApi PostCreateRoleRequest(string uri,
            GetRoleByNameResponse getRoleByNameResponse, string apiKey = null)
        {
            var children = getRoleByNameResponse.Children.ToList();
            children.RemoveAll(p => p == null);
            getRoleByNameResponse.Children = children;// remove null Children
            _apiKey = apiKey ?? _apiKey;
            var route = $"{uri}{ApiRouteAggregate.PostCreateRole()}?api_key={_apiKey}";
            var json = JsonConvert.SerializeObject(getRoleByNameResponse);
            var dto = JObject.Parse(json);
            dto.Property("_id").Remove();
            dto.Property("last_update").Remove();
            var response = _apiAccess.ExecutePostEntry(route, dto);
            _apiAccess.CheckStatusCode(route, response);

            return this;
        }

        public IRolesApi WaitForRollToCreate(string uri, string rollName)
        {
            var roll = GetRoleByNameRequest(uri, rollName);

            for (var i = 0; i < 10; i++)
            {
                if (roll == null)
                {
                    Thread.Sleep(200);

                    continue;
                }

                break;
            }

            return this;
        }

        public IRolesApi PostCreateCostomRoleRequest(string uri, string roleName,
            Dictionary<List<string>, string> permissionsToUpdate = null,
            Dictionary<List<string>, string> notificationsToUpdate = null)
        {
            var children = new List<string> { "admin", "affiliate", "agent", "manager", "support" };
            var adminRole = GetRoleByNameRequest(uri, "admin");
            adminRole.Show = true;
            adminRole.Affiliate = false;
            adminRole.ShowAllDeposits = false;
            adminRole.ShowDepositsAmount = false;
            adminRole.SeeSingleOffice = false;
            adminRole.ShowAttributionDate = false;
            adminRole.ShowSalesStatus = false;
            adminRole.Name = roleName;
            adminRole.UsersOnly = true;
            adminRole.DialerApi = false;
            adminRole.Notifications = adminRole.Notifications.UpdateList(notificationsToUpdate);
            adminRole.Children = children;
            adminRole.ErpPermissions = adminRole.ErpPermissions.UpdateList(permissionsToUpdate);
            var jsonRequest = JsonConvert.SerializeObject(adminRole);
            var jsonobj = JsonConvert.DeserializeObject<CreateRoleRequest>(jsonRequest);
            var route = $"{uri}{ApiRouteAggregate.PostCreateRole()}?api_key={_apiKey}";
            var response = _apiAccess.ExecutePostEntry(route, jsonobj);
            _apiAccess.CheckStatusCode(route, response);

            return this;
        }

        public IRolesApi PutEditRoleRequest(string uri,
               GetRoleByNameResponse getRoleByNameResponse, string apiKey = null)
        {
            _apiKey = apiKey ?? _apiKey;
            var route = $"{uri}{ApiRouteAggregate.UpdateRole()}{getRoleByNameResponse.Id}?api_key={_apiKey}";
            var json = JsonConvert.SerializeObject(getRoleByNameResponse);
            var dto = JObject.Parse(json);
            dto.Property("_id").Remove();
            dto.Property("last_update").Remove();
            var response = _apiAccess.ExecutePutEntry(route, dto);
            _apiAccess.CheckStatusCode(route, response);
            //Thread.Sleep(1000); // wait for change update

            return this;
        }

        public IRolesApi PostCreateAdminUsersOnlyRoleViewTradesApiRequest(string uri)
        {
            var children = new List<string> { "admin", "affiliate", "agent", "manager", "support" };
            var adminRole = GetRoleByNameRequest(uri, "admin");
            var erpPermissions = new List<string>();
            erpPermissions = adminRole.ErpPermissions.ToList().Where(p => p != "all_user_trades").ToList();
            adminRole.Name = DataRep.AdminUsersOnlyWithViewTradesOnly;
            adminRole.ErpPermissions = erpPermissions;
            adminRole.UsersOnly = true;
            adminRole.DialerApi = false;
            adminRole.Notifications = adminRole.Notifications;
            adminRole.Children = children;
            var jsonRequest = JsonConvert.SerializeObject(adminRole);
            var jsonobj = JsonConvert.DeserializeObject<CreateRoleRequest>(jsonRequest);
            var route = $"{uri}{ApiRouteAggregate.PostCreateRole()}?api_key={_apiKey}";
            var response = _apiAccess.ExecutePostEntry(route, jsonobj);
            _apiAccess.CheckStatusCode(route, response);

            return this;
        }

        public IRolesApi CreateAdminRoleWithDialerPipe(string uri)
        {
            var roleExist = _apiFactory
                .ChangeContext<IRolesApi>(_driver)
                .GetRolesRequest(uri)
                .Any(p => p.Name == DataRep.AdminWithDialerRole);

            if (!roleExist)
            {
                // get role by name
                var roleData = _apiFactory
                    .ChangeContext<IRolesApi>()
                    .GetRoleByNameRequest(uri, DataRep.AdminRole);

                roleData.Name = DataRep.AdminWithDialerRole;
                roleData.DialerApi = true;
                roleData.SeeSingleOffice = true;

                // create  role 
                _apiFactory
                    .ChangeContext<IRolesApi>()
                    .PostCreateRoleRequest(uri, roleData);
            }

            return this;
        }

        public T ChangeContext<T>(IWebDriver driver) where T : class
        {
            return _apiFactory.ChangeContext<T>(_driver);
        }
    }
}
