// Ignore Spelling: Api

using AirSoftAutomationFramework.Internals.DAL.ConfigurationManagement;
using AirSoftAutomationFramework.Internals.Factory;
using AirSoftAutomationFramework.Internals.Helpers;
using AirSoftAutomationFramework.Internals.NunitAttributes;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.Clients.ClientCard;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.Clients.ClientPage;
using AirSoftAutomationFramework.Objects.ForexObjects.ApiGeneral.Login;
using AirSoftAutomationFramework.Objects.ForexObjects.TradingPlatform.TradeObjects.TradeApi.Profile;
using NUnit.Framework;
using TestsProject.TestsInternals;

namespace TestsProject.Tests.ClientCard
{
    [TestFixture]
    public class VerifyGeneralDodPendingAfterUploadApi : TestSuitBase
    {
        #region Test Preparation
        private IApplicationFactory _apiFactory = new ApplicationFactory();
        private string _crmUrl = Config.appSettings.CrmUrl;
        private string _clientId;

        [SetUp]
        public void SetUp()
        {
            BeforeTest();
            #region PreCondition
            var tradingPlatformUrl =
                 Config.appSettings.tradingPlatformUrl;

            // create client 
            var clientName = TextManipulation.RandomString();
            var clientEmail = clientName + DataRep.EmailPrefix;

            _clientId = _apiFactory
                .ChangeContext<ICreateClientApi>()
                .CreateClientRequest(_crmUrl, clientName);

            // get login Data for trading Platform
            var loginData = _apiFactory
                 .ChangeContext<ILoginApi>()
                 .PostLoginToTradingPlatform(tradingPlatformUrl, clientEmail)
                 .GeneralResponse;

            #region upload file notification 
            //upload file notification
            _apiFactory
                .ChangeContext<IProfilePageApi>()
                .PatchKycFileRequest(tradingPlatformUrl,
                DataRep.FileNameToUpload, "dod", loginData);
            #endregion
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
        [Description("based on jira https://airsoftltd.atlassian.net/browse/AIRV2-4581")]
        [Category(DataRep.SanityCategory)]
        [Category(DataRep.RegressionCategory)]
        [RetryMenage(DataRep.NumOfRetraiesForFallingTest)]
        public void VerifyGeneralDodPendingAfterUploadApiTest()
        {
            var expectedGeneralDodStatus = "Pending";

            var actualGeneralDodStatus = _apiFactory
                .ChangeContext<IClientsApi>()
                .GetClientByIdRequest(_crmUrl, _clientId)
                .GeneralResponse
                .user
                .dod_status;

            Assert.True(actualGeneralDodStatus == expectedGeneralDodStatus,
                $" actual General Dod Status : {actualGeneralDodStatus} " +
                $" expected General Dod Status: {expectedGeneralDodStatus}");
        }
    }
}