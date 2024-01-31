using AirSoftAutomationFramework.Internals.DAL.ConfigurationManagement;
using AirSoftAutomationFramework.Internals.ExtensionsMethods;
using AirSoftAutomationFramework.Internals.Helpers;
using AirSoftAutomationFramework.Objects.DTOs;
using AirSoftAutomationFramework.Objects.MgmObjects.Api.Risk.AssetsCfd;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using NUnit.Framework;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using AirSoftAutomationFramework.Internals.DAL.ApiAccsess;
using AirSoftAutomationFramework.Internals.DAL.ApiAccsess.ApiRoutes;
using AirSoftAutomationFramework.Internals.Factorys;
using AirSoftAutomationFramework.Models;
using static AirSoftAutomationFramework.Objects.DTOs.TestCase;

namespace AirSoftAutomationFramework.Objects.ForexObjects.TradingPlatform.TradeObjects.TradeApi
{
    public class NftPageApi : INftPageApi
    {
        #region Members       
        private readonly IApplicationFactory _apiFactory;
        private IApiAccess _apiAccess;
        private IWebDriver _driver;
        private string _apiKey = Config.appSettings.ApiKey;
        #endregion Members

        public NftPageApi(IApplicationFactory apiFactory, IWebDriver driver,
            IApiAccess apiAccess)
        {
            _apiFactory = apiFactory;
            _driver = driver;
            _apiAccess = apiAccess;
        }

        public string PatchCloseNftRequest(string url, string tradeId,
            GetLoginResponse loginData, int tradeAmount = 1, bool checkStatusCode = true)
        {
            var route = ApiRouteAggregate.PatchCloseTrade(tradeId);
            route = url + route;

            var closeTradeDto = new
            {
                amount = tradeAmount,
                nft_trade = true
            };

            var response = _apiAccess.ExecutePatchEntry(route, closeTradeDto, loginData);
            var json = response.Content.ReadAsStringAsync().Result;

            if (checkStatusCode)
            {
                _apiAccess.CheckStatusCode(route, response);

                return json;
            }

            return json;
        }


        public string PatchPendingTradeRequest(string url, string tradeId, int tradeAmount,
            GetLoginResponse loginData, bool checkStatusCode = true)
        {
            var route = ApiRouteAggregate.PatchPendingTrade(tradeId);
            route = url + route;

            var closeTradeDto = new
            {
                amount = tradeAmount
            };

            var response = _apiAccess.ExecutePatchEntry(route, closeTradeDto, loginData);
            var json = response.Content.ReadAsStringAsync().Result;

            if (checkStatusCode)
            {
                _apiAccess.CheckStatusCode(route, response);

                return json;
            }

            return json;
        }

        public INftPageApi PatchCloseTradeWithAmountRequest(string url, string tradeId,
            GetLoginResponse loginData, int amount)
        {
            var route = ApiRouteAggregate.PatchCloseTrade(tradeId);
            route = url + route;

            var patchCloseTrade = new
            {
                amount
            };

            var response = _apiAccess.ExecutePatchEntry(route, patchCloseTrade, loginData);
            _apiAccess.CheckStatusCode(route, response);

            return this;
        }

        public INftPageApi PatchMassCloseTradesRequest(string url, string[] tradeIds,
            GetLoginResponse loginData)
        {
            var route = ApiRouteAggregate.PatchMassCloseTrade();
            route = url + route;

            var tradesIds = new
            {
                trades_ids = tradeIds
            };

            var response = _apiAccess.ExecutePatchEntry(route, tradesIds, loginData);
            _apiAccess.CheckStatusCode(route, response);

            return this;
        }

        public GeneralResult<List<CloseTrade>> GetClosedTradesRequest(string url,
            GetLoginResponse loginData, bool checkStatusCode = true)
        {
            var generalResult = new GeneralResult<List<CloseTrade>>();
            var route = ApiRouteAggregate.GetClosedTrades();
            route = url + route;
            var response = _apiAccess.ExecuteGetEntry(route, loginData);
            _apiAccess.CheckStatusCode(route, response);

            var json = response
                .Content
                .ReadAsStringAsync()
                .Result;

            if (checkStatusCode)
            {
                _apiAccess.CheckStatusCode(route, response);

                generalResult.GeneralResponse = JsonConvert
                    .DeserializeObject<List<CloseTrade>>(json);
            }
            else
            {
                generalResult.Message = json;
            }

            return generalResult;
        }

