using AirSoftAutomationFramework.CrmObjects.Ui.Accounts.RolesPage;
using AirSoftAutomationFramework.Internals;
using AirSoftAutomationFramework.Internals.Extensions;
using OpenQA.Selenium;

namespace AirSoftAutomationFramework.Objects.SharedObjectsForCbdAndForex.Ui.Accounts.RolesPage
{
    public class GridSearchResultRolesUi : IGridSearchResultRolesUi
    {
        private IWebDriver _driver;
        private IApplicationFactory _apiFactory;

        public GridSearchResultRolesUi(IApplicationFactory apiFactory, IWebDriver driver)
        {
            _driver = driver;
            _apiFactory = apiFactory;
        }

        #region Locator's
        private readonly By EditRoleButtonExp = By.CssSelector("button[class*='edit-role-btn']");
        private readonly By DeleteRoleButtonExp = By.CssSelector("button[class*='delete-role-btn']");
        private readonly By ConfirmDeleteRoleButtonExp = By.CssSelector("button[class*='confirm-delete-role-btn']");
        private readonly By CancelDeleteRoleButtonExp = By.CssSelector("button[class*='cancel-delete-role-btn']");
        private readonly By TableRowExp = By.XPath("//table[@id='users']/tbody/tr |" +
            " //table[@id='customersTable']/tbody/tr | //table[@id='DataTables_Table_5']/tbody/tr"); // need check
        #endregion Locator's

        public ICreateRoleUi ClickOnEditRoleButton()
        {
            _driver.WaitForNumberOfElements(TableRowExp, 1);
            _driver.GetElement(EditRoleButtonExp).ForceClick(_driver, EditRoleButtonExp);

            return _apiFactory.ChangeContext<ICreateRoleUi>(_driver);
        }

        public ICreateRoleUi ClickOnDeleteRoleButton()
        {
            _driver.WaitForNumberOfElements(TableRowExp, 1);
            _driver.GetElement(DeleteRoleButtonExp).ForceClick(_driver, DeleteRoleButtonExp);
            _driver.GetElement(ConfirmDeleteRoleButtonExp).ForceClick(_driver, ConfirmDeleteRoleButtonExp);

            return _apiFactory.ChangeContext<ICreateRoleUi>(_driver);
        }

        public T ChangeContext<T>(IWebDriver driver) where T : class
        {
            return _apiFactory.ChangeContext<T>(_driver);
        }
    }
}
