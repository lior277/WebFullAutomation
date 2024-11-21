// Ignore Spelling: Api

using AirSoftAutomationFramework.Internals.DAL.ConfigurationManagement;
using AirSoftAutomationFramework.Internals.Factorys;
using AirSoftAutomationFramework.Internals.Helpers;
using AirSoftAutomationFramework.Internals.NunitAtributes;
using AirSoftAutomationFramework.Models;
using AirSoftAutomationFramework.Objects.DTOs;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.Clients.ClientCard.FinancesTab;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.Clients.ClientPage;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.Settings;
using AirSoftAutomationFramework.Objects.ForexObjects.ApiGeneral.Login;
using AirSoftAutomationFramework.Objects.ForexObjects.TradingPlatform.TradeObjects.TradeApi;
using NUnit.Framework;
using TestsProject.TestsInternals;

namespace TestsProject.Tests.Settings.GeneralTab
{
    [TestFixture]
    public class VerifyMinAndMaxDepositAmountLimitationApi : TestSuitBase
    {
        #region members
        private IApplicationFactory _apiFactory = new ApplicationFactory();
        private string _crmUrl = Config.appSettings.CrmUrl;
        private string _clientEmail;
        private QaAutomation01Context _dbContext = new QaAutomation01Context();
        private string _tradingPlatformUrl =  Config.appSettings.tradingPlatformUrl;
        private int _firstDepositAmount = 400;    
        private GetLoginResponse _loginData;
        #endregion      

        [SetUp]
        public void SetUp()
        {
            BeforeTest();
            #region PreCondition

            _apiFactory
                .ChangeContext<IGeneralTabApi>()
                .PutMinimumDepositRequest(_crmUrl, minDepositUsd: 5) ;

            // create client 
            var clientName = TextManipulation.RandomString();
            _clientEmail = clientName + DataRep.EmailPrefix;

            var clientId = _apiFactory
                .ChangeContext<ICreateClientApi>()
                .CreateClientRequest(_crmUrl, clientName);

            // get login data
            _loginData = _apiFactory
                .ChangeContext<ILoginApi>()
                .PostLoginToTradingPlatform(_tradingPlatformUrl, _clientEmail)
                .GeneralResponse;

            // client create deposit 
            _apiFactory
                .ChangeContext<IFinancesTabApi>()
                .PostDepositRequest(_crmUrl,
                clientId, _firstDepositAmount);
            #endregion

            // create psp for deposit page
            _apiFactory
              .ChangeContext<IPspTabApi>()
              .PostCreateAirsoftSandboxPspRequest(_crmUrl);          
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
        [RetryMenage(DataRep.NumOfRetriesForFallingTest)]
        public void VerifyMinAndMaxDepositAmountLimitationApiTest()
        {
            var overTheMaxDepositAmount = 500000;
            var BelowThemMinDepositAmount = 2;
            var expectedMaxDepositErrorMessage = $"The maximum amount is 1000 {DataRep.DefaultUSDCurrencyName}";
            var expectedMinDepositErrorMessage = $"The minimum amount is 5 {DataRep.DefaultUSDCurrencyName}";

            // create deposit higher then max deposit in settings(1000)
            var actualMaxDepositErrorMessage =  _apiFactory
                .ChangeContext<ITradeDepositPageApi>()
                .PostCreatePaymentRequestPipe(_tradingPlatformUrl,
                _loginData, overTheMaxDepositAmount, false)
                .Message;

            // create deposit lower then min deposit in settings(5)
            var actualMinDepositErrorMessage = _apiFactory
                .ChangeContext<ITradeDepositPageApi>()
                .PostCreatePaymentRequestPipe(_tradingPlatformUrl,
                _loginData, BelowThemMinDepositAmount, false)
                .Message;

            Assert.Multiple(() =>
            {
                Assert.True(actualMaxDepositErrorMessage == expectedMaxDepositErrorMessage,
                    $" expected Max Deposit Error Message: {expectedMaxDepositErrorMessage}" +
                    $" actual Max Deposit Error Message :  {actualMaxDepositErrorMessage}");

                Assert.True(actualMinDepositErrorMessage == expectedMinDepositErrorMessage,
                    $" expected Min Deposit Error Message: {expectedMinDepositErrorMessage}" +
                    $" actual Min Deposit Error Message :  {actualMinDepositErrorMessage}");
            });
        }
    }
}
