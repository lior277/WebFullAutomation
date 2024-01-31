// Ignore Spelling: Api

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
using AirSoftAutomationFramework.Objects.ForexObjects.ApiGeneral.Login;
using AirSoftAutomationFramework.Objects.ForexObjects.TradingPlatform.TradeObjects.TradeApi.Settings;
using NUnit.Framework;
using TestsProject.TestsInternals;

namespace TestsProject.Tests.GlobalEventsAndExport
{
    [NonParallelizable]
    [TestFixture()]
    public class VerifyFileAndGlobalEventForExportWithdrawalsTableApi : TestSuitBase
    {
        #region Test Preparation
        #region Members
        private IApplicationFactory _apiFactory = new ApplicationFactory();
        private string _crmUrl = Config.appSettings.CrmUrl;
        private string _currentUserApiKey;
        private string _clientName;
        private string _clientEmail;
        private string _expectedCurrency = DataRep.DefaultUSDCurrencyName;
        private string _expectedCountry = "afghanistan";
        private string _clientId;
        private string _exportLink;
        private string _userId;
        private int _depositAmount = 100;
        private int _withdrawalAmount = 30;
        private int _withdrawalId;
        private string _userEmail;
        private string _userName;
        private string _tableName = DataRep.WithdrawalsTableName;
        private static string _date = DateTime.Now.ToString("dd.MM.yy");
        private string _groupName;
        #endregion

