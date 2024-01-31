// Ignore Spelling: Api

using AirSoftAutomationFramework.Internals.DAL.ConfigurationManagement;
using AirSoftAutomationFramework.Internals.Helpers;
using AirSoftAutomationFramework.Objects.DTOs;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.AccountsApi.UsersMenuApi.UserMenuApi;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.Banking;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.Clients.ClientCard.FinancesTab;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.Clients.ClientPage;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.Settings;
using AirSoftAutomationFramework.Objects.ForexObjects.ApiGeneral;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using AirSoftAutomationFramework.Internals.Factorys;
using AirSoftAutomationFramework.Internals.NunitAtributes;
using TestsProject.TestsInternals;

namespace TestsProject.Tests.GlobalEventsAndExport
{
    [TestFixture]
    public class VerifyFileAndGlobalEventForExportDepositsTableApi : TestSuitBase
    {
        #region Test Preparation
        #region Members
        private IApplicationFactory _apiFactory = new ApplicationFactory();
        private string _crmUrl = Config.appSettings.CrmUrl;
        private string _currentUserApiKey;
        private string _clientName;
        private string _clientEmail;
        private string _clientId;
        private string _exportLink;
        private string _expectedCurrency = DataRep.DefaultUSDCurrencyName;
        private string _expectedCountry = "afghanistan";
        private string _userId;
        private int _depositAmount = 100;
        private string _userEmail;
        private string _userName;
        private string _tableName = DataRep.DepositsTableName;
        private static string _date = DateTime.Now.ToString("dd.MM.yy");
        private string _expectedFileName = $"deposits_airsoftltd.com_{_date}_{_date}.csv";
        private string _expectedNoteAndFreeText = "NoteAndFreeText";
        Data _depositData;
        private string _groupName;
        private string _depositId;
        private string _campaignName;
        #endregion

        [SetUp]
        public void SetUp()
        {
            BeforeTest();
            _groupName = TextManipulation.RandomString();
            var emailPerfix = DataRep.TestimEmailPrefix;

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
              country: _expectedCountry,
              emailPrefix: emailPerfix, role: DataRep.AdminWithUsersOnlyRoleName);

            #region create ApiKey
            // create ApiKey
            _currentUserApiKey = _apiFactory
                .ChangeContext<IUserApi>()
                .PostCreateApiKeyRequest(_crmUrl, _userId);
            #endregion

            // create campaign
            var campaignData = _apiFactory
                .ChangeContext<ISharedStepsGenerator>()
                .CreateAffiliateAndCampaignApi(_crmUrl);

            var campaignId = campaignData.Values.First();
            _campaignName = campaignData.Keys.First();

            // create client 
            _clientName = TextManipulation.RandomString();
            _clientEmail = _clientName + DataRep.TestimEmailPrefix;

            _clientId = _apiFactory
                .ChangeContext<ICreateClientApi>()
                .CreateClientWithCampaign(_crmUrl, _clientName, campaignId,
                freeText: _expectedNoteAndFreeText, currency: _expectedCurrency,
                emailPrefix: DataRep.TestimEmailPrefix,
                apiKey: _currentUserApiKey, note: _expectedNoteAndFreeText);

            var clientsIds = new List<string> { _clientId };

            #region connect One User To One Client notification
            // connect One User To One Client notification
            _apiFactory
                .ChangeContext<IClientsApi>()
                .PatchMassAssignSaleAgentsRequest(_crmUrl, _userId,
                clientsIds, apiKey: _currentUserApiKey);
            #endregion

            // create deposit 
            _depositId = _apiFactory
                .ChangeContext<IFinancesTabApi>()
                .PostDepositRequest(_crmUrl, _clientId, _depositAmount);

            _depositData = _apiFactory
                .ChangeContext<IDepositsPageApi>()
                .GetDepositDataFromBankingRequest(_crmUrl, "status",
                "approved", _currentUserApiKey)
                .GeneralResponse
                .data
                .FirstOrDefault();
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

            #region  export deposit table
            // export deposits table
            _apiFactory
                .ChangeContext<IDepositsPageApi>()
                .ExportDepositsTablePipe(_crmUrl,
                _clientEmail, _userEmail, _currentUserApiKey);

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

