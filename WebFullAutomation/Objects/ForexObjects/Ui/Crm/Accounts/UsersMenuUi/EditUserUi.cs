using AirSoftAutomationFramework.CrmObjects.Ui;
using AirSoftAutomationFramework.CRMObjects.Accounts;
using AirSoftAutomationFramework.Internals;
using AirSoftAutomationFramework.Internals.DAL.ConfigurationManagement;
using AirSoftAutomationFramework.Internals.Extensions;
using OpenQA.Selenium;

namespace AirSoftAutomationFramework.Objects.SharedObjectsForCbdAndForex.Ui.Accounts.UsersPage
{
    public class EditUserUi : ApplicationFactory, IEditUserUi
    {
        #region Members's
        private string _salaryType;
        private string _accountType;
        private string _email;
        private string _firstName;
        private string _lastName;
        private string _phone;
        private string _userName;
        private string _country;
        private string _role;
        private string[] _allowedIpAddresses;
        private string _subUsers;
        private string _gmtTimezone;
        private bool _active;
        private string _extention;
        private string _office;
        private IWebDriver _driver;
        private string _setApiKey;
        private string _mailPerfix = Configuration.GetValue("mailPerfix");
        private string _apiKey = Configuration.GetValue("apiKey");
        private string _password = Configuration.GetValue("password");
        private string _mongodbconnection = Configuration.GetValue("mongodbconnection");
        private string _dbname = Configuration.GetValue("dbname");
        private string _tableName = Configuration.GetValue("tableName");
        #endregion Members's

        public EditUserUi(IWebDriver driver)
        {
            _driver = driver;
        }

        #region Locator's
        private readonly By FirstNameExp = By.CssSelector("input[class*='edit-user-first-name']");
        private readonly By LastNameExp = By.CssSelector("input[class*='user-last-name']");
        private readonly By UserNameExp = By.CssSelector("input[class*='user-user-name']");
        private readonly By EmailExp = By.CssSelector("input[class*='user-email']");
        private readonly By SubUsersClickExp = By.CssSelector("//div[@class='c-btn form-control']"); // need check
        private readonly By SubUsersSearchExp = By.CssSelector("input[class*='user-search-sub-users']");
        private readonly By PhoneExp = By.CssSelector("input[class*='user-phone']");
        private readonly By ExtensionExp = By.CssSelector("input[class*='user-extension']");
        private readonly By ActivecheckboxExp = By.CssSelector("label[class*='user-active']");      
        private readonly By OfficeExp = By.CssSelector("select[class*='user-offices']");
        private readonly By IpAddressExp = By.CssSelector("input[class*='user-allowed-ip-addresses']");
        private readonly By CountryExp = By.CssSelector("select[class*='user-country']");
        private readonly By GmtTimeZoneExp = By.CssSelector("select[class*='user-offices']");
        private readonly By RoleExp = By.CssSelector("select[class*='user-role']");
        private readonly By AccountTypeExp = By.CssSelector("select[class*='user-account-type']");
        private readonly By SalaryTypeExp = By.CssSelector("select[class*='user-salary-type']");
        private readonly By ApiKeyExp = By.CssSelector("input[class*='edit-user-api-key']");
        private readonly By SendApiKeyButtonExp = By.CssSelector("input[class*='edit-user-send-btn']");
        private readonly By CopyApiKeyButtonExp = By.CssSelector("input[class*='edit-user-copy-btn']");
        private readonly By SaveButtonExp = By.CssSelector("button[class*='user-save-btn']");
        private readonly By SaveAndCreateAnotherUserExp = By.CssSelector("button[class*='user-create-another-btn']");
        #endregion Locator's

        public IEditUserUi SetFirstName(string firstName = null)
        {
            _firstName = firstName ?? _userName;
            _driver.GetElement(FirstNameExp).ForceSendsKeysWithClear(_driver, FirstNameExp, _firstName);

            return this;
        }

        public IEditUserUi SetLastName(string lastName = null)
        {
            _lastName = lastName ?? _userName;
            _driver.GetElement(LastNameExp).ForceSendsKeysWithClear(_driver, LastNameExp, _lastName);

            return this;
        }

