// Ignore Spelling: Forex Api mongo psp

using System.Linq;
using AirSoftAutomationFramework.Internals.DAL.ApiAccess;
using AirSoftAutomationFramework.Internals.DAL.ApiAccess.ApiRoutes;
using AirSoftAutomationFramework.Internals.DAL.ConfigurationManagement;
using AirSoftAutomationFramework.Internals.DAL.MongoDb;
using AirSoftAutomationFramework.Internals.Factory;
using AirSoftAutomationFramework.Internals.Helpers;
using AirSoftAutomationFramework.Objects.DTOs;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.Settings;
using Newtonsoft.Json;
using OpenQA.Selenium;

namespace AirSoftAutomationFramework.Objects.ForexObjects.TradingPlatform.TradeObjects.TradeApi
{
    public class TradeDepositPageApi : ITradeDepositPageApi
    {
        #region Members       
        private readonly IApplicationFactory _apiFactory;
        private IApiAccess _apiAccess;
        private IWebDriver _driver;
        private IMongoDbAccess _mongoDbAccess;
        private string _apiKey = Config.appSettings.ApiKey;
        #endregion Members

        public TradeDepositPageApi(IApplicationFactory apiFactory,
            IWebDriver driver, IApiAccess apiAccess, IMongoDbAccess mongoDbAccess)
        {
            _apiFactory = apiFactory;
            _driver = driver;
            _apiAccess = apiAccess;
            _mongoDbAccess = mongoDbAccess;
        }

        public object GetDepositPageRequest(string url,
            GetLoginResponse loginData, bool checkStatusCode = true)
        {
            var route = ApiRouteAggregate.GetDepositPage();
            route = url + route;
            var response = _apiAccess.ExecuteGetEntry(route, loginData);

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

        public GeneralDto PostCreatePaymentRequestPipe(string url,
            GetLoginResponse loginData, double? depositAmount = null,
            bool checkStatusCode = true)
        {
            var airsoftSandboxInstancesPspId = _apiFactory
                .ChangeContext<IPspTabApi>(_driver)
                .GetPspInstancesByNameRequest(DataRep.AirsoftSandboxPspName)
                .Where(r => r.PspName.Equals(DataRep.AirsoftSandboxPspName))
                .FirstOrDefault()
                .Instances
                .Id;

            var airsoftSandboxPspId = _apiFactory
                .ChangeContext<IPspTabApi>(_driver)
                .GetPspIdByNameFromMongo(DataRep.AirsoftSandboxPspName);

            var route = ApiRouteAggregate.PostCreatePayment();
            route = url + route;

            var buyer = new Buyer
            {
                firstname = "first name",
                lastname = "last name",
                birthday = "03/08/2002",
                address = "address",
                city = "city",
                zip = "44444",
                country = "AF",
                state = "",
                phone = "4444",
                email = "email@auto.local",
                update_profile = false
            };
            var CreatePaymentDto = new CreatePaymentRequest
            {
                _id = airsoftSandboxPspId,
                instance_id = airsoftSandboxInstancesPspId,
                buyer = buyer,
                amount = depositAmount ?? 8
            };
            var response = _apiAccess.ExecutePostEntry(route,
                CreatePaymentDto, loginData);

            var json = response
                .Content
                .ReadAsStringAsync()
                .Result;

            if (checkStatusCode)
            {
                _apiAccess.CheckStatusCode(route, response);

                return JsonConvert.DeserializeObject<GeneralDto>(json);
            }

            return JsonConvert.DeserializeObject<GeneralDto>(json);
        }

        public ITradeDepositPageApi PostSendCustomPspBankDetailsRequest(string url,
            string pspBody, string pspTitle, GetLoginResponse loginData)
        {
            var route = ApiRouteAggregate.PostSendCustomPspBankDetails();
            route = url + route;
            var SendCustomPspBank = new
            {
                body = $"<p>{pspBody}</p>",
                title = pspTitle
            };
            var response = _apiAccess.ExecutePostEntry(route, SendCustomPspBank, loginData);
            _apiAccess.CheckStatusCode(route, response);

            return this;
        }

        public ITradeDepositPageApi PostSendCustomPspWalletDetailsRequest(string url, string pspBody,
            string pspTitle, string pspFooter, string pspWallet, GetLoginResponse loginData)
        {
            var route = ApiRouteAggregate.PostSendCustomPspWalletDetails();
            route = url + route;
            var SendCustomPspBank = new
            {
                body = $"<p>{pspBody}</p>",
                footer = $"<p>{pspFooter}</p>",
                title = $"{pspTitle}",
                wallet = $"{pspWallet}"
            };
            var response = _apiAccess.ExecutePostEntry(route, SendCustomPspBank, loginData);
            _apiAccess.CheckStatusCode(route, response);

            return this;
        }

        public T ChangeContext<T>(IWebDriver driver) where T : class
        {
            return _apiFactory.ChangeContext<T>(_driver);
        }
    }
}
