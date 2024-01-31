// Ignore Spelling: Api

using System;
using System.Collections.Generic;
using System.Linq;
using AirSoftAutomationFramework.Internals.DAL.ConfigurationManagement;
using AirSoftAutomationFramework.Internals.ExtensionsMethods;
using AirSoftAutomationFramework.Internals.Factorys;
using AirSoftAutomationFramework.Internals.Helpers;
using AirSoftAutomationFramework.Internals.NunitAtributes;
using AirSoftAutomationFramework.Models;
using AirSoftAutomationFramework.Objects.DTOs;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.AccountsApi.UsersMenuApi.UserMenuApi;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.Clients.ClientCard.FinancesTab;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.Clients.ClientPage;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.Settings;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Trade;
using AirSoftAutomationFramework.Objects.ForexObjects.ApiGeneral;
using AirSoftAutomationFramework.Objects.ForexObjects.ApiGeneral.Login;
using AirSoftAutomationFramework.Objects.ForexObjects.TradingPlatform.TradeObjects.TradeApi;
using NUnit.Framework;
using TestsProject.TestsInternals;

namespace TestsProject.Tests.TradingPlatform
{
    [TestFixture]
    public class VerifyBlockTradeAndEmailsWhenPnlChangesApi : TestSuitBase
    {
        #region Test Preparation

        private IApplicationFactory _apiFactory = new ApplicationFactory();
        private string _crmUrl = Config.appSettings.CrmUrl;
        private string _userEmail;
        private string _clientEmail;
        private string _clientId;
        private Default_Attr _tradeGroup;
        private List<string> _exportEmails = new List<string>();
        private int _suspiciousPercentage = 1;
        private string _testimUrl = DataRep.TesimUrl;
        private QaAutomation01Context _dbContext = new QaAutomation01Context();
        private string _tradingPlatformUrl =  Config.appSettings.tradingPlatformUrl;
        private string _tradeGroupId;
        private string _clientName; 
        private string _userId;    
        private GetLoginResponse _loginCookies;
        private int _tradeAmount = 3;

        [SetUp]
        public void SetUp()
        {
            BeforeTest();
            #region PreCondition                  
            var depositAmount = 10000;
            var testimEmailPerfix = DataRep.TestimEmailPrefix;

            _tradeGroup = new Default_Attr
            {
                commision = 0,
                leverage = 5,
                maintenance = 1,
                minimum_amount = 1,
                minimum_step = 1,
                spread = -1,
                margin_call = null,
                swap_long = 0.04,
                swap_short = 0.04,
                swap_time = "00:00:00",
            };

            // create user
            var userName = TextManipulation.RandomString();
            _userEmail = userName + testimEmailPerfix;

            _userId = _apiFactory
                .ChangeContext<IUserApi>()
                .PostCreateUserRequest(_crmUrl, userName,
                role: DataRep.AdminWithUsersOnlyRoleName);

            // create client 
            _clientName = TextManipulation.RandomString();
            _clientEmail = _clientName + DataRep.EmailPrefix;

            _clientId = _apiFactory
                .ChangeContext<ICreateClientApi>()
                .CreateClientRequest(_crmUrl, _clientName);

            var clientsIds = new List<string> { _clientId };

            // connect  User To  Client 
            _apiFactory
                .ChangeContext<IClientsApi>()
                .PatchMassAssignSaleAgentsRequest(_crmUrl, _userId,
                clientsIds);

            // create deposit
            _apiFactory
                .ChangeContext<IFinancesTabApi>()
                .PostDepositRequest(_crmUrl, _clientId, depositAmount);

            // get login Data for trading Platform
            _loginCookies = _apiFactory
                .ChangeContext<ILoginApi>()
                .PostLoginToTradingPlatform(_tradingPlatformUrl, _clientEmail)
                .GeneralResponse;

            // buy asset
            _apiFactory
                .ChangeContext<ITradePageApi>()
                .PostBuyAssetRequest(_tradingPlatformUrl,
                loginData: _loginCookies, amount: _tradeAmount);

            // set emails for export data
            _exportEmails.Add(_userEmail);

            // set values for Suspicious Pnl in settings
            _apiFactory
                .ChangeContext<IGeneralTabApi>()
                .PutSuspiciousPnlRequest(_crmUrl, _suspiciousPercentage,
                _suspiciousPercentage, _exportEmails.ToArray()); 
            #endregion
        }
        #endregion

