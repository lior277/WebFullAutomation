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
using System;
using TestsProject.TestsInternals;
using static AirSoftAutomationFramework.Objects.DTOs.GetInformationTabResponse;

namespace TestsProject.Tests.RolePage.Permissions.GlobalPermissions
{
    [TestFixture]
    public class VerifyClientProfileChildPermissionsApi : TestSuitBase
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
            roleData.ErpPermissions.Add("all_client_profile");
            roleData.ErpPermissions.Remove("all_phone");
            roleData.ErpPermissions.Remove("all_email");
            roleData.ErpPermissions.Remove("all_sales_agent");
            roleData.ErpPermissions.Remove("all_feed_tab");
            roleData.ErpPermissions.Remove("sales_agent_profile");

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
        public void VerifyClientProfileChildPermissionsApiTest()
        {
            var expectedErrorMessage = "Method Not Allowed";

            _apiFactory
                .ChangeContext<IInformationTabApi>()
                .PutInformationTabRequest(_crmUrl,
                _informationTabData, apiKey: _currentUserApiKey);

            var actualdPhone = _apiFactory
                .ChangeContext<IInformationTabApi>()
                .GetInformationTabRequest(_crmUrl, _clientId)
                .GeneralResponse
                .informationTab
                .phone;

            var actualdEmail = _apiFactory
                .ChangeContext<IInformationTabApi>()
                .GetInformationTabRequest(_crmUrl, _clientId)
                .GeneralResponse
                .informationTab
                .email;

            var actualdSalesAgent = _apiFactory
                .ChangeContext<IInformationTabApi>()
                .GetInformationTabRequest(_crmUrl, _clientId)
                .GeneralResponse
                .informationTab
                .sales_agent;

            var actualdBannerError = _apiFactory
               .ChangeContext<IBannerTabApi>()
               .PutBannerRequest(_crmUrl, _clientId, _bannerId, _currentUserApiKey, false);

            Assert.Multiple(() =>
            {
                Assert.True(actualdPhone == _expectedPhone,
                    $" expected Client Phone: {_expectedPhone}" +
                    $" actual Client Phone: {actualdPhone}");

                Assert.True(actualdEmail == _expectedEmail,
                    $" expected Client Email: {_expectedEmail}" +
                    $" actual Client Email: {actualdEmail}");

                Assert.True(actualdSalesAgent == _expectedSalesAgent,
                    $" expected Client Sales Agent: {_expectedSalesAgent}" +
                    $" actual Client Sales Agent: {actualdSalesAgent}");

                Assert.True(actualdBannerError == expectedErrorMessage,
                    $" expected put Banner Error Message: {expectedErrorMessage}" +
                    $" actual put Banner Error Message: {actualdBannerError}");
            });
        }
    }
}