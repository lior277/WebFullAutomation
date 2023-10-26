// Ignore Spelling: Api

using AirSoftAutomationFramework.Internals.DAL.ConfigurationManagement;
using AirSoftAutomationFramework.Internals.ExtensionsMethods;
using AirSoftAutomationFramework.Internals.Factory;
using AirSoftAutomationFramework.Internals.Helpers;
using AirSoftAutomationFramework.Internals.NunitAttributes;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.Clients.ClientCard.FinancesTab;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.Clients.ClientPage;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.Settings;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using TestsProject.TestsInternals;

namespace TestsProject.Tests.Emails
{
    [TestFixture]
    public class VerifyGlobalVariablesEmailApi : TestSuitBase
    {
        #region Test Preparation
        private IApplicationFactory _apiFactory = new ApplicationFactory();
        private string _crmUrl = Config.appSettings.CrmUrl;
        private string _clientId;
        private string _clientEmail;
        private string _currencySymbol = DataRep.DefaultUSDCurrencyName;
        private int _depositAmount = 100;
        private string _testimUrl = DataRep.TesimUrl;

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

            _apiFactory
               .ChangeContext<IFinancesTabApi>()
               .PostDepositRequest(_crmUrl, _clientId, _depositAmount);

            _apiFactory
               .ChangeContext<IGeneralTabApi>()
               .PutEditCompanyInformationRequest(_crmUrl);
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

        // check the global parameters with deposit email
        [Test]
        [Category(DataRep.SanityCategory)]
        [Category(DataRep.RegressionCategory)]
        [RetryMenage(DataRep.NumOfRetraiesForFallingTest)]
        public void VerifyGlobalVariablesEmailApiTest()
        {
            var subject = "Second Deposit Confirmation!";
            var expectedClientId = _clientId;
            var clientFirstName = _clientEmail.Split('@').First();
            var clientLastName = _clientEmail.Split('@').First();
            var expectedClientName = $"{clientFirstName} {clientFirstName}";
            var expectedCurrencySymbol = _currencySymbol;
            var expectedClientEmail = _clientEmail;
            var expectedClienBalance = _depositAmount + _depositAmount; // first and second deposit;
            var expectedCompanyLogo = "main_logo";

            var emailBody = _apiFactory
                .ChangeContext<IPlatformTabApi>()
                .ComposeEmailBody(DataRep.EmailGlobalVariables);

            var emailsParams = new Dictionary<string, string> {  { "type", "client_deposit" },
                { "language", "en" }, { "subject", subject }, { "body", emailBody } };

            _apiFactory
               .ChangeContext<IPlatformTabApi>()
               .PutEmailByIdTemplateRequest(_crmUrl, emailsParams);

            // trigger the mail
            _apiFactory
                .ChangeContext<IFinancesTabApi>()
                .PostDepositRequest(_crmUrl, _clientId, _depositAmount);

            var email = _apiFactory
                .ChangeContext<IPlatformTabApi>()
                .FilterEmailBySubject(_testimUrl, _clientEmail, subject)
                .First();

            var actualEmailBody = _apiFactory
                .ChangeContext<IPlatformTabApi>()
                .ParseMailToKeyValuePair(email);

            Assert.Multiple(() =>
            {
                Assert.True(actualEmailBody["CLIENT_ID"] == _clientId,
                    $" expected CLIENT ID: {_clientId}" +
                    $" actual CLIENT ID: {actualEmailBody["CLIENT_ID"]}" +
                    $" email body {actualEmailBody.DictionaryToString()}");

                Assert.True(actualEmailBody["CLIENT_NAME"] == expectedClientName,
                    $" expected CLIENT NAME: {expectedClientName}" +
                    $" actual CLIENT NAME: {actualEmailBody["CLIENT_NAME"]}" +
                    $" email body {actualEmailBody.DictionaryToString()}");

                Assert.True(actualEmailBody["CLIENT_FIRST_NAME"] == clientFirstName,
                    $" expected CLIENT FIRST NAME: {clientFirstName}" +
                    $" actual CLIENT FIRST NAME: {actualEmailBody["CLIENT_FIRST_NAME"]}" +
                    $" email body {actualEmailBody.DictionaryToString()}");

                Assert.True(actualEmailBody["CLIENT_PHONE"] != null,
                    $" expected CLIENT PHONE: not null" +
                    $" actual CLIENT PHONE: {actualEmailBody["CLIENT_PHONE"]}" +
                    $" email body {actualEmailBody.DictionaryToString()}");

                Assert.True(actualEmailBody["CLIENT_CURRENCY_SYMBOL"] == expectedCurrencySymbol,
                    $" expected Currency Symbol: {expectedCurrencySymbol}" +
                    $" actual Currency Symbol: {actualEmailBody["CLIENT_CURRENCY_SYMBOL"]}" +
                    $" email body {actualEmailBody.DictionaryToString()}");

                Assert.True(actualEmailBody["CLIENT_EMAIL"] == expectedClientEmail,
                    $" expected CLIENT EMAIL: {expectedClientEmail}" +
                    $" actual CLIENT NAME: {actualEmailBody["CLIENT_EMAIL"]}" +
                    $" email body {actualEmailBody.DictionaryToString()}");

                Assert.True(actualEmailBody["CLIENT_BALANCE"] == $"{expectedClienBalance}.00",
                    $" expected CLIENT BALANCE: {expectedClienBalance}.00" +
                    $" actual CLIENT BALANCE: {actualEmailBody["CLIENT_BALANCE"]}" +
                    $" email body {actualEmailBody.DictionaryToString()}");

                Assert.True(actualEmailBody["COMPANY_LOGO"].Contains(expectedCompanyLogo),
                    $" expected COMPANY LOGO: {expectedCompanyLogo}" +
                    $" actual COMPANY LOGO: {actualEmailBody["COMPANY_LOGO"]}" +
                    $" email body {actualEmailBody.DictionaryToString()}");
            });
        }
    }
}