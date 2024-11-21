// Ignore Spelling: Api

using AirSoftAutomationFramework.Internals.DAL.ConfigurationManagement;
using AirSoftAutomationFramework.Internals.ExtensionsMethods;
using AirSoftAutomationFramework.Internals.Helpers;
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
using System.Threading;
using AirSoftAutomationFramework.Internals.Factorys;
using AirSoftAutomationFramework.Internals.NunitAtributes;
using TestsProject.TestsInternals;

namespace TestsProject.Tests.TablesValues
{
    [TestFixture]
    public class VerifyClientsTableValuesApi : TestSuitBase
    {
        #region Test Preparation

        private IApplicationFactory _apiFactory = new ApplicationFactory();
        private string _crmUrl = Config.appSettings.CrmUrl;
        private string _tradingPlatformUrl = Config.appSettings.tradingPlatformUrl;
        private string _clientEmail;
        private string _clientId;
        private string _userName;
        private string _subCampaignName;
        private string _expectedPhone = DataRep.UserDefaultPhone;
        private string _currentUserApiKey;
        private int _expectedTransferToSavingAccount = 100;
        private int _depositAmount = 10000;
        private int _bonusAmount = 10000;
        private DateTime _expectedAttributionDate = DateTime.Today;

        [SetUp]
        public void SetUp()
        {
            BeforeTest();
            #region PreCondition
            var crmUrl = Config.appSettings.CrmUrl;

            // create user
            _userName = TextManipulation.RandomString();

            var userId = _apiFactory
                .ChangeContext<IUserApi>()
                .PostCreateUserRequest(_crmUrl, _userName,
                role: DataRep.AdminWithUsersOnlyRoleName);
            #endregion

            #region create ApiKey
            // create ApiKey
            _currentUserApiKey = _apiFactory
                .ChangeContext<IUserApi>()
                .PostCreateApiKeyRequest(_crmUrl, userId);
            #endregion

            // create first campaign and Affiliate
            var campaignData = _apiFactory
                .ChangeContext<ISharedStepsGenerator>()
                .CreateAffiliateAndCampaignApi(_crmUrl);

            var campaignId = campaignData.Values.First();

            // create client 
            var clientName = TextManipulation.RandomString();
            _clientEmail = clientName + DataRep.EmailPrefix;
            _subCampaignName = TextManipulation.RandomString();

            _clientId = _apiFactory
                .ChangeContext<ICreateClientApi>()
                .CreateClientWithCampaign(_crmUrl, clientName,
                campaignId: campaignId, apiKey: _currentUserApiKey,
                subCampaign: _subCampaignName);

            // deposit 10000
            #region deposit 10000
            _apiFactory
                .ChangeContext<IFinancesTabApi>()
                .PostDepositRequest(crmUrl, _clientId, _depositAmount);

            // create bonus
            _apiFactory
                .ChangeContext<IFinancesTabApi>()
                .PostBonusRequest(crmUrl, _clientId, _bonusAmount,
                apiKey: _currentUserApiKey);

            // CRM Transfer To Saving Account
            _apiFactory
                .ChangeContext<ISATabApi>()
                .PostTransferToSavingAccountRequest(_crmUrl, _clientId,
                _expectedTransferToSavingAccount, _currentUserApiKey);

            Thread.Sleep(500); // wait for saving account to update

            // create comment
            _apiFactory
                .ChangeContext<IPlanningTabApi>()
                .PostCreateAddCommentRequest(_crmUrl, _clientId, _currentUserApiKey);

            // create last comment
            _apiFactory
               .ChangeContext<IPlanningTabApi>()
               .PostCreateAddCommentRequest(_crmUrl, _clientId, _currentUserApiKey);

            var informationTabResponse = _apiFactory
                .ChangeContext<IInformationTabApi>()
                .GetInformationTabRequest(_crmUrl, _clientId)
                .GeneralResponse
                .informationTab;

            informationTabResponse.note = "Automation";
            informationTabResponse.phone_2 = _expectedPhone;
            informationTabResponse.attribution_date = _expectedAttributionDate;

            // update client card
            _apiFactory
                .ChangeContext<IInformationTabApi>()
                .PutInformationTabRequest(_crmUrl, informationTabResponse, _currentUserApiKey);
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
        [RetryMenage(DataRep.NumOfRetriesForFallingTest)]
        public void VerifyClientsTableValuesApiTest()
        {
            var date = DateTime.Now.ToString("dd/MM/yyyy");
            var expectedBalance = _depositAmount + _bonusAmount - _expectedTransferToSavingAccount;

            // ####
            // if adding column to the table we must add the two object
            // ###

            // get client data from table
            var clientData = _apiFactory
                .ChangeContext<IClientsApi>()
                .GetAllClientsRequest(_crmUrl)
                .clientData
                .Where(p => p._id == _clientId)
                .FirstOrDefault();

            for (var i = 0; i < 10; i++)
            {
                if (clientData.balance == "0" || clientData.bonus == 0)
                {
                    Thread.Sleep(1000);

                    clientData = _apiFactory
                        .ChangeContext<IClientsApi>()
                        .GetAllClientsRequest(_crmUrl)
                        .clientData
                        .Where(p => p._id == _clientId)
                        .FirstOrDefault();

                    continue;
                }

                break;
            }

            // get client data from client card
            var clientCardData = _apiFactory
                .ChangeContext<IClientsApi>()
                .GetClientByIdRequest(_crmUrl, _clientId)
                .GeneralResponse
                .user;

            // client Table Columns names
            var clientTableColumns = _apiFactory
                .ChangeContext<IClientsApi>()
                .GetClientTableColumns(_crmUrl, _clientEmail);

            // remove the columns that have diffident format
            var actualDiff = clientData.CompareTwoObjects(clientCardData, clientTableColumns);
            actualDiff.Remove("balance");
            actualDiff.Remove("sales_agent");

            Assert.Multiple(() =>
            {
                Assert.True(actualDiff.Count == 0,
                    $" this columns don't contains the values: {actualDiff.ListToString()}");

                Assert.True(clientData.balance == $"{expectedBalance}.00 $",
                    $" expected balance: {expectedBalance}.00 $" +
                    $" actual balance: {clientData.balance}");

                Assert.True(clientData.sales_agent == _userName,
                    $" expected sales agent: {_userName}" +
                    $" actual sales agent: {clientData.sales_agent}");
            });
        }
    }
}