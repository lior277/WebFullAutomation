// Ignore Spelling: Forex api Ip Ips

using AirSoftAutomationFramework.Internals.ExtensionsMethods;
using AirSoftAutomationFramework.Internals.Factorys;
using AirSoftAutomationFramework.Internals.Helpers;
using OpenQA.Selenium;

namespace AirSoftAutomationFramework.Objects.ForexObjects.Ui.Settings
{
    public class OfficesTabUi : IOfficesTabUi
    {
        private readonly IApplicationFactory _apiFactory;
        private IWebDriver _driver;
        private string[] _userAllowedIps = DataRep.UserAllowedIps;
        public OfficesTabUi(IApplicationFactory apiFactory, IWebDriver driver)
        {
            _driver = driver;
            _apiFactory = apiFactory;
        }

        #region Locator's       
        private readonly By EditOfficeButtonExp = By.CssSelector("i[class='fa fa-pencil']");
        private readonly By AllowedIpAddressExp = DataRep.AllowedIpAddressExp;
        private readonly By SaveButtonExp = By.CssSelector("div[class*='edit-office'] button[type='submit']");
        private readonly By OfficeAssignIpsButtonExp = By.CssSelector("button[class*='office-assign-ip']");
        private readonly By OverrideButtonExp = By.CssSelector("button[class*='custom-override']");
        private readonly By OfficeRowExp = By.CssSelector("table[id='officeTable'] tbody tr");
        #endregion Locator's 

        public IOfficesTabUi ClickOnEditOfficeButton()
        {
            _driver.SearchElement(EditOfficeButtonExp)
                .ForceClick(_driver, EditOfficeButtonExp);

            return this;
        }

        public IOfficesTabUi SetAllowedIpAddresses()
        {

            foreach (var ip in _userAllowedIps)
            {
                _driver.SearchElement(AllowedIpAddressExp)
                    .SendKeys(ip);
            }

            return this;
        }

        public IOfficesTabUi ClickOnSaveButton()
        {
            _driver.SearchElement(SaveButtonExp)
                .ForceClick(_driver, SaveButtonExp);

            _driver.SearchElement(DataRep.ConfirmExp)
                .ForceClickWithRetry(_driver, DataRep.ConfirmExp);

            return this;
        }

        public IOfficesTabUi ClickOnAssignIpsButton()
        {
            _driver.SearchElement(OfficeAssignIpsButtonExp)
                .ForceClick(_driver, OfficeAssignIpsButtonExp);

            return this;
        }

        public IOfficesTabUi ClickOnOverrideButton()
        {
            _driver.SearchElement(OverrideButtonExp)
                .ForceClick(_driver, OverrideButtonExp);

            return this;
        }

        private int CountOffices()
        {
            return _driver.SearchElements(OfficeRowExp).Count;
        }

        public IOfficesTabUi AssignIpsPipe()
        {
            var officesCount = CountOffices();

            for (var i = 0; i < officesCount; i++)
            {
                ClickOnEditOfficeButton();
                SetAllowedIpAddresses();
                ClickOnSaveButton();
                ClickOnAssignIpsButton();
                ClickOnOverrideButton();
            }

            return this;
        }

        public T ChangeContext<T>(IWebDriver driver) where T : class
        {
            return _apiFactory.ChangeContext<T>(_driver);
        }

    }
}
