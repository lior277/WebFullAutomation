// Ignore Spelling: Api

using AirSoftAutomationFramework.Internals.DAL.ConfigurationManagement;
using AirSoftAutomationFramework.Internals.Helpers;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.AccountsApi.Roles;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.AccountsApi.UsersMenuApi.UserMenuApi;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.Clients.ClientCard;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.Clients.ClientPage;
using NUnit.Framework;
using System;
using AirSoftAutomationFramework.Internals.Factorys;
using AirSoftAutomationFramework.Internals.NunitAtributes;
using TestsProject.TestsInternals;
using static AirSoftAutomationFramework.Objects.DTOs.GetInformationTabResponse;

namespace TestsProject.Tests.RolePage.Permissions.GlobalPermissions
{
    [TestFixture]
    public class VerifyEmailAndPhonePermissionsApi : TestSuitBase
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
            //roleData.ErpPermissions.Remove("all_client_profile");
            roleData.ErpPermissions.Remove("all_email");
            roleData.ErpPermissions.Remove("all_phone");
            //roleData.ErpPermissions.Remove("all_feed_tab");
            //roleData.ErpPermissions.Remove("all_sales_agent");

            // create  role 
            _apiFactory
                .ChangeContext<IRolesApi>()
                .PostCreateRoleRequest(_crmUrl, roleData);

            // create user
            // create user
            var userName = TextManipulation.RandomString();

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

            _informationTabData = _apiFactory
                .ChangeContext<IInformationTabApi>()
                .GetInformationTabRequest(_crmUrl, _clientId)
                .GeneralResponse
                .informationTab;

            var newclientEmail = $"new{clientEmail}";
            var newClientPhone = $"123{_informationTabData.phone}";

            _informationTabData.saving_account_id = "null";
            _informationTabData.sales_agent = "null";
            _expectedEmail = _informationTabData.email;
            _expectedPhone = _informationTabData.phone;
            _informationTabData.email = newclientEmail;
            _informationTabData.phone = newClientPhone;

            //_apiFactory
            //   .ChangeContext<IPlatformTabApi>()
            //    .CreateBannerPipe(_crmUrl, DataRep.AutomationBannerName);          

            //// get banner
            //_bannerId = _apiFactory
            //    .ChangeContext<IPlatformTabApi>()
            //    .GetBannersRequest(_crmUrl)
            //    .Where(p => p.Name == DataRep.AutomationBannerName)
            //    .FirstOrDefault()?
            //    .Id;
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
        public void VerifyEmailAndPhonePermissionApiTest()
        {
            _apiFactory
                .ChangeContext<IInformationTabApi>()
                .PutInformationTabRequest(_crmUrl, _informationTabData, _currentUserApiKey, false);

            var actualEmail = _apiFactory
                .ChangeContext<IInformationTabApi>()
                .GetInformationTabRequest(_crmUrl, _clientId)
                .GeneralResponse
                .informationTab
                .email;

            var actualPhone = _apiFactory
                .ChangeContext<IInformationTabApi>()
                .GetInformationTabRequest(_crmUrl, _clientId)
                .GeneralResponse
                .informationTab
                .phone;

            Assert.Multiple(() =>
            {
                Assert.True(actualEmail == _expectedEmail,
                    $" expected Email: {_expectedEmail}" +
                    $" actual Email: {actualEmail}");

                Assert.True(actualPhone == _expectedPhone,
                    $" expected Phone: {_expectedPhone}" +
                    $" actual Phone: {actualPhone}");
            });
        }
    }
}