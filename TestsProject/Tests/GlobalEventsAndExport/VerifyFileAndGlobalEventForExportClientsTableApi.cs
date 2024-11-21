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
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.Clients.ClientCard;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.Clients.ClientCard.FinancesTab;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.Clients.ClientCard.SATab;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.Clients.ClientPage;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.Settings;
using AirSoftAutomationFramework.Objects.ForexObjects.ApiGeneral;
using Microsoft.Graph.Models;
using NUnit.Framework;
using TestsProject.TestsInternals;

namespace TestsProject.Tests.GlobalEventsAndExport
{
    [TestFixture()]
    public class VerifyFileAndGlobalEventForExportClientsTableApi : TestSuitBase
    {
        #region Test Preparation
        private IApplicationFactory _apiFactory = new ApplicationFactory();
        private string _crmUrl = Config.appSettings.CrmUrl;
        private string _currentUserApiKey;
        private string _clientName;
        private string _clientEmail;
        private string _clientId;
        private string _exportLink;
        private string _campaignId;
        private string _expectedStatus2 = "2";
        private string _campaignName;
        private string _userId;
        private string _userName;  
        private string _subCampaignName;
        private string _expectedComment = "expectedComment";
        private int _depositAmount;
        private int _transferToSAAmount;
        private string _tableName = DataRep.ClientsTableName;
        private string _userEmail;

