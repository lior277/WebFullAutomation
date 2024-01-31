// Ignore Spelling: Api

using System.Collections.Generic;
using System.Linq;
using System.Threading;
using AirSoftAutomationFramework.Internals.DAL.ConfigurationManagement;
using AirSoftAutomationFramework.Internals.ExtensionsMethods;
using AirSoftAutomationFramework.Internals.Factorys;
using AirSoftAutomationFramework.Internals.Helpers;
using AirSoftAutomationFramework.Internals.NunitAtributes;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.AccountsApi.UsersMenuApi.UserMenuApi;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.CampaignsApi.Dashboard;
using AirSoftAutomationFramework.Objects.ForexObjects.ApiGeneral;
using NUnit.Framework;
using TestsProject.TestsInternals;

namespace TestsProject.Tests.GlobalEventsAndExport.GlobalEvents.Banner
{
    [TestFixture]
    public class VerifyGlobalEventForCreateEditDeleteCampaignApi : TestSuitBase
    {
        #region Test Preparation
        private IApplicationFactory _apiFactory = new ApplicationFactory();
        private string _crmUrl = Config.appSettings.CrmUrl;
        private string _currentUserApiKey;
        private string _userId;
        private string _userName;
        private string _affiliateId;
        private string _exceptedCampaignId;
        private string _exceptedCampaignName;

        [SetUp]
        public void SetUp()
        {
            BeforeTest();

            //###
            // Create the campaign and affiliate with super admin
            // so the campaign and affiliate will be in the db before creating the api key
            // then create another user and set the affiliate as sub user
            // and create api key
            //###

            // create campaign
            var campaignData = _apiFactory
                .ChangeContext<ISharedStepsGenerator>()
                .CreateAffiliateAndCampaignApi(_crmUrl);

            _exceptedCampaignId = campaignData.Values.First();
            _exceptedCampaignName = campaignData.Keys.First();
            _affiliateId = campaignData.Values.Last();

            // get campaign
            var getCampaignData = _apiFactory
                .ChangeContext<ICampaignsPageApi>()
                .GetCampaignByIdRequest(_crmUrl, _exceptedCampaignId)
                .GeneralResponse;

            // create user
            _userName = TextManipulation.RandomString();

            // create user
            _userId = _apiFactory
                .ChangeContext<IUserApi>()
                .PostCreateUserRequest(_crmUrl, _userName,
                role: DataRep.AdminWithUsersOnlyRoleName,
                subUsers: new string[] { _affiliateId });

            // create ApiKey
            _currentUserApiKey = _apiFactory
                .ChangeContext<IUserApi>()
                .PostCreateApiKeyRequest(_crmUrl, _userId);

            // create campaign
             _apiFactory
                .ChangeContext<ISharedStepsGenerator>()
                .CreateAffiliateAndCampaignApi(_crmUrl, apiKey: _currentUserApiKey);

            // update campaign 
            _apiFactory
                .ChangeContext<ICampaignPageApi>()
                .PutCampaignByIdRequest(_crmUrl, getCampaignData, _affiliateId,
                 apiKey: _currentUserApiKey);

            _apiFactory
                .ChangeContext<ICampaignPageApi>()
                .DeleteCampaignRequest(_crmUrl, _exceptedCampaignId,
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
        [RetryMenage(DataRep.NumOfRetraiesForFallingTest)]
        public void VerifyGlobalEventForCreateEditDeleteCampaignApiTest()
        {
            var expectedTypeList = new List<string>()
            { { "create_campaign" }, { "edit_campaign" },
                { "delete_campaign" }, { "create_erp_user" } };

            var expectedGlobal = true;
            var expectedActionMadeByUser = _userName;
            var expectedActionMadeByUserId = _userId;
            var actualTypeList = new List<string>();
            var actualGlobalList = new List<bool>();
            var actualMadeByList = new List<string>();
            var actualActionMadeByUserId = new List<string>();
            var actualCampainIdsList = new List<string>();

            // get global events
            var actualGlobals = _apiFactory
               .ChangeContext<IGlobalEventsApi>()
               .GetGlobalEventsByUserRequest(_crmUrl, _userName, _currentUserApiKey);

            actualGlobals.ForEach(p => actualMadeByList.Add(p.action_made_by));
            actualGlobals.ForEach(p => actualActionMadeByUserId.Add(p.action_made_by_user_id));
            actualGlobals.ForEach(p => actualTypeList.Add(p.type));
            actualGlobals.ForEach(p => actualGlobalList.Add(p.global));
            actualGlobals.ForEach(p => actualCampainIdsList.Add(p.campaign_id));

            Assert.Multiple(() =>
            {
                Assert.True(actualCampainIdsList.Contains(_exceptedCampaignId),
                    $" actual account type id : {actualCampainIdsList.ListToString()}" +
                    $" expected account type id: {_exceptedCampaignId}");

                Assert.True(actualTypeList.CompareTwoListOfString(expectedTypeList).Count == 0,
                    $" actual Type List : {actualTypeList.ListToString()}" +
                    $" expected type List: {expectedTypeList.ListToString()}");

                Assert.True(actualGlobalList.All(p => p),
                    $" actual Global list : {actualGlobalList.ListToString()}" +
                    $" expected Global list: {expectedGlobal}");

                Assert.True(actualMadeByList.All(p => p == _userName),
                    $" actual user name : {actualMadeByList.ListToString()}" +
                    $" expected user name: {_userName}");
            });
        }
    }
}