// Ignore Spelling: TimeLine

using AirSoftAutomationFramework.Internals.DAL.ConfigurationManagement;
using AirSoftAutomationFramework.Internals.ExtensionsMethods;
using AirSoftAutomationFramework.Internals.Helpers;
using AirSoftAutomationFramework.Objects.DTOs;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.AccountsApi.Roles;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.AccountsApi.UsersMenuApi.UserMenuApi;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.Clients.ClientCard;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.Clients.ClientCard.FinancesTab;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.Clients.ClientPage;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Trade;
using AirSoftAutomationFramework.Objects.ForexObjects.ApiGeneral;
using AirSoftAutomationFramework.Objects.ForexObjects.ApiGeneral.Login;
using AirSoftAutomationFramework.Objects.ForexObjects.TradingPlatform.TradeObjects.TradeApi;
using AirSoftAutomationFramework.Objects.ForexObjects.TradingPlatform.TradeObjects.Ui;
using AirSoftAutomationFramework.Objects.ForexObjects.Ui.Crm.ClientsPage;
using AirSoftAutomationFramework.Objects.ForexObjects.Ui.Crm.ClientsPage.ClientCard;
using AirSoftAutomationFramework.Objects.ForexObjects.Ui.Login;
using AirSoftAutomationFramework.Objects.ForexObjects.Ui.UiGeneral;
using AngleSharp.Text;
using NUnit.Framework;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using AirSoftAutomationFramework.Internals.Factorys;
using AirSoftAutomationFramework.Internals.NunitAtributes;
using TestsProject.TestsInternals;
using static AirSoftAutomationFramework.Internals.Enums.EnumFactory;

namespace TestsProject.Tests.ClientCard.TimeLine
{
    [TestFixture(DataRep.Chrome)]
    public class VerifyTradingActivitiesInTimeline : TestSuitBase
    {
        #region Test Preparation
        public VerifyTradingActivitiesInTimeline(string browser) : base(browser)
        {
            _browserName = browser;
        }

        #region members
        private IApplicationFactory _apiFactory = new ApplicationFactory();
        private string _crmUrl = Config.appSettings.CrmUrl;
        private string _userName;
        private string _clientId;
        private int _depositAmount = 800000;
        private string _fastLoginTradeId;
        private string _fastLoginPendingTradeId;
        private string _marginCloseTradeId;
        private string _takeProfitTradeId;
        private string _tradeIdForClose;
        private string _tradeIdForMarginClose;
        private string _tradeIdForTakeProfit;
        private string _tradeIdForStopLoss;
        private string _stopLossTradeId;
        private string _closePendingTradeId;
        private string _tradeIdForClosePendingTrade;
        private string _pendingActivationTradeId;
        private string _tradeIdForPendingActivation;
        private string _tradingPlatformUrl =
             Config.appSettings.tradingPlatformUrl;

        private GetLoginResponse _loginData;
        private GetRoleByNameResponse _roleData;
        private double _rateForPending;
        private double _currentRate;
        private string _tradeAssetName = DataRep.AssetName;
        private string _tradeAssetNameForUi = DataRep.AssetNameShort;
        private string _groupName;
        private string _clientName;
        private string _roleName;
        private int _tradeAmount = 2;
        private int _tradeAmountFastLoginTrade = 11;
        private int _tradeAmountPendingFastLoginTrade = 12;
        private IDictionary<string, string> _groupData;
        private List<string> _tradeGroupsIdsListForDelete = new List<string>();
        private IWebDriver _driver;
        private string _browserName;
        #endregion

