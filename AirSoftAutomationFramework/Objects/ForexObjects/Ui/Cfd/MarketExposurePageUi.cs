using System;
using System.Collections.Generic;
using System.Linq;
using AirSoftAutomationFramework.Internals.ExtensionsMethods;
using AirSoftAutomationFramework.Internals.Factory;
using AirSoftAutomationFramework.Internals.Helpers;
using OpenQA.Selenium;

namespace AirSoftAutomationFramework.Objects.ForexObjects.Ui.Cfd
{
    public class MarketExposurePageUi : IMarketExposurePageUi
    {
        #region Members
        public IWebDriver _driver;
        private readonly IApplicationFactory _apiFactory;
        #endregion Members

        public MarketExposurePageUi(IApplicationFactory apiFactory, IWebDriver driver)
        {
            _apiFactory = apiFactory;
            _driver = driver;
        }

        #region Locator's
        private readonly By FrontLongPositionBoxExp = By.CssSelector(
            "div[class='front total-long'] p");

        private readonly By FrontLongPositionTotalTradesBoxExp = By.CssSelector(
         "div[class='front total-long'] div[class*='statistics-notes'] span");

        private readonly By FrontShortPositionBoxExp = By.CssSelector(
          "div[class='front total-short'] p");

        private readonly By FrontShortPositionTotalTradesBoxExp = By.CssSelector(
          "div[class='front total-short'] div[class*='statistics-notes'] span");

        private readonly By FrontTotalExposureBoxExp = By.CssSelector(
         "div[class='front exposure-total'] p");

        private readonly By FrontTotalExposureTotalTradesBoxExp = By.CssSelector(
          "div[class='front exposure-total'] div[class*='statistics-notes'] span");

        private readonly By FrontPendingOrdersBuyBoxExp = By.XPath(
            "//div[@class='front pending-total']//span[contains(.,'Buy')]/following-sibling::p");

        private readonly By FrontPendingOrdersSellBoxExp = By.XPath(
           "//div[@class='front pending-total']//span[contains(.,'Sell')]/following-sibling::p");

        private readonly By FrontPendingOrdersExposureBoxExp = By.XPath(
           "//div[@class='front pending-total']//span[contains(.,'Exposure')]/following-sibling::p");

        private readonly By FrontPendingOrdersTotalTradesBoxExp = By.CssSelector(
                "div[class='front pending-total'] div[class*='statistics-notes'] span");
        #endregion

        public Dictionary<string, int> GetFrontCardLongPositionData()
        {
            var frontCardLongPositionData = new Dictionary<string, int>();
            //_driver.WaitForAnimationToLoad(10000);
            
             _driver.SearchElement(DataRep.ProgressBarExp);

            var tempAmount = _driver.SearchElement(FrontLongPositionBoxExp)
                .GetElementText(_driver, FrontLongPositionBoxExp);

            var trades = _driver.SearchElement(FrontLongPositionTotalTradesBoxExp)
                .GetElementText(_driver, FrontLongPositionTotalTradesBoxExp);

            var amount = (int)tempAmount.MathRoundFromGeneric(0, MidpointRounding.ToEven);
            var numOfTrades = Convert.ToInt32(new string(trades.Where(char.IsDigit).ToArray()));
            // amount in dollar
            frontCardLongPositionData.Add("amount", amount);
            frontCardLongPositionData.Add("trades", numOfTrades);

            return frontCardLongPositionData;
        }

        public Dictionary<string, int> GetFrontCardShortPositionData()
        {
            var frontCardShortPositionData = new Dictionary<string, int>();
            //_driver.WaitForAnimationToLoad(10000);

            _driver.SearchElement(DataRep.ProgressBarExp);

            var tempAmount = _driver.SearchElement(FrontShortPositionBoxExp)
                .GetElementText(_driver, FrontShortPositionBoxExp);

            var trades = _driver.SearchElement(FrontShortPositionTotalTradesBoxExp)
                .GetElementText(_driver, FrontShortPositionTotalTradesBoxExp);

            var amount = (int)tempAmount.MathRoundFromGeneric(0, MidpointRounding.ToEven);
            var numOfTrades = Convert.ToInt32(new string(trades.Where(char.IsDigit).ToArray()));
            frontCardShortPositionData.Add("amount", amount);
            frontCardShortPositionData.Add("trades", numOfTrades);

            return frontCardShortPositionData;
        }

        public Dictionary<string, int> GetFrontCardTotalExposureData()
        {
            var frontCardTotalExposureData = new Dictionary<string, int>();
            _driver.WaitForAnimationToLoad(3000);

            var tempAmount = _driver.SearchElement(FrontTotalExposureBoxExp)
                .GetElementText(_driver, FrontTotalExposureBoxExp);

            var trades = _driver.SearchElement(FrontTotalExposureTotalTradesBoxExp)
                .GetElementText(_driver, FrontTotalExposureTotalTradesBoxExp);

            var amount = (int)tempAmount.MathRoundFromGeneric(0, MidpointRounding.ToEven);
            var numOfTrades = Convert.ToInt32(new string(trades.Where(char.IsDigit).ToArray()));
            frontCardTotalExposureData.Add("amount", amount);
            frontCardTotalExposureData.Add("trades", numOfTrades);

            return frontCardTotalExposureData;
        }

        public Dictionary<string, int> GetFrontCardPendingOrdersData()
        {
            _driver.WaitForAnimationToLoad(10000);

            var frontCardPendingOrdersData = new Dictionary<string, int>();
            _driver.SearchElement(FrontLongPositionBoxExp);

            var tempBuyAmount = _driver.SearchElement(FrontPendingOrdersBuyBoxExp)
                .GetElementText(_driver, FrontPendingOrdersBuyBoxExp);

            var tempSellAmount = _driver.SearchElement(FrontPendingOrdersSellBoxExp)
                .GetElementText(_driver, FrontPendingOrdersSellBoxExp);

            var tempExposureAmount = _driver.SearchElement(FrontPendingOrdersExposureBoxExp)
                .GetElementText(_driver, FrontPendingOrdersExposureBoxExp);

            var tempTrades = _driver.SearchElement(FrontPendingOrdersTotalTradesBoxExp)
                .GetElementText(_driver, FrontPendingOrdersTotalTradesBoxExp);

            var buyAmount = (int)tempBuyAmount.MathRoundFromGeneric(0, MidpointRounding.ToEven);
            var sellAmount = (int)tempSellAmount.MathRoundFromGeneric(0, MidpointRounding.ToEven);
            var exposureAmount = (int)tempExposureAmount.MathRoundFromGeneric(0, MidpointRounding.ToEven);
            var trades = Convert.ToInt32(new string(tempTrades.Where(char.IsDigit).ToArray()));

            frontCardPendingOrdersData.Add("buyAmount", buyAmount);
            frontCardPendingOrdersData.Add("sellAmount", sellAmount);
            frontCardPendingOrdersData.Add("exposureAmount", exposureAmount);
            frontCardPendingOrdersData.Add("trades", trades);

            return frontCardPendingOrdersData;
        }

        public T ChangeContext<T>(IWebDriver driver) where T : class
        {
            return _apiFactory.ChangeContext<T>(_driver);
        }
    }
}
