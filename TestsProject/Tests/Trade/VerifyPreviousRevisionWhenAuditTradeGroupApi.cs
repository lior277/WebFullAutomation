using AirSoftAutomationFramework.Internals.DAL.ConfigurationManagement;

using AirSoftAutomationFramework.Internals.Helpers;
using NUnit.Framework;
using System.Linq;
using TestsProject.TestsInternals;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Trade;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.AccountsApi.UsersMenuApi.UserMenuApi;
using System.Collections.Generic;
using AirSoftAutomationFramework.Internals.Factorys;
using AirSoftAutomationFramework.Internals.NunitAtributes;
using AirSoftAutomationFramework.Objects.DTOs;

namespace TestsProject.Tests.Trade
{
    [TestFixture]
    public class VerifyPreviousRevisionWhenAuditTradeGroupApi : TestSuitBase
    {
        #region Test Preparation
        private IApplicationFactory _apiFactory = new ApplicationFactory();
        private string _crmUrl = Config.appSettings.CrmUrl;
        private string _newGroupId;
        private double _firstCommisionAfterEdit = 0.4;
        private double _secondCommisionAfterEdit = 0.5;


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
            var currentUserApiKey = _apiFactory
                .ChangeContext<IUserApi>()
                .PostCreateApiKeyRequest(_crmUrl, userId);

            var tradeGroup = new Default_Attr
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

            // create trade group 
            var cryptoGroupName = TextManipulation.RandomString();

            _newGroupId = _apiFactory
                .ChangeContext<ITradeGroupApi>()
                .PostCreateTradeGroupRequest(_crmUrl,
                new List<object> { tradeGroup },
               cryptoGroupName, apiKey: currentUserApiKey);

            // get new group data by name
            var newGroupData = _apiFactory
                .ChangeContext<ITradeGroupApi>()
                .GetTradeGroupsRequest(_crmUrl)
                .GeneralResponse
                .Where(p => p.name == cryptoGroupName)
                .FirstOrDefault();

            newGroupData.forex.commision = _firstCommisionAfterEdit;

            // edit cripto group
            _apiFactory
                .ChangeContext<ITradeGroupApi>()
                .PutEditTradeGroupRequest(_crmUrl, _newGroupId, newGroupData);

            // get new group data by name
            newGroupData = _apiFactory
                .ChangeContext<ITradeGroupApi>()
                .GetTradeGroupsRequest(_crmUrl)
                .GeneralResponse
                .Where(p => p.name == cryptoGroupName)
                .FirstOrDefault();

            newGroupData.forex.commision = _secondCommisionAfterEdit;

            // edit cripto group
            _apiFactory
                .ChangeContext<ITradeGroupApi>()
                .PutEditTradeGroupRequest(_crmUrl, _newGroupId, newGroupData);
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
        // create new  trading group 
        // assign to Group Data.forex.commision = _randomCommision;
        // edit the trading group twice  with new forex commission   0.4, 0.5 
        // verify last revision defaultGroupData.forex.commision = 0.4;

        [Test]
        [Category(DataRep.SanityCategory)]
        [Category(DataRep.RegressionCategory)]
        [RetryMenage(DataRep.NumOfRetraiesForFallingTest)]
        public void VerifyPreviousRevisionWhenAuditTradeGroupApiTest()
        {
            var actualNumOfRevisions = _apiFactory
                .ChangeContext<ITradeGroupApi>()
                .GetTradeGroupsRevisionsRequest(_crmUrl, _newGroupId)
                .Select(p => p.revision)
                .LastOrDefault();

            var actualCommision = _apiFactory
                .ChangeContext<ITradeGroupApi>()
                .GetTradeGroupsByRevisionIdRequest(_crmUrl,
                _newGroupId, actualNumOfRevisions)
                .body
                .forex
                .commision;

            Assert.Multiple(() =>
            {
                Assert.True(actualNumOfRevisions.Equals(2),
                    $" actual Num Of Revisions: {actualNumOfRevisions}" +
                    $" expected Num Of Revisions: {2}");

                Assert.True(actualCommision == _firstCommisionAfterEdit.ToString(),
                    $" actual forex commission after audit: {actualCommision}" +
                    $" expected forex commission after audit: {_firstCommisionAfterEdit}");
            });
        }
    }
}