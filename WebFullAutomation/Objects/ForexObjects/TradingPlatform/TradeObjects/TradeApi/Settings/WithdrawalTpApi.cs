using AirSoftAutomationFramework.Internals.DAL.ApiAccsess;
using AirSoftAutomationFramework.Internals.DAL.ApiAccsess.ApiRoutes;
using AirSoftAutomationFramework.Internals.Factorys;
using AirSoftAutomationFramework.Objects.DTOs;
using OpenQA.Selenium;
using static AirSoftAutomationFramework.Objects.DTOs.TestCase;

namespace AirSoftAutomationFramework.Objects.ForexObjects.TradingPlatform.TradeObjects.TradeApi.Settings
{
    public class WithdrawalTpApi : IWithdrawalTpApi
    {
        #region Members  
        private readonly IApplicationFactory _apiFactory;
        private IApiAccess _apiAccess;
        private int _amount = 10;
        private IWebDriver _driver;
        #endregion Members

        public WithdrawalTpApi(IApplicationFactory apiFactory, IWebDriver driver,
            IApiAccess apiAccess)
        {
            _apiFactory = apiFactory;
            _driver = driver;
            _apiAccess = apiAccess;
        }

        public string PostPendingWithdrawalRequest(string url,
            GetLoginResponse loginData,
            int amount = 10, bool checkStatusCode = true)
        {
            var route = ApiRouteAggregate.PostCreatePendingWithdrawal();
            route = url + route;

            var createWithdrawalRequestDto = new CreateWithdrawalRequest
            {
                amount = amount,
                credit_card_owner = "asd asd",
                last_digits = "1234",
                exp_date = "12/34",
            };

            var response = _apiAccess.ExecutePostEntry(route,
                createWithdrawalRequestDto, loginData);

            var json = response.Content.ReadAsStringAsync().Result;

            if (checkStatusCode)
            {
                _apiAccess.CheckStatusCode(route, response);
            }

            return json;
        }

        // on trading platform
        public IWithdrawalTpApi CreateWithdrawalPipeApi(string url,
            GetLoginResponse loginData, int withdrawalAmount, Steps steps = null)
        {
            PostPendingWithdrawalRequest(url, loginData, withdrawalAmount);

            return this;
        }

        public string GetAvailableWithdrawalRequest(string url, GetLoginResponse loginData, bool checkStatusCode = true)
        {
            var route = url + ApiRouteAggregate.AvailableWithdrawalRequest();
            var response = _apiAccess.ExecuteGetEntry(route, loginData);

            var json = response.Content.ReadAsStringAsync().Result;

            if (checkStatusCode)
            {
                _apiAccess.CheckStatusCode(route, response);
            }

            return json;
        }

        public string PostCancelPendingWithdrawalRequest(string url, int WithdrawalId,
            GetLoginResponse loginData, bool checkStatusCode = true)
        {
            var route = ApiRouteAggregate.PostCancelPendingWithdrawal();
            route = $"{url}{route}{WithdrawalId}";

            var cancelWithdrawalRequestDto = new
            {
                delete_reason = "User closed",
            };

            var response = _apiAccess.ExecuteDeleteEntry(route,
                cancelWithdrawalRequestDto, loginData);

            var json = response.Content.ReadAsStringAsync().Result;

            if (checkStatusCode)
            {
                _apiAccess.CheckStatusCode(route, response);
            }

            return json;
        }

        public IWithdrawalTpApi SetAmount(int? amount)
        {
            _amount = amount ?? 10;

            return this;
        }

        public T ChangeContext<T>(IWebDriver driver) where T : class
        {
            return _apiFactory.ChangeContext<T>(_driver);
        }
    }
}