        public List<GetCfdAssetsNamesResponse> GetCfdAssetsNamesRequest(string url,
           GetLoginResponse loginData)
        {
            var route = ApiRouteAggregate.GetCfdAssetsNames();
            route = url + route;
            var response = _apiAccess.ExecuteGetEntry(route, loginData);
            _apiAccess.CheckStatusCode(route, response);

            var json = response
                .Content
                .ReadAsStringAsync()
                .Result;

            _apiAccess.CheckStatusCode(route, response);

            return JsonConvert.DeserializeObject<List<GetCfdAssetsNamesResponse>>(json);
        }

        public int CalculateTradeAmountForMarginCall(double currentPrice,
            int tradeAmount, double marginPercent, double userBalance, double marginCallPercentage)
        {
            var temp1 = currentPrice * tradeAmount * marginPercent;
            var temp2 = userBalance * marginCallPercentage;

            while (temp1 - temp2 < 0 || temp1 - temp2 == 0)
            {
                tradeAmount++;
                temp1 = currentPrice * tradeAmount * marginPercent;
                temp2 = userBalance * marginCallPercentage;
            }

            return tradeAmount;
        }

        public GeneralResult<List<GetClientTradeCountResponse>>
            GetClientTradesCountRequest(string url,
            GetLoginResponse loginData, bool checkStatusCode = true)
        {
            var generalResult = new GeneralResult<List<GetClientTradeCountResponse>>();
            var route = ApiRouteAggregate.GetTradesCount();
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
                    .DeserializeObject<List<GetClientTradeCountResponse>>(json);
            }
            else
            {
                generalResult.Message = json;
            }

            return generalResult;
        }

        public INftPageApi GetStartMarginCallRequest(string url)
        {
            var route = ApiRouteAggregate.GetStartMarginCall();
            route = $"{url}{route}?api_key={_apiKey}";
            var response = _apiAccess.ExecuteGetEntry(route);
            _apiAccess.CheckStatusCode(route, response);

            return this;
        }

        public INftPageApi PutLanguage(string url,
            GetLoginResponse loginData,
            string clientId, string actualLang = "en")
        {
            var route = $"{url}{ApiRouteAggregate.SetLanguage(clientId)}";

            var languageDto = new
            {
                lang = actualLang
            };
            var response = _apiAccess.ExecutePutEntry(route, languageDto);
            _apiAccess.CheckStatusCode(route, response);

            return this;
        }

        public GeneralResult<TradingLanguage> GetLanguageByCode(string url, string languageCode,
            GetLoginResponse loginData, bool checkStatusCode = true)
        {
            var generalResult = new GeneralResult<TradingLanguage>();
            var route = $"{url}{ApiRouteAggregate.GetErpLanguageByLangCode(languageCode)}";
            var response = _apiAccess.ExecuteGetEntry(route, loginData);

            var json = response
               .Content
               .ReadAsStringAsync()
               .Result;

            if (checkStatusCode)
            {
                _apiAccess.CheckStatusCode(route, response);
                generalResult.GeneralResponse = JsonConvert
                    .DeserializeObject<TradingLanguage>(json);
            }
            else
            {
                generalResult.Message = json;
            }

            return generalResult;
        }

        public INftPageApi PutSetTemplateRequest(string url, string clientId,
            string templateName = "light-template")
        {
            var route = $"{url}{ApiRouteAggregate.PutSetTemplate(clientId)}";

            var SetTemplate = new
            {
                template = templateName
            };
            var response = _apiAccess.ExecutePutEntry(route, SetTemplate);
            _apiAccess.CheckStatusCode(route, response);

            return this;
        }

        public GeneralResult<List<GetTradesResponse>> GetClientTreadsRequest(string url,
            GetLoginResponse loginData, bool checkStatusCode = true)
        {
            var generalResult = new GeneralResult<List<GetTradesResponse>>();
            var route = ApiRouteAggregate.GetTradesByStatus();
            route = url + route;
            var response = _apiAccess.ExecuteGetEntry(route);

            var json = response
                .Content
                .ReadAsStringAsync()
                .Result;

            if (checkStatusCode)
            {
                _apiAccess.CheckStatusCode(route, response);

                generalResult.GeneralResponse = JsonConvert
                    .DeserializeObject<List<GetTradesResponse>>(json);
            }
            else
            {
                generalResult.Message = json;
            }

            return generalResult;
        }

