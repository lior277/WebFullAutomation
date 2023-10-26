using System.Collections.Generic;
using System.Threading;
using AirSoftAutomationFramework.Internals.ExtensionsMethods;
using AirSoftAutomationFramework.Internals.Factory;
using AirSoftAutomationFramework.Internals.Helpers;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.Settings;
using OpenQA.Selenium;

namespace AirSoftAutomationFramework.Objects.ForexObjects.Ui.Crm.Accounts.UsersMenuUi
{
    public class UserUi : IUserUi
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
        private string _setApiKey;
        //private string[] _subUsers = null;
        private string _gmtTimezone;
        private bool _active;
        private bool _addAsChild;
        private string _extention;
        private string _office;
        public IWebDriver _driver;
        private readonly IApplicationFactory _apiFactory;
        private string _mailPerfix = DataRep.EmailPrefix;
        private string _password = DataRep.Password;
        #endregion Members's

        public UserUi(IApplicationFactory apiFactory, IWebDriver driver)
        {
            _apiFactory = apiFactory;
            _driver = driver;
        }

        #region Locator's
        private readonly By OpenMultiselectSubUsersExp = By.CssSelector("div[class='selected-list']");
        private readonly By SearchSubUsersExp = By.CssSelector("div[class='list-filter ng-star-inserted'] input[class*='c-input']");
        private readonly By SelectUserExp = By.CssSelector("li[class='pure-checkbox ng-star-inserted']");
        private readonly By FirstNameExp = By.CssSelector("input[class*='user-first-name']");
        public static By InvalidPolicyExp = By.CssSelector("i[class*='check-green']");
        private readonly By LastNameExp = By.CssSelector("input[class*='user-last-name']");
        private readonly By UserNameExp = By.CssSelector("input[class*='user-user-name']");
        private readonly By EmailExp = By.CssSelector("input[class*='user-email']");
        private readonly By PhoneExp = By.CssSelector("input[class*='user-phone']");
        private readonly By ExtensionExp = By.CssSelector("input[class*='user-extension']");
        private readonly By ActivecheckboxExp = By.CssSelector("label[class*='user-active']");
        private readonly By AddAsChildcheckboxExp = By.CssSelector("label[class*='user-add-as-child']");
        private readonly By OfficeExp = By.XPath("//select[contains(@class,'user-offices')]");
        private readonly By AllowedIpAddressExp = DataRep.AllowedIpAddressExp;
        private readonly By CountryExp = By.XPath("//select[contains(@class,'user-country')]");
        private readonly By GmtTimeZoneExp = By.XPath("//select[contains(@class,'user-server-gmt-time-zone')]");
        private readonly By RoleExp = By.XPath("//select[contains(@class,'user-role')]");
        private readonly By GeneratedApiKeyBodyPopupExp = By.CssSelector("div[class='model in content-modal-dialog action-alert']"); //By.CssSelector("div[class*='list-error-service text-error-service']");
        private readonly By GenerateBtnExp = By.CssSelector("button[class*='btn btn-success custom-success confirm-action-btn']");
        private readonly By GenerateApiKeyAlertBtnExp = By.CssSelector("div[class='model in content-modal-dialog action-alert']");      
        private readonly By AccountTypeExp = By.XPath("//select[contains(@class,'user-account-type')]");
        private readonly By ClickOnCloseUserCardBtnExp = By.CssSelector("div[class*='edit-user-modal'] button[class='close pull-right']");
        private readonly By SalaryTypeExp = By.XPath("//select[contains(@class,'user-salary-type')]");
        private readonly By PasswordExp = By.CssSelector("input[class*='user-password']");
        private readonly By ApiKeyExp = By.CssSelector("input[class*='api-key']");
        public static By InvalidPasswordExp = By.CssSelector("input[class*='invalid'][class*='create-user-password']");
        private readonly By SendApiKeyButtonExp = By.CssSelector("input[class*='edit-user-send-btn']");
        private readonly By CopyApiKeyButtonExp = By.CssSelector("input[class*='edit-user-copy-btn']");
        private readonly By SaveButtonExp = By.CssSelector("button[class*='user-save-btn']");
        private readonly By SaveAndCreateAnotherUserExp = By.CssSelector("button[class*='user-save-and-create-another-user-btn']");
        private readonly By SendApiKeyExp = By.CssSelector("button[class='btn btn-primary send-api-key-btn']");
        private readonly By CopyApiKeyExp = By.CssSelector("button[class*='copy-api-key-btn']");
        #endregion Locator's

