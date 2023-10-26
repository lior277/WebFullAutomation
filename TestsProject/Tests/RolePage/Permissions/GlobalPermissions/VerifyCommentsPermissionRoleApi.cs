// Ignore Spelling: Api

using System;
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
using NUnit.Framework;
using TestsProject.TestsInternals;

namespace TestsProject.Tests.RolePage.Permissions.GlobalPermissions
{
    [TestFixture]
    public class VerifyCommentsPermissionRoleApi : TestSuitBase
    {
        #region Test Preparation
        private IApplicationFactory _apiFactory = new ApplicationFactory();
        private string _crmUrl = Config.appSettings.CrmUrl;
        private string _clientId;
        private string _userId;
        private string _currentUserApiKey;
        private string _commentForFirstDeleteId;
        private string _commentForSecondtDeleteId;
        private GetRoleByNameResponse _rolrData;
        private string _roleName;

        [SetUp]
        public void SetUp()
        {
            BeforeTest();
            _roleName = TextManipulation.RandomString();
            var expectedComment = "Comment";

            // get role by name
            _rolrData = _apiFactory
                .ChangeContext<IRolesApi>()
                .GetRoleByNameRequest(_crmUrl, DataRep.AdminRole);

            _rolrData.Name = _roleName;
            _rolrData.ErpPermissions.Add("all_comments");
            _rolrData.ErpPermissions.Remove("delete_comments");

            // create  role with all_comments enable and delete_comments disable  
            _apiFactory
                .ChangeContext<IRolesApi>()
                .PostCreateRoleRequest(_crmUrl, _rolrData);

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

            _clientId = _apiFactory
                .ChangeContext<ICreateClientApi>()
                .CreateClientRequest(_crmUrl, clientName,
                apiKey: _currentUserApiKey);

            // create first comment
            _apiFactory
                .ChangeContext<ICommentsTabApi>()
                .PostCommentRequest(_crmUrl, _clientId, expectedComment, _currentUserApiKey);

            // get  first Comment by client id
            _commentForFirstDeleteId = _apiFactory
                .ChangeContext<ICommentsTabApi>()
                .GetCommentsByClientIdRequest(_crmUrl, _clientId)
                .GeneralResponse
                .FirstOrDefault()
                .Id;

            // create second comment
            _apiFactory
                .ChangeContext<ICommentsTabApi>()
                .PostCommentRequest(_crmUrl, _clientId, expectedComment, _currentUserApiKey);

            // get second Comment id by client id
            _commentForSecondtDeleteId = _apiFactory
                .ChangeContext<ICommentsTabApi>()
                .GetCommentsByClientIdRequest(_crmUrl, _clientId)
                .GeneralResponse
                .LastOrDefault()
                .Id;

            // get role by name
            _rolrData = _apiFactory
                .ChangeContext<IRolesApi>()
                .GetRoleByNameRequest(_crmUrl, _roleName);
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
        public void VerifyCommentsPermissionRoleApiTest()
        {
            var expectedNumOfComments = 1;
            var expectedDeleteCommentErrorMessage = "Method Not Allowed";

            // delete comments permission disable 405
            var actualDeleteCommentErrorMessage = _apiFactory
                .ChangeContext<ICommentsTabApi>()
                .DeleteCommentRequest(_crmUrl, _commentForFirstDeleteId,
                _currentUserApiKey, false);

            // get role by name
            _rolrData = _apiFactory
                .ChangeContext<IRolesApi>()
                .GetRoleByNameRequest(_crmUrl, _roleName);

            var erpPermmisions = _rolrData.ErpPermissions;
            erpPermmisions.Remove("edit_comments");
            _rolrData.ErpPermissions = erpPermmisions;

            // edit role and enable edit comments
            _apiFactory
                .ChangeContext<IRolesApi>()
                .PutEditRoleRequest(_crmUrl, _rolrData);

            // edit comments permission disable 405
            var actualEditCommentErrorMessage = _apiFactory
                .ChangeContext<ICommentsTabApi>()
                .PatchEditCommentRequest(_crmUrl, _commentForFirstDeleteId, "edit",
                _currentUserApiKey, false);

            erpPermmisions = _rolrData.ErpPermissions;
            erpPermmisions.Remove("all_comments");
            _rolrData.ErpPermissions = erpPermmisions;

            // edit role and enable delete_comments
            _apiFactory
                .ChangeContext<IRolesApi>()
                .PutEditRoleRequest(_crmUrl, _rolrData);

            // view Comments permission enable delete comments permission disable 405
            var actualDeleteCommentWithViewErrorMessage = _apiFactory
                .ChangeContext<ICommentsTabApi>()
                .DeleteCommentRequest(_crmUrl, _commentForFirstDeleteId,
                _currentUserApiKey, false);

            erpPermmisions = _rolrData.ErpPermissions;
            erpPermmisions.Add("delete_comments");
            _rolrData.ErpPermissions = erpPermmisions;

            // edit role and unable delete_comments
            _apiFactory
                .ChangeContext<IRolesApi>()
                .PutEditRoleRequest(_crmUrl, _rolrData);

            // Delete Comment enable with all_comments permission in view
            _apiFactory
                .ChangeContext<ICommentsTabApi>()
                .DeleteCommentRequest(_crmUrl, _commentForSecondtDeleteId, _currentUserApiKey);

            // get second Comment by client id
            var actualNumOfCommentsAfterDelete = _apiFactory
                .ChangeContext<ICommentsTabApi>()
                .GetCommentsByClientIdRequest(_crmUrl, _clientId);

            Assert.Multiple(() =>
            {
                Assert.True(actualDeleteCommentErrorMessage == expectedDeleteCommentErrorMessage,
                    $" expected Delete Comment Error Message: {expectedDeleteCommentErrorMessage}" +
                    $" actual Delete Comment Error Message: {actualDeleteCommentErrorMessage}");

               Assert.True(actualEditCommentErrorMessage == expectedDeleteCommentErrorMessage,
                    $" expected Edit Comment Error Message: {expectedDeleteCommentErrorMessage}" + 
                    $" actual Edit Comment Error Message: {actualEditCommentErrorMessage}");

               Assert.True(actualDeleteCommentWithViewErrorMessage == expectedDeleteCommentErrorMessage,
                    $" expected Delete Comment With View Error Message: {expectedDeleteCommentErrorMessage}" +
                    $" actual Delete Comment With View Error Message: {actualDeleteCommentWithViewErrorMessage}");

               Assert.True(actualNumOfCommentsAfterDelete.GeneralResponse.Count == expectedNumOfComments,
                    $" expected Num Of Comments After Delete: {expectedNumOfComments}" +
                    $" actual Num Of Comments After Delete: {actualDeleteCommentErrorMessage}");
            });
        }
    }
}