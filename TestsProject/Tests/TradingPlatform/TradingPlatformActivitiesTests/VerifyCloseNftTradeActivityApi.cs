// Ignore Spelling: Api Nft

using AirSoftAutomationFramework.Internals.DAL.ConfigurationManagement;
using AirSoftAutomationFramework.Internals.Helpers;
using AirSoftAutomationFramework.Objects.DTOs;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.AccountsApi.UsersMenuApi.UserMenuApi;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.Clients.ClientCard.FinancesTab;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.Clients.ClientPage;
using AirSoftAutomationFramework.Objects.ForexObjects.ApiGeneral.Login;
using AirSoftAutomationFramework.Objects.ForexObjects.TradingPlatform.TradeObjects.TradeApi;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using AirSoftAutomationFramework.Internals.Factorys;
using AirSoftAutomationFramework.Internals.NunitAtributes;
using AirSoftAutomationFramework.Models;
using TestsProject.TestsInternals;

namespace TestsProject.Tests.TradingPlatform.TradingPlatformActivitiesTests
{
    [TestFixture]
    public class VerifyCloseNftTradeActivityApi : TestSuitBase
    {
        private IApplicationFactory _apiFactory = new ApplicationFactory();
        private string _clientId;
        private string _clientEmail;
        private string _userName;
        private string _crmUrl = Config.appSettings.CrmUrl;
        private string _userId;
        private string _tradeId;
        private int _depositAmount = 100000;
        private string _tradingPlatformUrl = Config.appSettings.tradingPlatformUrl;
        private List<string> _tradeGroupsIdsListForDelete = new List<string>();
        private QaAutomation01Context _dbContext = new QaAutomation01Context();
        private GetLoginResponse _loginData;

        [SetUp]
        public void SetUp()
        {
            BeforeTest();
            #region PreCondition

            // create user
            _userName = TextManipulation.RandomString();

            _userId = _apiFactory
                .ChangeContext<IUserApi>()
                 .PostCreateUserRequest(_crmUrl, _userName,
                 role: DataRep.AdminWithUsersOnlyRoleName);

            // create client 
            var clientName = TextManipulation.RandomString();
            _clientEmail = clientName + DataRep.EmailPrefix;

            _clientId = _apiFactory
                .ChangeContext<ICreateClientApi>()
                .CreateClientRequest(_crmUrl, clientName);

            #region login data 
            // login data
            _loginData = _apiFactory
                .ChangeContext<ILoginApi>()
                .PostLoginToTradingPlatform(_tradingPlatformUrl, _clientEmail)
                .GeneralResponse;
            #endregion

            #region deposit 
            // deposit 
            _apiFactory
                .ChangeContext<IFinancesTabApi>()
                .PostDepositRequest(_crmUrl, _clientId, _depositAmount);
            #endregion

            #region buy nft 
            // create nft trade 
            var tradeDetails = _apiFactory
                .ChangeContext<INftPageApi>()
                .PostBuyNftRequest(_tradingPlatformUrl, _loginData)
                .GeneralResponse;

            _tradeId = tradeDetails.TradeId;

            _apiFactory
               .ChangeContext<INftPageApi>()
               .PatchCloseNftRequest(_tradingPlatformUrl, _tradeId, _loginData);           
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
        public void VerifyCloseNftTradeActivityApiTest()
        {
            var expectedClientId = _clientId;
            var expectedUseId = _userId;
            var clientName = _clientEmail.Split('@').First();
            var expectedClientFullName = $"{clientName} {clientName}";
            var expectedTradeId = _tradeId;
            var expectedAssetSymbol = DataRep.AssetNftLongSymbol;
            var expectedPlatform = "cfd";
            var expectedCloseReason = "user_closed";
            var expectedAssetLabel = DataRep.AssetNftSymbol;
            var expectedSystemType = "NFT";
            var expectedType = "close_trade";
            var expectedActionMadeBy = clientName + clientName;
            var expectedActionMadeByClientId = _clientId;

            var actualCloseTrade = _apiFactory
                .ChangeContext<ITradePageApi>()
                .WaitForActivityToRegister(_tradingPlatformUrl, "close_trade", _loginData)
                .GetActivities(_tradingPlatformUrl, _loginData)
                .Where(p => p.close_reason.Equals(expectedCloseReason))
                .FirstOrDefault();

            Assert.Multiple(() =>
            {
                Assert.True(actualCloseTrade.user_id == _clientId,
                    $" expected  client Id: {_userId}" +
                    $" actual client Id: {actualCloseTrade.user_id}");

                Assert.True(actualCloseTrade.user_full_name == expectedClientFullName,
                    $" expected  client full name: {expectedClientFullName}" +
                    $" actual client full name: {actualCloseTrade.user_full_name}");

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
                    $" expected close reason: {expectedCloseReason}" +
                    $" actual :close reason: {actualCloseTrade.close_reason}");

                Assert.True(actualCloseTrade.asset_label == expectedAssetLabel,
                    $" expected Asset Label: {expectedAssetLabel}" +
                    $" actual Asset Label: {actualCloseTrade.asset_label}");

                Assert.True(actualCloseTrade.system_type == expectedSystemType,
                    $" expected system Type: {expectedSystemType}" +
                    $" actual system Type: {actualCloseTrade.system_type}");

                Assert.True(actualCloseTrade.type == expectedType,
                    $" expected Type: {expectedType}" +
                    $" actual Type: {actualCloseTrade.type}");

                Assert.True(actualCloseTrade.action_made_by == expectedActionMadeBy,
                    $" expected Action Made By: {expectedActionMadeBy}" +
                    $" actual Action Made By: {actualCloseTrade.action_made_by}");

                Assert.True(actualCloseTrade.action_made_by_user_id == expectedActionMadeByClientId,
                    $" expected Action Made By Client Id : {expectedActionMadeByClientId}" +
                    $" actual Action Made By Client Id: {actualCloseTrade.action_made_by_user_id}");
            });
        }
    }
}