using AirSoftAutomationFramework.Internals.ExtensionsMethods;
using AirSoftAutomationFramework.Internals.Factory;
using AirSoftAutomationFramework.Objects.ForexObjects.Ui.UiGeneral;
using OpenQA.Selenium;

namespace AirSoftAutomationFramework.Objects.ForexObjects.Ui.Crm.Accounts.RolesPage
{
    public class RolesPageUi : IRolesPageUi
    {
        private readonly IApplicationFactory _apiFactory;
        private IWebDriver _driver;

        public RolesPageUi(IApplicationFactory apiFactory, IWebDriver driver)
        {
            _driver = driver;
            _apiFactory = apiFactory;
        }

        #region Locator's       
        private readonly By CreateRoleButtonExp = By.CssSelector("button[class*='create-role-btn']");
        private readonly By RoleTableSearchExp = By.CssSelector("div[id*='DataTables_Table'] input[type='search']");
        #endregion Locator's 

        public IRoleUi ClickOnCreateRoleButton()
        {
            _driver.SearchElement(CreateRoleButtonExp)
                .ForceClick(_driver, CreateRoleButtonExp);
            //_driver.WaitForAnimationToLoad(300);

            return ChangeContext<IRoleUi>(_driver);
        }

        public ISearchResultsUi SearchRole(string searchText, bool refresh = false)
        {
            if (refresh)
            {
                _driver.Navigate().Refresh();
            }

            _driver.WaitForAnimationToLoad(3000);  
            
            _driver.SearchElement(RoleTableSearchExp, 60)
                .SendsKeysAuto(_driver, RoleTableSearchExp, searchText, 60);


            return _apiFactory.ChangeContext<ISearchResultsUi>(_driver);
        }

        public T ChangeContext<T>(IWebDriver driver) where T : class
        {
            return _apiFactory.ChangeContext<T>(_driver);
        }
    }
}