        public IUserUi SetFirstName(string firstName = null)
        {
            _firstName = firstName ?? _userName;
            // _driver.WaitForAnimationToLoad(400);
            _driver.SearchElement(FirstNameExp)
                .SendsKeysAuto(_driver, FirstNameExp, _firstName, 60);

            return this;
        }

        public IUserUi SetLastName(string lastName = null)
        {
            _lastName = lastName ?? _userName;
            // _driver.WaitForAnimationToLoad(400);
            _driver.SearchElement(LastNameExp)
                .SendsKeysAuto(_driver, LastNameExp, _lastName);

            return this;
        }

        public IUserUi SetUserName(string userName)
        {
            _userName = userName;
            _email = $"{_userName}{_mailPerfix}";

            _driver.SearchElement(UserNameExp)
                .SendsKeysAuto(_driver, UserNameExp, _userName);

            return this;
        }

        public IUserUi SetEmail(string email = null)
        {
            _email = email ?? _email;

            _driver.SearchElement(EmailExp)
                .SendsKeysAuto(_driver, EmailExp, _email);

            return this;
        }

        //public IUserUi SetSubUsers(string[] subUsers = null)
        //{
        //    _subUsers = subUsers ?? _subUsers;
        //    _driver.SelectElementFromMultiSelectDropDown(OpenMultiselectSubUsersExp, SearchSubUsersExp, SelectUserExp, _subUsers);

        //    return this;
        //}

        public IUserUi SetPhone(string phone = null)
        {
            _phone = phone ?? DataRep.UserDefaultPhone;
            _driver.SearchElement(PhoneExp)
                .SendsKeysAuto(_driver, PhoneExp, _phone);

            return this;
        }

        public IUserUi SetExtention(string extention = null)
        {
            _extention = extention ?? "8885445";
            _driver.SearchElement(ExtensionExp)
                .SendsKeysAuto(_driver, ExtensionExp, _extention);

            return this;
        }

        public IUserUi SetYourStatus(bool? active = null, bool? addAsChild = null)
        {
            _active = active ?? true;
            _addAsChild = addAsChild ?? true;

            SetActive(_active);
            AddAsChild(_addAsChild);

            return this;
        }

        public IUserUi SetActive(bool active)
        {
            // TBD selenium select

            return this;
        }

        public IUserUi AddAsChild(bool addAsChild)
        {
            // TBD selenium select

            return this;
        }

        public IUserUi SetOffice(string office = null)
        {
            _office = office ?? _office;
            _driver.SearchElement(OfficeExp)
                .SelectElementFromDropDownByText(_driver, OfficeExp, _office);

            return this;
        }

        public int CheckPasswordPolicy()
        {
            var elementText = new List<string>();
            _driver.SearchElement(DataRep.PasswordExp).Click();

            return _driver.WaitForAtListNumberOfElements(InvalidPolicyExp, 4).Count;
        }

        public Dictionary<string, bool> CheckIfPasswordValid(List<string> passwords)
        {
            bool passwordValidation;
            var results = new Dictionary<string, bool>();
            SetPassword("ff");

            foreach (var password in passwords)
            {
                Thread.Sleep(30);
                //SetPassword(password);
                SetPassword(password);
                passwordValidation = _driver.SearchElement(InvalidPasswordExp).Displayed;
                results.Add(password, passwordValidation);
            }

            return results;
        }

        public IUserUi SetAllowedIpAddresses(string[] allowedIpAddresses = null)
        {
            if (allowedIpAddresses != null)
            {
                foreach (var item in allowedIpAddresses)
                {
                    _driver.SearchElement(AllowedIpAddressExp)
                        .SendKeys(item);
                }
            }

            return this;
        }

        public IUserUi SetCountry(string country = null)
        {
            _country = country ?? "Afghanistan";
            _driver.SearchElement(CountryExp)
                .SelectElementFromDropDownByText(_driver, CountryExp, _country);

            return this;
        }

        public IUserUi SetGmtTimezone(string gmtTimezone = null)
        {
            _gmtTimezone = gmtTimezone ?? _gmtTimezone;
            _driver.SearchElement(GmtTimeZoneExp)
                .SelectElementFromDropDownByValue(_driver, _gmtTimezone);

            return this;
        }

