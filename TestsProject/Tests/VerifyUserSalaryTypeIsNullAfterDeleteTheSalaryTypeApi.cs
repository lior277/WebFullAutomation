// Ignore Spelling: Api


using AirSoftAutomationFramework.Internals.Helpers;
using NUnit.Framework;
using AirSoftAutomationFramework.Internals.DAL.ConfigurationManagement;
using AirSoftAutomationFramework.Internals.Factorys;
using AirSoftAutomationFramework.Internals.NunitAtributes;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.Settings;
using TestsProject.TestsInternals;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.AccountsApi.UsersMenuApi;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.AccountsApi.UsersMenuApi.UserMenuApi;

namespace TestsProject.Tests
{
    [TestFixture]
    public class VerifyUserSalaryTypeIsNullAfterDeleteTheSalaryTypeApi : TestSuitBase
    {
        #region Test Preparation
        private IApplicationFactory _apiFactory = new ApplicationFactory();
        private string _crmUrl = Config.appSettings.CrmUrl;
        private string _userId;

        [SetUp]
        public void SetUp()
        {
            BeforeTest();
            #region PreCondition
            var depositAmount = 10;

            // create Salas Agent Salary
            var salesAgentSalaryId = _apiFactory
                 .ChangeContext<ISalesTabApi>()
                 .CreateSalasAgentSalaryPipe(_crmUrl, depositAmount);

            // create user
            var userName = TextManipulation.RandomString();

            _userId = _apiFactory
                .ChangeContext<IUserApi>()
                .PostCreateUserRequest(_crmUrl, userName,
                salesAgentSalaryId: salesAgentSalaryId);

            // delete Salas Agent Salary
            _apiFactory
                 .ChangeContext<ISalesTabApi>()
                 .DeleteSalasAgentSalaryRequest(_crmUrl, salesAgentSalaryId);
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

        // create user with salary type
        // delete the salary type
        // verify user salary type equal to null
        [Test]
        [Description("based on jira https://airsoftltd.atlassian.net/browse/AIRV2-4497")]
        [Category(DataRep.SanityCategory)]
        [Category(DataRep.RegressionCategory)]
        [RetryMenage(DataRep.NumOfRetraiesForFallingTest)]
        public void VerifyUserSalaryTypeIsNullAfterDeleteTheSalaryTypeApiTest()
        {
            object expectedUserSalaryValue = null;

            var actualUserSalaryValue = _apiFactory
                .ChangeContext<IUsersApi>()
                .GetUserByIdRequest(_crmUrl, _userId)
                .GeneralResponse
                .user
                .salary_id;

            Assert.True(actualUserSalaryValue == expectedUserSalaryValue,
                   $" actual User Salary Value: {actualUserSalaryValue}" +
                   $" expected User Salary Value: {expectedUserSalaryValue}");
        }
    }
}