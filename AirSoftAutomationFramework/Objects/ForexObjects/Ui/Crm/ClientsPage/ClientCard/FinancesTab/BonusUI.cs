using System.Linq;
using AirSoftAutomationFramework.Internals.ExtensionsMethods;
using AirSoftAutomationFramework.Internals.Factory;
using AirSoftAutomationFramework.Internals.Helpers;
using OpenQA.Selenium;

namespace AirSoftAutomationFramework.Objects.ForexObjects.Ui.Crm.ClientsPage.ClientCard.FinancesTab
{
    public class BonusUi : IBonusUi
    {
        public IWebDriver _driver;
        private readonly IApplicationFactory _apiFactory;

        public BonusUi(IApplicationFactory apiFactory, IWebDriver driver)
        {
            _driver = driver;
            _apiFactory = apiFactory;
        }

        #region Locator's
        private readonly By BonusAmountTextBoxExp = By.CssSelector("input[id='amount']");
        private readonly By BonusMethodBoxExp = By.CssSelector("label[class*='ui-dropdown-label']");
        private readonly By BonusMethodItemExp = By.CssSelector("li[class*='ui-dropdown-item']");
        private readonly By SaveDepositButtonExp = By.CssSelector("button[class*='create-deposit-save-btn']");
        private readonly By BonusMethodDropDounExp = By.CssSelector("div[class='ui-dropdown-items-wrapper']");
        #endregion Locator's

        public IBonusUi SetBonusAmount(int bonusAmount = 1000)
        {
            var amount = bonusAmount;

            _driver.SearchElement(BonusAmountTextBoxExp)
                .SendsKeysAuto(_driver, BonusAmountTextBoxExp, amount.ToString());

            return this;
        }

        public IBonusUi SetBonusMethod(string bonusMethod = null)
        {
            var method = bonusMethod ?? "Bonus";

            _driver.SearchElement(BonusMethodBoxExp)
                .ForceClick(_driver, BonusMethodBoxExp);

            _driver.SearchElements(BonusMethodItemExp)
                .Where(p => p.Text == method)
                .FirstOrDefault()?
                .Click();

            _driver.WaitForElementNotExist(BonusMethodDropDounExp);

            return this;
        }

        public IFinanceTabUi SaveBonus()
        {
            _driver.SearchElement(DataRep.SaveExp)
               .ForceClick(_driver, DataRep.SaveExp);

            return _apiFactory.ChangeContext<IFinanceTabUi>(_driver);
        }

        public IFinanceTabUi CreateBonus(int bonusAmount = 1000)
        {
            SetBonusAmount(bonusAmount);
            SetBonusMethod();
            SaveBonus();

            return _apiFactory.ChangeContext<IFinanceTabUi>(_driver);
        }

        public T ChangeContext<T>(IWebDriver driver) where T : class
        {
            return _apiFactory.ChangeContext<T>(_driver);
        }

    }
}