        public INftPageApi PostExportCloseTradeTableRequest(string url,
            string userEmail, string apiKey = null)
        {
            _apiKey = apiKey ?? _apiKey;
            var route = $"{url}{ApiRouteAggregate.PostExportTableWithLink()}?api_key={_apiKey}";

            var routeForExport = ApiRouteAggregate
                .PostExportTradesTables(DataRep.CloseTradesTableName.Split('_').First());

            var today = DateTime.Now.ToString("yyyy-MM-dd");

            var exportDepositsTableDto = new
            {
                export_email = userEmail,
                export_params = $"filter[chrono]=true&filter[start_date]={today} 00:00:00&filter[end_date]={today} 23:59:59&draw=1&order[0][column]=0&order[0][dir]=asc&start=0&length=10000000&search[value]=&search[regex]=true&export=1&table=close_trades&projection[]=id&projection[]=user_id&projection[]=full_name&projection[]=erp_user_name&projection[]=trade_time_start&projection[]=transaction_type&projection[]=label&projection[]=amount&projection[]=rate&projection[]=current_rate&projection[]=trade_close_time&projection[]=profit_loss&projection[]=sltp&projection[]=commision&projection[]=swap_commission&projection[]=&selectedColumns[0][title]=id&selectedColumns[0][name]=id&selectedColumns[1][title]=Client ID&selectedColumns[1][name]=user_id&selectedColumns[2][title]=Client Name&selectedColumns[2][name]=full_name&selectedColumns[3][title]=Sales Agent&selectedColumns[3][name]=erp_user_name&selectedColumns[4][title]=Open time | GMT&selectedColumns[4][name]=trade_time_start&selectedColumns[5][title]=Type&selectedColumns[5][name]=transaction_type&selectedColumns[6][title]=Asset&selectedColumns[6][name]=label&selectedColumns[7][title]=Amount&selectedColumns[7][name]=amount&selectedColumns[8][title]=Open Price&selectedColumns[8][name]=rate&selectedColumns[9][title]=Close Price&selectedColumns[9][name]=current_rate&selectedColumns[10][title]=Close time | GMT&selectedColumns[10][name]=trade_close_time&selectedColumns[11][title]=PNL&selectedColumns[11][name]=profit_loss&selectedColumns[12][title]=SL TP&selectedColumns[12][name]=sltp&selectedColumns[13][title]=Commission&selectedColumns[13][name]=commision&selectedColumns[14][title]=Swap&selectedColumns[14][name]=swap_commission",
                export_table_name = DataRep.CloseTradesTableName,
                service_url = routeForExport
            };
            var response = _apiAccess.ExecutePostEntry(route, exportDepositsTableDto);
            _apiAccess.CheckStatusCode(route, response);

            return this;
        }

        public INftPageApi PostExportOpenTradeTableRequest(string url,
            string userEmail, string apiKey = null)
        {
            _apiKey = apiKey ?? _apiKey;
            var route = $"{url}{ApiRouteAggregate.PostExportTableWithLink()}?api_key={_apiKey}";

            var routeForExport = ApiRouteAggregate
                .PostExportTradesTables(DataRep.OpenTradesTableName.Split('_').First());

            var today = DateTime.Now.ToString("yyyy-MM-dd");

            var exportDepositsTableDto = new
            {
                export_email = userEmail,
                export_params = $"filter[chrono]=true&filter[start_date]={today} 00:00:00&filter[end_date]={today} 23:59:59&draw=1&order[0][column]=0&order[0][dir]=asc&start=0&length=10000000&search[value]=&search[regex]=true&export=1&table=open_trades&projection[]=id&projection[]=user_id&projection[]=full_name&projection[]=erp_user_name&projection[]=trade_time_start&projection[]=transaction_type&projection[]=label&projection[]=amount&projection[]=rate&projection[]=current_rate&projection[]=profit_loss&projection[]=sltp&projection[]=commision&projection[]=swap_commission&projection[]=&selectedColumns[0][title]=id&selectedColumns[0][name]=id&selectedColumns[1][title]=Client ID&selectedColumns[1][name]=user_id&selectedColumns[2][title]=Client Name&selectedColumns[2][name]=full_name&selectedColumns[3][title]=Sales Agent&selectedColumns[3][name]=erp_user_name&selectedColumns[4][title]=Open time | GMT&selectedColumns[4][name]=trade_time_start&selectedColumns[5][title]=Type&selectedColumns[5][name]=transaction_type&selectedColumns[6][title]=Asset&selectedColumns[6][name]=label&selectedColumns[7][title]=Amount&selectedColumns[7][name]=amount&selectedColumns[8][title]=Open Price&selectedColumns[8][name]=rate&selectedColumns[9][title]=Current Price&selectedColumns[9][name]=current_rate&selectedColumns[10][title]=PNL&selectedColumns[10][name]=profit_loss&selectedColumns[11][title]=SL TP&selectedColumns[11][name]=sltp&selectedColumns[12][title]=Commission&selectedColumns[12][name]=commision&selectedColumns[13][title]=Swap&selectedColumns[13][name]=swap_commission",
                export_table_name = DataRep.OpenTradesTableName,
                service_url = routeForExport
            };
            var response = _apiAccess.ExecutePostEntry(route, exportDepositsTableDto);
            _apiAccess.CheckStatusCode(route, response);

            return this;
        }

