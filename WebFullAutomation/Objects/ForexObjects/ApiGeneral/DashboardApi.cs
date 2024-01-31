// Ignore Spelling: Api Forex

using System;
using System.Collections.Generic;
using System.Text;
using AirSoftAutomationFramework.Internals.DAL.ApiAccsess;
using AirSoftAutomationFramework.Internals.DAL.ApiAccsess.ApiRoutes;
using AirSoftAutomationFramework.Internals.DAL.ConfigurationManagement;
using AirSoftAutomationFramework.Internals.Factorys;
using AirSoftAutomationFramework.Objects.DTOs;
using Newtonsoft.Json;
using OpenQA.Selenium;
using static AirSoftAutomationFramework.Objects.DTOs.GetUserTimeLine;

namespace AirSoftAutomationFramework.Objects.ForexObjects.ApiGeneral
{
    public class DashboardApi : IDashboardApi
    {
        #region Members      
        private IWebDriver _driver;
        private IApiAccess _apiAccess;
        private string _apiKey = Config.appSettings.ApiKey;
        private readonly IApplicationFactory _apiFactory;
        #endregion Members

        public DashboardApi(IApplicationFactory apiFactory,
            IWebDriver driver, IApiAccess apiAccess)
        {
            _apiFactory = apiFactory;
            _driver = driver;
            _apiAccess = apiAccess;
        }

        public GetBoxesStatisticsResponse GetBoxesStatisticsRequest(string uri,
            string apiKey = null)
        {
            _apiKey = apiKey ?? _apiKey;
            var startDate = DateTime.Now.AddDays(-1).ToString("yyyy/MM/dd");
            var endDate = DateTime.Now.AddDays(1).ToString("yyyy/MM/dd");
            var route = $"{uri}{ApiRouteAggregate.GetBoxesStatistics()}?start_date={startDate}&end_date={endDate}&api_key={_apiKey}";
            var response = _apiAccess.ExecuteGetEntry(route);
            _apiAccess.CheckStatusCode(route, response);
            var json = response.Content.ReadAsStringAsync().Result;

            return JsonConvert.DeserializeObject<GetBoxesStatisticsResponse>(json);
        }

        public string GetBoxesStatisticsRequest(string uri, string apiKey,
            bool checkStatusCode = true)
        {
            _apiKey = apiKey ?? _apiKey;
            var startDate = DateTime.Now.AddDays(-1).ToString("yyyy/MM/dd");
            var endDate = DateTime.Now.AddDays(1).ToString("yyyy/MM/dd");
            var route = $"{uri}{ApiRouteAggregate.GetBoxesStatistics()}?start_date={startDate}&end_date={endDate}&api_key={apiKey}";
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

        public List<UserTimeLine> GetUserTimeLineRequest(string uri,
            Dictionary<string, object> urlParams,
            string apiKey, bool checkStatusCode = true)
        {
            var startDate = DateTime.Now.ToString("yyyy-MM-dd");
            var sb = new StringBuilder();

            foreach (var kvp in urlParams)
            {
                sb.Append($"&{kvp.Key}={kvp.Value}");
            }

            var ft = sb.ToString();

            var route = $"{uri}{ApiRouteAggregate.GetUserTimeLine()}?type[]=change_sales_agent" +
                $"&start_date={startDate}" +
                $" 00:00:00&end_date={startDate} 23:59:59&api_key={apiKey}";

            route = route + ft;

            var response = _apiAccess.ExecuteGetEntry(route);

            var json = response
                .Content
                .ReadAsStringAsync()
                .Result;

            if (checkStatusCode)
            {
                _apiAccess.CheckStatusCode(route, response);
            }

            return JsonConvert.DeserializeObject<List<UserTimeLine>>(json);
        }


        public string GetSalesPerformanceRequest(string uri, string apiKey, bool checkStatusCode = true)
        {
            _apiKey = apiKey ?? _apiKey;
            var startDate = DateTime.Now.AddDays(-1).ToString("yyyy/MM/dd");
            var endDate = DateTime.Now.AddDays(1).ToString("yyyy/MM/dd");
            var route = $"{uri}{ApiRouteAggregate.GetSalesPerformance()}?start_date={startDate}&end_date={endDate}&api_key={apiKey}";
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

        public string GetCalendarRequest(string uri, string apiKey, bool checkStatusCode = true)
        {
            var startDate = DateTime.Now.AddDays(-1).ToString("yyyy/MM/dd");
            var endDate = DateTime.Now.AddDays(1).ToString("yyyy/MM/dd");
            var route = $"{uri}{ApiRouteAggregate.PostComment()}?start_date={startDate}&end_date={endDate}&api_key={apiKey}";
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

        public string GetActiveCampaignsRequest(string uri, string apiKey, bool checkStatusCode = true)
        {
            var startDate = DateTime.Now.AddDays(-1).ToString("yyyy/MM/dd");
            var endDate = DateTime.Now.AddDays(1).ToString("yyyy/MM/dd");
            var route = $"{uri}{ApiRouteAggregate.GetActiveCampaigns()}?start_date={startDate}&end_date={endDate}&api_key={apiKey}";
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

        public string GetDepositsRequest(string uri, string apiKey, bool checkStatusCode = true)
        {
            var startDate = DateTime.Now.AddDays(-1).ToString("yyyy/MM/dd");
            var endDate = DateTime.Now.AddDays(1).ToString("yyyy/MM/dd");
            var route = $"{uri}{ApiRouteAggregate.GetTransactions("deposit")}" +
                $"?start_date={startDate}&end_date={endDate}&api_key={apiKey}";

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

        public string GetLastRegistrationRequest(string uri, string apiKey, bool checkStatusCode = true)
        {
            var startDate = DateTime.Now.AddDays(-1).ToString("yyyy/MM/dd");
            var endDate = DateTime.Now.AddDays(1).ToString("yyyy/MM/dd");
            var route = $"{uri}{ApiRouteAggregate.GetLastRegistration()}?start_date={startDate}&end_date={endDate}&api_key={apiKey}";
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
