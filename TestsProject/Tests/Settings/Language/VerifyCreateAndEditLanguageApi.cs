// Ignore Spelling: Api

using System.Collections.Generic;
using AirSoftAutomationFramework.Internals.DAL.ConfigurationManagement;
using AirSoftAutomationFramework.Internals.Factorys;
using AirSoftAutomationFramework.Internals.Helpers;
using AirSoftAutomationFramework.Internals.NunitAtributes;
using AirSoftAutomationFramework.Objects.DTOs;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.Clients.ClientPage;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.Settings;
using AirSoftAutomationFramework.Objects.ForexObjects.ApiGeneral.Login;
using AirSoftAutomationFramework.Objects.ForexObjects.TradingPlatform.TradeObjects.TradeApi;
using NUnit.Framework;
using TestsProject.TestsInternals;

namespace TestsProject.Tests.Settings.Language
{
    [TestFixture]
    public class VerifyCreateAndEditLanguageApi : TestSuitBase
    {
        #region Test Preparation
        private IApplicationFactory _apiFactory = new ApplicationFactory();
        private string _crmUrl = Config.appSettings.CrmUrl;
        private string _tradingPlatformUrl =  Config.appSettings.tradingPlatformUrl;
        GetLoginResponse _loginData;

        [SetUp]
        public void SetUp()
        {
            BeforeTest();
            #region PreCondition
            // create client 
            var clientName = TextManipulation.RandomString();
            var clientEmail = clientName + DataRep.EmailPrefix;

            _apiFactory
                .ChangeContext<ICreateClientApi>()
                .CreateClientRequest(_crmUrl, clientName);

            // get login Data for trading Platform
            _loginData = _apiFactory
                 .ChangeContext<ILoginApi>()
                 .PostLoginToTradingPlatform(_tradingPlatformUrl, clientEmail)
                 .GeneralResponse;
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
        [Description("based on jira https://airsoftltd.atlassian.net/browse/AIRV2-4149")]
        [Category(DataRep.SanityCategory)]
        [Category(DataRep.RegressionCategory)]
        [RetryMenage(DataRep.NumOfRetraiesForFallingTest)]
        public void VerifyCreateAndEditLanguageApiTest()
        {
            var languageKey = "LANGUAGES";
            var expectedLanguageModifiedValue = "LanguagesAutomation";
            var languageCode = "it";
            var languageName = "Italiano";

            var modifyValue = new Dictionary<string, string>()
            { { languageKey, expectedLanguageModifiedValue } };

            var erpLanguage = _apiFactory
                .ChangeContext<ILanguagesTab>()
                .CreateErpLanguagePipe(_crmUrl);

            var tradingLanguage = _apiFactory
                .ChangeContext<ILanguagesTab>()
                .CreateTradingLanguagePipe(_crmUrl);

            var actualValueTradeLanguage = _apiFactory
                .ChangeContext<ILanguagesTab>()
                .PutErpLanguagePipe(_crmUrl, languageName, languageCode,
                modifiedLanguageItem: modifyValue)
                .PutTradingLanguagePipe(_crmUrl, languageName, languageCode,
                modifiedLanguageItem: modifyValue)
                .ChangeContext<ITradePageApi>()
                .GetLanguageByCode(_tradingPlatformUrl, languageCode, _loginData)
                .GeneralResponse
                .tradingTranslations
                .LANGUAGES;

            var actualValueErpLanguage = _apiFactory
                .ChangeContext<ILanguagesTab>()
                .GetLanguageByCode(_crmUrl, languageCode)
                .translations
                .LANGUAGES;

            Assert.Multiple(() =>
            {
                Assert.True(actualValueTradeLanguage == expectedLanguageModifiedValue,
                    $" actual Value Trade Language: {actualValueTradeLanguage}" +
                    $" expected Value Trade Language: {expectedLanguageModifiedValue}");

                Assert.True(actualValueErpLanguage == expectedLanguageModifiedValue,
                    $" actual Value Erp Language: {actualValueErpLanguage}" +
                    $" expected Value Erp Language: {expectedLanguageModifiedValue}");
            });
        }
    }
}