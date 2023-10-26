using AirSoftAutomationFramework.Internals.ExtensionsMethods;
using AirSoftAutomationFramework.Internals.Factory;
using AirSoftAutomationFramework.Objects.ForexObjects.ApiGeneral;
using AirSoftAutomationFramework.Objects.ForexObjects.Ui.UiGeneral;
using OpenQA.Selenium;

namespace AirSoftAutomationFramework.Objects.ForexObjects.Ui.Cfd
{
    public class ClosedTradesPageUi : IClosedTradesPageUi
    {
        private IWebDriver _driver;
        private readonly IApplicationFactory _apiFactory;

        public ClosedTradesPageUi(IApplicationFactory apiFactory, IWebDriver driver)
        {
            _apiFactory = apiFactory;
            _driver = driver;
        }

        #region Locator's    
        private readonly By CloseTradeTableIdColumnExp = By.XPath("//th[text()='Id']");// By.CssSelector("th[tabindex='1'][aria-label='Id: activate to sort column ascending']");
        private readonly By TooltipBtnExp = By.CssSelector("div[class='tooltip-timeline']");
        private readonly By TooltipExp = By.CssSelector("div[class='tooltip-timeline'] span[class='tooltip-timeline-text']"); private readonly By WaitForProcessingUsersExp = By.CssSelector("span[class='custom-date'][style='display: none']");
        private readonly By WaitForProcessingCloseTradeExp = By.CssSelector("div[id='openTradesTable_processing'][style='display: none;']");
        #endregion Locator's     

        public ISearchResultsUi SearchCloseTrades(string searchText)
        {
            WaitForCloseTradeTableToLoad();

            return _apiFactory
                .ChangeContext<ISharedStepsGenerator>(_driver)
                .SearchClient(searchText);
        }

        public IClosedTradesPageUi WaitForCloseTradeTableToLoad()
        {
            _apiFactory
                .ChangeContext<ISharedStepsGenerator>(_driver)
                .WaitForTableToLoad();

            return this;
        }

        public IClosedTradesPageUi CheckIfIdColumnExist()
        {
            _driver.SearchElement(CloseTradeTableIdColumnExp, 150);

            return this;
        }

        public string MoveToTooltip()
        {
            _driver.ClickAndWaitForNextElement(TooltipBtnExp, TooltipExp);

            return _driver.SearchElement(TooltipExp)
               .GetElementText(_driver);
        }

        public T ChangeContext<T>(IWebDriver driver) where T : class
        {
            return _apiFactory.ChangeContext<T>(_driver);
        }
    }
}
