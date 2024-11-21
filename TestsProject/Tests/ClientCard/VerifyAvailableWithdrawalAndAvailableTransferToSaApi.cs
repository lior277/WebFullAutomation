// Ignore Spelling: Api

using AirSoftAutomationFramework.Internals.DAL.ConfigurationManagement;
using AirSoftAutomationFramework.Internals.Factorys;
using AirSoftAutomationFramework.Internals.Helpers;
using AirSoftAutomationFramework.Internals.NunitAtributes;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.Clients.ClientCard.FinancesTab;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.Clients.ClientCard.SATab;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.Clients.ClientPage;
using NUnit.Framework;
using TestsProject.TestsInternals;

namespace TestsProject.Tests.ClientCard
{
    [TestFixture]
    public class VerifyAvailableWithdrawalAndAvailableTransferToSaApi : TestSuitBase
    {
        #region Test Preparation
        private IApplicationFactory _apiFactory = new ApplicationFactory();
        private string _crmUrl = Config.appSettings.CrmUrl;
        private string _clientId;
        private string _clientEmail;
        private int _depositAmount = 10;
        private int _bonusAmount = 10;

        [SetUp]
        public void SetUp()
        {
            BeforeTest();
            #region PreCondition

            // create client 
            var clientName = TextManipulation.RandomString();
            _clientEmail = clientName + DataRep.EmailPrefix;

            _clientId = _apiFactory
                .ChangeContext<ICreateClientApi>()
                .CreateClientRequest(_crmUrl, clientName);

            // create deposit
            _apiFactory
                .ChangeContext<IFinancesTabApi>()
                .PostDepositRequest(_crmUrl, _clientId, _depositAmount);

            // create bonus
            _apiFactory
                .ChangeContext<IFinancesTabApi>()
                .PostBonusRequest(_crmUrl, _clientId, _bonusAmount);
            #endregion
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

        // create client
        // create deposit
        // create trade 
        // create cripto group with low spread
        // assign client to the cripto group
        // try to Withdrawal all the deposit amount
        // try to transfer to sa all the deposit amount
        // verify that when the trade is open you cant
        // Withdrawal the invest amount and the PNL
        // verify that when the trade is open you cant
        // Transfer To Sa the invest amount and the PNL700147
        [Test]
        [Description("based on jira : https://airsoftltd.atlassian.net/browse/AIRV2-4643")]
        [Category(DataRep.SanityCategory)]
        [Category(DataRep.RegressionCategory)]
        [RetryMenage(DataRep.NumOfRetriesForFallingTest)]
        public void VerifyAvailableWithdrawalAndAvailableTransferToSaApiTest()
        {
            var expecteAvailableWithdrawal = _depositAmount;
            var expecteAvailableTransferToSa = _depositAmount + _bonusAmount;

            var actualAvailableWithdrawal = _apiFactory
                .ChangeContext<IFinancesTabApi>()
                .GetAvailableWithdrawalByClientIdRequest(_crmUrl, _clientId)
                .GeneralResponse
                .AvailableWithdrawal;

            var actualAvailableTransferToSa = _apiFactory
                .ChangeContext<ISATabApi>()
                .GetSaBalanceByClientIdRequest(_crmUrl, _clientId)
                .AvailableDeposit;

            Assert.Multiple(() =>
            {
                Assert.True(actualAvailableWithdrawal == expecteAvailableWithdrawal,
                    $" expected Available Withdrawal : {expecteAvailableWithdrawal}" +
                    $" actual Available Withdrawal : {actualAvailableWithdrawal}");

                Assert.True(actualAvailableTransferToSa == expecteAvailableTransferToSa,
                    $" expected Transfer To Sa  : {expecteAvailableTransferToSa}" +
                    $" actual Transfer To Sa : {actualAvailableTransferToSa}");
            });
        }
    }
}