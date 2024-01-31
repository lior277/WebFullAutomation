using AirSoftAutomationFramework.Internals.DAL.ApiAccsess;
using AirSoftAutomationFramework.Internals.DAL.ApiAccsess.ApiRoutes;
using AirSoftAutomationFramework.Internals.DAL.ConfigurationManagement;
using AirSoftAutomationFramework.Internals.Factorys;
using OpenQA.Selenium;

namespace AirSoftAutomationFramework.Objects.ForexObjects.Api.Trade
{
    public class MarketExposurePageApi : IMarketExposurePageApi
    {
        #region Members       
        private IApiAccess _apiAccess;
        private string _apiKey = Config.appSettings.ApiKey;
        private readonly IApplicationFactory _apiFactory;
        private IWebDriver _driver;
        #endregion Members

        public MarketExposurePageApi(IApplicationFactory appFactory,
          IWebDriver driver, IApiAccess apiAccess)
        {
            _apiFactory = appFactory;
            _driver = driver;
            _apiAccess = apiAccess;
        }

        public string GetMarketExposureRequest(string url, string apiKey,
            bool checkStatusCode = true)
        {
            var route = ApiRouteAggregate.GetAssetsOnFront();
            route = $"{url}{route}?api_key={apiKey}";
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
