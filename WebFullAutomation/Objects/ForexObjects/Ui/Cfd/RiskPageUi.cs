using AirSoftAutomationFramework.Internals.DAL.ConfigurationManagement;
using AirSoftAutomationFramework.Internals.ExtensionsMethods;
using AirSoftAutomationFramework.Objects.DTOs;
using AirSoftAutomationFramework.Objects.ForexObjects.ApiGeneral;
using AirSoftAutomationFramework.Objects.ForexObjects.Ui.UiGeneral;
using Newtonsoft.Json;
using OpenQA.Selenium;
using System.Collections.Generic;
using System.Linq;
using AirSoftAutomationFramework.Internals.Factorys;

namespace AirSoftAutomationFramework.Objects.ForexObjects.Ui.Cfd
{
    public class RiskPageUi : IRiskPageUi
    {
        private IWebDriver _driver;
        private readonly IApplicationFactory _apiFactory;

        #region Locator's    
        private readonly By RisksTableSearchFiledExp = By.CssSelector("div[id='riskTable_filter'] input[type='search']");
        private readonly By WaitForProcessingRiskExp = By.CssSelector("div[id='riskTable_processing'][style='display: none;']");
        private readonly By RowsExp = By.XPath("//table[contains(@class,'search-result-risk')]/tbody/tr/td[not(contains(@class,'dataTables_empty'))]/parent::tr");
        #endregion Locator's     

        public RiskPageUi(IApplicationFactory apiFactory, IWebDriver driver)
        {
            _apiFactory = apiFactory;
            _driver = driver;
        }

        public IRiskPageUi WaitForRiskTableToLoad()
        {
            _apiFactory
                .ChangeContext<ISharedStepsGenerator>(_driver)
                .WaitForTableToLoad();

            return this;
        }

        public IRiskPageUi WaitForNumOfElementInRiskTable(int expectedNumOfRows)
        {
            _driver.WaitForExactNumberOfElements(RowsExp, expectedNumOfRows);

            return this;
        }

        public ISearchResultsUi SearchRisks(string searchText)
        {
            WaitForRiskTableToLoad();
            var element = _driver.SearchElement(RisksTableSearchFiledExp);
            element.SendsKeysAuto(_driver, RisksTableSearchFiledExp, searchText);

            return _apiFactory.ChangeContext<ISearchResultsUi>(_driver);
        }

        public T ChangeContext<T>(IWebDriver driver) where T : class
        {
            return _apiFactory.ChangeContext<T>(_driver);
        }
    }
}