        [SetUp]
        public void SetUp()
        {
            BeforeTest();
            _groupName = TextManipulation.RandomString();
            var dbContext = new QaAutomation01Context();
            var emailPerfix = DataRep.TestimEmailPrefix;
            var tradingPlatformUrl = Config.appSettings.tradingPlatformUrl;

            // update Export Table Email Template
            _apiFactory
              .ChangeContext<ISharedStepsGenerator>()
              .UpdateExportTableEmailTemplate(_crmUrl);

            // create user
            _userName = TextManipulation.RandomString();
            _userEmail = _userName + DataRep.TestimEmailPrefix;

            _userId = _apiFactory
             .ChangeContext<IUserApi>()
             .PostCreateUserRequest(_crmUrl, _userName,
             emailPrefix: emailPerfix,
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
                currency: _expectedCurrency, apiKey: _currentUserApiKey);

            var clientsIds = new List<string> { _clientId };

            #region connect One User To One Client notification
            // connect One User To One Client notification
            _apiFactory
                .ChangeContext<IClientsApi>()
                .PatchMassAssignSaleAgentsRequest(_crmUrl, _userId,
                clientsIds, apiKey: _currentUserApiKey);
            #endregion

            #region deposit 
            // deposit 
            _apiFactory
                .ChangeContext<IFinancesTabApi>()
                .PostDepositRequest(_crmUrl, _clientId, _depositAmount);
            #endregion

            #region login data
            // login data
            var loginData = _apiFactory
                .ChangeContext<ILoginApi>()
                .PostLoginToTradingPlatform(tradingPlatformUrl, _clientEmail)
                .GeneralResponse;
            #endregion

            #region create approved withdrawal 
            // create pending withdrawal 
            _apiFactory
                .ChangeContext<IWithdrawalTpApi>()
                .PostPendingWithdrawalRequest(tradingPlatformUrl, loginData, _withdrawalAmount);

            // get pending Withdrawal id
            _withdrawalId =
                (from s in dbContext.FundsTransactions
                 where (s.UserId == _clientId && s.Type == "withdrawal" && s.Status == "pending")
                 select s.Id).First();

            //// proceed deposit Withdrawal
            //_apiFactory
            //    .ChangeContext<IFinancesTabApi>()
            //    .PatchWithdrawalStatusRequest(_crmUrl, clientMailAndId.Values.First(), _withdrawalId);
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

            #region  export withdrawal table
            // export withdrawal table
            _apiFactory
                .ChangeContext<IWithdrawalsPageApi>()
                .ExportWithdrawalsTablePipe(_crmUrl, _clientEmail,
                _userEmail, _currentUserApiKey);

            // extract the export link from mail body
            _exportLink = _apiFactory
                .ChangeContext<ISharedStepsGenerator>()
                .GetExportLinkFromExportEmailBody(_crmUrl, _userEmail, _tableName);
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
        public void VerifyFileAndGlobalEventForExportWithdrawalsTableApiTest()
        {
            var expectedEffectedTable = _tableName;
            var expectedType = "download_export_link";
            var expectedActionMadeByUserId = _userId;
            var expectedWithdrawalStatus = "pending";
            var expectedTitle = "withdrawal"; 
            var expectedBalance = _depositAmount;
            var expectedDateOfDeposit = DateTime.Now.ToString("dd/MM/yyyy");

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
                Assert.True(actualDataFromFile.ID == _withdrawalId.ToString(),
                    $" actual id : {actualDataFromFile.ID}" +
                    $" expected id {_withdrawalId}");

                Assert.True(actualDataFromFile.ClientId == _clientId,
                    $" actual Client id : {actualDataFromFile.ClientId}" +
                    $" expected Client id: {_clientId}");

                Assert.True(actualDataFromFile.ClientFullName == $"{_clientName}  {_clientName}",
                    $" actual Client Full Name : {actualDataFromFile.ClientFullName}" +
                    $" expected Client Full Name: {_clientName} {_clientName}");

                Assert.True(actualDataFromFile.TransactionId == string.Empty,
                    $" actual Transaction Id : {actualDataFromFile.TransactionId}" +
                    $" expected Transaction Id equal to Empty");

                Assert.True(actualDataFromFile.TransactionStatus == expectedWithdrawalStatus,
                    $" actual Transaction Status : {actualDataFromFile.TransactionStatus}" +
                    $" expected Transaction Status: {expectedWithdrawalStatus}");

                Assert.True(actualDataFromFile.WithdrawalAmount.Equals($"-{_withdrawalAmount}.00"),
                    $" actual Amount Of Withdrawal : {actualDataFromFile.WithdrawalAmount}" +
                    $" expected Amount Of Withdrawal :  -{ _withdrawalAmount}");

                Assert.True(actualDataFromFile.TotalWithdrawal == $"-{_withdrawalAmount}",
                    $" actual Amount Of Total Withdrawal : {actualDataFromFile.TotalWithdrawal}" +
                    $" expected Amount Of Total  Withdrawal : -{_withdrawalAmount}");

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

                Assert.True(actualDataFromFile.Currency == _expectedCurrency,
                    $" actual Client Currency : {actualDataFromFile.Currency}" +
                    $" expected Client Currency: {_expectedCurrency}");

                Assert.True(actualDataFromFile.Balance == $"{expectedBalance}.00",
                    $" actual Client Balance : {actualDataFromFile.Balance}" +
                    $" expected Client Balance: {_depositAmount}.00");

                Assert.True(actualDataFromFile.OriginalAmount == $"-{_withdrawalAmount}",
                    $" actual bonos Amount : {actualDataFromFile.OriginalAmount}" +
                    $" expected bonos Amount: {_withdrawalAmount}");

                Assert.True(actualDataFromFile.OriginalCurrency == _expectedCurrency,
                    $" actual Original Currency : {actualDataFromFile.OriginalCurrency}" +
                    $" expected Original Currency : {_expectedCurrency}");

                Assert.True(actualDataFromFile.EuroAmount != 0,
                    $" actual Euro Amount : {actualDataFromFile.EuroAmount}" +
                    $" expected Euro Amount not equal to 0");               

                Assert.True(actualDataFromFile.Title == expectedTitle,
                    $" actual Title : {actualDataFromFile.Title}" +
                    $" expected Title: {expectedTitle}");

                Assert.True(actualDataFromFile.RequestDateGMT?.Contains(expectedDateOfDeposit),
                    $" actual Request Date : {actualDataFromFile.RequestDateGMT}" +
                    $" expected Request Date = {expectedDateOfDeposit}");

                Assert.True(actualDataFromFile.Psp == string.Empty,
                    $" actual Psp : {actualDataFromFile.Psp}" +
                    $" expected Psp = Empty");

                Assert.True(actualDataFromFile.Method == string.Empty,
                    $" actual Method : {actualDataFromFile.Method}" +
                    $" expected Method Empty");

                Assert.True(actualDataFromFile.ProceedDateGMT == "",
                    $" actual Proceed Date GMT : {actualDataFromFile.ProceedDateGMT}" +
                    $" expected Proceed Date GMT: empty");

                Assert.True(actualDataFromFile.Office != null,
                    $" actual Office : {actualDataFromFile.Office}" +
                    $" expected Office not equal to null");

                Assert.True(actualDataFromFile.Country == _expectedCountry,
                    $" actual Client Country : {actualDataFromFile.Country}" +
                    $" expected Client Country: {_expectedCountry}");

                Assert.True(actualDataFromFile.Note == string.Empty,
                    $" actual Note : {actualDataFromFile.Note}" +
                    $" expected Note empty ");
            
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