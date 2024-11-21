// Ignore Spelling: Api

using AirSoftAutomationFramework.Internals.DAL.ConfigurationManagement;
using AirSoftAutomationFramework.Internals.Helpers;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.AccountsApi.Roles;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.AccountsApi.UsersMenuApi.UserMenuApi;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.Clients.ClientCard;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.Clients.ClientCard.FinancesTab;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.Clients.ClientPage;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Trade;
using AirSoftAutomationFramework.Objects.ForexObjects.ApiGeneral.Login;
using AirSoftAutomationFramework.Objects.ForexObjects.TradingPlatform.TradeObjects.TradeApi;
using MongoDB.Driver.Linq;
using NUnit.Framework;
using System.Linq;
using AirSoftAutomationFramework.Internals.Factorys;
using AirSoftAutomationFramework.Internals.NunitAtributes;
using TestsProject.TestsInternals;

namespace TestsProject.Tests.RolePage.Permissions.ClientProfilePermissions
{
    [TestFixture]
    public class VerifyNoPermissionsUsersTradesApi : TestSuitBase
    {
        #region Test Preparation
        private IApplicationFactory _apiFactory = new ApplicationFactory();
        private string _crmUrl = Config.appSettings.CrmUrl;
        private string _tradingPlatformUrl = Config.appSettings.tradingPlatformUrl;
        private string _roleName;
        private string _userId;
        private string _currentUserApiKey;
        private string _userEmail;
        private string _clientId;
        private string _tradeId;
        private string _lastBulkTradeId;  

        [SetUp]
        public void SetUp()
        {
            BeforeTest();
            var bonusAmount = 10000;
            var tradeAmount = 2;
            _roleName = TextManipulation.RandomString();

            // get role by name
            var roleData = _apiFactory
                .ChangeContext<IRolesApi>()
                .GetRoleByNameRequest(_crmUrl, DataRep.AdminRole);

            roleData.Name = _roleName;
            roleData.ErpPermissions.Remove("all_user_trades");
            roleData.ErpPermissions.Remove("see_user_trades");
            roleData.ErpPermissions.Remove("delete_trade");
            roleData.ErpPermissions.Remove("pending_trades");
            roleData.ErpPermissions.Remove("open_trades");
            roleData.ErpPermissions.Remove("closed_trades");
            roleData.ErpPermissions.Remove("market_exposure");

            // create  role 
            _apiFactory
                .ChangeContext<IRolesApi>()
                .PostCreateRoleRequest(_crmUrl, roleData);

            // create user
            var userName = TextManipulation.RandomString();
            _userEmail = userName + DataRep.EmailPrefix;

            _userId = _apiFactory
                .ChangeContext<IUserApi>()
                .PostCreateUserRequest(_crmUrl, userName, role: _roleName);

            // create ApiKey
            _currentUserApiKey = _apiFactory
                .ChangeContext<IUserApi>()
                .PostCreateApiKeyRequest(_crmUrl, _userId);

            // create client 
            var clientName = TextManipulation.RandomString();
            var clientEmail = clientName + DataRep.EmailPrefix;

            _clientId = _apiFactory
                .ChangeContext<ICreateClientApi>()
                .CreateClientRequest(_crmUrl, clientName,
                apiKey: _currentUserApiKey);

            // add bonus
            _apiFactory
                .ChangeContext<IFinancesTabApi>()
                .PostBonusRequest(_crmUrl, _clientId, bonusAmount);

            // create bulk trades 
            _apiFactory
                .ChangeContext<IBulkTradePageApi>()
                .PostCreateBulkTradeRequest(_crmUrl, new string[] { _clientId }, exposure: "1");

            // get the last bulk trade 
            _lastBulkTradeId = _apiFactory
                .ChangeContext<IBulkTradePageApi>()
                .GetBulkTradesRequest(_crmUrl)
                .GeneralResponse
                .data
                .OrderByDescending(p => p.id)
                .LastOrDefault()
                .id;

            // get login Data for trading Platform
            var loginData = _apiFactory
                .ChangeContext<ILoginApi>()
                .PostLoginToTradingPlatform(_tradingPlatformUrl, clientEmail)
                .GeneralResponse;

            var tradeData = _apiFactory
                 .ChangeContext<ITradePageApi>()
                 .PostBuyAssetRequest(_tradingPlatformUrl, tradeAmount,
                 loginData: loginData)
                 .GeneralResponse;

            _tradeId = tradeData.TradeId;
        }