        [SetUp]
        public void SetUp()
        {
            BeforeTest(_browserName);

            #region PreCondition
            var tradingPlatformUrl =
                 Config.appSettings.tradingPlatformUrl;

            _driver = GetDriver();
            _roleName = TextManipulation.RandomString();

            // get role by name
            _roleData = _apiFactory
                .ChangeContext<IRolesApi>()
                .GetRoleByNameRequest(_crmUrl, DataRep.AdminRole);

            _roleData.Name = _roleName;
            _roleData.ErpPermissions.Add("edit_swap");

            // create role 
            _apiFactory
                .ChangeContext<IRolesApi>()
                .PostCreateRoleRequest(_crmUrl, _roleData);

            // create user for the creation of api key
            _userName = TextManipulation.RandomString();

            var userId = _apiFactory
                .ChangeContext<IUserApi>(_driver)
                .CreateUserForUiPipe(_crmUrl, _userName, role: _roleName);

            _apiFactory
                .ChangeContext<ILoginPageUi>(_driver)
                .LoginPipe(_userName);

            // get ApiKey
            var userApiKey = _apiFactory
                .ChangeContext<IUserApi>(_driver)
                .PostCreateApiKeyRequest(_crmUrl, userId);

            // create client 
            _clientName = TextManipulation.RandomString();
            var clientEmail = _clientName + DataRep.EmailPrefix;

            _clientId = _apiFactory
                .ChangeContext<ICreateClientApi>()
                .CreateClientRequest(_crmUrl, _clientName, apiKey: userApiKey);

            var clientIds = new List<string> { _clientId };
            _clientName = _clientName.UpperCaseFirstLetter();

            // get login cookies
            _loginData = _apiFactory
                .ChangeContext<ILoginApi>(_driver)
                .PostLoginToTradingPlatform(_tradingPlatformUrl, clientEmail)
                .GeneralResponse;

            // create deposit 
            _apiFactory
                .ChangeContext<IFinancesTabApi>(_driver)
                .PostDepositRequest(_crmUrl, clientIds,
                _depositAmount, apiKey: userApiKey);

            #region Create trade for activate pending trade
            var tradeGroupSpreadMinosAttributes = new Default_Attr
            {
                commision = 0,
                leverage = 1000,
                maintenance = 0.1,
                minimum_amount = 1,
                minimum_step = 1,
                spread = -1,
                margin_call = null,
                swap_long = 0.04,
                swap_short = 0.04,
                swap_time = "00:00:00",
            };

            var tradeGroupSpreadTenAttributes = new Default_Attr
            {
                commision = 0,
                leverage = 1,
                maintenance = 0.1,
                minimum_amount = 1,
                minimum_step = 1,
                spread = 0.1,
                margin_call = null,
                swap_long = 0.04,
                swap_short = 0.04,
                swap_time = "00:00:00",
            };

            // Create Cripto Group And Assign It To client
            _groupData = _apiFactory
                 .ChangeContext<ISharedStepsGenerator>(_driver)
                 .CreateTradeGroupAndAssignItToClientPipe(_crmUrl,
                 tradeGroupSpreadMinosAttributes, _clientId);

            _tradeGroupsIdsListForDelete.Add(_groupData.Keys.First());

            // create trade
            var tradeDetails = _apiFactory
                .ChangeContext<ITradePageApi>()
                .PostBuyAssetRequest(_tradingPlatformUrl, _tradeAmount,
                _loginData, assetSymble: _tradeAssetName)
                .GeneralResponse;

            _tradeIdForPendingActivation = tradeDetails.TradeId;
            _currentRate = tradeDetails.TradeRate;
            _rateForPending = _currentRate + 100; // to open a pending trade  

            // edit trade on client card
            _apiFactory
                .ChangeContext<ITradesTabApi>()
                .PachtEditSwapByTradeIdRequest(_tradingPlatformUrl, _tradeIdForPendingActivation,
                0.1, 0.1, 0.1, userApiKey);

            // close the trade
            _apiFactory
                .ChangeContext<ITradePageApi>()
                .PatchCloseTradeRequest(_tradingPlatformUrl,
                _tradeIdForPendingActivation, _tradeAmount, _loginData);

            _apiFactory
               .ChangeContext<ITradePageApi>()
               .WaitForCfdTradeToClose(_tradingPlatformUrl, _tradeIdForPendingActivation, _loginData);

            // open the pending trade
            tradeDetails = _apiFactory
                .ChangeContext<ITradePageApi>()
                .PostPendingBuyOrderRequest(_tradingPlatformUrl,
                _tradeAmount, _loginData, _rateForPending);

            _pendingActivationTradeId = tradeDetails.TradeId;

            // Create Cripto Group And Assign It To client to open the pending trade
            _groupData = _apiFactory
                .ChangeContext<ISharedStepsGenerator>(_driver)
                .CreateTradeGroupAndAssignItToClientPipe(_crmUrl,
                tradeGroupSpreadTenAttributes, _clientId);

            // wait for pending to open
            _apiFactory
               .ChangeContext<ITradePageApi>()
               .WaitForPendingTradeToChangeStatusToOpen(_crmUrl, _loginData);

            _tradeGroupsIdsListForDelete.Add(_groupData.Keys.First());
            #endregion

            #region Stop loss trade close
            var tradeGroupAttributes = new Default_Attr
            {
                commision = 0,
                leverage = 5,
                maintenance = 1,
                minimum_amount = 1,
                minimum_step = 1,
                spread = 0.1,
                margin_call = null,
                swap_long = 0.04,
                swap_short = 0.04,
                swap_time = "00:00:00",
            };

            tradeDetails = _apiFactory // create trade to retrieve the current rate
                .ChangeContext<ITradePageApi>()
                .PostBuyAssetRequest(_tradingPlatformUrl, _tradeAmount, _loginData)
                .GeneralResponse;

            _tradeIdForStopLoss = tradeDetails.TradeId;
            _currentRate = tradeDetails.TradeRate;
            var stopLossRate = (_currentRate - 0.0001); // to open a takeProfit trade

            // close the trade
            _apiFactory
                .ChangeContext<ITradePageApi>()
                .PatchCloseTradeRequest(_tradingPlatformUrl,
                _tradeIdForStopLoss, _tradeAmount, _loginData);

            tradeDetails = _apiFactory // create trade with stop loss
                .ChangeContext<ITradePageApi>()
                .CreateStopLossApi(_tradingPlatformUrl, _tradeAmount, _loginData, stopLossRate);

            _stopLossTradeId = tradeDetails.TradeId;

            // Create Cripto Group And Assign It To client
            _groupData = _apiFactory
                .ChangeContext<ISharedStepsGenerator>()
                .CreateTradeGroupAndAssignItToClientPipe(_crmUrl,
                tradeGroupAttributes, _clientId);

            _tradeGroupsIdsListForDelete.Add(_groupData.Keys.First());
            #endregion

            #region Cancel pending trade
            // create trade 
            tradeDetails = _apiFactory
                .ChangeContext<ITradePageApi>()
                .PostBuyAssetRequest(_tradingPlatformUrl, _tradeAmount, _loginData)
                .GeneralResponse;

            _tradeIdForClosePendingTrade = tradeDetails.TradeId;

            // close the trade
            _apiFactory
                .ChangeContext<ITradePageApi>()
                .PatchCloseTradeRequest(_tradingPlatformUrl,
                _tradeIdForClosePendingTrade, _tradeAmount, _loginData);

            var currentRate = tradeDetails.TradeRate;
            var rateForPending = (double)(currentRate + 100); // to open a pending trade

            // create pending trade
            tradeDetails = _apiFactory
               .ChangeContext<ITradePageApi>()
               .PostPendingBuyOrderRequest(_tradingPlatformUrl,
               _tradeAmount, _loginData, rateForPending);

            // close trade 
            _closePendingTradeId = tradeDetails.TradeId;

            _apiFactory
              .ChangeContext<IOpenTradesPageApi>()
              .PatchCloseTradeRequest(_crmUrl, _closePendingTradeId, userApiKey);

            Thread.Sleep(1000); // wait for pending trade to cancel
            #endregion

            #region Take profit trade close
            tradeGroupAttributes = new Default_Attr
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

            // get group id by name
            var bronzeGroupId = _apiFactory
                .ChangeContext<ITradeGroupApi>()
                .GetTradeGroupsRequest(_crmUrl)
                .GeneralResponse
                .Where(p => p.name == "Default")
                .FirstOrDefault()
                ._id;

            // create trade group
            _groupName = _groupData.Values.First();

            _apiFactory
                .ChangeContext<IInformationTabApi>()
                .PatchSetTradingGroupRequest(_crmUrl, bronzeGroupId, clientIds, userApiKey);

            // create trade to retrieve the current rate
            tradeDetails = _apiFactory 
                .ChangeContext<ITradePageApi>()
                .PostBuyAssetRequest(_tradingPlatformUrl, _tradeAmount, _loginData)
                .GeneralResponse;

            _tradeIdForTakeProfit = tradeDetails.TradeId;
            currentRate = tradeDetails.TradeRate;
            var takeProfitRate = currentRate + 0.0001; // to open a takeProfit trade

            // close the trade
            _apiFactory
                .ChangeContext<ITradePageApi>()
                .PatchCloseTradeRequest(_tradingPlatformUrl,
                _tradeIdForTakeProfit, _tradeAmount, _loginData);

            // create trade with take profit
            tradeDetails = _apiFactory 
                .ChangeContext<ITradePageApi>()
                .CreateTakeProfitApi(_tradingPlatformUrl,
                _tradeAmount, _loginData, takeProfitRate);

            _takeProfitTradeId = tradeDetails.TradeId;

            // Create Cripto Group And Assign It To client
            _groupData = _apiFactory
                .ChangeContext<ISharedStepsGenerator>()
                .CreateTradeGroupAndAssignItToClientPipe(_crmUrl,
                tradeGroupAttributes, _clientId);

            // wait for pending to open
            _apiFactory
               .ChangeContext<ITradePageApi>()
               .WaitForCfdTradeToClose(_crmUrl, _takeProfitTradeId, _loginData);

           Thread.Sleep(3000); // wait for tp to close email

            _tradeGroupsIdsListForDelete.Add(_groupData.Keys.First());
            #endregion

            #region Trade margin close
            var tradeGroupSpreadZeroAttributes = new Default_Attr
            {
                commision = 0,
                leverage = 1000,
                maintenance = 0.1,
                minimum_amount = 1,
                minimum_step = 1,
                spread = 0,
                margin_call = null,
                swap_long = 0.04,
                swap_short = 0.04,
                swap_time = "00:00:00",
            };

            tradeGroupSpreadTenAttributes = new Default_Attr
            {
                commision = 0,
                leverage = 1,
                maintenance = 0.1,
                minimum_amount = 1,
                minimum_step = 1,
                spread = 0.1,
                margin_call = null,
                swap_long = 0.04,
                swap_short = 0.04,
                swap_time = "00:00:00",
            };

            // create trade to retrieve the current rate
            tradeDetails = _apiFactory
                 .ChangeContext<ITradePageApi>()
                 .PostBuyAssetRequest(tradingPlatformUrl, _tradeAmount, _loginData)
                 .GeneralResponse;

            _tradeIdForMarginClose = tradeDetails.TradeId;
            currentRate = tradeDetails.TradeRate;

            // close the trade
            _apiFactory
                .ChangeContext<ITradePageApi>()
                .PatchCloseTradeRequest(_tradingPlatformUrl,
                _tradeIdForMarginClose, _tradeAmount, _loginData);

            // Create Cripto Group And Assign It To client
            _groupData = _apiFactory
                .ChangeContext<ISharedStepsGenerator>()
                .CreateTradeGroupAndAssignItToClientPipe(_crmUrl,
                tradeGroupSpreadZeroAttributes, _clientId);

            _tradeGroupsIdsListForDelete.Add(_groupData.Keys.First());

            // _balance = 100 * leverage = 1000 = 100000 - 1000
            var tradeAmountForMargin = Convert.ToInt32(90000 / currentRate);

            // create trade for client with trade group "tradeGroupSreadZeroAttributes"
            tradeDetails = _apiFactory
                .ChangeContext<ITradePageApi>()
                .PostBuyAssetRequest(tradingPlatformUrl, tradeAmountForMargin, _loginData)
                .GeneralResponse;

            _marginCloseTradeId = tradeDetails.TradeId;

            // close the trade
            _apiFactory
                .ChangeContext<ITradePageApi>()
                .PatchCloseTradeRequest(_tradingPlatformUrl,
                _marginCloseTradeId, _tradeAmount, _loginData);

            // change the trade group in order to automaticly close the trade
            _groupData = _apiFactory
              .ChangeContext<ISharedStepsGenerator>()
              .CreateTradeGroupAndAssignItToClientPipe(_crmUrl,
              tradeGroupSpreadTenAttributes, _clientId);

            _tradeGroupsIdsListForDelete.Add(_groupData.Keys.First());
            #endregion

            #region Open trade/close trade/reopen trade/edit trade/delete trade
            // create trade
            tradeDetails = _apiFactory
                .ChangeContext<ITradePageApi>()
                .PostBuyAssetRequest(_tradingPlatformUrl, _tradeAmount,
                _loginData, assetSymble: _tradeAssetName)
                .GeneralResponse;

            // close trade 
            _tradeIdForClose = tradeDetails.TradeId;

            _apiFactory
                .ChangeContext<ITradePageApi>()
                .PatchCloseTradeWithAmountRequest(_tradingPlatformUrl,
                _tradeIdForClose, _loginData, _tradeAmount);

            // reopen the close trade
            _apiFactory
                .ChangeContext<ITradesTabApi>()
                .PachtReOpenTradeRequest(_crmUrl, _tradeIdForClose);

            // delete the trade
            _apiFactory
                .ChangeContext<ITradesTabApi>()
                .DeleteTradeRequest(_crmUrl, _tradeIdForClose);
            #endregion

            #region Fast login/open trade/open trade pending

            // login
            _apiFactory
                .ChangeContext<IMenus>(_driver)
                .ClickOnMenuItem<IClientsPageUi>()
                .SearchClientByEmail(_clientName)
                .ClickOnClientFullName()
                .ClickOnFastLoginBtn();

            // create fast login trade
            _apiFactory
                .ChangeContext<ITradePageUi>(_driver)
                .BuyAssetPipe(_tradeAssetNameForUi, _tradeAmountFastLoginTrade, false);

            Thread.Sleep(1000); // dont delete wait for trade to open

            var tradeData = _apiFactory
                .ChangeContext<ITradePageApi>()
                .GetTradesByStatusRequest(tradingPlatformUrl, _loginData, "open")
                .GeneralResponse
                .Where(p => p.amount == _tradeAmountFastLoginTrade)
                .FirstOrDefault();

            _currentRate = tradeData.rate;
            _fastLoginTradeId = tradeData.id;
            _rateForPending = (_currentRate + 100)
                .ToString()
                .Split('.')
                .First()
                .ToDouble(); // to open a pending trade  

            // close the trade
            _apiFactory
                .ChangeContext<ITradePageApi>()
                .PatchCloseTradeRequest(_tradingPlatformUrl,
                _fastLoginTradeId, _tradeAmount, _loginData);

            // create pending trade
            _apiFactory
                .ChangeContext<ITradePageUi>(_driver)
                .CreateConditionalTradePipe(_rateForPending.ToString(), _tradeAssetNameForUi,
                 _tradeAmountPendingFastLoginTrade, verify: false);

            _apiFactory
                .ChangeContext<ITradePageApi>()
                .WaitForPendingTrade(tradingPlatformUrl, _loginData);

            _fastLoginPendingTradeId = _apiFactory
                .ChangeContext<ITradesTabApi>()
                .GetTradesRequest(_crmUrl, _clientId)
                .GeneralResponse
                .Where(p => p.amount == _tradeAmountPendingFastLoginTrade)
                .FirstOrDefault()
                .id;

            // switch to crm
            _apiFactory
                .ChangeContext<IGeneral>(_driver)
                .SwitchToExistingWindow(TabToSwitch.First);

            _apiFactory
                .ChangeContext<IClientCardUi>(_driver)
                .ClickOnTimelineTab()
                .ChangeContext<IClientCardUi>(_driver)
                .ClickOnCaptureTab()
                .ClickOnTimelineTab()
                .ChangeContext<IClientCardUi>(_driver)
                .ClickOnCaptureTab()
                .ClickOnTimelineTab()
                .ChangeContext<ITimelineTabUi>(_driver)
                .SetNumOfLines();
            #endregion

            #endregion
        }

