// Ignore Spelling: api mongo

using System.Linq;
using AirSoftAutomationFramework.Internals.DAL.ApiAccsess;
using AirSoftAutomationFramework.Internals.DAL.ApiAccsess.ApiRoutes;
using AirSoftAutomationFramework.Internals.DAL.ConfigurationManagement;
using AirSoftAutomationFramework.Internals.DAL.MongoDb;
using AirSoftAutomationFramework.Internals.Factorys;
using AirSoftAutomationFramework.Internals.Helpers;
using AirSoftAutomationFramework.Objects.DTOs;
using AirSoftAutomationFramework.Objects.MgmObjects.Api;
using OpenQA.Selenium;
using static AirSoftAutomationFramework.Objects.DTOs.CreateBroadcastRequest;

namespace AirSoftAutomationFramework.Objects.ForexObjects.ApiGeneral
{
    public class BroadcastMessageApi : IBroadcastMessageApi
    {
        #region Members       
        private readonly IApplicationFactory _apiFactory;
        private IWebDriver _driver;
        private IApiAccess _apiAccess;
        private IMongoDbAccess _mongoDbAccess;
        private string _apiKey = Config.appSettings.ApiKey;
        #endregion Members

        public BroadcastMessageApi(IApplicationFactory apiFactory, IWebDriver driver,
            IApiAccess apiAccess, IMongoDbAccess mongoDbAccess)
        {
            _mongoDbAccess = mongoDbAccess;
            _apiFactory = apiFactory;
            _driver = driver;
            _apiAccess = apiAccess;
        }

        public IBroadcastMessageApi MgmPostCreateBroadcastMessageRequest(string uri,
            GetLoginResponse loginData)
        {
            var route = ApiRouteAggregate.PostCreateBroadcastMessageMgm();
            route = uri + route;

            var brandId = _apiFactory
                .ChangeContext<IMgmDashboardApi>()
                .GetBrandsRequest(DataRep.MgmUrl, loginData)
                .FirstOrDefault()
                .Id;

            var brand = new Brand
            {
                _id = brandId,
                name = "qa-auto01-crm"
            };

            var message = new Message
            {
                subject = "Automation Message",
                body = "QXV0b21hdGlvbiBNZXNzYWdl"
            };

            var broadcastDto = new CreateBroadcastRequest
            {
                brands = new Brand[] { brand },
                message = message,

                list_of_all_roles = new string[] { "admin", "affiliate", "agent", "brand_super_admin",
                    "default", "ib", "manager", "support", "other" },

                target_roles = new string[] { "admin", "agent" }
            };

            var response = _apiAccess.ExecutePostEntry(route, broadcastDto, loginData);
            _apiAccess.CheckStatusCode(route, response);

            return this;
        }

        public IBroadcastMessageApi PatchAllMessagesStatusRequest(string uri)
        {
            var route = ApiRouteAggregate.PatchAllMessagesStatus();
            route = $"{uri}{route}?api_key={_apiKey}";
            var response = _apiAccess.ExecutePatchEntry(route);
            _apiAccess.CheckStatusCode(route, response);

            return this;
        }

        public string CrmPostCreateBroadcastMessageRequest(string uri, string apiKey = null,
            bool checkStatusCode = true)
        {
            var route = ApiRouteAggregate.PostCreateBroadcastMessageCrm();
            route = $"{uri}{route}?api_key={apiKey}";

            var brand = new Brand
            {
                _id = "5f5f4d39e394bff2557430db",
                name = "qa-auto01-crm"
            };

            var message = new Message
            {
                subject = "Automation Message",
                body = "QXV0b21hdGlvbiBNZXNzYWdl"
            };

            var broadcastDto = new CreateBroadcastRequest
            {
                brands = new Brand[] { brand },
                message = message,

                list_of_all_roles = new string[] { "admin", "affiliate", "agent", "brand_super_admin",
                    "default", "ib", "manager", "support", "other" },

                target_roles = new string[] { "admin", "agent" }
            };

            var response = _apiAccess.ExecutePostEntry(route, broadcastDto);
            response = _apiAccess.ExecutePostEntry(route, broadcastDto);
            var json = response.Content.ReadAsStringAsync().Result;

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
