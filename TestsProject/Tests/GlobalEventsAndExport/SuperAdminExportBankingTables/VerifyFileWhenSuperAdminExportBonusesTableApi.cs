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
    public class VerifyFileWhenSuperAdminExportBonusesTableApi : TestSuitBase
    {
        #region Test Preparation
        private IApplicationFactory _apiFactory = new ApplicationFactory();
        private string _crmUrl = Config.appSettings.CrmUrl;
        private string _clientName;       
        private string _clientEmail;
        private string _clientId;
        private string _clientCurrency = DataRep.DefaultUSDCurrencyName;
        private string _apiKey = Config.appSettings.ApiKey;
        private string _campaignName;
        private static string _date = DateTime.Now.ToString("dd.MM.yy");
        private string _expectedFileName = $"bonuses_airsoftltd.com_{_date}_{_date}.csv";
        private int _bonosAmount = 1000;
        private decimal _expectedBonusAmountInEro;
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
            var userId = _apiFactory
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

            _apiFactory
                .ChangeContext<IClientsApi>()
                .PatchMassAssignSaleAgentsRequest(_crmUrl, userId,
                new List<string> { _clientId });

            // create bonus
            _apiFactory
                .ChangeContext<IFinancesTabApi>()
                .PostBonusRequest(_crmUrl, _clientId, actualAmount: _bonosAmount);

            _exportLink = $"{ApiRouteAggregate.PostExportBankingTables("deposit_bonus")}" +
                 $"?draw=4&order[0][column]=created_at&order[0][dir]" +
                 $"=desc&start=0&length=10000000&search[value]={_clientEmail}" +
                 $"&search[regex]=true&filter[start_date]={_today} 00:00:00&filter" +
                 $"[end_date]={_today} 23:59:59&export=1&table=bonuses";

            // USD  in EUR
            _expectedBonusAmountInEro = _apiFactory
                .ChangeContext<IGeneral>()
                .PostCurrencyConversionRequest(_crmUrl,
                _bonosAmount, DataRep.DefaultUSDCurrencyName, "EUR");
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
        public void VerifyFileWhenSuperAdminExportBonusesTableApiTest()
        {                    
            var expectedDateOfBonus = DateTime.Now.ToString("dd/MM/yyyy");
            var expectedMethod = "Bonus";
            var expectedClientCurrency = DataRep.DefaultUSDCurrencyName;

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
                    $" actual id : {actualDataFromFile.ID}" +
                    $" expected id not null");

                Assert.True(actualDataFromFile.DateOfBonusGMT?.Contains(expectedDateOfBonus),
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

                Assert.True(actualDataFromFile.SalesAgent == _userName,
                    $" actual Sales Agent : {actualDataFromFile.SalesAgent}" +
                    $" expected Sales Agent : {_userName}");

                Assert.True(actualDataFromFile.Status == "approved",
                    $" actual Status : {actualDataFromFile.Status}" +
                    $" expected Status approved");

                Assert.True(actualDataFromFile.Campaign == _campaignName,
                    $" actual Campaign : {actualDataFromFile.Campaign}" +
                    $" expected Campaign: {_campaignName}");

                Assert.True(actualDataFromFile.OriginalAmount == _bonosAmount.ToString(),
                    $" actual bonos Amount : {actualDataFromFile.OriginalAmount}" +
                    $" expected bonos Amount: {_bonosAmount}");

                Assert.True(actualDataFromFile.OriginalCurrency == expectedClientCurrency,
                    $" actual Original Currency : {actualDataFromFile.OriginalCurrency}" +
                    $" expected Original Currency : {expectedClientCurrency}");

                Assert.True(_expectedBonusAmountInEro.MathRoundFromGeneric(0)
                    == actualDataFromFile.EuroAmount.MathRoundFromGeneric(0),
                    $" actual Euro Amount : {actualDataFromFile.EuroAmount}" +
                    $" expected Euro Amount: {_expectedBonusAmountInEro}");

                Assert.True(actualDataFromFile.Method == expectedMethod,
                    $" actual Method : {actualDataFromFile.Method}" +
                    $" expected Method {expectedMethod}");              
             });
        }
    }
}