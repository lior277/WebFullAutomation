using AirSoftAutomationFramework.Internals.ExtensionsMethods;
using AirSoftAutomationFramework.Internals.Factorys;
using AirSoftAutomationFramework.Objects.ForexObjects.Ui.UiGeneral;
using OpenQA.Selenium;

namespace AirSoftAutomationFramework.Objects.ForexObjects.Ui.Cfd
{
    public class TradeGroupsUi : ITradeGroupsUi
    {
        #region Members
        public IWebDriver _driver;
        private readonly IApplicationFactory _apiFactory;
        #endregion Members

        public TradeGroupsUi(IApplicationFactory apiFactory, IWebDriver driver)
        {
            _apiFactory = apiFactory;
            _driver = driver;
        }

        #region Locator's
        private readonly By CreateCryptoGroupBtnExp = By.CssSelector(
            "button[class*='new-user']");

        private readonly By SearchCryptoGroupExp = By.CssSelector(
         "input[type='search']");

        private readonly By EditCryptoGroupBtnExp = By.CssSelector(
          "button[class*='btn-sm'] i[class*='pencil']");

        public static By SearchTradeGroupeExp = By.CssSelector("input[type='search']");
        public static By DefaultCheckboxExp = By.CssSelector("input[type='checkbox']");
        #endregion

        public ISearchResultsUi SearchTradeGroup(string searchText)
        {
            var element = _driver.SearchElement(SearchTradeGroupeExp);
            element.SendsKeysAuto(_driver, SearchTradeGroupeExp, searchText);

            return _apiFactory.ChangeContext<ISearchResultsUi>(_driver);
        }

        public ITradeGroupCardUi ClickOnEditBtn()
        {
            _driver.SearchElement(CreateCryptoGroupBtnExp)
                .ForceClick(_driver, CreateCryptoGroupBtnExp);

            return _apiFactory.ChangeContext<ITradeGroupCardUi>(_driver);
        }

        public T ChangeContext<T>(IWebDriver driver) where T : class
        {
            return _apiFactory.ChangeContext<T>(_driver);
        }
    }
}
