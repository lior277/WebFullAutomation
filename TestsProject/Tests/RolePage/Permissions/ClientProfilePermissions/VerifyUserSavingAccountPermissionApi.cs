// Ignore Spelling: Api

using System;
using System.Linq;
using AirSoftAutomationFramework.Internals.DAL.ConfigurationManagement;
using AirSoftAutomationFramework.Internals.Factorys;
using AirSoftAutomationFramework.Internals.Helpers;
using AirSoftAutomationFramework.Internals.NunitAtributes;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.AccountsApi.Roles;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.AccountsApi.UsersMenuApi.UserMenuApi;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.Clients.ClientCard;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.Clients.ClientCard.FinancesTab;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.Clients.ClientCard.SATab;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.Clients.ClientPage;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.Settings;
using NUnit.Framework;
using TestsProject.TestsInternals;

namespace TestsProject.Tests.RolePage.Permissions.ClientProfilePermissions
{
    [TestFixture]
    public class VerifyUserSavingAccountPermissionApi : TestSuitBase
    {
        #region Test Preparation
        private IApplicationFactory _apiFactory = new ApplicationFactory();
        private string _crmUrl = Config.appSettings.CrmUrl;
        private string _roleName;
        private string _userId;
        private string _currentUserApiKey;
        private string _userEmail;
        private string _clientId;

        [SetUp]
        public void SetUp()
        {
            BeforeTest();
            _roleName = TextManipulation.RandomString();

            // get role by name
            var roleData = _apiFactory
                .ChangeContext<IRolesApi>()
                .GetRoleByNameRequest(_crmUrl, DataRep.AdminRole);

            roleData.Name = _roleName;
            roleData.ErpPermissions.Remove("all_user_saving_accounts");

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

            _apiFactory
               .ChangeContext<IFinancesTabApi>()
               .PostDepositRequest(_crmUrl, _clientId, 200, apiKey: _currentUserApiKey);

            // verify default SA exist
            // create Saving Account
            var expectedSavingAccountName = _apiFactory
                .ChangeContext<ISalesTabApi>()
                .PostCreateSavingAccountRequest(_crmUrl, apiKey: _currentUserApiKey);

            var savingAccountId = _apiFactory
                .ChangeContext<ISalesTabApi>()
                .GetSavingAccountsRequest(_crmUrl, _currentUserApiKey)
                .SavingAccountData
                .Where(p => p.Name == expectedSavingAccountName)
                .FirstOrDefault()
                .Id;

            var informationTabResponse = _apiFactory
                .ChangeContext<IInformationTabApi>()
                .GetInformationTabRequest(_crmUrl, _clientId)
                .GeneralResponse
                .informationTab;

            informationTabResponse.saving_account_id = savingAccountId;

            // new first name, new last name, new sale Status, new all kyc, change campaign
            _apiFactory
                .ChangeContext<IInformationTabApi>()
                .PutInformationTabRequest(_crmUrl, informationTabResponse,
                apiKey: _currentUserApiKey);
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
        #endregion

        [Test]
        [Category(DataRep.SanityCategory)]
        [Category(DataRep.RegressionCategory)]
        [RetryMenage(DataRep.NumOfRetraiesForFallingTest)]
        public void VerifyUserSavingAccountPermissionApiTest()
        {
            var _tradingPlatformUrl = Config.appSettings.tradingPlatformUrl;

        var expectedErrorMessage = "Method Not Allowed";

            // CRM Transfer To Saving Account
            var actualTransferToSavingAccountFromCrmError =  _apiFactory
                .ChangeContext<ISATabApi>()
                .PostTransferToSavingAccountRequest(_crmUrl, _clientId, 10,
                _currentUserApiKey, false);

            // CRM Transfer To Balance
            var actualTransferToBalanceFromCrmError = _apiFactory
                .ChangeContext<ISATabApi>()
               .PostTransferToBalanceRequest(_crmUrl, _clientId, 10,
               _currentUserApiKey, false);

            // treading platform Transfer To Saving Account
            var actualTransferToSavingAccountFromTPError = _apiFactory
                .ChangeContext<ISATabApi>()
                .PostTransferToSavingAccountRequest(_tradingPlatformUrl, _clientId, 10,
                _currentUserApiKey, false);

            // treading platform Transfer To Balance
            var actualTransferToBalanceFromTPError = _apiFactory
                .ChangeContext<ISATabApi>()
                .PostTransferToBalanceRequest(_tradingPlatformUrl, _clientId, 10, 
                _currentUserApiKey, false);

            Assert.Multiple(() =>
            {
                Assert.True(actualTransferToSavingAccountFromCrmError == expectedErrorMessage,
                    $" expected Transfer To Saving Account From Crm Error: {expectedErrorMessage}" +
                    $" actual Transfer To Saving Account From Crm Error: {actualTransferToSavingAccountFromCrmError}");

                Assert.True(actualTransferToBalanceFromCrmError == expectedErrorMessage,
                    $" expected Transfer To Balance From Crm Error: {expectedErrorMessage}" +
                    $" actual Transfer To Balance From Crm Error: {actualTransferToBalanceFromCrmError}");

                Assert.True(actualTransferToSavingAccountFromTPError == expectedErrorMessage,
                    $" expected Transfer To Saving Account From TP Error: {expectedErrorMessage}" +
                    $" actual Transfer To Saving Account From TP Error: {actualTransferToSavingAccountFromTPError}");

                Assert.True(actualTransferToBalanceFromTPError == expectedErrorMessage,
                    $" expected Transfer To Balance From TP Error: {expectedErrorMessage}" +
                    $" actual Transfer To Balance From TP Error: {actualTransferToBalanceFromTPError}");
            });
        }
    }
}