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
    public class VerifyRiskRestrictionsOptionsForDifferentTitleApi : TestSuitBase
    {
        #region Test Preparation
        private IApplicationFactory _apiFactory = new ApplicationFactory();
        private string _crmUrl = Config.appSettings.CrmUrl;
        private Default_Attr _tradeGroup;
        private string _cryptoGroupId;
        private string _currentUserApiKey;
        private string _cryptoGroupName;
        private List<string> _tradeGroupsIdsListForDelete = new List<string>();

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

            // set Risk Restrictions to default values
            _apiFactory
                .ChangeContext<ISuperAdminTubApi>()
                .PutRiskRestrictionsRequest(_crmUrl)
                .PutGroupRestrictionsRequest(_crmUrl);

            // create group with random title
            _cryptoGroupId = _apiFactory
                .ChangeContext<ITradeGroupApi>()
                .PostCreateTradeGroupRequest(_crmUrl, new List<object>
                { _tradeGroup }, _cryptoGroupName,
                apiKey: _currentUserApiKey);
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

                // Delete Trade Group
                _apiFactory
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
        /// set Risk Restrictions to 0 num of groups   
        /// verify error message for create new group with random title
        /// verify error message for edit  group title with random title
        /// </summary>        
        [Test]
        [Description("based on jira https://airsoftltd.atlassian.net/browse/AIRV2-4642")]
        [Category(DataRep.SanityCategory)]
        [Category(DataRep.RegressionCategory)]
        [RetryMenage(DataRep.NumOfRetraiesForFallingTest)]
        public void VerifyRiskRestrictionsOptionsForDifferentTitleApiTest()
        {
            var expectedCreateGroupWithDiferenteTitle = "the name is not in the list";
            var expectedEditGroupWithDiferenteTitle = "you dont have permissions to change the group name";
            var expectedCryptoGroupName = _cryptoGroupName;

            // update allow Group Title Change to false
            _apiFactory
                .ChangeContext<ISuperAdminTubApi>()
                .PutRiskRestrictionsRequest(_crmUrl, allowGroupTitleChange: false);

            // create group with random title
            var actualCreateGroupWithDiferenteTitle = _apiFactory
                .ChangeContext<ITradeGroupApi>()
                .PostCreateTradeGroupRequest(_crmUrl,
                new List<object> { _tradeGroup }, _cryptoGroupName,
                 apiKey: _currentUserApiKey, false);

            // get group by id
            var cryptoGroup = _apiFactory
                .ChangeContext<ITradeGroupApi>()
                .GetTradeGroupsRequest(_crmUrl)
                .GeneralResponse
                .Where(p => p._id == _cryptoGroupId)
                .FirstOrDefault();

            var newTitle = cryptoGroup.name = $"{_cryptoGroupName}newTitle";
            cryptoGroup.name = newTitle;

            // edit group title
            var actualEditGroupWithDiferenteTitle = _apiFactory
                .ChangeContext<ITradeGroupApi>()
                .PutEditTradeGroupRequest(_crmUrl, _cryptoGroupId, cryptoGroup,
                apiKey: _currentUserApiKey, false);

            // get group by id
            var actualCryptoGroupName = _apiFactory
                .ChangeContext<ITradeGroupApi>()
                .GetTradeGroupsRequest(_crmUrl)
                .GeneralResponse
                .Where(p => p._id == _cryptoGroupId)
                .FirstOrDefault()
                .name;

            Assert.Multiple(() =>
            {
                Assert.True(actualCreateGroupWithDiferenteTitle == (expectedCreateGroupWithDiferenteTitle),
                     $" expected Create Group With Diferente Title : {expectedCreateGroupWithDiferenteTitle}" +
                     $" actual Create Group With Diferente Title : {actualCreateGroupWithDiferenteTitle}");

                Assert.True(actualEditGroupWithDiferenteTitle == expectedEditGroupWithDiferenteTitle,
                    $" expected edit Group With Diferente Title : {expectedEditGroupWithDiferenteTitle}" +
                    $" actual edit Group With Diferente Title : {actualEditGroupWithDiferenteTitle}");

                Assert.True(actualCryptoGroupName == (expectedCryptoGroupName),
                    $" expected Crypto Group Name : {expectedCryptoGroupName}" +
                    $" actual Crypto Group Name : {actualCryptoGroupName}");
            });
        }
    }
}