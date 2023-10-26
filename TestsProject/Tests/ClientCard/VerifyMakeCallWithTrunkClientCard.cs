using AirSoftAutomationFramework.Internals.DAL.ConfigurationManagement;
using AirSoftAutomationFramework.Internals.Factory;
using AirSoftAutomationFramework.Internals.Helpers;
using AirSoftAutomationFramework.Internals.NunitAttributes;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.AccountsApi.UsersMenuApi;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.AccountsApi.UsersMenuApi.UserMenuApi;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.Clients.ClientPage;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.Settings;
using AirSoftAutomationFramework.Objects.ForexObjects.Ui.Crm.ClientsPage;
using AirSoftAutomationFramework.Objects.ForexObjects.Ui.Crm.ClientsPage.ClientCard;
using AirSoftAutomationFramework.Objects.ForexObjects.Ui.Login;
using AirSoftAutomationFramework.Objects.ForexObjects.Ui.UiGeneral;
using NUnit.Framework;
using OpenQA.Selenium;
using System;
using TestsProject.TestsInternals;

namespace TestsProject.Tests.ClientCard
{
    [TestFixture(DataRep.Chrome)]
    public class VerifyMakeCallWithTrunkClientCard : TestSuitBase
    {
        public VerifyMakeCallWithTrunkClientCard(string browser) : base(browser)
        {
            _browserName = browser;
        }

        private IApplicationFactory _apiFactory = new ApplicationFactory();
        private string _crmUrl = Config.appSettings.CrmUrl;
        private string _userId;
        private string _officeId;
        private string _officeName;
        private string _browserName;
        private IWebDriver _driver;

        [SetUp]
        public void SetUp()
        {
            BeforeTest();
            #region PreCondition   
            _driver = GetDriver();
            _officeName = TextManipulation.RandomString();
            var pbxName = TextManipulation.RandomString();

            // create office
            var officeData = _apiFactory
                .ChangeContext<IOfficeTabApi>(_driver)
                .PostCreateOfficeRequest(_crmUrl, _officeName, pbxName)
                .GetOfficesByName(_crmUrl, _officeName);

            _officeId = officeData._id;
            var userName = TextManipulation.RandomString();

            _userId = _apiFactory
                .ChangeContext<IUserApi>(_driver)
                .CreateUserForUiPipe(_crmUrl, userName,
                pbxName: pbxName,
                role: DataRep.AdminWithUsersOnlyRoleName, officeData: officeData);

            // get user ApiKey
            var userApiKey = _apiFactory
                .ChangeContext<IUserApi>()
                .PostCreateApiKeyRequest(_crmUrl, _userId);

            _apiFactory
                .ChangeContext<ILoginPageUi>(_driver)
                .LoginPipe(userName);

            //var user = _apiFactory
            //   .ChangeContext<IUsersApi>(_driver)
            //   .GetUserByIdRequest(_crmUrl, _userId)
            //   .GeneralResponse;

            //user.user.office = officeId;

            //// assign new office to user
            //_apiFactory
            //   .ChangeContext<IUserApi>(_driver)
            //   .PutEditUserOfficeRequest(_crmUrl, user);

            // create client 
            var clientName = TextManipulation.RandomString();

            _apiFactory
                .ChangeContext<ICreateClientApi>()
                .CreateClientRequest(_crmUrl, clientName, apiKey: userApiKey);

            // create and get trunk created
            _apiFactory
                .ChangeContext<IOfficeTabApi>(_driver)
                .PostCreateTrunkPipe(_crmUrl, officeId: _officeId, pbxName: pbxName);

            _apiFactory
                .ChangeContext<IMenus>(_driver)
                .ClickOnMenuItem<IClientsPageUi>()
                .SearchClientByEmail(clientName)
                .ClickOnClientFullName(clientName);
            #endregion
        }

        [TearDown]
        public void TearDown()
        {
            try
            {
                var homeOfficeId = _apiFactory
                    .ChangeContext<IOfficeTabApi>(_driver)
                    .GetOfficesByName(_crmUrl, DataRep.MainOfficeName)
                    ._id;

                var user = _apiFactory
                    .ChangeContext<IUsersApi>(_driver)
                    .GetUserByIdRequest(_crmUrl, _userId)
                    .GeneralResponse;

                user.user.office = homeOfficeId;

                _apiFactory
                    .ChangeContext<IUserApi>(_driver)
                    .PutEditUserOfficeRequest(_crmUrl, user);

                // delete trunk and office
                _apiFactory
                   .ChangeContext<IOfficeTabApi>(_driver)
                   .DeleteTrunkByIdRequest(_crmUrl, _officeId);
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

        // based on ticket https://airsoftltd.atlassian.net/browse/AIRV2-4812
        // create client 
        // verify Internal Server Error message for both phones
        [Test]
        [Category(DataRep.SanityCategory)]
        [Category(DataRep.RegressionCategory)]
        [RetryMenage(DataRep.NumOfRetraiesForFallingTest)]
        public void VerifyMakeCallWithTrunkClientCardTest()
        {
            _apiFactory
                .ChangeContext<IClientCardUi>(_driver)
                .ClickOnPhoneIconButton()
                //.ChooseTrunk(_officeName)
                .VerifyPhoneCallIcon();         
        }
    }
}