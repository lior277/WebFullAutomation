using AirSoftAutomationFramework.Objects.DTOs;
using OpenQA.Selenium;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace AirSoftAutomationFramework.Objects.ForexObjects.TradingPlatform.TradeObjects.TradeApi
{
    public interface INftPageApi
    {
        int CalculateTradeAmountForMarginCall(double currentPrice, int tradeAmount, double marginPercent, double userBalance, double marginCallPercentage);
        T ChangeContext<T>(IWebDriver driver) where T : class;
        GeneralDto CreateStopLossApi(string url, int actualAmount, GetLoginResponse loginData, double rate);
        GeneralDto CreateTakeProfitApi(string url, int actualAmount, GetLoginResponse loginData, double rate);
        string CreateTradePipeApi(string url, GetLoginResponse loginData, TestCase.Steps steps);
        HttpResponseMessage DeleteTradeRequest(string url, string tradeId);
        List<GetActivitiesResponse> GetActivities(string url, GetLoginResponse loginData);
        GetAppleAsset GetAppleAssetRequest(string url, GetLoginResponse loginData);
        string GetAssetStatusPipe(GetAppleAsset getAppleAsset);
        List<GetCfdAssetsNamesResponse> GetCfdAssetsNamesRequest(string url, GetLoginResponse loginData);
        GeneralResult<List<GetClientTradeCountResponse>> GetClientTradesCountRequest(string url, GetLoginResponse loginData, bool checkStatusCode = true);
        GeneralResult<List<GetTradesResponse>> GetClientTreadsRequest(string url, GetLoginResponse loginData, bool checkStatusCode = true);
        GeneralResult<List<CloseTrade>> GetClosedTradesRequest(string url, GetLoginResponse loginData, bool checkStatusCode = true);
        GetCryptoVsUsdtResponse GetCryptoVsUsdtRequest(string url, GetLoginResponse loginData);
        GeneralResult<GetExportClientActivitiesResponse> GetExportActivitiesRequest(string url, string clientId, GetLoginResponse loginData, bool fullHistory = false, bool checkStatusCode = true);
        string GetFeedAssetsCfdMajorRequest(string url, GetLoginResponse loginData);
        GeneralResult<TradingLanguage> GetLanguageByCode(string url, string languageCode, GetLoginResponse loginData, bool checkStatusCode = true);
        INftPageApi GetStartMarginCallRequest(string url);
        int GetTotalPnlFromTradesView(string clientId);
        Task<string> GetTradeStatus(string tradeId);
        GetUsersSavingAccounts GetUsersSavingAccountsRequest(string url, GetLoginResponse loginData);
        string PatchCloseNftRequest(string url, string tradeId, GetLoginResponse loginData, int tradeAmount = 1, bool checkStatusCode = true);
        INftPageApi PatchCloseTradeWithAmountRequest(string url, string tradeId, GetLoginResponse loginData, int amount);
        INftPageApi PatchMassCloseTradesRequest(string url, string[] tradeIds, GetLoginResponse loginData);
        string PatchPendingTradeRequest(string url, string tradeId, int tradeAmount, GetLoginResponse loginData, bool checkStatusCode = true);
        string PostBuyAssetByAgentRequest(string url, string userId, int tradeAmount);
        string PostBuyCloseAssetRequest(string url, int amount, GetLoginResponse loginData, string status = "open", string assetSymble = null);
        GeneralResult<GeneralDto> PostBuyNftRequest(string url, GetLoginResponse loginData, int amount = 1, string status = "open", string assetSymble = null, bool checkStatusCode = true);
        CreateClosedTradeResponse PostClosedTradesByAgentRequest(string url);
        INftPageApi PostExportCloseTradeTableRequest(string url, string userEmail, string apiKey = null);
        INftPageApi PostExportOpenTradeTableRequest(string url, string userEmail, string apiKey = null);
        INftPageApi PostExportPendingTradeTableRequest(string url, string userEmail, string apiKey = null);
        GeneralDto PostPendingBuyOrderRequest(string url, int amount, GetLoginResponse loginData, double rate = 0, string assetName = null);
        string PostPendingSellOrderRequest(string url, int amount, GetLoginResponse loginData, double rate = 0);
        GeneralResult<GeneralDto> PostSellAssetRequest(string url, int amount, GetLoginResponse loginData, string status = "open", string assetSymble = null, bool checkStatusCode = true);
        string PostSellCloseAssetRequest(string url, int amount, GetLoginResponse loginData = null, string status = "open", string assetSymble = null);
        INftPageApi PutLanguage(string url, GetLoginResponse loginData, string clientId, string actualLang = "en");
        INftPageApi PutSetTemplateRequest(string url, string clientId, string templateName = "light-template");
        INftPageApi RemoveAssetPipe(string url, string assetName, GetLoginResponse _mgmLoginData, GetLoginResponse _tpLoginData);
        INftPageApi UpdateClientPnl(List<string> clientsIds, List<string> tradesIds, int pnlAmount = 50);
        INftPageApi UpdateClientPnl(string clientId, string tradeId, int pnlAmount = 50);
        INftPageApi UpdatelPnlOnTradesView(string clientId, int pnlAmountForEachTrade);
        INftPageApi UpdateTradePnl(List<string> tradeIds, int pnlAmount = 50);
        INftPageApi UpdateTradePnl(string tradeId, int pnlAmount = 50);
    }
}