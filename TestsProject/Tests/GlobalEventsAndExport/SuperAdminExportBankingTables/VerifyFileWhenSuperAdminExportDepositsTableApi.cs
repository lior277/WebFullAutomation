// Ignore Spelling: Api Admin

using System;
using System.Collections.Generic;
using System.Linq;
using AirSoftAutomationFramework.Internals.DAL.ApiAccsess.ApiRoutes;
using AirSoftAutomationFramework.Internals.DAL.ConfigurationManagement;
using AirSoftAutomationFramework.Internals.ExtensionsMethods;
using AirSoftAutomationFramework.Internals.Factorys;
using AirSoftAutomationFramework.Internals.Helpers;
using AirSoftAutomationFramework.Internals.NunitAtributes;
using AirSoftAutomationFramework.Objects.DTOs;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.AccountsApi.UsersMenuApi.UserMenuApi;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.Clients.ClientCard.FinancesTab;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.Clients.ClientPage;
using AirSoftAutomationFramework.Objects.ForexObjects.ApiGeneral;
using NUnit.Framework;
using TestsProject.TestsInternals;

namespace TestsProject.Tests.GlobalEventsAndExport.SuperAdminExportBankingTables
{
    [TestFixture]
    public class VerifyFileWhenSuperAdminExportDepositsTableApi : TestSuitBase
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
        private decimal _expectedDepositAmountInEro;
        private Dictionary<string, string> _campaignData;
        private string _today = DateTime.Now.ToString("yyyy-MM-dd");
        private string _userEmail;
        private string _userName;
        private string _exportLink;

