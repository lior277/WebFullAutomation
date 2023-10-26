using AirSoftAutomationFramework.Internals.DAL.ConfigurationManagement;
using AirSoftAutomationFramework.Internals.Factory;
using AirSoftAutomationFramework.Internals.Helpers;
using AirSoftAutomationFramework.Internals.NunitAttributes;
using AirSoftAutomationFramework.Objects.DTOs;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.AccountsApi.UsersMenuApi;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.AccountsApi.UsersMenuApi.UserMenuApi;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.Clients.ClientPage;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.Settings;
using AirSoftAutomationFramework.Objects.ForexObjects.Ui.Login;
using NUnit.Framework;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using TestsProject.TestsInternals;

namespace TestsProject.Login
{
    [TestFixture(DataRep.Chrome)]
    public class VerifyBlockUserAndClientWhenExceedLoginAttempt : TestSuitBase
    {
        public VerifyBlockUserAndClientWhenExceedLoginAttempt(string browser) : base(browser)
        {
            _browserName = browser;
        }

        private IApplicationFactory _apiFactory = new ApplicationFactory();
        private string _crmUrl = Config.appSettings.CrmUrl;
        private string _password = DataRep.Password;
        private string _tradingPlatformUrl = Config.appSettings.tradingPlatformUrl;
        private IWebDriver _driver;
        private string _browserName;
        private string _clientEmail;
        private string _clientId;
        private int _loginAttemps;
        private List<GetOfficeResponse> _officeList;
        private string _userName;
        private string _userId;
        private string _currentUserApiKey;

        [SetUp]

        #region PreCondition
        public void SetUp()
        {
            BeforeTest(_browserName);
            _driver = GetDriver();

            // create user
            _userName = TextManipulation.RandomString();

            _userId = _apiFactory
                .ChangeContext<IUserApi>(_driver)
                .CreateUserForUiPipe(_crmUrl, _userName);

            // create ApiKey
            _currentUserApiKey = _apiFactory
                .ChangeContext<IUserApi>()
                .PostCreateApiKeyRequest(_crmUrl, _userId);

            // create client 
            var clientName = TextManipulation.RandomString();
            _clientEmail = clientName + DataRep.EmailPrefix;

            _clientId = _apiFactory
                .ChangeContext<ICreateClientApi>()
                .CreateClientRequest(_crmUrl, clientName);

            // get default login Attempts 
            _loginAttemps = _apiFactory
                .ChangeContext<ISecurityTubApi>(_driver)
                .GetLoginSectionRequest(_crmUrl)
                .attempts;

            // get offices
            _officeList = _apiFactory
                .ChangeContext<IOfficeTabApi>()
                .GetOfficesRequest(_crmUrl);

            _officeList.ForEach(office => office
            .allowed_ip_addresses = DataRep.UserAllowedIps);

            _apiFactory
                .ChangeContext<IOfficeTabApi>()
                .PutOfficeRequest(_crmUrl, _officeList);
        }
        #endregion

        [TearDown]
        public void TearDown()
        {
            try
            {
                _officeList.ForEach(office => office
                .allowed_ip_addresses = DataRep.UserAllowedIps);

                _apiFactory
                    .ChangeContext<IOfficeTabApi>()
                    .PutOfficeRequest(_crmUrl, _officeList);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                AfterTest(_driver);
                DriverDispose(_driver);
            }
        }

        // CASE 1
        // incorrect password: ip exist in office
        // - result after finished login attempt: block user(no block ip)

        #region cant be checked
        // CASE 2
        // cant be checked
        // incorrect password: ip not exist in office
        // - result after passed login attempt: block ip and block user

        // release the block user

        // CASE 3
        // cant be checked
        // correct password: ip exist in user: ip not exist in office
        // fail to login because the ip is blocked from case 2

        // release the block ip

        // CASE 4
        // cant be checked
        // correct password: ip not exist in user: ip not exist in office
        // - result after passed login attempt: block ip login again block user

        // release the block user and ip
        #endregion

        // CASE 5
        // correct password: ip not exist in user: ip exist in office
        // - result after passed login attempt: block user(no block ip)

        // CASE 6
        // incorrect user name correct password: fail to login because the user not exist

        // CASE 7
        // correct user name incorrect password
        // - result after passed login attempt: block client

