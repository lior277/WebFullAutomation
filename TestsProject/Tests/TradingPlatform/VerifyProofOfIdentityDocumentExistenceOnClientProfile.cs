using AirSoftAutomationFramework.Internals.DAL.ConfigurationManagement;
using AirSoftAutomationFramework.Internals.Factory;
using AirSoftAutomationFramework.Internals.Helpers;
using AirSoftAutomationFramework.Internals.NunitAttributes;
using AirSoftAutomationFramework.Objects.DTOs;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.Clients.ClientPage;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.Settings;
using AirSoftAutomationFramework.Objects.ForexObjects.TradingPlatform.TradeObjects.Ui;
using AirSoftAutomationFramework.Objects.ForexObjects.Ui.Login;
using NUnit.Framework;
using OpenQA.Selenium;
using System;
using TestsProject.TestsInternals;

namespace TestsProject.Tests.TradingPlatform
{
    [TestFixture(DataRep.Chrome)]
    public class VerifyProofOfIdentityDocumentExistenceOnClientProfile : TestSuitBase
    {
        #region Test Preparation
        public VerifyProofOfIdentityDocumentExistenceOnClientProfile(string browser) : base(browser)
        {
            _browserName = browser;
        }

        private IApplicationFactory _apiFactory = new ApplicationFactory();
        private string _crmUrl = Config.appSettings.CrmUrl;
        private string _clientEmail;
        private string _tradingPlatformUrl =  Config.appSettings.tradingPlatformUrl;
        private IWebDriver _driver;
        private GetRegulationResponse _generalSettingData;
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
            _generalSettingData = _apiFactory
                .ChangeContext<ISuperAdminTubApi>()
                .GetRegulationRequest(_crmUrl);

            _generalSettingData.edit_client_profile.edit_doc_parts.proof_of_identity = true;

            _apiFactory
                .ChangeContext<IGeneralTabApi>(_driver)
                .PutGeneralSettingsRequest(_crmUrl, _generalSettingData);
            #endregion
        }

        [TearDown]
        public void TearDown()
        {
            try
            {
                _apiFactory
                   .ChangeContext<IGeneralTabApi>(_driver)
                   .PutGeneralSettingsRequest(_crmUrl, _generalSettingData);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                AfterTest(_driver);
                DriverDispose(_driver);
            }
        }

        #endregion
        // create client
        // Put General Settings - doc section for proof of identity true 
        // login => client profile
        // verify proof of identity exist
        // Put General Settings - doc section for proof of identity false
        // verify proof of identity not exist
        [Test]
        [Category(DataRep.SanityCategory)]
        [Category(DataRep.RegressionCategory)]
        [RetryMenage(DataRep.NumOfRetraiesForFallingTest)]
        public void VerifyProofOfIdentityDocumentExistenceOnClientProfileTest()
        {
            var actualStatusOfProofOfIdentitySectionDisplay = _apiFactory
                .ChangeContext<ILoginPageUi>(_driver)
                .LoginPipe(_clientEmail, _tradingPlatformUrl)
                .ChangeContext<ITradePageUi>(_driver)
                .ClickOnClientFirstName()
                .CheckIfProofOfIdentitySectionExist(1);

            _generalSettingData.
                edit_client_profile.edit_doc_parts.proof_of_identity = false;

            _apiFactory
                .ChangeContext<IGeneralTabApi>(_driver)
                .PutGeneralSettingsRequest(_crmUrl, _generalSettingData);

            _driver.Navigate().Refresh();

            var actualStatusOfProofOfIdentitySectionNotDisplay = _apiFactory
                .ChangeContext<IProfilePageUi>(_driver)                             
                .CheckIfProofOfIdentitySectionExist(0);

            Assert.Multiple(() =>
            {
                Assert.True(actualStatusOfProofOfIdentitySectionDisplay,
                          $" expected actual Proof Of Identity Section Display: true" +
                          $" actual expected actual Proof Of Identity Section Display: {actualStatusOfProofOfIdentitySectionDisplay}");

                Assert.True(actualStatusOfProofOfIdentitySectionNotDisplay,
                          $" expected actual Proof Of Identity Section not Display: true" +
                          $" actual expected actual Proof Of Identity Section not Display: {actualStatusOfProofOfIdentitySectionNotDisplay}");
            });
        }
    }
}