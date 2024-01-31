// Ignore Spelling: Api

using System.Linq;
using System.Threading;
using AirSoftAutomationFramework.Internals.DAL.ConfigurationManagement;
using AirSoftAutomationFramework.Internals.Factorys;
using AirSoftAutomationFramework.Internals.Helpers;
using AirSoftAutomationFramework.Internals.NunitAtributes;
using AirSoftAutomationFramework.Models;
using AirSoftAutomationFramework.Objects.DTOs;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.AccountsApi.Roles;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.AccountsApi.UsersMenuApi.UserMenuApi;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.Clients.ClientCard.FinancesTab;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.Clients.ClientPage;
using NUnit.Framework;
using TestsProject.TestsInternals;

namespace TestsProject.Tests.RolePage.Permissions.ClientProfilePermissions
{
    [TestFixture]
    public class VerifyNoPermissionsUsersTransactionsApi : TestSuitBase
    {
        #region Test Preparation
        private IApplicationFactory _apiFactory = new ApplicationFactory();
        private string _crmUrl = Config.appSettings.CrmUrl;
        private string _roleName;
        private GetRoleByNameResponse _roleData;
        private string _userId;
        private string _currentUserApiKey;
        private string _tradingPlatformUrl =  Config.appSettings.tradingPlatformUrl;
        private string _userEmail;
        private string _depositId;
        private string _bonusId;
        private string _withdrawalId;
        private int _depositAmount = 1000;
        private int _bonusAmount = 1000;
        private string _clientId;
        private QaAutomation01Context _dbContext = new QaAutomation01Context();

