// Ignore Spelling: Api

using System;
using System.Collections.Generic;
using System.Linq;
using AirSoftAutomationFramework.Internals.DAL.ConfigurationManagement;
using AirSoftAutomationFramework.Internals.Factory;
using AirSoftAutomationFramework.Internals.Helpers;
using AirSoftAutomationFramework.Internals.NunitAttributes;
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
    public class VerifyAccountTypeAndTradeGroupViewPermissionApi : TestSuitBase
    {
        #region Test Preparation
        private IApplicationFactory _apiFactory = new ApplicationFactory();
        private string _crmUrl = Config.appSettings.CrmUrl;
        private string _roleName;
        private string _userId;
        private string _clientId;
        private string _groupId;
        private string _currentUserApiKey;
        private string _userEmail;
        private string _expectedAccountTypeId;
        private string _expectedSavingAccountId;
        private string _expectedCfdGroupId;
        private InformationTab _informationTabResponse;

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
            roleData.ErpPermissions.Remove("all_client_trade_group");

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
                .CreateClientRequest(_crmUrl, clientName,
                apiKey: _currentUserApiKey);

            // create Account type with chrono disable
            var automationAccountType = _apiFactory
                 .ChangeContext<ISalesTabApi>()
                 .CreateAutomationAccountTypePipe(_crmUrl);

            // create group  
            _groupId = _apiFactory
                .ChangeContext<ITradeGroupApi>()
                .PostCreateTradeGroupRequest(_crmUrl,
                new List<object> { cryptoGroupDefaultAttr }, cryptoGroupName);

            // verify default SA exist
            // create Saving Account
            var savingAccountName = _apiFactory
                .ChangeContext<ISalesTabApi>()
                .PostCreateSavingAccountRequest(_crmUrl);

            // get SA id
            var savingAccountId = _apiFactory
                .ChangeContext<ISalesTabApi>()
                .GetSavingAccountsRequest(_crmUrl)
                .SavingAccountData
                .Where(p => p.Name == savingAccountName)
                .FirstOrDefault()
                .Id;

            _informationTabResponse = _apiFactory
                 .ChangeContext<IInformationTabApi>()
                 .GetInformationTabRequest(_crmUrl, _clientId)
                 .GeneralResponse
                 .informationTab;

            _expectedSavingAccountId = _informationTabResponse.saving_account_id;
            _expectedCfdGroupId = _informationTabResponse.cfd_group_id;
            _expectedAccountTypeId = _informationTabResponse.account_type_id;
            _informationTabResponse.cfd_group_id = _groupId;
            _informationTabResponse.saving_account_id = savingAccountId;
            _informationTabResponse.account_type_id = automationAccountType.AccountTypeId;
            _informationTabResponse.saving_account_id = "null";
            _informationTabResponse.sales_agent = _userId;
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

                // Delete Trade Group
                _apiFactory
                    .ChangeContext<ITradeGroupApi>()
                    .DeleteTradeGroupRequest(_crmUrl, _groupId);
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
        [RetryMenage(DataRep.NumOfRetraiesForFallingTest)]
        public void VerifyAccountTypeAndTradeGroupViewPermissionApiTest()
        {
            // update client card with the new account type
            _apiFactory
                .ChangeContext<IInformationTabApi>()
                .PutInformationTabRequest(_crmUrl, _informationTabResponse,
                _currentUserApiKey, false);

            var actualCfdGroupId = _apiFactory
                .ChangeContext<IInformationTabApi>()
                .GetInformationTabRequest(_crmUrl, _clientId)
                .GeneralResponse
                .informationTab
                .cfd_group_id;

            var actualAccountTypeId = _apiFactory
                .ChangeContext<IInformationTabApi>()
                .GetInformationTabRequest(_crmUrl, _clientId)
                .GeneralResponse
                .informationTab
                .account_type_id;

            Assert.Multiple(() =>
            {
                Assert.True(actualCfdGroupId == _expectedCfdGroupId,
                    $" expected Cfd Group Id: {_expectedCfdGroupId}" +
                    $" actual Cfd Group Id: {actualCfdGroupId}");

                Assert.True(actualAccountTypeId == _expectedAccountTypeId,
                    $" expected Account Type Id: {_expectedAccountTypeId}" +
                    $" actual Account Type Id: {actualAccountTypeId}");
            });
        }
    }
}