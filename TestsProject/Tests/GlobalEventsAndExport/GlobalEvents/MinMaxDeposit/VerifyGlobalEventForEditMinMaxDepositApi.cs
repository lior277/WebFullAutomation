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
    public class VerifyGlobalEventForEditMinMaxDepositApi : TestSuitBase
    {
        #region Test Preparation
        private IApplicationFactory _apiFactory = new ApplicationFactory();
        private string _crmUrl = Config.appSettings.CrmUrl;
        private string _userId;
        private string _currentUserApiKey;
        private string _userName;

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

            // update Minimum Deposit 
            _apiFactory
                .ChangeContext<IGeneralTabApi>()
                .PutMinimumDepositRequest(_crmUrl, apiKey: _currentUserApiKey);

            // update max Deposit 
            _apiFactory
                .ChangeContext<IGeneralTabApi>()
                .PutMaximumDepositRequest(_crmUrl, apiKey: _currentUserApiKey);
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
        public void VerifyGlobalEventForEditMinMaxDepositApiTest()
        {
            var expectedType = "edit_min_max_deposits";
            var expectedGlobal = true;
            var expectedActionMadeBy = _userId;
            var actualGlobalList = new List<bool>();
            var actualMadeByList = new List<string>();

            // get global event create office
            var actualTypeMinMaxDeposit = _apiFactory
               .ChangeContext<IGlobalEventsApi>()
               .GetGlobalEventsByUserRequest(_crmUrl, expectedActionMadeBy, _currentUserApiKey)
               .Where(p => p.type == "edit_min_max_deposits")
               .ToList();

            Assert.Multiple(() =>
            {
                Assert.True(actualTypeMinMaxDeposit.All(p => p.type == expectedType),
                    $" actual Min max Deposit type :" +
                    $" {actualTypeMinMaxDeposit.SelectMany(p => p.type).ListToString()}" +
                    $" expected Min max Deposit type: {expectedType}");

                Assert.True(actualTypeMinMaxDeposit.All(p => p.global == expectedGlobal),
                    $" actual Min max Deposit type : " +
                    $"{actualTypeMinMaxDeposit.Select(p => p.global).ListToString()}" +
                    $" expected Min max Deposit type: {expectedGlobal}");

                Assert.True(actualTypeMinMaxDeposit.All(p => p.action_made_by == _userName),
                    $" actual Min max action made by : " +
                    $"{actualTypeMinMaxDeposit.SelectMany(p => p.action_made_by).ListToString()}" +
                    $" expected Min max action made by: {_userName}");

                Assert.True(actualTypeMinMaxDeposit.All(p => p.action_made_by_user_id == _userId),
                    $" actual Min max action made by user id : " +
                    $"{actualTypeMinMaxDeposit.SelectMany(p => p.action_made_by_user_id).ListToString()}" +
                    $" expected Min max action made by user id: {_userId}");
            });
        }
    }
}