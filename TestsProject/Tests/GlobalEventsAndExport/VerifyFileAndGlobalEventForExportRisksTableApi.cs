// Ignore Spelling: Api

using System.Collections.Generic;
using System.Linq;
using AirSoftAutomationFramework.Internals.DAL.ConfigurationManagement;
using AirSoftAutomationFramework.Internals.Factorys;
using AirSoftAutomationFramework.Internals.Helpers;
using AirSoftAutomationFramework.Internals.NunitAtributes;
using AirSoftAutomationFramework.Objects.DTOs;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.AccountsApi.UsersMenuApi.UserMenuApi;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.Clients.ClientCard.FinancesTab;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.Clients.ClientPage;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.Settings;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Trade;
using AirSoftAutomationFramework.Objects.ForexObjects.ApiGeneral;
using AirSoftAutomationFramework.Objects.ForexObjects.ApiGeneral.Login;
using AirSoftAutomationFramework.Objects.ForexObjects.TradingPlatform.TradeObjects.TradeApi;
using NUnit.Framework;
using TestsProject.TestsInternals;

namespace TestsProject.Tests.GlobalEventsAndExport
{
    [NonParallelizable]
    [TestFixture]
    public class VerifyFileAndGlobalEventForExportRisksTableApi : TestSuitBase
    {
        #region Test Preparation
        private IApplicationFactory _apiFactory = new ApplicationFactory();
        private string _crmUrl = Config.appSettings.CrmUrl;
        private string _currentUserApiKey;
        private string _clientName;
        private string _clientId;
        private string _exportLink;
        private string _userId;
        private string _expectedCurrency = DataRep.DefaultUSDCurrencyName;
        private int _depositAmount = 10000;
        private string _userEmail;
        private string _tableName = DataRep.RiskTableName;
        private string _groupName;

        [SetUp]
        public void SetUp()
        {
            BeforeTest();
            _groupName = TextManipulation.RandomString();
            var emailPerfix = DataRep.TestimEmailPrefix;

            var tradingPlatformUrl = Config.appSettings.tradingPlatformUrl;

            var openTradeAmount = 2;

            // update Export Table Email Template
            _apiFactory
              .ChangeContext<ISharedStepsGenerator>()
              .UpdateExportTableEmailTemplate(_crmUrl);

            // create user
            var userName = TextManipulation.RandomString();
            _userEmail = userName + DataRep.TestimEmailPrefix;

            _userId = _apiFactory
             .ChangeContext<IUserApi>()
             .PostCreateUserRequest(_crmUrl, userName,
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
            var clientEmail = _clientName + DataRep.EmailPrefix;

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

            // get login Data for trading Platform
            var _loginData = _apiFactory
                .ChangeContext<ILoginApi>()
                .PostLoginToTradingPlatform(tradingPlatformUrl, clientEmail)
                .GeneralResponse;

            // create trade
            _apiFactory
                .ChangeContext<ITradePageApi>()
                .PostBuyAssetRequest(tradingPlatformUrl,
                amount: openTradeAmount, loginData: _loginData);

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

            #region  export risk table
            // export risk table
            _apiFactory
               .ChangeContext<IRiskPageApi>()
               .PostExportRisksTablePipeRequest(_crmUrl, _clientName, _userEmail, _currentUserApiKey);

            // extract the export link from mail body by table name
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
        [Description("based on bug https://airsoftltd.atlassian.net/browse/AIRV2-4587")]
        [Category(DataRep.SanityCategory)]
        [Category(DataRep.RegressionCategory)]
        [RetryMenage(DataRep.NumOfRetraiesForFallingTest)]
        public void VerifyFileAndGlobalEventForExportRisksTableApiTest()
        {
            var expectedEffectedTable = _tableName;
            var expectedType = "download_export_link";
            var expectedActionMadeByUserId = _userId;

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
                Assert.True(actualDataFromFile.FullName == $"{_clientName} {_clientName}",
                    $" actual Full Name : {actualDataFromFile.ClientId}" +
                    $" expected Full Name: {_clientName} {_clientName}");

                Assert.True(actualDataFromFile.SalesAgent.ToLower() == _userEmail.Split('@').First(),
                    $" actual Sales Agent : {actualDataFromFile.SalesAgent}" +
                    $" expected Sales Agent : {_userEmail.Split('@').First()}");

                Assert.True(actualDataFromFile.Deposits == $"{_depositAmount}.00$",
                    $" actual Deposits : {actualDataFromFile.Deposits}" +
                    $" expected Deposits: {_depositAmount}");

                Assert.True(actualDataFromFile.Bonus == "0.00$",
                    $" actual Bonus : {actualDataFromFile.Bonus}" +
                    $" expected Bonus: 0.00$");

                Assert.True(actualDataFromFile.Volume != null,
                    $" actual Volume : {actualDataFromFile.Volume}" +
                    $" expected Volume: not null");

                Assert.True(actualDataFromFile.Balance == $"{_depositAmount}.00$",
                    $" actual Balance : {actualDataFromFile.Balance}" +
                    $" expected Balance: {_depositAmount}");

                Assert.True(actualDataFromFile.PnlCloseTrades == "0.00$",
                    $" actual Pnl Close Trades : {actualDataFromFile.PnlCloseTrades}" +
                    $" expected Pnl Close Trades = 0.00$");

                Assert.True(actualDataFromFile.PnlOpenTrades != null,
                    $" actual Pnl Open Trades : {actualDataFromFile.PnlCloseTrades}" +
                    $" expected Pnl Open Trades not null");

                Assert.True(actualDataFromFile.Swap == "0$",
                    $" actual Total Swap  : {actualDataFromFile.Swap}" +
                    $" expected Total Swap:  0$");

                Assert.True(actualDataFromFile.Available != null,
                    $" actual Available : {actualDataFromFile.Available}" +
                    $" expected Available: not null");

                Assert.True(actualDataFromFile.Equity != null,
                    $" actual Equity : {actualDataFromFile.Equity}" +
                    $" expected Equity: not null");

                Assert.True(actualDataFromFile.MinMargin == null,
                    $" actual Min Margin : {actualDataFromFile.MinMargin}" +
                    $" expected Min Margin: null");

                Assert.True(actualDataFromFile.MarginUsage == 0,
                    $" actual Margin Usage : {actualDataFromFile.MarginUsage}" +
                    $" expected Margin Usage : 0");

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