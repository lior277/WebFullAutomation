// Ignore Spelling: Api

using System.Collections.Generic;
using System.Linq;
using AirSoftAutomationFramework.Internals.DAL.ConfigurationManagement;
using AirSoftAutomationFramework.Internals.Factorys;
using AirSoftAutomationFramework.Internals.Helpers;
using AirSoftAutomationFramework.Internals.NunitAtributes;
using AirSoftAutomationFramework.Objects.DTOs;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.AccountsApi.Roles;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.AccountsApi.UsersMenuApi;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.AccountsApi.UsersMenuApi.UserMenuApi;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.Clients.ClientCard;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.Clients.ClientPage;
using Microsoft.Graph;
using NUnit.Framework;
using TestsProject.TestsInternals;
using static AirSoftAutomationFramework.Objects.DTOs.GetInformationTabResponse;

namespace TestsProject.Tests.RolePage.Permissions
{
    [TestFixture]
    public class VerifyUserOnlyPermissionApi : TestSuitBase
    {
        #region Test Preparation
        private IApplicationFactory _apiFactory = new ApplicationFactory();
        private string _crmUrl = Config.appSettings.CrmUrl;
        private string _userId;
        private string _userName;
        private string _adminUserId;
        private GetUserResponse _adminUserData;
        private InformationTab _informationTabResponse;
        private string _clientEmail;
        private string _currentUserApiKey;

        [SetUp]
        public void SetUp()
        {
            BeforeTest();

            // create Admin With Users Only
            _userName = TextManipulation.RandomString();

            _userId = _apiFactory
                .ChangeContext<IUserApi>()
                .PostCreateUserRequest(_crmUrl, _userName, role: DataRep.AdminWithUsersOnlyRoleName);

            // create ApiKey of Admin With Users Only
            _currentUserApiKey = _apiFactory
                .ChangeContext<IUserApi>()
                .PostCreateApiKeyRequest(_crmUrl, _userId);

            // create Admin user
            var adminUserName = TextManipulation.RandomString();

            _adminUserId = _apiFactory
                .ChangeContext<IUserApi>()
                .PostCreateUserRequest(_crmUrl, adminUserName);

            // get admin user
            _adminUserData = _apiFactory
               .ChangeContext<IUsersApi>()
               .GetUserByIdRequest(_crmUrl, _adminUserId)
               .GeneralResponse;

            // create client
            var clientName = TextManipulation.RandomString();
            _clientEmail = clientName + DataRep.EmailPrefix;

            var clientId = _apiFactory
                .ChangeContext<ICreateClientApi>()
                .CreateClientRequest(_crmUrl, clientName);

            _informationTabResponse = _apiFactory
                .ChangeContext<IInformationTabApi>()
                .GetInformationTabRequest(_crmUrl, clientId)
                .GeneralResponse
                .informationTab;
        }

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
        #endregion

        [Test]
        [Category(DataRep.SanityCategory)]
        [Category(DataRep.RegressionCategory)]
        [RetryMenage(DataRep.NumOfRetriesForFallingTest)]
        public void VerifyUserOnlyPermissionApiTest()
        {
            // update the user with new data
            var actualEditUserError = _apiFactory
                .ChangeContext<IUserApi>()
                .PutEditUserRequest(_crmUrl, _adminUserData, _currentUserApiKey, false);

            // update Saving Account
            var actualEditClientError = _apiFactory
                .ChangeContext<IInformationTabApi>()
                .PutInformationTabRequest(_crmUrl, _informationTabResponse,
                apiKey: _currentUserApiKey, false)
                .Value;

            Assert.Multiple(() =>
            {
                Assert.True(actualEditUserError == "Not Found",
                    $" expected Edit User Error: Not Found" +
                    $" actual Edit User Error: {actualEditUserError}");

                Assert.True(actualEditClientError == 0,
                    $" expected Edit client Error: 0" +
                    $" actual Edit client Error: {actualEditClientError}");
            });
        }
    }
}