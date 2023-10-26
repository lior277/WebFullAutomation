// Ignore Spelling: Forex Chrono sql Api

using AirSoftAutomationFramework.Internals.DAL.ApiAccess;
using AirSoftAutomationFramework.Internals.DAL.ApiAccess.ApiRoutes;
using AirSoftAutomationFramework.Internals.DAL.ConfigurationManagement;
using AirSoftAutomationFramework.Internals.DAL.Sql;
using AirSoftAutomationFramework.Internals.Factory;
using AirSoftAutomationFramework.Objects.DTOs;
using AirSoftAutomationFramework.Objects.ForexObjects.TradingPlatform.TradeObjects.TradeApi;
using Newtonsoft.Json;
using NUnit.Framework;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace AirSoftAutomationFramework.Objects.ForexObjects.TradingPlatform.TradeObjects.TradeApi
{
    public class ChronoTradePageApi : IChronoTradePageApi
    {
        #region Members       
        private readonly IApplicationFactory _apiFactory;
        private IApiAccess _apiAccess;
        private ISqlDbAccess _sqlDbAccess;
        private IWebDriver _driver;
        private string _apiKey = Config.appSettings.ApiKey;
        #endregion Members

        public ChronoTradePageApi(ISqlDbAccess sqlDbAccess, IApplicationFactory apiFactory, IWebDriver driver, IApiAccess apiAccess)
        {
            _sqlDbAccess = sqlDbAccess;
            _apiFactory = apiFactory;
            _driver = driver;
            _apiAccess = apiAccess;
        }

        public GeneralDto PostSellChronoAssetApi(string url, GetLoginResponse loginData,
            string actualStatus = "open", string tradeTimeEnd = "30s", string assetSymbol = "ETHUSD")
        {
            var route = ApiRouteAggregate.PostSellAsset();
            route = url + route;
            var sellAssetRequestDto = new
            {
                asset_symbol = assetSymbol,
                chrono_leverage = 2,
                chrono_trade = true,
                transaction_type = "sell",
                trade_time_end = tradeTimeEnd,
                amount = 1,
                status = actualStatus,
            };
            var response = _apiAccess.ExecutePostEntry(route, sellAssetRequestDto, loginData);
            _apiAccess.CheckStatusCode(route, response);

            var json = response
               .Content
               .ReadAsStringAsync()
               .Result;

            return JsonConvert.DeserializeObject<GeneralDto>(json);
        }

        public GeneralResult<GeneralDto> PostBuyChronoAssetApi(string url,
            GetLoginResponse loginData, string actualStatus = "open",
            string tradeTimeEnd = "30s", string assetSymbol = "ETHUSD",
            bool checkStatusCode = true)
        {
            var generalResult = new GeneralResult<GeneralDto>();
            var route = ApiRouteAggregate.PostBuyAsset();
            route = url + route;
            var createTradeRequestDto = new
            {
                asset_symbol = assetSymbol,
                chrono_leverage = 2,
                chrono_trade = true,
                transaction_type = "buy",
                trade_time_end = tradeTimeEnd,
                amount = 1,
                status = actualStatus,
            };
            var response = _apiAccess.ExecutePostEntry(route, createTradeRequestDto, loginData);
            var json = response.Content.ReadAsStringAsync().Result;

            if (checkStatusCode)
            {
                _apiAccess.CheckStatusCode(route, response);
                generalResult.GeneralResponse = JsonConvert.DeserializeObject<GeneralDto>(json);
            }
            else
            {
                generalResult.Message = json;
            }

            return generalResult;
        }

        public string PatchCloseChronoTradeRequest(string url, string tradeId,
            GetLoginResponse loginData, bool checkStatusCode = true)
        {
            var route = ApiRouteAggregate.PatchCloseChronoTrade(tradeId);
            route = url + route;

            var response = _apiAccess.ExecutePatchEntry(route, loginData);

            if (checkStatusCode)
            {
                _apiAccess.CheckStatusCode(route, response);
            }

            return response.Content.ReadAsStringAsync().Result;
        }

        public IChronoTradePageApi WaitForChronoTradeToClose(string url, string chronoTradeId,
            GetLoginResponse loginData)
        {
            var actualChronoTradeId = GetChronoTradesRequest(url, loginData)
                .GeneralResponse
                .Where(p => p.id == chronoTradeId && p.status == "close")?
                .FirstOrDefault()?
                .id;

            for (var i = 0; i < 31; i++)
            {
                if (actualChronoTradeId == null)
                {
                    actualChronoTradeId = GetChronoTradesRequest(url, loginData)
                        .GeneralResponse
                        .Where(p => p.id == chronoTradeId && p.status == "close")?
                        .FirstOrDefault()?
                        .id;

                    Thread.Sleep(1000);
                }
                else
                {
                    break;
                }
            }

            if (actualChronoTradeId != chronoTradeId)
            {
                var exceMessage = $" the trade id: {chronoTradeId} is steel open";

                throw new Exception(exceMessage);
            }

            return this;
        }

        public GeneralResult<List<GetTradesResponse>> GetChronoTradesRequest(string url,
            GetLoginResponse loginData, bool checkStatusCode = true)
        {
            var generalResult = new GeneralResult<List<GetTradesResponse>>();
            var route = ApiRouteAggregate.GetChronoTrades();
            route = url + route;
            var response = _apiAccess.ExecuteGetEntry(route, loginData);

            var json = response
               .Content
               .ReadAsStringAsync()
               .Result;

            if (checkStatusCode)
            {
                _apiAccess.CheckStatusCode(route, response);

                generalResult.GeneralResponse = JsonConvert
                    .DeserializeObject<List<GetTradesResponse>>
                    (json, new JsonSerializerSettings
                    { NullValueHandling = NullValueHandling.Ignore });
            }
            else
            {
                generalResult.Message = json;
            }

            return generalResult;
        }

        public IChronoTradePageApi WaitForChronoTradeToClose(string url,
         List<string> chronoTradesIds, GetLoginResponse loginData)
        {
            foreach (var trade in chronoTradesIds)
            {
                WaitForChronoTradeToClose(url, trade, loginData);
            }

            return this;
        }

        public T ChangeContext<T>(IWebDriver driver) where T : class
        {
            return _apiFactory.ChangeContext<T>(_driver);
        }
    }
}