        public INftPageApi PostExportPendingTradeTableRequest(string url,
            string userEmail, string apiKey = null)
        {
            _apiKey = apiKey ?? _apiKey;
            var route = $"{url}{ApiRouteAggregate.PostExportTableWithLink()}?api_key={_apiKey}";

            var routeForExport = ApiRouteAggregate
                .PostExportTradesTables(DataRep.PendingTradesTableName.Split('_').First());

            var today = DateTime.Now.ToString("yyyy-MM-dd");

            var exportDepositsTableDto = new
            {
                export_email = userEmail,
                export_params = $"filter[start_date]={today} 00:00:00&filter[end_date]={today} 23:59:59&draw=1&order[0][column]=0&order[0][dir]=asc&start=0&length=10000000&search[value]=&search[regex]=true&export=1&table=pending_trades&projection[]=id&projection[]=user_id&projection[]=full_name&projection[]=erp_user_name&projection[]=trade_time_start&projection[]=transaction_type&projection[]=label&projection[]=amount&projection[]=rate&projection[]=current_rate&projection[]=profit_loss&projection[]=sltp&projection[]=commision&projection[]=swap_commission&projection[]=&selectedColumns[0][title]=id&selectedColumns[0][name]=id&selectedColumns[1][title]=Client ID&selectedColumns[1][name]=user_id&selectedColumns[2][title]=Client Name&selectedColumns[2][name]=full_name&selectedColumns[3][title]=Sales Agent&selectedColumns[3][name]=erp_user_name&selectedColumns[4][title]=Open time | GMT&selectedColumns[4][name]=trade_time_start&selectedColumns[5][title]=Type&selectedColumns[5][name]=transaction_type&selectedColumns[6][title]=Asset&selectedColumns[6][name]=label&selectedColumns[7][title]=Amount&selectedColumns[7][name]=amount&selectedColumns[8][title]=Open Price&selectedColumns[8][name]=rate&selectedColumns[9][title]=Current Price&selectedColumns[9][name]=current_rate&selectedColumns[10][title]=PNL&selectedColumns[10][name]=profit_loss&selectedColumns[11][title]=SL TP&selectedColumns[11][name]=sltp&selectedColumns[12][title]=Commission&selectedColumns[12][name]=commision&selectedColumns[13][title]=Swap&selectedColumns[13][name]=swap_commission",
                export_table_name = DataRep.PendingTradesTableName,
                service_url = routeForExport
            };
            var response = _apiAccess.ExecutePostEntry(route, exportDepositsTableDto);
            _apiAccess.CheckStatusCode(route, response);

            return this;
        }

        public List<GetActivitiesResponse> GetActivities(string url,
            GetLoginResponse loginData)
        {
            Thread.Sleep(1000); // wait for activities to resister

            var today = DateTime.Now.ToString("yyyy-MM-dd");
            var route = ApiRouteAggregate.GetActivities(today);
            route = url + route;
            var response = _apiAccess.ExecuteGetEntry(route, loginData);
            _apiAccess.CheckStatusCode(route, response);

            var json = response
               .Content
               .ReadAsStringAsync()
               .Result;

            return JsonConvert.DeserializeObject<List<GetActivitiesResponse>>(json);
        }

