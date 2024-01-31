// Ignore Spelling: Api

using AirSoftAutomationFramework.Internals.DAL.ConfigurationManagement;
using AirSoftAutomationFramework.Internals.ExtensionsMethods;
using AirSoftAutomationFramework.Internals.Factorys;
using AirSoftAutomationFramework.Internals.Helpers;
using AirSoftAutomationFramework.Internals.NunitAtributes;
using AirSoftAutomationFramework.Objects.DTOs;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.Clients.ClientPage;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.Settings;
using AirSoftAutomationFramework.Objects.ForexObjects.ApiGeneral.Login;
using AirSoftAutomationFramework.Objects.ForexObjects.TradingPlatform.TradeObjects.TradeApi.Profile;
using NUnit.Framework;
using TestsProject.TestsInternals;

namespace TestsProject.Tests.TradingPlatform.ClientProfile
{
    [TestFixture]
    public class VerifyDownloadGeneralDodFileOnClientProfileApi : TestSuitBase
    {
        #region Test Preparation
        private IApplicationFactory _apiFactory = new ApplicationFactory();
        private string _crmUrl = Config.appSettings.CrmUrl;
        private GetLoginResponse _loginData;
        private string _tradingPlatformUrl = Config.appSettings.tradingPlatformUrl;

        [SetUp]
        public void SetUp()
        {
            BeforeTest();
            #region PreCondition
            // create client 
            var clientName = TextManipulation.RandomString();
            var clientEmail = clientName + DataRep.EmailPrefix;

            _apiFactory
                .ChangeContext<ICreateClientApi>()
                .CreateClientRequest(_crmUrl, clientName);

            // get login Data for traderading Platform
            _loginData = _apiFactory
                .ChangeContext<ILoginApi>()
                .PostLoginToTradingPlatform(_tradingPlatformUrl, clientEmail)
                .GeneralResponse;

            _apiFactory
                .ChangeContext<IGeneralTabApi>()
                .PutGeneralDodRequest(_crmUrl);
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
        [Category(DataRep.SanityCategory)]
        [Category(DataRep.RegressionCategory)]
        [RetryMenage(DataRep.NumOfRetraiesForFallingTest)]
        public void VerifyDownloadGeneralDodFileOnClientProfileApiTest()
        {
            var expectedPdfText = "Pdf for automation";

            var actualPdfText = _apiFactory
                .ChangeContext<IProfilePageApi>()
                .GetGeneralDtoRequest(_tradingPlatformUrl, _loginData)
                .GetPdfFileText();

            Assert.Multiple(() =>
            {
                Assert.True(actualPdfText == expectedPdfText,
                    $" expected Pdf Text: {expectedPdfText}" +
                    $" actual Pdf Text: {actualPdfText}");
            });
        }
    }
}