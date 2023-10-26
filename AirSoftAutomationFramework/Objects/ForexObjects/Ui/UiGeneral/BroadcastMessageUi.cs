// Ignore Spelling: api Forex

using AirSoftAutomationFramework.Internals.ExtensionsMethods;
using AirSoftAutomationFramework.Internals.Factory;
using OpenQA.Selenium;

namespace AirSoftAutomationFramework.Objects.ForexObjects.Ui.UiGeneral
{
    public class BroadcastMessageUi : IBroadcastMessageUi
    {
        private IWebDriver _driver;
        private readonly IApplicationFactory _apiFactory;

        public BroadcastMessageUi(IApplicationFactory apiFactory, IWebDriver driver)
        {
            _apiFactory = apiFactory;
            _driver = driver;
        }

        #region Locator's          
        private readonly By GotItBtnExp = By.CssSelector("button[id='mgm-message-button']");
        private readonly By BroadcastBodyExp = By.CssSelector("div[id='mgm-message-body']");
        private readonly By BroadcastTitleExp = By.CssSelector("h3[id='mgmMessageModal']");
        private readonly By BroadcastBtnExp = By.CssSelector("a[id='broadcastMessage'] i[aria-hidden='true']");
        private readonly By NextBtnExp = By.CssSelector("button[class*='custom-button pull-right']");
        private readonly By BroadcasSubjectExp = By.CssSelector("input[id='subject']");
        private readonly By SelectUserExp = By.CssSelector("div[class='input-indicator user-checkbox-row']");
        private readonly By BroadcasMessageExp = By.CssSelector("div[class='note-editable']");
        private readonly By SendBtnExp = By.CssSelector("div[id='confirmation-buttons'] button[class='btn btn-success']");
        private readonly By ForwardBtnExp = By.CssSelector("span[id='forward-message']");

        private readonly By SearchUserExp = By.CssSelector(
            "div[id='broadcastUsersTable_wrapper'] input[type='search']");
        #endregion Locator's     

        public IBroadcastMessageUi ClickOnGotItButton()
        {
            _driver.SearchElement(GotItBtnExp)
                .ForceClick(_driver, GotItBtnExp);

            return this;
        }

        public IBroadcastMessageUi ClickOnBroadcastButton()
        {
            _driver.SearchElement(BroadcastBtnExp)
                .ForceClick(_driver, BroadcastBtnExp);

            return this;
        }

        public IBroadcastMessageUi SearchUser(string userName)
        {
            _driver.SearchElement(SearchUserExp, 60)
                .SendsKeysAuto(_driver, SearchUserExp, userName);

            return this;
        }

        public IBroadcastMessageUi CheckSelectedUser(string userNameToSelect)
        {
            _driver.SearchElement(SelectUserExp)
                .ForceClick(_driver, SelectUserExp);

            return this;
        }

        public IBroadcastMessageUi ClickOnNextButton()
        {
            _driver.SearchElement(NextBtnExp)
                .ForceClick(_driver, NextBtnExp);

            return this;
        }

        public IBroadcastMessageUi SetSubject(string subject)
        {
            _driver.SearchElement(BroadcasSubjectExp)
                .SendsKeysAuto(_driver, BroadcasSubjectExp, subject);

            return this;
        }

        public IBroadcastMessageUi SetMessage(string message)
        {
            _driver.SearchElement(BroadcasMessageExp)
                .SendKeys(message);

            return this;
        }

        public IBroadcastMessageUi ClickOnSendButton()
        {
            _driver.SearchElement(SendBtnExp)
                .ForceClick(_driver, SendBtnExp);

            return this;
        }

        public IBroadcastMessageUi ClickOnForewordButton()
        {
            _driver.SearchElement(ForwardBtnExp)
                .ForceClick(_driver, ForwardBtnExp);

            return this;
        }

        public IBroadcastMessageUi VerifyBroadCastTitle(string title)
        {
            _driver.SearchElement(BroadcastTitleExp)
                .GetElementText(_driver, BroadcastTitleExp)
                .StringContains(title);

            return this;
        }

        public IBroadcastMessageUi VerifyBroadCastBody(string body)
        {
            _driver.SearchElement(BroadcastBodyExp)
                .GetElementText(_driver, BroadcastBodyExp)
                .StringContains(body);

            return this;
        }

        public T ChangeContext<T>(IWebDriver driver) where T : class
        {
            return _apiFactory.ChangeContext<T>(_driver);
        }
    }
}