        [Test]
        [Category(DataRep.SanityCategory)]
        [Category(DataRep.RegressionCategory)]
        [RetryMenage(DataRep.NumOfRetraiesForFallingTest)]
        public void VerifyFileAndGlobalEventForExportDepositsTableApiTest()
        {
            var expectedEffectedTable = _tableName;
            var expectedType = "download_export_link";
            var expectedActionMadeByUserId = _userId;
            var expectedDateOfDeposit = DateTime.Now.ToString("dd/MM/yyyy");
            var expectedDateOfRegistration = DateTime.Now.ToString("yyyy-MM-dd");
            var expectedDisplayName = "bank transfer";
            var expectedMethod = "Bank Transfer: bank_transfer";

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
               .GetGlobalEventsByUserRequest(_crmUrl,
               _userEmail.Split('@').First(), _currentUserApiKey)
               .FirstOrDefault();

            Assert.Multiple(() =>
            {
                Assert.True(actualDataFromFile.ID.Equals(_depositData.id),
                    $" actual id : {actualDataFromFile.ID}" +
                    $" expected id {_depositData.id}");

                Assert.True(actualDataFromFile.DateOfDepositGMT?.Contains(expectedDateOfDeposit),
                    $" actual Date Of Deposit : {actualDataFromFile.DateOfDepositGMT}" +
                    $" expected Date Of Deposit: {expectedDateOfDeposit}");

                Assert.True(actualDataFromFile.TransactionId.Equals(_depositData.transaction_id),
                    $" actual Transaction Id : {actualDataFromFile.TransactionId}" +
                    $" expected Transaction Id {_depositData.transaction_id}");

                Assert.True(actualDataFromFile.Phone.Equals(_depositData.phone),
                    $" actual Phone : {actualDataFromFile.Phone}" +
                    $" expected Phone :  {_depositData.phone}");

                Assert.True(actualDataFromFile.Email.Equals(_depositData.email),
                    $" actual Client Email : {actualDataFromFile.Email}" +
                    $" expected Client Email: {_depositData.email}");

                Assert.True(actualDataFromFile.TotalDeposits.Equals(_depositData.total_deposit),
                    $" actual Total Deposit : {actualDataFromFile.TotalDeposit}" +
                    $" expected Total Deposit: {_depositData.total_deposit}");

                Assert.True(actualDataFromFile.AssignedTo.Equals(_userName),
                    $" actual Assigned To : {actualDataFromFile.AssignedTo}" +
                    $" expected Assigned To : {_userName}");

                Assert.True(actualDataFromFile.Campaign.Equals(_depositData.campaign),
                    $" actual Campaign name : {actualDataFromFile.Campaign}" +
                    $" expected Campaign name {_depositData.campaign}");

                Assert.True(actualDataFromFile.SalesAgent.Equals(_depositData.erp_assigned),
                    $" actual Sales Agent : {actualDataFromFile.SalesAgent}" +
                    $" expected Sales Agent : {_depositData.erp_assigned}");

                Assert.True(actualDataFromFile.TransactionStatus.Equals(_depositData.status),
                    $" actual Transaction Status : {actualDataFromFile.TransactionStatus}" +
                    $" expected Transaction Status: {_depositData.status}");

                Assert.True(actualDataFromFile.Ftd.ToString().Equals(_depositData.ftd),
                    $" actual Ftd : {actualDataFromFile.Ftd}" +
                    $" expected Ftd = {_depositData.ftd}");

                Assert.True(actualDataFromFile.FreeText == $"{_expectedNoteAndFreeText}",
                    $" actual Free Text : {actualDataFromFile.FreeText}" +
                    $" expected Free Text : {_expectedNoteAndFreeText}");

                Assert.True(actualDataFromFile.RegistrationGMT.Contains(expectedDateOfDeposit),
                    $" actual Date Of Registration Contains : {actualDataFromFile.RegistrationGMT}" +
                    $" expected Date Of Registration Contains : {expectedDateOfDeposit}");

                Assert.True(actualDataFromFile.DepositAmount.Equals(_depositData.amount),
                    $" actual deposit Amount : {actualDataFromFile.Amount}" +
                    $" expected deposit Amount : {_depositData.amount}");

                Assert.True(actualDataFromFile.Currency == _depositData.currency,
                    $" actual Client Currency : {actualDataFromFile.Currency}" +
                    $" expected Client Currency: {_depositData.currency}");

                Assert.True(actualDataFromFile.OriginalAmount.Equals(_depositData.original_amount),
                    $" actual bonos Amount : {actualDataFromFile.OriginalAmount}" +
                    $" expected bonos Amount: {_depositData.original_amount}");

                Assert.True(actualDataFromFile.OriginalCurrency == _depositData.original_currency,
                    $" actual Original Currency : {actualDataFromFile.OriginalCurrency}" +
                    $" expected Original Currency : {_expectedCurrency}");

                Assert.True(actualDataFromFile.EuroAmount != 0,
                    $" actual Euro Amount : {actualDataFromFile.EuroAmount}" +
                    $" expected Euro Amount not equal to 0");

                Assert.True(actualDataFromFile.DisplayName == expectedDisplayName,
                    $" actual Display Name : {actualDataFromFile.DisplayName}" +
                    $" expected Display Name : {expectedDisplayName}");

                Assert.True(actualDataFromFile.Note == _expectedNoteAndFreeText,
                    $" actual note : {actualDataFromFile.Note}" +
                    $" expected note : {_expectedNoteAndFreeText}");

                Assert.True(actualDataFromFile.RejectReason == string.Empty,
                    $" actual Reject Reason : {actualDataFromFile.RejectReason}" +
                    $" expected Reject Reason = null ");

                Assert.True(actualDataFromFile.Office.Equals(_depositData.office_city),
                    $" actual Office : {actualDataFromFile.Office}" +
                    $" expected Office: {_depositData.office_city}");

                Assert.True(actualDataFromFile.Country.Equals(_depositData.country),
                    $" actual Client Country : {actualDataFromFile.Country}" +
                    $" expected Client Country: {_depositData.country}");

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