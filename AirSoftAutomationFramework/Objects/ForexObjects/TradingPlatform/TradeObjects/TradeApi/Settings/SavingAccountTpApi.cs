using AirSoftAutomationFramework.Internals.DAL.ApiAccess;
using AirSoftAutomationFramework.Internals.DAL.ApiAccess.ApiRoutes;
using AirSoftAutomationFramework.Internals.Factory;
using AirSoftAutomationFramework.Objects.DTOs;
using OpenQA.Selenium;

namespace AirSoftAutomationFramework.Objects.ForexObjects.TradingPlatform.TradeObjects.TradeApi.Settings
{
    public class SavingAccountTpApi : ISavingAccountTpApi
    {
        #region Members  
        private readonly IApplicationFactory _apiFactory;
        private IApiAccess _apiAccess;
        private IWebDriver _driver;
        #endregion Members

        public SavingAccountTpApi(IApplicationFactory apiFactory,
            IWebDriver driver, IApiAccess apiAccess)
        {
            _apiFactory = apiFactory;
            _driver = driver;
            _apiAccess = apiAccess;
        }

        public string PostTransferToSavingAccountFromTpRequest(string url, string clientId,
            int sAAmount, GetLoginResponse loginData = null, bool checkStatusCode = true)
        {
            var route = ApiRouteAggregate.PostTransferToSA();
            route = url + route;

            var createTransferToSARequestDto = new
            {
                user_id = clientId,
                amount = sAAmount,
            };

            var response = _apiAccess.ExecutePostEntry(route,
                createTransferToSARequestDto, loginData);

            if (checkStatusCode)
            {
                _apiAccess.CheckStatusCode(route, response);
            }

            return response.Content.ReadAsStringAsync().Result;
        }

        public string PostTransferToBalanceFromTpRequest(string url, string clientId,
           int amount, GetLoginResponse loginData = null, bool checkStatusCode = true)
        {
            var route = ApiRouteAggregate.PostTransferToBalance();
            route = url + route;

            var createTransferToBalanceRequest = new
            {
                user_id = clientId,
                amount,
            };

            var response = _apiAccess.ExecutePostEntry(route,
                createTransferToBalanceRequest, loginData);

            if (checkStatusCode)
            {
                _apiAccess.CheckStatusCode(route, response);
            }

            return response.Content.ReadAsStringAsync().Result;
        }

        public T ChangeContext<T>(IWebDriver driver) where T : class
        {
            return _apiFactory.ChangeContext<T>(_driver);
        }
    }
}
