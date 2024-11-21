// Ignore Spelling: Api

using AirSoftAutomationFramework.Internals.DAL.ConfigurationManagement;
using AirSoftAutomationFramework.Internals.Helpers;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.AccountsApi.UsersMenuApi.UserMenuApi;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.Clients.ClientCard;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.Clients.ClientCard.FinancesTab;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.Clients.ClientPage;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.Settings;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Trade;
using AirSoftAutomationFramework.Objects.ForexObjects.ApiGeneral;
using AirSoftAutomationFramework.Objects.ForexObjects.Ui.Login;
using NUnit.Framework;
using OpenQA.Selenium;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using AirSoftAutomationFramework.Internals.Factorys;
using AirSoftAutomationFramework.Internals.NunitAtributes;
using TestsProject.TestsInternals;

namespace TestsProject.Tests.ClientsPage.Filters
{
    [TestFixture(DataRep.Chrome)]
    public class VerifyFiltersOnClientsPageApi : TestSuitBase
    {
        public VerifyFiltersOnClientsPageApi(string browser) : base(browser)
        {
            _browserName = browser;
        }

        #region members
        private string _firstClientEmail;
        private string _secondClientEmail;
        private string _accountTypeId;
        private string _savingAccountId;
        private string _campaignId;
        private string _browserName;
        private string _clientTimeZone;
        private string _freeText;
        private string _freeText2;
        private string _freeText3;
        private string _freeText4;
        private string _freeText5;
        private string _mainOfficeId;
        private string _defaultTradeGroupId;
        private string _userId;
        private string _currentUserApiKey;
        private string _tradingPlatformUrl = Config.appSettings.tradingPlatformUrl;
        private string _crmUrl = Config.appSettings.CrmUrl;
        private IWebDriver _driver;
        private IApplicationFactory _apiFactory = new ApplicationFactory();
        #endregion      

        [SetUp]
        public void SetUp()
        {
            BeforeTest(_browserName);
            _driver = GetDriver();
            #region PreCondition
            var depositAmount = 400;

            _mainOfficeId = _apiFactory
                .ChangeContext<IOfficeTabApi>()
                .GetOfficesByName(_crmUrl)
                ._id;

            // create user
            var userName = TextManipulation.RandomString();

            _userId = _apiFactory
                .ChangeContext<IUserApi>()
                .PostCreateUserRequest(_crmUrl, userName);

            #region create ApiKey
            // create ApiKey
            _currentUserApiKey = _apiFactory
                .ChangeContext<IUserApi>()
                .PostCreateApiKeyRequest(_crmUrl, _userId);
            #endregion

            // Create Affiliate And Campaign
            var campaignData = _apiFactory
                .ChangeContext<ISharedStepsGenerator>()
                .CreateAffiliateAndCampaignApi(_crmUrl);

            _campaignId = campaignData.Values.First();

            // create first client with campaign
            var firstClientName = TextManipulation.RandomString();
            _firstClientEmail = firstClientName + DataRep.EmailPrefix;

            _freeText = TextManipulation.RandomString();
            _freeText2 = $"{_freeText}_2";
            _freeText3 = $"{_freeText}_3";
            _freeText4 = $"{_freeText}_4";
            _freeText5 = $"{_freeText}_5";

            var firstClientsId = _apiFactory
                .ChangeContext<ICreateClientApi>()
                .CreateClientWithCampaign(_crmUrl, firstClientName,
                campaignId: _campaignId, freeText: _freeText,
                apiKey: _currentUserApiKey,
                freeText2: _freeText2, freeText3: _freeText3,
                freeText4: _freeText4, freeText5: _freeText5);

            _apiFactory
                .ChangeContext<ILoginPageUi>(_driver)
                .LoginPipe(_firstClientEmail, _tradingPlatformUrl);

            _clientTimeZone = _apiFactory
                .ChangeContext<IClientsApi>()
                .GetClientByIdRequest(_crmUrl, firstClientsId)
                .GeneralResponse
                .user
                .gmt_timezone;

            // create deposit
            _apiFactory
                .ChangeContext<IFinancesTabApi>()
                .PostDepositRequest(_crmUrl, firstClientsId, depositAmount);

            // get default SA id
            _savingAccountId = _apiFactory
                .ChangeContext<ISalesTabApi>()
                .GetSavingAccountsRequest(_crmUrl, _currentUserApiKey)
                .SavingAccountData
                .Where(p => p.Default)
                .FirstOrDefault()
                .Id;

            // get default account type id
            _accountTypeId = _apiFactory
                .ChangeContext<ISalesTabApi>()
                .GetAccountTypesRequest(_crmUrl)
                .AccountTypeData
                .Where(p => p.AccountTypeName == "Default")
                .FirstOrDefault()
                .AccountTypeId;

            // get first group id by name
            _defaultTradeGroupId = _apiFactory
                .ChangeContext<ITradeGroupApi>()
                .GetTradeGroupsRequest(_crmUrl)
                .GeneralResponse
                .Where(p => p.@default)
                .FirstOrDefault()
                ._id;

            // create second client
            var secondClientName = TextManipulation.RandomString();
            _secondClientEmail = secondClientName + DataRep.EmailPrefix;

            var secondClientId = _apiFactory
                 .ChangeContext<ICreateClientApi>()
                 .CreateClientRequest(_crmUrl, secondClientName,
                 apiKey: _currentUserApiKey);

            // connect One User To One Client notification
            _apiFactory
                .ChangeContext<IClientsApi>()
                .PatchMassAssignSaleAgentsRequest(_crmUrl, _userId,
                new List<string> { firstClientsId, secondClientId }, apiKey: _currentUserApiKey);

            // block client
            _apiFactory
                .ChangeContext<IInformationTabApi>()
                .PatchCilentStatusRequest(_crmUrl, secondClientId, false);
        }
        #endregion

