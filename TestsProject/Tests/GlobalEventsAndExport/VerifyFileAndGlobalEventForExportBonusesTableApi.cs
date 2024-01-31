// Ignore Spelling: Api

using System;
using System.Collections.Generic;
using System.Linq;
using AirSoftAutomationFramework.Internals.DAL.ConfigurationManagement;
using AirSoftAutomationFramework.Internals.Factorys;
using AirSoftAutomationFramework.Internals.Helpers;
using AirSoftAutomationFramework.Internals.NunitAtributes;
using AirSoftAutomationFramework.Objects.DTOs;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.AccountsApi.UsersMenuApi.UserMenuApi;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.Banking;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.Clients.ClientCard.FinancesTab;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.Clients.ClientPage;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.Settings;
using AirSoftAutomationFramework.Objects.ForexObjects.ApiGeneral;
using NUnit.Framework;
using TestsProject.TestsInternals;

namespace TestsProject.Tests.GlobalEventsAndExport
{
    [NonParallelizable]
    [TestFixture]
    public class VerifyFileAndGlobalEventForExportBonusesTableApi : TestSuitBase
    {
        #region Test Preparation
        private IApplicationFactory _apiFactory = new ApplicationFactory();
        private string _crmUrl = Config.appSettings.CrmUrl;
        private string _currentUserApiKey;
        private string _clientName;       
        private string _clientEmail;
        private string _clientId;
        private string _clientCurrency = DataRep.DefaultUSDCurrencyName;
        private string _userId;
        private static string _date = DateTime.Now.ToString("dd.MM.yy");
        private string _tableName = DataRep.BonusTableName;
        private int _bonosAmount = 1000;
        private string _userEmail;
        private string _exportLink;

