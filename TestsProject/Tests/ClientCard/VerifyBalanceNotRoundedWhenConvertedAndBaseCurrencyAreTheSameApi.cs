// Ignore Spelling: Api

using AirSoftAutomationFramework.Internals.DAL.ConfigurationManagement;
using AirSoftAutomationFramework.Internals.Helpers;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.Clients.ClientPage;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.Settings;
using AirSoftAutomationFramework.Objects.ForexObjects.ApiGeneral.Login;
using AirSoftAutomationFramework.Objects.ForexObjects.TradingPlatform.TradeObjects.TradeApi;
using NUnit.Framework;
using System.Linq;
using AirSoftAutomationFramework.Internals.Factorys;
using AirSoftAutomationFramework.Internals.NunitAtributes;
using TestsProject.TestsInternals;

namespace TestsProject.Tests.ClientCard
{
    [TestFixture]
    public class VerifyBalanceNotRoundedWhenConvertedAndBaseCurrencyAreTheSameApi : TestSuitBase
    {
        #region Test Preparation       
        private IApplicationFactory _apiFactory = new ApplicationFactory();
        private string _crmUrl = Config.appSettings.CrmUrl;
        private string _tradingPlatformUrl =
             Config.appSettings.tradingPlatformUrl;

        private string _clientEmail;
        private string _clientId;
        private double _depositAmount = 10.23;

        public VerifyBalanceNotRoundedWhenConvertedAndBaseCurrencyAreTheSameApi() { }

        [SetUp]
        public void SetUp()
        {
            #region PreCondition
            BeforeTest();
            var currency = DataRep.DefaultUSDCurrencyName;

            // create client
            var clientName = TextManipulation.RandomString();
            _clientEmail = clientName + DataRep.EmailPrefix;

            _clientId = _apiFactory
                .ChangeContext<ICreateClientApi>()
                .CreateClientRequest(_crmUrl, clientName,
                currency: currency);

            // get login data
            var loginData = _apiFactory
                .ChangeContext<ILoginApi>()
                .PostLoginToTradingPlatform(_tradingPlatformUrl, _clientEmail)
                .GeneralResponse;

            // create pending deposit
            _apiFactory
                .ChangeContext<ITradeDepositPageApi>()
                .PostCreatePaymentRequestPipe(_tradingPlatformUrl,
                loginData, depositAmount: _depositAmount);

            _apiFactory
                .ChangeContext<IPspTabApi>()
                .PostPaymentNotificationRequestPipe(_crmUrl,
                _clientId, _depositAmount, currency);

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

        [Test]
        [Description("based on https://airsoftltd.atlassian.net/browse/AIRV2-5003")]
        [Category(DataRep.SanityCategory)]
        [Category(DataRep.RegressionCategory)]
        [RetryMenage(DataRep.NumOfRetraiesForFallingTest)]
        public void VerifyBalanceNotRoundedWhenConvertedAndBaseCurrencyAreTheSameApiTest()
        {
            // based on that the airsoft psp has converted to USD value
            var actualClientCardBalance = _apiFactory
                .ChangeContext<IClientsApi>()
                .GetClientByIdRequest(_crmUrl, _clientId)
                .GeneralResponse
                .user
                .balance;

            Assert.Multiple(() =>
            {
                Assert.IsTrue(actualClientCardBalance == _depositAmount,
                    $" actual balance from client card :{actualClientCardBalance}" +
                    $" expected  balance from client card: {_depositAmount}");
            });
        }
    }
}