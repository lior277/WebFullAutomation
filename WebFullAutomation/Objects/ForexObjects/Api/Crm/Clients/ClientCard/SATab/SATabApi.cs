using AirSoftAutomationFramework.Internals.DAL.ConfigurationManagement;
using AirSoftAutomationFramework.Internals.ExtensionsMethods;
using AirSoftAutomationFramework.Objects.DTOs;
using Newtonsoft.Json;
using OpenQA.Selenium;
using System.Threading;
using AirSoftAutomationFramework.Internals.DAL.ApiAccsess;
using AirSoftAutomationFramework.Internals.DAL.ApiAccsess.ApiRoutes;
using AirSoftAutomationFramework.Internals.Factorys;
using AirSoftAutomationFramework.Models;

namespace AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.Clients.ClientCard.SATab
{
    public class SATabApi : ISATabApi
    {
        #region Members       
        private readonly IApplicationFactory _apiFactory;
        private IApiAccess _apiAccess;
        //private string _clientId;
        private IWebDriver _driver;
        private string _apiKey = Config.appSettings.ApiKey;
        #endregion Members

        public SATabApi(IApplicationFactory apiFactory, IWebDriver driver, IApiAccess apiAccess)
        {
            _apiFactory = apiFactory;
            _driver = driver;
            _apiAccess = apiAccess;
        }

        public string PostTransferToSavingAccountRequest(string url, string clientId,
            int sAAmount, string apiKey = null, bool checkStatusCode = true)
        {
            _apiKey = apiKey ?? _apiKey;

            var route = ApiRouteAggregate.PostTransferToSA();
            route = $"{url}{route}?api_key={_apiKey}";

            var createTransferToSARequestDto = new
            {
                user_id = clientId,
                amount = sAAmount,
            };

            var response = _apiAccess.ExecutePostEntry(route,
                createTransferToSARequestDto);

            if (checkStatusCode)
            {
                _apiAccess.CheckStatusCode(route, response);
            }

            return response.Content.ReadAsStringAsync().Result;
        }

        public GetSaBalanceByUserIdResponse GetSaBalanceByClientIdRequest(string url,
            string clientId, string apiKey = null)
        {
            _apiKey = apiKey ?? _apiKey;
            var route = ApiRouteAggregate.GetSaBalanceByClientId();
            route = $"{url}{route}{clientId}?api_key={_apiKey}";
            var response = _apiAccess.ExecuteGetEntry(route);
            _apiAccess.CheckStatusCode(route, response);
            var json = response.Content.ReadAsStringAsync().Result;

            return JsonConvert.DeserializeObject<GetSaBalanceByUserIdResponse>(json);
        }

        public ISATabApi CreateSaProfit(string url,
            GetActivitiesResponse getActivitiesResponse,
            int amount, int balance, string actionType = "profit")
        {
            var dbContext = new QaAutomation01Context();

            var Sa = dbContext.Set<UsersSavingAccount>();

            Sa.Add(new UsersSavingAccount
            {
                ActionType = actionType,
                UserId = getActivitiesResponse.user_id,
                CreatedAt = getActivitiesResponse.created_at,
                Amount = amount,
                Balance = balance,
                SaId = getActivitiesResponse._id,
                SaPercentage = 1
            });

            dbContext.VerifySaveForSqlManipulation();
            //Thread.Sleep(1000); // wait for transaction to finish

            return this;
        }

        public string PostTransferToBalanceRequest(string url, string clientId,
            int amount, string apiKey = null, bool checkStatusCode = true)
        {
            _apiKey = apiKey ?? _apiKey;
            var route = ApiRouteAggregate.PostTransferToBalance();
            route = $"{url}{route}?api_key={_apiKey}";

            var createTransferToBalanceRequest = new
            {
                user_id = clientId,
                amount,
            };

            var response = _apiAccess.ExecutePostEntry(route, createTransferToBalanceRequest);

            if (checkStatusCode)
            {
                _apiAccess.CheckStatusCode(route, response);
            }

            return response.Content.ReadAsStringAsync().Result;
        }
    }
}
