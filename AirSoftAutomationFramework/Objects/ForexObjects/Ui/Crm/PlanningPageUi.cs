using AirSoftAutomationFramework.Internals.ExtensionsMethods;
using AirSoftAutomationFramework.Internals.Factory;
using OpenQA.Selenium;

namespace AirSoftAutomationFramework.Objects.ForexObjects.Ui.Crm
{
    public class PlanningPageUi : IPlanningPageUi
    {
        #region Members
        private IWebDriver _driver;
        private readonly IApplicationFactory _apiFactory;
        private string _commentTitle = "div[title = ' {0}']";
        #endregion Members

        public PlanningPageUi(IApplicationFactory apiFactory, IWebDriver driver)
        {
            _apiFactory = apiFactory;
            _driver = driver;
        }

        #region Locator's   
        private readonly By AgentFilterExp = By.CssSelector("div[class='cuppa-dropdown']");
        private readonly By SearchAgentExp = By.CssSelector("input[class*='c-input']");
        private readonly By SearchResultExp = By.CssSelector("li[class*='pure-checkbox'] label");
        private readonly By CommentExp = By.CssSelector("div[title=' Automation Comment']");
        #endregion

        public IPlanningPageUi ClickToOpenAgentFilter()
        {
            _driver.SearchElement(AgentFilterExp)
                .ForceClick(_driver, AgentFilterExp);

            return this;
        }

        public IPlanningPageUi SearchAgent(string userName)
        {
            _driver.SearchElement(SearchAgentExp)
                .SendsKeysAuto(_driver, SearchAgentExp, userName);

            return this;
        }

        public IPlanningPageUi ClickOnSearchResult()
        {
            _driver.SearchElement(SearchResultExp)
                .ForceClick(_driver, SearchResultExp);

            return this;
        }

        public IPlanningPageUi SelectAgentPipe(string userName)
        {
            ClickToOpenAgentFilter();
            SearchAgent(userName);
            ClickOnSearchResult();

            return this;
        }

        public int SearchComment(string commentTitle)
        {
            var commentExp = By.CssSelector(string
                .Format(_commentTitle, commentTitle));

            return _driver.WaitForExactNumberOfElements(commentExp, 1);
        }

        public T ChangeContext<T>(IWebDriver driver) where T : class
        {
            return _apiFactory.ChangeContext<T>(_driver);
        }
    }
}
