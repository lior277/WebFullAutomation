// Ignore Spelling: Api Cfd

using AirSoftAutomationFramework.Internals.DAL.ApiAccess;
using AirSoftAutomationFramework.Internals.DAL.ApiAccess.ApiRoutes;
using AirSoftAutomationFramework.Internals.DAL.ConfigurationManagement;
using Newtonsoft.Json;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using AirSoftAutomationFramework.Objects.DTOs;
using AirSoftAutomationFramework.Internals.Factory;

namespace AirSoftAutomationFramework.Objects.MgmObjects.Api
{
    public class AssetsGroupApi : IAssetsGroupApi
    {
        #region Members       
        private readonly IApplicationFactory _apiFactory;
        private IWebDriver _driver;
        private IApiAccess _apiAccess;
        private string _crmUrl = Config.appSettings.CrmUrl;
        private string _apiKey = Config.appSettings.ApiKey;
        #endregion Members

        public AssetsGroupApi(IApplicationFactory apiFactory,
            IWebDriver driver, IApiAccess apiAccess)
        {
            _apiFactory = apiFactory;
            _driver = driver;
            _apiAccess = apiAccess;
        }

        public GetCfdResponse GetCfdRequest(string url,
            GetLoginResponse loginData)
        {
            var route = ApiRouteAggregate.GetCfd();
            route = url + route;
            var response = _apiAccess.ExecuteGetEntry(route, loginData);
            _apiAccess.CheckStatusCode(route, response);
            var json = response.Content.ReadAsStringAsync().Result;

            return JsonConvert.DeserializeObject<GetCfdResponse>(json);
        }      

        public T ChangeContext<T>(IWebDriver driver) where T : class
        {
            return _apiFactory.ChangeContext<T>(_driver);
        }

    }
}
