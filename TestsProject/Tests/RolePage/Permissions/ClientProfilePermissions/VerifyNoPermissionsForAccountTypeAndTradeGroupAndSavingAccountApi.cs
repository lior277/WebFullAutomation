// Ignore Spelling: Api

using System.Collections.Generic;
using System.Linq;
using AirSoftAutomationFramework.Internals.DAL.ConfigurationManagement;
using AirSoftAutomationFramework.Internals.Factorys;
using AirSoftAutomationFramework.Internals.Helpers;
using AirSoftAutomationFramework.Internals.NunitAtributes;
using AirSoftAutomationFramework.Objects.DTOs;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.AccountsApi.Roles;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.AccountsApi.UsersMenuApi.UserMenuApi;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.Clients.ClientCard;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.Clients.ClientPage;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.Settings;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Trade;
using NUnit.Framework;
using TestsProject.TestsInternals;
using static AirSoftAutomationFramework.Objects.DTOs.GetInformationTabResponse;

namespace TestsProject.Tests.RolePage.Permissions.ClientProfilePermissions
{
    [TestFixture]
    public class VerifyNoPermissionsForAccountTypeAndTradeGroupAndSavingAccountApi : TestSuitBase
    {
        #region Test Preparation
        private IApplicationFactory _apiFactory = new ApplicationFactory();
        private string _crmUrl = Config.appSettings.CrmUrl;
        private string _roleName;
        private string _userId;
        private string _clientId;
        private string _currentUserApiKey;
        private string _userEmail;

        [SetUp]
        public void SetUp()
        {
            BeforeTest();
            var cryptoGroupName = TextManipulation.RandomString();
            _roleName = TextManipulation.RandomString();

            var cryptoGroupDefaultAttr = new Default_Attr
            {
                commision = 0,
                leverage = 1,
                maintenance = 1,
                minimum_amount = 1,
                minimum_step = 1,
                spread = 1,
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
            roleData.ErpPermissions.Remove("all_client_account_type");
            roleData.ErpPermissions.Remove("see_client_account_type");
            roleData.ErpPermissions.Remove("all_client_trade_group");
            roleData.ErpPermissions.Remove("see_client_trade_group");

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

            _clientId = _apiFactory
                .ChangeContext<ICreateClientApi>()
                .CreateClientRequest(_crmUrl, clientName);
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
        public void VerifyNoPermissionsForAccountTypeAndTradeGroupAndSavingAccountApiTest()
        {
            // get client account type id
            var actualAccountTypeIdError = _apiFactory
                .ChangeContext<IInformationTabApi>()
                .GetInformationTabRequest(_crmUrl, _clientId, _currentUserApiKey, false)
                .GeneralResponse?
                .informationTab?
                .account_type_id;
            
            // get client cfd group id
            var actualCfdGroupIdError = _apiFactory
                .ChangeContext<IInformationTabApi>()
                .GetInformationTabRequest(_crmUrl, _clientId, _currentUserApiKey, false)
                .GeneralResponse?
                .informationTab?
                .cfd_group_id;

            // get client cfd group id
            var actualSavingAccountIdError = _apiFactory
                .ChangeContext<IInformationTabApi>()
                .GetInformationTabRequest(_crmUrl, _clientId, _currentUserApiKey, false)
                .GeneralResponse?
                .informationTab?
                .saving_account_id;

            Assert.Multiple(() =>
            {
                Assert.True(actualAccountTypeIdError == null,
                    $" expected Account Type Id Error: null" +
                    $" actual Account Type Id Error: {actualCfdGroupIdError}");

                Assert.True(actualCfdGroupIdError == null,
                     $" expected Cfd Group Id Error: null" +
                     $" actual Cfd Group Id Error: {actualCfdGroupIdError}");

                Assert.True(actualSavingAccountIdError == null,
                      $" expected Saving Account Id Error: null" +
                      $" actual Saving Account Id Error: {actualSavingAccountIdError}");
            });
        }
    }
}