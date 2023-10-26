// Ignore Spelling: Api Ftd

using System.Collections.Generic;
using System.Linq;
using AirSoftAutomationFramework.Internals.DAL.ConfigurationManagement;
using AirSoftAutomationFramework.Internals.Factory;
using AirSoftAutomationFramework.Internals.Helpers;
using AirSoftAutomationFramework.Internals.NunitAttributes;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.AccountsApi.UsersMenuApi.UserMenuApi;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.CampaignsApi.Dashboard;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.Clients.ClientCard.FinancesTab;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.Clients.ClientPage;
using AirSoftAutomationFramework.Objects.ForexObjects.ApiGeneral;
using NUnit.Framework;
using TestsProject.TestsInternals;

namespace TestsProject.Tests.Affiliate
{
    [NonParallelizable]
    [TestFixture("no permissions", 400, "000Z", 500)]
    [TestFixture("all permissions", 400, "000Z", 500)]
    public class VerifyAffiliateRolesForDepositByCampaignIdApi : TestSuitBase
    {
        #region Test Preparation       
        private IApplicationFactory _apiFactory = new ApplicationFactory();
        private string _crmUrl = Config.appSettings.CrmUrl; private int _firstDepositAmount = 400;
        private IDictionary<string, string> _campaignData;
        private int _secondDepositAmount = 500;
        private string _campaignId;
        private List<string> _clientsIds;
        private string _clientName;
        private string _affiliateId;
        private string _depositId;
        private string _affiliateApiKey;
        private string _description;
        private int _expectedFtdAmount;
        private string _expectedDepositDate;
        private int _expectedSecondDpositAmount;

        public VerifyAffiliateRolesForDepositByCampaignIdApi(string description, int expectedFtdAmount,
            string expectedDepositDate, int expectedSecondDepositAmount) 
        {
            _description = description;
            _expectedFtdAmount = expectedFtdAmount;
            _expectedDepositDate = expectedDepositDate;
            _expectedSecondDpositAmount = expectedSecondDepositAmount;
        }