        [TearDown]
        public void TearDown()
        {
            try
            {
                // delete role by name
                _apiFactory
                     .ChangeContext<IUserApi>()
                     .PutEditUserRoleRequest(_crmUrl, _userId, DataRep.AdminRole);

                _apiFactory
                    .ChangeContext<IRolesApi>()
                    .DeleteRoleRequest(_crmUrl, _roleName);
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
        public void VerifyNoPermissionsUsersTradesApiTest()
        {
            var expectedErrorMessage = "Method Not Allowed";

            // update client card with the new account type
            var actualDeleteTradeErrorMessage = _apiFactory
                .ChangeContext<ITradesTabApi>()
                .DeleteTradeRequest(_crmUrl, _tradeId, _currentUserApiKey, false);

            // get trades error
            var actualGetTradeErrorMessage = _apiFactory
                .ChangeContext<ITradesTabApi>()
                .GetTradesRequest(_crmUrl, _clientId, _currentUserApiKey, false)
                .Message;

            // get bulk trades error
            var actualGetBulkTradeErrorMessage = _apiFactory
                .ChangeContext<IBulkTradePageApi>()
                .GetBulkTradesRequest(_crmUrl, _currentUserApiKey, false)
                .Message;

            // create bulk trades error
            var actualCreateBulkTradeErrorMessage = _apiFactory
                .ChangeContext<IBulkTradePageApi>()
                .PostCreateBulkTradeRequest(_crmUrl, new string[] { _clientId },
                apiKey: _currentUserApiKey, checkStatusCode: false);

            // edit bulk trades error
            var actualEditBulkTradeErrorMessage = _apiFactory
                .ChangeContext<IBulkTradePageApi>()
                .PatchEditBulkTradeRequest(_crmUrl, _lastBulkTradeId, _currentUserApiKey, false);

            // close bulk trades error
            var actualCloseBulkTradeErrorMessage = _apiFactory
                .ChangeContext<IBulkTradePageApi>()
                .PatchCloseBulkTradeRequest(_crmUrl, _lastBulkTradeId, _currentUserApiKey, false);

            Assert.Multiple(() =>
            {
                Assert.True(actualDeleteTradeErrorMessage == expectedErrorMessage,
                    $" expected Delete Trade Error Message: {expectedErrorMessage}" +
                    $" actual Delete Trade Error Message: {actualDeleteTradeErrorMessage}");

                Assert.True(actualDeleteTradeErrorMessage == expectedErrorMessage,
                    $" expected get Trades Error Message: {expectedErrorMessage}" +
                    $" actual get Trades Error Message: {actualDeleteTradeErrorMessage}");

                Assert.True(actualGetBulkTradeErrorMessage == expectedErrorMessage,
                    $" expected Get Bulk Trade Error Message: {expectedErrorMessage}" +
                    $" actual Get Bulk Trade Error Message: {actualGetBulkTradeErrorMessage}");

                Assert.True(actualCreateBulkTradeErrorMessage == expectedErrorMessage,
                    $" expected get Trades Error Message: {expectedErrorMessage}" +
                    $" actual get Trades Error Message: {actualCreateBulkTradeErrorMessage}");

                Assert.True(actualEditBulkTradeErrorMessage == expectedErrorMessage,
                    $" expected Edit Bulk Trade Error Message: {expectedErrorMessage}" +
                    $" actual Edit Bulk Trade Error Message: {actualEditBulkTradeErrorMessage}");

                Assert.True(actualCreateBulkTradeErrorMessage == expectedErrorMessage,
                    $" expected close Bulk Trade Error Message: {expectedErrorMessage}" +
                    $" actual close Bulk Trade Error Message: {actualCreateBulkTradeErrorMessage}");
            });
        }
    }
}