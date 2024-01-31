using AirSoftAutomationFramework.Internals.ExtensionsMethods;
using AirSoftAutomationFramework.Internals.Factorys;
using OpenQA.Selenium;

namespace AirSoftAutomationFramework.Objects.ForexObjects.TradingPlatform.TradeObjects.Ui.Settings
{
    public class HistoryPageUi : IHistoryPageUi
    {
        #region Members
        private IWebDriver _driver;
        private ITradePageUi _tradePageUi;
        private readonly IApplicationFactory _apiFactory;
        #endregion Members

        public HistoryPageUi(IApplicationFactory apiFactory, IWebDriver driver, ITradePageUi tradePageUi)
        {
            _tradePageUi = tradePageUi;
            _driver = driver;
            _apiFactory = apiFactory;
        }

        #region Locator's   
        private readonly By SignDodBeforeExp = By.CssSelector("span[class*='sign-dod-no']");
        private readonly By SignDodAfterExp = By.CssSelector("span[class='sign-dod-yes']");
        private readonly By HistoryPageExp = By.CssSelector("div[class='content_body_container']");
        private readonly By ModalDodBodyExp = By.CssSelector("div[class='modal fade in show']");
        private readonly By SignaturePadExp = By.CssSelector("canvas[id='signature-pad']");
        #endregion Locator's 

        public IDeclarationOfDepositUi ClickOnSignDod()
        {
            //Thread.Sleep(1000);
            //_driver.SearchElement(HistoryPageExp);
            //_driver.SearchElement(SignDodBeforeExp)
            //    .ForceClick(_driver, SignDodBeforeExp);

           _driver.ClickAndWaitForNextElement(SignDodBeforeExp, ModalDodBodyExp);

            return _apiFactory.ChangeContext<IDeclarationOfDepositUi>(_driver);
        }

        public IHistoryPageUi VerifySignatur(string verifySignaturText = "YES")
        {
            _driver.SearchElement(SignDodAfterExp, 150)
                .GetElementText(_driver, SignDodAfterExp)
                .StringContains(verifySignaturText);

            return this;
        }

        public IHistoryPageUi VerifyApprovedSignaturAlert(string message)
        {
            _apiFactory
                .ChangeContext<ITradePageUi>(_driver)
                .VerifyMessages(message);

            return this;
        }

        public T ChangeContext<T>(IWebDriver driver) where T : class
        {
            return _apiFactory.ChangeContext<T>(_driver);
        }

    }
}
