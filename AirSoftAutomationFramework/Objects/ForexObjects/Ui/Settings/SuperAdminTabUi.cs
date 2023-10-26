// Ignore Spelling: api Admin Forex


using AirSoftAutomationFramework.Internals.Factory;
using OpenQA.Selenium;

namespace AirSoftAutomationFramework.Objects.ForexObjects.Ui.Settings
{
    public class SuperAdminTabUi : ISuperAdminTabUi
    {
        private readonly IApplicationFactory _apiFactory;
        private IWebDriver _driver;
        public SuperAdminTabUi(IApplicationFactory apiFactory, IWebDriver driver)
        {
            _driver = driver;
            _apiFactory = apiFactory;
        }

        public T ChangeContext<T>(IWebDriver driver) where T : class
        {
            return _apiFactory.ChangeContext<T>(_driver);
        }

    }
}
