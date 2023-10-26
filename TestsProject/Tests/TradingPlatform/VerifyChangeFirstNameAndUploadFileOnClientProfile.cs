using AirSoftAutomationFramework.Internals.DAL.ConfigurationManagement;
using AirSoftAutomationFramework.Internals.Factory;
using AirSoftAutomationFramework.Internals.Helpers;
using AirSoftAutomationFramework.Internals.NunitAttributes;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.Clients.ClientPage;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.Settings;
using AirSoftAutomationFramework.Objects.ForexObjects.TradingPlatform.TradeObjects.Ui;
using AirSoftAutomationFramework.Objects.ForexObjects.Ui.Login;
using NUnit.Framework;
using OpenQA.Selenium;
using TestsProject.TestsInternals;

namespace TestsProject.Tests.TradingPlatform
{
    [TestFixture(DataRep.Chrome)]
    public class VerifyChangeFirstNameAndUploadFileOnClientProfile : TestSuitBase
    {
        #region Test Preparation
        public VerifyChangeFirstNameAndUploadFileOnClientProfile(string browser) : base(browser)
        {
            _browserName = browser;
        }

        private IApplicationFactory _apiFactory = new ApplicationFactory();
        private string _crmUrl = Config.appSettings.CrmUrl;
        private string _clientEmail;
        private string _tradingPlatformUrl =  Config.appSettings.tradingPlatformUrl;
        private IWebDriver _driver;
        private string _browserName;

        [SetUp]
        public void SetUp()
        {
            BeforeTest(_browserName);
            #region PreCondition
            _driver = GetDriver();

            // create client 
            var clientName = TextManipulation.RandomString();
            _clientEmail = clientName + DataRep.EmailPrefix;

             _apiFactory
                .ChangeContext<ICreateClientApi>()
                .CreateClientRequest(_crmUrl, clientName);

            // Get general settings data
            var generalSettingData = _apiFactory
                .ChangeContext<ISuperAdminTubApi>()
                .GetRegulationRequest(_crmUrl);

            generalSettingData.edit_client_profile.edit_doc_parts.proof_of_identity = true;

            _apiFactory
                .ChangeContext<IGeneralTabApi>(_driver)
                .PutGeneralSettingsRequest(_crmUrl, generalSettingData);
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
                AfterTest(_driver);
                DriverDispose(_driver);
            }
        }
        #endregion

        // create client
        // Upload Kyc File("identity_proof")
        // Verify Messages("File uploaded.")
        // Verify Pending Status
        // Click On First Name Pencil
        // Set First Name(expectedNewFirstName)
        // Verify first name changed
        [Test]
        [Description("based on jira https://airsoftltd.atlassian.net/browse/AIRV2-4535")]
        [Category(DataRep.SanityCategory)]
        [Category(DataRep.RegressionCategory)]
        [RetryMenage(DataRep.NumOfRetraiesForFallingTest)]
        public void VerifyChangeFirstNameAndUploadFileOnClientProfileTest()
        {
            var expectedNewFirstName = "Newname";

            var actualPrifileName = _apiFactory
                .ChangeContext<ILoginPageUi>(_driver)
                .LoginPipe(_clientEmail, _tradingPlatformUrl)
                .ChangeContext<ITradePageUi>(_driver)
                .ClickOnClientFirstName()
                .UploadKycFile()
                .VerifyMessages("File uploaded.")
                .VerifyPendingStatus()
                .ClickOnFirstNamePencil()
                .SetFirstName(expectedNewFirstName)
                .ClickOnSaveProfilebutton()
                .VerifyMessages("Personal information was saved successfully.")
                .GetProfileName(expectedNewFirstName);

            Assert.Multiple(() =>
            {
                Assert.True(expectedNewFirstName == actualPrifileName,
                    $" expected New First Name: {expectedNewFirstName}" +
                    $" actual New First Name: {actualPrifileName}");
            });
        }
    }
}