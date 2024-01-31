using System;
using System.Globalization;
using System.Linq;
using System.Threading;
using AirSoftAutomationFramework.Internals.DAL.ConfigurationManagement;
using AirSoftAutomationFramework.Internals.Factorys;
using AirSoftAutomationFramework.Internals.Helpers;
using AirSoftAutomationFramework.Internals.NunitAtributes;
using AirSoftAutomationFramework.Models;
using AirSoftAutomationFramework.Objects.DTOs;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.AccountsApi.UsersMenuApi.UserMenuApi;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.Clients.ClientCard;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.Clients.ClientCard.FinancesTab;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.Clients.ClientPage;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Trade;
using AirSoftAutomationFramework.Objects.ForexObjects.ApiGeneral;
using AirSoftAutomationFramework.Objects.ForexObjects.ApiGeneral.Login;
using AirSoftAutomationFramework.Objects.ForexObjects.TradingPlatform.TradeObjects.TradeApi;
using DocumentFormat.OpenXml;
using NUnit.Framework;
using TestsProject.TestsInternals;

namespace TestsProject.Tests.TradingPlatform
{
    [TestFixture]
    public class VerifySwapCalculationForBuyAndSellApi : TestSuitBase
    {
        #region Test Preparation

        private IApplicationFactory _apiFactory = new ApplicationFactory();
        private string _crmUrl = Config.appSettings.CrmUrl;
        private string _userEmail;
        private string _clientEmail;
        private string _clientId;
        private Default_Attr _tradeGroup;
        private QaAutomation01Context _dbContext = new QaAutomation01Context();
        private string _tradingPlatformUrl =  Config.appSettings.tradingPlatformUrl;
        private string _tradeGroupId;
        private string _buyTradeId; 
        private string _sellTradeId;
        private double _buyRate;
        private double _sellRate;  
        private float? _swapLong;
        private float? _swapShort;
        private GetLoginResponse _loginCookies;
        private int _tradeAmount = 1;

        [SetUp]
        public void SetUp()
        {
            BeforeTest();
            #region PreCondition                  
            var depositAmount = 10000;
            var testimEmailPerfix = DataRep.TestimEmailPrefix;
            var swapTime = DateTime.UtcNow.AddSeconds(10).ToString("HH:mm:ss");    

            _tradeGroup = new Default_Attr
            {
                commision = 0,
                leverage = 5,
                maintenance = 1,
                minimum_amount = 1,
                minimum_step = 1,
                spread = 0,
                margin_call = null,
                swap_long = 0.01,
                swap_short = 0.02,
                swap_time = swapTime,
            };

            // create user
            var userName = TextManipulation.RandomString();
            _userEmail = userName + testimEmailPerfix;

            var userId = _apiFactory
                .ChangeContext<IUserApi>()
                .PostCreateUserRequest(_crmUrl, userName);

            // create client 
            var clientName = TextManipulation.RandomString();
            _clientEmail = clientName + DataRep.EmailPrefix;

            _clientId = _apiFactory
                .ChangeContext<ICreateClientApi>()
                .CreateClientRequest(_crmUrl, clientName);

            // Create trade Group And Assign It To client
            var groupData = _apiFactory
                .ChangeContext<ISharedStepsGenerator>()
                .CreateTradeGroupAndAssignItToClientPipe(_crmUrl, 
                _tradeGroup, _clientId);

            _tradeGroupId = groupData.Keys.FirstOrDefault();    

            // create deposit
            _apiFactory
                .ChangeContext<IFinancesTabApi>()
                .PostDepositRequest(_crmUrl, _clientId, depositAmount);

            // get login Data for trading Platform
            _loginCookies = _apiFactory
                .ChangeContext<ILoginApi>()
                .PostLoginToTradingPlatform(_tradingPlatformUrl, _clientEmail)
                .GeneralResponse;

            // buy asset
            var tradeData = _apiFactory
                .ChangeContext<ITradePageApi>()
                .PostBuyAssetRequest(_tradingPlatformUrl,
                _tradeAmount, _loginCookies);

            _buyTradeId = tradeData.GeneralResponse.TradeId;

            // sell asset
            tradeData = _apiFactory
                .ChangeContext<ITradePageApi>()
                .PostSellAssetRequest(_tradingPlatformUrl,
                _tradeAmount, _loginCookies);

            _sellTradeId = tradeData.GeneralResponse.TradeId;
            var lastSwapDateCharge = DateTime.UtcNow.AddDays(-1.5);

            // set last Swap Date Charge for buy trade
            (from s in _dbContext.Trades
             where s.Id.ToString() == _buyTradeId
             select s)
               .FirstOrDefault()
               .LastSwapDateCharge = lastSwapDateCharge;

            // set last Swap Date Charge for sell trade
            (from s in _dbContext.Trades
             where s.Id.ToString() == _sellTradeId
             select s)
               .FirstOrDefault()
               .LastSwapDateCharge = lastSwapDateCharge;

            Thread.Sleep(1000);
            _dbContext.SaveChanges();
            Thread.Sleep(1000);

            // get rate for buy trade
            _buyRate = 
            (from s in _dbContext.Trades
             where s.Id.ToString() == _buyTradeId
             select s)
               .FirstOrDefault()
               .Rate;

            // get rate for sell trade
            _sellRate =
            (from s in _dbContext.Trades
             where s.Id.ToString() == _sellTradeId
             select s)
               .FirstOrDefault()
               .Rate;

            // get swap long for buy trade
            _swapLong =
            (from s in _dbContext.Trades
             where s.Id.ToString() == _buyTradeId
             select s)
               .FirstOrDefault()
               .SwapLong;

            // get swap short for sell trade
            _swapShort =
            (from s in _dbContext.Trades
             where s.Id.ToString() == _sellTradeId
             select s)
               .FirstOrDefault()
               .SwapShort;
            #endregion
        }
        #endregion

