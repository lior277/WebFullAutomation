// Ignore Spelling: Forex api

using System;
using System.Collections.Generic;
using System.Linq;
using AirSoftAutomationFramework.Internals.ExtensionsMethods;
using AirSoftAutomationFramework.Internals.Factory;
using OpenQA.Selenium;

namespace AirSoftAutomationFramework.Objects.ForexObjects.Ui.UiGeneral
{
    public class AgentProfileUi : IAgentProfileUi
    {
        #region Members
        public IWebDriver _driver;
        private readonly IApplicationFactory _apiFactory;
        #endregion Members

        public AgentProfileUi(IApplicationFactory apiFactory, IWebDriver driver)
        {
            _apiFactory = apiFactory;
            _driver = driver;
        }

        #region Locator's
        private readonly By SellerRoundProgressDataExp = By.CssSelector(
            "h3[class='current-text']");

        private readonly By PrevMonthExp = By.CssSelector(
           "div[class='target-value-text']");

        private readonly By TopTenDepositsExp = By.CssSelector(
          "div[class='top-deposits-wrap'] li[class='li-top-item ng-star-inserted']");

        private readonly By TopTenPnlExp = By.CssSelector(
         "div[class='top-pnl-wrap'] li[class='li-top-item ng-star-inserted']");

        private readonly By TopStatisticsBoxCurrencySignExp = By.CssSelector(
        "div[class='top-statistics-box'] i[class='fa fa-euro']");

        private readonly By ChartExp = By.CssSelector(
        "div[class='chart-wrap']");

        private readonly By CloseAgentProfileBtnExp = By.CssSelector(
           "button[class='close pull-right close-sales-profile']");
        #endregion

        public List<string> GetSellerRoundProgressData()
        {
            _driver.WaitForExactNumberOfElements(SellerRoundProgressDataExp, 6);
            var elements = _driver.SearchElements(SellerRoundProgressDataExp);
            var actualSellerRoundProgressData = new List<string>();

            elements.ForEach(p => actualSellerRoundProgressData.Add(p.GetElementText(_driver)
                .TrimEnd()
                .Replace(Environment.NewLine, string.Empty)));

            return actualSellerRoundProgressData;
        }

        public List<string> GetPrevMouthData()
        {
            var elements = _driver.SearchElements(PrevMonthExp);
            var actualPrevMounthData = new List<string>();

            elements.ForEach(p => actualPrevMounthData.Add(p.GetElementText(_driver)
                .TrimEnd()
                .Replace(Environment.NewLine, string.Empty)));

            return actualPrevMounthData;
        }

        public IAgentProfileUi CloseAgentProfile()
        {
            _driver.SearchElement(CloseAgentProfileBtnExp)
                .ForceClick(_driver, CloseAgentProfileBtnExp);

            return this;
        }

        public List<string> GetTopTenDeposits()
        {
            _driver.WaitForExactNumberOfElements(TopTenDepositsExp, 3);
            var elements = _driver.SearchElements(TopTenDepositsExp);
            var actualTopTenDepositsData = new List<string>();
            elements.ForEach(p => actualTopTenDepositsData.Add(p.GetElementText(_driver)));

            return actualTopTenDepositsData;
        }

        public List<string> GetTopTenPnl()
        {
            _driver.WaitForExactNumberOfElements(TopTenPnlExp, 2);
            var elements = _driver.SearchElements(TopTenPnlExp);
            var actualTopTenPnlData = new List<string>();
            elements.ForEach(p => actualTopTenPnlData.Add(p.GetElementText(_driver)));

            return actualTopTenPnlData;
        }

        public bool IsChartExist()
        {
            return _driver.WaitForExactNumberOfElements(ChartExp, 1) > 0;
        }

        public int GetTopStatisticsBoxCurrencySign()
        {
            return _driver.WaitForExactNumberOfElements(TopStatisticsBoxCurrencySignExp, 5);
        }

        public T ChangeContext<T>(IWebDriver driver) where T : class
        {
            return _apiFactory.ChangeContext<T>(_driver);
        }
    }
}
