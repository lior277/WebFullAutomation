using System;
using System.IO;
using AirSoftAutomationFramework.Internals.DAL.ConfigurationManagement;
using AirSoftAutomationFramework.Internals.ExtensionsMethods;
using AirSoftAutomationFramework.Internals.Factorys;
using AirSoftAutomationFramework.Internals.Helpers;
using AirSoftAutomationFramework.Internals.NunitAtributes;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.AccountsApi.UsersMenuApi.UserMenuApi;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.Banking;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.Clients.ClientCard;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.Clients.ClientPage;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.Settings;
using AirSoftAutomationFramework.Objects.ForexObjects.ApiGeneral;
using AirSoftAutomationFramework.Objects.ForexObjects.ApiGeneral.Login;
using NUnit.Framework;
using OpenQA.Selenium;
using TestsProject.TestsInternals;

namespace TestsProject.Tests.GlobalEventsAndExport.ExportClientCard
{
    [TestFixture]
    public class VerifyCommentsSheetOnExportClientCardFile : TestSuitBase
    {
        #region Test Preparation
        private IApplicationFactory _apiFactory = new ApplicationFactory();
        private string _crmUrl = Config.appSettings.CrmUrl;
        private string _currentUserApiKey;
        private string _userId;
        private string _userName;
        private string _clientEmail;
        private string _clientName;
        private string _clientId;
        private Stream _clientCardStream;

        [SetUp]
        public void SetUp()
        {
            BeforeTest();

            // create user
            _userName = TextManipulation.RandomString();
            var testimEmailPerfix = DataRep.TestimEmailPrefix;
            var userEmail = _userName + testimEmailPerfix;

            // create user
            _userId = _apiFactory
                .ChangeContext<IUserApi>()
                .PostCreateUserRequest(_crmUrl, _userName,
                emailPrefix: testimEmailPerfix,
                role: DataRep.AdminWithUsersOnlyRoleName);

            #region create ApiKey
            // create ApiKey
            _currentUserApiKey = _apiFactory
                .ChangeContext<IUserApi>()
                .PostCreateApiKeyRequest(_crmUrl, _userId);
            #endregion

            // create client 
            _clientName = TextManipulation.RandomString();
            _clientEmail = _clientName + DataRep.EmailPrefix;

            _clientId = _apiFactory
                .ChangeContext<ICreateClientApi>()
                .CreateClientRequest(_crmUrl, _clientName,
                apiKey: _currentUserApiKey);

            // create comment    
            _apiFactory
                .ChangeContext<ICommentsTabApi>()
                .PostCommentRequest(_crmUrl, _clientId,
                apiKey: _currentUserApiKey);

            // enter the Email For Export in Super Admin Tub
            var brandRegulation = _apiFactory
               .ChangeContext<ISuperAdminTubApi>()
               .GetBrandRegulationRequest(_crmUrl);

            DataRep.EmailListForExport.Add(userEmail);

            brandRegulation.export_data_email_url = DataRep
                .EmailListForExport.ToArray();

            _apiFactory
                .ChangeContext<ISuperAdminTubApi>()
                .PutRegulationsRequest(_crmUrl, brandRegulation);

            #region  export comments tab
            // export comments tab
            _apiFactory
                .ChangeContext<IClientCardApi>()
                .ExportClientCardPipe(_crmUrl, _clientId,
                userEmail, _currentUserApiKey);

            // extract the export link from mail body
            var exportLink = _apiFactory
                .ChangeContext<ISharedStepsGenerator>()
                .GetExportLinkFromExportEmailBody(_crmUrl, userEmail, "export_link");

            // get the csv file 
            _clientCardStream = _apiFactory
                .ChangeContext<IClientCardApi>()
                .GetExportClientCardRequest(_crmUrl, exportLink, _currentUserApiKey)
                .Stream;
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
        [RetryMenage(DataRep.NumOfRetriesForFallingTest)]
        public void VerifyCommentsSheetOnExportClientCardFileTest()
        {
            var expectedComment = "automation comment"; // this is the default comment
            var expectedDate = DateTime.Now.ToString("yyyy-MM-dd");
            var expectedAgentName = $"{_userName.UpperCaseFirstLetter()}" +
                $" {_userName.UpperCaseFirstLetter()}";

            var dataSet = _apiFactory
                .ChangeContext<IFileHandler>()
                .ConvertXlsxStreamToDataSet(_clientCardStream);

            // comments table data
            var commentsTable = _apiFactory
                .ChangeContext<IFileHandler>()
                .ReadDataFromDataSet(dataSet, "comments");
         
            Assert.Multiple(() =>
            {
                Assert.True(commentsTable["Agent name"] == expectedAgentName,
                    $" actual client Agent name : {commentsTable["Agent name"]}" +
                    $" expected Agent name: {expectedAgentName}");

                Assert.True(commentsTable["Planning time"].Contains(expectedDate),
                    $" actual  Planning time : {commentsTable["Planning time"]}" +
                    $" expected  Planning time: {expectedDate}");

                Assert.True(commentsTable["Comment"] == expectedComment,
                    $" actual Comment : {commentsTable["Comment"]}" +
                    $" expected Comment: {expectedComment}");

                Assert.True(commentsTable["Created at"].Contains(expectedDate),
                    $" actual  Created at : {commentsTable["Created at"]}" +
                    $" expected  Created at: {expectedDate}");

                Assert.True(commentsTable["Username"] == _userName.UpperCaseFirstLetter(),
                    $" actual  user name : {commentsTable["Username"]}" +
                    $" expected  user name: {_userName}");          
            });
        }
    }
}