        [SetUp]
        public void SetUp()
        {
            BeforeTest();

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
                .CreateClientWithCampaign(_crmUrl, _clientName, campaignId,
                currency: _clientCurrency);

            var clientsIs = new List<string> { _clientId };

            #region connect One User To One Client notification
            // connect One User To One Client notification
            _apiFactory
                .ChangeContext<IClientsApi>()
                .PatchMassAssignSaleAgentsRequest(_crmUrl, _userId, clientsIs);
            #endregion

            // deposit 
            _apiFactory
                .ChangeContext<IFinancesTabApi>()
                .PostDepositRequest(_crmUrl, _clientId, _depositAmount);

            _exportLink = $"{ApiRouteAggregate.PostExportBankingTables("deposit")}" +
                $"?draw=12&order[0][column]=order_id&order[0][dir]=desc&start=0&length" +
                $"=10000000&search[value]={_clientEmail}&search[regex]=true&filter[status]" +
                $"[]=approved&filter[start_date]={_today} 00:00:00&filter[end_date]=" +
                $"{_today} 23:59:59&export=1&table=deposits";

            // USD  in EUR
            _expectedDepositAmountInEro = _apiFactory
                .ChangeContext<IGeneral>()
                .PostCurrencyConversionRequest(_crmUrl, _depositAmount,
                DataRep.DefaultUSDCurrencyName, "EUR");
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
        public void VerifyFileWhenSuperAdminExportDepositsTableApiTest()
        {
            var expectedDateOfDeposit = DateTime.Now.ToString("dd/MM/yyyy");
            var expectedMethod = "Bank Transfer: bank_transfer";
            var expectedDisplayName = "bank transfer";

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
                Assert.True(actualDataFromFile.ID != "0",
                     $" actual id : {actualDataFromFile.Id}" +
                     $" expected id not null");

                Assert.True(actualDataFromFile.OrderId == string.Empty,
                    $" actual Order Id : {actualDataFromFile.ClientId}" +
                    $" expected Order Id: Empty");

                Assert.True(actualDataFromFile.ClientId == _clientId,
                    $" actual id : {actualDataFromFile.ClientId}" +
                    $" expected id: {_clientId}");

                Assert.True(actualDataFromFile.DateOfDepositGMT?.Contains(expectedDateOfDeposit),
                    $" actual Date Of Deposit : {actualDataFromFile.DateOfDepositGMT}" +
                    $" expected Date Of Deposit: {expectedDateOfDeposit}");

                Assert.True(actualDataFromFile.ClientFullName == $"{_clientName}  {_clientName}",
                    $" actual Client Full Name : {actualDataFromFile.ClientFullName}" +
                    $" expected Client Full Name: {_clientName} {_clientName}");

                Assert.True(actualDataFromFile.TransactionId != null,
                    $" actual Transaction Id : {actualDataFromFile.TransactionId}" +
                    $" expected Transaction Id not null");

                Assert.True(actualDataFromFile.Phone != null,
                    $" actual Phone : {actualDataFromFile.Phone}" +
                    $" expected Phone :  not null");

                Assert.True(actualDataFromFile.Email == _clientEmail,
                    $" actual Client Email : {actualDataFromFile.Email}" +
                    $" expected Client Email: {_clientEmail}");

                Assert.True(actualDataFromFile.TotalDeposits == $"{_depositAmount}.00",
                    $" actual Total Deposit : {actualDataFromFile.TotalDeposit}" +
                    $" expected Total Deposit: {_depositAmount}");

                Assert.True(actualDataFromFile.AssignedTo == _userName,
                    $" actual Assigned To : {actualDataFromFile.AssignedTo}" +
                    $" expected Assigned To : {_userName}");

                Assert.True(actualDataFromFile.Campaign == _campaignName,
                    $" actual Campaign : {actualDataFromFile.Campaign}" +
                    $" expected Campaign: {_campaignName}");

                Assert.True(actualDataFromFile.SalesAgent == _userName,
                    $" actual Sales Agent : {actualDataFromFile.SalesAgent}" +
                    $" expected Sales Agent : {_userName}");

                Assert.True(actualDataFromFile.TransactionStatus == "approved",
                    $" actual Transaction Status : {actualDataFromFile.TransactionStatus}" +
                    $" expected Transaction Status: approved");

                Assert.True(actualDataFromFile.Ftd == 1,
                    $" actual Ftd : {actualDataFromFile.Ftd}" +
                    $" expected Ftd = 1");

                Assert.True(actualDataFromFile.DepositAmount == $"{_depositAmount}.00",
                    $" actual deposit Amount : {actualDataFromFile.Amount}" +
                    $" expected deposit Amount : {_depositAmount}");

                Assert.True(actualDataFromFile.Currency == _expectedCurrency,
                    $" actual Client Currency : {actualDataFromFile.Currency}" +
                    $" expected Client Currency: {_expectedCurrency}");

                Assert.True(actualDataFromFile.OriginalAmount == _depositAmount.ToString(),
                    $" actual bonos Amount : {actualDataFromFile.OriginalAmount}" +
                    $" expected bonos Amount: {_depositAmount}");

                Assert.True(actualDataFromFile.OriginalCurrency == _expectedCurrency,
                    $" actual Original Currency : {actualDataFromFile.OriginalCurrency}" +
                    $" expected Original Currency : {_expectedCurrency}");

                Assert.True(_expectedDepositAmountInEro.MathRoundFromGeneric(0)
                    == actualDataFromFile.EuroAmount.MathRoundFromGeneric(0),
                   $" actual Euro Amount : {actualDataFromFile.EuroAmount}" +
                   $" expected Euro Amount: {_expectedDepositAmountInEro}");

                Assert.True(actualDataFromFile.DisplayName == expectedDisplayName,
                    $" actual Display Name : {actualDataFromFile.DisplayName}" +
                    $" expected Display Name : {expectedDisplayName}");

                Assert.True(actualDataFromFile.RejectReason == string.Empty,
                    $" actual Reject Reason : {actualDataFromFile.RejectReason}" +
                    $" expected Reject Reason = null ");

                Assert.True(actualDataFromFile.Office != null,
                    $" actual Office : {actualDataFromFile.Office}" +
                    $" expected Office not equals to null");

                Assert.True(actualDataFromFile.Country == _expectedCountry,
                    $" actual Client Country : {actualDataFromFile.Country}" +
                    $" expected Client Country: {_expectedCountry}");

                Assert.True(actualDataFromFile.Method == expectedMethod,
                    $" actual Method : {actualDataFromFile.Method}" +
                    $" expected Method {expectedMethod}");
            });
        }
    }
}