// Ignore Spelling: Forex api

using AirSoftAutomationFramework.Internals.ExtensionsMethods;
using AirSoftAutomationFramework.Internals.Factory;
using AirSoftAutomationFramework.Internals.Helpers;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.AccountsApi.UsersMenuApi.UserMenuApi;
using AirSoftAutomationFramework.Objects.ForexObjects.TradingPlatform.TradeObjects.Ui.Settings;
using OpenQA.Selenium;
using System.Threading;

namespace AirSoftAutomationFramework.Objects.ForexObjects.Ui.Login
{
    public class LoginPageUi : ILoginPageUi
    {
        private string _mailPerfix = DataRep.EmailPrefix;
        private string _password = DataRep.Password;
        private IWebDriver _driver;
        private string _userName;
        private readonly IApplicationFactory _apiFactory;
        private IUserApi _createUserApi;
        private string _loginErorMessage = "//span[contains(.,'{0}')]";

        public LoginPageUi(IApplicationFactory apiFactory, IUserApi createUserApi,
            IWebDriver driver)
        {
            _createUserApi = createUserApi;
            _apiFactory = apiFactory;
            _driver = driver;
        }

        #region Locator's   
        private static By _onfirmExp = By.CssSelector("button[class*='confirm-action-btn']");
        private static By _onfirmPasswordExp = By.CssSelector("input[name='passwordValidation']");
        private static By _loginBtnSpinnerExp = By.CssSelector("button[class*='lds-ring']:disabled");
        public static By LoginMessageExp = By.CssSelector("span[class='error']");
        public static By LogOutBtnExp = By.CssSelector("a[title='Logout']");
        public static By OkBtnExp = By.CssSelector("button[class='btn btn-success']");
        public static By ApproveBtnExp = By.CssSelector("button[class*='btn btn-success']");
        public static By DeclineBtnExp = By.CssSelector("button[class*='btn btn-danger']");
        public static By PageTitleExp = By.CssSelector("title");
        public static By OpenSignaturePadExp = By.CssSelector("span[id='signature-field-id']");
        public static By LoginBtnExp = By.CssSelector("button[type='submit']");
        public static By GotItBtnExp = By.CssSelector("button[id = 'mgm-message-button']");
        #endregion Locator's

        public ILoginPageUi NavigateToUrl(string url)
        {
            // the Maximize is done in chrome options

            _driver.Navigate().GoToUrl(url);

            return this;
        }

        public ILoginPageUi SetUserName(string userName)
        {
            _userName = userName ?? userName + _mailPerfix;

            if (userName.Contains('@'))
            {
                _userName = userName;
            }
            else
            {
                _userName = userName + _mailPerfix;
            }
            //if (switchToTab)
            //{
            //    _driver.SwitchBetweenBrowsersTabs(_driver.Url, TabToSwitch.Last);
            //}

            //Thread.Sleep(1000);
            var element = _driver.SearchElement(DataRep.UserNamedExp);
            element.SendsKeysAuto(_driver, DataRep.UserNamedExp, _userName);

            return this;
        }

        public string GetLoginMessage(string errorMessage)
        {
            var loginErorMessageExp = By.XPath(string.Format(_loginErorMessage, errorMessage));

            return _driver.SearchElement(loginErorMessageExp)
                .GetElementText(_driver, loginErorMessageExp);
        }

        public ILoginPageUi SetPassword(string password = null)
        {
            _password = password ?? _password;

            _driver.SearchElement(DataRep.PasswordExp)
                .SendsKeysAuto(_driver, DataRep.PasswordExp, _password);

            return this;
        }

        public ILoginPageUi ChangePasswordPipe(string NewPassword, string currentUrl)
        {
            _driver.SearchElement(DataRep.PasswordExp)
                .SendsKeysAuto(_driver, DataRep.PasswordExp, NewPassword);

            _driver.SearchElement(_onfirmPasswordExp)
               .SendsKeysAuto(_driver, _onfirmPasswordExp, NewPassword);

            ClickOnLoginButtonForChangePassword(currentUrl);

            return this;
        }

        public ILoginPageUi ClickOnLoginButton()
        {
            var currentUrl = _driver.Url;

            _driver.SearchElement(LoginBtnExp, 60)
                .ForceClick(_driver, LoginBtnExp);
           
            return this;
        }

        public ILoginPageUi ClickOnLoginButtonForChangePassword(string currentUrl)
        {
            _driver.SearchElement(LoginBtnExp)
                .ForceClick(_driver, LoginBtnExp);

            for (var i = 0; i < 5; i++)
            {
                if (_driver.Url == currentUrl)
                {
                    Thread.Sleep(500);
                }
                else
                {
                    break;
                }
            }

            return this;
        }

        private ILoginPageUi ClickOnOkButton()
        {
            _driver.SearchElement(OkBtnExp)
                .ForceClick(_driver, OkBtnExp);

            return this;
        }

        public ILoginPageUi DeleteAllCookies()
        {
            _driver.Manage().Cookies.DeleteAllCookies();

            return this;
        }

        public ILoginPageUi ClickOnDeclineButton()
        {
            _driver.SearchElement(DeclineBtnExp)
                .ForceClick(_driver, DeclineBtnExp);

            return this;
        }

        public ILoginPageUi ClickOnApproveButton()
        {
            _driver.SearchElement(ApproveBtnExp)
                .ForceClick(_driver, ApproveBtnExp);

            return this;
        }

        public ILoginPageUi ClickOnSignatureField()
        {
            _driver.SearchElement(OpenSignaturePadExp)
                .ForceClick(_driver, OpenSignaturePadExp);

            return this;
        }

        public ILoginPageUi LoginPipe(string userName,
            string url = null, string password = null,
            bool deleteCookies = false, bool needRefresh = false, 
            string apiKey = null)
        {
            if (deleteCookies)
            {
                _driver.Manage().Cookies.DeleteAllCookies();
                _driver.Navigate().Refresh();
            }

            if (url != null)
            {
                NavigateToUrl(url);
            }

            SetUserName(userName);
            SetPassword(password);
            ClickOnLoginButton();

            if (needRefresh)
            {
                Thread.Sleep(1000);
                _driver.Navigate().Refresh();
            }

            return this;
        }

        public ILoginPageUi ClickOnLogOut()
        {
            _driver.SearchElement(LogOutBtnExp)
                .ForceClick(_driver, LogOutBtnExp);

            return this;
        }

        public ILoginPageUi WaitForUrlToChange(string expectedUrl)
        {
            var actualUrl = _driver.Url;

            for (var i = 0; i < 6; i++)
            {
                if (actualUrl != expectedUrl)
                {
                    Thread.Sleep(200);
                    actualUrl = _driver.Url;
                }

            }

            return this;
        }

        public T ChangeContext<T>(IWebDriver driver) where T : class
        {
            return _apiFactory.ChangeContext<T>(_driver);
        }
    }
}
