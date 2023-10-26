// Ignore Spelling: Api

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using AirSoftAutomationFramework.Internals.DAL.ConfigurationManagement;
using AirSoftAutomationFramework.Internals.Factory;
using AirSoftAutomationFramework.Internals.Helpers;
using AirSoftAutomationFramework.Internals.NunitAttributes;
using AirSoftAutomationFramework.Objects.DTOs;
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
    public class VerifyActivatePendingTradeActivityApi : TestSuitBase
    {
        #region members
        private IApplicationFactory _apiFactory = new ApplicationFactory();
        private string _crmUrl = Config.appSettings.CrmUrl;
        private string _tradingPlatformUrl =  Config.appSettings.tradingPlatformUrl;
        private GetLoginResponse _loginData;
        private double _rateForPending;
        private double _currentRate;
        private string _clientId;
        private string _clientEmail;
        private int _depositAmount = 10000;
        private string _tradeId;
        private string _pendingTradeId;
        private string _tradeAssetName = DataRep.AssetName;
        private int _tradeAmount = 10;
        private IDictionary<string, string> _groupData;
        private List<string> _tradeGroupsIdsListForDelete = new List<string>();
        private Default_Attr _tradeGroupSpreadTenAttributes;
        #endregion

        [SetUp]
        public void SetUp()
        {
            BeforeTest();
            #region PreCondition

            var tradeGroupSreadMinosAttributes = new Default_Attr
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

            _tradeGroupSpreadTenAttributes = new Default_Attr
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

            // create client 
            var clientName = TextManipulation.RandomString();
            _clientEmail = clientName + DataRep.EmailPrefix;

            _clientId = _apiFactory
                .ChangeContext<ICreateClientApi>()
                .CreateClientRequest(_crmUrl, clientName);

            // create deposit
            _apiFactory
               .ChangeContext<IFinancesTabApi>()
               .PostDepositRequest(_crmUrl, _clientId, _depositAmount);

            _loginData = _apiFactory
                  .ChangeContext<ILoginApi>()
                  .PostLoginToTradingPlatform(_tradingPlatformUrl, _clientEmail)
                  .GeneralResponse;

            // Create Crypto Group And Assign It To client
            _groupData = _apiFactory
                 .ChangeContext<ISharedStepsGenerator>()
                 .CreateTradeGroupAndAssignItToClientPipe(_crmUrl,
                 tradeGroupSreadMinosAttributes, _clientId);

            _tradeGroupsIdsListForDelete.Add(_groupData.Keys.First());

            // create trade
            var tradeDetails = _apiFactory
                .ChangeContext<ITradePageApi>()
                .PostBuyAssetRequest(_tradingPlatformUrl, _tradeAmount,
                _loginData, assetSymble: _tradeAssetName)
                .GeneralResponse;

            _currentRate = tradeDetails.TradeRate;
            _tradeId = tradeDetails.TradeId;
            _rateForPending = (_currentRate + 0.10); // to open a pending trade  

            // open the pending trade
            tradeDetails = _apiFactory
                .ChangeContext<ITradePageApi>()
                .PostPendingBuyOrderRequest(_tradingPlatformUrl,
                _tradeAmount, _loginData, _rateForPending);

            _pendingTradeId = tradeDetails.TradeId;

            // Create Cripto Group And Assign It To client to open the pending trade
            _groupData = _apiFactory
                 .ChangeContext<ISharedStepsGenerator>()
                 .CreateTradeGroupAndAssignItToClientPipe(_crmUrl,
                 _tradeGroupSpreadTenAttributes, _clientId);

            // wait for pending to open
            _apiFactory
               .ChangeContext<ITradePageApi>()
               .WaitForPendingTradeToChangeStatusToOpen(_crmUrl, _loginData);           

            _tradeGroupsIdsListForDelete.Add(_groupData.Keys.First());
            #endregion
        }

        [TearDown]
        public void TearDown()
        {
            try
            {
                _apiFactory
                    .ChangeContext<ITradeGroupApi>()
                    .DeleteTradeGroupRequest(_crmUrl, _tradeGroupsIdsListForDelete);

                AfterTest();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                
            }
        }

        [Test]
        [Category(DataRep.SanityCategory)]
        [Category(DataRep.RegressionCategory)]
        [RetryMenage(DataRep.NumOfRetraiesForFallingTest)]
        public void VerifyActivatePendingTradeActivityApiTest()
        {
            var clientName = _clientEmail.Split('@').First();
            var expectedStatus = "open";
            var expectedOldTradeId = _pendingTradeId;
            var expectedNewTradeId = _pendingTradeId + 1;        
            var expectedClientId = _clientId;
            var expectedClientFullName = $"{clientName} {clientName}";
            var expectedType = "activate_pending_trade";

            Thread.Sleep(1000);

            var pendingTradeOpened = _apiFactory
                .ChangeContext<ITradePageApi>()
                .WaitForActivityToRegister(_tradingPlatformUrl, expectedType, _loginData)
                .GetActivities(_tradingPlatformUrl, _loginData)
                .Where(p => p.type == expectedType)
                .FirstOrDefault();

            Assert.Multiple(() =>
            {
                Assert.True(pendingTradeOpened.status == expectedStatus,
                   $" expected status : {expectedStatus}" +
                   $" actual status: {pendingTradeOpened.status}");

                Assert.True(pendingTradeOpened.user_id == expectedClientId,
                   $" expected client id : {expectedClientId}" +
                   $" actual client id : {pendingTradeOpened.user_id}");

                Assert.True(pendingTradeOpened.user_full_name == expectedClientFullName,
                   $" expected Client Full Name: {expectedClientFullName}" +
                   $" actual Client Full Name: {pendingTradeOpened.user_full_name}");

                Assert.True(pendingTradeOpened.type == expectedType,
                   $" expected Type: {expectedType}" +
                   $" actual Type: {pendingTradeOpened.type}");
            });
        }
    }
}