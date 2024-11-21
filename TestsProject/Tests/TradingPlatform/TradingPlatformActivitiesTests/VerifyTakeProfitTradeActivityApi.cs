// Ignore Spelling: Api

using System.Collections.Generic;
using System.Linq;
using System.Threading;
using AirSoftAutomationFramework.Internals.DAL.ConfigurationManagement;
using AirSoftAutomationFramework.Internals.Factorys;
using AirSoftAutomationFramework.Internals.Helpers;
using AirSoftAutomationFramework.Internals.NunitAtributes;
using AirSoftAutomationFramework.Models;
using AirSoftAutomationFramework.Objects.DTOs;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.AccountsApi.UsersMenuApi.UserMenuApi;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.Clients.ClientCard.FinancesTab;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.Clients.ClientPage;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Trade;
using AirSoftAutomationFramework.Objects.ForexObjects.ApiGeneral;
using AirSoftAutomationFramework.Objects.ForexObjects.ApiGeneral.Login;
using AirSoftAutomationFramework.Objects.ForexObjects.TradingPlatform.TradeObjects.TradeApi;
using NUnit.Framework;
using TestsProject.TestsInternals;

namespace TestsProject.Tests.TradingPlatform.TradingPlatformActivitiesTests
{
    [TestFixture]
    public class VerifyTakeProfitTradeActivityApi : TestSuitBase
    {
        #region Test Preparation
        private IApplicationFactory _apiFactory = new ApplicationFactory();
        private string _crmUrl = Config.appSettings.CrmUrl;
        private string _tradingPlatformUrl =  Config.appSettings.tradingPlatformUrl;
        private string _userName;
        private string _userId;
        private string _tradeId;
        private string _clientId;
        private string _clientName;
        private string _clientEmail;   
        private List<string> _tradeGroupsIdsListForDelete = new List<string>();
        private QaAutomation01Context _dbContext = new QaAutomation01Context();
        private GetLoginResponse _loginData;

        [SetUp]
        public void SetUp()
        {
            BeforeTest();
            #region PreCondition
            var bonusAmount = 10000;
            var tradeAmount = 2;

            // create user
            _userName = TextManipulation.RandomString();

            _userId = _apiFactory
             .ChangeContext<IUserApi>()
             .PostCreateUserRequest(_crmUrl, _userName,
             role: DataRep.AdminWithUsersOnlyRoleName);

            var tradeGroupAttributes = new Default_Attr
            {
                commision = 0,
                leverage = 5,
                maintenance = 1,
                minimum_amount = 1,
                minimum_step = 1,
                spread = -0.1,
                margin_call = null,
                swap_long = 0.04,
                swap_short = 0.04,
                swap_time = "00:00:00",
            };

            // create client 
            _clientName = TextManipulation.RandomString();
            _clientEmail = _clientName + DataRep.EmailPrefix;

            _clientId = _apiFactory
                .ChangeContext<ICreateClientApi>()
                .CreateClientRequest(_crmUrl, _clientName);

            _apiFactory
                .ChangeContext<IFinancesTabApi>()
                .PostBonusRequest(_crmUrl, _clientId, bonusAmount);

            // get login Data for trading Platform
            _loginData = _apiFactory
                 .ChangeContext<ILoginApi>()
                 .PostLoginToTradingPlatform(_tradingPlatformUrl, _clientEmail)
                 .GeneralResponse;

            var tradeDetails = _apiFactory // create trade to retrieve the current rate
                 .ChangeContext<ITradePageApi>()
                 .PostBuyAssetRequest(_tradingPlatformUrl, tradeAmount, _loginData)
                 .GeneralResponse;

            var currentRate = tradeDetails.TradeRate;
            var takeProfitRate = (double)(currentRate + 0.0001); // to open a takeProfit trade

            var tradeData = _apiFactory // create trade with take profit
                .ChangeContext<ITradePageApi>()
                .CreateTakeProfitApi(_tradingPlatformUrl,
                tradeAmount, _loginData, takeProfitRate);

            _tradeId = tradeData.TradeId;

            var  groupData = _apiFactory // Create Cripto Group And Assign It To client
                .ChangeContext<ISharedStepsGenerator>()
                .CreateTradeGroupAndAssignItToClientPipe(_crmUrl,
                tradeGroupAttributes, _clientId);

            //Wait for trade to close
            _apiFactory
                .ChangeContext<ITradePageApi>()
                .WaitForCfdTradeToClose(_tradingPlatformUrl, _tradeId, _loginData);

            var  groupId = groupData.Keys.First();

            _apiFactory
                .ChangeContext<ITradeGroupApi>()
                .DeleteTradeGroupRequest(_crmUrl, groupId);
            #endregion
        }
        #endregion

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
        [RetryMenage(DataRep.NumOfRetriesForFallingTest)]
        public void VerifyTakeProfitTradeActivityApiTest()
        {
            var expectedClientId = _clientId;
            var expectedClientFullName = $"{_clientName} {_clientName}";
            var expectedTradeId = _tradeId;
            var expectedAssetSymbol = DataRep.AssetName;
            var expectedPlatform = "cfd";
            var expectedCloseReason = "trade_tp_close";
            var expectedAssetLabel = DataRep.AssetNameShort;
            var expectedSystemType = "Regular";
            var expectedType = "close_trade_tp_close";
          
            Thread.Sleep(1000);

            var actualCloseTrade = _apiFactory
                .ChangeContext<ITradePageApi>()
                .WaitForActivityToRegister(_tradingPlatformUrl, expectedType, _loginData)
                .GetActivities(_tradingPlatformUrl, _loginData)
                .FirstOrDefault();

            Assert.Multiple(() =>
            {
                Assert.True(actualCloseTrade.user_id == expectedClientId,
                    $" expected Client Id : {expectedClientId}" +
                    $" actual Client Id: {actualCloseTrade.user_id}");

                Assert.True(actualCloseTrade.user_full_name == expectedClientFullName,
                    $" expected Client Full Name: {expectedClientFullName}" +
                    $" actual Client Full Name: {actualCloseTrade.user_full_name}");

                Assert.True(actualCloseTrade.trade_id == expectedTradeId,
                    $" expected Trade Id: {expectedTradeId}" +
                    $" actual Trade Id: {actualCloseTrade.trade_id}");

                Assert.True(actualCloseTrade.asset_symbol == expectedAssetSymbol,
                    $" expected Asset Symbol: {expectedAssetSymbol}" +
                    $" actual Asset Symbol: {actualCloseTrade.asset_symbol}");

                Assert.True(actualCloseTrade.platform == expectedPlatform,
                    $" expected Platform: {expectedPlatform}" +
                    $" actual :Platform: {actualCloseTrade.platform}");

                Assert.True(actualCloseTrade.close_reason == expectedCloseReason,
                    $" expected Close Reason: {expectedCloseReason}" +
                    $" actual Close Reason:{actualCloseTrade.close_reason}");

                Assert.True(actualCloseTrade.asset_label == expectedAssetLabel,
                    $" expected Asset Label: {expectedAssetLabel}" +
                    $" actual Asset Label: {actualCloseTrade.asset_label}");

                Assert.True(actualCloseTrade.type == expectedType,
                    $" expected Type: {expectedType}" +
                    $" actual Type: {actualCloseTrade.type}");

                Assert.True(actualCloseTrade.system_type == expectedSystemType,
                    $" expected system type: {expectedSystemType}" +
                    $" actual system type: {actualCloseTrade.system_type }");
            });
        }
    }
}