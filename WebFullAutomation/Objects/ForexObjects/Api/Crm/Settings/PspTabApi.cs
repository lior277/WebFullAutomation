using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using AirSoftAutomationFramework.Internals.DAL.ApiAccsess;
using AirSoftAutomationFramework.Internals.DAL.ApiAccsess.ApiRoutes;
using AirSoftAutomationFramework.Internals.DAL.ConfigurationManagement;
using AirSoftAutomationFramework.Internals.DAL.MongoDb;
using AirSoftAutomationFramework.Internals.ExtensionsMethods;
using AirSoftAutomationFramework.Internals.Factorys;
using AirSoftAutomationFramework.Internals.Helpers;
using AirSoftAutomationFramework.Objects.DTOs;
using Newtonsoft.Json;
using OpenQA.Selenium;

namespace AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.Settings
{
    public class PspTabApi : IPspTabApi
    {
        #region Members  
        private string _apiKey = Config.appSettings.ApiKey;
        private string _crmUrl = Config.appSettings.CrmUrl;
        private readonly IApplicationFactory _apiFactory;
        private IApiAccess _apiAccess;
        private IMongoDbAccess _mongoDbAccess;
        private IWebDriver _driver;
        #endregion Members

        public PspTabApi(IApplicationFactory apiFactory,
            IWebDriver driver, IApiAccess apiAccess, IMongoDbAccess mongoDbAccess)
        {
            _mongoDbAccess = mongoDbAccess;
            _apiFactory = apiFactory;
            _driver = driver;
            _apiAccess = apiAccess;
        }

        public List<GetPspInstanceResponse> GetPspInstancesByNameRequest(string pspName)
        {
            var route = $"{_crmUrl}{ApiRouteAggregate.GetPspInstances()}?api_key={_apiKey}";
            var response = _apiAccess.ExecuteGetEntry(route);
            _apiAccess.CheckStatusCode(route, response);
            var json = response.Content.ReadAsStringAsync().Result;

            return JsonConvert.DeserializeObject<List<GetPspInstanceResponse>>(json)
                 .Where(r => r.PspName.Equals(pspName)).ToList();
        }

        public string GetPspIdByNameFromMongo(string pspName)
        {
            var mongoDatabase = InitializeMongoClient.ConnectToCrmMongoDb;

            // get psp id
            return _mongoDbAccess
                .SelectAllDocumentsFromTable<PspMongoTable>(mongoDatabase,
                DataRep.PspTable)?
                .Where(p => p.psp_name == pspName)
                .FirstOrDefault()
                ._id;
        }

        // create the psp in crm if not exist
        private IPspTabApi GetPspPspListMongo(string url)
        {
            var route = $"{url}{ApiRouteAggregate.GetPspList()}?api_key={_apiKey}";
            var response = _apiAccess.ExecuteGetEntry(route);
            _apiAccess.CheckStatusCode(route, response);

            return this;
        }

        public List<GetPspInstanceResponse> PostCreateAirsoftSandboxPspRequest(string url)
        {
            GetPspPspListMongo(url);
            var pspId = GetPspIdByNameFromMongo(DataRep.AirsoftSandboxPspName);
            var psp = GetPspInstancesByNameRequest(DataRep.AirsoftSandboxPspName);

            if (psp.Count == 0)
            {
                var route = ApiRouteAggregate.PostCreatePsp(pspId);
                route = $"{url}{route}?api_key={_apiKey}";

                var form = new MultipartFormDataContent
                {
                    { new StringContent("true"), "active" },
                     { new StringContent(DataRep.DefaultUSDCurrencyName), "convert_to" },
                     { new StringContent("{\"merchantId\":\"Airsoftest\",\"apiKey\":" +
                     "\"9429771322F9A\",\"private___apiSecret\":\"ZYxkCxuik5\",\"options\":" +
                     "{\"redirectByEmail\":true}}"), "credentials" },

                     { new StringContent("Airsoft sandbox"), "display_name" },
                     { new StringContent("Automation"), "free_text" },
                     { new StringContent("false"), "have_dod" },
                     { new StringContent("true"), "show_converted_to" },
                     { new StringContent("false"), "show_in_iframe" },
                     { new StringContent("false"), "allow_change_currency_on_front" },
                     { new StringContent("true"), "test_mode" }
                };

                var response = _apiAccess.ExecutePostEntry(route, form);
                _apiAccess.CheckStatusCode(route, response);
                psp = GetPspInstancesByNameRequest(DataRep.AirsoftSandboxPspName);
            }

            return psp;
        }