        [TearDown]
        public void TearDown()
        {
            try
            {
                // delete Email Suspicious Pnl list
                _apiFactory
                    .ChangeContext<IGeneralTabApi>()
                    .PutSuspiciousPnlRequest(_crmUrl);

                _apiFactory
                    .ChangeContext<ITradeGroupApi>()
                    .DeleteTradeGroupRequest(_crmUrl, _tradeGroupId);
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
        /// create user 
        /// create client
        /// connect client to user
        /// get login cookies
        /// create deposit
        /// buy asset
        /// set admin email in SuspiciousPnl in settings
        /// in settings set SET SUSPICIOUS PROFIT PERCENTAGE to 1
        /// change the crypto group of the user to group with positive sprade
        /// get Suspicions email
        /// set block user Suspicious Pnl in settings
        /// set the Suspicious Profit Sent filed in sql to 0 on DB
        /// get block email 
        /// verify client activation status
        /// buy asset and verify its disable
        /// </summary>        
        [Test]
        [Description("based on jira https://airsoftltd.atlassian.net/browse/AIRV2-4672")]
        [Category(DataRep.SanityCategory)]
        [Category(DataRep.RegressionCategory)]
        [RetryMenage(DataRep.NumOfRetraiesForFallingTest)]
        public void VerifyBlockTradeAndEmailsWhenPnlChangesApiTest()
        {
            var expectedSuspiciosSubject = "Suspicious Profit Of Clients On airsoftltd.com";         

            var expectedSuspiciosBody = $"The following users have passed the suspicious profit" +
                $" threshold of {_suspiciousPercentage}%";

            var expectedBlockSubject = "Clients is block due to Suspicious Profit On";

            var expectedBlockBody = $"Hello, The following users is block due to suspicious" +
                $" profit threshold of {_suspiciousPercentage}%: {_clientName} {_clientName} , ID {_clientId}";

            var expectedActivationStatus = "Block";
            var expectedBlockTradeMessage = "This account is blocked due to suspicious activity and abnormal PL.";

            // Create Cripto Group for Suspicions pnl And Assign It To client
            var groupData = _apiFactory
                .ChangeContext<ISharedStepsGenerator>()
                .CreateTradeGroupAndAssignItToClientPipe(_crmUrl, _tradeGroup, _clientId);

            _tradeGroupId = groupData.Keys.First();
            var newUserEmailForSearch = $"{_userEmail}, risk@airsoftltd.com";

            // get Suspicions email
            var actualSuspiciosEmail = _apiFactory
               .ChangeContext<IPlatformTabApi>()
               .FilterEmailBySubject(_testimUrl, newUserEmailForSearch, expectedSuspiciosSubject)
               .First();

            // delete Email Suspicious Pnl list
            _apiFactory
                .ChangeContext<IGeneralTabApi>()
                .PutSuspiciousPnlRequest(_crmUrl, _suspiciousPercentage,
                _suspiciousPercentage);

            // set block user Suspicious Pnl in settings
            _apiFactory
                .ChangeContext<IGeneralTabApi>()
                .PutSuspiciousPnlRequest(_crmUrl, _suspiciousPercentage,
                _suspiciousPercentage, _exportEmails.ToArray(), true);

            // set the Suspicious Profit Sent filed in sql to 0
            var client =
             (from s in _dbContext.UserAccounts
              where (s.UserId == _clientId)
              select s)
              .First();

            client.SuspiciousProfitSent = false;
            _dbContext.VerifySaveForSqlManipulation();

            // get block email For Admin
            var actualBlockEmailForAdmin = _apiFactory
                .ChangeContext<IPlatformTabApi>()
                .FilterEmailBySubject(_testimUrl, newUserEmailForSearch,
                expectedBlockSubject)
                .First();

            var actualActivationStatus = _apiFactory
                .ChangeContext<IClientsApi>()
                .GetClientByIdRequest(_crmUrl, _clientId)
                .GeneralResponse
                .user
                .activation_status;

            _loginCookies = _apiFactory
                .ChangeContext<ILoginApi>()
                .PostLoginToTradingPlatform(_tradingPlatformUrl, _clientEmail)
                .GeneralResponse;

            // buy asset
            var actualBlockTradeMessage = _apiFactory
                .ChangeContext<ITradePageApi>()
                .PostBuyAssetRequest(_tradingPlatformUrl, _tradeAmount,
                loginData: _loginCookies, checkStatusCode: false)
                .Message;

            Assert.Multiple(() =>
            {
                Assert.True(actualSuspiciosEmail.Subject == expectedSuspiciosSubject,
                    $" expected Suspicious Email Subject: {expectedSuspiciosSubject}" +
                    $" actual Suspicions Email Subject: {actualSuspiciosEmail.Subject}");

                Assert.True(actualSuspiciosEmail.Body.Contains(expectedSuspiciosBody),
                    $" expected Suspicions Email Body not Contains: {expectedSuspiciosBody}" +
                    $" actual Suspicions Email Body not Contains: {actualSuspiciosEmail.Body}");

                Assert.True(actualBlockEmailForAdmin.Subject.Contains(expectedBlockSubject),
                    $" expected block Email Subject: {expectedBlockSubject}" +
                    $" actual block Email Subject: {actualBlockEmailForAdmin.Subject}");

                Assert.True(actualBlockEmailForAdmin.Body.Contains(expectedBlockBody),
                    $" expected block Email Body not Contains: {expectedBlockBody}" +
                    $" actual block Email Body not Contains: {actualBlockEmailForAdmin.Body}");

                Assert.True(actualActivationStatus == expectedActivationStatus,
                    $" expected Activation Status: {expectedActivationStatus}" +
                    $" actual Activation Status: {actualActivationStatus}");

                Assert.True(actualBlockTradeMessage.Contains(expectedBlockTradeMessage),
                    $" expected Block Trade Message: {expectedBlockTradeMessage}" +
                    $" actual Block Trade Message: {actualBlockTradeMessage}");
            });
        }
    }
}