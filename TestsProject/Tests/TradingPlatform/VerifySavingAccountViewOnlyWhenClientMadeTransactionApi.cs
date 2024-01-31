// Ignore Spelling: Api

using System.Linq;
using AirSoftAutomationFramework.Internals.DAL.ConfigurationManagement;
using AirSoftAutomationFramework.Internals.Factorys;
using AirSoftAutomationFramework.Internals.Helpers;
using AirSoftAutomationFramework.Internals.NunitAtributes;
using AirSoftAutomationFramework.Objects.DTOs;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.AccountsApi.UsersMenuApi.UserMenuApi;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.Clients.ClientCard;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.Clients.ClientCard.FinancesTab;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.Clients.ClientCard.SATab;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.Clients.ClientPage;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.Settings;
using AirSoftAutomationFramework.Objects.ForexObjects.ApiGeneral.Login;
using AirSoftAutomationFramework.Objects.ForexObjects.TradingPlatform.TradeObjects.TradeApi.Settings;
using NUnit.Framework;
using TestsProject.TestsInternals;
using static AirSoftAutomationFramework.Objects.DTOs.GetInformationTabResponse;

namespace TestsProject.Tests.TradingPlatform
{
    [TestFixture]
    public class VerifySavingAccountViewOnlyWhenClientMadeTransactionApi : TestSuitBase
    {
        #region Test Preparation
        private IApplicationFactory _apiFactory = new ApplicationFactory();
        private string _crmUrl = Config.appSettings.CrmUrl;
        private string _userId;
        private string _currentUserApiKey;
        private string _tradedingPlatformUrl = Config.appSettings.tradingPlatformUrl;
        private InformationTab _informationTabResponse;
        private int _transferToSAAmount = 10;
        private int _transferToSABalance = 5;
        private GetLoginResponse _loginData;
        private int _depositAmount = 200;
        private string _clientId;
        private string _savingAccountId;

        [SetUp]
        public void SetUp()
        {
            BeforeTest();

            // create user
            var userName = TextManipulation.RandomString();

            // create user
            _userId = _apiFactory
             .ChangeContext<IUserApi>()
             .PostCreateUserRequest(_crmUrl, userName);

            // create ApiKey
            _currentUserApiKey = _apiFactory
                .ChangeContext<IUserApi>()
                .PostCreateApiKeyRequest(_crmUrl, _userId);

            // create client 
            var clientName = TextManipulation.RandomString();
            var clientEmail = clientName + DataRep.EmailPrefix;

            _clientId = _apiFactory
                .ChangeContext<ICreateClientApi>()
                .CreateClientRequest(_crmUrl, clientName,
                apiKey: _currentUserApiKey);

            // get login Data for trading Platform
            _loginData = _apiFactory
                 .ChangeContext<ILoginApi>()
                 .PostLoginToTradingPlatform(_tradedingPlatformUrl, clientEmail)
                 .GeneralResponse;

            _apiFactory
               .ChangeContext<IFinancesTabApi>()
               .PostDepositRequest(_crmUrl,
               _clientId, _depositAmount, apiKey: _currentUserApiKey);

            // verify default SA exist
            // create Saving Account
            var expectedSavingAccountName = _apiFactory
                .ChangeContext<ISalesTabApi>()
                .PostCreateSavingAccountRequest(_crmUrl,
                apiKey: _currentUserApiKey);

            _savingAccountId = _apiFactory
                .ChangeContext<ISalesTabApi>()
                .GetSavingAccountsRequest(_crmUrl, _currentUserApiKey)
                .SavingAccountData
                .Where(p => p.Name == expectedSavingAccountName)
                .FirstOrDefault()
                .Id;

            _informationTabResponse = _apiFactory
                .ChangeContext<IInformationTabApi>()
                .GetInformationTabRequest(_crmUrl, _clientId)
                .GeneralResponse
                .informationTab;

            _informationTabResponse.saving_account_id = _savingAccountId;

            // update client card with new saving account
            _apiFactory
                .ChangeContext<IInformationTabApi>()
                .PutInformationTabRequest(_crmUrl, _informationTabResponse,
                apiKey: _currentUserApiKey);

            // CRM Transfer To Saving Account
            _apiFactory
                .ChangeContext<ISATabApi>()
                .PostTransferToSavingAccountRequest(_crmUrl,
                _clientId, _transferToSAAmount, _currentUserApiKey);
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
        [Description("based on jira: https://airsoftltd.atlassian.net/browse/AIRV2-5104")]
        [Category(DataRep.SanityCategory)]
        [Category(DataRep.RegressionCategory)]
        [RetryMenage(DataRep.NumOfRetraiesForFallingTest)]
        public void VerifySavingAccountViewOnlyWhenClientMadeTransactionApiTest()
        {
            var expectedErrorMessage = "Method Not Allowed";
            _informationTabResponse.saving_account_id = "null";

            // delete client saving account
            _apiFactory
                .ChangeContext<IInformationTabApi>()
                .PutInformationTabRequest(_crmUrl, _informationTabResponse,
                apiKey: _currentUserApiKey);

            // CRM Transfer To Saving Account
            var actoalErrorMessageToSa = _apiFactory
                .ChangeContext<ISavingAccountTpApi>()
                .PostTransferToSavingAccountFromTpRequest(_tradedingPlatformUrl,
                _clientId, _transferToSAAmount, _loginData,
                checkStatusCode: false);

            var actoalErrorMessageToBalance = _apiFactory
              .ChangeContext<ISavingAccountTpApi>()
              .PostTransferToBalanceFromTpRequest(_tradedingPlatformUrl,
              _clientId, _transferToSABalance, _loginData,
              checkStatusCode: false);

            Assert.Multiple(() =>
            {
                Assert.True(actoalErrorMessageToSa == expectedErrorMessage,
                    $" expected Error Message when transfer To Sa: {expectedErrorMessage} $" +
                    $" actual Error Message when transfer To Sa: {actoalErrorMessageToSa}");

                Assert.True(actoalErrorMessageToBalance == expectedErrorMessage,
                    $" expected Error Message when transfer To Balance: {expectedErrorMessage} $" +
                    $" actual Error Message when transfer To Balance: {actoalErrorMessageToBalance}");
            });
        }
    }
}