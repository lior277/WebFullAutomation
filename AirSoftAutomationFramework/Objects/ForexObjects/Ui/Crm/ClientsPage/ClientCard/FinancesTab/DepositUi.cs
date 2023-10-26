using AirSoftAutomationFramework.Internals.ExtensionsMethods;
using AirSoftAutomationFramework.Internals.Factory;
using AirSoftAutomationFramework.Internals.Helpers;
using OpenQA.Selenium;

namespace AirSoftAutomationFramework.Objects.ForexObjects.Ui.Crm.ClientsPage.ClientCard.FinancesTab
{
    public class DepositUi : IDepositUi
    {
        public IWebDriver _driver;
        private readonly string _depositAmount = "10";
        private readonly string _depositMethod = "bank transfer";
        private readonly string _depositDetails = "deposit";
        private readonly string _transactionCurrency = DataRep.DefaultUSDCurrencyName;
        private readonly IApplicationFactory _apiFactory;

        public DepositUi(IApplicationFactory apiFactory, IWebDriver driver)
        {
            _driver = driver;
            _apiFactory = apiFactory;
        }

        #region Locator's
        private readonly By DepositAmountTextBoxExp = By.CssSelector("input[class*='create-deposit-amount']");
        private readonly By DepositMethodListBoxExp = By.XPath("//select[contains(@class,'create-deposit-method')]");
        private readonly By DepositDetailsTextBoxExp = By.CssSelector("input[class*='create-deposit-details']");
        private readonly By TransactionCurrencyListBoxExp = By.XPath("//select[contains(@class,'create-deposit-transaction-currency')]");
        private readonly By AddMoreInfoExp = By.CssSelector("div[class*='create-deposit-add-more-info']");
        private readonly By TransactionIdExp = By.CssSelector("input[class*='create-deposit-transaction-id']");
        private readonly By LastFourDigitsExp = By.CssSelector("input[class*='create-deposit-last-4-digit']");
        private readonly By SaveDepositButtonExp = By.CssSelector("button[class*='create-deposit-save-btn']");
        private readonly By ActionAlertExp = By.CssSelector("div[class*='action-alert']");
        private readonly By ConfirmDepositExp = By.XPath("//button[contains(.,'Confirm')]");
        #endregion Locator's

        public IDepositUi SetDepositAmount(string depositAmount = null)
        {
            depositAmount ??= _depositAmount;
            //_driver.WaitForAnimationToLoad(400);
            _driver.SearchElement(DepositAmountTextBoxExp, 360)
                .SendsKeysAuto(_driver, DepositAmountTextBoxExp, depositAmount);

            return this;
        }

        public IDepositUi SetDepositMethod(string typeDepositMethod = null)
        {
            typeDepositMethod ??= _depositMethod;

            _driver.SearchElement(DepositMethodListBoxExp)
                .SelectElementFromDropDownByText(_driver, DepositMethodListBoxExp, typeDepositMethod, 60);

            return this;
        }

        public IDepositUi SetDepositDetails(string TypeDepositDetails = null)
        {
            TypeDepositDetails ??= _depositDetails;

            _driver.SearchElement(DepositDetailsTextBoxExp)
                .SendsKeysAuto(_driver, DepositDetailsTextBoxExp, TypeDepositDetails);

            return this;
        }

        public IDepositUi SetTransactionCurrency(string typeTransactionCurrency = null)
        {
            typeTransactionCurrency ??= _transactionCurrency;

            _driver.SearchElement(TransactionCurrencyListBoxExp)
                .SelectElementFromDropDownByText(_driver, TransactionCurrencyListBoxExp, typeTransactionCurrency);

            _driver.SearchElement(TransactionCurrencyListBoxExp)
                .ForceClick(_driver, TransactionCurrencyListBoxExp);

            return this;
        }

        public IFinanceTabUi SaveDeposit()
        {
            _driver.SearchElement(DataRep.SaveExp)
               .ForceClick(_driver, DataRep.SaveExp);

            var element = _driver.SearchElement(ConfirmDepositExp);
            element.RetryClickTillElementNotDisplayJavaScript(_driver, ConfirmDepositExp);         

            return _apiFactory.ChangeContext<IFinanceTabUi>(_driver);
        }

        public IFinanceTabUi CreateDeposit(string depositAmount = "10")
        {
            SetDepositAmount(depositAmount);
            SetDepositMethod();
            SetDepositDetails();
            SetTransactionCurrency();
            SaveDeposit();

            return _apiFactory.ChangeContext<IFinanceTabUi>(_driver);
        }

        public T ChangeContext<T>(IWebDriver driver) where T : class
        {
            return _apiFactory.ChangeContext<T>(_driver);
        }
    }
}
