using AirSoftAutomationFramework.Internals.DAL.ApiAccess;
using AirSoftAutomationFramework.Internals.DAL.ApiAccess.ApiRoutes;
using AirSoftAutomationFramework.Internals.DAL.ConfigurationManagement;
using AirSoftAutomationFramework.Internals.Factory;
using AirSoftAutomationFramework.Internals.Helpers;
using AirSoftAutomationFramework.Objects.DTOs;
using Microsoft.Graph.Models;
using Newtonsoft.Json;
using OpenQA.Selenium;
using System.Collections.Generic;

namespace AirSoftAutomationFramework.Objects.ForexObjects.Api.Trade
{
    public class BulkTradePageApi : IBulkTradePageApi
    {
        #region Members       
        private IApiAccess _apiAccess;
        private readonly IApplicationFactory _apiFactory;
        private IWebDriver _driver;
        private string _apiKey = Config.appSettings.ApiKey;
        #endregion Members

        public BulkTradePageApi(IApplicationFactory appFactory,
          IWebDriver driver, IApiAccess apiAccess)
        {
            _apiFactory = appFactory;
            _driver = driver;
            _apiAccess = apiAccess;
        }

        public GeneralResult<SearchResultBulkTradeHistory>
            GetBulkTradesRequest(string url, string apiKey = null, bool checkStatusCode = true)
        {
            _apiKey = apiKey ?? _apiKey;
            var generalResult = new GeneralResult<SearchResultBulkTradeHistory>();
            var route = ApiRouteAggregate.GetBulkTrades();
            route = $"{url}{route}&api_key={_apiKey}";
            var response = _apiAccess.ExecuteGetEntry(route);

            var json = response
               .Content
               .ReadAsStringAsync()
               .Result;

            if (checkStatusCode)
            {
                _apiAccess.CheckStatusCode(route, response);

                generalResult.GeneralResponse = JsonConvert
                    .DeserializeObject<SearchResultBulkTradeHistory>(json);
            }
            else
            {
                generalResult.Message = json;
            }

            return generalResult;
        }

        public string PostCreateBulkTradeRequest(string url, string[] user_ids,
            string assetSymbol = DataRep.AssetName, string transactionType = "buy",
            string exposure = "100", int takeProfit = 0, int stopLoss = 0,
            double rate = 0, string marketLimit = "", string apiKey = null, bool checkStatusCode = true)
        {
            _apiKey = apiKey ?? _apiKey;
            var route = $"{url}{ApiRouteAggregate.PostCreateBulkTrades()}?api_key={_apiKey}";

            var postCreateBulkTradeDto = new
            {
                asset_symbol = assetSymbol,
                transaction_type = transactionType,
                exposure = exposure,
                take_profit = takeProfit,
                stop_loss = stopLoss,
                rate = rate,
                market_limit = marketLimit,
                user_ids = user_ids
            };

            var response = _apiAccess.ExecutePostEntry(route, postCreateBulkTradeDto);
            var json = response.Content.ReadAsStringAsync().Result;

            if (checkStatusCode)
            {
                _apiAccess.CheckStatusCode(route, response);

                return JsonConvert.DeserializeObject<GeneralDto>(json)
                    .MassTradeId;
            }

            return response.Content.ReadAsStringAsync().Result;
        }

        public string PatchCloseBulkTradeRequest(string url,
            string bulkTradeId, string apiKey = null, bool checkStatusCode = true)
        {
            _apiKey = apiKey ?? _apiKey;
            var route = ApiRouteAggregate.PatchCloseBulkTrade(bulkTradeId);
            route = $"{url}{route}?api_key={_apiKey}";
            var response = _apiAccess.ExecutePatchEntry(route);

            if (checkStatusCode)
            {
                _apiAccess.CheckStatusCode(route, response);
            }

            return response.Content.ReadAsStringAsync().Result;
        }

        public string PatchEditBulkTradeRequest(string url,
            string bulkTradeId, string apiKey = null, bool checkStatusCode = true)
        {
            _apiKey = apiKey ?? _apiKey;
            var route = ApiRouteAggregate.PatchEditBulkTrade(bulkTradeId);
            route = $"{url}{route}?api_key={_apiKey}";
            var response = _apiAccess.ExecutePatchEntry(route);

            if (checkStatusCode)
            {
                _apiAccess.CheckStatusCode(route, response);
            }

            return response.Content.ReadAsStringAsync().Result;
        }

        public List<GetMassTradeByIdRequest> GetMassTradeByIdRequest(string url, string MassTradeId)
        {
            var route = ApiRouteAggregate.GetMassTradeById(MassTradeId);
            route = $"{url}{route}?api_key={_apiKey}";
            var response = _apiAccess.ExecuteGetEntry(route);
            _apiAccess.CheckStatusCode(route, response);
            var json = response.Content.ReadAsStringAsync().Result;

            return JsonConvert
               .DeserializeObject<List<GetMassTradeByIdRequest>>(json);
        }

        public T ChangeContext<T>(IWebDriver driver) where T : class
        {
            return _apiFactory.ChangeContext<T>(_driver);
        }
    }
}
