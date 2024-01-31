// Ignore Spelling: Forex api

using AirSoftAutomationFramework.Internals.ExtensionsMethods;
using AirSoftAutomationFramework.Internals.Factorys;
using OpenQA.Selenium;

namespace AirSoftAutomationFramework.Objects.ForexObjects.Ui.Settings
{
    public class GeneralTabUi : IGeneralTabUi
    {
        private readonly IApplicationFactory _apiFactory;
        private IWebDriver _driver;
        public GeneralTabUi(IApplicationFactory apiFactory, IWebDriver driver)
        {
            _driver = driver;
            _apiFactory = apiFactory;
        }

        #region Locator's     
        private readonly By ButtonsOfLeadsStatusForNewExp = By.XPath("//td[text()='New']/parent::tr[not(button)]");
        #endregion Locator's 

        public int GetNumOfButtonsOfLeadsStatusForNew()
        {
            return _driver.WaitForExactNumberOfElements(ButtonsOfLeadsStatusForNewExp, 1, 60);
        }

        public T ChangeContext<T>(IWebDriver driver) where T : class
        {
            return _apiFactory.ChangeContext<T>(_driver);
        }

    }
}