        [TearDown]
        public void TearDown()
        {
            try
            {
            }
            finally
            {
                AfterTest(_driver);
                DriverDispose(_driver);
            }
        }

        [Test]
        [Description("Based on https://airsoftltd.atlassian.net/browse/AIRV2-5143")]
        [Category(DataRep.SanityCategory)]
        [Category(DataRep.RegressionCategory)]
        [RetryMenage(DataRep.NumOfRetriesForFallingTest)]
        public void VerifyFiltersOnClientsPageApiTest()
        {
            // dont delete , wait for client to be online in redis
            Thread.Sleep(10000);

            var filters = new Dictionary<string, string> {
                { "sales_agents[]", _userId },
                { "country[]", DataRep.UserDefaultCountry },
                { "timezones[]", _clientTimeZone },
                { "sales_status_text[]", "Double Phone Number" },
                { "cfd_group_id[]", _defaultTradeGroupId },
                { "free_text[]", _freeText },
                { "free_text_2[]", $"{_freeText2}" },
                { "free_text_3[]", _freeText3},
                { "free_text_4[]", _freeText4 },
                { "free_text_5[]", _freeText5 },
                { "campaigns[]", _campaignId },
                { "office[]", _mainOfficeId },
                { "account_type_id[]", _accountTypeId },
                { "saving_account_id[]", _savingAccountId },
                { "activation_status[]", "Active" },
                { "online", "true" },
                { "active", "true" },
                { "custom_deposit[]", "has_deposits" },
                { "kyc_proof_of_identity_status[]", "Waiting" },
                { "kyc_proof_of_residency_status[]", "Waiting" },
                { "kyc_status[]", "Pending" },
                { "kyc_credit_debit_card_documentation_status[]", "Waiting" },
                { "kyc_credit_debit_card_back_documentation_status[]", "Waiting" },
            };

            var actualFirstClientByFilter = _apiFactory
                .ChangeContext<IClientsApi>()
                .GetClientByFilterRequest(_crmUrl, filters, _currentUserApiKey)
                .GeneralResponse
                .data;

            var secondClientFilters = new Dictionary<string, string> {
                { "active", "false" },
                { "sales_agents[]", _userId }
            };

            var actualSecondClientByFilter = _apiFactory
                .ChangeContext<IClientsApi>()
                .GetClientByFilterRequest(_crmUrl, secondClientFilters, _currentUserApiKey)
                .GeneralResponse
                .data;

            Assert.Multiple(() =>
            {
                Assert.True(actualFirstClientByFilter.First().free_text == _freeText,
                    $" expected Client free text: {_freeText}" +
                    $" actual Client free text :  {actualFirstClientByFilter.First().free_text}");

                Assert.True(actualFirstClientByFilter.First().email == _firstClientEmail,
                    $" expected Client By Filter email: {_firstClientEmail}" +
                    $" actual Client By Filter email :  {actualFirstClientByFilter.First().email}");

                Assert.True(actualFirstClientByFilter.Count() == 1,
                    $" expected num of clients: {1}" +
                    $" actual num of clients :  {actualFirstClientByFilter.Count()}");

                Assert.True(actualSecondClientByFilter.First().email == _secondClientEmail,
                    $" expected Second Client By Filter: {_secondClientEmail}" +
                    $" actual Second Client By Filter :  {actualSecondClientByFilter.First().email}");

                Assert.True(actualSecondClientByFilter.Count() == 1,
                   $" expected Second Filter num of clients: {1}" +
                   $" actual Second Filter num of clients :  {actualSecondClientByFilter.Count()}");
            });
        }
    }
}