        [TearDown]
        public void TearDown()
        {
            try
            {
                _apiFactory
                    .ChangeContext<ITradeGroupApi>()
                    .DeleteTradeGroupRequest(_crmUrl, _tradeGroupId);
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }
            finally
            {
                AfterTest();
            }
        }
     
        [Test]
        [Description("based on jira https://airsoftltd.atlassian.net/browse/AIRV2-5420")]
        [Category(DataRep.SanityCategory)]
        [Category(DataRep.RegressionCategory)]
        [RetryMenage(DataRep.NumOfRetraiesForFallingTest)]
        public void VerifySwapCalculationForBuyAndSellApiTest()
        {
            // calculate the swap
            var swapForBuy = Convert.ToDouble(_tradeAmount * _swapLong);
            var swapForSell = Convert.ToDouble(_tradeAmount * _swapShort);

            // convert from eth to usd
            var expectedSwapLong = _apiFactory
                .ChangeContext<IGeneral>()
                .PostCurrencyConversionRequest(_crmUrl,
                swapForBuy, DataRep.AssetNameShort, DataRep.DefaultUSDCurrencyName)
                .ToString("0.##");

            // convert from eth to usd
            var expectedSwapShort = _apiFactory
                .ChangeContext<IGeneral>()
                .PostCurrencyConversionRequest(_crmUrl,
                swapForSell, DataRep.AssetNameShort, DataRep.DefaultUSDCurrencyName)
                .ToString("0.##");

            double tempSwapLong = 0;
            double tempSwapShort = 0;

            // wait for swap commission
            for (var i = 0; i < 20; i++)
            {
                if (tempSwapLong == 0 || tempSwapShort == 0)
                {
                    // get swap long 
                    tempSwapLong = _apiFactory
                        .ChangeContext<ITradesTabApi>()
                        .GetTradesRequest(_tradingPlatformUrl, _clientId)
                        .GeneralResponse
                        .Where(p => p.id == _buyTradeId)
                        .FirstOrDefault()
                        .swap_commission;

                    // get swap short 
                    tempSwapShort = _apiFactory
                        .ChangeContext<ITradesTabApi>()
                        .GetTradesRequest(_tradingPlatformUrl, _clientId)
                        .GeneralResponse
                        .Where(p => p.id == _sellTradeId)
                        .FirstOrDefault()
                        .swap_commission;

                    Thread.Sleep(300);

                    continue;
                }
                break;
            }

            var actualSwapLong = Convert.ToDouble(Math.Round(tempSwapLong, 2, MidpointRounding.AwayFromZero));
            var actualSwapShort = Convert.ToDouble(Math.Round(tempSwapShort, 2, MidpointRounding.AwayFromZero));

            Assert.Multiple(() => 
            {
                Assert.True(actualSwapLong.ToString().Contains(expectedSwapLong),
                    $" actual Swap Long: {actualSwapLong} " +
                    $" contains expected Swap Long: {expectedSwapLong}");

                Assert.True(actualSwapShort.ToString().Contains(expectedSwapShort),
                    $" actual Swap short: {actualSwapShort} " +
                    $" contains expected Swap short: {expectedSwapShort}");
            });
        }
    }
}