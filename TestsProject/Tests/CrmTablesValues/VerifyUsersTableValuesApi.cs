using AirSoftAutomationFramework.Internals.DAL.ConfigurationManagement;
using AirSoftAutomationFramework.Internals.Helpers;
using AirSoftAutomationFramework.Objects.DTOs;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.AccountsApi.UsersMenuApi;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.AccountsApi.UsersMenuApi.UserMenuApi;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.Clients.ClientCard;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.Clients.ClientCard.FinancesTab;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.Clients.ClientCard.SATab;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.Clients.ClientPage;
using AirSoftAutomationFramework.Objects.ForexObjects.ApiGeneral;
using DocumentFormat.OpenXml.Wordprocessing;
using NUnit.Framework;
using System;
using System.Linq;
using AirSoftAutomationFramework.Internals.Factorys;
using AirSoftAutomationFramework.Internals.NunitAtributes;
using TestsProject.TestsInternals;

namespace TestsProject.Tests.TablesValues
{
    [TestFixture]
    public class VerifyUsersTableValuesApi : TestSuitBase
    {
        #region Test Preparation

        private IApplicationFactory _apiFactory = new ApplicationFactory();
        private string _crmUrl = Config.appSettings.CrmUrl;
        private string _userName;
        private string _userId;
        private string _userEmail;

        [SetUp]
        public void SetUp()
        {
            BeforeTest();
            #region PreCondition
            var crmUrl = Config.appSettings.CrmUrl;

            // create user
            _userName = TextManipulation.RandomString();
            _userEmail = _userName + DataRep.EmailPrefix;

            _userId = _apiFactory
                .ChangeContext<IUserApi>()
                .PostCreateUserRequest(_crmUrl, _userName,
                role: DataRep.AdminWithUsersOnlyRoleName);
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

        [Test]
        [Category(DataRep.SanityCategory)]
        [Category(DataRep.RegressionCategory)]
        [RetryMenage(DataRep.NumOfRetraiesForFallingTest)]
        public void VerifyUsersTableValuesApiTest()
        {
            // get user data from table
            var userDataFromUserCard = _apiFactory
                .ChangeContext<IUsersApi>()
                .GetUserByIdRequest(_crmUrl, _userId)
                .GeneralResponse;

            // get user data from table
            var userDataFromTable = _apiFactory
                .ChangeContext<IUsersApi>()
                .GetActiveUsersRequest(_crmUrl)
                .userData
                .Where(p => p.username == _userName)
                .FirstOrDefault();


            Assert.Multiple(() =>
            {
                Assert.True(userDataFromTable._id == _userId,
                    $" expected id: {_userId}" +
                    $" actual id: {userDataFromTable._id}");

                Assert.True(userDataFromTable.first_name == _userName,
                    $" expected first name: {_userName}" +
                    $" actual first name: {userDataFromTable.first_name}");

                Assert.True(userDataFromTable.last_name == _userName,
                    $" expected last_name: {_userName}" +
                    $" actual last_name: {userDataFromTable.last_name}");

                Assert.True(userDataFromTable.username == _userName,
                    $" expected user name: {_userName}" +
                    $" actual user name: {userDataFromTable.username}");

                Assert.True(userDataFromTable.email == _userEmail,
                    $" expected email: {_userEmail}" +
                    $" actual email: {userDataFromTable.email}");

                Assert.True(userDataFromTable.role == userDataFromUserCard.user.role,
                    $" expected role: {userDataFromUserCard.user.role}" +
                    $" actual role: {userDataFromTable.role}");

                Assert.True(userDataFromTable.affiliate == userDataFromUserCard.user.affiliate,
                    $" expected affiliate: {userDataFromUserCard.user.affiliate}" +
                    $" actual affiliate: {userDataFromTable.affiliate}");

                Assert.True(userDataFromTable.gmt_timezone == userDataFromUserCard.user.gmt_timezone,
                    $" expected gmt_timezone: {userDataFromUserCard.user.gmt_timezone}" +
                    $" actual gmt_timezone: {userDataFromTable.gmt_timezone}");

                Assert.True(userDataFromTable.office == "Main Office",
                    $" expected office: Main Office" +
                    $" actual office: {userDataFromTable.office}");

                Assert.True(userDataFromTable.last_login == userDataFromUserCard.user.last_login,
                    $" expected role: {userDataFromUserCard.user.role}" +
                    $" actual role: {userDataFromTable.role}");
            });
        }
    }
}