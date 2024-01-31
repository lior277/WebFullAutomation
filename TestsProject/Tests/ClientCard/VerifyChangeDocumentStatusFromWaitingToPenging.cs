// Ignore Spelling: Penging

using System;
using System.Linq;
using System.Threading;
using AirSoftAutomationFramework.Internals.DAL.ConfigurationManagement;
using AirSoftAutomationFramework.Internals.Factorys;
using AirSoftAutomationFramework.Internals.Helpers;
using AirSoftAutomationFramework.Internals.NunitAtributes;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.AccountsApi.Roles;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.AccountsApi.UsersMenuApi.UserMenuApi;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.Clients.ClientCard.DocumentsAndFilesTab;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.Clients.ClientPage;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.Settings;
using AirSoftAutomationFramework.Objects.ForexObjects.TradingPlatform.TradeObjects.Ui;
using AirSoftAutomationFramework.Objects.ForexObjects.Ui.Crm.ClientsPage;
using AirSoftAutomationFramework.Objects.ForexObjects.Ui.Crm.ClientsPage.ClientCard;
using AirSoftAutomationFramework.Objects.ForexObjects.Ui.Login;
using AirSoftAutomationFramework.Objects.ForexObjects.Ui.UiGeneral;
using NUnit.Framework;
using OpenQA.Selenium;
using TestsProject.TestsInternals;

namespace TestsProject.Tests.ClientCard
{
    [TestFixture(DataRep.Chrome)]
    public class VerifyChangeDocumentStatusFromWaitingToPenging : TestSuitBase
    {
        #region Test Preparation
        public VerifyChangeDocumentStatusFromWaitingToPenging(string browser) : base(browser)
        {
            _browserName = browser;
        }

        private IApplicationFactory _apiFactory = new ApplicationFactory();
        private string _crmUrl = Config.appSettings.CrmUrl;
        private IWebDriver _driver;
        private string _browserName;
        private string _userName;
        private string _userEmail;
        private string _clientEmail;
        private string _clientId;
        private string _currentUserApiKey;
        private string _expectedDocumentName = "Privacy Policy";

