// Ignore Spelling: Api mongo

using AirSoftAutomationFramework.Internals.DAL.ConfigurationManagement;
using AirSoftAutomationFramework.Internals.DAL.MongoDb;
using AirSoftAutomationFramework.Internals.Helpers;
using Newtonsoft.Json.Linq;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using AirSoftAutomationFramework.Internals.DAL.ApiAccsess;
using AirSoftAutomationFramework.Internals.DAL.ApiAccsess.ApiRoutes;
using AirSoftAutomationFramework.Internals.Factorys;
using AirSoftAutomationFramework.Objects.DTOs;

namespace AirSoftAutomationFramework.Objects.MgmObjects.Api
{
    public class MgmCreateUserApi : IMgmCreateUserApi
    {
        #region Members        
        private readonly IApplicationFactory _apiFactory;
        private IApiAccess _apiAccess;
        private IMongoDbAccess _mongoDbAccess;
        private string _password = DataRep.Password;
        private IWebDriver _driver;
        #endregion Members

        public MgmCreateUserApi(IApplicationFactory apiFactory,
            IWebDriver driver, IApiAccess apiAccess, IMongoDbAccess mongoDbAccess)
        {
            _mongoDbAccess = mongoDbAccess;
            _apiFactory = apiFactory;
            _driver = driver;
            _apiAccess = apiAccess;
        }

        public GetLoginResponse PostMgmLoginCookiesRequest
            (string uri, string userName)
        {
            var route = ApiRouteAggregate.PostCreateMgmLogin();
            route = uri + route;

            var loginDto = new
            {
                email = userName,
                password = _password,
                //twofactor = "",
                fingerprint = Guid.NewGuid().ToString().Replace("-", ""),
            };
            var response = _apiAccess.ExecutePostEntry(route, loginDto);
            _apiAccess.CheckStatusCode(route, response);

            return _apiAccess.GetLoginCookies(response, route);
        }

        public string PostCreateMgmApiKeyRequest(string uri, string userId,
             GetLoginResponse loginData)
        {
            var route = $"{uri}{DataRep.ApiRoutePostCreateMgmApiKey}";

            var apiKeyDto = new
            {
                id = userId
            };
            var response = _apiAccess.ExecutePostEntry(route,
                apiKeyDto, loginData);

            _apiAccess.CheckStatusCode(route, response);

            var json = response
               .Content
               .ReadAsStringAsync()
               .Result;

            return JObject.Parse(json)["key"].ToString();
        }


        public T ChangeContext<T>(IWebDriver driver) where T : class
        {
            return _apiFactory.ChangeContext<T>(_driver);
        }
    }
}
