// Ignore Spelling: Chrono Forex api Popup Rgb Buttom

using System;
using System.Collections.Generic;
using System.Threading;
using AirSoftAutomationFramework.Internals.ExtensionsMethods;
using AirSoftAutomationFramework.Internals.Factory;
using AirSoftAutomationFramework.Internals.Helpers;
using OpenQA.Selenium;

namespace AirSoftAutomationFramework.Objects.ForexObjects.TradingPlatform.TradeObjects.Ui
{
    public class ChronoPageUi : IChronoPageUi
    {
        public IWebDriver _driver;
        private readonly IApplicationFactory _apiFactory;
        private string _chooseYourPAndLMultiplier = "//div[@id = '{0}']";
        private string _chooseYourTimeLimit = "//div[@id = '{0}']";
        private string _enableBoost = "//div[contains(@class,'chrono-radio-buttons-text') and contains(.,'{0}')]";
        private string _disableBoosts = "//span[contains(@class,'chrono-disabled') and contains(.,'{0}')]";
        public ChronoPageUi(IApplicationFactory apiFactory, IWebDriver driver)
        {
            _driver = driver;
            _apiFactory = apiFactory;
        }

        #region Locator's      
        private readonly By TradeConfirmationBodyPopupExp = By.CssSelector("div[class='modal-body start-transaction-modal-body']");
        private readonly By ConfirmTradeButtomExp = By.CssSelector("button[class='btn peach-gradient start-transaction-confirm-button']");
        private readonly By ChronoTimerExp = By.CssSelector("div[class='chrono-timer']");
        private readonly By ChronoPlatformMenuItemMenuItemExp = DataRep.ChronoPlatformMenuItemExp;
        private readonly By BuyButtonExp = By.CssSelector("div[class*='card_buttons'] span[class*='button_buy']");
        private readonly By BlockTradeMessageExp = By.CssSelector("span[class*='trading-closed asset-info-animated-in ng-star-inserted']");
        private readonly By DisableBoostsExp = By.CssSelector("span[class*='chrono-disabled']");
        private readonly By CloseButtonpopupExp = By.CssSelector("popover-container[role='tooltip']");
        private readonly By StopTradeButtonExp = By.CssSelector("button[class='btn orange-gradient-button stop-button']");
        private readonly By StopTradePnlExp = By.CssSelector("span[class='pnl-early-color']");// after click on stop button the pnl color change
        private readonly By CardButtonsForAnimationExp = DataRep.CardButtonsForAnimationExp;
        #endregion Locator's

        public IChronoPageUi SetMultiplier(string multiplierId = null)
        {
            var multiplierIdLocal = multiplierId ?? "2_boost";

            var chooseYourPAndLMultiplierExp =
                By.XPath(string.Format(_chooseYourPAndLMultiplier, multiplierIdLocal));

            _driver.WaitForAnimationToFinish(DataRep.BuyAndSellButtonsForAnimationExp);

            _driver.SearchElement(chooseYourPAndLMultiplierExp)
                .ForceClick(_driver, chooseYourPAndLMultiplierExp);

            return this;
        }

        public IChronoPageUi SetTimeLimit(string timeLimitId = null)
        {
            var timeLimitIdLocal = timeLimitId ?? "30s_period";

            var chooseYourTimeLimitExp = By.XPath(string.Format(_chooseYourTimeLimit, timeLimitIdLocal));
            //_driver.WaitForAnimationAndClick(DataRep.BuyAndSellButtonsForAnimationExp, 20);

            _driver.SearchElement(chooseYourTimeLimitExp)
                .ForceClick(_driver, chooseYourTimeLimitExp);

            return this;
        }

        public IChronoPageUi ClickBuyButton()
        {
            //_driver.WaitForAnimationToLoad(1050);

            _driver.SearchElement(BuyButtonExp)
                .ForceClick(_driver, BuyButtonExp);

            //_driver.RetryClickTillElementNotDisplay(BuyButtonExp);

            return this;
        }

