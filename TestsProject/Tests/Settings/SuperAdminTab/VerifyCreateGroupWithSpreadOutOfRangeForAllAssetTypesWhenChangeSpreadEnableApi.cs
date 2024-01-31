// Ignore Spelling: Api Admin

using System;
using System.Collections.Generic;
using AirSoftAutomationFramework.Internals.DAL.ConfigurationManagement;
using AirSoftAutomationFramework.Internals.Factorys;
using AirSoftAutomationFramework.Internals.Helpers;
using AirSoftAutomationFramework.Internals.NunitAtributes;
using AirSoftAutomationFramework.Objects.DTOs;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.AccountsApi.Roles;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.AccountsApi.UsersMenuApi.UserMenuApi;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.Settings;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Trade;
using NUnit.Framework;
using TestsProject.TestsInternals;

namespace TestsProject.Tests.Settings.SuperAdminTab
{
    [NonParallelizable]
    [TestFixture]    
    public class VerifyCreateGroupWithSpreadOutOfRangeForAllAssetTypesWhenChangeSpreadEnableApi : TestSuitBase
    {
        #region Test Preparation
        private IApplicationFactory _apiFactory = new ApplicationFactory();
        private string _crmUrl = Config.appSettings.CrmUrl;
        private Default_Attr _cryptoGroupDefaultAttr;
        private Indices _cryptoGroupIndiciesAttr;
        private string _currentUserApiKey;
        private GroupRestrictionsDefaultAttr _groupRestrictionsDefaultAttr;
        private GroupRestrictionsIndices _groupRestrictionsIndices;
        private Assets _asset;
        private Apple _cryptoGroupAppleAttr;
        private string _cryptoGroupName;
        private List<string> _tradeGroupsIdsListForDelete = new List<string>();

        [SetUp]
        public void SetUp()
        {
            BeforeTest();
            _cryptoGroupName = TextManipulation.RandomString();

            #region PreCondition 
            // create user
            var userName = TextManipulation.RandomString();

            var userId = _apiFactory
             .ChangeContext<IUserApi>()
             .PostCreateUserRequest(_crmUrl, userName,
             role: DataRep.AdminWithUsersOnlyRoleName);

            // create ApiKey
            _currentUserApiKey = _apiFactory
                .ChangeContext<IUserApi>()
                .PostCreateApiKeyRequest(_crmUrl, userId);

            _groupRestrictionsDefaultAttr = new GroupRestrictionsDefaultAttr
            {
                plus = 0,
                minus = 0
            };

            _groupRestrictionsIndices = new GroupRestrictionsIndices
            {
                plus = 0,
                minus = 0
            };

            _cryptoGroupDefaultAttr = new Default_Attr
            {
                commision = 0,
                leverage = 1,
                maintenance = 1,
                minimum_amount = 1,
                minimum_step = 1,
                spread = 1,
                margin_call = null,
                swap_long = 0.04,
                swap_short = 0.04,
                swap_time = "00:00:00",
            };

            _cryptoGroupIndiciesAttr = new Indices
            {
                commision = 0,
                leverage = 1,
                maintenance = 1,
                minimum_amount = 1,
                minimum_step = 1,
                spread = -1,
                margin_call = null,
                swap_long = 0.04,
                swap_short = 0.04,
                swap_time = "00:00:00",
            };
           
            _cryptoGroupAppleAttr = new Apple
            {
                commision = 0,
                leverage = 1,
                maintenance = 1,
                minimum_amount = 1,
                minimum_step = 1,
                spread = -1,
                swap_long = 0.04,
                swap_short = 0.04,
                swap_time = "00:00:00",
            };

            _asset = new Assets
            {
                APPLE = _cryptoGroupAppleAttr
            };

            // set GroupRestrictions To Default Values
            _apiFactory
              .ChangeContext<ISuperAdminTubApi>()
              .PutRiskRestrictionsRequest(_crmUrl, allowSpreadChange: true)
              .PutGroupRestrictionsRequest(_crmUrl);
            #endregion
        }
        #endregion

