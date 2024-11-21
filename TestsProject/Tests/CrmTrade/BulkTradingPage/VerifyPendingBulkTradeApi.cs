// Ignore Spelling: Crm Api

using System;
using System.Linq;
using AirSoftAutomationFramework.Internals.DAL.ConfigurationManagement;
using AirSoftAutomationFramework.Internals.ExtensionsMethods;
using AirSoftAutomationFramework.Internals.Factorys;
using AirSoftAutomationFramework.Internals.Helpers;
using AirSoftAutomationFramework.Internals.NunitAtributes;
using AirSoftAutomationFramework.Objects.DTOs;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.AccountsApi.UsersMenuApi.UserMenuApi;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.Clients.ClientCard;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.Clients.ClientCard.FinancesTab;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.Clients.ClientPage;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Trade;
using AirSoftAutomationFramework.Objects.ForexObjects.ApiGeneral.Login;
using AirSoftAutomationFramework.Objects.ForexObjects.TradingPlatform.TradeObjects.TradeApi;
using AirSoftAutomationFramework.Objects.ForexObjects.Ui.Cfd;
using AirSoftAutomationFramework.Objects.ForexObjects.Ui.Login;
using AirSoftAutomationFramework.Objects.ForexObjects.Ui.UiGeneral;
using Microsoft.Graph.Models;
using NUnit.Framework;
using OpenQA.Selenium;
using TestsProject.TestsInternals;

namespace TestsProject.Tests.CrmTrade.BulkTradingPage
{
    [TestFixture]
    public class VerifyPendingBulkTradeApi : TestSuitBase
    {
        #region Test Preparation

        private IApplicationFactory _apiFactory = new ApplicationFactory();
        private string _crmUrl = Config.appSettings.CrmUrl;
        private string _tradingPlatformUrl = Config.appSettings.tradingPlatformUrl;
        private string _clientEmail;
        private string _clientId;
        private int _tradeAmount = 2;

        [SetUp]
        public void SetUp()
        {
            BeforeTest();
            #region PreCondition
            var bonusAmount = 10000;
            var assetName = DataRep.AssetName;

            // create client 
            var clientName = TextManipulation.RandomString();
            _clientEmail = clientName + DataRep.EmailPrefix;

            _clientId = _apiFactory
                .ChangeContext<ICreateClientApi>()
                .CreateClientRequest(_crmUrl, clientName);

            // create bonus
            _apiFactory
                 .ChangeContext<IFinancesTabApi>()
                 .PostBonusRequest(_crmUrl, _clientId, bonusAmount);

            // get login cookies
            var loginCookies = _apiFactory
                 .ChangeContext<ILoginApi>()
                 .PostLoginToTradingPlatform(_tradingPlatformUrl, _clientEmail)
                 .GeneralResponse;

            // buy asset
            var tradeDetails = _apiFactory
                .ChangeContext<ITradePageApi>()
                .PostBuyAssetRequest(_tradingPlatformUrl, _tradeAmount,
                loginCookies, assetSymble: assetName)
                .GeneralResponse;

            var currentRate = Convert.ToInt32(tradeDetails.TradeRate);
            var RateForPending = (double)(currentRate + 100); // to open a pending trade

            // create bulk trades 
            _apiFactory
                .ChangeContext<IBulkTradePageApi>()
                .PostCreateBulkTradeRequest(_crmUrl, new string[] { _clientId },
                rate: RateForPending, marketLimit: "limit");
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
        [RetryMenage(DataRep.NumOfRetriesForFallingTest)]
        public void VerifyPendingBulkTradeApiTest()
        {
            var expectedTradeStatus = "pending";

            var actualTradeStatus = _apiFactory
              .ChangeContext<ITradesTabApi>()
              .GetTradesRequest(_crmUrl, _clientId)
              .GeneralResponse
              .FirstOrDefault()
              .status;

            Assert.Multiple(() =>
            {
                Assert.True(actualTradeStatus == expectedTradeStatus,
                    $" expected Trade Status: {expectedTradeStatus}" +
                    $" actual Trade Status: {actualTradeStatus}");
            });
        }
    }
}