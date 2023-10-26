// Ignore Spelling: Api

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

namespace TestsProject.Tests.GlobalEventsAndExport.GlobalEvents.SavingAccount
{
    [TestFixture]
    public class VerifyGlobalEventForCreateEditDeleteSaleStatus2Api : TestSuitBase
    {
        #region Test Preparation
        private IApplicationFactory _apiFactory = new ApplicationFactory();
        private string _crmUrl = Config.appSettings.CrmUrl;
        private string _currentUserApiKey;
        private string _userId;
        private string _userName;
        private string _expectedEditSalesStatusName;
        private string _expectedSalesStatusName;

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

            // get Sales Status 2
            var salesStatusText = _apiFactory
                 .ChangeContext<IGeneralTabApi>()
                 .GetSalesStatus2Request(_crmUrl);

            _expectedSalesStatusName = TextManipulation.RandomString();
            _expectedEditSalesStatusName = $"{TextManipulation.RandomString()}Automation";

            // create new sales status
            _apiFactory
                .ChangeContext<IGeneralTabApi>()
                .PutSalesStatus2Request(_crmUrl, salesStatusText,
                _expectedSalesStatusName, null, apiKey: _currentUserApiKey);

            // edit sales status
            _apiFactory
               .ChangeContext<IGeneralTabApi>()
               .PutSalesStatus2Request(_crmUrl, salesStatusText, _expectedEditSalesStatusName,
               _expectedSalesStatusName, apiKey: _currentUserApiKey);

            // delete sales status 
            _apiFactory
               .ChangeContext<IGeneralTabApi>()
               .PutSalesStatus2Request(_crmUrl, salesStatusText,
               oldStatusName: _expectedEditSalesStatusName, apiKey: _currentUserApiKey);
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

        [Test]
        [Category(DataRep.SanityCategory)]
        [Category(DataRep.RegressionCategory)]
        [RetryMenage(DataRep.NumOfRetraiesForFallingTest)]
        public void VerifyGlobalEventForCreateEditDeleteSaleStatus2ApiTest()
        {
            var expectedTypeList = new List<string>()
            { { "create_sales_status_text2" }, { "edit_sales_status_text2" },
                { "delete_sales_status_text2" } };

            var expectedGlobal = true;
            var expectedActionMadeByUser = _userName;
            var expectedActionMadeByUserId = _userId;
            var actualTypeList = new List<string>();
            var actualGlobalList = new List<bool>();
            var actualMadeByList = new List<string>();
            var actualMadeByUserIdList = new List<string>();
            var actualSalesStatusNameList = new List<string>();

            // get global events
            var actualGlobals = _apiFactory
               .ChangeContext<IGlobalEventsApi>()
               .GetGlobalEventsByUserRequest(_crmUrl, _userName, _currentUserApiKey);

            actualGlobals.ForEach(p => actualGlobalList.Add(p.global));
            actualGlobals.ForEach(p => actualTypeList.Add(p.type));
            actualGlobals.ForEach(p => actualMadeByList.Add(p.action_made_by));
            actualGlobals.ForEach(p => actualMadeByUserIdList.Add(p.action_made_by_user_id));
            actualGlobals.ForEach(p => actualSalesStatusNameList.Add(p.sales_status_name));
         
            Assert.Multiple(() =>
            {
                Assert.True(actualGlobalList.All(p => p.Equals(true)),
                    $" actual Global list : {actualGlobalList.ListToString()}" +
                    $" expected Global list: {expectedGlobal}");

                Assert.True(actualTypeList.CompareTwoListOfString(expectedTypeList).Count == 0,
                    $" actual Type List : {actualTypeList.ListToString()}" +
                    $" expected type List: {expectedTypeList.ListToString()}");

                Assert.True(actualMadeByList.All(p => p.Equals(_userName)),
                    $" actual user name : {actualMadeByList.ListToString()}" +
                    $" expected user name: {_userName}");

                Assert.True(actualMadeByUserIdList.All(p => p.Equals(expectedActionMadeByUserId)),
                    $" actual user id : {actualMadeByUserIdList.ListToString()}" +
                    $" expected user name: {expectedActionMadeByUserId}");

                Assert.True(actualSalesStatusNameList[0].Equals(_expectedSalesStatusName) &&
                    actualSalesStatusNameList[2].Equals(_expectedEditSalesStatusName),
                    $" actual Sales Status Name : {actualSalesStatusNameList.ListToString()}" +
                    $" expected Sales Status Name: {_expectedSalesStatusName}");
            });
        }
    }
}