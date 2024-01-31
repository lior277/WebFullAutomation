using System.Collections.Generic;
using System.Linq;
using System.Threading;
using AirSoftAutomationFramework.Internals.ExtensionsMethods;
using AirSoftAutomationFramework.Internals.Factorys;
using OpenQA.Selenium;

namespace AirSoftAutomationFramework.Objects.ForexObjects.Ui.Crm.ClientsPage.ClientCard
{
    public class SavingAccountsTabUI : ISavingAccountsTabUI
    {
        private IWebDriver _driver;
        private readonly IApplicationFactory _apiFactory;

        public SavingAccountsTabUI(IApplicationFactory apiFactory, IWebDriver driver)
        {
            _driver = driver;
            _apiFactory = apiFactory;
        }

        #region Locator's
        private readonly By SavingAccountTitlesExp = By.CssSelector("span[class='trades-heading-info']");

        #endregion Locator's

        public List<string> GetSavingAccountTitlesText()
        {
            var savingAccountTitles = new List<string>();
            Thread.Sleep(1000);

            _driver.SearchElements(SavingAccountTitlesExp)
                .ForEach(p => savingAccountTitles
                .Add(p.GetElementText(_driver)));

            return savingAccountTitles;
        }

        public T ChangeContext<T>(IWebDriver driver) where T : class
        {
            return _apiFactory.ChangeContext<T>(_driver);
        }

    }
}
