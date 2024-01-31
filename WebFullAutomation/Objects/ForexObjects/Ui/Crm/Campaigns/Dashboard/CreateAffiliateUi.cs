using AirSoftAutomationFramework.Internals.ExtensionsMethods;
using AirSoftAutomationFramework.Internals.Factorys;
using AirSoftAutomationFramework.Internals.Helpers;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.Settings;
using OpenQA.Selenium;

namespace AirSoftAutomationFramework.Objects.ForexObjects.Ui.Crm.Campaigns.Dashboard
{
    public class CreateAffiliateUi : ICreateAffiliateUi
    {
        #region Members
        public IWebDriver _driver;
        private static string _affiliateName;
        private string _firstName;
        private string _lastName;
        private string _userName;
        private string _email;
        private string _phone;
        private string _office;
        private string _ipAddresses;
        private string _role;
        private string _affiliateManager;
        private string _password = DataRep.Password;
        private readonly IApplicationFactory _apiFactory;
        private string _mailPerfix = DataRep.EmailPrefix;
        #endregion Members

        public CreateAffiliateUi(IApplicationFactory apiFactory, IWebDriver driver)
        {
            _apiFactory = apiFactory;
            _driver = driver;
        }

        #region Locator's  
        private readonly By AffiliateNameExp = By.XPath("//select[contains(@class,'affiliate-name')]");
        private readonly By FirstNameExp = By.CssSelector("input[class*='first-name']");
        private readonly By LastNameExp = By.CssSelector("input[class*='last-name']");
        private readonly By UserNameExp = By.CssSelector("input[class*='user-name']");
        private readonly By EmailExp = By.CssSelector("input[class*='email']");
        private readonly By PhoneExp = By.CssSelector("input[class*='phone']");
        private readonly By OfficeExp = By.XPath("//select[contains(@class,'campaign-office')]");
        private readonly By AllowedIpAddressExp = By.CssSelector("div[class*='allowed-ip-addresses'] input");
        private readonly By RoleExp = By.XPath("//select[contains(@class,'role')]");
        private readonly By AffiliateManagerExp = By.XPath("//select[contains(@class,'affiliate-manager')]");
        private readonly By PasswordExp = By.CssSelector("input[class*='password']");
        #endregion Locator's

        public ICreateAffiliateUi SetAffiliateName(string affiliateName = null)
        {
            _affiliateName = affiliateName ?? "New";

            _driver.SearchElement(AffiliateNameExp)
                .SelectElementFromDropDownByText(_driver, AffiliateNameExp, _affiliateName, 400);

            return this;
        }

        public ICreateAffiliateUi SetFirstName(string firstName = null)
        {
            _firstName = firstName ?? _userName;

            _driver.SearchElement(DataRep.FirstNameExp, 500)
                .SendsKeysAuto(_driver, DataRep.FirstNameExp, _firstName);

            return this;
        }

        public ICreateAffiliateUi SetLastName(string lastName = null)
        {
            _lastName = lastName ?? _userName;

            _driver.SearchElement(DataRep.LastNameExp)
                .SendsKeysAuto(_driver, DataRep.LastNameExp, _lastName);

            return this;
        }

        public ICreateAffiliateUi SetUserName(string userName)
        {
            _userName = userName;
            _email = $"{_userName}{_mailPerfix}";
            _driver.SearchElement(UserNameExp)
                .SendsKeysAuto(_driver, UserNameExp, _userName);

            return this;
        }

        public ICreateAffiliateUi SetEmail(string email = null)
        {
            _email = email ?? _email;

            _driver.SearchElement(DataRep.EmailExp)
                .SendsKeysAuto(_driver, DataRep.EmailExp, _email);

            return this;
        }

        public ICreateAffiliateUi SetPhone(string phone = null)
        {
            _phone = phone ?? DataRep.UserDefaultPhone;

            _driver.SearchElement(DataRep.PhoneExp, 150)
                .SendsKeysAuto(_driver, DataRep.PhoneExp, _phone);

            return this;
        }

        public ICreateAffiliateUi SetOffice(string office = null)
        {
            _office = office ?? _office;

            _driver.SearchElement(OfficeExp)
                .SelectElementFromDropDownByText(_driver, OfficeExp, _office);

            return this;
        }

        public ICreateAffiliateUi SetIpAddresses(string ipAddresses = null)
        {
            _ipAddresses = ipAddresses ?? "demo";

            _driver.SearchElement(AllowedIpAddressExp)
                .SendsKeysAuto(_driver, AllowedIpAddressExp, _ipAddresses);

            return this;
        }

        public ICreateAffiliateUi SetRole(string role = null)
        {
            _role = role ?? "affiliate";

            _driver.SearchElement(RoleExp)
                .SelectElementFromDropDownByText(_driver, RoleExp, _role);

            return this;
        }

        public ICreateAffiliateUi SetAffiliateManager(string affiliateManager)
        {
            _affiliateManager = affiliateManager ?? "admin";

            _driver.SearchElement(AffiliateManagerExp)
                .SelectElementFromDropDownByText(_driver, AffiliateManagerExp, _affiliateManager);

            return this;
        }

        public ICreateAffiliateUi SetPassword(string password = null)
        {
            _password = password ?? _password;

            _driver.SearchElement(DataRep.PasswordExp)
                .SendsKeysAuto(_driver, DataRep.PasswordExp, _password);

            return this;
        }

        public ICampaignsPageUi ClickOnSaveButton()
        {
            _driver.SearchElement(DataRep.SaveExp)
                .ForceClick(_driver, DataRep.SaveExp);

            return _apiFactory.ChangeContext<ICampaignsPageUi>(_driver);
        }

        public ICampaignsPageUi CreateAffiliateUiPipe(string url, string userName)
        {
            _firstName = userName;
            var email = userName + _mailPerfix;

            var OfficeDetails = _apiFactory
                .ChangeContext<IOfficeTabApi>(_driver)
                .GetOfficesByName(url);

            _office = OfficeDetails.city;

            SetAffiliateName();
            SetFirstName(_firstName);
            SetLastName(_firstName);
            SetUserName(userName);
            SetEmail(email);
            SetPhone();
            SetOffice();
            SetRole();
            SetPassword();

            return ClickOnSaveButton();
        }

        public ICampaignsPageUi CreateCbdAffiliateUiPipe(string url, string userName)
        {
            _firstName = userName;
            var email = userName + _mailPerfix;

            var OfficeDetails = _apiFactory
                .ChangeContext<IOfficeTabApi>(_driver)
                .GetOfficesByName(url);

            _office = OfficeDetails.city;

            SetFirstName(_firstName);
            SetLastName(_firstName);
            SetUserName(userName);
            SetEmail(email);
            SetPhone();
            SetOffice();
            SetRole();
            SetPassword();

            return ClickOnSaveButton();
        }

        public T ChangeContext<T>(IWebDriver driver) where T : class
        {
            return _apiFactory.ChangeContext<T>(_driver);
        }
    }
}