        [SetUp]
        public void SetUp()
        {
            BeforeTest();
            #region PreCondition

            if (_description == "no permissions")
            {
                // create affiliate role with no permissions
                _apiFactory
                    .ChangeContext<ICreateEditRoleApi>()
                    .PostAffiliateRoleWithNoPermissionsRequest(_crmUrl);

                // Create Affiliate And Campaign
                _campaignData = _apiFactory
                    .ChangeContext<ISharedStepsGenerator>()
                    .CreateAffiliateAndCampaignApi(_crmUrl,
                    roleName: DataRep.AffiliateWithNoPermissionRole);
            }
            else
            {
                // create affiliate role with all permissions
                _apiFactory
                    .ChangeContext<ICreateEditRoleApi>()
                    .PostAffiliateRoleWithAllPermissionsRequest(_crmUrl);

                // Create Affiliate And Campaign
                _campaignData = _apiFactory
                    .ChangeContext<ISharedStepsGenerator>()
                    .CreateAffiliateAndCampaignApi(_crmUrl,
                    roleName: DataRep.AffiliateWithAllPermissionRole);
            }

            _campaignId = _campaignData.Values.First();
            _affiliateId = _campaignData.Values.Last();

            // get affiliate ApiKey
            _affiliateApiKey = _apiFactory
                .ChangeContext<IUserApi>()
                .PostCreateApiKeyRequest(_crmUrl, _affiliateId);

            // create client with  campaign
            _clientName = TextManipulation.RandomString();

            var clientId = _apiFactory
                .ChangeContext<ICreateClientApi>()
                .CreateClientWithCampaign(_crmUrl, _clientName,
                campaignId: _campaignId, apiKey: _affiliateApiKey);

            _clientsIds = new List<string> { clientId };

            // deposit 400
            _depositId = _apiFactory
                .ChangeContext<IFinancesTabApi>()
                .PostDepositRequest(_crmUrl, clientId, _firstDepositAmount);

            // deposit 500
            _depositId = _apiFactory
                .ChangeContext<IFinancesTabApi>()
                .PostDepositRequest(_crmUrl, clientId, _secondDepositAmount);
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

        /// <summary>
        /// pre condition
        /// campaign and affiliate connected
        /// client connected to the campaign
        /// all affiliate roles are checked
        /// verify affiliate can see 
        /// Ftd Amount, Second Deposit Amount, Deposit Date
        /// </summary>
        [Test]
        [Category(DataRep.ApiDocCategory)]
        [Category(DataRep.SanityCategory)]
        [Category(DataRep.RegressionCategory)]
        [RetryMenage(DataRep.NumOfRetraiesForFallingTest)]
        public void VerifyAffiliateRolesForDepositByCampaignIdApiTest()
        {
            // Affiliate connected to the Campaign can see the deposit
            var actualAffiliateCanSeeTheDeposit = _apiFactory
                .ChangeContext<ICampaignPageApi>()
                .GetDepositsByCampaignIdRequest(_crmUrl, _campaignId, _affiliateApiKey)
                .GeneralResponse;

            Assert.Multiple(() =>
            {
                if (_description == "no permissions")
                {
                    Assert.True(actualAffiliateCanSeeTheDeposit.First().UserId == _clientsIds.First(),
                        $" actual client Id for first deposit: {actualAffiliateCanSeeTheDeposit.First().UserId}",
                        $" expected client Id for first deposit : {_clientsIds.First()}");

                    Assert.True(actualAffiliateCanSeeTheDeposit.First().OriginalCurrency == DataRep.DefaultUSDCurrencyName,
                        $" actual currency for first deposit: {actualAffiliateCanSeeTheDeposit.First().OriginalCurrency}",
                        $" expected currency for first deposit : {DataRep.DefaultUSDCurrencyName}");

                    Assert.True(actualAffiliateCanSeeTheDeposit.First().PspTransactionId == DataRep.DefaultPspTransactionId,
                        $" actual Psp Transaction Id for first deposit: {actualAffiliateCanSeeTheDeposit.First().PspTransactionId}",
                        $" expected Psp Transaction Id for first deposit : {DataRep.DefaultPspTransactionId}");

                    Assert.True(actualAffiliateCanSeeTheDeposit.First().Name == $"{_clientName} {_clientName}",
                        $" actual client full name for first deposit: {actualAffiliateCanSeeTheDeposit.First().Name}",
                        $" expected client full name  for first deposit : {_clientName} {_clientName}");

                    Assert.True(actualAffiliateCanSeeTheDeposit.First().Country == DataRep.UserDefaultCountry,
                        $" actual country for first deposit: {actualAffiliateCanSeeTheDeposit.First().Country}",
                        $" expected Country for first deposit {DataRep.UserDefaultCountry}");

                    Assert.True(actualAffiliateCanSeeTheDeposit.First().FreeText == DataRep.ClientFreeText,
                        $" actual free text for first deposit: {actualAffiliateCanSeeTheDeposit.First().Country}",
                        $" expected  free text for first deposit {DataRep.ClientFreeText}");
                }
                else
                {
                    Assert.True(actualAffiliateCanSeeTheDeposit.First().UserId == _clientsIds.First(),
                        $" actual client Id for first deposit: {actualAffiliateCanSeeTheDeposit.First().UserId}" ,
                        $" expected client Id for first deposit : {_clientsIds.First()}");
                
                    Assert.True(actualAffiliateCanSeeTheDeposit.First().OriginalAmount == _firstDepositAmount.ToString(),
                        $" actual deposit amount for first deposit: {actualAffiliateCanSeeTheDeposit.First().OriginalAmount}",
                        $" expected deposit amount for first deposit : {_firstDepositAmount}");

                    Assert.True(actualAffiliateCanSeeTheDeposit.First().OriginalCurrency == DataRep.DefaultUSDCurrencyName,
                        $" actual currency for first deposit: {actualAffiliateCanSeeTheDeposit.First().OriginalCurrency}",
                        $" expected currency for first deposit : {DataRep.DefaultUSDCurrencyName}");

                    Assert.True(actualAffiliateCanSeeTheDeposit.First().PspTransactionId == DataRep.DefaultPspTransactionId,
                        $" actual Psp Transaction Id for first deposit: {actualAffiliateCanSeeTheDeposit.First().PspTransactionId}",
                        $" expected Psp Transaction Id for first deposit : {DataRep.DefaultPspTransactionId}");

                    Assert.True(actualAffiliateCanSeeTheDeposit.First().Name == $"{_clientName} {_clientName}",
                        $" actual client full name for first deposit: {actualAffiliateCanSeeTheDeposit.First().Name}",
                        $" expected client full name  for first deposit : {_clientName} {_clientName}");

                    Assert.True(actualAffiliateCanSeeTheDeposit.First().Country == DataRep.UserDefaultCountry,
                        $" actual country for first deposit: {actualAffiliateCanSeeTheDeposit.First().Country}",
                        $" expected Country for first deposit {DataRep.UserDefaultCountry}");

                    Assert.True(actualAffiliateCanSeeTheDeposit.First().FreeText == DataRep.ClientFreeText,
                        $" actual free text for first deposit: {actualAffiliateCanSeeTheDeposit.First().Country}",
                        $" expected  free text for first deposit {DataRep.ClientFreeText}");

                    Assert.True(actualAffiliateCanSeeTheDeposit.Last().UserId == _clientsIds.First(),
                        $" actual client Id for second deposit: {actualAffiliateCanSeeTheDeposit.Last().UserId}",
                        $" expected client Id for second deposit : {_clientsIds.First()}");

                    Assert.True(actualAffiliateCanSeeTheDeposit.Last().OriginalAmount == _secondDepositAmount.ToString(),
                        $" actual deposit amount for second deposit: {actualAffiliateCanSeeTheDeposit.Last().OriginalAmount}",
                        $" expected deposit amount for second deposit : {_secondDepositAmount}");

                    Assert.True(actualAffiliateCanSeeTheDeposit.Last().OriginalCurrency == DataRep.DefaultUSDCurrencyName,
                        $" actual currency for second deposit: {actualAffiliateCanSeeTheDeposit.Last().OriginalCurrency}",
                        $" expected currency for second deposit : {DataRep.DefaultUSDCurrencyName}");

                    Assert.True(actualAffiliateCanSeeTheDeposit.Last().PspTransactionId == DataRep.DefaultPspTransactionId,
                        $" actual Psp Transaction Id for second deposit: {actualAffiliateCanSeeTheDeposit.Last().PspTransactionId}",
                        $" expected Psp Transaction Id  for second deposit : {DataRep.DefaultPspTransactionId}");

                    Assert.True(actualAffiliateCanSeeTheDeposit.Last().Name == $"{_clientName} {_clientName}",
                        $" actual client full name for second deposit: {actualAffiliateCanSeeTheDeposit.Last().Name}",
                        $" expected client full name  for second deposit : = {_clientName} {_clientName}");

                    Assert.True(actualAffiliateCanSeeTheDeposit.Last().Country == DataRep.UserDefaultCountry,
                        $" actual country for second deposit: {actualAffiliateCanSeeTheDeposit.Last().Country}",
                        $" expected Country for second deposit {DataRep.UserDefaultCountry}");

                    Assert.True(actualAffiliateCanSeeTheDeposit.First().FreeText == DataRep.ClientFreeText,
                        $" actual free text for second deposit: {actualAffiliateCanSeeTheDeposit.First().Country}",
                        $"expected  free text for second deposit {DataRep.ClientFreeText}");
                }
            });
        }
    }
}