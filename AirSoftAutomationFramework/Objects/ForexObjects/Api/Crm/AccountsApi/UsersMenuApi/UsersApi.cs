// Ignore Spelling: Api mongo Forex Crm Timeline

using System.Collections.Generic;
using AirSoftAutomationFramework.Internals.DAL.ApiAccess;
using AirSoftAutomationFramework.Internals.DAL.ApiAccess.ApiRoutes;
using AirSoftAutomationFramework.Internals.DAL.ConfigurationManagement;
using AirSoftAutomationFramework.Internals.DAL.MongoDb;
using AirSoftAutomationFramework.Internals.Factory;
using AirSoftAutomationFramework.Internals.Helpers;
using AirSoftAutomationFramework.Objects.DTOs;
using Newtonsoft.Json;
using OpenQA.Selenium;

namespace AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.AccountsApi.UsersMenuApi
{
    public class UsersApi : IUsersApi
    {
        #region Members    
        private readonly IApplicationFactory _apiFactory;
        private IApiAccess _apiAccess;
        private IMongoDbAccess _mongoDbAccess;
        private string _apiKey = Config.appSettings.ApiKey;
        private string _usersTable = DataRep.UsersTable;
        private IWebDriver _driver;
        #endregion Members

        public UsersApi(IApplicationFactory apiFactory,
            IWebDriver driver, IApiAccess apiAccess, IMongoDbAccess mongoDbAccess)
        {
            _mongoDbAccess = mongoDbAccess;
            _apiFactory = apiFactory;
            _driver = driver;
            _apiAccess = apiAccess;
        }

        public GeneralResult<GetUserResponse> GetUserByIdRequest(string url,
            string userId, string apiKey = null, bool checkStatusCode = true)
        {
            _apiKey = apiKey ?? _apiKey;
            var generalResult = new GeneralResult<GetUserResponse>();
            var route = $"{url}{ApiRouteAggregate.PostCreateUser()}/{userId}?api_key={_apiKey}";
            var response = _apiAccess.ExecuteGetEntry(route);

            var json = response
                .Content
                .ReadAsStringAsync()
                .Result;

            if (checkStatusCode)
            {
                _apiAccess.CheckStatusCode(route, response);

                generalResult.GeneralResponse = JsonConvert
                    .DeserializeObject<GetUserResponse>(json);
            }
            else
            {
                generalResult.Message = json;
            }

            return generalResult;
        }

        public GetUsersResponse GetActiveUsersRequest(string url)
        {
            var route = $"{url}{ApiRouteAggregate.PostCreateUser()}?active=true&api_key={_apiKey}";
            var response = _apiAccess.ExecuteGetEntry(route);
            _apiAccess.CheckStatusCode(route, response);
            var json = response.Content.ReadAsStringAsync().Result;

            return JsonConvert.DeserializeObject<GetUsersResponse>(json);
        }

        public GetUsersResponse GetDeletedUsersRequest(string url, string apiKey = null)
        {
            var route = $"{url}{ApiRouteAggregate.PostCreateUser()}?active=&deleted=true&api_key={apiKey}";
            var response = _apiAccess.ExecuteGetEntry(route);
            _apiAccess.CheckStatusCode(route, response);
            var json = response.Content.ReadAsStringAsync().Result;

            return JsonConvert.DeserializeObject<GetUsersResponse>(json);
        }

        public IUsersApi DeleteMassUserRequest(string url,
            string[] usersIds, string apiKey = null)
        {
            _apiKey = apiKey ?? _apiKey;
            var route = $"{url}{ApiRouteAggregate.MassDeleteUserUser()}?api_key={_apiKey}";

            var deleteMassUser = new
            {
                ids = usersIds
            };

            var response = _apiAccess.ExecuteDeleteEntry(route, deleteMassUser);
            _apiAccess.CheckStatusCode(route, response);

            return this;
        }

        public List<GetUserLastLogin> GetUserLastLoginsTimelineRequest(string url,
            string userId, string apiKey)
        {
            var route = $"{url}{ApiRouteAggregate.GetUserLastLoginsTimeline(userId)}?api_key={apiKey}";
            var response = _apiAccess.ExecuteGetEntry(route);
            _apiAccess.CheckStatusCode(route, response);
            var json = response.Content.ReadAsStringAsync().Result;

            return JsonConvert.DeserializeObject<List<GetUserLastLogin>>(json);
        }

        public IUsersApi PostForgotPasswordRequest(string url, string userEmail)
        {
            var route = $"{url}{ApiRouteAggregate.PostForgotPasswordErp()}?api_key={_apiKey}";

            var forgetPasswordRequestDto = new
            {
                email = userEmail
            };

            var response = _apiAccess.ExecutePostEntry(route, forgetPasswordRequestDto);
            _apiAccess.CheckStatusCode(route, response);

            return this;
        }

        public (object, string) GetAllSalesAgentsByAgentRequest(string url,
            bool checkStatusCode = true)
        {
            var route = $"{url}{ApiRouteAggregate.GetAllSalesAgentsByAffiliate()}?api_key={_apiKey}";
            var response = _apiAccess.ExecuteGetEntry(route);

            var json = response
                .Content
                .ReadAsStringAsync()
                .Result;

            if (checkStatusCode)
            {
                _apiAccess.CheckStatusCode(route, response);

                return (null, json);
            }

            return (null, json);
        }

        public List<GetAllSalesAgentsByDialerResponse> GetAllSalesAgentsByDialerRequest(
            string url, string apiKey)
        {
            var route = $"{url}{ApiRouteAggregate.GetAllSalesAgentsByDialer()}?api_key={apiKey}";
            var response = _apiAccess.ExecuteGetEntry(route);
            _apiAccess.CheckStatusCode(route, response);
            var json = response.Content.ReadAsStringAsync().Result;

            return JsonConvert.DeserializeObject<List<GetAllSalesAgentsByDialerResponse>>(json);
        }

        public T ChangeContext<T>(IWebDriver driver) where T : class
        {
            return _apiFactory.ChangeContext<T>(_driver);
        }
    }
}