        [SetUp]
        public void SetUp()
        {
            BeforeTest();
            var testimEmailPerfix = DataRep.TestimEmailPrefix;
            _depositAmount = 7;
            _transferToSAAmount = 2;
            // update Export Table Email Template
            _apiFactory
              .ChangeContext<ISharedStepsGenerator>()
              .UpdateExportTableEmailTemplate(_crmUrl);

            // create user
            _userName = TextManipulation.RandomString();
            _userEmail = _userName + testimEmailPerfix;

            // create user
            _userId = _apiFactory
                .ChangeContext<IUserApi>()
                .PostCreateUserRequest(_crmUrl, _userName,
                role: DataRep.AdminWithUsersOnlyRoleName,
                country: "afghanistan", emailPrefix: testimEmailPerfix);

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

            _campaignId = campaignData.Values.First();
            _campaignName = campaignData.Keys.First();

            // create client 
            _clientName = TextManipulation.RandomString();
            _clientEmail = _clientName + DataRep.EmailPrefix;
            _subCampaignName = TextManipulation.RandomString();

            _clientId = _apiFactory
                .ChangeContext<ICreateClientApi>()
                .CreateClientWithCampaign(_crmUrl,
                _clientName, campaignId: _campaignId, 
                currency: DataRep.DefaultUSDCurrencyName,
                apiKey: _currentUserApiKey, subCampaign: _subCampaignName);

            var clientsIds = new List<string> { _clientId };

            // create Comment
            _apiFactory
                .ChangeContext<ICommentsTabApi>()
                .PostCommentRequest(_crmUrl, _clientId,
                _expectedComment,  apiKey: _currentUserApiKey);

            #region connect One User To One Client notification
            // connect One User To One Client notification
            _apiFactory
                .ChangeContext<IClientsApi>()
                .PatchMassAssignSaleAgentsRequest(_crmUrl, _userId,
                clientsIds, apiKey: _currentUserApiKey);
            #endregion

            // get client card
            var informationTabResponse = _apiFactory
                .ChangeContext<IInformationTabApi>()
                .GetInformationTabRequest(_crmUrl, _clientId)
                .GeneralResponse
                .informationTab;

            // set sales status 2 
            informationTabResponse.sales_status2 = _expectedStatus2;

            // update client card
            _apiFactory
                .ChangeContext<IInformationTabApi>()
                .PutInformationTabRequest(_crmUrl, informationTabResponse);

            // Assign Campaign
            var actualAssignCampaignErrorMessage = _apiFactory
                .ChangeContext<IClientsApi>()
                .PatchMassAssignCampaign(_crmUrl,
                new List<string> { _clientId }, _campaignId, _currentUserApiKey);

            // deposit 
            _apiFactory
                .ChangeContext<IFinancesTabApi>()
                .PostDepositRequest(_crmUrl,
                clientsIds, _depositAmount);

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

            // CRM Transfer To Saving Account
            _apiFactory
                .ChangeContext<ISATabApi>()
                .PostTransferToSavingAccountRequest(_crmUrl, _clientId, _transferToSAAmount,
                _currentUserApiKey);

            var expectedBalance = _depositAmount - _transferToSAAmount;

            // Wait For Balance In Client Card To Be Updated
            _apiFactory
                .ChangeContext<IClientCardApi>()
                .WaitForBalanceInClientCardToBeUpdated(_crmUrl, _clientId, expectedBalance);

            #region export clients table
            // export clients table
            _apiFactory
               .ChangeContext<IClientsApi>()
               .ExportClientsTablePipe(_crmUrl, _clientEmail, _userEmail, _currentUserApiKey);

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
        [Description("BUG SUB CAMPAIGN")]
        [Category(DataRep.SanityCategory)]
        [Category(DataRep.RegressionCategory)]
        [RetryMenage(DataRep.NumOfRetriesForFallingTest)]
        public void VerifyFileAndGlobalEventForExportClientsTableApiTest()
        {
            var expectedEffectedTable = _tableName;
            var expectedActionMadeByUserId = _userId;
            var expectedClientStatus = "Double Phone Number";
            var expectedCeatedAt = DateTime.Now.ToString("MM/dd/yyyy");
            var expectedAttributionDate = DateTime.Now.ToString("dd/MM/yyyy");
            var expectedCountry = "afghanistan";
            var expectedNumOfDeposit = "1";
            var expectedTreadingGroupName = "Default";
            var expectedBalance = _depositAmount - _transferToSAAmount;          

            // get the csv file 
            var fileString = _apiFactory
                .ChangeContext<IFileHandler>()
                .GetCsvFile(_crmUrl, _exportLink, _currentUserApiKey)
                .Split("link=")
                .First();

            // read the csv file
            var actualDataFromFile = _apiFactory
                .ChangeContext<IFileHandler>()
                .ReadCSVFile<ExportClientTableResponse>(fileString)
                .FirstOrDefault();

            // get global events
            var actualGlobalEvents = _apiFactory
                .ChangeContext<IGlobalEventsApi>()
                .GetGlobalEventsByUserRequest(_crmUrl, _userName, _currentUserApiKey)
                .FirstOrDefault();

            Assert.Multiple(() =>
            {
                Assert.True(actualGlobalEvents.action_made_by == _userName,
                    $" actual action made by : {actualGlobalEvents.action_made_by}" +
                    $" expected action made by: {_userName}");

                Assert.True(actualGlobalEvents.action_made_by_user_id == _userId,
                    $" actual action made by user_id : {actualGlobalEvents.action_made_by_user_id}" +
                    $" expected action made by user_id: {_userId}");

                Assert.True(actualGlobalEvents.erp_user_full_name == $"{_userName} {_userName}",
                    $" actual action made by user full_name : {actualGlobalEvents.erp_user_full_name}" +
                    $" expected action made by user full_name: {_userName} {_userName}");

                Assert.True(actualDataFromFile.Answer == "No",
                    $" actual Answer : {actualDataFromFile.Answer}" +
                    $" expected Answer : No");

                Assert.True(actualDataFromFile.Id == _clientId,
                    $" actual client Id : {actualDataFromFile.Id}" +
                    $" expected client Id: {_clientId}");

                Assert.True(actualDataFromFile.FullName == $"{_clientName} {_clientName}",
                    $" actual full name : {actualDataFromFile.FullName}" +
                    $" expected full name: {_clientName} {_clientName}");

                Assert.True(actualDataFromFile.Country == expectedCountry,
                    $" actual Client Country : {actualDataFromFile.Country}" +
                    $" expected Client Country: {expectedCountry}");

                Assert.True(actualDataFromFile.SalesAgent == _userEmail.Split('@').First(),
                    $" actual Sales Agent : {actualDataFromFile.SalesAgent}" +
                    $" expected Sales Agent : {_userEmail.Split('@').First()}");

                Assert.True(actualDataFromFile.Campaign == _campaignName,
                    $" actual Campaign : {actualDataFromFile.Campaign}" +
                    $" expected Campaign = {_campaignName}");

                Assert.True(actualDataFromFile.Email == _clientEmail,
                    $" actual Client Email : {actualDataFromFile.Email}" +
                    $" expected Client Email: {_clientEmail}");

                Assert.True(actualDataFromFile.Phone != null,
                    $" actual Phone : {actualDataFromFile.Phone}" +
                    $" expected Phone not equal to null");

                Assert.True(actualDataFromFile.LastAttributionDateGMT?.Contains(expectedAttributionDate),
                    $" actual Attribution Date : {actualDataFromFile.LastAttributionDateGMT}" +
                    $" expected Attribution Date: {expectedAttributionDate}");

                Assert.True(actualDataFromFile.FreeText == "free_text",
                    $" actual Free Text : {actualDataFromFile.FreeText}" +
                    $" expected Free Text = free_text");

                Assert.True(actualDataFromFile.Note == "Automation note",
                    $" actual Note : {actualDataFromFile.Note}" +
                    $" expected Note = ");

                Assert.True(actualDataFromFile.TotalDeposit == $"{_depositAmount}.00 $",
                    $" actual Total Deposit : {actualDataFromFile.TotalDeposit}" +
                    $" expected Total Deposit = {_depositAmount}.00 $");

                Assert.True(actualDataFromFile.Balance == $"{expectedBalance}.00 $",
                    $" actual Balance : {actualDataFromFile.Balance}" +
                    $" expected Balance {expectedBalance}.00 $" +
                    $" user Email: {_userEmail}");

                Assert.True(actualDataFromFile.Bonus == "0.00",
                    $" actual Bonus : {actualDataFromFile.Bonus}" +
                    $" expected Bonus = 0");

                Assert.True(actualDataFromFile.KycPoi == "Waiting",
                    $" actual Kyc Proof Of Identity Status : {actualDataFromFile.KycPoi}" +
                    $" expected Kyc Proof Of Identity Status = Waiting");

                Assert.True(actualDataFromFile.KycPor == "Waiting",
                    $" actual Kyc Proof Of Residency Status : {actualDataFromFile.KycPor}" +
                    $" expected Kyc Proof Of Residency Status = Waiting");

                Assert.True(actualDataFromFile.KycCcFront == "Waiting",
                    $" actual Credit Debit Card Documentation front Status : {actualDataFromFile.KycCcFront}" +
                    $" expected  Credit Debit Card Documentation front Status = Waiting");

                Assert.True(actualDataFromFile.KycCcBack == "Waiting",
                    $" actual Credit Debit Card Documentation back Status : {actualDataFromFile.KycCcBack}" +
                    $" expected  Credit Debit Card Documentation back Status = Waiting");

                Assert.True(actualDataFromFile.KycStatus == "Pending",
                    $" actual Kyc Status : {actualDataFromFile.KycStatus}" +
                    $" expected Kyc Status = Pending");

                Assert.True(actualDataFromFile.TotalBonus == "0.00 $",
                    $" actual Total Bonus : {actualDataFromFile.TotalBonus}" +
                    $" expected Total Bonus 0.00 $");

                Assert.True(actualDataFromFile.RegistrationGMT?.Contains(expectedAttributionDate),
                    $" actual Registration : {actualDataFromFile.RegistrationGMT}" +
                    $" expected Registration: {expectedAttributionDate}");

                Assert.True(actualDataFromFile.SABalance == $"{_transferToSAAmount}.00 $",
                    $" actual SA balance : {actualDataFromFile.SABalance}" +
                    $" expected SA balance: {_transferToSAAmount}");

                Assert.True(actualDataFromFile.Status == expectedClientStatus,
                    $" actual Client Status : {actualDataFromFile.Status}" +
                    $" expected Client Status = {expectedClientStatus}");

                Assert.True(actualDataFromFile.Status2 == _expectedStatus2,
                    $" actual Client Status 2 : {actualDataFromFile.Status2}" +
                    $" expected Client Status 2 = {_expectedStatus2}");

                Assert.True(actualDataFromFile.subCampaign == _subCampaignName,
                    $" actual Client sub Campaign : {actualDataFromFile.subCampaign}" +
                    $" expected Client sub Campaign = {_subCampaignName}");

                Assert.True(actualDataFromFile.Office != null,
                    $" actual Office : {actualDataFromFile.Office}" +
                    $" expected Office not equals to null");

                Assert.True(actualDataFromFile.LastDigits == DataRep.LastDigits,
                    $" actual Last Digits : {actualDataFromFile.LastDigits}" +
                    $" expected Last Digits = DataRep.ForexLastDigits");

                Assert.True(actualDataFromFile.LastLoginGMT == "-",
                    $" actual Last Login : {actualDataFromFile.LastLoginGMT}" +
                    $" expected Last Login = -");

                Assert.True(actualDataFromFile.LastComment == _expectedComment,
                    $" actual Last Comment : {actualDataFromFile.LastComment}" +
                    $" expected Last Comment: _expectedComment");

                Assert.True(actualDataFromFile.LastTrade == null,
                    $" actual Last Trade : {actualDataFromFile.LastTrade}" +
                    $" expected Last Trade = -");

                Assert.True(actualDataFromFile.LastCall == null,
                    $" actual Last Call : {actualDataFromFile.LastCall}" +
                    $" expected Last Call = -");

                Assert.True(actualDataFromFile.LastDepositGMT.Contains(expectedAttributionDate),
                    $" actual Last Deposit : {actualDataFromFile.LastDepositGMT}" +
                    $" expected Last Deposit = {expectedAttributionDate}");

                Assert.True(actualDataFromFile.NumOfDeposit == expectedNumOfDeposit,
                    $" actual Num Of Deposit : {actualDataFromFile.NumOfDeposit}" +
                    $" expected Num Of Deposit = {expectedNumOfDeposit}");

                Assert.True(actualDataFromFile.TradingGroup == expectedTreadingGroupName,
                    $" actual Treading Group Name : {actualDataFromFile.TradingGroup}" +
                    $" expected Treading Group Name = {expectedTreadingGroupName}");

                Assert.True(actualDataFromFile.FtdGMT.Contains(expectedAttributionDate),
                    $" actual Ftd : {actualDataFromFile.FtdGMT}" +
                    $" expected Ftd = {expectedAttributionDate}");
            });
        }
    }
}