using System;
using System.IO;
using AirSoftAutomationFramework.Internals.DAL.ConfigurationManagement;
using AirSoftAutomationFramework.Internals.Factorys;
using AirSoftAutomationFramework.Internals.Helpers;
using AirSoftAutomationFramework.Internals.NunitAtributes;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.AccountsApi.UsersMenuApi.UserMenuApi;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.Clients.ClientCard;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.Clients.ClientCard.FinancesTab;
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
    public class VerifyFinancesSheetOnExportClientCardFile : TestSuitBase
    {

        #region Test Preparation
        private IApplicationFactory _apiFactory = new ApplicationFactory();
        private string _crmUrl = Config.appSettings.CrmUrl;
        private string _currentUserApiKey;
        private string _userId;
        private int _depositAmount = 10000;
        private string _depositId;
        private string _userName;
        private string _clientEmail;
        private string _clientName;
        private string _clientId;
        private string _expectedCurrency = DataRep.DefaultUSDCurrencyName;
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

            // create deposit 
            _depositId = _apiFactory
                .ChangeContext<IFinancesTabApi>()
                .PostDepositRequest(_crmUrl, _clientId, _depositAmount,
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
        public void VerifyFinancesSheetOnExportClientCardFileTest()
        {
            var expectedType = "deposit";
            var expectedDate = DateTime.Now.ToString("dd/MM/yyyy");

            var dataSet = _apiFactory
                .ChangeContext<IFileHandler>()
                .ConvertXlsxStreamToDataSet(_clientCardStream);

            // transactions table data
            var transactionsTable = _apiFactory
                .ChangeContext<IFileHandler>()
                .ReadDataFromDataSet(dataSet, "Finances");
        
            Assert.Multiple(() =>
            {
                Assert.True(transactionsTable["Id"] == _depositId,
                    $" actual deposit id : {transactionsTable["Id"]}" +
                    $" expected deposit id: {_depositId}");

                Assert.True(transactionsTable["Order id"] == string.Empty,
                    $" actual Order id : {transactionsTable["Order id"]}" +
                    $" expected Order id: empty");

                Assert.True(transactionsTable["Transaction id"] != null,
                    $" actual client Transaction id : {transactionsTable["Transaction id"]}" +
                    $" expected Transaction id not null");

                Assert.True(transactionsTable["Date| GMT"].Contains(expectedDate),
                    $" actual Type : {transactionsTable["Date| GMT"]}" +
                    $" expected Type: {expectedDate}");

                Assert.True(transactionsTable["Assigned"] == _userName,
                    $" actual Assigned : {transactionsTable["Assigned"]}" +
                    $" expected Assigned: {_userName}");

                Assert.True(transactionsTable["Original amount"] == _depositAmount.ToString(),
                    $" actual Original amount : {transactionsTable["Original amount"]}" +
                    $" expected Original amount: {_depositAmount}");

                Assert.True(transactionsTable["Original currency"] == _expectedCurrency,
                    $" actual Original currency : {transactionsTable["Original currency"]}" +
                    $" expected Original currency: {_expectedCurrency}");

                Assert.True(transactionsTable["Last digits"] != null,
                    $" actual Last digits : {transactionsTable["Last digits"]}" +
                    $" expected Last digits not null");

                Assert.True(transactionsTable["Type"] == expectedType,
                    $" actual Type : {transactionsTable["Type"]}" +
                    $" expected Type: {expectedType}");

                Assert.True(transactionsTable["Status"] == "approved",
                    $" actual Status : {transactionsTable["Status"]}" +
                    $" expected Status: approved");

                Assert.True(transactionsTable["Reject reason"] == string.Empty,
                    $" actual Reject reason : {transactionsTable["Reject reason"]}" +
                    $" expected Reject reason: empty");

                Assert.True(transactionsTable["Method"].Contains("Bank Transfer"),
                    $" actual Method : {transactionsTable["Method"]}" +
                    $" expected Method: Bank Transfer");

                Assert.True(transactionsTable["DOD"] == string.Empty,
                    $" actual DOD : {transactionsTable["DOD"]}" +
                    $" expected DOD: empty");
            });
        }
    }
}