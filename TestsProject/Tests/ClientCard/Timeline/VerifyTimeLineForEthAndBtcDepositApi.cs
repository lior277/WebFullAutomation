// Ignore Spelling: Btc Eth Api TimeLine

using System.Collections.Generic;
using System.Linq;
using AirSoftAutomationFramework.Internals.DAL.ConfigurationManagement;
using AirSoftAutomationFramework.Internals.ExtensionsMethods;
using AirSoftAutomationFramework.Internals.Factorys;
using AirSoftAutomationFramework.Internals.Helpers;
using AirSoftAutomationFramework.Internals.NunitAtributes;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.AccountsApi.Roles;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.AccountsApi.UsersMenuApi.UserMenuApi;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.Clients.ClientCard;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.Clients.ClientCard.FinancesTab;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.Clients.ClientPage;
using NUnit.Framework;
using TestsProject.TestsInternals;

namespace TestsProject.Tests.ClientCard.TimeLine
{
    [TestFixture]
    public class VerifyTimeLineForEthAndBtcDepositApi : TestSuitBase
    {
        #region Test Preparation
        private IApplicationFactory _apiFactory = new ApplicationFactory();
        private string _crmUrl = Config.appSettings.CrmUrl;
        private string _clientId;
        private string _currentUserApiKey;

        [SetUp]
        public void SetUp()
        {
            BeforeTest();
            #region PreCondition

            // create user
            var userName = TextManipulation.RandomString();

            // create user
            var userId = _apiFactory
                .ChangeContext<IUserApi>()
                .PostCreateUserRequest(_crmUrl, userName,
                role: DataRep.AdminWithUsersOnlyRoleName);

            // create ApiKey
            _currentUserApiKey = _apiFactory
                .ChangeContext<IUserApi>()
                .PostCreateApiKeyRequest(_crmUrl, userId);

            // create client 
            var clientName = TextManipulation.RandomString();

            _clientId = _apiFactory
                .ChangeContext<ICreateClientApi>()
                .CreateClientRequest(_crmUrl, clientName);
            #endregion         
        }

        [TearDown]
        public void TearDown()
        {
            try
            {
            }
            finally
            {
                AfterTest();
            }
        }
        #endregion

        [Test]
        [Description("based on jira: https://airsoftltd.atlassian.net/browse/AIRV2-4625")]
        [Category(DataRep.SanityCategory)]
        [Category(DataRep.RegressionCategory)]
        [RetryMenage(DataRep.NumOfRetriesForFallingTest)]
        public void VerifyTimeLineForEthAndBtcDepositApiTest()
        {          
            var depositAmount = 10.11111111;

            // create Deposit in ETH deposit 
            _apiFactory
                 .ChangeContext<IFinancesTabApi>()
                .PostDepositRequest(_crmUrl,
                _clientId, depositAmount, originalCurrency:
                "ETH", apiKey: _currentUserApiKey);

            // create Deposit in BTC deposit 
            _apiFactory
                 .ChangeContext<IFinancesTabApi>()
                 .PostDepositRequest(_crmUrl,
                 _clientId, depositAmount, originalCurrency:
                 "BTC", apiKey: _currentUserApiKey);

            var actualTimeLineDepositValues = new List<double>();

            _apiFactory
                .ChangeContext<ITimeLineTabApi>()
                .GetTimelineRequest(_crmUrl, _clientId)
                .Where(p => p.original_amount == depositAmount)
                .ForEach(p => actualTimeLineDepositValues.Add(p.original_amount));

            Assert.Multiple(() =>
            {
                Assert.True(actualTimeLineDepositValues.All(p =>
                p.Equals(depositAmount)),
                    $" expected TimeLine Deposit Values:" +
                    $" {depositAmount}, {depositAmount} " +
                    $" actual TimeLine Deposit Values:" +
                    $" {actualTimeLineDepositValues.ListToString()}");
            });
        }
    }
}