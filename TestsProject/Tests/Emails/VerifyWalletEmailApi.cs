// Ignore Spelling: Api

using System.Collections.Generic;
using System.Linq;
using AirSoftAutomationFramework.Internals.DAL.ConfigurationManagement;
using AirSoftAutomationFramework.Internals.Factory;
using AirSoftAutomationFramework.Internals.Helpers;
using AirSoftAutomationFramework.Internals.NunitAttributes;
using AirSoftAutomationFramework.Objects.DTOs;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.Clients.ClientPage;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.Settings;
using AirSoftAutomationFramework.Objects.ForexObjects.ApiGeneral.Login;
using AirSoftAutomationFramework.Objects.ForexObjects.TradingPlatform.TradeObjects.TradeApi;
using NUnit.Framework;
using TestsProject.TestsInternals;

namespace TestsProject.Tests.Emails
{
    [TestFixture]
    public class VerifyWalletEmailApi : TestSuitBase
    {
        #region Test Preparation
        private IApplicationFactory _apiFactory = new ApplicationFactory();
        private string _crmUrl = Config.appSettings.CrmUrl;
        private string _clientId;
        private string _clientEmail;
        private string _testimUrl = DataRep.TesimUrl;
        private string _tradingPlatformUrl =  Config.appSettings.tradingPlatformUrl;
        private GetLoginResponse _loginData;

        [SetUp]
        public void SetUp()
        {
            BeforeTest();

            // create client 
            var clientName = TextManipulation.RandomString();
            _clientEmail = clientName + DataRep.TestimEmailPrefix;

            _clientId = _apiFactory
                .ChangeContext<ICreateClientApi>()
                .CreateClientRequest(_crmUrl, clientName,
              emailPrefix: DataRep.TestimEmailPrefix);

            _loginData = _apiFactory
                .ChangeContext<ILoginApi>()
                .PostLoginToTradingPlatform(_tradingPlatformUrl, _clientEmail)
                .GeneralResponse;

            // create psp for deposit page
            //_apiFactory
            //  .ChangeContext<IPspTabApi>()
            //  .PostCreateBankTransferPspRequest(_crmUrl);
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
        [RetryMenage(DataRep.NumOfRetraiesForFallingTest)]
        public void VerifyWalletEmailApiTest()
        {
            var pspTitle = "wallet title";
            var pspBody = "wallet body";
            var pspFooter = "wallet footer";
            var pspWallet = "wallet wallet";
            var subject = "Wallet Details - BRAND_NAME";
            var emailBodyParams = new List<string> { "WALLET_TITLE", "WALLET_BODY", "WALLET_WALLET", "WALLET_FOOTER" };
            var emailsParams = new Dictionary<string, string> {  { "type", "wallet_psp" },
                { "language", "en" }, { "subject", subject }};

            // update the existing template
            _apiFactory
               .ChangeContext<IPlatformTabApi>()
               .UpdateEmailTemplatePipe(_crmUrl, emailsParams, emailBodyParams);

            // trigger the mail
            _apiFactory
               .ChangeContext<ITradeDepositPageApi>()
               .PostSendCustomPspWalletDetailsRequest(_tradingPlatformUrl, pspBody, 
               pspTitle, pspFooter, pspWallet, _loginData);

            var email = _apiFactory
                .ChangeContext<IPlatformTabApi>()
                .FilterEmailBySubject(_testimUrl, _clientEmail, subject)
                .First();

            var actualEmailBody = _apiFactory
                .ChangeContext<IPlatformTabApi>()
                .ParseMailToKeyValuePair(email);

            Assert.Multiple(() =>
            {
                Assert.True(actualEmailBody["WALLET_TITLE"].TrimStart().TrimEnd() == pspTitle,
                    $" expected TITLE: {pspTitle}" +
                    $" actual TITLE: {actualEmailBody["WALLET_TITLE"]}");

                Assert.True(actualEmailBody["WALLET_BODY"].TrimStart().TrimEnd() == pspBody,
                    $" expected BODY: {pspBody}" +
                    $" actual BODY: {actualEmailBody["WALLET_BODY"]}");

                Assert.True(actualEmailBody["WALLET_WALLET"] == pspWallet,
                    $" expected psp Wallet: {pspWallet}" +
                    $" actual psp Wallet: {actualEmailBody["WALLET_WALLET"]}");

                Assert.True(actualEmailBody["WALLET_FOOTER"].TrimStart().TrimEnd() == pspFooter,
                    $" expected psp Footer: {pspFooter}" +
                    $" actual psp Footer: {actualEmailBody["WALLET_FOOTER"]}");

                Assert.True(email.Subject == subject,
                    $" actual email subject: {email.Subject}" +
                    $" expected email subject: {subject}");
            });
        }
    }
}