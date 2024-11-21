// Ignore Spelling: Api

using System.Collections.Generic;
using AirSoftAutomationFramework.Internals.DAL.ConfigurationManagement;
using AirSoftAutomationFramework.Internals.Factorys;
using AirSoftAutomationFramework.Internals.Helpers;
using AirSoftAutomationFramework.Internals.NunitAtributes;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.AccountsApi.UsersMenuApi.UserMenuApi;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.Banking;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.Clients.ClientCard.FinancesTab;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.Clients.ClientPage;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.Settings;
using AirSoftAutomationFramework.Objects.ForexObjects.ApiGeneral.Login;
using AirSoftAutomationFramework.Objects.ForexObjects.TradingPlatform.TradeObjects.TradeApi;
using NUnit.Framework;
using TestsProject.TestsInternals;

namespace TestsProject.Tests.Banking.DepositsPage.Filters
{
    [TestFixture]
    public class VerifyStatusFilterOnBankingDepositPageApi : TestSuitBase
    {
        #region members
        private IApplicationFactory _apiFactory = new ApplicationFactory();
        private string _crmUrl = Config.appSettings.CrmUrl;
        private string _userId;
        private string _clientEmail;
        private string _tradingPlatformUrl = Config.appSettings.tradingPlatformUrl;
        private int _firstDepositAmount = 400;
        private string _currentUserApiKey;
        #endregion      

        [SetUp]
        public void SetUp()
        {
            BeforeTest();
            #region PreCondition

            var userName = TextManipulation.RandomString();

            // create user
            _userId = _apiFactory
             .ChangeContext<IUserApi>()
             .PostCreateUserRequest(_crmUrl, userName,
             role: DataRep.AdminWithUsersOnlyRoleName);

            #region create ApiKey
            // create ApiKey
            _currentUserApiKey = _apiFactory
                .ChangeContext<IUserApi>()
                .PostCreateApiKeyRequest(_crmUrl, _userId);
            #endregion

            #region create client
            // create client
            var clientName = TextManipulation.RandomString();
            _clientEmail = clientName + DataRep.EmailPrefix;

            var clientId = _apiFactory
                .ChangeContext<ICreateClientApi>()
                .CreateClientRequest(_crmUrl, clientName,
                currency: "EUR", apiKey: _currentUserApiKey);

            var clientsId = new List<string> { clientId };
            #endregion

            #region connect One User To One Client 
            // connect One User To One Client 
            _apiFactory
                .ChangeContext<IClientsApi>()
                .PatchMassAssignSaleAgentsRequest(_crmUrl, _userId,
                clientsId, apiKey: _currentUserApiKey);
            #endregion

            #region get login data
            // get login data
            var loginData = _apiFactory
                .ChangeContext<ILoginApi>()
                .PostLoginToTradingPlatform(_tradingPlatformUrl, _clientEmail)
                .GeneralResponse;
            #endregion

            #region deposit 400
            // deposit 400
            _apiFactory
                .ChangeContext<IFinancesTabApi>()
                .PostDepositRequest(_crmUrl,
                clientsId, _firstDepositAmount,
                 originalCurrency: "EUR", apiKey: _currentUserApiKey);
            #endregion

            // create psp for deposit page
            _apiFactory
              .ChangeContext<IPspTabApi>()
              .PostCreateAirsoftSandboxPspRequest(_crmUrl);

            // create pending deposit
            _apiFactory
              .ChangeContext<ITradeDepositPageApi>()
              .PostCreatePaymentRequestPipe(_tradingPlatformUrl, loginData);
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
                AfterTest();
            }
        }

        [Test]
        [Category(DataRep.SanityCategory)]
        [Category(DataRep.RegressionCategory)]
        [RetryMenage(DataRep.NumOfRetriesForFallingTest)]
        public void VerifyStatusFilterOnBankingDepositPageApiTest()
        {
            var getApprovedDeposits = _apiFactory
                .ChangeContext<IDepositsPageApi>()
                .GetDepositDataFromBankingRequest(_crmUrl, "status", "approved",
                _currentUserApiKey)
                .GeneralResponse;

            var getPendingDeposits = _apiFactory
                .ChangeContext<IDepositsPageApi>()
                .GetDepositDataFromBankingRequest(_crmUrl, "status", "pending",
                _currentUserApiKey)
                .GeneralResponse;

            Assert.Multiple(() =>
            {
                Assert.True(getApprovedDeposits.recordsTotal == 1,
                    $" expected Total records: 1" +
                    $" actual Total records :  {getApprovedDeposits.recordsTotal}");

                Assert.True(getApprovedDeposits.data[0].status == "approved",
                    $" expected deposit status: approved" +
                    $" actual deposit status :  {getApprovedDeposits.data[0].status}");

                Assert.True(getPendingDeposits.recordsTotal == 1,
                     $" expected Total records: 1" +
                     $" actual Total records :  {getApprovedDeposits.recordsTotal}");

                Assert.True(getPendingDeposits.data[0].status == "pending",
                    $" expected deposit status: pending" +
                    $" actual deposit status :  {getApprovedDeposits.data[0].status}");
            });
        }
    }
}