        [TearDown]
        public void TearDown()
        {
            try
            {

                _apiFactory
                    .ChangeContext<ISuperAdminTubApi>()
                    .PutRiskRestrictionsRequest(_crmUrl)
                    .PutGroupRestrictionsRequest(_crmUrl);
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

        /// <summary>
        /// create user Admin With Users Only 
        /// set Risk Restrictions to default values
        /// set Risk Restrictions to change spread enable  
        /// set group Restrictions to 0 0
        /// verify error message for create new group with out of range spread
        /// verify error message for edit  group spread with out of range spread
        /// </summary>        
        [Test]
        [Description("based on jira https://airsoftltd.atlassian.net/browse/AIRV2-4642")]
        [Category(DataRep.SanityCategory)]
        [Category(DataRep.RegressionCategory)]
        [RetryMenage(DataRep.NumOfRetraiesForFallingTest)]
        public void VerifyCreateGroupWithSpreadOutOfRangeForAllAssetTypesWhenChangeSpreadEnableApiTest()
        {
            var expectedCreateGroupGeneralStreadMessage = "default_attr spread is out of range,(0% - 0%)";
            var expectedCreateGroupAppleAssetSpreadMessage = "APPLE spread is out of range,(0% - 0%)";
            var expectedCreateGroupIndiciesSpreadMessage = "indices spread is out of range,(0% - 0%)";

            // set default_attr to 0 in mongo table
            _apiFactory
                .ChangeContext<ISuperAdminTubApi>()
                .PutGroupRestrictionsRequest(_crmUrl, _groupRestrictionsDefaultAttr);

            // create group general with out of limit spread spread
            var actualCreateGroupGeneralStreadMessage = _apiFactory
                .ChangeContext<ITradeGroupApi>()
                .PostCreateTradeGroupRequest(_crmUrl,
                new List<object> { _cryptoGroupDefaultAttr },
                _cryptoGroupName, apiKey: _currentUserApiKey, false);

            // create group apple asset with out of limit spread spread
            var actualCreateGroupAppleAssetSpreadMessage = _apiFactory
                .ChangeContext<ITradeGroupApi>()
                .PostCreateTradeGroupRequest(_crmUrl,
                new List<object> { _asset },
                _cryptoGroupName, apiKey: _currentUserApiKey, false);

            // set GroupRestrictions Table values
            _apiFactory
                .ChangeContext<ISuperAdminTubApi>()      
                .PutGroupRestrictionsRequest(_crmUrl)
                .PutGroupRestrictionsRequest(_crmUrl, _groupRestrictionsIndices);

            // create group indicies with out of limit spread
            var actualCreateGroupIndiciesSpreadMessage = _apiFactory
                 .ChangeContext<ITradeGroupApi>()
                 .PostCreateTradeGroupRequest(_crmUrl,
                 new List<object> { _cryptoGroupIndiciesAttr },
                _cryptoGroupName, apiKey: _currentUserApiKey, false);


            Assert.Multiple(() =>
            {
                Assert.True(actualCreateGroupGeneralStreadMessage ==
                    (expectedCreateGroupGeneralStreadMessage),
                    $" expected Create Group General Spread Message :" +
                    $" {expectedCreateGroupGeneralStreadMessage}" +
                    $" actual Create Group General Spread Message :" +
                    $" {actualCreateGroupGeneralStreadMessage}");

                Assert.True(actualCreateGroupAppleAssetSpreadMessage ==
                    (expectedCreateGroupAppleAssetSpreadMessage),
                    $" expected Create Group Cash Spread Message :" +
                    $" {expectedCreateGroupAppleAssetSpreadMessage}" +
                    $" actual Create Group Cash Spread Message :" +
                    $" {actualCreateGroupAppleAssetSpreadMessage}");

                Assert.True(actualCreateGroupIndiciesSpreadMessage ==
                    (expectedCreateGroupIndiciesSpreadMessage),
                    $" expected Create Group Asset Spread Message :" +
                    $" {expectedCreateGroupIndiciesSpreadMessage}" +
                    $" actual Create Group Asset Spread Message :" +
                    $" {actualCreateGroupIndiciesSpreadMessage}");
            });
        }
    }
}