        [SetUp]
        public void SetUp()
        {
            BeforeTest();
            var testimEmailPerfix = DataRep.TestimEmailPrefix;

            // update Export Table Email Template
            _apiFactory
              .ChangeContext<ISharedStepsGenerator>()
              .UpdateExportTableEmailTemplate(_crmUrl);

            // create user
            var userName = TextManipulation.RandomString();
            _userEmail = userName + testimEmailPerfix;

            // create user
            _userId = _apiFactory
             .ChangeContext<IUserApi>()
             .PostCreateUserRequest(_crmUrl, userName,
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
            _clientEmail = _clientName + DataRep.TestimEmailPrefix;

            _clientId = _apiFactory
                .ChangeContext<ICreateClientApi>()
                .CreateClientRequest(_crmUrl, _clientName, currency: _clientCurrency,
                emailPrefix: DataRep.TestimEmailPrefix, apiKey: _currentUserApiKey);

            #region connect One User To One Client notification
            // connect One User To One Client notification
            _apiFactory
                .ChangeContext<IClientsApi>()
                .PatchMassAssignSaleAgentsRequest(_crmUrl, _userId,
                new List<string> { _clientId }, apiKey: _currentUserApiKey);
            #endregion

            #region Bonus 
            // Bonus 
            // create bonus
            _apiFactory
                .ChangeContext<IFinancesTabApi>()
                .PostBonusRequest(_crmUrl, _clientId, actualAmount:
                _bonosAmount, apiKey: _currentUserApiKey);
            #endregion

            // enter the Email For Export in Super Admin Tub
            var brandRegulation = _apiFactory
               .ChangeContext<ISuperAdminTubApi>()
               .GetBrandRegulationRequest(_crmUrl);

            DataRep.EmailListForExport.Add(_userEmail);

            brandRegulation.export_data_email_url = DataRep
                .EmailListForExport.ToArray();

            _apiFactory
                .ChangeContext<ISuperAdminTubApi>()
                .PutRegulationsRequest(_crmUrl, brandRegulation);

            #region  export Bonus table
            // export Bonus table
            _apiFactory
                .ChangeContext<IBonusPageApi>()
                .ExportBonusTablePipe(_crmUrl, _clientEmail,
                _userEmail, _currentUserApiKey);

            // extract the export link from mail body
            _exportLink = _apiFactory
               .ChangeContext<ISharedStepsGenerator>()
               .GetExportLinkFromExportEmailBody(_crmUrl,
               _userEmail, _tableName);                          
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
        public void VerifyFileAndGlobalEventForExportBonusesTableApiTest()
        {                    
            var expectedEffectedTable = _tableName;
            var expectedType = "download_export_link";
            var expectedActionMadeByUserId = _userId;
            var expectedDateOfBonus = DateTime.Now.ToString("dd/MM/yyyy");
            var expectedMethod = "Bonus";
            var expectedClientCurrency = DataRep.DefaultUSDCurrencyName;

            // get the csv file 
            var fileString = _apiFactory
                .ChangeContext<IFileHandler>()
                .GetCsvFile(_crmUrl, _exportLink, _currentUserApiKey)
                .Split("link=")
                .First();

            var actualDataFromFile = _apiFactory
                .ChangeContext<IFileHandler>()
                .ReadCSVFile<ExportFinancesTablesResponse>(fileString)
                .FirstOrDefault();

            // get global events
            var actualGlobalEvents = _apiFactory
                .ChangeContext<IGlobalEventsApi>()
                .GetGlobalEventsByUserRequest(_crmUrl, _userEmail.Split('@').First(), _currentUserApiKey)
                .FirstOrDefault();

            Assert.Multiple(() =>
            {
                Assert.True(actualDataFromFile.ID != "0",
                    $" actual id : {actualDataFromFile.ID}" +
                    $" expected id not null");

                Assert.True(actualDataFromFile.DateOfBonusGMT.Contains(expectedDateOfBonus),
                    $" actual Date Of Deposit : {actualDataFromFile.DateOfBonusGMT}" +
                    $" expected Date Of Deposit: {expectedDateOfBonus}");

                Assert.True(actualDataFromFile.ClientId == _clientId,
                    $" actual id : {actualDataFromFile.ClientId}" +
                    $" expected id: {_clientId}");

                Assert.True(actualDataFromFile.ClientFullName == $"{_clientName}  {_clientName}",
                    $" actual Client Full Name : {actualDataFromFile.ClientFullName}" +
                    $" expected Client Full Name: {_clientName} {_clientName}");

                Assert.True(actualDataFromFile.AmountOfBonus == $"{_bonosAmount}.00",
                    $" actual bonos Amount : {actualDataFromFile.OriginalAmount}" +
                    $" expected bonos Amount: {_bonosAmount}");

                Assert.True(actualDataFromFile.Currency == expectedClientCurrency,
                    $" actual Client Currency : {actualDataFromFile.Currency}" +
                    $" expected Client Currency: {expectedClientCurrency}");

                Assert.True(actualDataFromFile.Phone != null,
                    $" actual Phone : {actualDataFromFile.Phone}" +
                    $" expected Phone :  not null");

                Assert.True(actualDataFromFile.Email == _clientEmail,
                    $" actual Client Email : {actualDataFromFile.Email}" +
                    $" expected Client Email: {_clientEmail}");

                Assert.True(actualDataFromFile.SalesAgent == _userEmail.Split('@').First(),
                    $" actual Sales Agent : {actualDataFromFile.SalesAgent}" +
                    $" expected Sales Agent : {_userEmail.Split('@').First()}");

                Assert.True(actualDataFromFile.Status == "approved",
                    $" actual Status : {actualDataFromFile.Status}" +
                    $" expected Status approved");

                Assert.True(actualDataFromFile.Campaign == string.Empty,
                    $" actual Campaign : {actualDataFromFile.Campaign}" +
                    $" expected Campaign Empty");

                Assert.True(actualDataFromFile.OriginalAmount == _bonosAmount.ToString(),
                    $" actual bonos Amount : {actualDataFromFile.OriginalAmount}" +
                    $" expected bonos Amount: {_bonosAmount}");

                Assert.True(actualDataFromFile.OriginalCurrency == expectedClientCurrency,
                     $" actual Original Currency : {actualDataFromFile.OriginalCurrency}" +
                     $" expected Original Currency : {expectedClientCurrency}");

                Assert.True(actualDataFromFile.EuroAmount != 0,
                    $" actual Euro Amount : {actualDataFromFile.EuroAmount}" +
                    $" expected Euro Amount not equal to 0");

                Assert.True(actualDataFromFile.Method == expectedMethod,
                    $" actual Method : {actualDataFromFile.Method}" +
                    $" expected Method {expectedMethod}");

                Assert.True(actualGlobalEvents.table == expectedEffectedTable,
                    $" actual table : {actualGlobalEvents.table}" +
                    $" expected table: {expectedEffectedTable}");

                Assert.True(actualGlobalEvents.type == expectedType,
                    $" actual type : {actualGlobalEvents.type}" +
                    $" expected type: {expectedType}");

                Assert.True(actualGlobalEvents.action_made_by_user_id == _userId,
                    $" actual user Id : {actualGlobalEvents.action_made_by_user_id}" +
                    $" expected user Id: {_userId}");
             });
        }
    }
}