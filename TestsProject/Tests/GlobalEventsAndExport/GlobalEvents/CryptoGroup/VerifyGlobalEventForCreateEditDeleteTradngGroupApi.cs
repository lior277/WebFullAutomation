// Ignore Spelling: Api

using System.Collections.Generic;
using System.Linq;
using AirSoftAutomationFramework.Internals.DAL.ConfigurationManagement;
using AirSoftAutomationFramework.Internals.ExtensionsMethods;
using AirSoftAutomationFramework.Internals.Factorys;
using AirSoftAutomationFramework.Internals.Helpers;
using AirSoftAutomationFramework.Internals.NunitAtributes;
using AirSoftAutomationFramework.Objects.DTOs;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.AccountsApi.UsersMenuApi.UserMenuApi;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Trade;
using AirSoftAutomationFramework.Objects.ForexObjects.ApiGeneral;
using Microsoft.Graph;
using NUnit.Framework;
using TestsProject.TestsInternals;

namespace TestsProject.Tests.GlobalEventsAndExport.GlobalEvents.SavingAccount
{
    [TestFixture]
    public class VerifyGlobalEventForCreateEditDeleteTradingGroupApi : TestSuitBase
    {
        #region Test Preparation
        private IApplicationFactory _apiFactory = new ApplicationFactory();
        private string _crmUrl = Config.appSettings.CrmUrl;
        private string _currentUserApiKey;
        private string _userId;
        private string _userName;
        private string _expectedTradeGroupId;
        private string _expectedTradeGroupName;

        [SetUp]
        public void SetUp()
        {
            BeforeTest();
            var tradingPlatformUrl = Config.appSettings.tradingPlatformUrl;

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

            #region create crypto group
            // create crypto group
            var tradeGroupAttributes = new Default_Attr
            {
                commision = 0,
                leverage = 5,
                maintenance = 1,
                minimum_amount = 1,
                minimum_step = 1,
                spread = -0.1,
                margin_call = null,
                swap_long = 0.04,
                swap_short = 0.04,
                swap_time = "00:00:00",
            };

            // create trade group
            _expectedTradeGroupName = TextManipulation.RandomString();

            _expectedTradeGroupId = _apiFactory
                .ChangeContext<ITradeGroupApi>()
                .PostCreateTradeGroupRequest(_crmUrl, new List<object> { tradeGroupAttributes },
                _expectedTradeGroupName, apiKey: _currentUserApiKey)
                .Trim('"');

            var cryptoGroup = _apiFactory
                .ChangeContext<ITradeGroupApi>()
                .GetTradeGroupsRequest(_crmUrl)
                .GeneralResponse
                .Where(p => p._id == _expectedTradeGroupId)
                .FirstOrDefault();

            // edit group 
            var actualEditGroupWithDiferenteTitle = _apiFactory
                .ChangeContext<ITradeGroupApi>()
                .PutEditTradeGroupRequest(_crmUrl, _expectedTradeGroupId, cryptoGroup,
                apiKey: _currentUserApiKey);

            // delete group 
            _apiFactory
               .ChangeContext<ITradeGroupApi>()
               .DeleteTradeGroupRequest(_crmUrl, _expectedTradeGroupId, _currentUserApiKey);
            #endregion
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
        public void VerifyGlobalEventForCreateEditDeleteTradingGroupApiTest()
        {
            var expectedTypeList = new List<string>()
            { { "cfd_group_create" }, { "cfd_group_change" }, { "delete_trade_group" } };

            var expectedGlobal = true;
            var expectedActionMadeByUser = _userName;
            var expectedActionMadeByUserId = _userId;
            var actualTypeList = new List<string>();
            var actualGlobalList = new List<bool>();
            var actualMadeByList = new List<string>();
            var actualMadeByUserIdList = new List<string>();
            var actualTradingGroupNameList = new List<string>();
            var actualTradingGroupIdList = new List<string>();

            // get global events
            var actualGlobals = _apiFactory
               .ChangeContext<IGlobalEventsApi>()
               .GetGlobalEventsByUserRequest(_crmUrl,
               _userName, _currentUserApiKey);

            actualGlobals.ForEach(p => actualTypeList.Add(p.type));
            actualGlobals.ForEach(p => actualGlobalList.Add(p.global));
            actualGlobals.ForEach(p => actualMadeByList.Add(p.action_made_by));
            actualGlobals.ForEach(p => actualMadeByUserIdList.Add(p.action_made_by_user_id));
            actualGlobals.ForEach(p => actualTradingGroupNameList.Add(p.trade_group_name));
            actualGlobals.ForEach(p => actualTradingGroupIdList.Add(p.trade_group_id));      

            Assert.Multiple(() =>
            {
                Assert.True(actualTypeList.CompareTwoListOfString(expectedTypeList).Count == 0,
                    $" actual Type List : {actualTypeList.ListToString()}" +
                    $" expected type List: {expectedTypeList.ListToString()}");

                Assert.True(actualGlobalList.All(p => p.Equals(true)),
                    $" actual Global list : {actualGlobalList.ListToString()}" +
                    $" expected Global list: {expectedGlobal}");

                Assert.True(actualMadeByList.All(p => p.Equals(_userName)),
                    $" actual Made By user name : {actualMadeByList.ListToString()}" +
                    $" expected Made By user name: {_userName}");

                Assert.True(actualMadeByUserIdList.All(p => p.Equals(_userId)),
                    $" actual Made By User Id : {actualMadeByUserIdList.ListToString()}" +
                    $" expected Made By User Id: {_userId}");

                Assert.True(actualTradingGroupNameList.All(p => p.Equals(_expectedTradeGroupName)),
                    $" actual Trading Group Name : {actualTradingGroupNameList.ListToString()}" +
                    $" expected Trading Group Name: {_expectedTradeGroupName}");

                Assert.True(actualTradingGroupIdList[0].Equals(_expectedTradeGroupId)
                    && actualTradingGroupIdList[1].Equals(_expectedTradeGroupId),
                    $" actual Trading Group id : {actualTradingGroupIdList.ListToString()}" +
                    $" expected Trading Group id: {_expectedTradeGroupId}");
            });
        }
    }
}