        public IPspTabApi PostPaymentNotificationRequestPipe(string url,
           string cientId, double amount, string currency, string status = "APPROVED")
        {
            // get airsoft Sand box Instances Psp Id
            var airsoftSandboxInstancesPspId = _apiFactory
                 .ChangeContext<IPspTabApi>(_driver)
                 .GetPspInstancesByNameRequest(DataRep.AirsoftSandboxPspName)
                 .Where(r => r.PspName.Equals(DataRep.AirsoftSandboxPspName))?
                 .FirstOrDefault()
                 .Instances
                 .Id;

            var mongoDatabase = InitializeMongoClient.ConnectToCrmMongoDb;
            PspLogsMongoTable tableData = null;

            for (var i = 0; i < 5; i++)
            {
                tableData = _mongoDbAccess
                    .SelectAllDocumentsFromTable<PspLogsMongoTable>(mongoDatabase,
                    DataRep.PspLogsTable)
                    .Where(p => p.create_payment.request.body.user_id.Equals(cientId))
                    .FirstOrDefault();

                if (tableData == null)
                {
                    Thread.Sleep(400);

                    tableData = _mongoDbAccess
                        .SelectAllDocumentsFromTable<PspLogsMongoTable>(mongoDatabase,
                        DataRep.PspLogsTable)
                        .Where(p => p.create_payment.request.body.user_id.Equals(cientId))
                        .FirstOrDefault();
                }
            }
            // get payment order Id
            var orderId = tableData.order_id;

            var route = ApiRouteAggregate.PostPaymentNotification();
            route = $"{url}{route}{airsoftSandboxInstancesPspId}/" +
                $"{DataRep.AirsoftSandboxPspName}";

            // create secret
            var base64Secret = (DataRep.AirsoftSandboxPspMerchantId
                + amount + currency + status + orderId
                + DataRep.AirsoftSandboxPspPrivateApiSecret)
                .ConvertToSha256()
                .EncodeBase64();

            var postPaymentNotification = new
            {
                api_key = DataRep.AirsoftSandboxPspApiKey,
                merchant_id = DataRep.AirsoftSandboxPspMerchantId,
                order_id = orderId,
                amount = amount,
                currency = currency,
                status = status,
                secret = base64Secret,
                card = "4580*********1111",
                //reject_reason:insufficient funds
            };

            var response = _apiAccess.ExecutePostEntry(route, postPaymentNotification);
            _apiAccess.CheckStatusCode(route, response);

            return this;
        }

        public IPspTabApi PostCreateBankTransferPspRequest(string url)
        {
            GetPspPspListMongo(url);
            var pspId = GetPspIdByNameFromMongo("bank-transfer");
            var psp = GetPspInstancesByNameRequest("bank-transfer");

            if (psp.Count == 0)
            {
                var route = ApiRouteAggregate.PostCreatePsp(pspId);

                route = $"{url}{route}?api_key={_apiKey}";
                var credetials = "{\"bankPspTitle\":\"YmFuayB0cmFuc2ZlciB0aXRsZQ==\",\"bankPspBody\":\"PHA+YmFuayB0cmFuc2ZlciBib2R5PGJyPjwvcD4=\"}";

                var form = new MultipartFormDataContent
                {
                    { new StringContent("true"), "active" },
                    { new StringContent("false"), "convert_to" },
                    { new StringContent("[]"), "countries" },
                    { new StringContent("[]"), "currencies" },
                    { new StringContent("Bank transfer"), "display_name" },
                    { new StringContent(""), "free_text" },
                    { new StringContent("false"), "have_dod" },
                    { new StringContent("true"), "hide_min_amount" },
                    { new StringContent("null"), "logo" },
                    { new StringContent(credetials), "credentials" },
                    { new StringContent("true"), "show_converted_to" },
                    { new StringContent("false"), "show_in_iframe" },
                    { new StringContent("false"), "test_mode" },
                };
                _apiAccess.ExecutePostEntry(route, form);
            }

            return this;
        }

        public IPspTabApi PostCreateAuthorizePspRequest(string url)
        {
            var psp = GetPspInstancesByNameRequest("authorize");
            //var route = ApiRouteAggregate.PostCreatePspCbd(DataRep.CbdAuthorizePspId);
            var route = "";
            var credetials = "{\"private___loginId\":\"4fvYyG8V5fN\",\"private___clientKey\":\"2H92umhGgG85WC6qZhu6cHuQHprP954f9gZDm27cca372rz2HkS34DbLDSY2yTyU\",\"private___transactionKey\":\"76sds6Bg52fQ3JL3\"}";

            if (psp.Count == 0)
            {
                var form = new MultipartFormDataContent
                {
                    { new StringContent("authorize"), "display_name" },
                    { new StringContent("Authorize free_text"), "free_text" },
                    { new StringContent("null"), "logo" },
                    { new StringContent("true"), "test_mode" },
                    { new StringContent("true"), "hide_min_amount" },
                    { new StringContent("false"), "show_in_iframe" },
                    { new StringContent("true"), "have_dod" },
                    { new StringContent("false"), "subscription" },
                    { new StringContent("[]"), "countries" },
                    { new StringContent("true"), "active" },
                    { new StringContent("secured_payment_form"), "integration_type" },
                    { new StringContent(credetials), "credentials" },
                };
                var response = _apiAccess.ExecutePostEntry(route, form);
                _apiAccess.CheckStatusCode(route, response);
            }

            return this;
        }

        public T ChangeContext<T>(IWebDriver driver) where T : class
        {
            return _apiFactory.ChangeContext<T>(_driver);
        }
    }
}
