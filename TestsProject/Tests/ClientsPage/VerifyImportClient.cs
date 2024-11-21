using System.Linq;
using AirSoftAutomationFramework.Internals.DAL.ConfigurationManagement;
using AirSoftAutomationFramework.Internals.DAL.ExcelAccess;
using AirSoftAutomationFramework.Internals.Factorys;
using AirSoftAutomationFramework.Internals.Helpers;
using AirSoftAutomationFramework.Internals.NunitAtributes;
using AirSoftAutomationFramework.Objects.DTOs;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.AccountsApi.UsersMenuApi.UserMenuApi;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.Clients.ClientPage;
using AirSoftAutomationFramework.Objects.ForexObjects.Ui.Crm.ClientsPage;
using AirSoftAutomationFramework.Objects.ForexObjects.Ui.Login;
using AirSoftAutomationFramework.Objects.ForexObjects.Ui.UiGeneral;
using NUnit.Framework;
using OpenQA.Selenium;
using TestsProject.TestsInternals;

namespace TestsProject.Tests.ClientsPage
{
    [TestFixture(DataRep.Chrome)]
    public class VerifyImportClient : TestSuitBase
    {
        #region Test Preparation
        public VerifyImportClient(string browser) : base(browser)
        {
            _browserName = browser;
        }

        private IApplicationFactory _apiFactory = new ApplicationFactory();
        private string _crmUrl = Config.appSettings.CrmUrl;
        private string _mailPerfix = DataRep.EmailPrefix;
        private ImportClientRequest _importClient = new ImportClientRequest();
        private string _browserName;
        private string _newClientEmail;
        private string _newClientName;
        private string _expectedClientCurrency = DataRep.DefaultUSDCurrencyName;
        private string _expectedClientCountry = "Italy";
        private string _expectedClientNote = "Automation note";
        private IWebDriver _driver;

        [SetUp]
        public void SetUp()
        {
            BeforeTest(_browserName);
            _driver = GetDriver();

            // create user
            var userName = TextManipulation.RandomString();

            _apiFactory
                .ChangeContext<IUserApi>(_driver)
                .CreateUserForUiPipe(_crmUrl, userName);

            _apiFactory
                .ChangeContext<ILoginPageUi>(_driver)
                .LoginPipe(userName);

            // create client 
            var clientName = TextManipulation.RandomString();
            var clientEmail = clientName + DataRep.EmailPrefix;

            _apiFactory
                .ChangeContext<ICreateClientApi>()
                .CreateClientRequest(_crmUrl, clientName);

            var newClientData = _apiFactory
               .ChangeContext<IClientsApi>(_driver)
               .GetClientRequest(_crmUrl, clientEmail)
               .GeneralResponse
               .data
               .FirstOrDefault();

            _newClientName = 
                $"New{newClientData.full_name.Split(" ").First()}";

            _importClient.LastName = _newClientName;
            _importClient.FirstName = _newClientName;
            _importClient.PhoneNumber = newClientData.phone;
            _newClientEmail = $"{_newClientName}@auto.local";
            _importClient.EMail = _newClientEmail;
            _importClient.Currency = _expectedClientCurrency;
            _importClient.CountryIsoCodeId = "IT";
            //_importClient.ClientPaysId = _expectedClientCountry;
            _importClient.Note = _expectedClientNote;
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
        #endregion

        [Test]
        [Category(DataRep.SanityCategory)]
        [Category(DataRep.RegressionCategory)]
        [RetryMenage(DataRep.NumOfRetriesForFallingTest)]
        public void VerifyImportClientTest()
        {
            var expectedClientFullName = _newClientEmail.Split('@').First();
            var filePath = ExcelHelper.CreateCsvFile(_importClient, "import.csv");

            _apiFactory
                .ChangeContext<IMenus>(_driver)
                .ClickOnMenuItem<IClientsPageUi>() 
                .ClickOnImportButton()
                .UploadCsvFile(filePath)
                .ClickOnUploadFileButton();

            var newClientId = _apiFactory
                .ChangeContext<IClientsApi>(_driver)
                .GetClientRequest(_crmUrl, _newClientEmail)
                .GeneralResponse
                .data
                .FirstOrDefault()
                ._id;

            var clientData = _apiFactory
                .ChangeContext<IClientsApi>(_driver)
                .GetClientByIdRequest(_crmUrl, newClientId)
                .GeneralResponse
                .user;

            Assert.Multiple(() =>
            {
                Assert.True(clientData.first_name == $"{_newClientName.ToLower()}",
                    $" expected first name: {_newClientName} {_newClientName}" +
                    $" actual first name: {clientData.first_name}");

                Assert.True(clientData.email == _newClientEmail.ToLower(),
                    $" expected email: {_newClientEmail}" +
                    $" actual email: {clientData.email}");

                Assert.True(clientData.country == _expectedClientCountry.ToLower(),
                    $" expected country: {_expectedClientCountry}" +
                    $" actual country: {clientData.country}");

                Assert.True(clientData.note == _expectedClientNote,
                    $" expected note: {_expectedClientNote}" +
                    $" actual note: {clientData.note}");

                Assert.True(clientData.currency_code == _expectedClientCurrency,
                    $" expected Currency: {_expectedClientCurrency}" +
                    $" actual Currency: {clientData.currency_code}");
            });
        }
    }
}
