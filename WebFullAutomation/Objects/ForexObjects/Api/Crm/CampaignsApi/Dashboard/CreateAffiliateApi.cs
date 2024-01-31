// Ignore Spelling: Api Forex Crm Ip

using AirSoftAutomationFramework.Internals.DAL.ApiAccsess;
using AirSoftAutomationFramework.Internals.DAL.ApiAccsess.ApiRoutes;
using AirSoftAutomationFramework.Internals.DAL.ConfigurationManagement;
using AirSoftAutomationFramework.Internals.Factorys;
using AirSoftAutomationFramework.Internals.Helpers;
using AirSoftAutomationFramework.Objects.DTOs;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.Settings;
using Newtonsoft.Json.Linq;
using OpenQA.Selenium;

namespace AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.CampaignsApi.Dashboard
{
    public class CreateAffiliateApi : ICreateAffiliateApi
    {
        #region Members
        private string _firstName;
        private string _lastName;
        private string _userName;
        private string _email;
        private string _phone;
        private string _office;
        private string[] _ipAddresses;
        private string _role;
        private string _affiliateManager;
        private IWebDriver _driver;
        private IApiAccess _apiAccess;
        private string _apiKey = Config.appSettings.ApiKey;
        private string _password = DataRep.Password;
        private readonly IApplicationFactory _apiFactory;
        private string _mailPerfix = DataRep.EmailPrefix;
        #endregion Members

        public CreateAffiliateApi(IApplicationFactory apiFactory,
            IWebDriver driver, IApiAccess apiAccess)
        {
            _apiFactory = apiFactory;
            _driver = driver;
            _apiAccess = apiAccess;
        }

        public string PostCreateAffiliateRequest(string uri, string apiKey = null)
        {
            _apiKey = apiKey ?? _apiKey;
            var route = $"{uri}{ApiRouteAggregate.PostCreateAffiliate()}?api_key={_apiKey}";

            var userDto = new CreateAffiliateRequest
            {
                affiliate_manager = "",
                first_name = _firstName,
                last_name = _lastName,
                username = _userName,
                phone = _phone,
                email = _email,
                password = _password,
                role = _role,
                allowed_ip_addresses = _ipAddresses,
                office = _office,
            };
            var response = _apiAccess.ExecutePostEntry(route, userDto);
            _apiAccess.CheckStatusCode(route, response);

            var json = response
               .Content
               .ReadAsStringAsync()
               .Result;

            return JObject.Parse(json)["id"].ToString();
        }

        public ICreateAffiliateApi SetFirstName(string firstName = null)
        {
            _firstName = firstName ?? _userName;

            return this;
        }

        public ICreateAffiliateApi SetLastName(string lastName = null)
        {
            _lastName = lastName ?? _userName;

            return this;
        }

        public ICreateAffiliateApi SetUserName(string userName)
        {
            _userName = userName;

            return this;
        }

        public ICreateAffiliateApi SetEmail(string email = null)
        {
            _email = email ?? _email;

            return this;
        }

        public ICreateAffiliateApi SetPhone(string phone = null)
        {
            _phone = phone ?? DataRep.UserDefaultPhone;

            return this;
        }

        public ICreateAffiliateApi SetOfficeId(string url, string office = null)
        {
            var officeDetails = _apiFactory
                  .ChangeContext<IOfficeTabApi>(_driver)
                  .GetOfficesByName(url);

            _office = office ?? officeDetails._id;  //"5e4e4dbba844288a08de503e";

            return this;
        }

        public ICreateAffiliateApi SetIpAddresses(string[] ipAddresses = null)
        {

            _ipAddresses = ipAddresses ?? DataRep.UserAllowedIps;

            return this;
        }

        public ICreateAffiliateApi SetRole(string role = null)
        {
            _role = role ?? "affiliate";

            return this;
        }

        public ICreateAffiliateApi SetAffiliateManager(string affiliateManager)
        {
            _affiliateManager = affiliateManager ?? "admin";

            return this;
        }

        public ICreateAffiliateApi SetPassword(string password = null)
        {
            _password = password ?? _password;

            return this;
        }

        public string CreateAffiliateApiPipe(string url,
            string userName, string roleName = null, string apiKey = null)
        {
            _apiKey = apiKey ?? _apiKey;
            _firstName = userName;
            var email = userName + _mailPerfix;

            SetFirstName(_firstName);
            SetLastName(_firstName);
            SetUserName(userName);
            SetEmail(email);
            SetPhone();
            SetOfficeId(url);
            SetIpAddresses();
            SetRole(roleName);
            SetPassword();

            return PostCreateAffiliateRequest(url, _apiKey);
        }

        public T ChangeContext<T>(IWebDriver driver) where T : class
        {
            return _apiFactory.ChangeContext<T>(_driver);
        }
    }
}
