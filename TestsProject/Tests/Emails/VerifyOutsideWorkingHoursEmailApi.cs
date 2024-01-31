// Ignore Spelling: Api

using System;
using System.Collections.Generic;
using System.Linq;
using AirSoftAutomationFramework.Internals.DAL.ConfigurationManagement;
using AirSoftAutomationFramework.Internals.Factorys;
using AirSoftAutomationFramework.Internals.Helpers;
using AirSoftAutomationFramework.Internals.NunitAtributes;
using AirSoftAutomationFramework.Objects.DTOs;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.AccountsApi.UsersMenuApi.UserMenuApi;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.Settings;
using AirSoftAutomationFramework.Objects.ForexObjects.Ui.Login;
using NUnit.Framework;
using OpenQA.Selenium;
using TestsProject.TestsInternals;

namespace TestsProject.Tests.Emails
{
    [TestFixture(DataRep.Chrome)]
    public class VerifyOutsideWorkingHoursEmailApi : TestSuitBase
    {
        public VerifyOutsideWorkingHoursEmailApi(string browser) : base(browser) { }

        #region Test Preparation
        private IApplicationFactory _apiFactory = new ApplicationFactory();
        private string _crmUrl = Config.appSettings.CrmUrl;
        private string _userEmail;
        private GetOfficeResponse _airSoftOffice;
        private string _testimUrl = DataRep.TesimUrl;
        private string _testimEmailPerfix = DataRep.TestimEmailPrefix;
        private IWebDriver _driver;

        [SetUp]
        public void SetUp()
        {
            BeforeTest();
            _driver = GetDriver();

            _airSoftOffice = _apiFactory
                .ChangeContext<IOfficeTabApi>(_driver)
                .GetOfficesRequest(_crmUrl)
                .Where(p => p.city.Equals("Airsoft"))
                .FirstOrDefault();

            var userName = TextManipulation.RandomString();
            _userEmail = userName + _testimEmailPerfix;

            // create user
            _apiFactory
                .ChangeContext<IUserApi>()
                .PostCreateUserRequest(_crmUrl, userName,
                emailPrefix: _testimEmailPerfix, officeData: _airSoftOffice);

            _airSoftOffice.working_hours.active = true;
            _airSoftOffice.working_hours.block_user = true;
            _airSoftOffice.working_hours.send_alert_to = new string[] { _userEmail };

            // set default office with working hours 12:00 - 12:01 in the night
            _apiFactory
               .ChangeContext<IOfficeTabApi>(_driver)
               .PutOfficeRequest(_crmUrl, _airSoftOffice);
        }

        [TearDown]
        public void TearDown()
        {          
            try
            {
                _airSoftOffice.working_hours.active = false;
                _airSoftOffice.working_hours.block_user = false;
                _airSoftOffice.working_hours.send_alert_to = new string[] { "auto@auto.auto" };

                // set default office with SEND ALERT TO default email
                _apiFactory
                   .ChangeContext<IOfficeTabApi>(_driver)
                   .PutOfficeRequest(_crmUrl, _airSoftOffice);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                AfterTest(_driver);
                DriverDispose(_driver);
            }
        }
        #endregion

        [Test]
        [Category(DataRep.SanityCategory)]
        [Category(DataRep.RegressionCategory)]
        [RetryMenage(DataRep.NumOfRetraiesForFallingTest)]
        public void VerifyOutsideWorkingHoursEmailApiTest()
        {
            var selerFirstName = _userEmail.Split('@').First();
            var sellerLastName = selerFirstName;
            var sellerEmail = _userEmail.Split('@')[1];
            var subject = "Login attempt outside working hours";
            var emailBodyParams = new List<string> { "SELLER_FIRST_NAME", "SELLER_LAST_NAME", "SELLER_EMAIL" };

            var emailsParams = new Dictionary<string, string> { 
                { "type", "outside_working_hours" }, 
                { "language", "en" }, { "subject", subject }};

            // update the existing template
            _apiFactory
               .ChangeContext<IPlatformTabApi>(_driver)
               .UpdateEmailTemplatePipe(_crmUrl, emailsParams, emailBodyParams);

            // trigger the mail
             _apiFactory
                .ChangeContext<ILoginPageUi>(_driver)
                .LoginPipe(_userEmail, _crmUrl);

            var email = _apiFactory
                .ChangeContext<IPlatformTabApi>(_driver)
                .FilterEmailBySubject(_testimUrl, _userEmail, subject)
                .First();

            var actualEmailBody = _apiFactory
                .ChangeContext<IPlatformTabApi>(_driver)
                .ParseMailToKeyValuePair(email);

            Assert.Multiple(() =>
            {
                Assert.True(!actualEmailBody["SELLER_FIRST_NAME"].Contains("undefined")
                    && !actualEmailBody["SELLER_FIRST_NAME"].Contains("null"),
                    $" expected SELLER FIRST NAME: {selerFirstName}" +
                    $" actual SELLER FIRST NAME: {actualEmailBody["SELLER_FIRST_NAME"]}");

                Assert.True(!actualEmailBody["SELLER_LAST_NAME"].Contains("undefined")
                    && !actualEmailBody["SELLER_LAST_NAME"].Contains("null"),
                    $" expected SELLER LAST NAME: {sellerLastName}" +
                    $" actual SELLER LAST NAME: {actualEmailBody["SELLER_LAST_NAME"]}");

                Assert.True(!actualEmailBody["SELLER_EMAIL"].Contains("undefined")
                    && !actualEmailBody["SELLER_EMAIL"].Contains("null"),
                    $" expected SELLER EMAIL: {sellerEmail}" +
                    $" actual SELLER EMAIL: {actualEmailBody["SELLER_EMAIL"]}");

                Assert.True(email.Subject == subject,
                    $"actual email subject: {email.Subject}" +
                    $"expected email subject: {subject}");
            });
        }
    }
}