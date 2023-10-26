using AirSoftAutomationFramework.Internals.DAL.ConfigurationManagement;
using AirSoftAutomationFramework.Internals.Factory;
using AirSoftAutomationFramework.Internals.Helpers;
using AirSoftAutomationFramework.Internals.NunitAttributes;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.AccountsApi.UsersMenuApi.UserMenuApi;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.Clients.ClientCard.FinancesTab;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.Clients.ClientPage;
using NUnit.Framework;
using System.Linq;
using TestsProject.TestsInternals;

namespace TestsProject.Tests.ClientCard
{
    [TestFixture]
    public class VerifyCreateDepositWithWooCommerceAgent : TestSuitBase
    {
        #region Members
        private IApplicationFactory _apiFactory = new ApplicationFactory();
        private string _crmUrl = Config.appSettings.CrmUrl;
        private string _clientId;
        private int _depositAmount = 1000;
        #endregion        

        [SetUp]
        public void SetUp()
        {
            #region Test Preparation
            BeforeTest();

            // create wooCommerce user
            var userId = _apiFactory
                .ChangeContext<IUserApi>()
                .PostCreateWooCommerceUserRequest(_crmUrl);

            // create client 
            var clientName = TextManipulation.RandomString();

            _clientId = _apiFactory
                .ChangeContext<ICreateClientApi>()
                .CreateClientRequest(_crmUrl, clientName);

            // create Deposit With WooCommerce User
            _apiFactory
                .ChangeContext<IFinancesTabApi>()
                .PostCreateDepositWithWooCommerceUserRequest(_crmUrl, clientName,
                _depositAmount, DataRep.ApiKeyOfWooCommerceUser);
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

        [Test]
        [Category(DataRep.SanityCategory)]
        [Category(DataRep.RegressionCategory)]
        [RetryMenage(DataRep.NumOfRetraiesForFallingTest)]
        public void VerifyCreateDepositWithWooCommerceAgentTest()
        {
            var expectedPspTransactionId = "Beginner courses - woocommerce-1572"; // default values in the request;

            var actualFinanceData = _apiFactory
                .ChangeContext<IFinancesTabApi>()
                .GetFinanceDataRequest(_crmUrl, _clientId)
                .GeneralResponse
                .FirstOrDefault();


            Assert.Multiple(() =>
            {
                Assert.True(actualFinanceData.amount == _depositAmount,
                    $" expected amount: {_depositAmount}," +
                    $" actual amount: {actualFinanceData.amount}");

                Assert.True(actualFinanceData.original_currency == "USD",
                    $" expected original_currency: USD," +
                    $" actual original_currency: {actualFinanceData.original_currency}");

                Assert.True(actualFinanceData.psp_transaction_id == expectedPspTransactionId,
                    $" expected psp transaction id: {expectedPspTransactionId}," +
                    $" actual psp transaction id: {expectedPspTransactionId}");

                Assert.True(actualFinanceData.method == "Other psp: bacs",
                    $" expected method: Other psp: bacs," +
                    $" actual method: {actualFinanceData.method}");

                Assert.True(actualFinanceData.status == "approved",
                    $" expected status: approved," +
                    $" actual status: approved");

                Assert.True(actualFinanceData.type == "deposit",
                   $" expected type: {actualFinanceData.type}," +
                   $" actual type: deposit");
            });
        }
    }
}