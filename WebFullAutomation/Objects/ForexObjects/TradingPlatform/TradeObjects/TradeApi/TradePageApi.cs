// Ignore Spelling: Forex Api Cfd Kyc Crypto Usdt tp

using AirSoftAutomationFramework.Internals.DAL.ConfigurationManagement;
using AirSoftAutomationFramework.Internals.ExtensionsMethods;
using AirSoftAutomationFramework.Internals.Helpers;
using AirSoftAutomationFramework.Objects.DTOs;
using AirSoftAutomationFramework.Objects.MgmObjects.Api.Risk.AssetsCfd;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using AirSoftAutomationFramework.Internals.DAL.ApiAccsess;
using AirSoftAutomationFramework.Internals.DAL.ApiAccsess.ApiRoutes;
using AirSoftAutomationFramework.Internals.Factorys;
using AirSoftAutomationFramework.Models;
using static AirSoftAutomationFramework.Objects.DTOs.TestCase;

namespace AirSoftAutomationFramework.Objects.ForexObjects.TradingPlatform.TradeObjects.TradeApi
{
    public class TradePageApi : ITradePageApi
    {
        #region Members       
        private readonly IApplicationFactory _apiFactory;
        private IApiAccess _apiAccess;
        private IWebDriver _driver;
        private string _apiKey = Config.appSettings.ApiKey;
        #endregion Members

        public TradePageApi(IApplicationFactory apiFactory, IWebDriver driver,
            IApiAccess apiAccess)
        {
            _apiFactory = apiFactory;
            _driver = driver;
            _apiAccess = apiAccess;
        }

