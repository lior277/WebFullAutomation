// Ignore Spelling: Api

using System.Collections.Generic;
using System.Linq;
using AirSoftAutomationFramework.Internals.DAL.ConfigurationManagement;
using AirSoftAutomationFramework.Internals.ExtensionsMethods;
using AirSoftAutomationFramework.Internals.Factorys;
using AirSoftAutomationFramework.Internals.Helpers;
using AirSoftAutomationFramework.Internals.NunitAtributes;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.AccountsApi.Roles;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.AccountsApi.UsersMenuApi.UserMenuApi;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.Settings;
using AirSoftAutomationFramework.Objects.ForexObjects.ApiGeneral;
using NUnit.Framework;
using TestsProject.TestsInternals;

namespace TestsProject.Tests.GlobalEventsAndExport.GlobalEvents
{
    [TestFixture]
    public class VerifyGlobalEventForCreateEditDeleteSellerEmailTemplateApi : TestSuitBase
    {
        #region Test Preparation
        private IApplicationFactory _apiFactory = new ApplicationFactory();
        private string _crmUrl = Config.appSettings.CrmUrl;
        private string _currentUserApiKey;
        private string _userId;
        private string _userName;
        private string _expectedEmailName;

        [SetUp]
        public void SetUp()
        {
            BeforeTest();
            // create user
            _userName = TextManipulation.RandomString();

            // create user
            _userId = _apiFactory
             .ChangeContext<IUserApi>()
             .PostCreateUserRequest(_crmUrl, _userName,
             role: DataRep.AdminWithUsersOnlyRoleName);

            #region create ApiKey
            // create ApiKey
            _currentUserApiKey = _apiFactory
                .ChangeContext<IUserApi>()
                .PostCreateApiKeyRequest(_crmUrl, _userId);
            #endregion   

            _expectedEmailName = $"Test{TextManipulation.RandomString()}";
 
            var emailsParams = new Dictionary<string, string> {
                { "type", "custom" }, { "language", "en" },
                { "subject", _expectedEmailName }, { "body",
                    _expectedEmailName }, { "name", _userName }};

            // save seller template email 
            var emailBody = _apiFactory
               .ChangeContext<IPlatformTabApi>()
               .PostSaveCustomEmailRequest(_crmUrl,
               emailsParams, _currentUserApiKey);

            // get seller template email 
            var sellerTemplateEmails = _apiFactory
               .ChangeContext<IPlatformTabApi>()
               .GetCustomEmailsRequest(_crmUrl)
               .Where(p => p.subject == _expectedEmailName)
               .FirstOrDefault();

            _apiFactory
               .ChangeContext<IPlatformTabApi>()
               .PutCustomEmailRequest(_crmUrl, sellerTemplateEmails,
               _currentUserApiKey);

            _apiFactory
              .ChangeContext<IPlatformTabApi>()
              .DeleteCustomEmailRequest(_crmUrl, sellerTemplateEmails._id,
              _currentUserApiKey);
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
        public void VerifyGlobalEventForCreateEditDeleteSellerEmailTemplateApiTest()
        {
            var expectedTypes = new List<string>()
            { { "create_seller_email_template" }, { "edit_seller_email_template" },
                { "delete_seller_email_template" } };

            var expectedGlobal = true;
            var expectedActionMadeByUser = _userName;
            var expectedActionMadeByUserId = _userId;
            var actualTypeList = new List<string>();
            var actualGlobalList = new List<bool>();
            var actualMadeByUserList = new List<string>();
            var actualEmailNameList = new List<string>();

            // get global event create saving account
            var actualTypeCreateSellerEmail = _apiFactory
               .ChangeContext<IGlobalEventsApi>()
               .GetGlobalEventsByUserRequest(_crmUrl, _userName, _currentUserApiKey)
               .Where(p => p.type == "create_seller_email_template")
               .FirstOrDefault();

            actualTypeList.Add(actualTypeCreateSellerEmail.type);
            actualGlobalList.Add(actualTypeCreateSellerEmail.global);
            actualMadeByUserList.Add(actualTypeCreateSellerEmail.action_made_by_user_id);

            // get global event edit saving account
            var actualTypeEditSellerEmail = _apiFactory
               .ChangeContext<IGlobalEventsApi>()
               .GetGlobalEventsByUserRequest(_crmUrl, _userName, _currentUserApiKey)
               .Where(p => p.type == "edit_seller_email_template")
               .FirstOrDefault();

            actualTypeList.Add(actualTypeEditSellerEmail.type);
            actualGlobalList.Add(actualTypeEditSellerEmail.global);
            actualMadeByUserList.Add(actualTypeEditSellerEmail.action_made_by_user_id);
            actualEmailNameList.Add(actualTypeEditSellerEmail.email_name);

            // get global event delete saving account
            var actualTypeDeleteAccountType = _apiFactory
               .ChangeContext<IGlobalEventsApi>()
               .GetGlobalEventsByUserRequest(_crmUrl, _userName, _currentUserApiKey)
               .Where(p => p.type == "delete_seller_email_template")
               .FirstOrDefault();

            actualTypeList.Add(actualTypeDeleteAccountType.type);
            actualGlobalList.Add(actualTypeDeleteAccountType.global);
            actualMadeByUserList.Add(actualTypeEditSellerEmail.action_made_by_user_id);
            actualEmailNameList.Add(actualTypeDeleteAccountType.email_name);

            Assert.Multiple(() =>
            {
                Assert.True(actualTypeList.CompareTwoListOfString(expectedTypes).Count == 0,
                    $" actual Type List : {actualTypeList.ListToString()}" +
                    $" expected type List: {expectedTypes.ListToString()}");

                Assert.True(actualGlobalList.All(p => p.Equals(true)),
                    $" actual Global list : {actualGlobalList.ListToString()}" +
                    $" expected Global list: {expectedGlobal}");

                Assert.True(actualMadeByUserList.All(p => p.Equals(expectedActionMadeByUserId)),
                    $" actual Made By User List : {actualMadeByUserList.ListToString()}" +
                    $" expected Made By User List: {expectedActionMadeByUserId}");

                Assert.True(actualEmailNameList.All(p => p.Equals(_userName)),
                    $" actual Email Name List : {actualEmailNameList.ListToString()}" +
                    $" expected Email Name List: {_userName}");
            });
        }
    }
}