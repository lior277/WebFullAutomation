using System.Collections.Generic;
using System.Linq;
using AirSoftAutomationFramework.Internals.DAL.ConfigurationManagement;
using AirSoftAutomationFramework.Internals.ExtensionsMethods;
using AirSoftAutomationFramework.Internals.Factory;
using AirSoftAutomationFramework.Internals.Helpers;
using AirSoftAutomationFramework.Internals.NunitAttributes;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.AccountsApi.UsersMenuApi.UserMenuApi;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.Settings;
using AirSoftAutomationFramework.Objects.ForexObjects.ApiGeneral;
using NUnit.Framework;
using TestsProject.TestsInternals;

namespace TestsProject.Tests.GlobalEventsAndExport.GlobalEvents.Banner
{
    [TestFixture]
    public class VerifyGlobalEventsForCreateEditDeleteBannerApi : TestSuitBase
    {
        #region Test Preparation
        private IApplicationFactory _apiFactory = new ApplicationFactory();
        private string _crmUrl = Config.appSettings.CrmUrl;
        private string _currentUserApiKey;
        private string _userId;
        private string _userName;
        private string _bannerName;

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

            // create ApiKey
            _currentUserApiKey = _apiFactory
                .ChangeContext<IUserApi>()
                .PostCreateApiKeyRequest(_crmUrl, _userId);

            // create banner
            _bannerName = TextManipulation.RandomString();
           
            _apiFactory
                .ChangeContext<IPlatformTabApi>()
                .PostCreateBannerRequest(_crmUrl, _bannerName, apiKey: _currentUserApiKey);

            var bannerData = _apiFactory
                .ChangeContext<IPlatformTabApi>()
                .GetBannersRequest(_crmUrl)
                .Where(p => p.Name == _bannerName)
                .FirstOrDefault();

            // edit banner
            _apiFactory
                .ChangeContext<IPlatformTabApi>()
                .PutBannerInSettingsRequest(_crmUrl, bannerData, apiKey: _currentUserApiKey);

            // delete banner
            _apiFactory
                .ChangeContext<IPlatformTabApi>()
                .DeleteBannerRequest(_crmUrl, bannerData.Id, apiKey: _currentUserApiKey);
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
        [RetryMenage(DataRep.NumOfRetraiesForFallingTest)]
        public void VerifyGlobalEventsForCreateEditDeleteBannerApiTest()
        {
            var expectedTypeList = new List<string>()
            { { "create_banner" }, { "edit_banner" }, { "delete_banner" } };

            var expectedGlobal = true;
            var expectedActionMadeByUser = _userName;
            var expectedActionMadeByUserId = _userId;
            var actualMadeByList = new List<string>();
            var actualActionMadeByUserId = new List<string>();
            var actualTypeList = new List<string>();
            var actualGlobalList = new List<bool>();           

            // get global events
            var actualGlobals = _apiFactory
               .ChangeContext<IGlobalEventsApi>()
               .GetGlobalEventsByUserRequest(_crmUrl, _userName, _currentUserApiKey);

            actualGlobals.ForEach(p => actualMadeByList.Add(p.action_made_by));
            actualGlobals.ForEach(p => actualActionMadeByUserId.Add(p.action_made_by_user_id));
            actualGlobals.ForEach(p => actualTypeList.Add(p.type));
            actualGlobals.ForEach(p => actualGlobalList.Add(p.global));
                        
            Assert.Multiple(() =>
            {
                Assert.True(actualTypeList.CompareTwoListOfString(expectedTypeList).Count == 0,
                    $" actual Type List : {actualTypeList.ListToString()}" +
                    $" expected type List: {expectedTypeList.ListToString()}");

                Assert.True(actualGlobalList.All(p => p.Equals(true)),
                    $" actual Global list : {actualGlobalList.ListToString()}" +
                    $" expected Global list: {expectedGlobal}");

                Assert.True(actualActionMadeByUserId.All(p => p.Equals(expectedActionMadeByUserId)),
                    $" actual Made By user id : {actualActionMadeByUserId.ListToString()}" +
                    $" expected  Made By user id: {expectedActionMadeByUserId}");

                Assert.True(actualMadeByList.All(p => p.Equals(_userName)),
                    $" actual Made By user name : {actualMadeByList.ListToString()}" +
                    $" expected Made By user name: {_userName}");
            });
        }
    }
}