using AirSoftAutomationFramework.Internals.ExtensionsMethods;
using AirSoftAutomationFramework.Internals.Factorys;
using OpenQA.Selenium;

namespace AirSoftAutomationFramework.Objects.ForexObjects.TradingPlatform.TradeObjects.Ui
{
    public class TopPanelUi : ApplicationFactory
    {
        public IWebDriver _driver;

        public TopPanelUi(IWebDriver driver)
        {
            _driver = driver;
        }

        #region Locator's
        private readonly By AvailableExp = By.XPath("//span[@class='balance_num']");
        private readonly By ClientsMeueItemExp = By.XPath("//a[@uisref='crm.clients']");
        private readonly By ClientTitleExp = By.XPath("//h1[@class='heading attribution-head attribution-title']");
        private readonly By PasswordTextBoxExp = By.XPath("//input[@name='password']");
        private readonly By LoginhButtonExp = By.XPath("//button[contains(@class,'new-user')]");
        #endregion Locator's

        public string GetAvailable()
        {
            var amount = _driver.SearchElement(AvailableExp).Text;
            return _driver.SearchElement(AvailableExp).Text;
        }
    }
}
