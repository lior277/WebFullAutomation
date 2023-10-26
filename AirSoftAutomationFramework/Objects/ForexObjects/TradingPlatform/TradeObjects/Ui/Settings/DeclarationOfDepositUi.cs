using AirSoftAutomationFramework.Internals.ExtensionsMethods;
using AirSoftAutomationFramework.Internals.Factory;
using AirSoftAutomationFramework.Internals.Helpers;
using OpenQA.Selenium;

namespace AirSoftAutomationFramework.Objects.ForexObjects.TradingPlatform.TradeObjects.Ui.Settings
{
    public class DeclarationOfDepositUi : IDeclarationOfDepositUi
    {
        #region Members
        public IWebDriver _driver;
        private readonly IApplicationFactory _apiFactory;
        #endregion Members

        public DeclarationOfDepositUi(IApplicationFactory apiFactory, IWebDriver driver)
        {
            _driver = driver;
            _apiFactory = apiFactory;
        }

        #region Locator's   
        #endregion Locator's

        public IDeclarationOfDepositUi SetSignature()
        {
            _driver.SearchElement(DataRep.SignaturePadExp)
              .DrawSignatureOnCanvas(_driver, DataRep.SignaturePadExp);

            return this;
        }

        public IHistoryPageUi ClickOnSaveSignatureButton()
        {
            _driver.SearchElement(DataRep.SaveSignatureBtnExp)
              .ForceClick(_driver, DataRep.SaveSignatureBtnExp);

            return _apiFactory.ChangeContext<IHistoryPageUi>(_driver);
        }

        public T ChangeContext<T>(IWebDriver driver) where T : class
        {
            return _apiFactory.ChangeContext<T>(_driver);
        }
    }
}