        [SetUp]
        public void SetUp()
        {
            BeforeTest(_browserName);
            #region PreCondition
            _driver = GetDriver();
            var tradingPlatformUrl = Config.appSettings.tradingPlatformUrl;     

            #region create user
            // create user
            _userName = TextManipulation.RandomString();
            _userEmail = _userName + DataRep.EmailPrefix;

            // create user
            var userId = _apiFactory
             .ChangeContext<IUserApi>()
             .PostCreateUserRequest(_crmUrl,
             _userName, role: DataRep.AdminWithUsersOnlyRoleName);
            #endregion

            // create ApiKey
            _currentUserApiKey = _apiFactory
                .ChangeContext<IUserApi>(_driver)
                .PostCreateApiKeyRequest(_crmUrl, userId);

            // create client 
            var clientName = TextManipulation.RandomString();
            _clientEmail = clientName + DataRep.EmailPrefix;

            _clientId = _apiFactory
                .ChangeContext<ICreateClientApi>()
                .CreateClientRequest(_crmUrl, clientName,
                apiKey: _currentUserApiKey);

            _apiFactory
                .ChangeContext<IPlatformTabApi>(_driver)
                .ChangeDocumentStatusPipe(_crmUrl, _expectedDocumentName);

            // login
            _apiFactory
                .ChangeContext<ILoginPageUi>(_driver)
                .LoginPipe(_clientEmail, tradingPlatformUrl);
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

        #region test description
        // create user 
        // create client
        // set privacy policy document as active
        // login to TP
        // click on documents
        // click on status waiting on privacy policy
        // wait for status to change to pending
        // login to crm navigate to client card document&files tab
        // click on documents 
        // verify document status pending
        // click on files
        // verify file titles
        // click on download
        // verify status 200 for the download request
        #endregion

        [Test]
        [Category(DataRep.SanityCategory)]
        [Category(DataRep.RegressionCategory)]
        [RetryMenage(DataRep.NumOfRetraiesForFallingTest)]
        public void VerifyChangeDocumentStatusFromWaitingToPengingTest()
        {
            var documentsSection = "Documents";
            var documentStatusAfterSign = "Pending";
            var filesSection = "Files";
            var expectedDocumentLastModifiedDate = DateTime.Now.ToString("yyy-MM-dd");
            var expectedFileLastModifiedDate = DateTime.Now.ToString("dd.MM.yy");
            var expectedFileName = "Privacy Policy_document";

            var actualDocumentTitlesBeforeSign = _apiFactory
                .ChangeContext<IMenus>(_driver)
                .ClickOnMenuItem<ITradePageUi>(DataRep.DocumentsMenuItem)
                .GetDocumentTitles(_expectedDocumentName);

            _apiFactory
                .ChangeContext<ITradePageUi>(_driver)
                .ClickOnDocumentStatus(_expectedDocumentName)
                .ClickOnDocumentPreview()
                .SetSignature()
                .ClickOnSaveSignatureForDocumentButton()
                .VerifyNewDocumentStatus(_expectedDocumentName,
                documentStatusAfterSign);

            _apiFactory
               .ChangeContext<ILoginPageUi>(_driver)
               .LoginPipe(_userName, _crmUrl)
               .ChangeContext<IUserApi>(_driver)
               .PatchAddFingerPrintPipe(_crmUrl, _userEmail);

            _apiFactory
                .ChangeContext<ILoginPageUi>(_driver)
                .LoginPipe(_userName);

            Thread.Sleep(1000);

            var actualDocumentTitles = _apiFactory
                .ChangeContext<IMenus>(_driver)
                .ClickOnMenuItem<IClientsPageUi>()
                .SearchClientByEmail(_clientEmail)
                .ClickOnClientFullName()
                .ClickOnDocumentsTab()
                .ClickOnSectionByTitle(documentsSection)
                .GetDocumentTitles(_expectedDocumentName, documentStatusAfterSign);

            var actualFileTitles = _apiFactory
                .ChangeContext<IDocumentsTabUI>(_driver)
                .ClickOnSectionByTitle(filesSection)
                .GetFileTitles(_expectedDocumentName);

            var fileName = $"{actualFileTitles.Split("pdf").First()}pdf";

            _apiFactory
                .ChangeContext<IDocumentsAndFilesTabApi>(_driver)
                .VerifyFileDonloaded(_crmUrl, _clientId, fileName, _currentUserApiKey);

            Assert.Multiple(() =>
            {
                Assert.True(actualDocumentTitles["actualDocumentName"] == _expectedDocumentName,
                    $" expected Document Name: {_expectedDocumentName}" +
                    $" actual Document Name: {actualDocumentTitles["actualDocumentName"]}");

                Assert.True(actualDocumentTitles["actualDocumentStatus"] == documentStatusAfterSign.ToLower(),
                    $" expected document Status After Sign: {documentStatusAfterSign.ToLower()}" +
                    $" actual  document Status After Sign: {actualDocumentTitles["actualDocumentStatus"]}");

                Assert.True(actualDocumentTitles["documentIp"] != null,
                    $" expected document Ip: {documentStatusAfterSign.ToLower()} not null" +
                    $" actual document Ip: {actualDocumentTitles["documentIp"]}");

                Assert.True(actualDocumentTitles["documentLastModified"].Contains(expectedDocumentLastModifiedDate),
                    $" expected document Last Modified: {expectedDocumentLastModifiedDate}" +
                    $" actual  document Last Modified: {actualDocumentTitles["documentLastModified"]}");

                Assert.True(actualFileTitles.Contains(expectedFileLastModifiedDate),
                    $" expected File Last Modified Date: {expectedFileLastModifiedDate} not null" +
                    $" actual File Last Modified Date: {actualFileTitles}");

                Assert.True(actualFileTitles.Contains(expectedFileName),
                    $" expected File Name: {expectedFileName}" +
                    $" actual File Name: {actualFileTitles}");
            });
        }
    }
}