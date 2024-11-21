// Ignore Spelling: Api

using AirSoftAutomationFramework.Internals.DAL.ConfigurationManagement;
using AirSoftAutomationFramework.Internals.DAL.ExcelAccess;
using AirSoftAutomationFramework.Internals.Helpers;
using AirSoftAutomationFramework.Objects.DTOs;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.AccountsApi.UsersMenuApi.UserMenuApi;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.CampaignsApi.Dashboard;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.Clients.ClientPage;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.Settings;
using AirSoftAutomationFramework.Objects.ForexObjects.ApiGeneral;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using AirSoftAutomationFramework.Internals.Factorys;
using AirSoftAutomationFramework.Internals.NunitAtributes;
using TestsProject.TestsInternals;

namespace TestsProject.Tests.Emails
{
    [TestFixture]
    public class VerifyCampaignCapErrorAndEmailForImportClientApi : TestSuitBase
    {
        #region Test Preparation
        private IApplicationFactory _apiFactory = new ApplicationFactory();
        private string _crmUrl = Config.appSettings.CrmUrl;
        private string _currentUserApiKey;
        private string _campaignId;
        private string _campaignName;
        private string _clientName;
        private List<string> _clientNames;
        private int _leadsLimit = 1;
        private string _testimUrl = DataRep.TesimUrl;
        private string _timeFrame = "Daily";
        private string _firstCountryNameForLimitation = "algeria";
        private string _secondCountryNameForLimitation = "andorra";
        private ImportClientRequest _importFirstClient = new ImportClientRequest();
        private ImportClientRequest _importSecondClient = new ImportClientRequest();
        private ImportClientRequest _importThirdClient = new ImportClientRequest();
        private ImportClientRequest _importForthClient = new ImportClientRequest();
        private string _expectedClientNote = "Automation note";
        private string _expectedClientCurrency = DataRep.DefaultUSDCurrencyName;
        private string _newFirstClientEmail;
        private string _userEmail;
        private string _newSecondClientEmail;
        private string _newThirdClientEmail;
        private string _newForthClientEmail;
        private string _newFirstClientName;
        private string _newSecondClientName;
        private string _newThirdClientName;
        private string _newForthClientName;

