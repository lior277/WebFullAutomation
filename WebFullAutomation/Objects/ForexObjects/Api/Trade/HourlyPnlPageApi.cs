using AirSoftAutomationFramework.Internals.DAL.ApiAccsess;
using AirSoftAutomationFramework.Internals.DAL.ApiAccsess.ApiRoutes;
using AirSoftAutomationFramework.Internals.Factorys;
using OpenQA.Selenium;

namespace AirSoftAutomationFramework.Objects.ForexObjects.Api.Trade
{
    public class HourlyPnlPageApi : IHourlyPnlPageApi
    {
        #region Members       
        private IApiAccess _apiAccess;
        private readonly IApplicationFactory _apiFactory;
        private IWebDriver _driver;
        #endregion Members

        public HourlyPnlPageApi(IApplicationFactory appFactory,
          IWebDriver driver, IApiAccess apiAccess)
        {
            _apiFactory = appFactory;
            _driver = driver;
            _apiAccess = apiAccess;
        }

        public string GetHourlyPnlRequest(string url, string apiKey, bool checkStatusCode = true)
        {
            var route = ApiRouteAggregate.GetHourlyPnl();
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
