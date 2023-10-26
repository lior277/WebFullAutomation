using AirSoftAutomationFramework.Internals.ExtensionsMethods;
using AirSoftAutomationFramework.Internals.Factory;
using AirSoftAutomationFramework.Internals.Helpers;
using AirSoftAutomationFramework.Objects.ForexObjects.Ui.UiGeneral;
using OpenQA.Selenium;

namespace AirSoftAutomationFramework.Objects.ForexObjects.Ui.Crm.Banking
{
    public class ChargebacksPageUi : IChargebacksPageUi
    {
        private IWebDriver _driver;
        private IHandleFiltersUi _handleFilters;
        private readonly IApplicationFactory _apiFactory;

        public ChargebacksPageUi(IHandleFiltersUi handleFilters,
            IApplicationFactory apiFactory, IWebDriver driver)
        {
            _handleFilters = handleFilters;
            _apiFactory = apiFactory;
            _driver = driver;
        }

        #region Locator's   
        private readonly By ChargebacksTableIdColumnExp = By.XPath("//th[text()='Id']");// By.CssSelector("table thead th[aria-label='Id']:not([style*='padding-top'])");
        private readonly By ChargebackGridSearchExp = By.CssSelector("div[id*='transactionTable'] input[type='search']");
        private readonly By SelectAssignToExp = By.CssSelector("select[class='select-assigned-to']");
        private readonly By SelectPspExp = By.CssSelector("select[class='select-psp-name']");
        #endregion Locator's     

        public IChargebacksPageUi CheckIfIdColumnExist()
        {
            _driver.SearchElement(ChargebacksTableIdColumnExp);

            return this;
        }

        public IChargebacksPageUi SearchChargeback(int ChargebackId)
        {
            _driver.SearchElement(ChargebackGridSearchExp)
                .SendsKeysAuto(_driver, ChargebackGridSearchExp, ChargebackId.ToString());

            return this;
        }

        public IChargebacksPageUi SelectAssignTo(string assignTo)
        {
            _driver.SearchElement(SelectAssignToExp)
                .SelectElementFromDropDownByText(_driver, SelectAssignToExp, assignTo);

            return this;
        }

        public IChargebacksPageUi SelectPsp(string pspName)
        {
            _driver.SearchElement(SelectPspExp)
                .SelectElementFromDropDownByText(_driver, SelectPspExp, pspName);

            return this;
        }

        public IChargebacksPageUi VerifyMessages(string message)
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