        public IEditUserUi SetUserName(string userName)
        {
            _userName = userName;
            _email = $"{_userName}{_mailPerfix}";
            _driver.GetElement(UserNameExp).ForceSendsKeysWithClear(_driver, UserNameExp, _email);

            return this;
        }

        public IEditUserUi SetEmail(string email = null)
        {
            _email = email ?? _email;
            _driver.GetElement(EmailExp).ForceSendsKeysWithClear(_driver, EmailExp, _email);

            return this;
        }

        public IEditUserUi SetSubUsers(string subUsers = null)
        {
            _subUsers = subUsers ?? _subUsers;

            return this;
        }

        public IEditUserUi SetPhone(string phone = null)
        {
            _phone = phone ?? "888";
            _driver.GetElement(PhoneExp).ForceSendsKeysWithClear(_driver, PhoneExp, _phone);

            return this;
        }

        public IEditUserUi SetExtention(string extention = null)
        {
            _extention = extention ?? "Extention";
            _driver.GetElement(ExtensionExp).ForceSendsKeysWithClear(_driver, ExtensionExp, _extention);

            return this;
        }

        public IEditUserUi SetActive(bool active)
        {
            // TBD selenium select

            return this;
        }

        public IEditUserUi SetOffice(string office = null)
        {
            _office = office ?? "Airsoft";
            _driver.GetElement(OfficeExp).SelectElementFromdropDown(_driver, OfficeExp, _office);

            return this;
        }

        public IEditUserUi SetAllowedIpAddresses(string[] allowedIpAddresses = null)
        {
            _allowedIpAddresses = allowedIpAddresses ?? _allowedIpAddresses;

            return this;
        }

        public IEditUserUi SetCountry(string country = null)
        {
            _country = country ?? "Afghanistan";
            _driver.GetElement(CountryExp).SelectElementFromdropDown(_driver, CountryExp, _country);

            return this;
        }

        public IEditUserUi SetGmtTimezone(string gmtTimezone = null)
        {
            _gmtTimezone = gmtTimezone ?? "04:30";
            _driver.GetElement(GmtTimeZoneExp).SelectElementFromdropDown(_driver, GmtTimeZoneExp, _gmtTimezone);

            return this;
        }

        public IEditUserUi SetRole(string role = null)
        {
            _role = role ?? "admin";
            _driver.GetElement(RoleExp).SelectElementFromdropDown(_driver, RoleExp, _role);

            return this;
        }

        public IEditUserUi SetAccountType(string accountType = null)
        {
            _accountType = accountType ?? _accountType;
            _driver.GetElement(AccountTypeExp).SelectElementFromdropDown(_driver, AccountTypeExp, _accountType);

            return this;
        }

        public IEditUserUi SetSalaryType(string salaryType = null)
        {
            _salaryType = salaryType ?? _salaryType;
            _driver.GetElement(SalaryTypeExp).SelectElementFromdropDown(_driver, SalaryTypeExp, _salaryType);

            return this;
        }

        public IEditUserUi SetApiKey(string setApiKey = null)
        {
            _setApiKey = setApiKey ?? _setApiKey;
            _driver.GetElement(ApiKeyExp).ForceSendsKeysWithClear(_driver, ApiKeyExp, _password);

            return this;
        }

        public IUCliensUi ClickOnSaveButton()
        {
            _driver.GetElement(SaveButtonExp).ForceClick(_driver, SaveButtonExp);

            return ChangeContext<IUCliensUi>(_driver);
        }

        public ICreateUserUi SaveAndCreateAnotherUserButton()
        {
            _driver.GetElement(SaveAndCreateAnotherUserExp).ForceClick(_driver, SaveAndCreateAnotherUserExp);

            return ChangeContext<ICreateUserUi>(_driver);
        }

        public string GetEmail()
        {
            var email = _driver.GetElement(EmailExp).ForceGetAttribute(_driver, EmailExp, "value");

            return email;
        }
    }
}
