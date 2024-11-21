// Ignore Spelling: Api

using System;
using System.Collections.Generic;
using AirSoftAutomationFramework.Internals.DAL.ConfigurationManagement;
using AirSoftAutomationFramework.Internals.Factorys;
using AirSoftAutomationFramework.Internals.Helpers;
using AirSoftAutomationFramework.Internals.NunitAtributes;
using AirSoftAutomationFramework.Models;
using AirSoftAutomationFramework.Objects.DTOs;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.AccountsApi.Roles;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.AccountsApi.UsersMenuApi.UserMenuApi;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.Clients.ClientCard;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.Clients.ClientCard.FinancesTab;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.Clients.ClientPage;
using AirSoftAutomationFramework.Objects.ForexObjects.ApiGeneral.Login;
using AirSoftAutomationFramework.Objects.ForexObjects.TradingPlatform.TradeObjects.TradeApi;
using NUnit.Framework;
using TestsProject.TestsInternals;

namespace TestsProject.Tests.ClientCard
{
    [TestFixture]
    public class VerifyEditSwapPermissionApi : TestSuitBase
    {
        #region Members
        private string _currentUserApiKey;
        private IApplicationFactory _apiFactory = new ApplicationFactory();
        private string _crmUrl = Config.appSettings.CrmUrl;
        private QaAutomation01Context _dbContext = new QaAutomation01Context();
        private string _tradingPlatformUrl = Config.appSettings.tradingPlatformUrl;
        private string _clientId;
        private string _clientEmail;
        private GetRoleByNameResponse _roleData;
        private string _roleName;
        private string _userId;
        private string _userName;
        private string _tradeId;
        private int _tradeAmount = 2;
        #endregion        

        [SetUp]
        public void SetUp()
        {
            #region Test Preparation
            BeforeTest();
            var depositAmount = 10000;

            _roleName = TextManipulation.RandomString();

            // get role by name
            _roleData = _apiFactory
                .ChangeContext<IRolesApi>()
                .GetRoleByNameRequest(_crmUrl, DataRep.AdminRole);

            _roleData.Name = _roleName;
            _roleData.UsersOnly = true;
            _roleData.ErpPermissions.Remove("edit_swap");
            _roleData.ErpPermissions.Add("edit_swap");

            // create  role 
            _apiFactory
                .ChangeContext<IRolesApi>()
                .PostCreateRoleRequest(_crmUrl, _roleData);

            // create user
            _userName = TextManipulation.RandomString();

            // create user
            _userId = _apiFactory
                .ChangeContext<IUserApi>()
                .PostCreateUserRequest(_crmUrl, _userName, role: _roleName);

            // create ApiKey
            _currentUserApiKey = _apiFactory
                .ChangeContext<IUserApi>()
                .PostCreateApiKeyRequest(_crmUrl, _userId);

            // create client 
            var clientName = TextManipulation.RandomString();
            _clientEmail = clientName + DataRep.EmailPrefix;

            _clientId = _apiFactory
                .ChangeContext<ICreateClientApi>()
                .CreateClientRequest(_crmUrl, clientName,
                apiKey: _currentUserApiKey);

            var clientsIds = new List<string> { _clientId };

            // connect One User To One Client 
            _apiFactory
                .ChangeContext<IClientsApi>()
                .PatchMassAssignSaleAgentsRequest(_crmUrl, _userId,
                clientsIds, apiKey: _currentUserApiKey);

            // get login Data for trading Platform
            var loginData = _apiFactory
                .ChangeContext<ILoginApi>()
                .PostLoginToTradingPlatform(_tradingPlatformUrl, _clientEmail)
                .GeneralResponse;

            // create deposit for trade
            _apiFactory
               .ChangeContext<IFinancesTabApi>()
               .PostDepositRequest(_crmUrl, _clientId,
               depositAmount, apiKey: _currentUserApiKey);

            // create tread
            var tradeData = _apiFactory
                .ChangeContext<ITradePageApi>()
                .PostBuyAssetRequest(_tradingPlatformUrl, _tradeAmount, loginData: loginData);

            _tradeId = tradeData.GeneralResponse.TradeId;
            #endregion
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
                AfterTest();
            }
        }

        [Test]
        [Description("Based on https://airsoftltd.atlassian.net/browse/AIRV2-5229")]
        [Category(DataRep.SanityCategory)]
        [Category(DataRep.RegressionCategory)]
        [RetryMenage(DataRep.NumOfRetriesForFallingTest)]
        public void VerifyEditSwapPermissionApiTest()
        {
            var expectedError = "Method Not Allowed";

            // edit trade enable with super admin
            var actualEditTradeWithSuperAdmin = _apiFactory
                .ChangeContext<ITradesTabApi>()
                .PachtEditTradeByIdRequest(_tradingPlatformUrl, _tradeId,
                0.1, 0.1, 0.1, "open", 0);

            // edit swap With Permission
            var actualEditSwapWithAdminPermission = _apiFactory
                .ChangeContext<ITradesTabApi>()
                .PachtEditSwapByTradeIdRequest(_tradingPlatformUrl,
                _tradeId, 0.1, 0.1, 0.1, _currentUserApiKey);

            // get role by name
            _roleData = _apiFactory
                .ChangeContext<IRolesApi>()
                .GetRoleByNameRequest(_crmUrl, _roleName);

            _roleData.ErpPermissions.Remove("edit_swap");

            // edit role and enable edit_swap
            _apiFactory
                .ChangeContext<IRolesApi>()
                .PutEditRoleRequest(_crmUrl, _roleData);

            // edit swap Without Permission
            var actualEditSwapWithoutPermission = _apiFactory
                .ChangeContext<ITradesTabApi>()
                .PachtEditSwapByTradeIdRequest(_tradingPlatformUrl,
                _tradeId, 0.1, 0.1, 0.1, _currentUserApiKey, false);

            Assert.Multiple(() =>
            {
                Assert.True(actualEditTradeWithSuperAdmin.Contains("ok"),
                    $" expected Edit Trade With Super Admin : Contains Ok," +
                    $" actual Edit Trade With Super Admin: {actualEditTradeWithSuperAdmin}");

                Assert.True(actualEditSwapWithAdminPermission.Contains("ok"),
                    $" expected Edit Swap With Admin Permission: Contains Ok," +
                    $" actual Edit Swap With Admin Permission: {actualEditSwapWithAdminPermission}");

                Assert.True(actualEditSwapWithoutPermission == expectedError,
                    $" expected Edit Swap Without Permission: {expectedError}," +
                    $" actual Edit Swap Without Permission: {actualEditSwapWithoutPermission}");
            });
        }
    }
}