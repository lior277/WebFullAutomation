using AirSoftAutomationFramework.Internals;
using AirSoftAutomationFramework.Internals.Extensions;
using OpenQA.Selenium;

namespace AirSoftAutomationFramework.Objects.SharedObjectsForCbdAndForex.Ui.Accounts.UsersPage
{
    public class GridSearchResultUsersUi : ApplicationFactory, IGridSearchResultUsersUi
    {
        private IWebDriver _driver;

        public GridSearchResultUsersUi(IWebDriver driver)
        {
            _driver = driver;
        }

        #region Locator's
        private readonly By EditUserButtonExp = By.CssSelector("button[class*='search-result-edit-user-btn']");      
        private readonly By TableRowExp = By.XPath("//table[@id='users']/tbody/tr | //table[@id='customersTable']/tbody/tr"); // need check
        #endregion Locator's

        public IEditUserUi ClickOnEditUserButton()
        {
            _driver.WaitForNumberOfElements(TableRowExp, 1);
            _driver.GetElement(EditUserButtonExp).ForceClick(_driver, EditUserButtonExp);

            return ChangeContext<IEditUserUi>(_driver); ;
        }
    }
}
