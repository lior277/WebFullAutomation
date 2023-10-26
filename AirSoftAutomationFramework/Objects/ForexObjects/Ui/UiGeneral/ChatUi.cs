// Ignore Spelling: Forex api

using AirSoftAutomationFramework.Internals.ExtensionsMethods;
using AirSoftAutomationFramework.Internals.Factory;
using OpenQA.Selenium;

namespace AirSoftAutomationFramework.Objects.ForexObjects.Ui.UiGeneral
{
    public class ChatUi : IChatUi
    {
        #region Members
        public IWebDriver _driver;
        private readonly IApplicationFactory _apiFactory;
        private string _chatParticipant = "strong[title = '{0}']";
        private string _chatContent = "//div[contains(@class,'chat-message-received')" +
            " and contains(.,'{0}')]//div[contains(@class,'received-chat-message')]";
        #endregion Members

        public ChatUi(IApplicationFactory apiFactory, IWebDriver driver)
        {
            _apiFactory = apiFactory;
            _driver = driver;
        }

        #region Locator's
        private readonly By ChatIconExp = By.CssSelector("a[title='Chat']");
        private readonly By LoaderIconExp = By.CssSelector("div[class='loader']");

        private readonly By ChatInputExp = By
           .CssSelector("input[class*='chat-window-input']");

        private readonly By BadgeRedNotificationNumberExp = By
          .CssSelector("span[class*='badge bg-red notification-number']");

        private readonly By ChatWindowInputExp = By
            .CssSelector("input[class*='chat-window-input']");
        #endregion

        public IChatUi ClickOnChatIcon()
        {
            _driver.SearchElement(ChatIconExp)
                .ForceClick(_driver, ChatIconExp);         

            return this;
        }

        public IChatUi ClickOnChatParticipant(string name)
        {
            var chatParticipantExp = By
                .CssSelector(string.Format(_chatParticipant, name));

            _driver.SearchElement(chatParticipantExp)
                .ForceClick(_driver, chatParticipantExp);

            WaitForLoaderForNotDisplay();

            return this;
        }

        public IChatUi WaitForLoaderForNotDisplay()
        {
            _driver.SearchElement(LoaderIconExp)
                .ClickAndWaitForElementNotExist(_driver);

            return this;
        }

        public IChatUi SetChatMessage(string chatMessage)
        {
            var element = _driver.SearchElement(ChatWindowInputExp);
            element.SendsKeysAuto(_driver, ChatWindowInputExp, chatMessage);
            element.SendKeys(Keys.Enter);

            return this;
        }

        public string GetRevivedChatMessage(string chatMessage)
        {
            var chatContentExp = By
                .XPath(string.Format(_chatContent, chatMessage));

            return _driver.SearchElement(chatContentExp)
                .GetElementText(_driver);
        }

        public IChatUi WaitForBadgeRedNotificationNumber()
        {
            _driver.SearchElement(BadgeRedNotificationNumberExp, 70);

            return this;
        }

        public T ChangeContext<T>(IWebDriver driver) where T : class
        {
            return _apiFactory.ChangeContext<T>(_driver);
        }
    }
}
