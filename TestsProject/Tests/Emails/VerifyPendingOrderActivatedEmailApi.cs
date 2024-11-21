// Ignore Spelling: Api

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
    public class VerifyPendingOrderActivatedEmailApi : TestSuitBase
    {
        #region Test Preparation
        private IApplicationFactory _apiFactory = new ApplicationFactory();
        private string _crmUrl = Config.appSettings.CrmUrl;
        private GetLoginResponse _loginData;
        private double _rateForPending;
        private double _currentRate;
        private string _clientId;
        private string _clientEmail;
        private int _depositAmount = 10000;
        private string _tradeId;
        private string _tradeAssetName = DataRep.AssetName;       
        private int _tradeAmount = 2;
        private string _testimUrl = DataRep.TesimUrl;
        private IDictionary<string, string> _groupData;
        private List<string> _tradeGroupsIdsListForDelete = new List<string>();
        private Default_Attr _tradeGroupSpreadTenAttributes;
        private string _tradingPlatformUrl =  Config.appSettings.tradingPlatformUrl;

        [SetUp]
        public void SetUp()
        {
            BeforeTest();

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
            _clientEmail = clientName + DataRep.TestimEmailPrefix;

            _clientId = _apiFactory
                .ChangeContext<ICreateClientApi>()
                .CreateClientRequest(_crmUrl, clientName,
              emailPrefix: DataRep.TestimEmailPrefix);

            _apiFactory
               .ChangeContext<IFinancesTabApi>()
               .PostDepositRequest(_crmUrl, _clientId, _depositAmount);

            _loginData = _apiFactory
              .ChangeContext<ILoginApi>()
              .PostLoginToTradingPlatform(_tradingPlatformUrl, _clientEmail)
              .GeneralResponse;

            // Create Trade Group And Assign It To client
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
            _rateForPending = (double)(_currentRate + 0.01); // to open a pending trade  

            _apiFactory
              .ChangeContext<ITradePageApi>()
              .PostPendingBuyOrderRequest(_tradingPlatformUrl, _tradeAmount, _loginData, _rateForPending);
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
        public void VerifyPendingOrderActivatedEmailApiTest()
        {
            var subject = "ies has been activated";
            var emailBodyParams = new List<string> { "EMAIL BODY IS EMPTY" };

            var emailsParams = new Dictionary<string, string> { 
                { "type", "trade_order_activated" }, { "language", "en" },
                { "subject", subject }};

            // update the existing template
            _apiFactory
               .ChangeContext<IPlatformTabApi>()
               .UpdateEmailTemplatePipe(_crmUrl, emailsParams, emailBodyParams);

            // trigger the mail
           // Create Cripto Group And Assign It To client
           _groupData = _apiFactory
                .ChangeContext<ISharedStepsGenerator>()
                .CreateTradeGroupAndAssignItToClientPipe(_crmUrl, _tradeGroupSpreadTenAttributes, _clientId);

            _tradeGroupsIdsListForDelete.Add(_groupData.Keys.First());

            var email = _apiFactory
               .ChangeContext<IPlatformTabApi>()
               .FilterEmailBySubject(_testimUrl, _clientEmail, subject)
               .First();

            var actualEmailBody = _apiFactory
                .ChangeContext<IPlatformTabApi>()
                .ParseMailToKeyValuePair(email);

            Assert.Multiple(() =>
            {              
                Assert.True(email.Subject == subject,
                    $"actual email subject: {email.Subject}" +
                    $"expected email subject: {subject}");
            });
        }
    }
}