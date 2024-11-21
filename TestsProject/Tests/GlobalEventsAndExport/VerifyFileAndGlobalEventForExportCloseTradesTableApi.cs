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
    public class VerifyFileAndGlobalEventForExportCloseTradesTableApi : TestSuitBase
    {
        #region Test Preparation
        private IApplicationFactory _apiFactory = new ApplicationFactory();
        private string _currentUserApiKey;
        private string _clientName;
        private string _clientId;
        private string _exportLink;
        private string _userId;
        private string _tradeId;
        private string _userEmail;
        private int _depositAmount = 10000;
        private int _openTradeAmount = 10;
        private string _tableName = DataRep.CloseTradesTableName;
        private static string _date = DateTime.Now.ToString("dd.MM.yy");
        private string _tradingPlatformUrl =  Config.appSettings.tradingPlatformUrl;
        private string _groupName;
        private string _crmUrl = Config.appSettings.CrmUrl;

        [SetUp]
        public void SetUp()
        {          
            BeforeTest();
            var emailPerfix = DataRep.TestimEmailPrefix;
            _groupName = TextManipulation.RandomString();

            // update Export Table Email Template
            _apiFactory
              .ChangeContext<ISharedStepsGenerator>()
              .UpdateExportTableEmailTemplate(_crmUrl);

            // create user
            var userName = TextManipulation.RandomString();
            _userEmail = userName + emailPerfix;

            // create user
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
            var clientEmail = $"{_clientName}{DataRep.TestimEmailPrefix}";

            _clientId = _apiFactory
                .ChangeContext<ICreateClientApi>()
                .CreateClientRequest(_crmUrl, _clientName,
                emailPrefix: DataRep.TestimEmailPrefix,
                apiKey: _currentUserApiKey);

            // connect One User To One Client notification
            _apiFactory
                .ChangeContext<IClientsApi>()
                .PatchMassAssignSaleAgentsRequest(_crmUrl, _userId,
                new List<string> { _clientId }, apiKey: _currentUserApiKey);

            // deposit 
            _apiFactory
                .ChangeContext<IFinancesTabApi>()
                .PostDepositRequest(_crmUrl, _clientId, _depositAmount);

            // get login Data for trading Platform
            var loginCookies = _apiFactory
                .ChangeContext<ILoginApi>()
                .PostLoginToTradingPlatform(_tradingPlatformUrl, clientEmail)
                .GeneralResponse;

            // buy asset for current rate
            var tradeDetails = _apiFactory
                .ChangeContext<ITradePageApi>()
                .PostBuyAssetRequest(_tradingPlatformUrl,
                amount: _openTradeAmount, loginData: loginCookies)
                .GeneralResponse;

            _tradeId = tradeDetails.TradeId;

            //  close trade
            _apiFactory
                .ChangeContext<IOpenTradesPageApi>()
                .PatchCloseTradeRequest(_crmUrl, tradeDetails.TradeId);

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

            // export close trades table
            _apiFactory
               .ChangeContext<IClosedTradesPageApi>()
               .ExportCloseTradeTablePipe(_crmUrl, _userEmail, _currentUserApiKey);

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
        public void VerifyFileAndGlobalEventForExportCloseTradesTableApiTest()
        {
            var expectedEffectedTable = _tableName;
            var expectedType = "download_export_link";
            var expectedActionMadeByUserId = _userId;
            var expectedTradeType = "buy";
            char[] MyChar = { 'U', 'S', 'D' };
            var expectedAssetName = DataRep.AssetName.TrimEnd(MyChar);
            var expectedOpenTime = DateTime.Now.ToString("dd/MM/yyyy");

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
               .GetGlobalEventsByUserRequest(_crmUrl,  _userEmail.Split('@')
               .First(), _currentUserApiKey)
               .FirstOrDefault();

            Assert.Multiple(() =>
            {
                Assert.True(actualDataFromFile.ID == _tradeId,
                    $" actual id : {actualDataFromFile.ID}" +
                    $" expected id: {_tradeId}");

                Assert.True(actualDataFromFile.ClientId == _clientId,
                    $" actual id : {actualDataFromFile.ClientId}" +
                    $" expected id: {_clientId}");

                Assert.True(actualDataFromFile.ClientName == $"{_clientName}  {_clientName}",
                    $" actual Client Name : {actualDataFromFile.ClientName}" +
                    $" expected Client Name: {_clientName} {_clientName}");

                Assert.True(actualDataFromFile.OpenTimeGMT?.Contains(expectedOpenTime),
                    $" actual Open Time : {actualDataFromFile.OpenTimeGMT}" +
                    $" expected Open Time: {expectedOpenTime}");

                Assert.True(actualDataFromFile.Type == expectedTradeType,
                    $" actual Trade Type: {actualDataFromFile.Type}" +
                    $" expected TradeType: {expectedTradeType}");

                Assert.True(actualDataFromFile.Asset == expectedAssetName,
                    $" actual Asset Name: {actualDataFromFile.Asset}" +
                    $" expected Asset Name: {expectedAssetName}");

                Assert.True(actualDataFromFile.Amount == _openTradeAmount,
                    $" actual open Trade Amount : {actualDataFromFile.Amount}" +
                    $" expected open Trade Amount: {_openTradeAmount}");

                Assert.True(actualDataFromFile.OpenPrice != null,
                    $" actual  Open Price : {actualDataFromFile.OpenPrice}" +
                    $" expected  Price not null");

                Assert.True(actualDataFromFile.Pnl != null,
                    $" actual Pnl : {actualDataFromFile.Pnl}" +
                    $" expected Pnl not null");                                 
     
                Assert.True(actualDataFromFile.CloseTimeGMT?.Contains(expectedOpenTime),
                    $" actual Close Time : {actualDataFromFile.CloseTimeGMT}" +
                    $" expected Close Time: {expectedOpenTime}");

                Assert.True(actualDataFromFile.SlTp == "0/0",
                    $" actual Sl Tp : {actualDataFromFile.SlTp}" +
                    $" expected Sl Tp: 0/0");

                Assert.True(actualDataFromFile.Commission == "0",
                    $" actual Commission : {actualDataFromFile.Commission}" +
                    $" expected Commission = 0");

                Assert.True(actualDataFromFile.Swap == "0",
                    $" actual Commission : {actualDataFromFile.Swap}" +
                    $" expected Commission = 0");

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