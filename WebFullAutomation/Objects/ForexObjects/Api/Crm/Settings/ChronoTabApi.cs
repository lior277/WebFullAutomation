using AirSoftAutomationFramework.Internals.DAL.ApiAccsess;
using AirSoftAutomationFramework.Internals.DAL.ApiAccsess.ApiRoutes;
using AirSoftAutomationFramework.Internals.DAL.ConfigurationManagement;
using AirSoftAutomationFramework.Internals.Factorys;
using AirSoftAutomationFramework.Objects.DTOs;
using Newtonsoft.Json;
using OpenQA.Selenium;

namespace AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.Settings
{
    public class ChronoTabApi : IChronoTabApi
    {
        #region Members  
        private string _apiKey = Config.appSettings.ApiKey;
        private readonly IApplicationFactory _apiFactory;
        private IApiAccess _apiAccess;
        private IWebDriver _driver;
        #endregion Members

        public ChronoTabApi(IApplicationFactory apiFactory, IWebDriver driver, IApiAccess apiAccess)
        {
            _apiFactory = apiFactory;
            _driver = driver;
            _apiAccess = apiAccess;
        }

        public GetChronoTabResponse GetChronoTabRequest(string url, string apiKey = null)
        {
            _apiKey = apiKey ?? _apiKey;
            var route = $"{url}{ApiRouteAggregate.GetChronoTab()}&api_key={_apiKey}";
            var response = _apiAccess.ExecuteGetEntry(route);
            _apiAccess.CheckStatusCode(route, response);
            var json = response.Content.ReadAsStringAsync().Result;

            return JsonConvert.DeserializeObject<GetChronoTabResponse>(json);
        }

        public IChronoTabApi PutChronoSettingsRequest(string url)
        {
            var route = ApiRouteAggregate.PutConfigChrono();
            route = $"{url}{route}?api_key={_apiKey}";
            var response = _apiAccess.ExecutePutEntry(route);
            _apiAccess.CheckStatusCode(route, response);

            return this;
        }

        public IChronoTabApi PatchBoostOptionsToDefaultRequest(string url, string apiKey = null)
        {
            var defaultBoostOptions = @"{ ""boosts"":[2,10,50,100,300,8],""min_amounts"":
                                            { ""2"":100,""4"":100,""5"":100,""8"":100,""10"":500,""15"":500,
                                            ""20"":500,""30"":500,""40"":500,""50"":500,""100"":2000,""200"":2000,
                                            ""300"":2000,""400"":200,""500"":2000} }";


            _apiKey = apiKey ?? _apiKey;
            var route = ApiRouteAggregate.PatchBoostOptionsInChronoTab();
            route = $"{url}{route}?api_key={_apiKey}";
            var patchBoostOptions = JsonConvert.DeserializeObject<PatchBoostOptionRequest>(defaultBoostOptions);

            var response = _apiAccess.ExecutePatchEntry(route, patchBoostOptions);
            _apiAccess.CheckStatusCode(route, response);

            return this;
        }

        public IChronoTabApi PatchMinAmountForBoostOptionRequest(string url, string apiKey = null)
        {
            var defaultBoostOptions = @"{ ""boosts"":[2,10,50,100,300,8],""min_amounts"":
                                            { ""2"":100,""4"":100,""6"":100,""8"":100,""10"":500,""15"":500,
                                            ""20"":500,""30"":500,""40"":500,""50"":500,""100"":2000,""200"":2000,
                                            ""300"":2000000,""400"":200,""500"":2000} }";

            _apiKey = apiKey ?? _apiKey;
            var route = ApiRouteAggregate.PatchBoostOptionsInChronoTab();
            route = $"{url}{route}?api_key={_apiKey}";
            var patchBoostOptions = JsonConvert.
                DeserializeObject<PatchBoostOptionRequest>(defaultBoostOptions);

            var response = _apiAccess.ExecutePatchEntry(route, patchBoostOptions);
            _apiAccess.CheckStatusCode(route, response);

            return this;
        }

        public T ChangeContext<T>(IWebDriver driver) where T : class
        {
            return _apiFactory.ChangeContext<T>(_driver);
        }
    }
}
