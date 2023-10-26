using System;
using AirSoftAutomationFramework.Internals.ExtensionsMethods;
using AirSoftAutomationFramework.Internals.Factory;
using AirSoftAutomationFramework.Internals.Helpers;
using AirSoftAutomationFramework.Objects.ForexObjects.Ui.UiGeneral;
using OpenQA.Selenium;

namespace AirSoftAutomationFramework.Objects.ForexObjects.Ui.Crm.Banking
{
    public class WithdrawalsPageUi : IWithdrawalsPageUi
    {
        private IWebDriver _driver;
        private IHandleFiltersUi _handleFilters;
        private readonly IApplicationFactory _apiFactory;

        public WithdrawalsPageUi(IHandleFiltersUi handleFilters,
            IApplicationFactory apiFactory, IWebDriver driver)
        {
            _handleFilters = handleFilters;
            _apiFactory = apiFactory;
            _driver = driver;
        }

        #region Locator's   
        private readonly By SelectTransactionStatusExp = By.CssSelector("select[class='select-sales-agent withdrawal-status banking-withdrawal']");
        private readonly By WithdrawalsTableIdColumnExp = By.XPath("//th[text()='Id']"); //By.CssSelector("table thead th[aria-label='Id']:not([style*='padding-top'])");
        private readonly By SelectPspExp = By.CssSelector("select[class='select-psp-name']");
        private readonly By SelectAssignToExp = By.CssSelector("select[class='select-assigned-to']");
        private readonly By WithdrawalGridSearchExp = By.CssSelector("div[id*='transactionTable'] input[type='search']");

        #endregion Locator's     

        public IWithdrawalsPageUi CheckIfIdColumnExist()
        {
            _driver.SearchElement(WithdrawalsTableIdColumnExp, 100);

            return this;
        }

        public IWithdrawalsPageUi SelectTransactionStatus(string transactionStatus)
        {
            _driver.SearchElement(SelectTransactionStatusExp)
                .SelectElementFromDropDownByText(_driver, SelectTransactionStatusExp, transactionStatus);

            return this;
        }

        public IWithdrawalsPageUi SelectAssignTo(string assignTo)
        {
            _driver.SearchElement(SelectAssignToExp)
                .SelectElementFromDropDownByText(_driver, SelectAssignToExp, assignTo);

            return this;
        }

        public IWithdrawalsPageUi SelectPsp(string pspName)
        {
            _driver.SearchElement(SelectPspExp)
                .SelectElementFromDropDownByText(_driver, SelectPspExp, pspName);

            return this;
        }

        public IWithdrawalsPageUi VerifyMessages(string message)
        {
            var alertExp = By.XPath(string.Format(DataRep.AlertOnFront, message));

            var text = _driver.SearchElement(alertExp)
                .GetElementText(_driver, alertExp);

            text.StringContains(message);

            return this;
        }

        public IWithdrawalsPageUi SearchWithdrawal(int withdrawalId)
        {
            _driver.SearchElement(WithdrawalGridSearchExp)
                .SendsKeysAuto(_driver, WithdrawalGridSearchExp, withdrawalId.ToString());

            return this;
        }

        public T ChangeContext<T>(IWebDriver driver) where T : class
        {
            return _apiFactory.ChangeContext<T>(_driver);
        }
    }
}
