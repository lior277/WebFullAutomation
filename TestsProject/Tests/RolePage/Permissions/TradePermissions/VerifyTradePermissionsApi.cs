// Ignore Spelling: Api
using System;
using System.Collections.Generic;
using AirSoftAutomationFramework.Internals.DAL.ConfigurationManagement;
using AirSoftAutomationFramework.Internals.Factory;
using AirSoftAutomationFramework.Internals.Helpers;
using AirSoftAutomationFramework.Internals.NunitAttributes;
using AirSoftAutomationFramework.Objects.DTOs;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.AccountsApi.Roles;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.AccountsApi.UsersMenuApi.UserMenuApi;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Trade;
using AirSoftAutomationFramework.Objects.ForexObjects.ApiGeneral;
using AirSoftAutomationFramework.Objects.ForexObjects.Ui.Login;
using AirSoftAutomationFramework.Objects.ForexObjects.Ui.UiGeneral;
using NUnit.Framework;
using OpenQA.Selenium;
using TestsProject.TestsInternals;

namespace TestsProject.Tests.RolePage.Permissions.TradePermissions
{
    [TestFixture(DataRep.Chrome)]
    public class VerifyTradePermissionsApi : TestSuitBase
    {
        #region Test Preparation

        public VerifyTradePermissionsApi(string browser) : base(browser)
        {
            _browserName = browser;
        }

        private IApplicationFactory _apiFactory = new ApplicationFactory();
        private string _crmUrl = Config.appSettings.CrmUrl;
        private string _roleName;
        private string _userId;
        private string _browserName;
        private IWebDriver _driver;
        private Default_Attr _tradeGroup;
        private string _currentUserApiKey;

