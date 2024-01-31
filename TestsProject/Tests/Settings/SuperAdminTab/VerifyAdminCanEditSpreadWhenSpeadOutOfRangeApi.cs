﻿// Ignore Spelling: Admin Api

using System;
using System.Collections.Generic;
using System.Linq;
using AirSoftAutomationFramework.Internals.DAL.ConfigurationManagement;
using AirSoftAutomationFramework.Internals.Factorys;
using AirSoftAutomationFramework.Internals.Helpers;
using AirSoftAutomationFramework.Internals.NunitAtributes;
using AirSoftAutomationFramework.Objects.DTOs;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.AccountsApi.UsersMenuApi.UserMenuApi;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.Settings;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Trade;
using NUnit.Framework;
using TestsProject.TestsInternals;

namespace TestsProject.Tests.Settings.SuperAdminTab
{
    [TestFixture]    
    public class VerifyAdminCanEditSpreadWhenSpreadOutOfRangeApi : TestSuitBase
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
        private string _cryptoGroupId;
        private Apple _cryptoGroupAppleAttr;
        private string _cryptoGroupName;

        [SetUp]
        public void SetUp()
        {
            BeforeTest();
            #region PreCondition 
            _cryptoGroupName = TextManipulation.RandomString();

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

            _apiFactory
                .ChangeContext<ISuperAdminTubApi>()
                .PutRiskRestrictionsRequest(_crmUrl)
                .PutGroupRestrictionsRequest(_crmUrl);

            // create group with random title
            _cryptoGroupId = _apiFactory
                .ChangeContext<ITradeGroupApi>()
                .PostCreateTradeGroupRequest(_crmUrl,
                new List<object> 
                {  _cryptoGroupDefaultAttr, _cryptoGroupIndiciesAttr, _asset },
               _cryptoGroupName, apiKey: _currentUserApiKey);
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
                    .DeleteTradeGroupRequest(_crmUrl, _cryptoGroupId);
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
        public void VerifyAdminCanEditSpreadWhenSpreadOutOfRangeApiTest()
        {
            var cryptoGroup = _apiFactory
                .ChangeContext<ITradeGroupApi>()
                .GetTradeGroupsRequest(_crmUrl)
                .GeneralResponse
                .Where(p => p._id == _cryptoGroupId)
                .FirstOrDefault();

            // set GroupRestrictions To Default Values
            _apiFactory
              .ChangeContext<ISuperAdminTubApi>()
              .PutGroupRestrictionsRequest(_crmUrl, _groupRestrictionsDefaultAttr);

            // Edit group general with out of limit spread spread
            var actualEditGroupGeneralStreadMessage = _apiFactory
                .ChangeContext<ITradeGroupApi>()
                .PutEditTradeGroupRequest(_crmUrl,
                _cryptoGroupId, cryptoGroup, apiKey: _currentUserApiKey);

            // Edit group apple asset with out of limit spread spread
            var actualEditGroupAppleAssetSpreadMessage = _apiFactory
                .ChangeContext<ITradeGroupApi>()
                .PutEditTradeGroupRequest(_crmUrl,
                _cryptoGroupId, cryptoGroup, apiKey: _currentUserApiKey);

            // set Group Restrictions To Indices
            _apiFactory
              .ChangeContext<ISuperAdminTubApi>()
              .PutGroupRestrictionsRequest(_crmUrl)
              .PutGroupRestrictionsRequest(_crmUrl, _groupRestrictionsIndices);

            // Edit group indicies with out of limit spread
            var actualEditGroupIndiciesSpreadMessage = _apiFactory
                 .ChangeContext<ITradeGroupApi>()
                 .PutEditTradeGroupRequest(_crmUrl,
                _cryptoGroupId, cryptoGroup, apiKey: _currentUserApiKey);
        }
    }
}