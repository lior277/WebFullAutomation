using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using AirSoftAutomationFramework.Internals.DAL.ConfigurationManagement;
using AirSoftAutomationFramework.Internals.ExtensionsMethods;
using AirSoftAutomationFramework.Internals.Factorys;
using AirSoftAutomationFramework.Internals.Helpers;
using AirSoftAutomationFramework.Internals.NunitAtributes;
using AirSoftAutomationFramework.Objects.DTOs;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.AccountsApi.UsersMenuApi.UserMenuApi;
using AirSoftAutomationFramework.Objects.ForexObjects.ApiGeneral;
using Microsoft.Graph.Models;
using Newtonsoft.Json;
using NUnit.Framework;
using TestsProject.TestsInternals;

namespace TestsProject.Tests.CampaignPage.Dashboard
{
    [TestFixture]
    public class VerifyErrorWhenCreatingTwoCampaignWithTheSamePromoCodeApi : TestSuitBase
    {
        #region Test Preparation

        private IApplicationFactory _apiFactory = new ApplicationFactory();
        private string _crmUrl = Config.appSettings.CrmUrl;
        private IDictionary<string, string> _campaignData;
        private string _userName;
        private string _userId;
        private string _currentUserApiKey;

        [SetUp]
        public void SetUp()
        {
            BeforeTest();
            #region PreCondition

            // create user
            _userName = TextManipulation.RandomString();

            // create user
            var userId = _apiFactory
                .ChangeContext<IUserApi>()
                .PostCreateUserRequest(_crmUrl, _userName);

            // get ApiKey
            _currentUserApiKey = _apiFactory
                .ChangeContext<IUserApi>()
                .PostCreateApiKeyRequest(_crmUrl, userId);
            #endregion
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
        [Category(DataRep.SanityCategory)]
        [Category(DataRep.RegressionCategory)]
        [RetryMenage(DataRep.NumOfRetriesForFallingTest)]
        public void VerifyErrorWhenCreatingTwoCampaignWithTheSamePromoCodeApiTest()
        {
            var expectedDuplicateCampaignCodeError = "{promo_code:The promo code is already in use";

            // create first campaign and Affiliate
            var campaignCode = TextManipulation.RandomString();

            _apiFactory
                .ChangeContext<ISharedStepsGenerator>()
                .CreateAffiliateAndCampaignApi(_crmUrl,
                campaignCode: campaignCode, apiKey: _currentUserApiKey);

            var temp = _apiFactory
                .ChangeContext<ISharedStepsGenerator>()
                .CreateAffiliateAndCampaignApi(_crmUrl,
                campaignCode: campaignCode, apiKey: _currentUserApiKey,
                checkStatusCode: false)
                .Values
                .First();
            

            var actualCreateCampaignError = temp.Replace("[", "")
               .Replace("\"", "")
               .Replace("]", "");

            Assert.Multiple(() =>
            {
                Assert.True(actualCreateCampaignError.Contains(expectedDuplicateCampaignCodeError),
                    $" expected Duplicate Campaign Code Error: {expectedDuplicateCampaignCodeError}" +
                    $" actual Duplicate Campaign Code Error: {actualCreateCampaignError.ListToString()}");
            });
        }
    }
}