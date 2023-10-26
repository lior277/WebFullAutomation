// Ignore Spelling: Api

using AirSoftAutomationFramework.Internals.DAL.ConfigurationManagement;
using AirSoftAutomationFramework.Internals.Factory;
using AirSoftAutomationFramework.Internals.Helpers;
using AirSoftAutomationFramework.Internals.NunitAttributes;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.AccountsApi.Roles;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.AccountsApi.UsersMenuApi;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.AccountsApi.UsersMenuApi.UserMenuApi;
using NUnit.Framework;
using TestsProject.TestsInternals;

namespace TestsProject.Tests.Agent
{
    [TestFixture]
    public class VerifyAgentCanSeeAllSalesAgentsApi : TestSuitBase
    {
        #region Test Preparation       
        private IApplicationFactory _apiFactory = new ApplicationFactory();
        private string _crmUrl = Config.appSettings.CrmUrl;
        private string _userId;
        private string _userName;

        [SetUp]
        public void SetUp()
        {
            BeforeTest();
            #region PreCondition

            _userName = TextManipulation.RandomString();

            // create user
            _userId = _apiFactory
             .ChangeContext<IUserApi>()
             .PostCreateUserRequest(_crmUrl, _userName,
             role: DataRep.AdminWithDialerRole);
            #endregion
        }
        #endregion

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
        /// <summary>
        /// pre condition
        /// campaign and affiliate connected     
        /// verify affiliate can see all sales agents
        /// </summary>
        [Test]
        [Category(DataRep.ApiDocCategory)]
        [Category(DataRep.SanityCategory)]
        [Category(DataRep.RegressionCategory)]
        [RetryMenage(DataRep.NumOfRetraiesForFallingTest)]
        public void VerifyAgentCanSeeAllSalesAgentsApiTest()
        {
            var salesAgent = _apiFactory
                .ChangeContext<IUsersApi>()
                .GetAllSalesAgentsByAgentRequest(_crmUrl)
                .Item2;

            Assert.Multiple(() =>
            {
                Assert.IsTrue(salesAgent.Contains(_userId),
                    $"actual userDetailsByDialer not contains: {_userId}");

                Assert.IsTrue(salesAgent.Contains(_userName),
                   $"actual userDetailsByDialer not contains: {_userName}");
            });
        }
    }
}