        public string PatchCloseTradeRequest(string url, string tradeId, int tradeAmount,
            GetLoginResponse loginData, bool checkStatusCode = true)
        {
            var route = ApiRouteAggregate.PatchCloseTrade(tradeId);
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

        public ITradePageApi PatchCloseTradeRequest(string url, List<string> tradeIds,
            int tradeAmount, GetLoginResponse loginData)
        {
            foreach (var id in tradeIds)
            {
                PatchCloseTradeRequest(url, id, tradeAmount, loginData);
            }

            return this;
        }

        public ITradePageApi PatchCloseTradeWithAmountRequest(string url, string tradeId,
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

        public ITradePageApi PatchMassCloseTradesRequest(string url, string[] tradeIds,
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

        public ITradePageApi PatchSignKycFileRequest(string url, string fileType,
            GetLoginResponse loginData)
        {
            var route = ApiRouteAggregate.PatchKycFile();
            route = $"{url}{route}/{fileType}";

            var path = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location),
                DataRep.PdfFileNameToUpload);

            var documentFileContent = _apiFactory
                .ChangeContext<IFileHandler>(_driver)
                .ConvertToBytesArray(path);

            documentFileContent.Headers.ContentDisposition =
                new ContentDispositionHeaderValue("form-data")
                {
                    Name = "document",
                    FileName = $"{fileType}.pdf"
                };

            documentFileContent.Headers.ContentType = new MediaTypeHeaderValue("application/pdf");
            var form = new MultipartFormDataContent();
            form.Add(documentFileContent);

            var response = _apiAccess.ExecutePatchEntry(route, form, loginData);
            _apiAccess.CheckStatusCode(route, response);

            return this;
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

        public ITradePageApi GetStartMarginCallRequest(string url)
        {
            var route = ApiRouteAggregate.GetStartMarginCall();
            route = $"{url}{route}?api_key={_apiKey}";
            var response = _apiAccess.ExecuteGetEntry(route);
            _apiAccess.CheckStatusCode(route, response);

            return this;
        }

        public ITradePageApi PutLanguage(string url,
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


        public ITradePageApi PutSetTemplateRequest(string url, string clientId,
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

        public ITradePageApi WaitForPendingTradeToChangeStatusToOpen(string url, GetLoginResponse loginData)
        {
            var numOfPendingTrades = GetTradesByStatusRequest(url, loginData, "pending")
                .GeneralResponse
                .Count();

            for (var i = 0; i < 10; i++)
            {
                if (numOfPendingTrades != 0)
                {
                    numOfPendingTrades = GetTradesByStatusRequest(url, loginData, "pending")
                        .GeneralResponse
                        .Count();

                    Thread.Sleep(500);
                }
                else
                {
                    break;
                }
            }

            return this;
        }

        public ITradePageApi WaitForPendingTrade(string url, GetLoginResponse loginData)
        {
            var numOfPendingTrades = GetTradesByStatusRequest(url, loginData, "pending")
                .GeneralResponse
                .Count();

            for (var i = 0; i < 50; i++)
            {
                if (numOfPendingTrades == 0)
                {
                    numOfPendingTrades = GetTradesByStatusRequest(url, loginData, "pending")
                        .GeneralResponse
                        .Count();

                    Thread.Sleep(100);
                }
                else
                {
                    break;
                }
            }

            return this;
        }

        public ITradePageApi WaitForCfdTradeToClose(string url, string tradeId,
            GetLoginResponse loginData)
        {
            var closeTradeId = GetTradesByStatusRequest(url, loginData, "close")
                .GeneralResponse
                .Where(p => p.id == tradeId)?
                .FirstOrDefault()?
                .id;

            for (var i = 0; i < 20; i++)
            {
                if (closeTradeId != tradeId)
                {
                    closeTradeId = GetTradesByStatusRequest(url, loginData, "close")
                        .GeneralResponse
                        .Where(p => p.id == tradeId)?
                        .FirstOrDefault()?
                        .id;

                    Thread.Sleep(100);
                }
                else
                {
                    break;
                }
            }

            if (closeTradeId != tradeId)
            {
                var exceMessage = $" the trade id: {tradeId} is steel open";

                throw new Exception(exceMessage);
            }

            return this;
        }

        public ITradePageApi WaitForCfdTradeToClose(string url,
            List<string> tradesIds, GetLoginResponse loginData)
        {
            foreach (var trade in tradesIds)
            {
                WaitForCfdTradeToClose(url, trade, loginData);
            }

            return this;
        }

        public ITradePageApi WaitForTradeToOpen(string url, string tradeId,
            GetLoginResponse loginData)
        {
            var closeTradeId = GetTradesByStatusRequest(url, loginData, "open")
                .GeneralResponse
                .Where(p => p.id == tradeId)?
                .FirstOrDefault()?
                .id;

            for (var i = 0; i < 10; i++)
            {
                if (closeTradeId != tradeId)
                {
                    closeTradeId = GetTradesByStatusRequest(url, loginData, "open")
                        .GeneralResponse
                        .Where(p => p.id == tradeId)?
                        .FirstOrDefault()?
                        .id;

                    Thread.Sleep(100);
                }
                else
                {
                    break;
                }
            }

            if (closeTradeId != tradeId)
            {
                var exceMessage = $" the trade id: {tradeId} is steel open";

                throw new Exception(exceMessage);
            }

            return this;
        }

        public GeneralResult<List<GetTradesResponse>> GetTradesByStatusRequest(string url,
            GetLoginResponse loginData, string tradeStatus = "open", bool checkStatusCode = true)
        {
            var generalResult = new GeneralResult<List<GetTradesResponse>>();
            var route = ApiRouteAggregate.GetTradesByStatus(tradeStatus);
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


        public List<GetActivitiesResponse> GetActivities(string url,
            GetLoginResponse loginData)
        {
            var today = DateTime.Now.ToString("yyyy-MM-dd");
            var route = ApiRouteAggregate.GetActivities(today);
            route = url + route;
            var response = _apiAccess.ExecuteGetEntry(route, loginData);
            _apiAccess.CheckStatusCode(route, response);

            var json = response
               .Content
               .ReadAsStringAsync()
               .Result;

            var ff = JsonConvert.DeserializeObject<List<GetActivitiesResponse>>(json);
            return JsonConvert.DeserializeObject<List<GetActivitiesResponse>>(json);
        }

        public ITradePageApi WaitForActivityToRegister(string url,
            string activityType, GetLoginResponse loginData)
        {
            var today = DateTime.Now.ToString("yyyy-MM-dd");
            var route = ApiRouteAggregate.GetActivities(today);
            route = url + route;
            var response = _apiAccess.ExecuteGetEntry(route, loginData);
            _apiAccess.CheckStatusCode(route, response);

            var json = response
               .Content
               .ReadAsStringAsync()
               .Result;

            var ff = JsonConvert.DeserializeObject<List<GetActivitiesResponse>>(json);

            var activitie = ff
                .Where(p => p.type.Equals(activityType))?
                .FirstOrDefault();

            for (var i = 0; i < 10; i++)
            {
                if (activitie == null)
                {
                    response = _apiAccess.ExecuteGetEntry(route, loginData);
                    _apiAccess.CheckStatusCode(route, response);

                    json = response
                       .Content
                       .ReadAsStringAsync()
                       .Result;

                    activitie = JsonConvert.DeserializeObject<List<GetActivitiesResponse>>(json)
                        .Where(p => p.type.Equals(activityType))
                        .FirstOrDefault();

                    Thread.Sleep(300);

                    continue;
                }

                break;
            }

            if (activitie == null)
            {
                var exceMessage = ($" activity: {activityType} not exist");

                throw new NullReferenceException(exceMessage);
            }

            return this;
        }

        public GetUsersSavingAccounts GetUsersSavingAccountsRequest(string url,
            GetLoginResponse loginData)
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

        public ITradePageApi UpdateClientPnl(string clientId, string tradeId,
            int pnlAmount = 50)
        {
            //var status = GetTradeStatus(tradeId);
            var dbContext = new QaAutomation01Context();

            (from s in dbContext.UserAccounts
             where s.UserId == clientId
             select s)
            .First()
            .ClosedProfitLoss = pnlAmount;

            dbContext.VerifySaveForSqlManipulation();

            return this;
        }

        public ITradePageApi UpdateClientPnl(List<string> clientsIds, List<string> tradesIds,
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

        public ITradePageApi UpdateTradePnl(string tradeId, double pnlAmount = 50)
        {
            //var status = GetTradeStatus(tradeId);
            var _dbContext = new QaAutomation01Context();
            double actualPnl = 0;

            actualPnl = (double)(from s in _dbContext.Trades
                                 where s.Id.ToString() == tradeId
                                 select s)
                                 .FirstOrDefault()
                                 .ClosedProfitLoss;

            (from s in _dbContext.Trades
             where s.Id.ToString() == tradeId
             select s)
                .FirstOrDefault()
                .ClosedProfitLoss = pnlAmount;

            Thread.Sleep(1000);
            _dbContext.SaveChanges();
            Thread.Sleep(1000);

            for (var i = 0; i < 30; i++)
            {
                actualPnl = (double)(from s in _dbContext.Trades
                                     where s.Id.ToString() == tradeId
                                     select s)
                                     .FirstOrDefault()
                                     .ClosedProfitLoss;

                if (actualPnl != pnlAmount)
                {
                    Thread.Sleep(1000);

                    continue;
                }

                break;
            }

            if (actualPnl != pnlAmount)
            {
                var exceMessage = $" the trade id: {tradeId} is steel open";

                throw new Exception(exceMessage);
            }

            return this;
        }

        public ITradePageApi UpdateTradePnl(List<string> tradeIds, int pnlAmount = 50)
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

        public ITradePageApi UpdatePnlOnTradesView(string clientId, int pnlAmountForEachTrade)
        {
            var _dbContext = new QaAutomation01Context();

            (from s in _dbContext.TradesViews
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

        public ITradePageApi RemoveAssetPipe(string url, string assetName,
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
            // get Apple Asset.appleAsset.Times.CustomOpen.FirstOrDefault().Date = new DateTime(2021,12,08);
            var status = false;
            // check if Utc Now is in Custom Open 
            var utcTime = DateTime.UtcNow.ToString("HH:mm:ss");
            var utcTimeNow = TimeSpan.Parse(utcTime);
            var utcDateAndTime = DateTime.UtcNow;

            var customOpen = getAppleAsset.appleAsset.Times.CustomOpen
                .Where(p => p.Date == utcDateAndTime.Date)
                .FirstOrDefault();

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
                status = utcTimeNow.CheckIfInTimeRange(getAppleAsset
                    .appleAsset.Times.Open.First().From,
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

        public GeneralResult<GeneralDto> PostBuyAssetRequest(string url,
            int amount, GetLoginResponse loginData, string status = "open",
            string assetSymbol = null, bool checkStatusCode = true)
        {
            var generalResult = new GeneralResult<GeneralDto>();
            var route = ApiRouteAggregate.PostBuyAsset();
            route = url + route;
            var actualassetSymble = assetSymbol ?? DataRep.AssetName;

            var createTradeRequestDto = new PostTradeRequest
            {
                asset_symbol = actualassetSymble,
                transaction_type = "buy",
                amount = amount,
                status = status,
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

        public List<GeneralDto> PostBuyAssetRequest(string url, int amount,
            int numOfTransactions, GetLoginResponse loginData,
            string status = "open", string assetSymbol = null)
        {
            var actualAssetSymble = assetSymbol ?? DataRep.AssetName;
            var responses = new List<GeneralDto>();

            for (var i = 0; i < numOfTransactions; i++)
            {
                responses.Add(PostBuyAssetRequest(url, amount, loginData,
                    status, actualAssetSymble).GeneralResponse);
            }

            return responses;
        }

        // buy asset when market is closed
        public string PostBuyCloseAssetRequest(string url, int amount,
            GetLoginResponse loginData,
            string status = "open", string assetSymbol = null)
        {
            var route = ApiRouteAggregate.PostBuyAsset();
            route = url + route;
            var createTradeRequestDto = new PostCloseTradeRequest
            {
                asset_symbol = assetSymbol ?? "APPLE",
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
            string assetSymbol = null, bool checkStatusCode = true)
        {
            var generalResult = new GeneralResult<GeneralDto>();
            var route = ApiRouteAggregate.PostSellAsset();
            route = url + route;

            var createTradeRequestDto = new PostTradeRequest
            {
                asset_symbol = assetSymbol ?? DataRep.AssetName,
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
            string status = "open", string assetSymbol = null)
        {
            var route = ApiRouteAggregate.PostSellAsset();
            route = url + route;
            var createTradeRequestDto = new PostCloseTradeRequest
            {
                asset_symbol = assetSymbol ?? "APPLE",
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
            route = $"{url}{route}?api_key={_apiKey}";
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