        public GetUsersSavingAccounts GetUsersSavingAccountsRequest(string url, GetLoginResponse loginData)
        {
            var today = DateTime.Now.ToString("yyyy-MM-dd");
            var route = ApiRouteAggregate.GetUsersSavingAccounts(today);
            route = url + route;
            var response = _apiAccess.ExecuteGetEntry(route, loginData);
            _apiAccess.CheckStatusCode(route, response);
            var json = response.Content.ReadAsStringAsync().Result;

            return JsonConvert.DeserializeObject<GetUsersSavingAccounts>(json);
        }

        public GeneralResult<GetExportClientActivitiesResponse> GetExportActivitiesRequest(string url,
            string clientId, GetLoginResponse loginData,
            bool fullHistory = false, bool checkStatusCode = true)
        {
            var generalResult = new GeneralResult<GetExportClientActivitiesResponse>();
            var today = DateTime.Now.ToString("yyyy-MM-dd");
            var route = ApiRouteAggregate.GetActivitiesByName(today,
                clientId, fullHistory);

            route = url + route;

            // wait for event to register
            //Thread.Sleep(1000);
            var response = _apiAccess.ExecuteGetEntry(route, loginData);

            var json = response
                .Content
                .ReadAsStringAsync()
                .Result;

            if (checkStatusCode)
            {
                _apiAccess.CheckStatusCode(route, response);

                generalResult.GeneralResponse = JsonConvert
                    .DeserializeObject<GetExportClientActivitiesResponse>(json);
            }
            else
            {
                generalResult.Message = json;
            }

            return generalResult;
        }

        public GetCryptoVsUsdtResponse GetCryptoVsUsdtRequest(string url,
            GetLoginResponse loginData)
        {
            var route = ApiRouteAggregate.GetCfdVsUsdt();
            route = url + route;
            var response = _apiAccess.ExecuteGetEntry(route, loginData);
            _apiAccess.CheckStatusCode(route, response);
            var json = response.Content.ReadAsStringAsync().Result;

            return JsonConvert.DeserializeObject<GetCryptoVsUsdtResponse>(json);
        }

        public INftPageApi UpdateClientPnl(string clientId, string tradeId,
            int pnlAmount = 50)
        {
            //var status = GetTradeStatus(tradeId);
            var _dbContext = new QaAutomation01Context();

            (from s in _dbContext.UserAccounts
             where s.UserId == clientId
             select s)
            .First()
            .ClosedProfitLoss = pnlAmount;

            _dbContext.VerifySaveForSqlManipulation();

            return this;
        }

        public INftPageApi UpdateClientPnl(List<string> clientsIds, List<string> tradesIds,
            int pnlAmount = 50)
        {

            for (var i = 0; i < clientsIds.Count(); i++)
            {
                UpdateClientPnl(clientsIds.ElementAt(i),
                    tradesIds.ElementAt(i), pnlAmount);
            }

            return this;
        }

        public string GetFeedAssetsCfdMajorRequest(string url,
            GetLoginResponse loginData)
        {
            var route = ApiRouteAggregate.GetFeedAssetsCfdMajor();
            route = url + route;
            var response = _apiAccess.ExecuteGetEntry(route, loginData);
            _apiAccess.CheckStatusCode(route, response);

            return response.Content.ReadAsStringAsync().Result;
        }

        public INftPageApi UpdateTradePnl(string tradeId, int pnlAmount = 50)
        {
            //var status = GetTradeStatus(tradeId);
            var _dbContext = new QaAutomation01Context();

            (from s in _dbContext.Trades
             where s.Id.ToString() == tradeId
             select s)
                .First()
                .ClosedProfitLoss = pnlAmount;

            _dbContext.VerifySaveForSqlManipulation();

            return this;
        }

        public INftPageApi UpdateTradePnl(List<string> tradeIds, int pnlAmount = 50)
        {
            foreach (var tradeId in tradeIds)
            {
                UpdateTradePnl(tradeId, pnlAmount);
            }

            return this;
        }

        public int GetTotalPnlFromTradesView(string clientId)
        {
            var _dbContext = new QaAutomation01Context();

            var openPnlValue = _dbContext.TradesViews
                .Where(t => t.UserId == clientId)
                .Sum(p => Math.Abs((decimal)p.ProfitLoss));

            return (int)openPnlValue.MathRoundFromGeneric(0);
        }