        [SetUp]
        public void SetUp()
        {
            BeforeTest();
            _roleName = TextManipulation.RandomString();
            _driver = GetDriver();

            _tradeGroup = new Default_Attr
            {
                commision = 0,
                leverage = 5,
                maintenance = 1,
                minimum_amount = 1,
                minimum_step = 1,
                spread = -0.01,
                margin_call = null,
                swap_long = 0.04,
                swap_short = 0.04,
                swap_time = "00:00:00",
            };

            // get role by name
            var roleData = _apiFactory
                .ChangeContext<IRolesApi>()
                .GetRoleByNameRequest(_crmUrl, DataRep.AdminRole);

            roleData.Name = _roleName;
            roleData.ErpPermissions.Remove("assets_on_front");
            roleData.ErpPermissions.Remove("market_exposure");
            roleData.ErpPermissions.Remove("all_trade_groups");
            roleData.ErpPermissions.Remove("see_trade_groups");
            roleData.ErpPermissions.Remove("open_trades");
            roleData.ErpPermissions.Remove("pending_trades");
            roleData.ErpPermissions.Remove("closed_trades");
            roleData.ErpPermissions.Remove("hourly_pnl");
            roleData.ErpPermissions.Remove("see_risk");

            // create  role 
            _apiFactory
                .ChangeContext<IRolesApi>()
                .PostCreateRoleRequest(_crmUrl, roleData);

            // create user
            var userName = TextManipulation.RandomString();

            _userId = _apiFactory
                .ChangeContext<IUserApi>(_driver)
                .CreateUserForUiPipe(_crmUrl, userName, role: _roleName);

            _apiFactory
                .ChangeContext<ILoginPageUi>(_driver)
                .LoginPipe(userName);

            // create ApiKey
            _currentUserApiKey = _apiFactory
                .ChangeContext<IUserApi>()
                .PostCreateApiKeyRequest(_crmUrl, _userId);
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
        #endregion

        [Test]
        [Description("BUG get Trade Groups Error Message: Method Not Allowed")]
        [Category(DataRep.SanityCategory)]
        [Category(DataRep.RegressionCategory)]
        [RetryMenage(DataRep.NumOfRetraiesForFallingTest)]
        public void VerifyTradePermissionsApiTest()
        {
            var cryptoGroupName = TextManipulation.RandomString();
            var expected405Error = "Method Not Allowed";

            var expected404Error = "AIRSOFT Error 404 something's" +
                " not right here  the page you are looking for" +
                " cannot be found!  TRY AGAIN RETURN TO HOME";

            var actualGetAssetsOnFrontErrorMessage = _apiFactory
                .ChangeContext<IAssetsOnFrontPageApi>()
                .GetAssetsOnFrontRequest(_crmUrl, _currentUserApiKey, false);

            var actualCreateTradeGroupsErrorMessage = _apiFactory
                .ChangeContext<ITradeGroupApi>()
                .PostCreateTradeGroupRequest(_crmUrl, 
                 new List<object> { _tradeGroup }, cryptoGroupName, _currentUserApiKey, false);

            var actualGetTradeGroupErrorMessage = _apiFactory
                 .ChangeContext<ITradeGroupApi>()
                 .GetTradeGroupsRequest(_crmUrl, _currentUserApiKey, false)
                 .Message;

            var actualOpenTradeError = _apiFactory
                .ChangeContext<IMenus>(_driver)
                .ChangeContext<ISharedStepsGenerator>(_driver)
                .NavigateToPageByName(_crmUrl, "/groups/trades/open", checkUrl: false)
                .GetError404();

            var actualPendingTradeError = _apiFactory
                .ChangeContext<IMenus>(_driver)
                .ChangeContext<ISharedStepsGenerator>(_driver)
                .NavigateToPageByName(_crmUrl, "/groups/trades/pending", checkUrl: false)
                .GetError404();

            var actualCloseTradeError = _apiFactory
                .ChangeContext<IMenus>(_driver)
                .ChangeContext<ISharedStepsGenerator>(_driver)
                .NavigateToPageByName(_crmUrl, "/groups/trades/close", checkUrl: false)
                .GetError404();

            var actualGetPnlErrorMessage = _apiFactory
                 .ChangeContext<IHourlyPnlPageApi>()
                 .GetHourlyPnlRequest(_crmUrl, _currentUserApiKey, false);

            var actualGetRisksErrorMessage = _apiFactory
                .ChangeContext<IRiskPageApi>()
                .GetRisksRequest(_crmUrl, _currentUserApiKey, false);

            Assert.Multiple(() =>
            {
                Assert.True(actualGetAssetsOnFrontErrorMessage == expected405Error,
                    $" expected Get Assets OnFront Error Message: {expected405Error}" +
                    $" actual Get Assets OnFront Error Message: {actualGetAssetsOnFrontErrorMessage}");

                Assert.True(actualCreateTradeGroupsErrorMessage == expected405Error,
                    $" expected Create Trade Groups Error Message: {expected405Error}" +
                    $" actual Create Trade Groups Error Message: {actualCreateTradeGroupsErrorMessage}");

                Assert.True(actualGetTradeGroupErrorMessage == expected405Error,
                    $" expected get Trade Groups Error Message: {expected405Error}" +
                    $" actual get Trade Groups Error Message: {actualGetTradeGroupErrorMessage}");

                Assert.True(actualOpenTradeError == expected404Error,
                    $" expected Open Trade Error  Message: {expected404Error}" +
                    $" actual Open Trade Error  Message: {actualOpenTradeError}");

                Assert.True(actualPendingTradeError == expected404Error,
                    $" expected Pending Trade Error  Message: {expected404Error}" +
                    $" actual Pending Trade Error  Message: {actualPendingTradeError}");

                Assert.True(actualCloseTradeError == expected404Error,
                    $" expected Close Trade Error  Message: {expected404Error}" +
                    $" actual Close Trade Error  Message: {actualCloseTradeError}");

                Assert.True(actualGetPnlErrorMessage == expected405Error,
                    $" expected Get Pnl Error Message: {expected405Error}" +
                    $" actual Get Pnl Error Message: {actualGetPnlErrorMessage}");

                Assert.True(actualGetRisksErrorMessage == expected405Error,
                    $" expected Get Risks Error Message: {expected405Error}" +
                    $" actual Get Risks Error Message: {actualGetRisksErrorMessage}");
            });
        }
    }
}