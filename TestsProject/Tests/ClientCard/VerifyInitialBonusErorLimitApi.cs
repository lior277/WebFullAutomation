// Ignore Spelling: Api

using System;
using System.Linq;
using AirSoftAutomationFramework.Internals.DAL.ConfigurationManagement;
using AirSoftAutomationFramework.Internals.ExtensionsMethods;
using AirSoftAutomationFramework.Internals.Factorys;
using AirSoftAutomationFramework.Internals.Helpers;
using AirSoftAutomationFramework.Internals.NunitAtributes;
using AirSoftAutomationFramework.Objects.DTOs;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.AccountsApi.UsersMenuApi.UserMenuApi;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.Clients.ClientCard.FinancesTab;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.Clients.ClientPage;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.Settings;
using Newtonsoft.Json;
using NUnit.Framework;
using TestsProject.TestsInternals;

namespace TestsProject.Tests.ClientCard
{
    [TestFixture]
    public class VerifyInitialBonusErrorLimitApi : TestSuitBase
    {
        private IApplicationFactory _apiFactory = new ApplicationFactory();
        private string _crmUrl = Config.appSettings.CrmUrl;
        private string _clientId;
        private decimal _expectedBonusAmountInUsd;
        private string _currentUserApiKey;

        [SetUp]
        public void SetUp()
        {
            BeforeTest();

            #region PreCondition
            var initialBonusAmountInEro = _apiFactory
                .ChangeContext<ISuperAdminTubApi>()
                .GetCurrenciesRequest(_crmUrl)
                .Currencies
                .Eur
                .MaxInitialBonus;

            var userName = TextManipulation.RandomString();

            // create user
            var userId = _apiFactory
                .ChangeContext<IUserApi>()
                .PostCreateUserRequest(_crmUrl, userName);

            // user ApiKey
            _currentUserApiKey = _apiFactory
                .ChangeContext<IUserApi>()
                .PostCreateApiKeyRequest(_crmUrl, userId);

            // create client 
            var clientName = TextManipulation.RandomString();

            _clientId = _apiFactory
                .ChangeContext<ICreateClientApi>()
                .CreateClientRequest(_crmUrl, clientName,
                apiKey: _currentUserApiKey);

            // EUR in USD
            _expectedBonusAmountInUsd = _apiFactory
                .ChangeContext<IGeneral>()
                .PostCurrencyConversionRequest(_crmUrl,
                initialBonusAmountInEro, "EUR", DataRep.DefaultUSDCurrencyName);
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
        // based on ticket https://airsoftltd.atlassian.net/browse/AIRV2-4495
        // create user
        // create client with the user api key
        // convert the bonus amount from EUR to USD
        // create bonus with user api key with amount larger then maximum
        // create bonus with apikey of super admin
        // verify error message when admin bonus exceed the maximum
        // verify super admin can exceed the maximum bonus  
        [Test]
        [Category(DataRep.SanityCategory)]
        [Category(DataRep.RegressionCategory)]
        [RetryMenage(DataRep.NumOfRetriesForFallingTest)]
        public void VerifyInitialBonusErrorLimitApiTest()
        {
            var expectedBonusAmountInUsd = _expectedBonusAmountInUsd.MathRoundFromGeneric(2);
            var bonusAmountInUsd = expectedBonusAmountInUsd + 300;

            var expectedMessageForAdmin = 
                $"The maximum bonus that can be given before deposits is " +
                $"{expectedBonusAmountInUsd}";

            var adminCreateBonus = _apiFactory
                .ChangeContext<IFinancesTabApi>()
                .PostBonusRequest(_crmUrl, _clientId, Convert.ToInt32(bonusAmountInUsd),
                apiKey: _currentUserApiKey, checkStatusCode: false)
                .Message;

            var actualTempMessage =
                JsonConvert.DeserializeObject<GeneralDto>(adminCreateBonus)
                .Amount
                .First()
                .Split("is");

            var maximumBonus = actualTempMessage
                .ToList()
                .Last()
                .Split('$')
                .First();

            var maximumBonusAmountRounded = maximumBonus.MathRoundFromGeneric(2);
            var actualExeedingBonusMessageForAdmin = string.Concat(actualTempMessage[0], "is", " ", maximumBonusAmountRounded);

            var superAdminCreateBonusErrorMessage = _apiFactory
                .ChangeContext<IFinancesTabApi>()
                .PostBonusRequest(_crmUrl, _clientId, Convert.ToInt32(bonusAmountInUsd))
                .GeneralResponse
                .InsertId;

            Assert.Multiple(() =>
            {
                Assert.True(actualExeedingBonusMessageForAdmin.Contains(expectedMessageForAdmin),
                    $"expected Exceeding Bonus Message For Admin: {expectedMessageForAdmin}" +
                    $" actual Exceeding Bonus Message For Admin: {actualExeedingBonusMessageForAdmin}");

                Assert.True(Convert.ToInt32(superAdminCreateBonusErrorMessage) != 0,
                    $"expected Exceeding Bonus Amount For Super Admin:not equal to zero" +
                    $" actual Exceeding Bonus Amount For Super Admin: {superAdminCreateBonusErrorMessage}");
            });
        }
    }
}