        public INftPageApi UpdatelPnlOnTradesView(string clientId, int pnlAmountForEachTrade)
        {
            var _dbContext = new QaAutomation01Context();

            (from s in _dbContext.Trades
             where s.UserId == clientId
             select s)
             .ForEach(p => p.ClosedProfitLoss = pnlAmountForEachTrade);

            //Thread.Sleep(1000);
            _dbContext.VerifySaveForSqlManipulation();
            //Thread.Sleep(1000);

            return this;
        }

        public async Task<string> GetTradeStatus(string tradeId)
        {
            try
            {
                //Thread.Sleep(1000);
                var _dbContext = new QaAutomation01Context();

                var response = await (from s in _dbContext.Trades
                                      select s.Status)
                    .ToListAsync();

                return response
                    .First();
            }
            catch (Exception ex)
            {
                var exceMessage = $" Exception Message: {ex.Message}";

                throw new Exception(exceMessage);
            }
        }

        public GetAppleAsset GetAppleAssetRequest(string url,
          GetLoginResponse loginData)
        {
            var route = ApiRouteAggregate.GetAppleAsset();
            route = url + route;
            var response = _apiAccess.ExecuteGetEntry(route, loginData);
            _apiAccess.CheckStatusCode(route, response);
            var json = response.Content.ReadAsStringAsync().Result;

            return JsonConvert.DeserializeObject<GetAppleAsset>(json);
        }

        public INftPageApi RemoveAssetPipe(string url, string assetName,
            GetLoginResponse _mgmLoginData, GetLoginResponse _tpLoginData)
        {
            var checkIfAssetsExist = _apiFactory
                .ChangeContext<ITradePageApi>()
                .GetCfdAssetsNamesRequest(url, _tpLoginData)
                .Any(p => p.label.Equals(assetName));

            if (checkIfAssetsExist)
            {
                var cfdData = _apiFactory
                    .ChangeContext<IAssetsCfdApi>()
                    .GetCfdRequest(DataRep.MgmUrl, _mgmLoginData);

                // set show_in_front to false
                var asset = cfdData
                    .Where(p => p.label == assetName)
                    .FirstOrDefault();

                asset
                    .cfd
                    .show_in_front = false;

                _apiFactory
                   .ChangeContext<IAssetsCfdApi>()
                   .PatchCfdRequest(_mgmLoginData, asset);
            }

            checkIfAssetsExist = _apiFactory
                .ChangeContext<ITradePageApi>()
                .GetCfdAssetsNamesRequest(url, _tpLoginData)
                .Any(p => p.label.Equals(assetName));

            if (checkIfAssetsExist)
            {
                var exceMessage = (" exception: After removing the" +
                    $" Asset:{assetName} from MAG it is still exist on TP");

                throw new Exception(exceMessage);
            }

            return this;
        }

        public string GetAssetStatusPipe(GetAppleAsset getAppleAsset)
        {
            // getAppleAsset.appleAsset.Times.CustomOpen.FirstOrDefault().Date = new DateTime(2021,12,08);
            var status = false;
            // check if Utc Now is in Custom Open 
            var utcTime = DateTime.UtcNow.ToString("HH:mm:ss");
            var utcTimeNow = TimeSpan.Parse(utcTime);
            var utcDateAndTime = DateTime.UtcNow;

            var customOpen = getAppleAsset.appleAsset.Times.CustomOpen
                .SingleOrDefault(p => p.Date.Date == utcDateAndTime.Date);

            if (customOpen != null)
            {
                status = utcTimeNow.CheckIfInTimeRange(customOpen.From, customOpen.To);

                if (status)
                {
                    return "open";
                }
                else
                {
                    return "close";
                }
            }
            else
            {
                status = utcTimeNow.CheckIfInTimeRange(getAppleAsset.appleAsset.Times.Open.First().From,
                    getAppleAsset.appleAsset.Times.Open.First().To);

                if (status)
                {
                    return "open";
                }
                else
                {
                    return "close";
                }
            }
        }

