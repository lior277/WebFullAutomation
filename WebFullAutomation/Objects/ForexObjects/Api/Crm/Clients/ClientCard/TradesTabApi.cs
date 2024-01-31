using System.Collections.Generic;
using AirSoftAutomationFramework.Internals.DAL.ApiAccsess;
using AirSoftAutomationFramework.Internals.DAL.ApiAccsess.ApiRoutes;
using AirSoftAutomationFramework.Internals.DAL.ConfigurationManagement;
using AirSoftAutomationFramework.Internals.Factorys;
using AirSoftAutomationFramework.Objects.DTOs;
using Newtonsoft.Json;
using OpenQA.Selenium;

namespace AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.Clients.ClientCard
{
    public class TradesTabApi : ITradesTabApi
    {
        #region Members
        private IApiAccess _apiAccess;
        private string _apiKey = Config.appSettings.ApiKey;
        private readonly IApplicationFactory _apiFactory;
        #endregion Members

        public TradesTabApi(IApplicationFactory appFactory, IApiAccess apiAccess)
        {
            _apiFactory = appFactory;
            _apiAccess = apiAccess;
        }

        public ITradesTabApi PachtReOpenTradeRequest(string url, string tradeId)
        {
            var route = $"{url}{ApiRouteAggregate.PatchReOpenTrade()}?api_key={_apiKey}";

            var pachtReOpenTrade = new
            {
                trades_ids = new string[] { tradeId },
            };
            var response = _apiAccess.ExecutePatchEntry(route, pachtReOpenTrade);
            _apiAccess.CheckStatusCode(route, response);

            return this;
        }

        public ITradesTabApi DeleteTradeRequest(string url, string tradeId)
        {
            var route = $"{url}{ApiRouteAggregate.DeleteTrade(tradeId)}?api_key={_apiKey}";
            var response = _apiAccess.ExecuteDeleteEntry(route);
            _apiAccess.CheckStatusCode(route, response);

            return this;
        }

        public string DeleteTradeRequest(string url, string tradeId,
            string apiKey, bool checkStatusCode = true)
        {
            var route = $"{url}{ApiRouteAggregate.DeleteTrade(tradeId)}?api_key={apiKey}";
            var response = _apiAccess.ExecuteDeleteEntry(route);

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

        public string PachtEditTradeByIdRequest(string url, string tradeId,
            double swapCommission, double swapLong,
            double swapShort, string status, int commision, string closeAtLoss = null,
            string closeAtProfit = null, string apiKey = null, bool checkStatusCode = true)
        {
            var route = ApiRouteAggregate.PatchEditTrade();
            _apiKey = apiKey ?? _apiKey;
            route = $"{url}{route}{tradeId}?api_key={_apiKey}";

            var patchEditTrade = new
            {
                close_at_profit = closeAtProfit,
                close_at_loss = closeAtLoss,
                commision = commision,
                status = status,
                swap_commission = swapCommission,
                swap_long = swapLong,
                swap_short = swapShort,
            };
            var response = _apiAccess.ExecutePatchEntry(route, patchEditTrade);
            var json = response.Content.ReadAsStringAsync().Result;

            if (checkStatusCode)
            {
                _apiAccess.CheckStatusCode(route, response);
            }

            return json;
        }

        public string PachtEditSwapByTradeIdRequest(string url, string tradeId,
            double swapCommission, double swapLong,
            double swapShort, string apiKey = null, bool checkStatusCode = true)
        {
            var route = ApiRouteAggregate.PatchEditSwap();
            _apiKey = apiKey ?? _apiKey;
            route = $"{url}{route}{tradeId}?api_key={_apiKey}";

            var patchEditTrade = new
            {
                swap_commission = swapCommission,
                swap_long = swapLong,
                swap_short = swapShort,
            };
            var response = _apiAccess.ExecutePatchEntry(route, patchEditTrade);
            var json = response.Content.ReadAsStringAsync().Result;

            if (checkStatusCode)
            {
                _apiAccess.CheckStatusCode(route, response);
            }

            return json;
        }

        public GeneralResult<List<GetTradesResponse>> GetTradesRequest(string url,
            string clientId, string apiKey = null, bool checkStatusCode = true)
        {
            _apiKey = apiKey ?? _apiKey;
            var route = $"{url}{ApiRouteAggregate.GetTrades(clientId)}?api_key={_apiKey}";
            var generalResult = new GeneralResult<List<GetTradesResponse>>();  
            var response = _apiAccess.ExecuteGetEntry(route);

            var json = response.Content.ReadAsStringAsync().Result;

            if (checkStatusCode)
            {
                _apiAccess.CheckStatusCode(route, response);

                generalResult.GeneralResponse =  JsonConvert.
                    DeserializeObject<List<GetTradesResponse>>(json,
                    new JsonSerializerSettings
                    { NullValueHandling = NullValueHandling.Ignore });
            }
            else
            {
                generalResult.Message = json;
            }

            return generalResult;        
        }

        public T ChangeContext<T>(IWebDriver driver) where T : class
        {
            return _apiFactory.ChangeContext<T>(driver);
        }
    }
}
