using AirSoftAutomationFramework.CRMObjects.Accounts;
using AirSoftAutomationFramework.Internals;
using AirSoftAutomationFramework.Internals.DAL.ConfigurationManagement;
using AirSoftAutomationFramework.Internals.Extensions;
using OpenQA.Selenium;

namespace AirSoftAutomationFramework.Objects.SharedObjectsForCbdAndForex.Ui.Accounts.UsersPage
{
    public class CreateUserUi : ApplicationFactory, ICreateUserUi
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
        private bool _addAsChild;
        private string _extention;
        private string _office;
        public IWebDriver _driver;
        private string _mailPerfix = Configuration.GetValue("mailPerfix");
        private string _apiKey = Configuration.GetValue("apiKey");
        private string _password = Configuration.GetValue("password");
        private string _mongodbconnection = Configuration.GetValue("mongodbconnection");
        private string _dbname = Configuration.GetValue("dbname");
        private string _tableName = Configuration.GetValue("tableName");
        #endregion Members's

        public CreateUserUi(IWebDriver driver)
        {
            _driver = driver;
        }

        #region Locator's
        private readonly By FirstNameExp = By.CssSelector("input[class*='user-first-name']");
        private readonly By LastNameExp = By.CssSelector("input[class*='user-last-name']");
        private readonly By UserNameExp = By.CssSelector("input[class*='user-user-name']");
        private readonly By EmailExp = By.CssSelector("input[class*='user-email']");
        private readonly By SubUsersClickExp = By.CssSelector("//div[@class='c-btn form-control']"); // need check
        private readonly By SubUsersSearchExp = By.CssSelector("input[class*='user-search-sub-users']");
        private readonly By PhoneExp = By.CssSelector("input[class*='user-phone']");
        private readonly By ExtensionExp = By.CssSelector("input[class*='user-extension']");
        private readonly By ActivecheckboxExp = By.CssSelector("label[class*='user-active']");
        private readonly By AddAsChildcheckboxExp = By.CssSelector("label[class*='user-add-as-child']");
        private readonly By OfficeExp = By.CssSelector("select[class*='user-offices']");
        private readonly By IpAddressExp = By.CssSelector("input[class*='user-allowed-ip-addresses']");
        private readonly By CountryExp = By.CssSelector("select[class*='user-country']");
        private readonly By GmtTimeZoneExp = By.CssSelector("select[class*='user-server-gmt-time-zone']");
        private readonly By RoleExp = By.CssSelector("select[class*='user-role']");
        private readonly By AccountTypeExp = By.CssSelector("select[class*='user-account-type']");
        private readonly By SalaryTypeExp = By.CssSelector("select[class*='user-salary-type']");
        private readonly By PasswordExp = By.CssSelector("input[class*='user-password']");
        private readonly By SaveButtonExp = By.CssSelector("button[class*='user-save-btn']");
        private readonly By SaveAndCreateAnotherUserExp = By.CssSelector("button[class*='user-save-and-create-another-user-btn']");
        #endregion Locator's

        public ICreateUserUi SetFirstName(string firstName = null)
        {
            _firstName = firstName ?? _userName;
            _driver.GetElement(FirstNameExp).ForceSendsKeysWithClear(_driver, FirstNameExp, _firstName);

            return this;
        }
      
        public ICreateUserUi SetLastName(string lastName = null)
        {
            _lastName = lastName ?? _userName;
            _driver.GetElement(LastNameExp).ForceSendsKeysWithClear(_driver, LastNameExp, _lastName);

            return this;
        }

        public ICreateUserUi SetUserName(string userName)
        {
            _userName = userName;
            _email = $"{_userName}{_mailPerfix}";
            _driver.GetElement(UserNameExp).ForceSendsKeysWithClear(_driver, UserNameExp, _email);

            return this;
        }

