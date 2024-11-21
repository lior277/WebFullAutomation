// Ignore Spelling: Api Crm

using System.Collections.Generic;
using System.Linq;
using System.Threading;
using AirSoftAutomationFramework.Internals.DAL.ConfigurationManagement;
using AirSoftAutomationFramework.Internals.Factorys;
using AirSoftAutomationFramework.Internals.Helpers;
using AirSoftAutomationFramework.Internals.NunitAtributes;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.AccountsApi.UsersMenuApi.UserMenuApi;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.Clients.ClientCard.FinancesTab;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.Clients.ClientPage;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.Settings;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Trade;
using AirSoftAutomationFramework.Objects.ForexObjects.Ui.Login;
using NUnit.Framework;
using OpenQA.Selenium;
using Pomelo.EntityFrameworkCore.MySql.Query.ExpressionVisitors.Internal;
using TestsProject.TestsInternals;

namespace TestsProject.Tests.CrmTrade.RiskPage
{
    [TestFixture(DataRep.Chrome)]
    public class VerifyFiltersOnRisksPageApi : TestSuitBase
    {
        public VerifyFiltersOnRisksPageApi(string browser) : base(browser)
        {
            _browserName = browser;
        }

        #region members
        private string _firstClientEmail;
        private string _secondClientEmail;
        private string _accountTypeId;
        private string _savingAccountId;
        private string _mainOfficeId;
        private string _defaultTradeGroupId;
        private string _userId;
        private string _browserName;
        private string _currentUserApiKey;
        private string _tradingPlatformUrl = Config.appSettings.tradingPlatformUrl;
        private IWebDriver _driver;
        private string _crmUrl = Config.appSettings.CrmUrl;
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

            // create first client 
            var firstClientName = TextManipulation.RandomString();
            _firstClientEmail = firstClientName + DataRep.EmailPrefix;

            var firstClientsId = _apiFactory
                .ChangeContext<ICreateClientApi>()
                .CreateClientRequest(_crmUrl, firstClientName,
                apiKey: _currentUserApiKey);

            _apiFactory
                .ChangeContext<ILoginPageUi>(_driver)
                .LoginPipe(_firstClientEmail, _tradingPlatformUrl);

            _apiFactory
                .ChangeContext<IClientsApi>()
                .WaitForClientToBeLogin(_tradingPlatformUrl,
                firstClientsId, _currentUserApiKey);

            // create deposit
            _apiFactory
                .ChangeContext<IFinancesTabApi>()
                .PostDepositRequest(_crmUrl, firstClientsId, depositAmount);

            // connect One User To One Client notification
            _apiFactory
                .ChangeContext<IClientsApi>()
                .PatchMassAssignSaleAgentsRequest(_crmUrl, _userId,
                new List<string> { firstClientsId }, apiKey: _currentUserApiKey);

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

            _apiFactory
                .ChangeContext<ICreateClientApi>()
                .CreateClientRequest(_crmUrl, secondClientName,
                apiKey: _currentUserApiKey);
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
        [Description("bug on main office")]
        [Category(DataRep.SanityCategory)]
        [Category(DataRep.RegressionCategory)]
        [RetryMenage(DataRep.NumOfRetriesForFallingTest)]
        public void VerifyFiltersOnRisksPageApiTest()
        {
            // dont delete , wait for client to on line in redis
            Thread.Sleep(5000);

            var filters = new Dictionary<string, string> {
                { "sales_agents[]", _userId },
                { "status[]", "Double Phone Number" },
                { "cfd_group_id[]", _defaultTradeGroupId },
                { "office[]", _mainOfficeId },
                { "account_type_id[]", _accountTypeId },
                { "saving_account_id[]", _savingAccountId },
                { "activation_status[]", "Active" },
                { "online", "true" },
                { "custom_deposit", "has_deposits" },
                { "ftd", "1" },
            };

            var actualRisksByFilter = _apiFactory
                .ChangeContext<IRiskPageApi>()
                .GetRisksByFilterRequest(_crmUrl, filters, _currentUserApiKey)
                .GeneralResponse
                .data
                .ToList();

            Assert.Multiple(() =>
            {
                Assert.True(actualRisksByFilter.Any(p => p.erp_user_id == _userId),
                   $" expected risk By Filter user id: {_userId}" +
                   $" actual risk By Filter user id :  {actualRisksByFilter}");

                Assert.True(actualRisksByFilter.Count() == 1,
                   $" expected num of risks: {1}" +
                   $" actual num of risks :  {actualRisksByFilter.Count()}");
            });
        }
    }
}
