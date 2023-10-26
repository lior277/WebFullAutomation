// Ignore Spelling: Admin Api

using System;
using System.Collections.Generic;
using System.Linq;
using AirSoftAutomationFramework.Internals.DAL.ConfigurationManagement;
using AirSoftAutomationFramework.Internals.Factory;
using AirSoftAutomationFramework.Internals.Helpers;
using AirSoftAutomationFramework.Internals.NunitAttributes;
using AirSoftAutomationFramework.Objects.DTOs;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.AccountsApi.UsersMenuApi.UserMenuApi;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.Settings;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Trade;
using NUnit.Framework;
using TestsProject.TestsInternals;

namespace TestsProject.Tests.Settings.SuperAdminTab
{
    [NonParallelizable]
    [TestFixture]    
    public class VerifyCreateGroupWhenChangeSpreadDisableApi : TestSuitBase
    {
        #region Test Preparation
        private IApplicationFactory _apiFactory = new ApplicationFactory();
        private string _crmUrl = Config.appSettings.CrmUrl;
        private Default_Attr _cryptoGroupDefaultAttr;
        private Indices _cryptoGroupIndiciesAttr;
        private string _currentUserApiKey;
        private Assets _asset;
        private Apple _cryptoGroupAppleAttr;
        private List<string> _tradeGroupsIdsListForDelete = new List<string>();

        [SetUp]
        public void SetUp()
        {
            BeforeTest();
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
              .PutRiskRestrictionsRequest(_crmUrl, allowSpreadChange: false)
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
                    .PutGroupRestrictionsRequest(_crmUrl)
                    .ChangeContext<ITradeGroupApi>()
                    .DeleteTradeGroupRequest(_crmUrl, _tradeGroupsIdsListForDelete);
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
        public void VerifyCreateGroupWhenChangeSpreadDisableApiTest()
        {
            double? expectedSpread = null;
            var expectedGeneralSpread = 0;
            var cryptoGroupName = TextManipulation.RandomString();

            // create group general with out of limit spread 
            var groupId = _apiFactory
                .ChangeContext<ITradeGroupApi>()
                .PostCreateTradeGroupRequest(_crmUrl,
                new List<object> { _cryptoGroupDefaultAttr },
                cryptoGroupName, apiKey: _currentUserApiKey);

            var actualGeneralSpread = _apiFactory
                .ChangeContext<ITradeGroupApi>()
                .GetTradeGroupsRequest(_crmUrl)
                .GeneralResponse
                .Where(p => p._id == groupId)
                .FirstOrDefault()
                .default_attr
                .spread;

            _tradeGroupsIdsListForDelete.Add(groupId);
            cryptoGroupName = TextManipulation.RandomString();

            // create group apple asset with out of limit spread
            groupId = _apiFactory
                .ChangeContext<ITradeGroupApi>()
                .PostCreateTradeGroupRequest(_crmUrl, new List<object>
                { _cryptoGroupDefaultAttr, _asset },
                cryptoGroupName, apiKey: _currentUserApiKey);

            var actualAppleSpread = _apiFactory
               .ChangeContext<ITradeGroupApi>()
               .GetTradeGroupsRequest(_crmUrl)
               .GeneralResponse
               .Where(p => p._id == groupId)
               .FirstOrDefault()
               .assets
               .APPLE
               .spread;

            _tradeGroupsIdsListForDelete.Add(groupId);
            cryptoGroupName = TextManipulation.RandomString();

            // create group indicies with out of limit spread
            groupId = _apiFactory
                 .ChangeContext<ITradeGroupApi>()
                 .PostCreateTradeGroupRequest(_crmUrl, new List<object>
                { _cryptoGroupDefaultAttr, _cryptoGroupIndiciesAttr },
                cryptoGroupName, apiKey: _currentUserApiKey);

            _tradeGroupsIdsListForDelete.Add(groupId);

            var actualIndicesSpread = _apiFactory
                .ChangeContext<ITradeGroupApi>()
                .GetTradeGroupsRequest(_crmUrl)
                .GeneralResponse
                .Where(p => p._id == groupId)
                .FirstOrDefault()
                .indices?
                .spread;

            Assert.Multiple(() =>
            {
                Assert.True(actualGeneralSpread == expectedGeneralSpread,
                     $" expected General Spread : {expectedGeneralSpread}" +
                     $" actual General Spread : {actualGeneralSpread}");

                Assert.True(actualAppleSpread.Equals(expectedSpread),
                     $" expected Apple Spread : {expectedSpread}" +
                     $" actual Apple Spread : {actualAppleSpread}");

                Assert.True(actualIndicesSpread.Equals(expectedSpread),
                     $" expected Indices Spread : {expectedSpread}" +
                     $" actual Indices Spread : {actualIndicesSpread}");
            });
        }
    }
}