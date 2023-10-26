using AirSoftAutomationFramework.Internals.DAL.ConfigurationManagement;
using AirSoftAutomationFramework.Internals.Factory;
using AirSoftAutomationFramework.Internals.Helpers;
using AirSoftAutomationFramework.Internals.NunitAttributes;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.AccountsApi.UsersMenuApi.UserMenuApi;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.Clients.ClientCard.FinancesTab;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.Clients.ClientCard.SATab;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.Clients.ClientPage;
using NUnit.Framework;
using TestsProject.TestsInternals;

namespace TestsProject.Tests.TradingPlatform
{
    [TestFixture]
    public class VerifyErrorWhenTransferFromToSAZeroOrNegativeAmount : TestSuitBase
    {
        #region Test Preparation

        private IApplicationFactory _apiFactory = new ApplicationFactory();
        private string _crmUrl = Config.appSettings.CrmUrl;
        private string _currentUserApiKey;
        private string _clientId;  
        private string _tradingPlatformUrl 
            =  Config.appSettings.tradingPlatformUrl;

        [SetUp]
        public void SetUp()
        {
            BeforeTest();
            #region PreCondition

            // create user
            var userName = TextManipulation.RandomString();

            var userId = _apiFactory
             .ChangeContext<IUserApi>()
             .PostCreateUserRequest(_crmUrl, userName);

            // create ApiKey
            _currentUserApiKey = _apiFactory
                .ChangeContext<IUserApi>()
                .PostCreateApiKeyRequest(_crmUrl, userId);

            // create client 
            var clientName = TextManipulation.RandomString();
            var clientEmail = clientName + DataRep.EmailPrefix;

            _clientId = _apiFactory
                .ChangeContext<ICreateClientApi>()
                .CreateClientRequest(_crmUrl, clientName);

            _apiFactory
                .ChangeContext<IFinancesTabApi>()
                .PostDepositRequest(_crmUrl, _clientId,
                1000, apiKey: _currentUserApiKey);
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
        [Description("based on jira: https://airsoftltd.atlassian.net/browse/AIRV2-5038")]
        [Category(DataRep.SanityCategory)]
        [Category(DataRep.RegressionCategory)]
        [RetryMenage(DataRep.NumOfRetraiesForFallingTest)]
        public void VerifyErrorWhenTransferFromToSAZeroOrNegativeAmountTest()
        {           
            var expectedErrormessage = "amount must be a positive number";

            var actualErrorTransferZeroAmountInCrm = _apiFactory
                .ChangeContext<ISATabApi>()
                .PostTransferToSavingAccountRequest(_crmUrl, _clientId,
                0, _currentUserApiKey, false);

            var actualErrorTransferNegativeAmountInCrm = _apiFactory
                .ChangeContext<ISATabApi>()
                .PostTransferToSavingAccountRequest(_crmUrl, _clientId,
                -10, _currentUserApiKey, false);

            var actualErrorTransferZeroAmountInTP = _apiFactory
                .ChangeContext<ISATabApi>()
                .PostTransferToSavingAccountRequest(_tradingPlatformUrl, _clientId,
                0, _currentUserApiKey, false);

            var actualErrorTransferNegativeAmountInTP = _apiFactory
                .ChangeContext<ISATabApi>()
                .PostTransferToSavingAccountRequest(_tradingPlatformUrl, _clientId,
                -10, _currentUserApiKey, false);

            Assert.Multiple(() =>
            {
                Assert.True(actualErrorTransferZeroAmountInCrm.Contains(expectedErrormessage),
                    $" expected Error message For Transfer Zero Amount In Crm:" +
                    $" {expectedErrormessage}" +
                    $" actual Error message For Transfer Zero Amount In Crm:" +
                    $" {actualErrorTransferZeroAmountInCrm}");

                Assert.True(actualErrorTransferNegativeAmountInCrm.Contains(expectedErrormessage),
                    $" expected Error message For Transfer Negative Amount In Crm:" +
                    $" {expectedErrormessage}" +
                    $" actual Error message For Transfer Negative Amount In Crm:" +
                    $" {actualErrorTransferNegativeAmountInCrm}");

                Assert.True(actualErrorTransferZeroAmountInTP.Contains(expectedErrormessage),
                    $" expected Error message For Transfer Zero Amount In TP:" +
                    $" {expectedErrormessage}" +
                    $" actual Error message For Transfer Zero Amount In TP:" +
                    $" {actualErrorTransferZeroAmountInTP}");

                Assert.True(actualErrorTransferNegativeAmountInTP.Contains(expectedErrormessage),
                    $" expected Error message For Transfer Negative Amount In TP:" +
                    $" {expectedErrormessage}" +
                    $" actual Error message For Transfer Negative Amount In TP:" +
                    $" {actualErrorTransferNegativeAmountInTP}");
            });
        }
    }
}