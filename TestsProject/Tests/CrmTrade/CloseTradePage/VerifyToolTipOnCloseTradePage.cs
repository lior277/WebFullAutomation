// Ignore Spelling: Crm

using System;
using System.Linq;
using System.Threading;
using AirSoftAutomationFramework.Internals.DAL.ConfigurationManagement;
using AirSoftAutomationFramework.Internals.Factorys;
using AirSoftAutomationFramework.Internals.Helpers;
using AirSoftAutomationFramework.Internals.NunitAtributes;
using AirSoftAutomationFramework.Objects.DTOs;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.AccountsApi.UsersMenuApi.UserMenuApi;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.Clients.ClientCard.FinancesTab;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.Clients.ClientPage;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Trade;
using AirSoftAutomationFramework.Objects.ForexObjects.ApiGeneral;
using AirSoftAutomationFramework.Objects.ForexObjects.ApiGeneral.Login;
using AirSoftAutomationFramework.Objects.ForexObjects.TradingPlatform.TradeObjects.TradeApi;
using AirSoftAutomationFramework.Objects.ForexObjects.Ui.Cfd;
using AirSoftAutomationFramework.Objects.ForexObjects.Ui.Login;
using NUnit.Framework;
using OpenQA.Selenium;
using TestsProject.TestsInternals;

namespace TestsProject.Tests.CrmTrade.CloseTradePage
{
    [TestFixture(DataRep.Chrome)]
    public class VerifyToolTipOnCloseTradePage : TestSuitBase
    {
        #region Test Preparation
        public VerifyToolTipOnCloseTradePage(string browser) : base(browser)
        {
            _browserName = browser;
        }

        private IApplicationFactory _apiFactory = new ApplicationFactory();
        private string _crmUrl = Config.appSettings.CrmUrl;
        private string _browserName;
        private string _tradeId;
        private double _strartingSpread = 0.1;
        private double _currentSpread = 0.2;
        private int _leverage = 5;
        private string _roleName;
        private string _clientId;
        private string _tradeGroupId;
        private IWebDriver _driver;

        [SetUp]
        public void SetUp()
        {
            BeforeTest(_browserName);
            _roleName = TextManipulation.RandomString();
            var tradingPlatformUrl = Config.appSettings.tradingPlatformUrl;

            _driver = GetDriver();
            var tradeAmount = 2;
            var bonusAmount = 10000;
            var assetName = DataRep.AssetName;

            // create user
            var userName = TextManipulation.RandomString();

            var userId = _apiFactory
                .ChangeContext<IUserApi>(_driver)
                .CreateUserForUiPipe(_crmUrl, userName,
                role: DataRep.AdminWithUsersOnlyRoleName);

            // get user apiKey
            var userApiKey = _apiFactory
                .ChangeContext<IUserApi>(_driver)
                .PostCreateApiKeyRequest(_crmUrl, userId);

            // create client 
            var clientName = TextManipulation.RandomString();
            var clientEmail = clientName + DataRep.EmailPrefix;

            _clientId = _apiFactory
                .ChangeContext<ICreateClientApi>()
                .CreateClientRequest(_crmUrl, clientName,
                apiKey: userApiKey);

            // create bonus
            _apiFactory
                .ChangeContext<IFinancesTabApi>(_driver)
                .PostBonusRequest(_crmUrl, _clientId, bonusAmount);

            // get login Data for trading Platform
            var loginData = _apiFactory
                .ChangeContext<ILoginApi>(_driver)
                .PostLoginToTradingPlatform(tradingPlatformUrl, clientEmail)
                .GeneralResponse;

            // cripto group attributes
            var tradeGroupAttributes = new Default_Attr
            {
                commision = 0,
                leverage = _leverage,
                maintenance = 1,
                minimum_amount = 1,
                minimum_step = 1,
                spread = _strartingSpread,
                margin_call = null,
                swap_long = 0.04,
                swap_short = 0.04,
                swap_time = "00:00:00",
            };

            // create Trade group
            var groupData = _apiFactory // Create Cripto Group And Assign It To client
                .ChangeContext<ISharedStepsGenerator>(_driver)
                .CreateTradeGroupAndAssignItToClientPipe(_crmUrl,
                tradeGroupAttributes, _clientId);

            _tradeGroupId = groupData.Keys.First();

            // open trade
            var tradeDetails = _apiFactory
                .ChangeContext<ITradePageApi>(_driver)
                .PostBuyAssetRequest(tradingPlatformUrl, tradeAmount,
                loginData: loginData, assetSymble: assetName)
                .GeneralResponse;

            // get group by id
            var cryptoGroup = _apiFactory
                .ChangeContext<ITradeGroupApi>(_driver)
                .GetTradeGroupsRequest(_crmUrl)
                .GeneralResponse
                .Where(p => p._id == _tradeGroupId)
                .FirstOrDefault();

            cryptoGroup.default_attr.spread = _currentSpread;

            // edit cripto group
            _apiFactory
              .ChangeContext<ITradeGroupApi>(_driver)
              .PutEditTradeGroupRequest(_crmUrl, _tradeGroupId, cryptoGroup);

            _tradeId = tradeDetails.TradeId;

            // close the trade
            _apiFactory
              .ChangeContext<IOpenTradesPageApi>(_driver)
              .PatchCloseTradeRequest(_crmUrl, _tradeId, userApiKey);

            // login
            _apiFactory
                .ChangeContext<ILoginPageUi>(_driver)
                .LoginPipe(userName, _crmUrl);

            Thread.Sleep(1000);
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
                throw new Exception(ex.Message);
            }
            finally
            {
                AfterTest(_driver);
                DriverDispose(_driver);
            }
        }

        [Test]
        [Category(DataRep.SanityCategory)]
        [Category(DataRep.RegressionCategory)]
        [RetryMenage(DataRep.NumOfRetraiesForFallingTest)]
        public void VerifyToolTipOnCloseTradePageTest()
        {
            var expectedTooltipText = $"Starting Spread: 0.1 Current Spread: 0.2 leverage: {_leverage}";

            // cfd open trades table
            _apiFactory
                .ChangeContext<ISharedStepsGenerator>(_driver)
                .NavigateToPageByName(_crmUrl, "/groups/trades/close")
                .ChangeContext<IClosedTradesPageUi>(_driver)
                .SearchCloseTrades(_clientId);

            var actualTooltipText = _apiFactory
                .ChangeContext<IClosedTradesPageUi>(_driver)
                .MoveToTooltip();

            Assert.True(actualTooltipText == expectedTooltipText,
                $" expected Tooltip Text: {expectedTooltipText}" +
                $" actual Tooltip Text: {actualTooltipText}");
        }
    }
}
