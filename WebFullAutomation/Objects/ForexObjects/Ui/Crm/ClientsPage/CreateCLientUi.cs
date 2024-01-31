using AirSoftAutomationFramework.Internals.ExtensionsMethods;
using AirSoftAutomationFramework.Internals.Helpers;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.Settings;
using OpenQA.Selenium;
using System.Linq;
using AirSoftAutomationFramework.Internals.Factorys;

namespace AirSoftAutomationFramework.Objects.ForexObjects.Ui.Crm.ClientsPage
{
    public class CreateClientUi : ICreateClientUi
    {
        #region Members
        public IWebDriver _driver;
        private static string _clientName;
        //private static string _email;
        private string _country;
        private string _currency;
        private string _lastName;
        private string _phone;
        private string _gmtTimezone;
        private readonly IApplicationFactory _apiFactory;
        private string _mailPerfix = DataRep.EmailPrefix;
        #endregion Members

        public CreateClientUi(IApplicationFactory apiFactory, IWebDriver driver)
        {
            _apiFactory = apiFactory;
            _driver = driver;
        }
      
        public ICreateClientUi SetCountry(string country = null)
        {
            _country = country ?? "Afghanistan";
            _driver.SearchElement(DataRep.CountryExp)
                .SelectElementFromDropDownByText(_driver, DataRep.CountryExp, _country);

            return this;
        }

        public ICreateClientUi SetCurrency(string currency = null)
        {
            _currency = currency ?? DataRep.DefaultUSDCurrencyName;
            _driver.SearchElement(DataRep.CurrencyCodeExp)
                .SelectElementFromDropDownByText(_driver, DataRep.CurrencyCodeExp, _currency);

            return this;
        }

        public ICreateClientUi SetEmail(string email)
        {
            _driver.SearchElement(DataRep.EmailExp)
                .SendsKeysAuto(_driver, DataRep.EmailExp, email);

            return this;
        }

        public ICreateClientUi SetFirstName(string firstName)
        {
            _clientName = firstName;
            _driver.SearchElement(DataRep.FirstNameExp)
                .SendsKeysAuto(_driver, DataRep.FirstNameExp, firstName);

            return this;
        }

        public ICreateClientUi SetLastName(string lastName = null)
        {
            _lastName = lastName ?? _clientName;
            _driver.SearchElement(DataRep.LastNameExp)
                .SendsKeysAuto(_driver, DataRep.LastNameExp, lastName);

            return this;
        }

        public ICreateClientUi SetPhone(string phone = null)
        {
            _phone = phone ?? DataRep.UserDefaultPhone;

            _driver.SearchElement(DataRep.PhoneExp)
                .SendsKeysCharByChar(_driver, DataRep.PhoneExp, _phone);

            return this;
        }

        public ICreateClientUi SetGmtTimeZone(string gmtTimezone = null)
        {
            _gmtTimezone = gmtTimezone ?? "04:30";
            _driver.SearchElement(DataRep.GmtTimeZoneExp)
                .SelectElementFromDropDownByText(_driver, DataRep.GmtTimeZoneExp, _gmtTimezone);

            return this;
        }

        public IClientsPageUi ClickOnSaveButton()
        {
            _driver.SearchElement(DataRep.SaveExp)
                .ForceClick(_driver, DataRep.SaveExp);

            _driver.WaitForElementNotExist(DataRep.SaveExp);

            return _apiFactory.ChangeContext<IClientsPageUi>(_driver);
        }
        
        public IClientsPageUi CreateClientUiPipe(string clientMail)
        {
           var clientName = clientMail.Split('@').First();

            SetFirstName(clientName);
            SetLastName(clientName);
            SetEmail(clientMail);
            SetPhone();
            SetCountry();
            SetGmtTimeZone();
            SetCurrency();
            ClickOnSaveButton();

            return _apiFactory.ChangeContext<IClientsPageUi>(_driver);
        }

        public IClientsPageUi CreateCbdClientUiPipe(string url, string clientName)
        {
            var email = clientName + _mailPerfix;

            var OfficeDetails = _apiFactory
                .ChangeContext<IOfficeTabApi>(_driver)
                .GetOfficesByName(url);

            _gmtTimezone = OfficeDetails.gmt_timezone;
            SetFirstName(clientName);
            SetLastName(clientName);
            SetEmail(email);
            SetPhone();
            SetCountry();
            SetGmtTimeZone();
            ClickOnSaveButton();

            return _apiFactory.ChangeContext<IClientsPageUi>(_driver);
        }

        public T ChangeContext<T>(IWebDriver driver) where T : class
        {
            return _apiFactory.ChangeContext<T>(_driver);
        }
    }
}
