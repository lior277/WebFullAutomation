// Ignore Spelling: Api

using System;
using System.Collections.Generic;
using AirSoftAutomationFramework.Internals.DAL.ConfigurationManagement;
using AirSoftAutomationFramework.Internals.Factorys;
using AirSoftAutomationFramework.Internals.Helpers;
using AirSoftAutomationFramework.Internals.NunitAtributes;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.AccountsApi.Roles;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.AccountsApi.UsersMenuApi.UserMenuApi;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.Banking;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.Clients.ClientCard.FinancesTab;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.Clients.ClientPage;
using NUnit.Framework;
using TestsProject.TestsInternals;

namespace TestsProject.Tests.Banking.DepositsPage.Filters
{
    [TestFixture]
    public class VerifyAssignedToFilterOnBankingDepositPageApi : TestSuitBase
    {
        #region members
        private IApplicationFactory _apiFactory = new ApplicationFactory();
        private string _crmUrl = Config.appSettings.CrmUrl;
        private string _firstUserId;
        private string _roleName;
        private string _secondUserId;
        private int _depositAmount = 400;
        private string _firstUserApiKey;
        private string _secondUserApiKey;
        #endregion      

        [SetUp]
        public void SetUp()
        {
            BeforeTest();
            #region PreCondition

            #region create admin role with users only
            // get role by name
            var roleData = _apiFactory
                .ChangeContext<IRolesApi>()
                .GetRoleByNameRequest(_crmUrl, DataRep.AdminWithUsersOnlyRoleName);

            _roleName = TextManipulation.RandomString();
            roleData.Name = _roleName;
            roleData.UsersOnly = true;

            // create  role 
            _apiFactory
                .ChangeContext<IRolesApi>()
                .PostCreateRoleRequest(_crmUrl, roleData);
            #endregion

            // create first user
            var userName = TextManipulation.RandomString();

            // create user
            _firstUserId = _apiFactory
                .ChangeContext<IUserApi>()
                .PostCreateUserRequest(_crmUrl, userName,
                role: _roleName);

            // create first ApiKey
            _firstUserApiKey = _apiFactory
                .ChangeContext<IUserApi>()
                .PostCreateApiKeyRequest(_crmUrl, _firstUserId);

            // create first client with first campaign
            var clientName = TextManipulation.RandomString();

            var firstClientId = _apiFactory
                .ChangeContext<ICreateClientApi>()
                .CreateClientRequest(_crmUrl, clientName,
                apiKey: _firstUserApiKey);

            var clientsIds = new List<string> { firstClientId };

            // deposit 400 for first client
            _apiFactory
                .ChangeContext<IFinancesTabApi>()
                .PostDepositRequest(_crmUrl, firstClientId,
                _depositAmount, originalCurrency: "EUR", apiKey: _firstUserApiKey);

            // connect first User To first Client 
            _apiFactory
                .ChangeContext<IClientsApi>()
                .PatchMassAssignSaleAgentsRequest(_crmUrl, _firstUserId,
                clientsIds, apiKey: _firstUserApiKey);

            // create first user
            userName = TextManipulation.RandomString();

            // create user
            _secondUserId = _apiFactory
                .ChangeContext<IUserApi>()
                 .PostCreateUserRequest(_crmUrl, userName,
                 role: _roleName);

            // create first ApiKey
            _secondUserApiKey = _apiFactory
                .ChangeContext<IUserApi>()
                .PostCreateApiKeyRequest(_crmUrl, _secondUserId);

            // create second client
            clientName = TextManipulation.RandomString();

            var secondClientId = _apiFactory
                .ChangeContext<ICreateClientApi>()
                .CreateClientRequest(_crmUrl, clientName,
                apiKey: _secondUserApiKey);

            // deposit 400 for second client
            _apiFactory
                .ChangeContext<IFinancesTabApi>()
                .PostDepositRequest(_crmUrl, secondClientId,
                _depositAmount, originalCurrency: "EUR", apiKey: _secondUserApiKey);

            // connect second User To second Client 
            _apiFactory
                .ChangeContext<IClientsApi>()
                .PatchMassAssignSaleAgentsRequest(_crmUrl, _secondUserId,
                new List<string> { secondClientId }, apiKey: _secondUserApiKey);
        }
        #endregion

        [TearDown]
        public void TearDown()
        {
            try
            {
                // delete role by name
                _apiFactory
                     .ChangeContext<IUserApi>()
                     .PutEditUserRoleRequest(_crmUrl, _firstUserId, DataRep.AdminRole);

                _apiFactory
                   .ChangeContext<IUserApi>()
                   .PutEditUserRoleRequest(_crmUrl, _secondUserId, DataRep.AdminRole);

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

        // create first user
        // create first client 
        // create deposit for first client
        // connect first client to first user
        // create second user
        // create second client 
        // create deposit for second client
        // connect second client to second user
        // verify search result in deposit banking
        [Test]
        [Category(DataRep.SanityCategory)]
        [Category(DataRep.RegressionCategory)]
        [RetryMenage(DataRep.NumOfRetriesForFallingTest)]
        public void VerifyAssignedToFilterOnBankingDepositPageApiTest()
        {
            var getDepositsByFirstUserIdAssigned = _apiFactory
                .ChangeContext<IDepositsPageApi>()
                .GetDepositDataFromBankingRequest(_crmUrl, "erp_user_id_assigned",
                _firstUserId, _firstUserApiKey)
                .GeneralResponse;

            var getDepositsBySecondUserIdAssigned = _apiFactory
                .ChangeContext<IDepositsPageApi>()
                .GetDepositDataFromBankingRequest(_crmUrl, "erp_user_id_assigned",
                _secondUserId, _secondUserApiKey)
                .GeneralResponse;

            Assert.Multiple(() =>
            {
                Assert.True(getDepositsByFirstUserIdAssigned.recordsTotal == 1,
                    $" expected Total records: 1" +
                    $" actual Total records :  {getDepositsByFirstUserIdAssigned.recordsTotal}");

                Assert.True(getDepositsByFirstUserIdAssigned.data[0].status == "approved",
                    $" expected deposit status: approved" +
                    $" actual deposit status :  {getDepositsByFirstUserIdAssigned.data[0].status}");

                Assert.True(getDepositsBySecondUserIdAssigned.recordsTotal == 1,
                    $" expected Total records: 1" +
                    $" actual Total records :  {getDepositsBySecondUserIdAssigned.recordsTotal}");

                Assert.True(getDepositsBySecondUserIdAssigned.data[0].status == "approved",
                    $" expected deposit status: approved" +
                    $" actual deposit status :  {getDepositsBySecondUserIdAssigned.data[0].status}");
            });
        }
    }
}