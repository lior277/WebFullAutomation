using AirSoftAutomationFramework.Internals.DAL.ConfigurationManagement;
using AirSoftAutomationFramework.Objects.DTOs;
using Newtonsoft.Json;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using AirSoftAutomationFramework.Internals.DAL.ApiAccsess;
using AirSoftAutomationFramework.Internals.DAL.ApiAccsess.ApiRoutes;
using AirSoftAutomationFramework.Internals.Factorys;

namespace AirSoftAutomationFramework.Objects.ForexObjects.ApiGeneral
{
    public class GlobalEventsApi : IGlobalEventsApi
    {

        #region Members      
        private IWebDriver _driver;
        private IApiAccess _apiAccess;
        private string _apiKey = Config.appSettings.ApiKey;
        private readonly IApplicationFactory _apiFactory;
        #endregion Members

        public GlobalEventsApi(IApplicationFactory apiFactory,
            IWebDriver driver, IApiAccess apiAccess)
        {
            _apiFactory = apiFactory;
            _driver = driver;
            _apiAccess = apiAccess;
        }

        public List<GetGlobalEventsResponse> GetGlobalEventsByUserRequest(string url,
              string userName, string apiKey = null)
        {
            _apiKey = apiKey ?? _apiKey;
            var today = DateTime.Now.ToString("yyyy-MM-dd");
            var route = ApiRouteAggregate.GetGlobalEvents(today);
            route = $"{url}{route}&api_key={_apiKey}";
            var response = _apiAccess.ExecuteGetEntry(route);
            _apiAccess.CheckStatusCode(route, response);
            var json = response.Content.ReadAsStringAsync().Result;

            var globalEvents = JsonConvert
                .DeserializeObject<List<GetGlobalEventsResponse>>(json);

            for (var i = 0; i < 10; i++)
            {
                if (globalEvents.Count == 0 || globalEvents == null) 
                {              
                    response = _apiAccess.ExecuteGetEntry(route);
                    _apiAccess.CheckStatusCode(route, response);
                    json = response.Content.ReadAsStringAsync().Result;

                    globalEvents = JsonConvert
                        .DeserializeObject<List<GetGlobalEventsResponse>>(json);

                    Thread.Sleep(300);// wait for event to register

                    continue;
                }

                break;
            }

            if (globalEvents == null)
            {
                throw new InvalidOperationException("global Events response is null");
            }

            return globalEvents
                .Where(p => p.action_made_by == userName)
                .ToList();
        }

        public string GetGlobalEventsRequest(string url, string apiKey, bool checkStatusCode = true)
        {
            var today = DateTime.Now.ToString("yyyy-MM-dd");
            var route = ApiRouteAggregate.GetGlobalEvents(today);
            route = $"{url}{route}&api_key={apiKey}";
            var response = _apiAccess.ExecuteGetEntry(route);

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

        public T ChangeContext<T>(IWebDriver driver) where T : class
        {
            return _apiFactory.ChangeContext<T>(_driver);
        }
    }
}