        [Test]
        [Category(DataRep.SanityCategory)]
        [Category(DataRep.RegressionCategory)]
        [RetryMenage(DataRep.NumOfRetraiesForFallingTest)]
        public void VerifyBlockUserAndClientWhenExceedLoginAttemptTest()
        {
            var expectedLoginErrorMessage = "The account is blocked." +
                " Contact the support team for more information";

            var expectedWrongUserNameErrorMessage = "Invalid credentials";

            var badPasword = $"bad{_password}";

            #region CASE 1 block user incorrect password: ip exist in office
            // crm login with wrong password
            _apiFactory
                .ChangeContext<ILoginPageUi>(_driver)
                .LoginPipe(_userName, password: badPasword);

            for (var i = 0; i < _loginAttemps - 1; i++)
            {
                // crm login
                _apiFactory
                    .ChangeContext<ILoginPageUi>(_driver)
                    .ClickOnLoginButton();
            }

            // get the Crm Login Wrong Password Error Message
            var actualCrmLoginWrongPasswordMessage = _apiFactory
                .ChangeContext<ILoginPageUi>(_driver)
                .GetLoginMessage(expectedLoginErrorMessage);

            // get id blocked user by wrong password
            var actualBlockedByPasswordUserId = _apiFactory
                .ChangeContext<ISecurityTubApi>()
                .GetBlockUsersRequest(_crmUrl)
                .Where(p => p._id.Equals(_userId))
                .FirstOrDefault()
                ._id;

            // get blocked user data
            var userData = _apiFactory
                .ChangeContext<IUsersApi>(_driver)
                .GetUserByIdRequest(_crmUrl, _userId)
                .GeneralResponse;
            #endregion

            _userId = userData.user._id;

            // release blocked user
            _apiFactory
                .ChangeContext<ISecurityTubApi>()
                .PatchReleaseBlockUserRequest(_crmUrl, _userId);

            #region cant be checked
            #region CASE 2 block ip incorrect password: ip not exist in office
            //// office tab
            //var officesList = _apiFactory
            //    .ChangeContext<IOfficeTabApi>()
            //    .GetOfficesRequest(_crmUrl);

            //var tempList = officesList.First().allowed_ip_addresses.ToList();
            //tempList.Remove(DataRep.OfficeIp);
            //officesList.First().allowed_ip_addresses = tempList.ToArray();

            //tempList = officesList.Last().allowed_ip_addresses.ToList();
            //tempList.Remove(DataRep.OfficeIp);
            //officesList.Last().allowed_ip_addresses = tempList.ToArray();

            //_apiFactory
            //    .ChangeContext<IOfficeTabApi>()
            //    .PutOfficeRequest(_crmUrl, officesList);           

            //// get released user data
            //userData = _apiFactory
            //    .ChangeContext<IUsersApi>(_driver)
            //    .GetUserByIdRequest(_crmUrl, _userId)
            //    .GeneralResponse;         

            //// crm login with wrong ip
            //for (var i = 0; i < _loginAttemps; i++)
            //{
            //    // crm login
            //    _apiFactory
            //        .ChangeContext<ILoginPageUi>(_driver)
            //        .ClickOnLoginButton(true);
            //}

            //// get id blocked user by wrong ip
            //var actualBlockedByIpUserId = _apiFactory
            //    .ChangeContext<ISecurityTubApi>()
            //    .GetBlockIpsRequest(_crmUrl)
            //    .FirstOrDefault()
            //    .users;

            //// get the Crm Login Wrong ip Error Message
            //var actualCrmLoginWrongIpMessage = _apiFactory
            //    .ChangeContext<ILoginPageUi>(_driver)
            //    .GetLoginMessage();
            #endregion

            ////// release blocked user
            ////_apiFactory
            ////    .ChangeContext<ISecurityTubApi>()
            ////    .PatchReleaseBlockUserRequest(_crmUrl, blockedByPasswordUserId);

            ///eran should ask sed
            #region CASE 3 fail to login because the ip is blocked from case 2
            /// crm login with correct password but ip is blocked
            //_apiFactory
            //    .ChangeContext<ILoginPageUi>(_driver)
            //    .LoginPipe(_userName, loginWithWarning: true);

            //// get the Crm Login Error Message
            //var actualBlockIpLoginMessage = _apiFactory
            //    .ChangeContext<ILoginPageUi>(_driver)
            //    .GetLoginMessage();
            #endregion

            //// get the id of the blocked ip
            //var blockedIpId = _apiFactory
            //    .ChangeContext<ISecurityTubApi>()
            //    .GetBlockIpsRequest(_crmUrl)
            //    .FirstOrDefault()
            //    ._id;

            //// release blocked ip
            //_apiFactory
            //    .ChangeContext<ISecurityTubApi>()
            //    .DeleteReleaseBlockIpUserRequest(_crmUrl, blockedIpId);

            #region CASE 4 block user correct password: ip not exist in user and not exist in office

            //for (var i = 0; i < _loginAttemps; i++)
            //{
            //    // crm login
            //    _apiFactory
            //        .ChangeContext<ILoginPageUi>(_driver)
            //        .ClickOnLoginButton(true);
            //}

            //// get the Crm Login Wrong Password Error Message
            //var actualCrmCorrectPasswordMessage = _apiFactory
            //    .ChangeContext<ILoginPageUi>(_driver)
            //    .GetLoginMessage();

            //// get id blocked user by wrong password
            //var blockedBywwPasswordUserId = _apiFactory
            //    .ChangeContext<ISecurityTubApi>()
            //    .GetBlockUsersRequest(_crmUrl)
            //    .Where(p => p._id.Equals(_userId))
            //    .FirstOrDefault()
            //    ._id;

            //// get blocked user data
            //userData = _apiFactory
            //    .ChangeContext<IUsersApi>(_driver)
            //    .GetUserByIdRequest(_crmUrl, _userId)
            //    .GeneralResponse;
            #endregion

            #endregion

            #region CASE 5 block user correct password: ip not exist in user exist in office
            // remove the ip from the user
            var ipAddresses = userData.user.allowed_ip_addresses.ToList();
            ipAddresses.RemoveAll(p => p.Equals(DataRep.OfficeIp));
            userData.user.allowed_ip_addresses = ipAddresses.ToArray();

            // update the user with new data
            _apiFactory
                .ChangeContext<IUserApi>(_driver)
                .PutEditUserRequest(_crmUrl, userData);

            // crm login correct password
            _apiFactory
                .ChangeContext<ILoginPageUi>(_driver)
                .LoginPipe(_userName);

            // get the Crm Login Wrong Password Error Message
            var actualCrmLoginUserNoIpMessage = _apiFactory
                .ChangeContext<ILoginPageUi>(_driver)
                .GetLoginMessage(expectedLoginErrorMessage);

            // get blocked user id
            var blockedByUserUserNoIpId = _apiFactory
                .ChangeContext<ISecurityTubApi>()
                .GetBlockUsersRequest(_crmUrl)
                .Where(p => p._id.Equals(_userId))
                .FirstOrDefault()
                ._id;

            // get blocked user data
            userData = _apiFactory
                .ChangeContext<IUsersApi>(_driver)
                .GetUserByIdRequest(_crmUrl, _userId)
                .GeneralResponse;
            #endregion

            _userId = userData.user._id;

            // release blocked user
            _apiFactory
                .ChangeContext<ISecurityTubApi>()
                .PatchReleaseBlockUserRequest(_crmUrl, _userId);

            #region CASE 6 incorrect user name correct password: fail to login because the user not exist
            var wrongUserName = _userName[..^2];

            _apiFactory
                .ChangeContext<ILoginPageUi>(_driver)
                .LoginPipe(wrongUserName);

            // Login Error Message
            var actualWrongUserNameErrorMessage = _apiFactory
                .ChangeContext<ILoginPageUi>(_driver)
                .GetLoginMessage(expectedWrongUserNameErrorMessage);

            #endregion

            #region CASE 7 trading Platform block client incorrect password
            for (var i = 0; i < _loginAttemps; i++)
            {
                // tp login with wrong password
                _apiFactory
                    .ChangeContext<ILoginPageUi>(_driver)
                    .LoginPipe(_clientEmail, _tradingPlatformUrl,
                    password: badPasword);
            }

            // get the tp Login Wrong Password Error Message
            var actualTpLoginWrongPasswordMessage = _apiFactory
                .ChangeContext<ILoginPageUi>(_driver)
                .GetLoginMessage(expectedLoginErrorMessage);

            string actualTpBlockClientId = null;

            for (var i = 0; i < 20; i++)
            {

                var actualTpBlockClients = _apiFactory
                    .ChangeContext<ISecurityTubApi>()
                    .GetBlockUsersRequest(_crmUrl)
                    .Where(p => p._id.Equals(_clientId))
                    .ToList();

                if (actualTpBlockClients.Count != 0)
                {
                    actualTpBlockClientId = actualTpBlockClients
                        .FirstOrDefault()
                        ._id;

                    break;
                }

                Thread.Sleep(400);

                continue;
            }

            // get blocked user id
            //var actualTpBlockClientId = temp


            #endregion

            Assert.Multiple(() =>
            {
                Assert.True(actualCrmLoginWrongPasswordMessage == expectedLoginErrorMessage,
                    $" expected Crm Login Error Message: {expectedLoginErrorMessage}" +
                    $" actual Crm Login Error Message: {actualCrmLoginWrongPasswordMessage}");

                Assert.True(actualBlockedByPasswordUserId == _userId,
                    $" expected Blocked wrong Password : {_userId}" +
                    $" actual Blocked wrong Password: {actualBlockedByPasswordUserId}");

                Assert.True(actualCrmLoginUserNoIpMessage == expectedLoginErrorMessage,
                    $" expected Crm Login User No Ip Error Message: {expectedLoginErrorMessage}" +
                    $" actual Crm Login User No Ip Error Error Message: {actualCrmLoginUserNoIpMessage}");

                Assert.True(blockedByUserUserNoIpId == _userId,
                    $" expected Blocked User No Ip : {_userId}" +
                    $" actual Blocked User No Ip: {blockedByUserUserNoIpId}");

                Assert.True(actualWrongUserNameErrorMessage == expectedWrongUserNameErrorMessage,
                    $" expected Wrong User Name Error Message: {expectedWrongUserNameErrorMessage}" +
                    $" actual Wrong User Name Error Message: {actualWrongUserNameErrorMessage}");

                Assert.True(actualTpLoginWrongPasswordMessage == expectedLoginErrorMessage,
                    $" expected Tp Login Wrong Password Error Message: {expectedLoginErrorMessage}" +
                    $" actual Tp Login Wrong Password Error Message: {actualTpLoginWrongPasswordMessage}");

                Assert.True(actualTpBlockClientId == _clientId,
                    $" expected Tp Block Client Id : {_clientId}" +
                    $" actual Tp Block Client Id: {actualTpBlockClientId}");
            });
        }
    }
}