        [SetUp]
        public void SetUp()
        {
            BeforeTest();

            var userName = TextManipulation.RandomString();
            _userEmail = userName + DataRep.TestimEmailPrefix;

            // create admin for cup
            var userId = _apiFactory
                .ChangeContext<IUserApi>()
                .PostCreateUserRequest(_crmUrl, userName,
                emailPrefix: DataRep.TestimEmailPrefix,
                role: DataRep.AdminWithUsersOnlyRoleName);

            // user ApiKey
            _currentUserApiKey = _apiFactory
                .ChangeContext<IUserApi>()
                .PostCreateApiKeyRequest(_crmUrl, userId);

            // create first campaign and Affiliate
            var campaignData = _apiFactory
                .ChangeContext<ISharedStepsGenerator>()
                .CreateAffiliateAndCampaignApi(_crmUrl);

            _campaignId = campaignData.Values.First();
            var affiliateId = campaignData.Values.Last();

            // get Campaign Data
            var getCampaignData = _apiFactory
                .ChangeContext<ICampaignPageApi>()
                .GetCampaignByIdRequest(_crmUrl, _campaignId);

            _campaignName = getCampaignData.Name;
            _campaignId = getCampaignData.Id;

            #region add limitation to the campaign
            var limitation = new List<Limitation>();

            limitation.Add(new Limitation()
            {
                country = _firstCountryNameForLimitation,
                timeframe = _timeFrame,
                leads_num = _leadsLimit
            });

            limitation.Add(new Limitation()
            {
                country = _secondCountryNameForLimitation,
                timeframe = _timeFrame,
                leads_num = _leadsLimit
            });

            getCampaignData.cap.email = _userEmail;
            getCampaignData.cap.limitations = limitation;
            #endregion

            // update Campaign 
            _apiFactory
                .ChangeContext<ICampaignPageApi>()
                .PutCampaignByIdRequest(_crmUrl, getCampaignData, affiliateId);

            // create client 
            var clientName = TextManipulation.RandomString();
            var clientEmail = clientName + DataRep.EmailPrefix;

            _apiFactory
                .ChangeContext<ICreateClientApi>()
                .CreateClientRequest(_crmUrl, clientName);

            var newClientData = _apiFactory
               .ChangeContext<IClientsApi>()
               .GetClientRequest(_crmUrl, clientEmail)
               .GeneralResponse
               .data
               .FirstOrDefault();

            _newFirstClientName =
                $"First{newClientData.full_name.Split(" ").First()}";

            _importFirstClient.LastName = _newFirstClientName;
            _importFirstClient.FirstName = _newFirstClientName;
            _importFirstClient.PhoneNumber = newClientData.phone;
            _newFirstClientEmail = $"{_newFirstClientName}@auto.local";
            _importFirstClient.EMail = _newFirstClientEmail;
            _importFirstClient.Currency = _expectedClientCurrency;
            _importFirstClient.CountryIsoCodeId = "DZ";
            _importFirstClient.Note = _expectedClientNote;

            _newSecondClientName =
                $"Second{newClientData.full_name.Split(" ").First()}";

            _importSecondClient.LastName = _newSecondClientName;
            _importSecondClient.FirstName = _newSecondClientName;
            _importSecondClient.PhoneNumber = newClientData.phone;
            _newSecondClientEmail = $"{_newSecondClientName}@auto.local";
            _importSecondClient.EMail = _newSecondClientEmail;
            _importSecondClient.Currency = _expectedClientCurrency;
            _importSecondClient.CountryIsoCodeId = "AD";
            _importSecondClient.Note = _expectedClientNote;

            _newThirdClientName =
                $"Third{newClientData.full_name.Split(" ").First()}";

            _importThirdClient.LastName = _newThirdClientName;
            _importThirdClient.FirstName = _newThirdClientName;
            _importThirdClient.PhoneNumber = newClientData.phone;
            _newThirdClientEmail = $"{_newThirdClientName}@auto.local";
            _importThirdClient.EMail = _newThirdClientEmail;
            _importThirdClient.Currency = _expectedClientCurrency;
            _importThirdClient.CountryIsoCodeId = "DZ";
            _importThirdClient.Note = _expectedClientNote;

            _newForthClientName =
                $"Forth{newClientData.full_name.Split(" ").First()}";

            _importForthClient.LastName = _newForthClientName;
            _importForthClient.FirstName = _newForthClientName;
            _importForthClient.PhoneNumber = newClientData.phone;
            _newForthClientEmail = $"{_newForthClientName}@auto.local";
            _importForthClient.EMail = _newForthClientEmail;
            _importForthClient.Currency = _expectedClientCurrency;
            _importForthClient.CountryIsoCodeId = "AD";
            _importForthClient.Note = _expectedClientNote;

            _clientNames = new List<string>() { _newFirstClientName, _newSecondClientName };
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

        [Test]
        //[Ignore("under develop")]
        [Category(DataRep.SanityCategory)]
        [Category(DataRep.RegressionCategory)]
        [RetryMenage(DataRep.NumOfRetriesForFallingTest)]
        public void VerifyCampaignCapErrorAndEmailForImportClientApiTest()
        {
            var expectedEmailBody = "  Import Leads Report " +
                "    Total leads imported successfully: 2 " +
                "  The following rows had errors:     Row 3  Campaign limit CAP Row 4  Campaign limit CAP    ";

            var fileName = "import.csv";

            var filePath = ExcelHelper.CreateCsvFile(new List<ImportClientRequest>
            { _importFirstClient, _importSecondClient, _importThirdClient,
                _importForthClient}, fileName);

            // update the existing template
            _apiFactory
               .ChangeContext<IClientsApi>()
               .PostImportLeadsRequest(_crmUrl, fileName,
               filePath, _campaignId, _currentUserApiKey)
               .WaitForClientToCreate(_crmUrl, _clientNames);

            // get admin email body
            var actualEmailBody = _apiFactory
                .ChangeContext<IPlatformTabApi>()
                .FilterEmailBySubject(_testimUrl,
                _userEmail, "Import Clients Report")
                .FirstOrDefault()
                .Body;

            Assert.Multiple(() =>
            {
                Assert.True(actualEmailBody.Equals(expectedEmailBody),
                    $" actual Email Body: {actualEmailBody}" +
                    $" expected Body: {expectedEmailBody}");               
            });
        }
    }
}