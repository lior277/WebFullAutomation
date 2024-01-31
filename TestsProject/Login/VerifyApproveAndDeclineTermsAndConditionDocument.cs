// Ignore Spelling: api TimeLine crm

using AirSoftAutomationFramework.Internals.DAL.ConfigurationManagement;
using AirSoftAutomationFramework.Internals.Helpers;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.Clients.ClientCard;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.Clients.ClientPage;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.Settings;
using AirSoftAutomationFramework.Objects.ForexObjects.TradingPlatform.TradeObjects.Ui;
using AirSoftAutomationFramework.Objects.ForexObjects.TradingPlatform.TradeObjects.Ui.Settings;
using AirSoftAutomationFramework.Objects.ForexObjects.Ui.Login;
using NUnit.Framework;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using AirSoftAutomationFramework.Internals.Factorys;
using AirSoftAutomationFramework.Internals.NunitAtributes;
using TestsProject.TestsInternals;

namespace TestsProject.Login
{
    [TestFixture(DataRep.Chrome), NonParallelizable]
    public class VerifyApproveAndDeclineTermsAndConditionDocument : TestSuitBase
    {
        #region Test Preparation
        public VerifyApproveAndDeclineTermsAndConditionDocument(string browser) : base(browser)
        {
            _browserName = browser;
        }

        private IApplicationFactory _apiFactory = new ApplicationFactory();
        private string _crmUrl = Config.appSettings.CrmUrl;
        private IWebDriver _driver;
        private string _clientId;
        //private string _currentUserApiKey;
        private string _dodName = "SignDodOnPlatformBankTransfer";
        private string _clientEmail;
        //private GetRegulationResponse _brandRegulation;
        private string _tradingPlatformUrl;
        private string _browserName;

        [SetUp]
        public void SetUp()
        {
            BeforeTest(_browserName);

            #region PreCondition
            _driver = GetDriver();

            _tradingPlatformUrl = Config.appSettings.tradingPlatformUrl;      

            // create client 
            var clientName = TextManipulation.RandomString();
            _clientEmail = clientName + DataRep.EmailPrefix;

            _clientId = _apiFactory
                .ChangeContext<ICreateClientApi>()
                .CreateClientRequest(_crmUrl, clientName);

            // create document
            var documentParams = new List<string> { "CLIENT_NAME" };

            var emailsParams = new Dictionary<string, string> {
                { "type", "terms_and_conditions" }, { "language", "en" },
                { "name", "terms_and_conditions" }};

            // create document body
            var documentBody = _apiFactory
                .ChangeContext<IPlatformTabApi>()
                .ComposeEmailBody(documentParams);

            // create document
            _apiFactory
                .ChangeContext<IPlatformTabApi>()
                .CreateTermsAndConditionPipe(_crmUrl, emailsParams, documentBody);

            // update Terms And Condition to true
            _apiFactory
                .ChangeContext<IPlatformTabApi>()
                .PutTermsAndConditionRequest(_crmUrl);
            #endregion
        }

        [TearDown]
        public void TearDown()
        {
            try
            {
                // delete dod if exist
                _apiFactory
                    .ChangeContext<IPlatformTabApi>()
                    .DeleteDod(_crmUrl, _dodName);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                // update Terms And Condition to false
                _apiFactory
                    .ChangeContext<IPlatformTabApi>()
                    .PutTermsAndConditionRequest(_crmUrl, false);

                AfterTest(_driver);
                DriverDispose(_driver);
            }
        }
        #endregion

        [Test]
        [Category(DataRep.SanityCategory)]
        [Category(DataRep.RegressionCategory)]
        [RetryMenage(DataRep.NumOfRetraiesForFallingTest)]
        public void VerifyApproveAndDeclineTermsAndConditionDocumentTest()
        {
            var expectedTimeLineDeclineType = "block_client_due_decline_terms_and_conditions";
            var expectedTimeLineSignedType = "sign_document";
            var expectedUploadMessage = "FILE_UPLOADED";

            // decline Terms And Condition Document
            _apiFactory
                .ChangeContext<ILoginPageUi>(_driver)
                .LoginPipe(_clientEmail, _tradingPlatformUrl)
                .ClickOnDeclineButton();

            // release the blocked client
            _apiFactory
                .ChangeContext<ISecurityTubApi>()
                .WaitForUserToGetBlocked(_crmUrl, _clientId)
                .PatchReleaseBlockUserRequest(_crmUrl, _clientId);
          
            // approve Terms And Condition Document
            _apiFactory
                .ChangeContext<ILoginPageUi>(_driver)
                .LoginPipe(_clientEmail, _tradingPlatformUrl)
                .ClickOnApproveButton()
                .ClickOnSignatureField()
                .ChangeContext<IDeclarationOfDepositUi>(_driver)
                .SetSignature()
                .ClickOnSaveSignatureButton()
                .ChangeContext<ITradePageUi>(_driver)
                .VerifyMessages(expectedUploadMessage);

            // checking here the timeline because its risky test
            var actualClientTimelineDetails = _apiFactory
                .ChangeContext<ITimeLineTabApi>()
                .GetTimelineRequest(_crmUrl, _clientId)
                .Select(p => p.type)
                .ToList();

            var actualUrl = _driver.Url;

            Assert.Multiple(() =>
            {
                Assert.True(actualUrl.Contains("trade.airsoftltd.com/trade"),
                    $" expected URL : trade.airsoftltd.com/trade" +
                    $" actual URL: {actualUrl}");

                Assert.True(actualClientTimelineDetails.Contains(expectedTimeLineDeclineType),
                    $" expected TimeLine Decline Type : {expectedTimeLineDeclineType}" +
                    $" actual TimeLine Decline Type: {actualClientTimelineDetails}");

                Assert.True(actualClientTimelineDetails.Contains(expectedTimeLineSignedType),
                    $" expected TimeLine Signed Type : {expectedTimeLineSignedType}" +
                    $" actual TimeLine Signed Type: {actualClientTimelineDetails}");
            });
        }
    }
}