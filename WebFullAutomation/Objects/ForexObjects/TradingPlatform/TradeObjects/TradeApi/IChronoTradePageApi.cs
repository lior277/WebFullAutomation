using AirSoftAutomationFramework.Objects.DTOs;
using OpenQA.Selenium;
using System.Collections.Generic;

namespace AirSoftAutomationFramework.Objects.ForexObjects.TradingPlatform.TradeObjects.TradeApi
{
    public interface IChronoTradePageApi
    {
        T ChangeContext<T>(IWebDriver driver) where T : class;
        GeneralResult<List<GetTradesResponse>> GetChronoTradesRequest(string url, GetLoginResponse loginData, bool checkStatusCode = true);
        string PatchCloseChronoTradeRequest(string url, string tradeId, GetLoginResponse loginData, bool checkStatusCode = true);
        GeneralResult<GeneralDto> PostBuyChronoAssetApi(string url, GetLoginResponse loginData, string actualStatus = "open", string tradeTimeEnd = "30s", string assetSymble = "ETHUSD", bool checkStatusCode = true);
        GeneralDto PostSellChronoAssetApi(string url, GetLoginResponse loginData, string actualStatus = "open", string tradeTimeEnd = "30s", string assetSymble = "ETHUSD");
        IChronoTradePageApi WaitForChronoTradeToClose(string url, List<string> chronoTradesIds, GetLoginResponse loginData);
        IChronoTradePageApi WaitForChronoTradeToClose(string url, string chronoTradeId, GetLoginResponse loginData);
    }
}