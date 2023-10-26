// Ignore Spelling: Api

using System;
using System.Collections.Generic;
using System.Linq;
using AirSoftAutomationFramework.Internals.DAL.ConfigurationManagement;
using AirSoftAutomationFramework.Internals.Factory;
using AirSoftAutomationFramework.Internals.Helpers;
using AirSoftAutomationFramework.Internals.NunitAttributes;
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
    public class VerifyFileAndGlobalEventForExportOpenTradesTableApi : TestSuitBase
    {
        #region Test Preparation
        private IApplicationFactory _apiFactory = new ApplicationFactory();
        private string _crmUrl = Config.appSettings.CrmUrl;
        private string _currentUserApiKey;
        private string _clientName;
        private string _clientId;
        private string _exportLink;
        private string _userId;
        private string _tradeId;
        private int _depositAmount = 10000;
        private int _openTradeAmount = 10;
        private string _userEmail;
        private string _tableName = DataRep.OpenTradesTableName;
        private static string _date = DateTime.Now.ToString("dd.MM.yy");
        private string _tradingPlatformUrl =  Config.appSettings.tradingPlatformUrl;
        private string _groupName;

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
            var userName = TextManipulation.RandomString();
            _userEmail = userName + DataRep.TestimEmailPrefix;

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
            var clientEmail = _clientName + DataRep.EmailPrefix;

            _clientId = _apiFactory
                .ChangeContext<ICreateClientApi>()
                .CreateClientRequest(_crmUrl, _clientName,
                apiKey: _currentUserApiKey);

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
                .PostDepositRequest(_crmUrl, clientsIds, _depositAmount);

            // get login Data for trading Platform
            var _loginData = _apiFactory
                .ChangeContext<ILoginApi>()
                .PostLoginToTradingPlatform(_tradingPlatformUrl, clientEmail)
                .GeneralResponse;

            var tradeDetails = _apiFactory
                .ChangeContext<ITradePageApi>()
                .PostBuyAssetRequest(_tradingPlatformUrl,
                amount: _openTradeAmount, loginData: _loginData)
                .GeneralResponse;
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

            _tradeId = tradeDetails.TradeId;

            #region export open trades table
            // export open trades table
            _apiFactory
               .ChangeContext<IOpenTradesPageApi>()
               .ExportOpenTradeTablePipe(_crmUrl, _userEmail, _currentUserApiKey);

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
        public void VerifyFileAndGlobalEventForExportOpenTradesTableApiTest()
        {
            var expectedEffectedTable = _tableName;
            var expectedType = "download_export_link";
            var expectedActionMadeByUserId = _userId;
            var expectedTradeType = "buy";
            char[] MyChar = { 'U', 'S', 'D' };
            var expectedAssetName = DataRep.AssetName.TrimEnd(MyChar);
            var date = DateTime.Now.ToString("dd/MM/yyyy");
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
               .GetGlobalEventsByUserRequest(_crmUrl, _userEmail.Split('@').First(), _currentUserApiKey)
               .FirstOrDefault();

            Assert.Multiple(() =>
            {
                Assert.True(actualDataFromFile.ID == _tradeId,
                    $" actual id : {actualDataFromFile.ID}" +
                    $" expected id: {_tradeId}");

                Assert.True(actualDataFromFile.ClientId.Equals(_clientId),
                    $" actual Client ID : {actualDataFromFile.ClientId}" +
                    $" expected Client ID: {_clientId}");

                Assert.True(actualDataFromFile.ClientName == $"{_clientName}  {_clientName}",
                    $" actual Client Name : {actualDataFromFile.ClientName}" +
                    $" expected Client Name: {_clientName} {_clientName}");

                Assert.True(actualDataFromFile.OpenTimeGMT?.Contains(date),
                    $" actual Open Time : {actualDataFromFile.OpenTimeGMT}" +
                    $" expected Open Time: {date}");

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

                Assert.True(actualDataFromFile.SlTp == "0/0",
                    $" actual Sl  : {actualDataFromFile.SlTp}" +
                    $" expected Sl : 0/0");

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