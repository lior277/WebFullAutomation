// Ignore Spelling: Chrono Api

using AirSoftAutomationFramework.Internals.DAL.ConfigurationManagement;
using AirSoftAutomationFramework.Internals.ExtensionsMethods;
using AirSoftAutomationFramework.Internals.Factory;
using AirSoftAutomationFramework.Internals.Helpers;
using AirSoftAutomationFramework.Internals.NunitAttributes;
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
using NUnit.Framework;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using TestsProject.TestsInternals;
using static AirSoftAutomationFramework.Internals.Enums.EnumFactory;

namespace TestsProject.Tests.ClientCard.TimeLine
{
    [TestFixture]
    public class VerifyCommissionNotCalculatedForChronoTradeApi : TestSuitBase
    {
        #region Test Preparation

        #region members
        private IApplicationFactory _apiFactory = new ApplicationFactory();
        private string _tradeId;
        private string _crmUrl = Config.appSettings.CrmUrl;
        private string _tradingPlatformUrl = Config.appSettings.tradingPlatformUrl;
        private GetLoginResponse _loginData;
        private List<string> _tradeGroupsIdsListForDelete = new List<string>();
        #endregion

        [SetUp]
        public void SetUp()
        {
            BeforeTest();
            var depositAmount = 800000;
            IDictionary<string, string> _groupData;

            // create client 
            var clientName = TextManipulation.RandomString();
            var clientEmail = clientName + DataRep.EmailPrefix;

            var clientId = _apiFactory
                .ChangeContext<ICreateClientApi>()
                .CreateClientRequest(_crmUrl, clientName);

            var clientIds = new List<string> { clientId };
            clientName = clientName.UpperCaseFirstLetter();

            // get login cookies
            _loginData = _apiFactory
                .ChangeContext<ILoginApi>()
                .PostLoginToTradingPlatform(_tradingPlatformUrl, clientEmail)
                .GeneralResponse;

            // create deposit 
            _apiFactory
                .ChangeContext<IFinancesTabApi>()
                .PostDepositRequest(_crmUrl, clientIds, depositAmount);

            var defaultAttr = new Default_Attr
            {
                commision = 100,
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

            // Create Cripto Group And Assign It To client
            _groupData = _apiFactory
                 .ChangeContext<ISharedStepsGenerator>()
                 .CreateTradeGroupAndAssignItToClientPipe(_crmUrl, defaultAttr, clientId);

            _tradeGroupsIdsListForDelete.Add(_groupData.Keys.First());

            // buy chrono trade 
            var tradeDetails = _apiFactory
                .ChangeContext<IChronoTradePageApi>()
                .PostBuyChronoAssetApi(_tradingPlatformUrl, _loginData);

            _tradeId = tradeDetails.GeneralResponse.TradeId;

            Thread.Sleep(1000);
        }

        [TearDown]
        #endregion

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

        // create client 
        // create crypto group with commission
        // connect the trade group to the client
        // create chrono trade
        // verify commission amount is zero
        [Test]
        [Description("based on ticket: https://airsoftltd.atlassian.net/browse/AIRV2-5378")]
        [Category(DataRep.SanityCategory)]
        [Category(DataRep.RegressionCategory)]
        [RetryMenage(DataRep.NumOfRetraiesForFallingTest)]
        public void VerifyCommissionNotCalculatedForChronoTradeApiTest()
        {  
            var actualCommission = _apiFactory
               .ChangeContext<IChronoTradePageApi>()
               .GetChronoTradesRequest(_tradingPlatformUrl, _loginData)
               .GeneralResponse
               .Where(p => p.id == _tradeId)
               .FirstOrDefault()
               .commision;       

            Assert.Multiple(() =>
            {
                Assert.True(actualCommission == 0,
                    $" expected Commission: 0" +
                    $" actual Commission: {actualCommission}");
            });
        }
    }
}