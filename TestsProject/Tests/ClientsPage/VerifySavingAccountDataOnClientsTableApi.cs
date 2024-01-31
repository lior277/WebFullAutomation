using System.Linq;
using AirSoftAutomationFramework.Internals.DAL.ConfigurationManagement;
using AirSoftAutomationFramework.Internals.Factorys;
using AirSoftAutomationFramework.Internals.Helpers;
using AirSoftAutomationFramework.Internals.NunitAtributes;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.AccountsApi.UsersMenuApi.UserMenuApi;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.Clients.ClientCard;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.Clients.ClientCard.FinancesTab;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.Clients.ClientCard.SATab;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.Clients.ClientPage;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.Settings;
using NUnit.Framework;
using TestsProject.TestsInternals;

namespace TestsProject.Tests.ClientsPage
{
    [TestFixture]
    public class VerifySavingAccountDataOnClientsTableApi : TestSuitBase
    {
        #region Test Preparation
        private IApplicationFactory _apiFactory = new ApplicationFactory();
        private string _crmUrl = Config.appSettings.CrmUrl;
        private string _userId;
        private string _currentUserApiKey;
        private string _clientId;

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

            _clientId = _apiFactory
                .ChangeContext<ICreateClientApi>()
                .CreateClientRequest(_crmUrl, clientName,
                apiKey: _currentUserApiKey);

            _apiFactory
               .ChangeContext<IFinancesTabApi>()
               .PostDepositRequest(_crmUrl, _clientId, 200, apiKey: _currentUserApiKey);

            // verify default SA exist
            // create Saving Account
            var expectedSavingAccountName = _apiFactory
                .ChangeContext<ISalesTabApi>()
                .PostCreateSavingAccountRequest(_crmUrl, apiKey: _currentUserApiKey);

            var savingAccountId = _apiFactory
                .ChangeContext<ISalesTabApi>()
                .GetSavingAccountsRequest(_crmUrl, _currentUserApiKey)
                .SavingAccountData
                .Where(p => p.Name == expectedSavingAccountName)
                .FirstOrDefault()
                .Id;

            var informationTabResponse = _apiFactory
                .ChangeContext<IInformationTabApi>()
                .GetInformationTabRequest(_crmUrl, _clientId)
                .GeneralResponse
                .informationTab;

            informationTabResponse.saving_account_id = savingAccountId;

            // new saving Account
            _apiFactory
                .ChangeContext<IInformationTabApi>()
                .PutInformationTabRequest(_crmUrl, informationTabResponse,
                apiKey: _currentUserApiKey);
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
        [Category(DataRep.SanityCategory)]
        [Category(DataRep.RegressionCategory)]
        [RetryMenage(DataRep.NumOfRetraiesForFallingTest)]
        public void VerifySavingAccountDataOnClientsTableApiTest()
        {
            var _tradingPlatformUrl = Config.appSettings.tradingPlatformUrl;
            var transferToSAAmount = 100;
            var transferToSABalance = 50;

            // CRM Transfer To Saving Account
            _apiFactory
                .ChangeContext<ISATabApi>()
                .PostTransferToSavingAccountRequest(_crmUrl, _clientId, transferToSAAmount,
                _currentUserApiKey);

            // CRM Transfer To Balance
            _apiFactory
                .ChangeContext<ISATabApi>()
                .PostTransferToBalanceRequest(_crmUrl, _clientId, transferToSABalance,
               _currentUserApiKey);

            // treading platform Transfer To Saving Account
            _apiFactory
                .ChangeContext<ISATabApi>()
                .PostTransferToSavingAccountRequest(_tradingPlatformUrl, _clientId,
                transferToSAAmount, _currentUserApiKey);

            // treading platform Transfer To Balance
            _apiFactory
                .ChangeContext<ISATabApi>()
                .PostTransferToBalanceRequest(_tradingPlatformUrl,
                _clientId, transferToSABalance, _currentUserApiKey);

           var actualSABalanceInClientTable = _apiFactory
               .ChangeContext<IClientsApi>()
               .GetClientRequest(_crmUrl, _clientId)
               .GeneralResponse
               .data
                .FirstOrDefault()
               .sa_balance;

            Assert.Multiple(() =>
            {
                Assert.True(actualSABalanceInClientTable == $"{transferToSAAmount}.00 $",
                    $" expected SA Balance In Client Table: {transferToSAAmount} $" +
                    $" actual SA Balance In Client Table: {actualSABalanceInClientTable}");
            });
        }
    }
}