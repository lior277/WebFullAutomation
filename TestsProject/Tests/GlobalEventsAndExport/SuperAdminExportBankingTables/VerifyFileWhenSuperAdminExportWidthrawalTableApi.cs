// Ignore Spelling: Widthrawal Admin Api

using System;
using System.Collections.Generic;
using System.Linq;
using AirSoftAutomationFramework.Internals.DAL.ApiAccsess.ApiRoutes;
using AirSoftAutomationFramework.Internals.DAL.ConfigurationManagement;
using AirSoftAutomationFramework.Internals.ExtensionsMethods;
using AirSoftAutomationFramework.Internals.Factorys;
using AirSoftAutomationFramework.Internals.Helpers;
using AirSoftAutomationFramework.Internals.NunitAtributes;
using AirSoftAutomationFramework.Models;
using AirSoftAutomationFramework.Objects.DTOs;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.AccountsApi.UsersMenuApi.UserMenuApi;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.Clients.ClientCard.FinancesTab;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.Clients.ClientPage;
using AirSoftAutomationFramework.Objects.ForexObjects.ApiGeneral;
using AirSoftAutomationFramework.Objects.ForexObjects.ApiGeneral.Login;
using AirSoftAutomationFramework.Objects.ForexObjects.TradingPlatform.TradeObjects.TradeApi.Settings;
using NUnit.Framework;
using TestsProject.TestsInternals;

namespace TestsProject.Tests.GlobalEventsAndExport.SuperAdminExportBankingTables
{
    [TestFixture]
    public class VerifyFileWhenSuperAdminExportWidthrawalTableApi : TestSuitBase
    {
        #region Test Preparation
        private IApplicationFactory _apiFactory = new ApplicationFactory();
        private string _crmUrl = Config.appSettings.CrmUrl;
        private string _clientName;       
        private string _clientEmail;
        private string _clientId;
        private string _clientCurrency = DataRep.DefaultUSDCurrencyName;
        private string _expectedCurrency = DataRep.DefaultUSDCurrencyName;
        private string _expectedCountry = "afghanistan";
        private string _campaignName;
        private string _userId;
        private int _depositAmount = 100;
        private int _withdrawalId;
        private int _withdrawalAmount = 30;
        private decimal _expectedWithdrawalAmountInEro;
        private Dictionary<string, string> _campaignData;
        private string _today = DateTime.Now.ToString("yyyy-MM-dd");
        private string _userEmail;
        private string _userName;
        private string _exportLink;

        [SetUp]
        public void SetUp()
        {
            BeforeTest();
            var dbContext = new QaAutomation01Context();
            var tradingPlatformUrl = Config.appSettings.tradingPlatformUrl;

            // create user
            _userName = TextManipulation.RandomString();
            _userEmail = _userName + DataRep.EmailPrefix;

            // create user
            _userId = _apiFactory
                .ChangeContext<IUserApi>()
                .PostCreateUserRequest(_crmUrl, _userName);

            _campaignData = _apiFactory
                .ChangeContext<ISharedStepsGenerator>()
                .CreateAffiliateAndCampaignApi(_crmUrl);

            var campaignId = _campaignData.Values.First();
            _campaignName = _campaignData.Keys.First();

            // create client
            _clientName = TextManipulation.RandomString();
            _clientEmail = _clientName + DataRep.EmailPrefix;

            _clientId = _apiFactory
                .ChangeContext<ICreateClientApi>()
                .CreateClientWithCampaign(_crmUrl, _clientName,
                campaignId, currency: _clientCurrency);

            #region connect One User To One Client notification
            // connect One User To One Client notification
            _apiFactory
                .ChangeContext<IClientsApi>()
                .PatchMassAssignSaleAgentsRequest(_crmUrl, _userId,
                new List<string> { _clientId });
            #endregion

            // deposit 
            _apiFactory
                .ChangeContext<IFinancesTabApi>()
                .PostDepositRequest(_crmUrl, new List<string> { _clientId },
                _depositAmount);

            // login data
            var loginData = _apiFactory
                .ChangeContext<ILoginApi>()
                .PostLoginToTradingPlatform(tradingPlatformUrl, _clientEmail)
                .GeneralResponse;

            // create pending withdrawal 
            _apiFactory
                .ChangeContext<IWithdrawalTpApi>()
                .PostPendingWithdrawalRequest(tradingPlatformUrl, loginData,
                _withdrawalAmount);

            // get pending Withdrawal id
            _withdrawalId =
                (from s in dbContext.FundsTransactions
                 where (s.UserId == _clientId && s.Type == "withdrawal" && s.Status == "pending")
                 select s.Id).First();

            _exportLink = $"{ApiRouteAggregate.PostExportBankingTables("withdrawal")}" +
                $"?draw=4&order[0][column]=user_id&order[0][dir]=desc&start=0&length" +
                $"=10000000&search[value]={_clientEmail}&search[regex]=true&filter[status]" +
                $"[]=pending&filter[start_date]={_today} 00:00:00&filter[end_date]={_today}" +
                $" 23:59:59&export=1&table=withdrawals";

            // USD  in EUR
            _expectedWithdrawalAmountInEro = _apiFactory
                .ChangeContext<IGeneral>()
                .PostCurrencyConversionRequest(_crmUrl, _withdrawalAmount, DataRep.DefaultUSDCurrencyName, "EUR");
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
        public void VerifyFileWhenSuperAdminExportWidthrawalTableApiTest()
        {                    
            var expectedDateOfDeposit = DateTime.Now.ToString("dd/MM/yyyy");
            var expectedTitle = "withdrawal";
            var expectedBalance = _depositAmount;
            var expectedWithdrawalStatus = "pending";

            // get the csv file 
            var fileString = _apiFactory
                .ChangeContext<IFileHandler>()
                .GetCsvFile(_crmUrl, _exportLink);

            var actualDataFromFile = _apiFactory
                .ChangeContext<IFileHandler>()
                .ReadCSVFile<ExportFinancesTablesResponse>(fileString)
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

                Assert.True(actualDataFromFile.WithdrawalAmount == $"-{_withdrawalAmount}.00",
                    $" actual Amount Of Withdrawal : {actualDataFromFile.WithdrawalAmount}" +
                    $" expected Amount Of Withdrawal :  -{ _withdrawalAmount}");

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

                Assert.True(actualDataFromFile.Campaign == _campaignName,
                    $" actual Campaign : {actualDataFromFile.Campaign}" +
                    $" expected Campaign: {_campaignName}");

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

                Assert.True(_expectedWithdrawalAmountInEro
                    == actualDataFromFile.EuroAmount.MathRoundFromGeneric(2),
                   $" actual Euro Amount : {actualDataFromFile.EuroAmount}" +
                   $" expected Euro Amount: {_expectedWithdrawalAmountInEro}");

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
                    $" expected Proceed Date GMT empty");

                Assert.True(actualDataFromFile.Office != null,
                    $" actual Office : {actualDataFromFile.Office}" +
                    $" expected Office not equal to null");

                Assert.True(actualDataFromFile.Country == _expectedCountry,
                    $" actual Client Country : {actualDataFromFile.Country}" +
                    $" expected Client Country: {_expectedCountry}");

                Assert.True(actualDataFromFile.Note == string.Empty,
                    $" actual Note : {actualDataFromFile.Note}" +
                    $" expected Note empty ");             
            });
        }
    }
}