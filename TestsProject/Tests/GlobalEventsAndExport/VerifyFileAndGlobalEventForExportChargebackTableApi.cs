// Ignore Spelling: Api Chargeback

using System;
using System.Collections.Generic;
using System.Linq;
using AirSoftAutomationFramework.Internals.DAL.ConfigurationManagement;
using AirSoftAutomationFramework.Internals.Factorys;
using AirSoftAutomationFramework.Internals.Helpers;
using AirSoftAutomationFramework.Internals.NunitAtributes;
using AirSoftAutomationFramework.Models;
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
    [TestFixture()]
    public class VerifyFileAndGlobalEventForExportChargebackTableApi : TestSuitBase
    {
        #region Test Preparation
        #region Members
        private IApplicationFactory _apiFactory = new ApplicationFactory();
        private string _crmUrl = Config.appSettings.CrmUrl;
        private string _currentUserApiKey;
        private string _clientName;
        private string _clientEmail;
        private string _clientId;
        private string _userId;
        private string _expectedCurrency = DataRep.DefaultUSDCurrencyName;
        private string _expectedCountry = "afghanistan";
        private string _exportLink;
        private string _userName;  
        private int _depositAmount = 100;
        private int _chargebackId;
        private string _tableName = DataRep.ChargebacksTableName;
        private string _userEmail;
        private static string _date = DateTime.Now.ToString("dd.MM.yy");
        private string _expectedFileName = 
            $"chargebacks_airsoftltd.com_{_date}_{_date}.csv";

        private string _groupName;
        #endregion

        [SetUp]
        public void SetUp()
        {
            BeforeTest();
            var emailPerfix = DataRep.TestimEmailPrefix;
            var dbContext = new QaAutomation01Context();
            _groupName = TextManipulation.RandomString();

            // update Export Table Email Template
            _apiFactory
              .ChangeContext<ISharedStepsGenerator>()
              .UpdateExportTableEmailTemplate(_crmUrl);

            // create user
            _userName = TextManipulation.RandomString();
            _userEmail = _userName + emailPerfix;

            // create user
            _userId = _apiFactory
             .ChangeContext<IUserApi>()
             .PostCreateUserRequest(_crmUrl, _userName,
             country: _expectedCountry, emailPrefix: emailPerfix,
              role: DataRep.AdminWithUsersOnlyRoleName);        

            #region create ApiKey
            // create ApiKey
            _currentUserApiKey = _apiFactory
                .ChangeContext<IUserApi>()
                .PostCreateApiKeyRequest(_crmUrl, _userId);
            #endregion

            // create client 
            _clientName = TextManipulation.RandomString();
            _clientEmail = $"{_clientName}{DataRep.TestimEmailPrefix}";

            _clientId = _apiFactory
                .ChangeContext<ICreateClientApi>()
                .CreateClientRequest(_crmUrl, _clientName,
                emailPrefix: DataRep.TestimEmailPrefix,
                apiKey: _currentUserApiKey);

            #region connect One User To One Client notification
            // connect One User To One Client notification
            _apiFactory
                .ChangeContext<IClientsApi>()
                .PatchMassAssignSaleAgentsRequest(_crmUrl, _userId,
                new List<string> { _clientId }, apiKey: _currentUserApiKey);
            #endregion

            #region deposit 
            // deposit 
            _apiFactory
                .ChangeContext<IFinancesTabApi>()
                .PostDepositRequest(_crmUrl, _clientId,
                _depositAmount, apiKey: _currentUserApiKey);

            // get deposit id by amount
            var  depositId =
              (from s in dbContext.FundsTransactions
               where (s.UserId == _clientId && s.OriginalAmount ==
               _depositAmount && s.Type == "deposit")
               select s.Id).First()
               .ToString();
            #endregion

            #region chargeback
            // chargeback
            _chargebackId = _apiFactory
                .ChangeContext<IFinancesTabApi>()
                .DeleteChargeBackDepositRequest(_crmUrl, _clientId,
                _depositAmount, depositId, apiKey: _currentUserApiKey);
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

            // export chargeback table
            _apiFactory
               .ChangeContext<IChargebacksPageApi>()
               .PostExportChargebacksTableRequest(_crmUrl,
               _clientEmail, _userEmail, _currentUserApiKey);

            // extract the export link from mail body
            _exportLink = _apiFactory
               .ChangeContext<ISharedStepsGenerator>()
               .GetExportLinkFromExportEmailBody(_crmUrl, _userEmail, _tableName);
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
        public void VerifyFileAndGlobalEventForExportChargebackTableApiTest()
        {
            var expectedEffectedTable = _tableName;
            var expectedType = "download_export_link";
            var expectedActionMadeByUserId = _userId;
            var expectedMethod = "Bank Transfer: bank_transfer";
            var expectedDateOfChargeback = DateTime.Now.ToString("dd/MM/yyyy");
            var expectedChargebackAmount = 100;

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
               .GetGlobalEventsByUserRequest(_crmUrl, _userName, _currentUserApiKey)
               .FirstOrDefault();

            Assert.Multiple(() =>
            {
                Assert.True(actualDataFromFile.ID == _chargebackId.ToString(),
                    $" actual id : {actualDataFromFile.ID}" +
                    $" expected id {_chargebackId}");

                Assert.True(actualDataFromFile.ClientId == _clientId,
                    $" actual id : {actualDataFromFile.ClientId}" +
                    $" expected id: {_clientId}");

                Assert.True(actualDataFromFile.ClientFullName == $"{_clientName}  {_clientName}",
                    $" actual Client Full Name : {actualDataFromFile.ClientFullName}" +
                    $" expected Client Full Name: {_clientName} {_clientName}");

                Assert.True(actualDataFromFile.TransactionId == string.Empty,
                    $" actual Transaction Id : {actualDataFromFile.TransactionId}" +
                    $" expected Transaction Id equal to Empty");

                Assert.True(actualDataFromFile.TransactionStatus == "approved",
                    $" actual Transaction Status : {actualDataFromFile.TransactionStatus}" +
                    $" expected Transaction Status: approved");

                Assert.True(actualDataFromFile.AmountOfChargeback == expectedChargebackAmount.ToString(),
                    $" actual Amount Of Chargeback : {actualDataFromFile.AmountOfChargeback}" +
                    $" expected Amount Of Chargeback : { expectedChargebackAmount}");

                Assert.True(actualDataFromFile.Phone != null,
                    $" actual Phone : {actualDataFromFile.Phone}" +
                    $" expected Phone :  not null");

                Assert.True(actualDataFromFile.Email == _clientEmail,
                    $" actual Client Email : {actualDataFromFile.Email}" +
                    $" expected Client Email: {_clientEmail}");

                Assert.True(actualDataFromFile.TotalDeposits == $"{_depositAmount}.00",
                    $" actual Total Deposits : {actualDataFromFile.TotalDeposits}" +
                    $" expected Total Deposits :  {_depositAmount}");

                Assert.True(actualDataFromFile.AssignedTo == _userName,
                    $" actual Assigned To : {actualDataFromFile.AssignedTo}" +
                    $" expected AssignedTo: = {_userName}");

                Assert.True(actualDataFromFile.Campaign == string.Empty,
                    $" actual Campaign : {actualDataFromFile.Campaign}" +
                    $" expected Campaign: Empty");

                Assert.True(actualDataFromFile.SalesAgent == _userName,
                    $" actual Sales Agent : {actualDataFromFile.SalesAgent}" +
                    $" expected Sales Agent : {_userName}");

                Assert.True(actualDataFromFile.Title == "Unassign",
                    $" actual Title : {actualDataFromFile.Title}" +
                    $" expected Title = Unassign");

                Assert.True(actualDataFromFile.Currency == _expectedCurrency,
                    $" actual Client Currency : {actualDataFromFile.Currency}" +
                    $" expected Client Currency: {_expectedCurrency}");

                Assert.True(actualDataFromFile.OriginalAmount == _depositAmount.ToString(),
                    $" actual Original Amount : {actualDataFromFile.OriginalAmount}" +
                    $" expected Original Amount: {_depositAmount}");

                Assert.True(actualDataFromFile.OriginalCurrency == _expectedCurrency,
                    $" actual Original Currency : {actualDataFromFile.OriginalCurrency}" +
                    $" expected Original Currency : {_expectedCurrency}");

                Assert.True(actualDataFromFile.Balance == "0.00",
                    $" actual Client Balance : {actualDataFromFile.Balance}" +
                    $" expected Client Balance: 0.00");

                Assert.True(actualDataFromFile.EuroAmount != 0,
                    $" actual Euro Amount : {actualDataFromFile.EuroAmount}" +
                    $" expected Euro Amount not equal to 0");

                Assert.True(actualDataFromFile.DateOfChargebackGMT?.Contains(expectedDateOfChargeback),
                    $" actual Date Of Deposit : {actualDataFromFile.DateOfChargebackGMT}" +
                    $" expected Date Of Deposit: {expectedDateOfChargeback}");

                Assert.True(actualDataFromFile.Psp == string.Empty,
                    $" actual Psp : {actualDataFromFile.Psp}" +
                    $" expected Psp string Empty");

                Assert.True(actualDataFromFile.Method == expectedMethod,
                    $" actual Method : {actualDataFromFile.Method}" +
                    $" expected Method: {expectedMethod}");

                Assert.True(actualDataFromFile.Office != null,
                    $" actual Office : {actualDataFromFile.Office}" +
                    $" expected Office not equals to null");

                Assert.True(actualDataFromFile.Country == _expectedCountry,
                    $" actual Client Country : {actualDataFromFile.Country}" +
                    $" expected Client Country: {_expectedCountry}");

                Assert.True(actualDataFromFile.Note == string.Empty,
                    $" actual Note : {actualDataFromFile.Note}" +
                    $" expected Note empty ");            
    
                Assert.True(actualGlobalEvents.table.Contains(expectedEffectedTable),
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