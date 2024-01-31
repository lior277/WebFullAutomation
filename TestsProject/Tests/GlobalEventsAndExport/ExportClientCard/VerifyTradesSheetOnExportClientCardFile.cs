using System;
using System.IO;
using AirSoftAutomationFramework.Internals.DAL.ConfigurationManagement;
using AirSoftAutomationFramework.Internals.Factorys;
using AirSoftAutomationFramework.Internals.Helpers;
using AirSoftAutomationFramework.Internals.NunitAtributes;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.AccountsApi.UsersMenuApi.UserMenuApi;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.Clients.ClientCard;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.Clients.ClientCard.FinancesTab;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.Clients.ClientPage;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.Settings;
using AirSoftAutomationFramework.Objects.ForexObjects.ApiGeneral;
using AirSoftAutomationFramework.Objects.ForexObjects.ApiGeneral.Login;
using AirSoftAutomationFramework.Objects.ForexObjects.TradingPlatform.TradeObjects.TradeApi;
using NUnit.Framework;
using OpenQA.Selenium;
using TestsProject.TestsInternals;

namespace TestsProject.Tests.GlobalEventsAndExport.ExportClientCard
{
    [TestFixture(DataRep.Chrome)]
    public class VerifyTradesSheetOnExportClientCardFile : TestSuitBase
    {
        public VerifyTradesSheetOnExportClientCardFile(string browser) : base(browser)
        {
            _browserName = browser;
        }

        #region Test Preparation
        private IApplicationFactory _apiFactory = new ApplicationFactory();
        private string _crmUrl = Config.appSettings.CrmUrl;
        private string _currentUserApiKey;
        private string _userId;
        private int _tradeAmount = 3;
        private int _depositAmount = 10000;
        private string _userName;
        private string _clientEmail;
        private string _clientName;
        private string _clientId;
        private string _tradeId;   
        private Stream _clientCardStream;
        private IWebDriver _driver;
        private string _browserName;

        [SetUp]
        public void SetUp()
        {
            BeforeTest();
            var tradingPlatformUrl = Config.appSettings.tradingPlatformUrl;
            _driver = GetDriver();

            // create user
            _userName = TextManipulation.RandomString();
            var testimEmailPerfix = DataRep.TestimEmailPrefix;
            var userEmail = _userName + testimEmailPerfix;

            // create user
            _userId = _apiFactory
             .ChangeContext<IUserApi>()
             .PostCreateUserRequest(_crmUrl, _userName,
             emailPrefix: testimEmailPerfix,
              role: DataRep.AdminWithUsersOnlyRoleName);

            #region create ApiKey
            // create ApiKey
            _currentUserApiKey = _apiFactory
                .ChangeContext<IUserApi>(_driver)
                .PostCreateApiKeyRequest(_crmUrl, _userId);
            #endregion

            // create client 
            _clientName = TextManipulation.RandomString();
            _clientEmail = _clientName + DataRep.EmailPrefix;

            _clientId = _apiFactory
                .ChangeContext<ICreateClientApi>()
                .CreateClientRequest(_crmUrl, _clientName,
                apiKey: _currentUserApiKey);

            // tp login data
            var loginData = _apiFactory
                .ChangeContext<ILoginApi>(_driver)
                .PostLoginToTradingPlatform(tradingPlatformUrl, _clientEmail)
                .GeneralResponse;

            // create deposit 
            _apiFactory
                .ChangeContext<IFinancesTabApi>(_driver)
                .PostDepositRequest(_crmUrl, _clientId, _depositAmount,
                apiKey: _currentUserApiKey);

            // create trade 
            var tradeData = _apiFactory
                .ChangeContext<ITradePageApi>(_driver)
                .PostBuyAssetRequest(tradingPlatformUrl, _tradeAmount, loginData);

            _tradeId = tradeData.GeneralResponse.TradeId;

            // enter the Email For Export in Super Admin Tub
            var brandRegulation = _apiFactory
               .ChangeContext<ISuperAdminTubApi>()
               .GetBrandRegulationRequest(_crmUrl);

            DataRep.EmailListForExport.Add(userEmail);

            brandRegulation.export_data_email_url = DataRep
                .EmailListForExport.ToArray();

            _apiFactory
                .ChangeContext<ISuperAdminTubApi>()
                .PutRegulationsRequest(_crmUrl, brandRegulation);

            #region  export Client Card
            // export comments tab
            _apiFactory
                .ChangeContext<IClientCardApi>()
                .ExportClientCardPipe(_crmUrl, _clientId,
                userEmail, _currentUserApiKey);

            // extract the export link from mail body
            var exportLink = _apiFactory
                .ChangeContext<ISharedStepsGenerator>()
                .GetExportLinkFromExportEmailBody(_crmUrl, userEmail, "export_link");

            // get the csv file 
            _clientCardStream = _apiFactory
                .ChangeContext<IClientCardApi>(_driver)
                .GetExportClientCardRequest(_crmUrl, exportLink, _currentUserApiKey)
                .Stream;
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
                AfterTest(_driver);
                DriverDispose(_driver);
            }
        }
        #endregion

