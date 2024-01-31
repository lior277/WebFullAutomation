using System;
using System.Collections.Generic;
using System.Linq;
using AirSoftAutomationFramework.Internals.ExtensionsMethods;
using AirSoftAutomationFramework.Internals.Factorys;
using AirSoftAutomationFramework.Objects.ForexObjects.Ui.UiGeneral;
using OpenQA.Selenium;

namespace AirSoftAutomationFramework.Objects.ForexObjects.Ui.Crm.ClientsPage.ClientCard.FinancesTab
{
    public class FinanceTabUi : IFinanceTabUi
    {
        private IWebDriver _driver;
        private readonly IApplicationFactory _apiFactory;
        private string _financeButtons = "//button[contains(.,'{0}')]";

        public FinanceTabUi(IApplicationFactory apiFactory, IWebDriver driver)
        {
            _driver = driver;
            _apiFactory = apiFactory;
        }

        #region Locator's
        private readonly By TransactionSammeryExp = By.CssSelector("span[class='trades-heading-info']");
        private readonly By FinanceTableSearchExp = By.CssSelector("div[id='financeTabTable_filter'] input[type='search']");
        private readonly By CustomersTableShowingCounterExp = By.CssSelector("div[id*='financeTab_info']");
        private readonly By FinanceTableIdColumnExp = By.XPath("//th[text()='Id']");// By.CssSelector("table thead th[aria-label='Id: activate to sort column ascending']:not([style*='padding-top'])");
        #endregion Locator's

        public IFinanceTabUi ClickOnFinanceButton(string financeButtonsName)
        {
            var FinanceButtonExp = By.XPath(string.Format(_financeButtons, financeButtonsName));

            _driver.WaitForAnimationToLoad();

            _driver.SearchElement(FinanceButtonExp)
                .ForceClick(_driver, FinanceButtonExp);

            return this;
        }

        public IFinanceTabUi CheckIfIdColumnExist()
        {
            _driver.SearchElement(FinanceTableIdColumnExp);

            return this;
        }

        public IDictionary<string, string> GetTransactionSammery()
        {
            var transactionSammery = new Dictionary<string, string>();
            _driver.WaitForExactNumberOfElements(TransactionSammeryExp, 9);
            var elements = _driver.SearchElements(TransactionSammeryExp);

            elements.ForEach(p => transactionSammery.Add(p.GetElementText(_driver).Split(':').First().TrimStart().TrimEnd(),
                p.GetElementText(_driver).Split(':').Last().TrimStart().TrimEnd()));

            return transactionSammery;
        }

        public ISearchResultsUi SearchFinance(string searchText)
        {
            _driver.SearchElement(FinanceTableSearchExp)
                .SendsKeysAuto(_driver, FinanceTableSearchExp, searchText);

            return _apiFactory.ChangeContext<ISearchResultsUi>(_driver);
        }

        public T ChangeContext<T>(IWebDriver driver) where T : class
        {
            return _apiFactory.ChangeContext<T>(_driver);
        }
    }
}
