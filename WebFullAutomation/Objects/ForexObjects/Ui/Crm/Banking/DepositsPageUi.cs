using System;
using AirSoftAutomationFramework.Internals.ExtensionsMethods;
using AirSoftAutomationFramework.Internals.Factorys;
using AirSoftAutomationFramework.Internals.Helpers;
using AirSoftAutomationFramework.Objects.ForexObjects.ApiGeneral;
using AirSoftAutomationFramework.Objects.ForexObjects.Ui.UiGeneral;
using OpenQA.Selenium;

namespace AirSoftAutomationFramework.Objects.ForexObjects.Ui.Crm.Banking
{
    public class DepositsPageUi : IDepositsPageUi
    {
        private IWebDriver _driver;
        private IHandleFiltersUi _handleFilters;
        private readonly IApplicationFactory _apiFactory;

        public DepositsPageUi(IHandleFiltersUi handleFilters,
            IApplicationFactory apiFactory, IWebDriver driver)
        {
            _handleFilters = handleFilters;
            _apiFactory = apiFactory;
            _driver = driver;
        }

        #region Locator's    
        private readonly By DepositGridSearchExp = By.CssSelector("div[id*='transactionTable'] input[type='search']");
        private readonly By FullNameExp = By.CssSelector("a[class='getClient']");
        private readonly By DepositsTableIdColumnExp = By.XPath("//th[text()='Id']");// By.CssSelector("table thead th[aria-label='Id']:not([style*='padding-top'])");
        private readonly By DepositStatusFilterExp = By.CssSelector("div[class*='status-filter']");
        private readonly By DepositTableRowExp = By.CssSelector("table tr");
        private readonly By NumOfRowsInDepositTableExp = By.CssSelector("div[class='dataTables_scrollBody']");
        private readonly By DepositDataTableRowsExp = By.CssSelector("table[id='transactionTable'] tr[class='odd']");
        private readonly By SearchDepositExp = By.CssSelector("input[type='search']");
        private readonly By WaitForProcessingExp = By.CssSelector("div[id='transactionTable_processing'][style='display: none;']");
        #endregion Locator's     

        public ISearchResultsUi SearchDeposit(string searchText)
        {
            _driver.SearchElement(DepositGridSearchExp)
                .SendsKeysAuto(_driver, DepositGridSearchExp, searchText);

            _driver.WaitForAtListOneElement(DepositDataTableRowsExp, 60);

            return _apiFactory.ChangeContext<ISearchResultsUi>(_driver);
        }

        public string GetDepositStatusFilterValue()
        {
            return _driver.SearchElement(DepositStatusFilterExp)
                .GetElementText(_driver, DepositStatusFilterExp);
        }

        public IDepositsPageUi WaitForNumOfRowsInDepositTable()
        {
            _driver.SearchElement(WaitForProcessingExp, 60);

            return this;
        }

        public IDepositsPageUi WaitForDepositTableToLoad()
        {
            _apiFactory
                .ChangeContext<ISharedStepsGenerator>(_driver)
                .WaitForTableToLoad();

            return this;
        }

        public IDepositsPageUi SearchDepositByClientEmail(string clientEmail)
        {
            _driver.SearchElement(SearchDepositExp)
                .SendsKeysAuto(_driver, SearchDepositExp, clientEmail);

            return this;
        }

        public IDepositsPageUi CheckIfIdColumnExist()
        {
            _driver.SearchElement(DepositsTableIdColumnExp);

            return this;
        }

        public T ChangeContext<T>(IWebDriver driver) where T : class
        {
            return _apiFactory.ChangeContext<T>(_driver);
        }
    }
}
