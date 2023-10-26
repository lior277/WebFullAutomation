// Ignore Spelling: api Crypto app Forex

using AirSoftAutomationFramework.Internals.DAL.ApiAccess;
using AirSoftAutomationFramework.Internals.DAL.ApiAccess.ApiRoutes;
using AirSoftAutomationFramework.Internals.DAL.ConfigurationManagement;
using AirSoftAutomationFramework.Internals.Factory;
using AirSoftAutomationFramework.Objects.DTOs;
using Microsoft.Graph.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using OpenQA.Selenium;
using System.Collections.Generic;

namespace AirSoftAutomationFramework.Objects.ForexObjects.Api.Trade
{
    public class TradeGroupApi : ITradeGroupApi
    {
        #region Members       
        private IApiAccess _apiAccess;
        private string _apiKey = Config.appSettings.ApiKey;
        private readonly IApplicationFactory _apiFactory;
        private IWebDriver _driver;
        #endregion Members

        public TradeGroupApi(IApplicationFactory appFactory,
            IWebDriver driver, IApiAccess apiAccess)
        {
            _apiFactory = appFactory;
            _driver = driver;
            _apiAccess = apiAccess;
        }

        public string PostCreateTradeGroupRequest(string uri, List<object> assetsTypes,
            string groupName, string apiKey = null, bool checkStatusCode = true)
        {
            _apiKey = apiKey ?? _apiKey;
            var route = ApiRouteAggregate.CfdGroup();
            route = $"{uri}{route}?api_key={_apiKey}";
            var tradeGroupInstance = new TradeGroup();
            tradeGroupInstance.name = groupName;

            foreach (var assetType in assetsTypes)
            {
                var typeName = assetType?.GetType().Name;

                tradeGroupInstance
                     .GetType()
                     .GetProperty(typeName.ToLower())
                     .SetValue(tradeGroupInstance, assetType);

                tradeGroupInstance = BuildTradeGroupInstance(tradeGroupInstance);
            }
            var json = JsonConvert.SerializeObject(tradeGroupInstance);
            var dto = JObject.Parse(json);

            dto.Property("_id")?.Remove();
            dto.Property("default")?.Remove();
            dto.Property("last_update")?.Remove();
            var response = _apiAccess.ExecutePostEntry(route, dto);
            json = response.Content.ReadAsStringAsync().Result;

            if (checkStatusCode)
            {
                _apiAccess.CheckStatusCode(route, response);

                return JsonConvert.DeserializeObject<string>(json);
            }

            return json;
        }

        private TradeGroup BuildTradeGroupInstance(TradeGroup cryptoGroup)
        {
            cryptoGroup.assets ??= new Assets
            {
                APPLE = new Apple()
            };

            cryptoGroup.default_attr ??= new Default_Attr { leverage = 1 };
            cryptoGroup.stk ??= new Stk();
            cryptoGroup.cash ??= new Cash();
            cryptoGroup.cmdty ??= new Cmdty();
            cryptoGroup.ind ??= new Ind();
            cryptoGroup.forex ??= new Forex();
            cryptoGroup.commodities ??= new Commodities();
            cryptoGroup.indices ??= new Indices();
            cryptoGroup.stock ??= new Stock();
            cryptoGroup.crypto ??= new Crypto();
            cryptoGroup.futures ??= new Futures();

            return cryptoGroup;
        }

        public GeneralResult<List<TradeGroup>> GetTradeGroupsRequest(string uri,
            string apiKey = null, bool checkStatusCode = true)
        {
            _apiKey = apiKey ?? _apiKey;
            var generalResult = new GeneralResult<List<TradeGroup>>();
            var route = ApiRouteAggregate.CfdGroup();
            route = $"{uri}{route}?api_key={_apiKey}";
            var response = _apiAccess.ExecuteGetEntry(route);
            var json = response.Content.ReadAsStringAsync().Result;

            if (checkStatusCode)
            {
                _apiAccess.CheckStatusCode(route, response);
                generalResult.GeneralResponse = JsonConvert
                    .DeserializeObject<List<TradeGroup>>(json);
            }
            else
            {
                generalResult.Message = json;
            }

            return generalResult;
        }

        public GetAuditTradeGroupResponse GetTradeGroupsByRevisionIdRequest(string uri,
            string tradeGroupId, int revisionId)
        {
            var route = ApiRouteAggregate.GetTradeGroupByRevisionId(tradeGroupId, revisionId);
            route = $"{uri}{route}?api_key={_apiKey}";
            var response = _apiAccess.ExecuteGetEntry(route);
            var json = response.Content.ReadAsStringAsync().Result;
            _apiAccess.CheckStatusCode(route, response);     

            return JsonConvert.DeserializeObject<GetAuditTradeGroupResponse>(json);
        }

        public List<GetTradeGroupsRevisionsResponse> GetTradeGroupsRevisionsRequest(string uri,
            string tradeGroupId, bool checkStatusCode = true)
        {
            var route = ApiRouteAggregate.GetTradeGroupsRevisions(tradeGroupId);
            route = $"{uri}{route}?api_key={_apiKey}";
            var response = _apiAccess.ExecuteGetEntry(route);
            var json = response.Content.ReadAsStringAsync().Result;
            _apiAccess.CheckStatusCode(route, response);

            return JsonConvert.DeserializeObject<List<GetTradeGroupsRevisionsResponse>>(json);
        }

        public string PutEditTradeGroupRequest(string uri, string groupId,
            TradeGroup CryptoGroup, string apiKey = null, bool checkStatusCode = true)
        {
            var json = JsonConvert.SerializeObject(CryptoGroup);
            var dto = JObject.Parse(json);

            if (CryptoGroup.assets.APPLE == null)
            {
                var temp = (JObject)dto.SelectToken("assets");
                temp.Property("APPLE").Remove();
            }

            dto.Property("_id").Remove();
            //dto.Property("default").Remove();
            dto.Property("last_update").Remove();
            _apiKey = apiKey ?? _apiKey;
            var route = ApiRouteAggregate.CfdGroup();
            route = $"{uri}{route}/{groupId}?api_key={_apiKey}";
            var response = _apiAccess.ExecutePutEntry(route, dto);
            json = response.Content.ReadAsStringAsync().Result;

            if (checkStatusCode)
            {
                _apiAccess.CheckStatusCode(route, response);
            }

            return json;
        }

        public string DeleteTradeGroupRequest(string uri, string groupId,
            string apiKey = null, bool checkStatusCode = true)
        {
            _apiKey = apiKey ?? _apiKey;
            var route = $"{uri}{ApiRouteAggregate.CfdGroup()}/{groupId}?api_key={_apiKey}";
            var response = _apiAccess.ExecuteDeleteEntry(route);
            var json = response.Content.ReadAsStringAsync().Result;

            if (checkStatusCode)
            {
                _apiAccess.CheckStatusCode(route, response);
            }

            return json;
        }

        public ITradeGroupApi DeleteTradeGroupRequest(string uri, List<string> groupsIds,
            bool checkStatusCode = true)
        {
            if (groupsIds.Count != 0 || groupsIds != null)
            {
                foreach (var id in groupsIds)
                {
                    if (id != null)
                    {
                        DeleteTradeGroupRequest(uri, id);
                    }
                }
            }

            return this;
        }

        public T ChangeContext<T>(IWebDriver driver) where T : class
        {
            return _apiFactory.ChangeContext<T>(_driver);
        }
    }
}
