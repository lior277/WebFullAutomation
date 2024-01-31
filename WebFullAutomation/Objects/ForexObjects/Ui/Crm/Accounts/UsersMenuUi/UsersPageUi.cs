using AirSoftAutomationFramework.Internals.ExtensionsMethods;
using AirSoftAutomationFramework.Internals.Factorys;
using AirSoftAutomationFramework.Internals.Helpers;
using AirSoftAutomationFramework.Objects.ForexObjects.ApiGeneral;
using AirSoftAutomationFramework.Objects.ForexObjects.Ui.UiGeneral;
using OpenQA.Selenium;

namespace AirSoftAutomationFramework.Objects.ForexObjects.Ui.Crm.Accounts.UsersMenuUi
{
    public class UsersPageUi : IUsersPageUi
    {
        //private string _email;
        private IWebDriver _driver;
        private readonly IApplicationFactory _apiFactory;
        private string _mailPerfix = DataRep.EmailPrefix;

        public UsersPageUi(IApplicationFactory apiFactory, IWebDriver driver)
        {
            _apiFactory = apiFactory;
            _driver = driver;
        }

        #region Locator's    
        private readonly By CreateUserDialogIsOpenExp = By.CssSelector("div[aria-hidden= 'false']");
        private readonly By UserGridSearchExp = By.CssSelector("div[id*='users'] input[type='search']");
        private readonly By CreateUserButtonExp = By.CssSelector("button[class*='new-user']");
        private readonly By EditUserButtonExp = By.CssSelector("button[class*='edit-user']");
        private readonly By CustomersTableShowingCounterExp = By.CssSelector("div[id*='users_info']");
        private readonly By WaitForProcessingUsersExp = By.CssSelector("span[class='custom-date'][style='display: none']");
        #endregion Locator's     

        public IUserUi ClickOnCreateUserButton()
        {
            _driver.SearchElement(CreateUserButtonExp)
                .ForceClick(_driver, CreateUserButtonExp);

            _driver.SearchElement(CreateUserDialogIsOpenExp);

            return _apiFactory.ChangeContext<IUserUi>(_driver);
        }

        public IUserUi ClickOnEditUserButton()
        {
            _driver.SearchElement(EditUserButtonExp)
                .ForceClick(_driver, EditUserButtonExp);

            _driver.SearchElement(CreateUserDialogIsOpenExp);

            return _apiFactory.ChangeContext<IUserUi>(_driver);
        }

        public ISearchResultsUi SearchUser(string searchText)
        {
            var element = _driver.SearchElement(UserGridSearchExp);
            element.ForceClick(_driver, UserGridSearchExp);
            element.SendsKeysCharByChar(_driver, UserGridSearchExp, searchText);

            return _apiFactory.ChangeContext<ISearchResultsUi>(_driver);
        }

        public T ChangeContext<T>(IWebDriver driver) where T : class
        {
            return _apiFactory.ChangeContext<T>(_driver);
        }
    }
}