        [SetUp]
        public void SetUp()
        {
            BeforeTest();
            _roleName = TextManipulation.RandomString();

            // get role by name
            _roleData = _apiFactory
                .ChangeContext<IRolesApi>()
                .GetRoleByNameRequest(_crmUrl, DataRep.AdminRole);

            _roleData.Name = _roleName;         

            // create  role 
            _apiFactory
                .ChangeContext<IRolesApi>()
                .PostCreateRoleRequest(_crmUrl, _roleData);

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

            // create deposit
            _depositId = _apiFactory
                .ChangeContext<IFinancesTabApi>()
                .PostDepositRequest(_crmUrl,
                _clientId, _depositAmount);

            _bonusId = _apiFactory
               .ChangeContext<IFinancesTabApi>()
               .PostBonusRequest(_crmUrl, _clientId, _bonusAmount)
               .GeneralResponse
               .InsertId;

            var WithdrawalAmount = 30;

            // Withdrawal deposit
             _apiFactory
                .ChangeContext<IFinancesTabApi>()
                .PostWithdrawalDepositRequest(_crmUrl, _clientId,
                _userId, WithdrawalAmount);

            // get bonus id by amount
            _withdrawalId =
                 (from s in _dbContext.FundsTransactions
                  where (s.UserId == _clientId
                  && s.OriginalAmount == -WithdrawalAmount
                  && s.Type == "withdrawal")
                  select s.Id)
                  .First()
                  .ToString();

            // get role by name
            _roleData = _apiFactory
                .ChangeContext<IRolesApi>()
                .GetRoleByNameRequest(_crmUrl, _roleName);

            _roleData.ErpPermissions.Remove("all_user_transactions");
            _roleData.ErpPermissions.Remove("see_user_transactions");
            _roleData.ErpPermissions.Remove("deposit");
            _roleData.ErpPermissions.Remove("deposit_bonus");
            _roleData.ErpPermissions.Remove("chargeback");
            _roleData.ErpPermissions.Remove("withdrawal");
            _roleData.ErpPermissions.Remove("withdrawal_bonus");
            _roleData.ErpPermissions.Remove("delete_bonus");
            _roleData.ErpPermissions.Remove("dashboard_sales_performance");
            _roleData.ErpPermissions.Remove("dashboard_transactions");
            _roleData.ErpPermissions.Remove("see_transactions");
            _roleData.ErpPermissions.Remove("all_transactions");
            _roleData.ErpPermissions.Remove("sales_deposit");

            // edit role and enable edit comments
            _apiFactory
                .ChangeContext<IRolesApi>()
                .PutEditRoleRequest(_crmUrl, _roleData);      
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
        [RetryMenage(DataRep.NumOfRetraiesForFallingTest)]
        public void VerifyNoPermissionsUsersTransactionsApiTest()
        {
            var expectedErrorMessage = "Method Not Allowed";

            var actualAddDepositErrorMessage = _apiFactory
                .ChangeContext<IFinancesTabApi>()
                .PostDepositRequest(_crmUrl, _clientId, 10000,
                apiKey: _currentUserApiKey, checkStatusCode: false);

            var actualAddBonusErrorMessage = _apiFactory
                .ChangeContext<IFinancesTabApi>()
                .PostBonusRequest(_crmUrl, _clientId,
                apiKey: _currentUserApiKey, checkStatusCode: false)
                .Message;

            // Withdrawal deposit
            var actualWithdrawalErrorMessage = _apiFactory
                .ChangeContext<IFinancesTabApi>()
                .PostWithdrawalDepositRequest(_crmUrl, _clientId, _userId,
                30, apiKey: _currentUserApiKey, checkStatusCode: false);      

            // Withdrawal bonus
            var actualWithdrawalBonusErrorMessage = _apiFactory
                 .ChangeContext<IFinancesTabApi>()
                 .PostWithdrawalBonusRequest(_crmUrl,
                _clientId, 30, _currentUserApiKey, false);

            // chargeback
            var actualChargebackErrorMessage = _apiFactory
                .ChangeContext<IFinancesTabApi>()
                .DeleteChargeBackDepositRequest(_crmUrl,
                _clientId, _depositAmount, _depositId, _currentUserApiKey, false);

            // delete bonus
            var actualDeleteBonusErrorMessage = _apiFactory
                .ChangeContext<IFinancesTabApi>()
                .DeleteFinanceItemRequest(_crmUrl, _bonusId, _currentUserApiKey, false);

            // delete bonus
            var actualGetFinanceDataErrorMessage = _apiFactory
                .ChangeContext<IFinancesTabApi>()
                .GetFinanceDataRequest(_crmUrl, _clientId, _currentUserApiKey, false)
                .Message;

            Assert.Multiple(() =>
            {
                Assert.True(actualAddDepositErrorMessage == expectedErrorMessage,
                    $" expected Add Deposit Error Message: {expectedErrorMessage}" +
                    $" actual Add Deposit Error Message: {actualAddDepositErrorMessage}");

                Assert.True(actualAddBonusErrorMessage == expectedErrorMessage,
                    $" expected Add Bonus Error Message: {expectedErrorMessage}" +
                    $" actual Add Bonus Error Message: {actualAddBonusErrorMessage}");

                Assert.True(actualWithdrawalErrorMessage == expectedErrorMessage,
                    $" expected Add Withdrawal Error Message: {expectedErrorMessage}" +
                    $" actual Add Withdrawal Error Message: {actualWithdrawalErrorMessage}");

                Assert.True(actualWithdrawalBonusErrorMessage == expectedErrorMessage,
                    $" expected Add Withdrawal Bonus Error Message: {expectedErrorMessage}" +
                    $" actual Add Withdrawal Bonus Error Message: {actualWithdrawalBonusErrorMessage}");

                Assert.True(actualChargebackErrorMessage == expectedErrorMessage,
                    $" expected Add Chargeback Error Message: {expectedErrorMessage}" +
                    $" actual Add Chargeback Error Message: {actualChargebackErrorMessage}");

                Assert.True(actualDeleteBonusErrorMessage == expectedErrorMessage,
                    $" expected Add Delete Bonus Error Message: {expectedErrorMessage}" +
                    $" actual Add Delete Bonus Error Message: {actualDeleteBonusErrorMessage}");

                Assert.True(actualGetFinanceDataErrorMessage == expectedErrorMessage,
                   $" expected Get Finance Data Error Message: {expectedErrorMessage}" +
                   $" actual Get Finance Data Error Message: {actualGetFinanceDataErrorMessage}");
            });
        }
    }
}