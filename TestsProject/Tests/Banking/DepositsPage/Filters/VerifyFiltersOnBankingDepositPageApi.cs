// Ignore Spelling: Api

using AirSoftAutomationFramework.Internals.DAL.ConfigurationManagement;
using AirSoftAutomationFramework.Internals.Helpers;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.AccountsApi.UsersMenuApi.UserMenuApi;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.Banking;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.Clients.ClientCard;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.Clients.ClientCard.FinancesTab;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.Clients.ClientPage;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.Settings;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Trade;
using AirSoftAutomationFramework.Objects.ForexObjects.ApiGeneral;
using AirSoftAutomationFramework.Objects.ForexObjects.Ui.Login;
using NUnit.Framework;
using OpenQA.Selenium;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using AirSoftAutomationFramework.Internals.Factorys;
using AirSoftAutomationFramework.Internals.NunitAtributes;
using TestsProject.TestsInternals;

namespace TestsProject.Tests.Banking.DepositsPage.Filters
{
    [TestFixture]
    public class VerifyFiltersOnBankingDepositPageApi : TestSuitBase
    {
        #region members
        private string _clientEmail;      
        private string _freeText;
        private string _freeText2;
        private string _freeText3;
        private string _freeText4;
        private string _freeText5;
        private string _mainOfficeId;
        private string _userId;
        private string _campaignId;
        private string _banktransferPspId;
        private string _currentUserApiKey;
        private string _tradingPlatformUrl = Config.appSettings.tradingPlatformUrl;
        private string _crmUrl = Config.appSettings.CrmUrl;
        private IApplicationFactory _apiFactory = new ApplicationFactory();
        #endregion      

        [SetUp]
        public void SetUp()
        {
            BeforeTest();
            #region PreCondition
            var depositAmount = 400;

            _mainOfficeId = _apiFactory
                .ChangeContext<IOfficeTabApi>()
                .GetOfficesByName(_crmUrl)
                ._id;

            // create user
            var userName = TextManipulation.RandomString();

            _userId = _apiFactory
                .ChangeContext<IUserApi>()
                .PostCreateUserRequest(_crmUrl, userName);

            #region create ApiKey
            // create ApiKey
            _currentUserApiKey = _apiFactory
                .ChangeContext<IUserApi>()
                .PostCreateApiKeyRequest(_crmUrl, _userId);
            #endregion

            // Create Affiliate And Campaign
            var campaignData = _apiFactory
                .ChangeContext<ISharedStepsGenerator>()
                .CreateAffiliateAndCampaignApi(_crmUrl);

            _campaignId = campaignData.Values.First();

            // create first client with campaign
            var clientName = TextManipulation.RandomString();
            _clientEmail = clientName + DataRep.EmailPrefix;

            _freeText = TextManipulation.RandomString();
            _freeText2 = $"{_freeText}_2";
            _freeText3 = $"{_freeText}_3";
            _freeText4 = $"{_freeText}_4";
            _freeText5 = $"{_freeText}_5";

            var firstClientsId = _apiFactory
                .ChangeContext<ICreateClientApi>()
                .CreateClientWithCampaign(_crmUrl, clientName,
                campaignId: _campaignId, freeText: _freeText,
                apiKey: _currentUserApiKey,
                freeText2: _freeText2, freeText3: _freeText3,
                freeText4: _freeText4, freeText5: _freeText5);

            // create deposit
            _apiFactory
                .ChangeContext<IFinancesTabApi>()
                .PostDepositRequest(_crmUrl, firstClientsId, depositAmount,
                apiKey: _currentUserApiKey);

            // connect One User To One Client notification
            _apiFactory
                .ChangeContext<IClientsApi>()
                .PatchMassAssignSaleAgentsRequest(_crmUrl, _userId,
                new List<string> { firstClientsId }, apiKey: _currentUserApiKey);

            _banktransferPspId = _apiFactory
                .ChangeContext<IPspTabApi>()
                .GetPspInstancesByNameRequest("bank-transfer")
                .FirstOrDefault()
                .Id;
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
        public void VerifyFiltersOnBankingDepositPageApiTest()
        {
            var filters = new Dictionary<string, string> {
                { "method[]", "Bank Transfer: bank_transfer" },
                { "erp_user_id_assigned[]", _userId },
                { "sales_agents[]", _userId },
                { "countries[]", DataRep.UserDefaultCountry },
                { "currency[]", DataRep.DefaultUSDCurrencyName },
                { "free_text[]", _freeText },
                { "free_text_2[]", _freeText2 },
                { "free_text_3[]", _freeText3},
                { "free_text_4[]", _freeText4 },
                { "free_text_5[]", _freeText5 },
                { "campaign_id[]", _campaignId },
                { "office[]", _mainOfficeId },
                { "psp_instance_name[]", DataRep.BankTransferPspName },
                { "psp_instance_id[]", _banktransferPspId },
                { "status[]", "approved" },
                { "ftd", "1" },
            };

            var actualClientByFilter = _apiFactory
                .ChangeContext<IDepositsPageApi>()
                .GetDepositByFiltersRequest(_crmUrl, filters, _currentUserApiKey)
                .GeneralResponse
                .data;

            Assert.Multiple(() =>
            {
                Assert.True(actualClientByFilter.First().email == _clientEmail,
                    $" expected Client By Filter email: {_clientEmail}" +
                    $" actual Client By Filter email :  {actualClientByFilter.First().email}");

                Assert.True(actualClientByFilter.Count() == 1,
                    $" expected num of clients: {1}" +
                    $" actual num of clients :  {actualClientByFilter.Count()}");
            });
        }
    }
}
