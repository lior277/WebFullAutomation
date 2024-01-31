using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading;
using AirSoftAutomationFramework.Internals.ExtensionsMethods;
using AirSoftAutomationFramework.Internals.Factorys;
using AirSoftAutomationFramework.Internals.Helpers;
using OpenQA.Selenium;

namespace AirSoftAutomationFramework.Objects.ForexObjects.TradingPlatform.TradeObjects.Ui
{
    public class NftPageUi : INftPageUi
    {
        public IWebDriver _driver;
        private readonly IApplicationFactory _apiFactory;
        private string _buyNftButton = "//app-nft-header-box[contains(.,'{0}')]/following::div[@class='nft-buttons-buy'][1]";
        public NftPageUi(IApplicationFactory apiFactory, IWebDriver driver)
        {
            _driver = driver;
            _apiFactory = apiFactory;
        }

        #region Locator's      
        private readonly By CloseDealBtnExp = By.CssSelector("button[id='nft-trade-button']");
        private readonly By ConfirmTradeButtomExp = By.CssSelector("button[class='btn peach-gradient start-transaction-confirm-button']");
        private readonly By ChronoTimerExp = By.CssSelector("div[class='chrono-timer']");
        private readonly By ChronoPlatformMenuItemMenuItemExp = DataRep.ChronoPlatformMenuItemExp;
        private readonly By BuyButtonExp = By.CssSelector("div[class*='card_buttons'] span[class*='button_buy']");
        private readonly By BlockTradeMessageExp = By.CssSelector("span[class*='trading-closed asset-info-animated-in ng-star-inserted']");
        private readonly By DisableBoostsExp = By.CssSelector("span[class*='chrono-disabled']");
        private readonly By CloseButtonpopupExp = By.CssSelector("popover-container[role='tooltip']");
        private readonly By StopTradeButtonExp = By.CssSelector("button[class='btn orange-gradient-button stop-button']");
        private readonly By CardButtonsForAnimationExp = DataRep.CardButtonsForAnimationExp;
        #endregion Locator's

        public NftPageUi BuyNftPipe(string assetName)
        {
            var buyNftMessage = string.Format(DataRep.BuyNftOrderExecutedMessage,
                DataRep.AssetNftSymbol);

            ClickOnBuyNft(assetName);
            ClickOnCloseDealNft();
            VerifyMessages(buyNftMessage);

            return this;
        }

        public NftPageUi ClickOnBuyNft(string nftName)
        {
            var buyButtonExp = By.XPath(string.Format(_buyNftButton, nftName));

            _driver.SearchElement(buyButtonExp)
                .ForceClick(_driver, buyButtonExp);

            return this;
        }

        public INftPageUi ClickOnCloseDealNft()
        {
            _driver.SearchElement(CloseDealBtnExp)
                .ForceClick(_driver, CloseDealBtnExp);

            return this;
        }

        public INftPageUi VerifyMessages(string message)
        {
            var alertExp = By.XPath(string.Format(DataRep.AlertOnFront, message));

            _driver.SearchElement(alertExp)
                .GetElementText(_driver, alertExp)
                .StringContains(message);

            return this;
        }

        public T ChangeContext<T>(IWebDriver driver) where T : class
        {
            return _apiFactory.ChangeContext<T>(_driver);
        }
    }
}
