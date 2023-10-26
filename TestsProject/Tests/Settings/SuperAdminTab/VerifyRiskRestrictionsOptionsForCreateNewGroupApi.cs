// Ignore Spelling: Admin Api

using System;
using System.Collections.Generic;
using System.Threading;
using AirSoftAutomationFramework.Internals.DAL.ConfigurationManagement;
using AirSoftAutomationFramework.Internals.Factory;
using AirSoftAutomationFramework.Internals.Helpers;
using AirSoftAutomationFramework.Internals.NunitAttributes;
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
    public class VerifyRiskRestrictionsOptionsForCreateNewGroupApi : TestSuitBase
    {
        #region Test Preparation

        private IApplicationFactory _apiFactory = new ApplicationFactory();
        private string _crmUrl = Config.appSettings.CrmUrl;     
        private Default_Attr _tradeGroup;
        private string _currentUserApiKey;
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

            _tradeGroup = new Default_Attr
            {
                commision = 0,
                leverage = 5,
                maintenance = 1,
                minimum_amount = 1,
                minimum_step = 1,
                spread = -0.01,
                margin_call = null,
                swap_long = 0.04,
                swap_short = 0.04,
                swap_time = "00:00:00",
            };
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
        /// set Risk Restrictions to 0 num of groups   
        /// verify error message for create new group
        /// </summary>        
        [Test]
        [Description("based on jira https://airsoftltd.atlassian.net/browse/AIRV2-4642")]
        [Category(DataRep.SanityCategory)]
        [Category(DataRep.RegressionCategory)]
        [RetryMenage(DataRep.NumOfRetraiesForFallingTest)]
        public void VerifyRiskRestrictionsOptionsForCreateNewGroupApiTest()
        {
            var expectedErrorMessageForCreateGroup = "maximum groups has reached";

            // update Risk Restrictions
            _apiFactory
                .ChangeContext<ISuperAdminTubApi>()            
                .PutRiskRestrictionsRequest(_crmUrl, 0, true, true);

            Thread.Sleep(100); // wait for Put Risk Restrictions

            var actualErrorMessageForCreateGroup = _apiFactory
                .ChangeContext<ITradeGroupApi>()
                .PostCreateTradeGroupRequest(_crmUrl, new List<object> { _tradeGroup },
                _cryptoGroupName, apiKey: _currentUserApiKey, false);

            Assert.True(actualErrorMessageForCreateGroup == expectedErrorMessageForCreateGroup,
                $" expected Error Message For Create Group: {expectedErrorMessageForCreateGroup}" +
                $" actual Error Message For Create Group: {actualErrorMessageForCreateGroup}");
        }
    }
}