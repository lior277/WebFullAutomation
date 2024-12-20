﻿// Ignore Spelling: Api

using System;
using System.Collections.Generic;
using System.Linq;
using AirSoftAutomationFramework.Internals.DAL.ConfigurationManagement;
using AirSoftAutomationFramework.Internals.Factorys;
using AirSoftAutomationFramework.Internals.Helpers;
using AirSoftAutomationFramework.Internals.NunitAtributes;
using AirSoftAutomationFramework.Objects.DTOs;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.Clients.ClientCard.FinancesTab;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.Clients.ClientPage;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.Settings;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Trade;
using AirSoftAutomationFramework.Objects.ForexObjects.ApiGeneral;
using AirSoftAutomationFramework.Objects.ForexObjects.ApiGeneral.Login;
using AirSoftAutomationFramework.Objects.ForexObjects.TradingPlatform.TradeObjects.TradeApi;
using NUnit.Framework;
using TestsProject.TestsInternals;

namespace TestsProject.Tests.Emails
{
    [TestFixture]
    public class VerifyDepositAndBonusIsNotPossibleForNegativeBalanceApi : TestSuitBase
    {
        #region Test Preparation
        private IApplicationFactory _apiFactory = new ApplicationFactory();
        private string _crmUrl = Config.appSettings.CrmUrl;
        private GetLoginResponse _loginData;
        private IDictionary<string, string> _groupData;
        private string _clientEmail;
        private string _clientId;
        private int _depositAmount = 10000;
        private Default_Attr _tradeGroupSpreadTenAttributes;
        private List<string> _tradeGroupsIdsListForDelete = new List<string>();
        private string _tradingPlatformUrl =  Config.appSettings.tradingPlatformUrl;

        [SetUp]
        public void SetUp()
        {
            BeforeTest();
           var tradeAmount = 2;
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

            _tradeGroupSpreadTenAttributes = new Default_Attr
            {
                commision = 0,
                leverage = 1,
                maintenance = 0.1,
                minimum_amount = 1,
                minimum_step = 1,
                spread = 0.2,
                margin_call = null,
                swap_long = 0.04,
                swap_short = 0.04,
                swap_time = "00:00:00",
            };

            // create client 
            var clientName = TextManipulation.RandomString();
            _clientEmail = clientName + DataRep.TestimEmailPrefix;

            _clientId = _apiFactory
                .ChangeContext<ICreateClientApi>()
                .CreateClientRequest(_crmUrl, clientName,
              emailPrefix: DataRep.TestimEmailPrefix);

            // create deposit
            _apiFactory
                .ChangeContext<IFinancesTabApi>()
                .PostDepositRequest(_crmUrl, _clientId, _depositAmount);

            // login data
            _loginData = _apiFactory
                .ChangeContext<ILoginApi>()
                .PostLoginToTradingPlatform(_tradingPlatformUrl, _clientEmail)
                .GeneralResponse;
            

            // create trade to retrieve the current rate
            var tradeDetails = _apiFactory 
                 .ChangeContext<ITradePageApi>()
                 .PostBuyAssetRequest(_tradingPlatformUrl, tradeAmount, _loginData)
                 .GeneralResponse;

            var openPrice = tradeDetails.TradeRate;

            // Create Cripto Group And Assign It To client
            _groupData = _apiFactory
                .ChangeContext<ISharedStepsGenerator>()
                .CreateTradeGroupAndAssignItToClientPipe(_crmUrl,
                tradeGroupSpreadZeroAttributes, _clientId);

            _tradeGroupsIdsListForDelete.Add(_groupData.Keys.First());

            // _balance = 100// * //leverage = 1000// //= 100000// // - 1000
            tradeAmount = Convert.ToInt32(90000 / openPrice);

            // create trade for client with trade group "tradeGroupSreadZeroAttributes"
            _apiFactory
                .ChangeContext<ITradePageApi>()
                .PostBuyAssetRequest(_tradingPlatformUrl, tradeAmount, _loginData);

            _groupData = _apiFactory
                .ChangeContext<ISharedStepsGenerator>()
                .CreateTradeGroupAndAssignItToClientPipe(_crmUrl,
                _tradeGroupSpreadTenAttributes, _clientId);

            _tradeGroupsIdsListForDelete.Add(_groupData.Keys.First());
        }

        [TearDown]
        public void TearDown()
        {    
            try
            {
                _apiFactory
                    .ChangeContext<ITradeGroupApi>()
                    .DeleteTradeGroupRequest(_crmUrl, _tradeGroupsIdsListForDelete);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
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
        public void VerifyDepositAndBonusIsNotPossibleForNegativeBalanceTest()
        {
            var expectedErrorForDepositWithNegativeBalance = 
                "You can not deposit due to a negative balance. Contact the support team for more information";

            // create deposit
            var actualErrorForDepositWithNegativeBalance = _apiFactory
                .ChangeContext<IFinancesTabApi>()
                .PostDepositRequest(_crmUrl, _clientId,
                _depositAmount, checkStatusCode: false);

            // create bonus
            var actualErrorForBonusWithNegativeBalance = _apiFactory
                .ChangeContext<IFinancesTabApi>()
                .PostBonusRequest(_crmUrl, _clientId,
                _depositAmount, checkStatusCode: false)
                .Message;       

            Assert.Multiple(() =>
            {
                Assert.True(actualErrorForDepositWithNegativeBalance.Contains(expectedErrorForDepositWithNegativeBalance),
                    $" expected Error For Deposit With Negative Balance {expectedErrorForDepositWithNegativeBalance}" +
                    $" actual error For Deposit With Negative Balance: {actualErrorForDepositWithNegativeBalance}");

                Assert.True(actualErrorForBonusWithNegativeBalance.Contains(expectedErrorForDepositWithNegativeBalance),
                    $" expected Error For Bonus With Negative Balance {expectedErrorForDepositWithNegativeBalance}" +
                    $" actual error For Bonus With Negative Balance: {actualErrorForBonusWithNegativeBalance}");
            });
        }
    }
}