        public IChronoPageUi ClickOnBoostByName(string boostName)
        {
            var boostExt = By.XPath(string.Format(_enableBoost, boostName));
           // _driver.WaitForAnimationToLoad(1000);

            _driver.SearchElement(boostExt)
                .ForceClick(_driver, boostExt);

            return this;
        }

        public string GetTradeConfirmationBodyPopup()
        {
            var tradeConfirmationBody = _driver.SearchElement(TradeConfirmationBodyPopupExp)
                .GetElementText(_driver, TradeConfirmationBodyPopupExp);

            return tradeConfirmationBody;
        }

        public string GetBlockTradeMessage()
        {
            return _driver.SearchElement(BlockTradeMessageExp, 130)
                .GetElementText(_driver, by: BlockTradeMessageExp);
        }

        public IList<IWebElement> GetDisabledBoosts()
        {
            return _driver.SearchElements(DisableBoostsExp);
        }

        public string GetEarlyStopPnlRgbColor()
        {
            return _driver.SearchElement(StopTradePnlExp).GetCssValue("color");
        }

        public IChronoPageUi ClickOnConfirmTradeButton()
        {
            _driver.SearchElement(ConfirmTradeButtomExp, 80)
                .ForceClick(_driver, ConfirmTradeButtomExp);

            return this;
        }

        public IChronoPageUi VerifyBoostEnableByName(string boostName)
        {
            var boostExt = By.XPath(string.Format(_enableBoost, boostName));
            _driver.SearchElement(boostExt);

            return this;
        }

        public IChronoPageUi VerifyBlockChronoTradeMessage(string message)
        {
            var alertExp = By.XPath(string.Format(DataRep.AlertOnFront, message));

            _driver.SearchElement(alertExp)
                .GetElementText(_driver, alertExp)
                .StringContains(message);

            return this;
        }

        public IChronoPageUi VerifyBoostDisableByName(string boostName)
        {
            var boostExt = By.XPath(string.Format(_disableBoosts, boostName));
            _driver.SearchElement(boostExt);

            return this;
        }

        public bool CheckIfChronoMenuItemExist()
        {
            return _driver.FindElements(ChronoPlatformMenuItemMenuItemExp)
                .Count > 0;
        }

        public IChronoPageUi ClickOnStopChronoTradeButton()
        {
            _driver.SearchElement(StopTradeButtonExp)
                .ForceClick(_driver, StopTradeButtonExp);

            //_driver.SearchElement(CloseButtonpopupExp);

            //_driver.SearchElement(StopTradeButtonExp)
            //    .ForceClick(_driver, StopTradeButtonExp);

            //_driver.RetryClickTillElementNotDisplay(StopTradeButtonExp);

            //_driver.WaitForElementNotExist(StopTradeButtonExp, 200);

            return this;
        }

        public IChronoPageUi VerifyChronoOrderActivatedMessage(string message)
        {
            var alertExp = By.XPath(string.Format(DataRep.AlertOnFront, message));

            _driver.SearchElement(alertExp, 150)
                .GetElementText(_driver, alertExp, 100)
                .StringContains(message);

            return this;
        }

        public IChronoPageUi WaitForChronoTimerToFinish(string expectedWait)
        {
            var expectedWaitInMilliseconds = Convert.ToInt32(TimeSpan.Parse(expectedWait).TotalMilliseconds);
            Thread.Sleep(expectedWaitInMilliseconds);

            return this;
        }

        public IChronoPageUi BuyChronoPipe(string assetName, string multiplier = null,
            string timeLimit = null)
        {
            _apiFactory
                .ChangeContext<ITradePageUi>(_driver)
                .SearchAssetPipe(assetName);

            _driver.WaitForAnimationToFinish(CardButtonsForAnimationExp);

            SetMultiplier(multiplier);
            SetTimeLimit(timeLimit);
            ClickBuyButton();

            return this;
        }

        public T ChangeContext<T>(IWebDriver driver) where T : class
        {
            return _apiFactory.ChangeContext<T>(_driver);
        }
    }
}
