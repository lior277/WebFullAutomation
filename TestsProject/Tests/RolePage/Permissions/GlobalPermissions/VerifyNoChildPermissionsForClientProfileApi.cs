// Ignore Spelling: Api

using AirSoftAutomationFramework.Internals.DAL.ConfigurationManagement;
using AirSoftAutomationFramework.Internals.Factory;
using AirSoftAutomationFramework.Internals.Helpers;
using AirSoftAutomationFramework.Internals.NunitAttributes;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.AccountsApi.Roles;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.AccountsApi.UsersMenuApi.UserMenuApi;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.Clients.ClientCard;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.Clients.ClientPage;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.Settings;
using NUnit.Framework;
using TestsProject.TestsInternals;
using static AirSoftAutomationFramework.Objects.DTOs.GetInformationTabResponse;

namespace TestsProject.Tests.RolePage.Permissions.GlobalPermissions
{
    [TestFixture]
    public class VerifyNoChildPermissionsForClientProfileApi : TestSuitBase
    {
        #region Test Preparation
        private IApplicationFactory _apiFactory = new ApplicationFactory();
        private string _crmUrl = Config.appSettings.CrmUrl;
        private string _currentUserApiKey;
        private string _roleName;
        private string _clientId;
        private InformationTab _informationTabData;
        private string _expectedEmail;
        private string _expectedPhone;
        private string _userId;
        private string _bannerId;
        private string _expectedSalesAgent;

        [SetUp]
        public void SetUp()
        {
            BeforeTest();
            _roleName = TextManipulation.RandomString();

            _bannerId = _apiFactory
                 .ChangeContext<IPlatformTabApi>()
                 .CreateBannerPipe(_crmUrl, DataRep.AutomationBannerName);

            // get role by name
            var roleData = _apiFactory
                .ChangeContext<IRolesApi>()
                .GetRoleByNameRequest(_crmUrl, DataRep.AdminRole);

            roleData.Name = _roleName;
            roleData.ErpPermissions.Remove("all_client_profile");
            roleData.ErpPermissions.Remove("see_client_profile");
            roleData.ErpPermissions.Remove("all_phone");
            roleData.ErpPermissions.Remove("see_phone");
            roleData.ErpPermissions.Remove("all_email");
            roleData.ErpPermissions.Remove("see_email");
            roleData.ErpPermissions.Remove("all_sales_agent");
            roleData.ErpPermissions.Remove("see_sales_agent");
            roleData.ErpPermissions.Remove("all_feed_tab");
            roleData.ErpPermissions.Remove("see_feed_tab");

            // create  role 
            _apiFactory
                .ChangeContext<IRolesApi>()
                .PostCreateRoleRequest(_crmUrl, roleData);

            // create user
            var userName = TextManipulation.RandomString();

            _userId = _apiFactory
                .ChangeContext<IUserApi>()
                .PostCreateUserRequest(_crmUrl, userName, role: _roleName);

            // create ApiKey
            _currentUserApiKey = _apiFactory
                .ChangeContext<IUserApi>()
                .PostCreateApiKeyRequest(_crmUrl, _userId);

            // create user
            userName = TextManipulation.RandomString();

            var userIdForRole = _apiFactory
                .ChangeContext<IUserApi>()
                .PostCreateUserRequest(_crmUrl, userName);

            // create client 
            var clientName = TextManipulation.RandomString();
            var clientEmail = clientName + DataRep.EmailPrefix;

            _clientId = _apiFactory
                .ChangeContext<ICreateClientApi>()
                .CreateClientRequest(_crmUrl, clientName,
                apiKey: _currentUserApiKey);

            _informationTabData = _apiFactory
                .ChangeContext<IInformationTabApi>()
                .GetInformationTabRequest(_crmUrl, _clientId)
                .GeneralResponse
                .informationTab;

            var newclientEmail = $"new{clientEmail}";
            var newClientPhone = $"123{_informationTabData.phone}";

            _informationTabData.saving_account_id = "null";
            _expectedEmail = _informationTabData.email;
            _expectedPhone = _informationTabData.phone;
            _expectedSalesAgent = _informationTabData.sales_agent;
            _informationTabData.email = newclientEmail;
            _informationTabData.phone = newClientPhone;
            _informationTabData.sales_agent = userIdForRole;
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
        public void VerifyNoChildPermissionForClientProfileApiTest()
        {
            var expectedErrorMessage = "Method Not Allowed";

           var actualGetClientCardError =  _apiFactory
                .ChangeContext<IInformationTabApi>()
                .PutInformationTabRequest(_crmUrl,
                _informationTabData, apiKey: _currentUserApiKey, false)
                .Message;

            var actualdPutClientCardError = _apiFactory
                .ChangeContext<IInformationTabApi>()
                .GetInformationTabRequest(_crmUrl, _clientId, _currentUserApiKey, false)
                .Message;

            Assert.Multiple(() =>
            {
                Assert.True(actualGetClientCardError == expectedErrorMessage,
                    $" expected Get Client Card Error: {expectedErrorMessage}" +
                    $" actual Get Client Card Error: {actualGetClientCardError}");

                Assert.True(actualdPutClientCardError == expectedErrorMessage,
                    $" expected put Client Card Error: {expectedErrorMessage}" +
                    $" actual put Client Card Error: {actualdPutClientCardError}");
            });
        }
    }
}