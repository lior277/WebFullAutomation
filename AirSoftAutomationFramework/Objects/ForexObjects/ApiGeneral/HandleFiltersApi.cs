using AirSoftAutomationFramework.Internals.DAL.ApiAccess;
using AirSoftAutomationFramework.Internals.DAL.ApiAccess.ApiRoutes;
using AirSoftAutomationFramework.Internals.DAL.ConfigurationManagement;
using AirSoftAutomationFramework.Internals.ExtensionsMethods;
using AirSoftAutomationFramework.Internals.Factory;
using AirSoftAutomationFramework.Objects.DTOs;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace AirSoftAutomationFramework.Objects.ForexObjects.ApiGeneral
{
    public class HandleFiltersApi : IHandleFiltersApi
    {
        #region Members      
        private IWebDriver _driver;
        private IApiAccess _apiAccess;
        private string _apiKey = Config.appSettings.ApiKey;
        private readonly IApplicationFactory _apiFactory;
        #endregion Members

        public HandleFiltersApi(IApplicationFactory apiFactory,
            IWebDriver driver, IApiAccess apiAccess)
        {
            _apiFactory = apiFactory;
            _driver = driver;
            _apiAccess = apiAccess;
        }

        public string PostCreateFilterRequest(string url, string filterName,
            bool @default, string id, string itemName, string lable, string componentName,
            string time_filter = "month", bool checkStatusCode = true)
        {
            var route = $"{url}{ApiRouteAggregate.PostCreateFilter(componentName)}?api_key={_apiKey}";

            var filterBody = new FilterBody
            {
                id = id,
                itemName = itemName,
                label = lable,
                inputName = lable
            };

            var PostCreateFilter = new PostFilterRequest
            {
                name = filterName,
                @default = @default,
                filterBody = new FilterBody[] { filterBody },
                time_filter = time_filter,
                period_range = null
            };

            var response = _apiAccess.ExecutePostEntry(route, PostCreateFilter);
            var json = response.Content.ReadAsStringAsync().Result;

            if (checkStatusCode)
            {
                _apiAccess.CheckStatusCode(route, response);
            }

            return json;
        }

        public IHandleFiltersApi DeleteFilterRequest(string url, string filterName,
            string componentName)
        {
            var route = $"{url}{ApiRouteAggregate.DeleteFilterFilter(componentName)}" +
                $"?api_key={_apiKey}";

            var deleteFilter = new
            {
                filter_name = filterName,
            };

            var response = _apiAccess.ExecuteDeleteEntry(route, deleteFilter);
            _apiAccess.CheckStatusCode(route, response);

            return this;
        }

        public IHandleFiltersApi DeleteFilterRequest(string url, List<string> filtersNames,
            string componentName)
        {
            foreach (var filterName in filtersNames)
            {
                DeleteFilterRequest(url, filterName, componentName);
            }

            return this;
        }

        public List<GetFiltersReponse> GetFiltersRequest(string url, string componentName)
        {
            var route =
                $"{url}{ApiRouteAggregate.PostCreateFilter(componentName)}?api_key={_apiKey}";

            var response = _apiAccess.ExecuteGetEntry(route);
            _apiAccess.CheckStatusCode(route, response);

            var json = response
                .Content
                .ReadAsStringAsync()
                .Result;

            return JsonConvert.DeserializeObject<List<GetFiltersReponse>>(json);
        }

        public Dictionary<string, SaleStatusValues> GetSalesStatusTextFilter(string url)
        {
            var tempDic = new Dictionary<string, string>();
            var tempList = new List<string>();

            var route =
                $"{url}{ApiRouteAggregate.GetSalesStatusTextFilter()}&api_key={_apiKey}";

            var response = _apiAccess.ExecuteGetEntry(route);
            _apiAccess.CheckStatusCode(route, response);

            var json = response
                .Content
                .ReadAsStringAsync()
                .Result;

            var jObject = JObject.Parse(json);
            var salesStatuses = jObject["sales_status_text"]["sales_status_text"];
            salesStatuses.ForEach(x => tempList.Add(x.Parent.ToString()));

            return JsonConvert.DeserializeObject<Dictionary<string, SaleStatusValues>>(tempList.First());
        }

        public IHandleFiltersApi VerifyFilterCreated(string url,
            string filterName, string componentName)
        {
            var filter = _apiFactory
                .ChangeContext<IHandleFiltersApi>()
                .GetFiltersRequest(url, componentName)
                .FirstOrDefault()?
                .Filters?
                .Clients?
                .Where(p => p.Name == filterName);

            for (var i = 0; i < 20; i++)
            {
                if (filter == null)
                {
                    continue;
                }

                if (filter.Any())
                {
                    if (filter.First().Name != filterName)
                    {
                        filter = _apiFactory
                            .ChangeContext<IHandleFiltersApi>()
                            .GetFiltersRequest(url, componentName)
                            .FirstOrDefault()?
                            .Filters?
                            .Clients?
                            .Where(p => p.Name == filterName);

                        Thread.Sleep(300);

                        continue;
                    }

                    break;
                }
            }

            if (filter == null)
            {
                throw new InvalidOperationException($"filter name: {filterName} not created");
            }

            return this;
        }      

        public T ChangeContext<T>(IWebDriver driver) where T : class
        {
            return _apiFactory.ChangeContext<T>(_driver);
        }
    }
}
