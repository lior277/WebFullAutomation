// Ignore Spelling: Api Forex Crm

using System;
using AirSoftAutomationFramework.Internals.DAL.ApiAccess;
using AirSoftAutomationFramework.Internals.DAL.ApiAccess.ApiRoutes;
using AirSoftAutomationFramework.Internals.DAL.ConfigurationManagement;
using AirSoftAutomationFramework.Internals.Factory;
using AirSoftAutomationFramework.Objects.DTOs;
using Newtonsoft.Json;
using OpenQA.Selenium;

namespace AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.sales
{
    public class SalesPageApi : ISalesPageApi
    {
        #region Members       
        private readonly IApplicationFactory _apiFactory;
        private IWebDriver _driver;
        private IApiAccess _apiAccess;
        private string _apiKey = Config.appSettings.ApiKey;
        #endregion Members

        public SalesPageApi(IApplicationFactory apiFactory,
            IWebDriver driver, IApiAccess apiAccess)
        {
            _apiFactory = apiFactory;
            _driver = driver;
            _apiAccess = apiAccess;
        }

        public GeneralResult<GetAgentProfileInfoResponse> GetAgentProfileInfoRequest(string url,
            string userId, bool checkStatusCode = true)
        {
            var generalResult = new GeneralResult<GetAgentProfileInfoResponse>();
            var route = $"{url}{ApiRouteAggregate.GetAgentProfileInfo(userId)}&api_key={_apiKey}";
            var response = _apiAccess.ExecuteGetEntry(route);

            var json = response
                .Content
                .ReadAsStringAsync()
                .Result;

            if (checkStatusCode)
            {
                _apiAccess.CheckStatusCode(route, response);

                generalResult.GeneralResponse = JsonConvert.DeserializeObject<GetAgentProfileInfoResponse>(json);
            }
            else
            {
                generalResult.Message = json;
            }

            return generalResult;
        }

        public string GetSalesPerformanceRequest(string uri, string apiKey, bool checkStatusCode = true)
        {
            _apiKey = apiKey ?? _apiKey;
            var startDate = DateTime.Now.AddDays(-1).ToString("yyyy/MM/dd");
            var endDate = DateTime.Now.AddDays(1).ToString("yyyy/MM/dd");
            var route = $"{uri}{ApiRouteAggregate.GetSalesPerformance()}?time[start_date]={startDate}&&time[end_date]={endDate}&api_key={apiKey}";
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

        public string GetApprovedDepositRequest(string uri, string apiKey,
            bool checkStatusCode = true)
        {
            _apiKey = apiKey ?? _apiKey;
            var startDate = DateTime.Now.AddDays(-1).ToString("yyyy/MM/dd");
            var endDate = DateTime.Now.AddDays(1).ToString("yyyy/MM/dd");
            var route = $"{uri}{ApiRouteAggregate.GetSalesPerformance()}?time[start_date]={startDate}&&time[end_date]={endDate}&api_key={apiKey}";
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
    }
}
