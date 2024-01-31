// Ignore Spelling: Api

using AirSoftAutomationFramework.Internals.DAL.ConfigurationManagement;
using AirSoftAutomationFramework.Internals.DAL.ExcelAccess;
using AirSoftAutomationFramework.Internals.Helpers;
using AirSoftAutomationFramework.Objects.DTOs;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.AccountsApi.UsersMenuApi.UserMenuApi;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.Clients.ClientPage;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.sales;
using AirSoftAutomationFramework.Objects.ForexObjects.ApiGeneral;
using AirSoftAutomationFramework.Objects.ForexObjects.Ui.Crm.ClientsPage;
using AirSoftAutomationFramework.Objects.ForexObjects.Ui.Login;
using AirSoftAutomationFramework.Objects.ForexObjects.Ui.UiGeneral;
using NUnit.Framework;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using AirSoftAutomationFramework.Internals.Factorys;
using AirSoftAutomationFramework.Internals.NunitAtributes;
using TestsProject.TestsInternals;

namespace TestsProject.Tests.ClientsPage.AttributionRoles
{
    [NonParallelizable]
    [TestFixture(DataRep.Chrome)]
    public class VerifySplitTypeEquallyWhenImportClientsApi : TestSuitBase
    {
        #region Test Preparation
        public VerifySplitTypeEquallyWhenImportClientsApi(string browser) : base(browser)
        {
            _browserName = browser;
        }

        private IApplicationFactory _apiFactory = new ApplicationFactory();
        private string _crmUrl = Config.appSettings.CrmUrl;
        private string _mailPerfix = DataRep.EmailPrefix;
        private ImportClientRequest _importClient = new ImportClientRequest();
        private string _browserName;
        private string _firstFtdSalesAgentId;
        private string _secondFtdSalesAgentId;
        private string _attributionRoleName = "CountryItaly";
        private string _filePath;
        private List<ImportClientRequest> _clientsData;
        private IWebDriver _driver;

        [SetUp]
        public void SetUp()
        {
            BeforeTest(_browserName);
            _driver = GetDriver();
            var country = "italy";

            var userName = TextManipulation.RandomString();

            var userId = _apiFactory
                .ChangeContext<IUserApi>(_driver)
                .CreateUserForUiPipe(_crmUrl, userName);

            // Create Affiliate And Campaign
            var campaignData = _apiFactory
                .ChangeContext<ISharedStepsGenerator>(_driver)
                .CreateAffiliateAndCampaignApi(_crmUrl);

            var campaignId = campaignData.Values.First();

            // create first ftd user
            var firstFtdUserName = TextManipulation.RandomString();

            // create first Ftd Sales Agent
            _firstFtdSalesAgentId = _apiFactory
                .ChangeContext<IUserApi>()
                .PostCreateUserRequest(_crmUrl, firstFtdUserName, salesType: "ftd");

            var secondFtdUserName = TextManipulation.RandomString();

            // create second Ftd Sales Agent
            _secondFtdSalesAgentId = _apiFactory
                .ChangeContext<IUserApi>()
                .PostCreateUserRequest(_crmUrl, secondFtdUserName, salesType: "ftd");

            // get Attribution Role By Name
            var attributionRoleByName = _apiFactory
               .ChangeContext<IClientsApi>(_driver)
               .GetAttributionRolesRequest(_crmUrl)
               .GeneralResponse
               .Where(p => p.name == _attributionRoleName)
               .ToList();

            if (attributionRoleByName != null)
            {
                _apiFactory
                    .ChangeContext<IClientsApi>(_driver)
                    .DeleteAttributionRolesRequest(_crmUrl, attributionRoleByName);
            }

            // create attribution role for at first deposit
            _apiFactory
                .ChangeContext<IClientsApi>(_driver)
                .PostCreateAttributionRoleRequest(_crmUrl, _attributionRoleName,
                actualType: "country", countryNames: new string[] { country },
                ftdAgentIds: new string[] { _firstFtdSalesAgentId, _secondFtdSalesAgentId },
                actualSplit: "equal");

            _apiFactory
                .ChangeContext<ILoginPageUi>(_driver)
                .LoginPipe(userName);

            _clientsData = _apiFactory
               .ChangeContext<IClientsApi>(_driver)
               .CreateClientDataForImport(4);

            _filePath = ExcelHelper.CreateCsvFile(_clientsData, "import.csv");
        }
        #endregion

        [TearDown]
        public void TearDown()
        {
            try
            {

                // delete attribution role
                var attributionRoleByName = _apiFactory
                   .ChangeContext<IClientsApi>(_driver)
                   .GetAttributionRolesRequest(_crmUrl)
                   .GeneralResponse
                   .Where(p => p.name == _attributionRoleName)
                   .ToList();

                _apiFactory
                   .ChangeContext<IClientsApi>(_driver)
                   .DeleteAttributionRolesRequest(_crmUrl, attributionRoleByName);
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

        /// <summary>
        /// create 2 users type FTD
        /// create attribution role type country italy with the two users split equal
        /// import 4 clients
        /// verify each ftd user gets 2 clients
        /// </summary>
        [Test]
        [Category(DataRep.ApiDocCategory)]
        [Category(DataRep.SanityCategory)]
        [Category(DataRep.RegressionCategory)]
        [RetryMenage(DataRep.NumOfRetraiesForFallingTest)]
        public void VerifySplitTypeEquallyWhenImportClientsApiTest()
        {
            var expectedSalesAgents = new List<string>();
            var actualNumOfCustomersFirstFtdSalesAgent = 0;
            var actualNumOfCustomersSecondFtdSalesAgent = 0;

            var clientDataUi = _apiFactory
                .ChangeContext<IMenus>(_driver)
                .ClickOnMenuItem<IClientsPageUi>()
                .ClickOnImportButton()
                .UploadCsvFile(_filePath)
                .ClickOnUploadFileButton();


            actualNumOfCustomersFirstFtdSalesAgent = _apiFactory
                .ChangeContext<ISalesPageApi>(_driver)
                .GetAgentProfileInfoRequest(_crmUrl, _firstFtdSalesAgentId)
                .GeneralResponse
                .Customers
                .Total;


            actualNumOfCustomersSecondFtdSalesAgent = _apiFactory
                .ChangeContext<ISalesPageApi>(_driver)
                .GetAgentProfileInfoRequest(_crmUrl, _secondFtdSalesAgentId)
                .GeneralResponse
                .Customers
                .Total;

            Assert.Multiple(() =>
            {
                Assert.IsTrue(actualNumOfCustomersFirstFtdSalesAgent == 2,
                    $" actual Num Of Customers First Ftd Sales Agent : {actualNumOfCustomersFirstFtdSalesAgent} " +
                    $" expected Num Of Customers First Ftd Sales Agent equals 2");

                Assert.IsTrue(actualNumOfCustomersSecondFtdSalesAgent == 2,
                    $" actual Num Of Customers First Ftd Sales Agent : {actualNumOfCustomersSecondFtdSalesAgent} " +
                    $" expected Num Of Customers First Ftd Sales Agent equals 2");
            });
        }
    }
}