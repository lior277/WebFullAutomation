using AirSoftAutomationFramework.Internals.ExtensionsMethods;
using AirSoftAutomationFramework.Internals.Factorys;
using AirSoftAutomationFramework.Objects.ForexObjects.ApiGeneral;
using AirSoftAutomationFramework.Objects.ForexObjects.Ui.Crm.ClientsPage.ClientCard;
using AirSoftAutomationFramework.Objects.ForexObjects.Ui.UiGeneral;
using OpenQA.Selenium;

namespace AirSoftAutomationFramework.Objects.ForexObjects.Ui.Cfd
{
    public class OpenTradesPageUi : IOpenTradesPageUi
    {
        private IWebDriver _driver;
        private readonly IApplicationFactory _apiFactory;
        private IHandleFiltersUi _handleFilters;

        public OpenTradesPageUi(IHandleFiltersUi handleFilters, IApplicationFactory apiFactory, IWebDriver driver)
        {
            _handleFilters = handleFilters;
            _apiFactory = apiFactory;
            _driver = driver;
        }

        #region Locator's    
        private readonly By OpenTradesTableIdColumnExp = By.XPath("//th[text()='Id']");// By.CssSelector("table thead" +
                                                                                       //" th[aria-label*='Id: activate to sort column ascending']:not([style*='padding-top'])");

        private readonly By CloseTradeButtonExp = By.CssSelector("button[class*='closeTrade']");
        private readonly By ClientNameExp = By.CssSelector("a[class='getClient trade-client-link']");
        private readonly By WaitForProcessingOpenTradeExp = By.CssSelector("div[id='openTradesTable_processing'][style='display: none;']");
        #endregion Locator's     

        public ISearchResultsUi SearchOpenTrades(string searchText)
        {
            WaitForOpenTradeTableToLoad();

            return _apiFactory
                .ChangeContext<ISharedStepsGenerator>(_driver)
                .SearchClient(searchText);
        }

        public IOpenTradesPageUi WaitForOpenTradeTableToLoad()
        {
            _apiFactory
                .ChangeContext<ISharedStepsGenerator>(_driver)
                .WaitForTableToLoad();

            return this;
        }

        public IOpenTradesPageUi CheckIfIdColumnExist()
        {
            _driver.SearchElement(OpenTradesTableIdColumnExp);

            return this;
        }

        public IOpenTradesPageUi VerifyCloseTradeButtonNotExist()
        {
            _driver.WaitForExactNumberOfElements(CloseTradeButtonExp, 0);

            return this;
        }

        public IClientCardUi ClickOnClientName()
        {
            _driver.SearchElement(ClientNameExp)
                .ForceClick(_driver, ClientNameExp);

            return _apiFactory.ChangeContext<IClientCardUi>(_driver);
        }

        public T ChangeContext<T>(IWebDriver driver) where T : class
        {
            return _apiFactory.ChangeContext<T>(_driver);
        }
    }
}
