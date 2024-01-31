// Ignore Spelling: Admin Api

using System.Collections.Generic;
using System.Linq;
using AirSoftAutomationFramework.Internals.DAL.ConfigurationManagement;
using AirSoftAutomationFramework.Internals.ExtensionsMethods;
using AirSoftAutomationFramework.Internals.Factorys;
using AirSoftAutomationFramework.Internals.Helpers;
using AirSoftAutomationFramework.Internals.NunitAtributes;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.AccountsApi.UsersMenuApi.UserMenuApi;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.Clients.ClientCard.FinancesTab;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.Clients.ClientPage;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.Settings;
using NUnit.Framework;
using TestsProject.TestsInternals;

namespace TestsProject.Tests.Emails
{
    [NonParallelizable]
    [TestFixture]
    public class VerifyAdminDepositEmailApi : TestSuitBase
    {
        #region Test Preparation
        private IApplicationFactory _apiFactory = new ApplicationFactory();
        private string _crmUrl = Config.appSettings.CrmUrl;
        private string _clientId;
        private string _userEmail;
        private string _clientEmail;
        private string _currency = DataRep.DefaultUSDCurrencyName;
        private string _testimUrl = DataRep.TesimUrl;

        [SetUp]
        public void SetUp()
        {
            BeforeTest();
            // create user
            var userName = TextManipulation.RandomString();
            _userEmail = userName + DataRep.TestimEmailPrefix;

            // create user
            _apiFactory
             .ChangeContext<IUserApi>()
             .PostCreateUserRequest(_crmUrl, userName,
                emailPrefix: DataRep.TestimEmailPrefix);

            // set emails for Admin Emails For Deposit
            var brandRegulation = _apiFactory
                .ChangeContext<ISuperAdminTubApi>()
                .GetBrandRegulationRequest(_crmUrl);

            DataRep.EmailListForExport.Add(_userEmail);

            brandRegulation.export_data_email_url = DataRep
                .EmailListForExport.ToArray();

            DataRep.EmailListForAdminDeposit.Add(_userEmail);

            brandRegulation.admin_email_for_deposit = DataRep
                .EmailListForAdminDeposit.ToArray();

            // enter the Email For Export in Super Admin Tub
            _apiFactory
                .ChangeContext<ISuperAdminTubApi>()
                .PutRegulationsRequest(_crmUrl, brandRegulation);

            // create client 
            var clientName = TextManipulation.RandomString();
            _clientEmail = clientName + DataRep.TestimEmailPrefix;

            _clientId = _apiFactory
                .ChangeContext<ICreateClientApi>()
                .CreateClientRequest(_crmUrl, clientName, currency: _currency,
                emailPrefix: DataRep.TestimEmailPrefix);           
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
        public void VerifyAdminDepositEmailApiTest()
        {
            var firstDepositAmount = 10;
            var secondDepositAmount = 20;
            var subject = "New deposit on airsoftltd.com";
            var expectedBrandName = "BrandName";
            var expectedDepositFtd = new List<string>() { "Yes", "No" };
            var expectedDepositAmounts = new List<string>() { "10.00", "20.00" };          

            var emailBodyParams = new List<string> { "DEPOSIT_AMOUNT",
                "DEPOSIT_CURRENCY", "DEPOSIT_FTD",
                "DEPOSIT_EURO_AMOUNT", "BRAND_NAME" };

            var emailsParams = new Dictionary<string, string> { 
                { "type", "admin_deposit" }, { "language", "en" }, { "subject", subject }};

            // update the existing template
            _apiFactory
               .ChangeContext<IPlatformTabApi>()
               .UpdateEmailTemplatePipe(_crmUrl, emailsParams, emailBodyParams);

            // trigger the email
            // create first deposit
            _apiFactory
                .ChangeContext<IFinancesTabApi>()
                .PostDepositRequest(_crmUrl, _clientId, firstDepositAmount);

            // create second deposit
            var actualDepositId = _apiFactory
                .ChangeContext<IFinancesTabApi>()
                .PostDepositRequest(_crmUrl, _clientId, secondDepositAmount);

            var emails = _apiFactory
                .ChangeContext<IPlatformTabApi>()
                .WaitForNumOfEmails(_testimUrl, _userEmail, 2)
                .FilterEmailBySubject(_testimUrl, _userEmail, subject);

            var actualFirstDepositEmailBody = _apiFactory
                .ChangeContext<IPlatformTabApi>()
                .ParseMailToKeyValuePair(emails.First());

            var actualSecondDepositEmailBody = _apiFactory
                .ChangeContext<IPlatformTabApi>()
                .ParseMailToKeyValuePair(emails.Last());

            // first deposit in EUR
            var expectedFirstDepositAmountInEUR = _apiFactory
                .ChangeContext<IGeneral>()
                .PostCurrencyConversionRequest(_crmUrl,
                firstDepositAmount, DataRep.DefaultUSDCurrencyName, "EUR")
                .ToString()
                .Split('.')
                .First();

            // second deposit in EUR
            var expectedSecondDepositAmountInEUR = _apiFactory
                .ChangeContext<IGeneral>()
                .PostCurrencyConversionRequest(_crmUrl,
                secondDepositAmount, DataRep.DefaultUSDCurrencyName, "EUR")
                .ToString()
                .Split('.')
                .First();

            var actualDepositFtdValuesList = new List<string>()
            { actualFirstDepositEmailBody["DEPOSIT_FTD"],
                actualSecondDepositEmailBody["DEPOSIT_FTD"] };

            var actualDepositAmountsList = new List<string>()
            { actualFirstDepositEmailBody["DEPOSIT_AMOUNT"],
                actualSecondDepositEmailBody["DEPOSIT_AMOUNT"] };

            var actualDepositCurrencyList = new List<string>() 
            { actualFirstDepositEmailBody["DEPOSIT_CURRENCY"],
                actualSecondDepositEmailBody["DEPOSIT_CURRENCY"] };

            var actualDepositEroAmountsList = new List<string>() 
            {actualFirstDepositEmailBody["DEPOSIT_EURO_AMOUNT"].Split('.').First(),
                actualSecondDepositEmailBody["DEPOSIT_EURO_AMOUNT"].Split('.').First() };

            var actualBrandNamesList = new List<string>()
            { actualFirstDepositEmailBody["BRAND_NAME"],
                actualSecondDepositEmailBody["BRAND_NAME"] };

            Assert.Multiple(() =>
            {
                Assert.True(actualDepositFtdValuesList
                    .CompareTwoListOfString(expectedDepositFtd).Count == 0,
                    $" expected deposit ftd: yes, no" +
                    $" actual deposit ftd: {actualDepositFtdValuesList.ListToString()}" +
                    $" admin email: {_userEmail}");

                Assert.True(actualDepositAmountsList
                    .CompareTwoListOfString(expectedDepositAmounts).Count == 0,
                    $" expected Deposit Amounts List: {expectedDepositAmounts.ListToString()}" +
                    $" actual Deposit Amounts List: {actualDepositAmountsList.ListToString()}" +
                    $" admin email: {_userEmail}");

                Assert.True(actualDepositCurrencyList.All(p => p.Equals(_currency)),
                    $" expected Deposit Amounts List: {_currency}" +
                    $" actual Deposit Amounts List: {actualDepositCurrencyList.ListToString()}" +
                    $" admin email: {_userEmail}");

                Assert.True(actualDepositEroAmountsList.Contains(expectedFirstDepositAmountInEUR),
                    $" expected first Deposit Ero Amounts : {expectedFirstDepositAmountInEUR}" +
                    $" actual Deposit Ero Amounts: {actualDepositEroAmountsList.ListToString()}" +
                    $" admin email: {_userEmail}");

                Assert.True(actualDepositEroAmountsList.Contains(expectedSecondDepositAmountInEUR),
                   $" expected  Deposit Ero Amounts : {expectedSecondDepositAmountInEUR}" +
                   $" actual Deposit Ero Amounts: {actualDepositEroAmountsList.ListToString()}" +
                   $" admin email: {_userEmail}");

                Assert.True(actualBrandNamesList.All(p => p.Equals(expectedBrandName)),
                    $" expected Brand Names List: {expectedBrandName}" +
                    $" actual Brand Names List: {actualBrandNamesList.ListToString()}" +
                    $" admin email: {_userEmail}");

                Assert.True(emails.All(p => p.Subject.Equals(subject)),
                    $" actual email subject: {emails.Select(p => p.Subject.ToList().ListToString())}" +
                    $" expected email subject: {subject}" +
                    $" admin email: {_userEmail}");
            });
        }
    }
}