        public GeneralResult<GeneralDto> PostBuyNftRequest(string url,
            GetLoginResponse loginData, int amount = 1, string status = "open",
            string assetSymble = null, bool checkStatusCode = true)
        {
            var generalResult = new GeneralResult<GeneralDto>();
            var route = ApiRouteAggregate.PostBuyAsset();
            route = url + route;
            var actualassetSymble = assetSymble ?? DataRep.AssetNftLongSymbol;

            var createNftRequestDto = new
            {
                asset_symbol = actualassetSymble,
                transaction_type = "buy",
                amount = amount,
                status = status
            };

            var response = _apiAccess.ExecutePostEntry(route, createNftRequestDto, loginData);

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

        // buy asset when market is closed
        public string PostBuyCloseAssetRequest(string url, int amount,
            GetLoginResponse loginData,
            string status = "open", string assetSymble = null)
        {
            var route = ApiRouteAggregate.PostBuyAsset();
            route = url + route;
            var createTradeRequestDto = new PostCloseTradeRequest
            {
                asset_symbol = assetSymble ?? "APPLE",
                transaction_type = "buy",
                on_asset_open = true,
                amount = amount,
                status = "pending",
            };

            var response = _apiAccess
                .ExecutePostEntry(route, createTradeRequestDto, loginData);

            _apiAccess.CheckStatusCode(route, response);

            return response
                .Content
                .ReadAsStringAsync()
                .Result;
        }

        public GeneralResult<GeneralDto> PostSellAssetRequest(string url, int amount,
            GetLoginResponse loginData, string status = "open",
            string assetSymble = null, bool checkStatusCode = true)
        {
            var generalResult = new GeneralResult<GeneralDto>();
            var route = ApiRouteAggregate.PostSellAsset();
            route = url + route;

            var createTradeRequestDto = new PostTradeRequest
            {
                asset_symbol = assetSymble ?? DataRep.AssetName,
                transaction_type = "sell",
                amount = amount,
                status = status,
            };

            var response = _apiAccess.ExecutePostEntry(route, createTradeRequestDto, loginData);
            var json = response.Content.ReadAsStringAsync().Result;

            if (checkStatusCode)
            {
                _apiAccess.CheckStatusCode(route, response);

                generalResult.GeneralResponse = JsonConvert
                    .DeserializeObject<GeneralDto>(json);
            }
            else
            {
                generalResult.Message = json;
            }

            return generalResult;
        }

        // sell asset when market is closed
        public string PostSellCloseAssetRequest(string url, int amount,
            GetLoginResponse loginData = null,
            string status = "open", string assetSymble = null)
        {
            var route = ApiRouteAggregate.PostSellAsset();
            route = url + route;
            var createTradeRequestDto = new PostCloseTradeRequest
            {
                asset_symbol = assetSymble ?? "APPLE",
                transaction_type = "sell",
                on_asset_open = true,
                amount = amount,
                status = "pending",
            };
            var response = _apiAccess.ExecutePostEntry(route, createTradeRequestDto, loginData);
            _apiAccess.CheckStatusCode(route, response);

            return response
                .Content
                .ReadAsStringAsync()
                .Result;
        }

        public string PostBuyAssetByAgentRequest(string url, string userId, int tradeAmount)
        {
            var route = ApiRouteAggregate.PostBuyAsset();
            route = $"{url}{route}?api_key={_apiKey}";

            var createTradeRequestDto = new
            {
                asset_symbol = DataRep.AssetName,
                transaction_type = "buy",
                amount = tradeAmount,
                status = "open",
                user_id = userId
            };
            var response = _apiAccess.ExecutePostEntry(route, createTradeRequestDto);
            _apiAccess.CheckStatusCode(route, response);

            return response
                .Content
                .ReadAsStringAsync()
                .Result;
        }

        public CreateClosedTradeResponse PostClosedTradesByAgentRequest(string url)
        {
            var route = ApiRouteAggregate.RetrieveClosedTradesByAgent();
            route = $"{url}{route}?api_key={_apiKey}";
            var startDate = DateTime.Now.AddDays(-1).ToUnixTimestamps();
            var endDate = DateTime.Now.AddDays(1).ToUnixTimestamps();

            var createTradeRequestDto = new
            {
                start_date = startDate,
                end_date = endDate,
                limit = 100000
            };
            var response = _apiAccess.ExecutePostEntry(route, createTradeRequestDto);
            _apiAccess.CheckStatusCode(route, response);

            var json = response
               .Content
               .ReadAsStringAsync()
               .Result;

            return JsonConvert.DeserializeObject<CreateClosedTradeResponse>(json);
        }

        public GeneralDto PostPendingBuyOrderRequest(string url, int amount,
            GetLoginResponse loginData,
            double rate = default, string assetName = null)
        {
            var route = ApiRouteAggregate.PostBuyAsset();
            var actualAssetName = assetName ?? DataRep.AssetName;
            route = url + route;

            var createTradeRequestDto = new PostTradeRequest
            {
                asset_symbol = actualAssetName,
                transaction_type = "buy",
                amount = amount,
                status = "pending",
                rate = rate,
            };
            var response = _apiAccess.ExecutePostEntry(route, createTradeRequestDto, loginData);
            _apiAccess.CheckStatusCode(route, response);

            var json = response
               .Content
               .ReadAsStringAsync()
               .Result;

            return JsonConvert.DeserializeObject<GeneralDto>(json);
        }

        public string PostPendingSellOrderRequest(string url, int amount,
            GetLoginResponse loginData, double rate = default)
        {
            var route = ApiRouteAggregate.PostSellAsset();
            route = url + route;
            var createTradeRequestDto = new PostTradeRequest
            {
                asset_symbol = DataRep.AssetName,
                transaction_type = "sell",
                amount = amount,
                status = "pending",
                rate = rate,
            };
            var response = _apiAccess.ExecutePostEntry(route, createTradeRequestDto, loginData);
            _apiAccess.CheckStatusCode(route, response);

            return response
                .Content
                .ReadAsStringAsync()
                .Result;
        }

        public HttpResponseMessage DeleteTradeRequest(string url, string tradeId)
        {
            var route = ApiRouteAggregate.DeleteTrade(tradeId);
            route = $"{url}{route}{tradeId}?api_key={_apiKey}";
            var response = _apiAccess.ExecuteDeleteEntry(route);
            _apiAccess.CheckStatusCode(route, response);

            return response;
        }

        public string CreateTradePipeApi(string url,
            GetLoginResponse loginData, Steps steps)
        {
            var route = ApiRouteAggregate.PostBuyAsset();
            route = url + route;

            var tradeAmount = steps.trade_amount;
            var assetSymbol = steps.asset_symbol;
            var transactionType = steps.transaction_type;
            var status = steps.trade_status;
            var currentRate = steps.current_rate;

            var createTradeRequestDto = new PostTradeRequest
            {
                asset_symbol = assetSymbol ?? DataRep.AssetName,
                transaction_type = transactionType ?? "buy",
                amount = Convert.ToInt32(tradeAmount),
                status = status ?? "open",
                rate = currentRate,
            };
            var response = _apiAccess.ExecutePostEntry(route, createTradeRequestDto, loginData);
            _apiAccess.CheckStatusCode(route, response);

            return response
                .Content
                .ReadAsStringAsync()
                .Result;
        }

        public GeneralDto CreateTakeProfitApi(string url, int actualAmount,
            GetLoginResponse loginData, double rate)
        {
            var route = ApiRouteAggregate.PostBuyAsset();
            route = url + route;

            var createTradeRequestDto = new PostTradeRequest
            {
                asset_symbol = DataRep.AssetName,
                transaction_type = "buy",
                amount = actualAmount,
                status = "open",
                close_at_profit = rate,
            };

            var response = _apiAccess.ExecutePostEntry(route, createTradeRequestDto, loginData);
            _apiAccess.CheckStatusCode(route, response);

            var json = response
               .Content
               .ReadAsStringAsync()
               .Result;

            return JsonConvert.DeserializeObject<GeneralDto>(json);
        }

        public GeneralDto CreateStopLossApi(string url, int actualAmount,
            GetLoginResponse loginData, double rate)
        {
            var route = ApiRouteAggregate.PostBuyAsset();
            route = url + route;

            var createTradeRequestDto = new PostTradeRequest
            {
                asset_symbol = DataRep.AssetName,
                transaction_type = "buy",
                amount = actualAmount,
                status = "open",
                close_at_loss = rate,
            };

            var response = _apiAccess.ExecutePostEntry(route, createTradeRequestDto, loginData);
            _apiAccess.CheckStatusCode(route, response);

            var json = response
               .Content
               .ReadAsStringAsync()
               .Result;

            return JsonConvert.DeserializeObject<GeneralDto>(json);
        }

        public T ChangeContext<T>(IWebDriver driver) where T : class
        {
            return _apiFactory.ChangeContext<T>(_driver);
        }
    }
}
