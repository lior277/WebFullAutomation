using AirSoftAutomationFramework.Internals.DAL.ConfigurationManagement;
using AirSoftAutomationFramework.Internals.Factory;
using AirSoftAutomationFramework.Internals.Helpers;
using AirSoftAutomationFramework.Internals.NunitAttributes;
using AirSoftAutomationFramework.Objects.DTOs;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.AccountsApi.UsersMenuApi.UserMenuApi;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.Clients.ClientCard.FinancesTab;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.Clients.ClientPage;
using AirSoftAutomationFramework.Objects.ForexObjects.ApiGeneral.Login;
using AirSoftAutomationFramework.Objects.ForexObjects.TradingPlatform.TradeObjects.TradeApi.Settings;
using NUnit.Framework;
using TestsProject.TestsInternals;

namespace TestsProject.Tests.TradingPlatform
{
    [TestFixture]
    public class VerifyErrorMessageWhenWithdrawalAmountIsZeroOrNegative : TestSuitBase
    {
        #region Test Preparation

        private IApplicationFactory _apiFactory = new ApplicationFactory();
        private string _crmUrl = Config.appSettings.CrmUrl;
        private GetLoginResponse _loginData;
        private string _tradingPlatformUrl 
            =  Config.appSettings.tradingPlatformUrl;

        [SetUp]
        public void SetUp()
        {
            BeforeTest();
            #region PreCondition

            // create user
            var userName = TextManipulation.RandomString();

            var userId =  _apiFactory
             .ChangeContext<IUserApi>()
             .PostCreateUserRequest(_crmUrl, userName);

            // create ApiKey
            var currentUserApiKey = _apiFactory
                .ChangeContext<IUserApi>()
                .PostCreateApiKeyRequest(_crmUrl, userId);

            // create client 
            var clientName = TextManipulation.RandomString();
            var clientEmail = clientName + DataRep.EmailPrefix;

            var clientId = _apiFactory
                .ChangeContext<ICreateClientApi>()
                .CreateClientRequest(_crmUrl, clientName);

            // login data
            _loginData = _apiFactory
                .ChangeContext<ILoginApi>()
                .PostLoginToTradingPlatform(_tradingPlatformUrl, clientEmail)
                .GeneralResponse;

            _apiFactory
                .ChangeContext<IFinancesTabApi>()
                .PostDepositRequest(_crmUrl, clientId,
                1000, apiKey: currentUserApiKey);
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
        [Description("based on jira: https://airsoftltd.atlassian.net/browse/AIRV2-5018")]
        [Category(DataRep.SanityCategory)]
        [Category(DataRep.RegressionCategory)]
        [RetryMenage(DataRep.NumOfRetraiesForFallingTest)]
        public void VerifyErrorMessageWhenWithdrawalAmountIsZeroOrNegativeTest()
        {
            var expectedErrormessage = "amount must be a positive number";

            // create Withdrawal
            var actualErrormessageForWithdrawalZero = _apiFactory
                .ChangeContext<IWithdrawalTpApi>()
                .PostPendingWithdrawalRequest(_tradingPlatformUrl,
                _loginData, 0, false);

            // create Withdrawal
            var actualErrormessageForWithdrawalNegative = _apiFactory
                .ChangeContext<IWithdrawalTpApi>()
                .PostPendingWithdrawalRequest(_tradingPlatformUrl,
                _loginData, -10, false);

            Assert.Multiple(() =>
            {
                Assert.True(actualErrormessageForWithdrawalZero.Contains(expectedErrormessage),
                    $" expected Error message For Withdrawal Zero:" +
                    $" {expectedErrormessage}" +
                    $" actual Error message For Withdrawal Zero:" +
                    $" {actualErrormessageForWithdrawalZero}");

                Assert.True(actualErrormessageForWithdrawalNegative.Contains(expectedErrormessage),
                  $" expected Error message For Withdrawal Negative:" +
                  $" {expectedErrormessage}" +
                  $" actual Error message For Withdrawal Negative:" +
                  $" {actualErrormessageForWithdrawalNegative}");
            });
        }
    }
}