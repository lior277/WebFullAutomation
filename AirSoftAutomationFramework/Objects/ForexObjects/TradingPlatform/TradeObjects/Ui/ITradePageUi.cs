using AirSoftAutomationFramework.Objects.ForexObjects.TradingPlatform.TradeObjects.Ui.Settings;
using AirSoftAutomationFramework.Objects.ForexObjects.Ui.UiGeneral;
using OpenQA.Selenium;

namespace AirSoftAutomationFramework.Objects.ForexObjects.TradingPlatform.TradeObjects.Ui
{
    public interface ITradePageUi
    {
        ISearchResultsUi SearchTrade(string searchValue);
        string AvMounthRoi(string expectedText);
        ITradePageUi BuyAssetPipe(string assetName = "ETH", int amount = 2, bool verify = true);
        T ChangeContext<T>(IWebDriver driver) where T : class;
        ITradePageUi ClickCloseAtLossWhenRateIsButton();
        ITradePageUi ClickToOpenAsset(string assetName);
        ITradePageUi ClickCloseAtProfitWhenRateIsButton();
        ITradePageUi ClickCloseTradeButton();
        ITradePageUi ClickOnAssetFavoriteBtn(string assetName);
        ITradePageUi ClickOnBuyButton();
        ITradePageUi ClickOnChatBtn();
        IProfilePageUi ClickOnClientFirstName();
        ITradePageUi ClickOnCloseDealButton();
        ITradePageUi ClickOnCloseDealForCloedAssetButton();
        ITradePageUi ClickOnCloseDealWithErrorButton();
        ITradePageUi ClickOnDocumentPreview();
        ITradePageUi ClickOnDocumentStatus(string documentName);
        ITradePageUi ClickOnFavoritesBtnOnTopMenu();
        ITradePageUi ClickOnLimitBtn();
        ITradePageUi ClickOnMarketBtn();
        ITradePageUi ClickOnSaveSignatureForDocumentButton();
        IHistoryPageUi ClickOnSaveSignatureForDodButton();
        ITradePageUi ClickOnSellButton();
        ITradePageUi ClickOnSetTemplateBtn();
        ITradePageUi CloseTradePipe();
        ITradePageUi CreateConditionalTradePipe(string rate, string assetName = "ETH", int amount = 2, bool verify = true);
        ITradePageUi CreateTakeProfitTradePipe(string rate, string assetName = "ETH", int amount = 2, bool verify = true);
        string GetAccountRoi(string expectedText);
        string GetAmountErrorMessage();
        string GetAssetStatus();
        string GetClientFirstName();
        string GetCurrentRate();
        string GetDocumentTitles(string documentName);
        string GetLongTradesRgbColorValue();
        string GetLongTradesValue();
        string GetShortTradesRgbColorValue();
        string GetShortTradesValue();
        int GetTotalInvestmentValue();
        string GetTotalPnlCurrencySign();
        int GetTotalPnlValue();
        string GetTotalTradesValue();
        string GetWinPercentageValue();
        ITradePageUi SearchAssetPipe(string assetName);
        void SearchGraph();
        ITradePageUi SellAssetPipe(string assetName = "ETH", int amount = 2);
        ITradePageUi SetAmount(int amount = 2);
        ITradePageUi SetEntryPriceFiled(string Rate);
        ITradePageUi SetSignature();
        ITradePageUi SetStopLossRate(string Rate);
        ITradePageUi SetTakeProfitRate(string Rate);
        ITradePageUi VerifyAssetOnFavoritesMenu(string assetName);
        ITradePageUi VerifyBanner(string bannerText);
        ITradePageUi VerifyChatBtnNotExist();
        ITradePageUi VerifyClientFirstName(string clientFirstName);
        int VerifyFavoritAssetOnAssetsList(string assetName);
        ITradePageUi VerifyLiveTv();
        ITradePageUi VerifyMessages(string message);
        ITradePageUi VerifyNewDocumentStatus(string documentName, string newStatus);
        ITradePageUi VerifyNumOfCloseTrades(string numOfTrades = "1");
        ITradePageUi VerifyNumOfOpenTrades(string numOfTrades = "1");
        ITradePageUi VerifyNumOfPendingTrades(string numOfTrades = "1");
        ITradePageUi WaitForMyProfileToLoad();
        ITradePageUi WaitForTradeTableToLoad();
    }
}