        [TearDown]
        public void TearDown()
        {
            try
            {
                _apiFactory
                    .ChangeContext<ITradeGroupApi>()
                    .DeleteTradeGroupRequest(_crmUrl, _tradeGroupsIdsListForDelete,
                    checkStatusCode: false);
            }
            finally
            {
                AfterTest(_driver);
                DriverDispose(_driver);
            }
        }
        #endregion

        // change_activation_status
        // open trade/open trade pending/activate pending trade
        // open trade/open trade pending/cancel pending trade
        // create crypto group/open trade/change crypto group/close trade tp close
        // create crypto group/open trade/change crypto group/close trade sl close
        // create crypto group/open trade/change crypto group/trade margin close
        // open trade/close trade/reopen trade/edit trade/delete trade
        // fast login/open trade/open trade pending
        [Test]
        [Category(DataRep.SanityCategory)]
        [Category(DataRep.RegressionCategory)]
        [RetryMenage(DataRep.NumOfRetriesForFallingTest)]
        public void VerifyTradingActivitiesInTimelineTest()
        {
            #region Expected data
            var expectedDate = DateTime.Now.ToString("ddd MMM dd yyy");
            var expectedTimelineData = new List<string>();

            expectedTimelineData.Add($"{_userName} opened trade pending asset {_tradeAssetName} trade id");

            expectedTimelineData.Add($"{_userName} has edited trade: trade id: : {_tradeIdForPendingActivation} Asset Name:" +
                $" {_tradeAssetName} Swap Commission: from 0 to 0.1Swap Long: from 0.04 to 0.1Swap Short: from 0.04 to 0.1");

            expectedTimelineData.Add($"{_userName} opened trade asset {_tradeAssetName}, trade id");
            expectedTimelineData.Add($"{_userName} Has logged into account");
            expectedTimelineData.Add($"{_clientName} {_clientName} Close Trade asset {_tradeAssetName}, trade id");
            expectedTimelineData.Add($"{_clientName} {_clientName} opened trade asset {_tradeAssetName}, trade id");
            expectedTimelineData.Add($"{_clientName} {_clientName} opened trade asset {_tradeAssetName}, trade id");
            expectedTimelineData.Add($"{_clientName} {_clientName} opened trade asset {_tradeAssetName}, trade id");
            expectedTimelineData.Add($"trade id: {_takeProfitTradeId} Has closed due to take profit");
            expectedTimelineData.Add("Email trade tp close has been sent to client by System");
            expectedTimelineData.Add($"{_clientName} {_clientName} opened trade asset {_tradeAssetName}, trade id");
            expectedTimelineData.Add($"{_clientName} {_clientName} opened trade asset {_tradeAssetName}, trade id");
            expectedTimelineData.Add($"{_userName} changed trading group from {_groupName} to Default");
            expectedTimelineData.Add($"trade id: {_stopLossTradeId} Has closed due to stop loss");
            expectedTimelineData.Add($"{_userName} Pending Trade Cancelled asset, {_tradeAssetName} trade id");
            expectedTimelineData.Add("System Has sent email trade_sl_close to client");
            expectedTimelineData.Add("Email trade order activated has been sent to client by System");
            expectedTimelineData.Add($"{_clientName} {_clientName} opened trade pending asset {_tradeAssetName}, trade id");
            expectedTimelineData.Add($"{_clientName} {_clientName} opened trade asset {_tradeAssetName}, trade id");
            expectedTimelineData.Add($"{_clientName} {_clientName} opened trade asset {_tradeAssetName}, trade id");
            expectedTimelineData.Add($"{_clientName} {_clientName} opened trade asset {_tradeAssetName}, trade id");
            expectedTimelineData.Add($"opened trade asset {_tradeAssetName}, trade id");
            expectedTimelineData.Add("Activate pending trade, Trade ID ");
            expectedTimelineData.Add($"{_clientName} {_clientName} opened trade pending asset {_tradeAssetName}, trade id");
            expectedTimelineData.Add($"{_clientName} {_clientName} opened trade asset {_tradeAssetName}, trade id");
            expectedTimelineData.Add("Email first deposit has been sent to client by System");
            expectedTimelineData.Add($"{_userName} Added a Deposit of {_depositAmount}.00 {DataRep.DefaultUSDCurrencyName}");
            expectedTimelineData.Add($"{_clientName} {_clientName} has logged into the trading-platform and is currently online");
            expectedTimelineData.Add($"{_userName} registered from API");
            #endregion

            var timelineDetails = _apiFactory
                 .ChangeContext<ISearchResultsUi>(_driver)
                 .GetSearchResultDetails<SearchResultTimeline>(_clientName, checkNumOfRows: false)
                 .ToList();

            var actualTimelineActions = new List<string>();

            foreach (var item in timelineDetails)
            {
                if ((item.action.Contains("trade id:") && !item.action.Contains("due")
                    && !item.action.Contains("has edited trade")) || item.action.Contains("Trade ID"))
                {
                    actualTimelineActions.Add(item.action.Split(':').First());
                }
                else
                {
                    actualTimelineActions.Add(item.action);
                }
            }

            actualTimelineActions.RemoveAll(x => x.Equals("Email admin deposit has been sent to client by System"));

            // fix the stop loss
            actualTimelineActions.RemoveAll(x => x
            .Contains($"trade id: {_stopLossTradeId} Has closed due to stop loss"));

            actualTimelineActions.RemoveAll(x => x
            .Contains("Email trade sl close has been sent to client by System"));

            actualTimelineActions.Add($"trade id: {_stopLossTradeId} Has closed due to stop loss");
            actualTimelineActions.Add("System Has sent email trade_sl_close to client");

            // fix the take profit
            actualTimelineActions.RemoveAll(x => x
            .Contains($"trade id: {_takeProfitTradeId} Has closed due to take profit"));

            actualTimelineActions.RemoveAll(x => x
            .Contains("Email trade tp close has been sent to client by System"));

            actualTimelineActions.Add($"trade id: {_takeProfitTradeId} Has closed due to take profit");
            actualTimelineActions.Add("Email trade tp close has been sent to client by System");

            // fix the stop loss
            actualTimelineActions.RemoveAll(x => x
            .Contains($"{_clientName} {_clientName} Close Trade asset {_tradeAssetName}, trade id"));

            actualTimelineActions.Add($"{_clientName} {_clientName} Close Trade asset {_tradeAssetName}, trade id");

            // check the outcome
            var actualAgainstExpected = actualTimelineActions
                .CompareTwoListOfString(expectedTimelineData);

            var actualDate = timelineDetails
                .Select(d => d.date).ToList().All(p => p.Contains(expectedDate));

            Assert.Multiple(() =>
            {
                Assert.True(actualDate,
                    $" expected: {expectedDate}" +
                    $" actual : different then {actualTimelineActions.ListToString()}");

                Assert.True(actualAgainstExpected.Count == 0,
                    $" Actual  Against expected list: {actualAgainstExpected.ListToString()}" +
                    $" expected TIMELINE DATA: {expectedTimelineData.ListToString()}" +
                    $" Actual TIMELINE DATA: {actualTimelineActions.ListToString()}");
            });
        }
    }
}