        public IUserUi SetRole(string role = null)
        {
            _role = role ?? "admin";
            _driver.SearchElement(RoleExp)
                .SelectElementFromDropDownByText(_driver, RoleExp, _role);

            return this;
        }

        public IUserUi SetAccountType(string accountType = null)
        {
            _accountType = accountType ?? "Retention";
            _driver.SearchElement(AccountTypeExp)
                .SelectElementFromDropDownByText(_driver, AccountTypeExp, _accountType);

            return this;
        }

        public IUserUi SetSalaryType(string salaryType = null)
        {
            _salaryType = salaryType ?? _salaryType;
            _driver.SearchElement(SalaryTypeExp)
                .SelectElementFromDropDownByText(_driver, SalaryTypeExp, _salaryType);

            return this;
        }

        public IUserUi SetPassword(string password = null)
        {
            _password = password ?? _password;
            _driver.SearchElement(DataRep.PasswordExp)
                .SendsKeysAuto(_driver, DataRep.PasswordExp, _password);

            return this;
        }

        public IUserUi SetApiKey(string setApiKey = null)
        {
            _setApiKey = setApiKey ?? _setApiKey;
            _driver.SearchElement(ApiKeyExp)
                .SendsKeysAuto(_driver, ApiKeyExp, _setApiKey);

            return this;
        }

        public IUserUi SendApiKey(string setApiKey = null)
        {
            // need
            _setApiKey = setApiKey ?? _setApiKey;
            _driver.SearchElement(ApiKeyExp)
                .SendsKeysAuto(_driver, ApiKeyExp, _password);

            return this;
        }

        public IUsersPageUi ClickOnSaveButton()
        {
            _driver.SearchElement(SaveButtonExp)
                .ForceClick(_driver, SaveButtonExp);

            return _apiFactory.ChangeContext<IUsersPageUi>(_driver);
        }

        public string GetGeneratedApiKeyBodyPopup()
        {
            var text = _driver.SearchElement(GeneratedApiKeyBodyPopupExp)
                 .GetElementText(_driver, GeneratedApiKeyBodyPopupExp);

            return text;
        }

        public string GetApiKey()
        {
            Thread.Sleep(400); // wait for the creation of the new apikey

            return _driver.SearchElement(ApiKeyExp)
                .GetElementText(_driver, ApiKeyExp);
        }

        public IUserUi ClickOnSendApiKeyBtn()
        {
            //_driver.WaitForAnimationToLoad(600);

            _driver.SearchElement(SendApiKeyExp)
                .ForceClick(_driver, SendApiKeyExp);

            return this;
        }

        public IUserUi ClickOnGenerateButton()
        {
            _driver.SearchElement(GenerateBtnExp)
                .ClickAndWaitForElementNotExist(_driver);

            return this;
        }

        public IUsersPageUi ClickOnCloseUserCardButton()
        {
            _driver.SearchElement(ClickOnCloseUserCardBtnExp)
                .ForceClick(_driver, ClickOnCloseUserCardBtnExp);

            return _apiFactory.ChangeContext<IUsersPageUi>(_driver);
        }

        public IUserUi SaveAndCreateAnotherUserButton()
        {
            _driver.SearchElement(SaveAndCreateAnotherUserExp)
                .ForceClick(_driver, SaveAndCreateAnotherUserExp);

            return this;
        }

        public IUsersPageUi CreateUserUiPipe(string url, string userName,
            string password = null)
        {
            var OfficeDetails = _apiFactory
                .ChangeContext<IOfficeTabApi>(_driver)
                .GetOfficesByName(url);

            var email = userName + _mailPerfix;
            _gmtTimezone = OfficeDetails.gmt_timezone;
            _office = OfficeDetails.city;

            SetFirstName(userName);
            SetLastName(userName);
            SetUserName(userName);
            SetEmail(email);
            //SetSubUsers();
            SetPhone();
            //SetExtention();
            //SetYourStatus();
            SetOffice();
            SetAllowedIpAddresses();
            SetGmtTimezone();
            SetCountry();
            SetRole();
            SetAccountType();
            //SetSalaryType();
            SetPassword();
            ClickOnSaveButton();

            return _apiFactory.ChangeContext<IUsersPageUi>(_driver);
        }

        public T ChangeContext<T>(IWebDriver driver) where T : class
        {
            return _apiFactory.ChangeContext<T>(_driver);
        }
    }
}
