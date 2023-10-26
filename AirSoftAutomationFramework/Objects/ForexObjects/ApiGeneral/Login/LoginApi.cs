// Ignore Spelling: apikey Crm Forex Api app

using System;
using AirSoftAutomationFramework.Internals.DAL.ApiAccess;
using AirSoftAutomationFramework.Internals.DAL.ApiAccess.ApiRoutes;
using AirSoftAutomationFramework.Internals.DAL.ConfigurationManagement;
using AirSoftAutomationFramework.Internals.Factory;
using AirSoftAutomationFramework.Internals.Helpers;
using AirSoftAutomationFramework.Objects.DTOs;
using Newtonsoft.Json;
using OpenQA.Selenium;

namespace AirSoftAutomationFramework.Objects.ForexObjects.ApiGeneral.Login
{
    public class LoginApi : ILoginApi
    {
        #region Members
        private IApiAccess _apiAccess;
        private string _password = DataRep.Password;
        private string _apiKey = Config.appSettings.ApiKey;
        private readonly IApplicationFactory _apiFactory;
        #endregion Members

        public LoginApi(IApplicationFactory appFactory, IWebDriver driver,
            IApiAccess apiAccess)
        {
            _apiFactory = appFactory;
            _apiAccess = apiAccess;
        }

        public GeneralResult<GetLoginResponse> PostLoginToTradingPlatform(string uri,
            string expectedEmail, string password = null, bool checkStatusCode = true)
        {
            var generalResult = new GeneralResult<GetLoginResponse>();
            var route = ApiRouteAggregate.PostLogin();
            route = uri + route;

            var postLogin = new
            {
                email = expectedEmail,
                password = password ?? DataRep.Password,
                platform = "trading-platform",
                fingerprint = Guid.NewGuid().ToString().Replace("-", "")
            };

            var response = _apiAccess.ExecutePostEntry(route, postLogin);
            var json = response.Content.ReadAsStringAsync().Result;

            if (checkStatusCode)
            {
                _apiAccess.CheckStatusCode(route, response);
                generalResult.GeneralResponse = _apiAccess.GetLoginCookies(response, route);
            }
            else
            {
                generalResult.Message = json;
            }

            return generalResult;
        }

        public GeneralResult<CreateLoginResponse> PostLoginCrmRequest(string url,
            string email, string fingerprint, string password = null, string apikey = null,
            bool checkStatusCode = true)
        {
            _apiKey = apikey ?? _apiKey;
            var generalResult = new GeneralResult<CreateLoginResponse>();
            var route = $"{url}{ApiRouteAggregate.PostLogin()}?api_key={_apiKey}";
            string json;

            var postLogin = new
            {
                email = email,
                fingerprint = fingerprint,
                password = password ?? DataRep.Password,
                platform = "erp",
                twofactor = ""
            };

            var response = _apiAccess.ExecutePostEntry(route, postLogin);
            json = response.Content.ReadAsStringAsync().Result;

            if (checkStatusCode)
            {
                _apiAccess.CheckStatusCode(route, response);

                generalResult.GeneralResponse = JsonConvert
                    .DeserializeObject<CreateLoginResponse>(json);
            }
            else
            {
                generalResult.Message = json;
            }

            return generalResult;
        }
    }
}
