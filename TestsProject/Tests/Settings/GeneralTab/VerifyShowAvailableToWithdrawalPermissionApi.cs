// Ignore Spelling: Api

using AirSoftAutomationFramework.Internals.DAL.ConfigurationManagement;
using AirSoftAutomationFramework.Internals.Factorys;
using AirSoftAutomationFramework.Internals.Helpers;
using AirSoftAutomationFramework.Internals.NunitAtributes;
using AirSoftAutomationFramework.Objects.DTOs;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.Clients.ClientCard.FinancesTab;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.Clients.ClientPage;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.Settings;
using AirSoftAutomationFramework.Objects.ForexObjects.ApiGeneral.Login;
using AirSoftAutomationFramework.Objects.ForexObjects.TradingPlatform.TradeObjects.TradeApi.Settings;
using NUnit.Framework;
using TestsProject.TestsInternals;

namespace TestsProject.Tests.Settings.GeneralTab
{
    [TestFixture]
    public class VerifyShowAvailableToWithdrawalPermissionApi : TestSuitBase
    {
        #region Test Preparation
        private IApplicationFactory _apiFactory = new ApplicationFactory();
        private string _crmUrl = Config.appSettings.CrmUrl;
        private GetLoginResponse _loginData;
        private string _tradingPlatformUrl = Config.appSettings.tradingPlatformUrl;

        [SetUp]
        public void SetUp()
        {
            #region PreCondition
            BeforeTest();
            var depositAmount = 1000;

            // Get general settings data
            var brandRegulationData = _apiFactory
                .ChangeContext<ISuperAdminTubApi>()
                .GetRegulationRequest(_crmUrl);

            brandRegulationData.edit_client_profile.show_available_to_withdrawal = false;

            // put BrandRegulation data
            _apiFactory
                .ChangeContext<IGeneralTabApi>()
                .PutGeneralSettingsRequest(_crmUrl, brandRegulationData);

            // create client 
            var clientName = TextManipulation.RandomString();
            var clientEmail = clientName + DataRep.EmailPrefix;

            var clientId = _apiFactory
                .ChangeContext<ICreateClientApi>()
                .CreateClientRequest(_crmUrl, clientName);

            // get login data
            _loginData = _apiFactory
                .ChangeContext<ILoginApi>()
                .PostLoginToTradingPlatform(_tradingPlatformUrl, clientEmail)
                .GeneralResponse;

            // client create deposit 
            _apiFactory
                .ChangeContext<IFinancesTabApi>()
                .PostDepositRequest(_crmUrl, clientId, depositAmount);
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

        // based on bug https://airsoftltd.atlassian.net/browse/AIRV2-5180
        [Test]
        [Description("Based on https://airsoftltd.atlassian.net/browse/AIRV2-5180")]
        [Category(DataRep.SanityCategory)]
        [Category(DataRep.RegressionCategory)]
        [RetryMenage(DataRep.NumOfRetraiesForFallingTest)]
        public void VerifyShowAvailableToWithdrawalPermissionApiTest()
        {
            var expectedError = "Method Not Allowed";

            var availableWithdrawalErrorMessage = _apiFactory
                .ChangeContext<IWithdrawalTpApi>()
                .GetAvailableWithdrawalRequest(_tradingPlatformUrl, _loginData, false);

            Assert.Multiple(() =>
            {
                Assert.True(availableWithdrawalErrorMessage == expectedError,
                    $" expected available Withdrawal Error Message: {expectedError}" +
                    $" actual available Withdrawal Error Message: {availableWithdrawalErrorMessage}");
            });
        }
    }
}