        [Test]
        [Category(DataRep.SanityCategory)]
        [Category(DataRep.RegressionCategory)]
        [RetryMenage(DataRep.NumOfRetraiesForFallingTest)]
        public void VerifyTradesSheetOnExportClientCardFileTest()
        {
            var expectedDate = DateTime.Now.ToString("yyyy-MM-dd");

            var dataSet = _apiFactory
                .ChangeContext<IFileHandler>(_driver)
                .ConvertXlsxStreamToDataSet(_clientCardStream);         

            // trades table data
            var tradesSheet = _apiFactory
                .ChangeContext<IFileHandler>(_driver)
                .ReadDataFromDataSet(dataSet, "Trades");        

            Assert.Multiple(() =>
            {
                Assert.True(tradesSheet["Id"] == _tradeId,
                    $" actual trade Id : {tradesSheet["Id"]}" +
                    $" expected trade id: {_tradeId}");

                Assert.True(tradesSheet["Open time | GMT"].Contains(expectedDate),
                    $" actual Open time | GMT : {tradesSheet["Open time | GMT"]}" +
                    $" expected Open time | GMT: {expectedDate}");

                Assert.True(tradesSheet["Asset"] == DataRep.AssetNameShort,
                    $" actual Asset : {tradesSheet["Asset"]}" +
                    $" expected Asset: {DataRep.AssetNameShort}");

                Assert.True(tradesSheet["Platform"] == DataRep.Platform,
                    $" actual Platform : {tradesSheet["Platform"]}" +
                    $" expected Platform: {DataRep.Platform}");

                Assert.True(tradesSheet["Type"] == "buy",
                    $" actual Type : {tradesSheet["Type"]}" +
                    $" expected Type: buy");

                Assert.True(tradesSheet["Amount"] == $"{_tradeAmount}.00",
                    $" actual Amount : {tradesSheet["Amount"]}" +
                    $" expected Amount: {_tradeAmount}");

                Assert.True(tradesSheet["Status"] == "open",
                    $" actual Type : {tradesSheet["Status"]}" +
                    $" expected Type: open");

                Assert.True(tradesSheet["Open price"] != null,
                    $" actual Open price : {tradesSheet["Open price"]}" +
                    $" expected Open price: not null");

                Assert.True(tradesSheet["Close price"] != null,
                    $" actual Close price : {tradesSheet["Close price"]}" +
                    $" expected Close price: not null");

                Assert.True(tradesSheet["Required margin"] != null,
                    $" actual Required margin : {tradesSheet["Required margin"]}" +
                    $" expected Required margin: not null");

                Assert.True(tradesSheet["Close time | GMT"] == "N/A",
                    $" actual Close time | GMT : {tradesSheet["Close time | GMT"]}" +
                    $" expected Close time | GMT: N/A");

                Assert.True(tradesSheet["Close reason"] != null,
                    $" actual Close reason : {tradesSheet["Close reason"]}" +
                    $" expected Close reason: not null");

                Assert.True(tradesSheet["PNL Real"] != null,
                    $" actual PNL Real : {tradesSheet["PNL Real"]}" +
                    $" expected PNL Real: not null");

                Assert.True(tradesSheet["PNL Bonus"] == "-0.00",
                    $" actual PNL Bonus : {tradesSheet["PNL Bonus"]}" +
                    $" expected PNL Bonus: 0");

                Assert.True(tradesSheet["Commission"] == "0",
                    $" actual Commission : {tradesSheet["Commission"]}" +
                    $" expected Commission: 0");

                Assert.True(tradesSheet["Swap commission"] == "0",
                    $" actual Swap commission : {tradesSheet["Swap commission"]}" +
                    $" expected Swap commission: 0");

                Assert.True(tradesSheet["Take profit"] == "0",
                    $" actual Take profit : {tradesSheet["Take profit"]}" +
                    $" expected Take profit: 0");

                Assert.True(tradesSheet["Stop loss"] == "0",
                    $" actual Stop loss : {tradesSheet["Stop loss"]}" +
                    $" expected Stop loss: 0");

                Assert.True(tradesSheet["Error"] == "N/A",
                    $" actual Error : {tradesSheet["Error"]}" +
                    $" expected Error: N/A");
            });
        }
    }
}