        public ICreateUserUi SetEmail(string email = null)
        {
            _email = email ?? _email;
            _driver.GetElement(EmailExp).ForceSendsKeysWithClear(_driver, EmailExp, _email);

            return this;
        }

        public ICreateUserUi SetSubUsers(string subUsers = null)
        {
            _subUsers = subUsers ?? _subUsers;

            return this;
        }

        public ICreateUserUi SetPhone(string phone = null)
        {
            _phone = phone ?? "888";
            _driver.GetElement(PhoneExp).ForceSendsKeysWithClear(_driver, PhoneExp, _phone);

            return this;
        }

        public ICreateUserUi SetExtention(string extention = null)
        {
            _extention = extention ?? "8885445";
            _driver.GetElement(ExtensionExp).ForceSendsKeysWithClear(_driver, ExtensionExp, _extention);

            return this;
        }

        public ICreateUserUi SetYourStatus(bool? active = null, bool? addAsChild = null)
        {
            _active = active ?? true;
            _addAsChild = addAsChild ?? true;

            SetActive(_active);
            AddAsChild(_addAsChild);

            return this;
        }

        public ICreateUserUi SetActive(bool active)
        {
            // TBD selenium select

            return this;
        }

        public ICreateUserUi AddAsChild(bool addAsChild)
        {
            // TBD selenium select

            return this;
        }

        public ICreateUserUi SetOffice(string office = null)
        {
            _office = office ?? "Airsoft";
            _driver.GetElement(OfficeExp).SelectElementFromdropDown(_driver, OfficeExp, _office);

            return this;
        }

        public ICreateUserUi SetAllowedIpAddresses(string[] allowedIpAddresses = null)
        {
            _allowedIpAddresses = allowedIpAddresses ?? _allowedIpAddresses;

            return this;
        }

        public ICreateUserUi SetCountry(string country = null)
        {
            _country = country ?? "Afghanistan";
            _driver.GetElement(CountryExp).SelectElementFromdropDown(_driver, CountryExp, _country);

            return this;
        }

        public ICreateUserUi SetGmtTimezone(string gmtTimezone = null)
        {
            _gmtTimezone = gmtTimezone ?? "04:30";
            _driver.GetElement(GmtTimeZoneExp).SelectElementFromdropDown(_driver, GmtTimeZoneExp, _gmtTimezone);

            return this;
        }

        public ICreateUserUi SetRole(string role = null)
        {
            _role = role ?? "admin";
            _driver.GetElement(RoleExp).SelectElementFromdropDown(_driver, RoleExp, _role);

            return this;
        }

        public ICreateUserUi SetAccountType(string accountType = null)
        {
            _accountType = accountType ?? _accountType;
            _driver.GetElement(AccountTypeExp).SelectElementFromdropDown(_driver, AccountTypeExp, _accountType);

            return this;
        }

        public ICreateUserUi SetSalaryType(string salaryType = null)
        {
            _salaryType = salaryType ?? _salaryType;
            _driver.GetElement(SalaryTypeExp).SelectElementFromdropDown(_driver, SalaryTypeExp, _salaryType);

            return this;
        }

        public ICreateUserUi SetPassword(string password = null)
        {
            _password = password ?? _password;
            _driver.GetElement(PasswordExp).ForceSendsKeysWithClear(_driver, PasswordExp, _password);

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

            return this;
        }

        public IUCliensUi CreateDefaultUserUiPipe(string userName, string password = null)
        {
            SetFirstName(userName);
            SetLastName(userName);
            SetUserName(userName);
            SetEmail();
            //SetSubUsers();
            SetPhone();
            SetExtention();
            //SetYourStatus();
            SetOffice();
            //SetAllowedIpAddresses();
            SetCountry();
            SetGmtTimezone();
            SetRole();
            //SetAccountType();
            //SetSalaryType();
            SetPassword();
            ClickOnSaveButton();

            return ChangeContext<IUCliensUi>(_driver);
        }
    }
}
