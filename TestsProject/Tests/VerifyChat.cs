using AirSoftAutomationFramework.Internals.DAL.ConfigurationManagement;
using AirSoftAutomationFramework.Internals.ExtensionsMethods;

using AirSoftAutomationFramework.Internals.Helpers;
using AirSoftAutomationFramework.Internals.NunitAttributes;
using NUnit.Framework;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Threading;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.AccountsApi.Roles;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.Clients.ClientPage;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.Settings;
using AirSoftAutomationFramework.Objects.ForexObjects.TradingPlatform.TradeObjects.Ui;
using AirSoftAutomationFramework.Objects.ForexObjects.Ui.Crm.ClientsPage;
using AirSoftAutomationFramework.Objects.ForexObjects.Ui.Crm.ClientsPage.ClientCard;
using AirSoftAutomationFramework.Objects.ForexObjects.Ui.Login;
using AirSoftAutomationFramework.Objects.ForexObjects.Ui.UiGeneral;
using TestsProject.TestsInternals;
using static AirSoftAutomationFramework.Internals.Enums.EnumFactory;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.AccountsApi.UsersMenuApi.UserMenuApi;
using AirSoftAutomationFramework.Internals.Factory;

namespace TestsProject.Tests
{
    //[TestFixture(DataRep.Firefox)]
    [TestFixture(DataRep.Chrome)]
    public class VerifyChat : TestSuitBase
    {
        public VerifyChat(string browser) : base(browser) 
        {
            _browserName = browser;
        }

        private IApplicationFactory _apiFactory = new ApplicationFactory();
        private string _crmUrl = Config.appSettings.CrmUrl;
        private string _clientName;
        private string _userName;
        private string _browserName;
        private IWebDriver _driver;

        [SetUp]
        public void SetUp()
        {
            BeforeTest(_browserName);
            _driver = GetDriver();

            // create user
            _userName = TextManipulation.RandomString();

            var userId = _apiFactory
                .ChangeContext<IUserApi>(_driver)
                .CreateUserForUiPipe(_crmUrl, _userName,
                role: DataRep.AdminWithUsersOnlyRoleName);

            // create ApiKey
            var currentUserApiKey = _apiFactory
                .ChangeContext<IUserApi>(_driver)
                .PostCreateApiKeyRequest(_crmUrl, userId);

            // create client 
            _clientName = TextManipulation.RandomString();

            var clientId = _apiFactory
                .ChangeContext<ICreateClientApi>()
                .CreateClientRequest(_crmUrl, _clientName,
                apiKey: currentUserApiKey);

            var clientsIds = new List<string> { clientId };

            // connect One User To One Client notification
            _apiFactory
                .ChangeContext<IClientsApi>(_driver)
                .PatchMassAssignSaleAgentsRequest(_crmUrl, userId,
                clientsIds);

            _apiFactory
                .ChangeContext<IChatTabApi>(_driver)
                .PatchEnableChatForUserRequest(_crmUrl, userId);
        }

        [TearDown]
        public void TearDown()
        {
            try
            {
            }
            finally
            {
                AfterTest(_driver);
                DriverDispose(_driver);
            }
        }

        [Test]
        [Category(DataRep.SanityCategory)]
        [Category(DataRep.RegressionCategory)]
        [RetryMenage(DataRep.NumOfRetraiesForFallingTest)]
        public void VerifyChatTest()
        {
            var date = DateTime.Now.ToString("M/d/yy");
            var expectedClientMessage = $"ClientMessage {date}";
            var expectedAgentMessage = $"AgentMessage {date}";

            var clientFullName = $"{_clientName.UpperCaseFirstLetter()}" +
                $" {_clientName.UpperCaseFirstLetter()}";

            var userFullName = $"{_userName.UpperCaseFirstLetter()}" +
                $" {_userName.UpperCaseFirstLetter()}";

            // Fast Login to platform
            _apiFactory
                .ChangeContext<ILoginPageUi>(_driver)
                .LoginPipe(_userName)
                .ChangeContext<IMenus>(_driver)
                .ClickOnMenuItem<IClientsPageUi>()
                .SearchClientByEmail(_clientName)
                .ClickOnClientFullName()
                .ClickOnFastLoginBtn();

            // client send message to agent
            _apiFactory
                .ChangeContext<ITradePageUi>(_driver)
                .ClickOnChatBtn()
                .ChangeContext<IChatUi>(_driver)
                .SetChatMessage(expectedClientMessage);

            // switch to crm
            _apiFactory
                .ChangeContext<IGeneral>(_driver)
                .SwitchToExistingWindow(TabToSwitch.First);

            // get the client message
            var actualClientMessage = _apiFactory
                .ChangeContext<IClientCardUi>(_driver)
                .ClickOnCloseBtn()
                .ChangeContext<IChatUi>(_driver)
                .WaitForBadgeRedNotificationNumber()
                .ClickOnChatIcon()
                .ClickOnChatParticipant(clientFullName)
                .GetRevivedChatMessage(expectedClientMessage);

            // agent send message to client
            _apiFactory
                .ChangeContext<IChatUi>(_driver)
                .SetChatMessage(expectedAgentMessage);

            // switch to platform
            _apiFactory
                .ChangeContext<IGeneral>(_driver)
                .SwitchToExistingWindow(TabToSwitch.Last);

            //Thread.Sleep(1000);

            // get the agent message
            var actualAgentMessage = _apiFactory
                .ChangeContext<IChatUi>(_driver)
                .GetRevivedChatMessage(expectedAgentMessage);

            Assert.Multiple(() =>
            {
                Assert.True(actualClientMessage.Contains($"{expectedClientMessage}"),
                    $" expected Client Message: {expectedClientMessage}" +
                    $" actual Client Message: {actualClientMessage}");

                Assert.True(actualAgentMessage.Contains($"{expectedAgentMessage}"),
                    $" expected Agent Message: {expectedAgentMessage}" +
                    $" actual Agent Message: {actualAgentMessage}");
            });
        }
    }
}