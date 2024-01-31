using System.Collections.Generic;
using System.Threading;
using AirSoftAutomationFramework.Internals.ExtensionsMethods;
using AirSoftAutomationFramework.Internals.Factorys;
using OpenQA.Selenium;

namespace AirSoftAutomationFramework.Objects.ForexObjects.Ui.Crm
{
    public class SalesPageUi : ISalesPageUi
    {
        private IWebDriver _driver;
        private string _salesProgressFilterRadio = "//label[@class='checkbox minotaur-radio sales-radio' and text()='{0} ']";
        private readonly IApplicationFactory _apiFactory;

        public SalesPageUi(IApplicationFactory apiFactory, IWebDriver driver)
        {
            _driver = driver;
            _apiFactory = apiFactory;
        }

        #region Locator's
        private readonly By BoxesTitlesExp = By.CssSelector("h4[class='group-title'],h4[class='title-per-currency']");
        private readonly By BoxesValuesExp = By.CssSelector("span[class='number-sales'],span[class='pnl-center'],span[class='volume-center'],li[class='list-currencies ng-star-inserted'],span[class='value-sales']");
        private readonly By FtdBoxExp = By.CssSelector("div[class='ftd wrapper-value']");
        private readonly By RetentionBoxExp = By.CssSelector("div[class='retention wrapper-value']");
        private readonly By DepositBoxExp = By.CssSelector("div[class='deposit-personal']");
        private readonly By TotalDepositBoxExp = By.CssSelector("div[class='total-deposit-personal']");
        private readonly By TotalPnlBoxExp = By.CssSelector("div[class='total-pnl-personal']");
        private readonly By ConversionBoxExp = By.CssSelector("div[class='conversion-personal']");
        private readonly By TotalVolumeBoxExp = By.CssSelector("div[class='total-volume-personal']");
        private readonly By SaleTitleExp = By.CssSelector("span[class='sale-title']");
        private readonly By SaleBarValueExp = By.CssSelector("span[class='sale-bar-value']");
        private readonly By ShowSellerButtonExp = By.CssSelector("label[btnradio='seller']");
        private readonly By ShowOfficeButtonExp = By.CssSelector("label[btnradio='office']");
        private readonly By ShowManagerButtonExp = By.CssSelector("label[btnradio='manager']");
        #endregion Locator's

        public List<string> GetBoxesTitles()
        {
            var FtdData = new List<string>();
            _driver.SearchElement(BoxesTitlesExp);

            _driver.SearchElements(BoxesTitlesExp)
                 .ForEach(p => FtdData.Add(p.GetElementText(_driver)));

            return FtdData;
        }

        public List<string> GetSalesProgressData()
        {
            var salesProgressData = new List<string>();
            var titleValuesList = new List<string>();
            var salseBarValuesList = new List<string>();
            var tempList = new List<string>();

            Thread.Sleep(2000);

            _driver.SearchElements(SaleTitleExp)
                .ForEach(p => tempList.AddRange(p.GetElementText(_driver).Split("|")));

            tempList.ForEach(y => titleValuesList.Add(y.Trim()));

            _driver.SearchElements(SaleBarValueExp)
                .ForEach(p => salseBarValuesList.Add(p.GetElementText(_driver).Trim()));

            salesProgressData.AddRange(titleValuesList);
            salesProgressData.AddRange(salseBarValuesList);

            return salesProgressData;
        }

        public ISalesPageUi ClickOnSalesProgressFilterByName(string filterName)
        {
            var SalesProgressFilterRadioExp = By.XPath(string.Format(_salesProgressFilterRadio, filterName));

            _driver.SearchElement(SalesProgressFilterRadioExp)
                .ForceClick(_driver, SalesProgressFilterRadioExp);

            return this;
        }

        public ISalesPageUi ClickOnShowSeller()
        {
            _driver.SearchElement(ShowSellerButtonExp)
                .ForceClick(_driver, ShowSellerButtonExp);

            return this;
        }

        public ISalesPageUi ClickOnShowOffice()
        {
            _driver.SearchElement(ShowOfficeButtonExp)
                .ForceClick(_driver, ShowOfficeButtonExp);

            return this;
        }

        public ISalesPageUi ClickOnShowManager()
        {
            _driver.SearchElement(ShowManagerButtonExp)
                .ForceClick(_driver, ShowManagerButtonExp);

            // wait for contant change
           // Thread.Sleep(3000);

            return this;
        }

        public ISalesPageUi ClickOnSalesModeFilterByName(string filterName)
        {
            var SalesProgressFilterRadioExp = By.XPath(string.Format(_salesProgressFilterRadio, filterName));

            _driver.SearchElement(SalesProgressFilterRadioExp)
                .ForceClick(_driver, SalesProgressFilterRadioExp);

            return this;
        }

        public List<string> GetBoxesValues()
        {
            var BoxesValues = new List<string>();
            _driver.SearchElement(BoxesValuesExp);

            _driver.SearchElements(BoxesValuesExp)
                 .ForEach(p => BoxesValues.Add(p.GetElementText(_driver)
                 ?.Trim()));

            return BoxesValues;
        }

        public T ChangeContext<T>(IWebDriver driver) where T : class
        {
            return _apiFactory.ChangeContext<T>(_driver);
        }

    }
}
