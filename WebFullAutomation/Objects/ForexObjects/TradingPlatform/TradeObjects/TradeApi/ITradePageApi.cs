using AirSoftAutomationFramework.Objects.DTOs;
using OpenQA.Selenium;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace AirSoftAutomationFramework.Objects.ForexObjects.TradingPlatform.TradeObjects.TradeApi
{
    public interface ITradePageApi
    {
        int CalculateTradeAmountForMarginCall(double currentPrice, int tradeAmount, double marginPercent, double userBalance, double marginCallPercentage);
        T ChangeContext<T>(IWebDriver driver) where T : class;
        ITradePageApi PatchSignKycFileRequest(string url, string fileType,
            GetLoginResponse loginData);
        GeneralDto CreateStopLossApi(string url, int actualAmount, GetLoginResponse loginData, double rate);
        GeneralDto CreateTakeProfitApi(string url, int actualAmount, GetLoginResponse loginData, double rate);
        string CreateTradePipeApi(string url, GetLoginResponse loginData, TestCase.Steps steps);
        HttpResponseMessage DeleteTradeRequest(string url, string tradeId);
        List<GetActivitiesResponse> GetActivities(string url, GetLoginResponse loginData);
        GetAppleAsset GetAppleAssetRequest(string url, GetLoginResponse loginData);
        string GetAssetStatusPipe(GetAppleAsset getAppleAsset);
        List<GetCfdAssetsNamesResponse> GetCfdAssetsNamesRequest(string url, GetLoginResponse loginData);
        GeneralResult<List<GetClientTradeCountResponse>> GetClientTradesCountRequest(string url, GetLoginResponse loginData, bool checkStatusCode = true);
        GetCryptoVsUsdtResponse GetCryptoVsUsdtRequest(string url, GetLoginResponse loginData);
        GeneralResult<GetExportClientActivitiesResponse> GetExportActivitiesRequest(string url, string clientId, GetLoginResponse loginData, bool fullHistory = false, bool checkStatusCode = true);
        string GetFeedAssetsCfdMajorRequest(string url, GetLoginResponse loginData);
        GeneralResult<TradingLanguage> GetLanguageByCode(string url, string languageCode, GetLoginResponse loginData, bool checkStatusCode = true);
        ITradePageApi GetStartMarginCallRequest(string url);
        int GetTotalPnlFromTradesView(string clientId);
        GeneralResult<List<GetTradesResponse>> GetTradesByStatusRequest(string url, GetLoginResponse loginData, string tradeStatus = "open", bool checkStatusCode = true);
        Task<string> GetTradeStatus(string tradeId);
        GetUsersSavingAccounts GetUsersSavingAccountsRequest(string url, GetLoginResponse loginData);
        ITradePageApi PatchCloseTradeRequest(string url, List<string> tradeIds, int tradeAmoun, GetLoginResponse loginData);
        string PatchCloseTradeRequest(string url, string tradeId, int tradeAmount, GetLoginResponse loginData, bool checkStatusCode = true);
        ITradePageApi PatchCloseTradeWithAmountRequest(string url, string tradeId, GetLoginResponse loginData, int amount);
        ITradePageApi PatchMassCloseTradesRequest(string url, string[] tradeIds, GetLoginResponse loginData);
        string PatchPendingTradeRequest(string url, string tradeId, int tradeAmount, GetLoginResponse loginData, bool checkStatusCode = true);
        string PostBuyAssetByAgentRequest(string url, string userId, int tradeAmount);
        GeneralResult<GeneralDto> PostBuyAssetRequest(string url, int amount, GetLoginResponse loginData, string status = "open", string assetSymble = null, bool checkStatusCode = true);
        List<GeneralDto> PostBuyAssetRequest(string url, int amount, int numOfTransactuions, GetLoginResponse loginData, string status = "open", string assetSymble = null);
        string PostBuyCloseAssetRequest(string url, int amount, GetLoginResponse loginData, string status = "open", string assetSymble = null);
        CreateClosedTradeResponse PostClosedTradesByAgentRequest(string url);
        GeneralDto PostPendingBuyOrderRequest(string url, int amount, GetLoginResponse loginData, double rate = 0, string assetName = null);
        string PostPendingSellOrderRequest(string url, int amount, GetLoginResponse loginData, double rate = 0);
        GeneralResult<GeneralDto> PostSellAssetRequest(string url, int amount, GetLoginResponse loginData, string status = "open", string assetSymble = null, bool checkStatusCode = true);
        string PostSellCloseAssetRequest(string url, int amount, GetLoginResponse loginData = null, string status = "open", string assetSymble = null);
        ITradePageApi PutLanguage(string url, GetLoginResponse loginData, string clientId, string actualLang = "en");
        ITradePageApi PutSetTemplateRequest(string url, string clientId, string templateName = "light-template");
        ITradePageApi RemoveAssetPipe(string url, string assetName, GetLoginResponse _mgmLoginData, GetLoginResponse _tpLoginData);
        ITradePageApi UpdateClientPnl(List<string> clientsIds, List<string> tradesIds, int pnlAmount = 50);
        ITradePageApi UpdateClientPnl(string clientId, string tradeId, int pnlAmount = 50);
        ITradePageApi UpdatePnlOnTradesView(string clientId, int pnlAmountForEachTrade);
        ITradePageApi UpdateTradePnl(List<string> tradeIds, int pnlAmount = 50);
        ITradePageApi UpdateTradePnl(string tradeId, double pnlAmount = 50);
        ITradePageApi WaitForActivityToRegister(string url, string activiteType, GetLoginResponse loginData);
        ITradePageApi WaitForCfdTradeToClose(string url, List<string> tradesIds, GetLoginResponse loginData);
        ITradePageApi WaitForCfdTradeToClose(string url, string tradeId, GetLoginResponse loginData);
        ITradePageApi WaitForPendingTrade(string url, GetLoginResponse loginData);
        ITradePageApi WaitForPendingTradeToChangeStatusToOpen(string url, GetLoginResponse loginData);
        ITradePageApi WaitForTradeToOpen(string url, string tradeId, GetLoginResponse loginData);
    }
}