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
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.Clients.ClientPage;
using NUnit.Framework;
using TestsProject.TestsInternals;

namespace TestsProject.Tests.RolePage.Permissions.GlobalPermissions
{
    [TestFixture]
    public class VerifyDeleteCommentRoleApi : TestSuitBase
    {
        #region Test Preparation
        private IApplicationFactory _apiFactory =new ApplicationFactory();
        private string _crmUrl =Config.appSettings.CrmUrl;
        private string _clientId;
        private string _userId;
        private string _currentUserApiKey;
        private string _roleName;

        [SetUp]
        public void SetUp()
        {
            BeforeTest();
            _roleName = TextManipulation.RandomString();
            var expectedComment = "Comment";

            // get role by name
            var roleData = _apiFactory
                .ChangeContext<IRolesApi>()
                .GetRoleByNameRequest(_crmUrl, DataRep.AdminRole);

            roleData.Name = _roleName;
            roleData.ErpPermissions.Remove("show_deleted_comments");

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

            // create client 
            var clientName = TextManipulation.RandomString();
            var clientEmail = clientName + DataRep.EmailPrefix;

            _clientId = _apiFactory
                .ChangeContext<ICreateClientApi>()
                .CreateClientRequest(_crmUrl, clientName,
                apiKey: _currentUserApiKey);

            _apiFactory
                .ChangeContext<ICommentsTabApi>()
                .PostCommentRequest(_crmUrl, _clientId, expectedComment, _currentUserApiKey);

            // get Comment by client id
            var comment = _apiFactory
                .ChangeContext<ICommentsTabApi>()
                .GetCommentsByClientIdRequest(_crmUrl, _clientId);

            var commentId = comment.GeneralResponse.FirstOrDefault().Id;

            _apiFactory
                .ChangeContext<ICommentsTabApi>()
                .DeleteCommentRequest(_crmUrl, commentId, _currentUserApiKey);
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

        // 
        [Test]
        [Description("based on jira: https://airsoftltd.atlassian.net/browse/AIRV2-4745")]
        [Category(DataRep.SanityCategory)]
        [Category(DataRep.RegressionCategory)]
        [RetryMenage(DataRep.NumOfRetraiesForFallingTest)]
        public void VerifyDeleteCommentRoleApiTest()
        {
            // Get Deleted Comments By Client Id for no permission
            var actualDeletedCommentsNoPermission = _apiFactory
                .ChangeContext<ICommentsTabApi>()
                .GetDeletedCommentsByClientIdRequest(_crmUrl, _clientId, _currentUserApiKey);

            // get role by name
            var roleData = _apiFactory
                .ChangeContext<IRolesApi>()
                .GetRoleByNameRequest(_crmUrl, _roleName);

            var erpPermmisions = roleData.ErpPermissions;
            erpPermmisions.Add("show_deleted_comments");
            roleData.ErpPermissions = erpPermmisions;

            // edit role and unable show_deleted_comments
            _apiFactory
                .ChangeContext<IRolesApi>()
                .PutEditRoleRequest(_crmUrl, roleData);

            // Get Deleted Comments By Client Id with permission
            var actualDeletedCommentsWithPermission = _apiFactory
                .ChangeContext<ICommentsTabApi>()
                .GetDeletedCommentsByClientIdRequest(_crmUrl, _clientId, _currentUserApiKey);

            Assert.Multiple(() =>
            {
                Assert.True(actualDeletedCommentsNoPermission.Count == 0,
                    $" expected Deleted Comments No Permission = 0" +
                    $" actualDeleted Comments No Permission: {actualDeletedCommentsNoPermission.Count}");

                Assert.True(actualDeletedCommentsWithPermission.Count == 1,
                    $" expected Deleted Comments With Permission = 1" +
                    $" actualDeleted Comments With Permission: {actualDeletedCommentsWithPermission.Count}");
            });
        }
    }
}