// Ignore Spelling: TimeLine

using AirSoftAutomationFramework.Internals.DAL.ConfigurationManagement;
using AirSoftAutomationFramework.Internals.ExtensionsMethods;
using AirSoftAutomationFramework.Internals.Helpers;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.AccountsApi.UsersMenuApi.UserMenuApi;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.Clients.ClientCard;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.Clients.ClientCard.FinancesTab;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.Clients.ClientCard.SATab;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.Clients.ClientPage;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.Settings;
using AirSoftAutomationFramework.Objects.ForexObjects.ApiGeneral;
using DocumentFormat.OpenXml.Wordprocessing;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using AirSoftAutomationFramework.Internals.Factorys;
using AirSoftAutomationFramework.Internals.NunitAtributes;
using TestsProject.TestsInternals;

namespace TestsProject.Tests.GlobalEventsAndExport.ExportClientCard
{
    [TestFixture]
    public class VerifyTimelineSheetOnExportClientCardFileApi : TestSuitBase
    {
        #region Test Preparation
        private IApplicationFactory _apiFactory = new ApplicationFactory();
        private string _crmUrl = Config.appSettings.CrmUrl;
        private string _currentUserApiKey;
        private string _userId;
        private int _depositAmount = 10000;
        private string _userName;
        private string _clientEmail;
        private string _clientName;
        private string _clientId;
        private Stream _clientCardStream;
        private int _transferToSavingAccountAmount = 1;

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

            // create deposit 
            _apiFactory
                .ChangeContext<IFinancesTabApi>()
                .PostDepositRequest(_crmUrl, _clientId, _depositAmount,
                apiKey: _currentUserApiKey);

            // Transfer from balance to Saving Account
            _apiFactory
                .ChangeContext<ISATabApi>()
                .PostTransferToSavingAccountRequest(_crmUrl,
                _clientId, _transferToSavingAccountAmount,
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

            #region  export Client Card
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
        public void VerifyTimelineSheetOnExportClientCardFileApiTest()
        {
            var expectedCurrency = DataRep.DefaultUSDCurrencyName;
            var expectedDate = DateTime.Now.ToString("yyyy-MM-dd");
            var requestedExportMessage = $"{_userName} requested export for client";


            var saToBalanceMessage = $"{_userName}" +
                $" Move {_transferToSavingAccountAmount}.00" +
                $" {expectedCurrency} From Balance to SA";

            var firstDepositMessage = "System Has sent email first_deposit to client";
            var depositMessage = $"{_userName} Added Deposit of 10000.00 USD";
            var registeredMessage = $"{_userName} registered from API";

            var expectedMessagesList = new List<string>() { requestedExportMessage,
                saToBalanceMessage, firstDepositMessage, depositMessage, registeredMessage };

            var dataSet = _apiFactory
                .ChangeContext<IFileHandler>()
                .ConvertXlsxStreamToDataSet(_clientCardStream);

            // timeline table data
            var timelineTable = _apiFactory
                .ChangeContext<IFileHandler>()
                .ReadDataFromDataSetNew(dataSet, "TimeLine");

            var actualDatesList = timelineTable
                .Select(x => x["Date"])
                .ToList();

            var actualMessagesList = timelineTable
                .Select(x => x["Message"])
                .ToList();

            actualMessagesList.RemoveAll(p => p.Equals("System Has sent email admin_deposit to client"));

            var compareTwoListOfString =
                actualMessagesList.CompareTwoListOfString(expectedMessagesList);

            Assert.Multiple(() =>
            {
                Assert.True(actualDatesList.All(p => p.Contains(expectedDate)),
                    $" actual Dates : {actualDatesList.ListToString()}" +
                    $" expected Dates: {expectedDate}");

                Assert.True(compareTwoListOfString.Count == 0,
                    $" actual timeline Message : {compareTwoListOfString.ListToString()}" +
                    $" expected Messages List: {expectedMessagesList.ListToString()}");
            });
        }
    }
}
