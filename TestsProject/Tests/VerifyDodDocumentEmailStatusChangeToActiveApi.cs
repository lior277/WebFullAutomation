// Ignore Spelling: Api

using AirSoftAutomationFramework.Internals.DAL.ConfigurationManagement;

using AirSoftAutomationFramework.Internals.Helpers;
using AirSoftAutomationFramework.Internals.NunitAttributes;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.Settings;
using TestsProject.TestsInternals;
using AirSoftAutomationFramework.Internals.Factory;

namespace TestsProject.Tests
{
    [TestFixture]
    public class VerifyAuditTradeGroupApi : TestSuitBase
    {
        #region Test Preparation
        private IApplicationFactory _apiFactory = new ApplicationFactory();
        private string _crmUrl = Config.appSettings.CrmUrl;
        private string _documentAndDodName = "Automation";
        private string _emailType = "dod";
        private bool _expectedDocumentNewStatus = false;
        private bool _expectedDodNewStatus = false;
        private bool _emailNewStatus;

        [SetUp]
        public void SetUp()
        {
            BeforeTest();
            #region PreCondition
            #region Create Dod
            var dodAttributes = new List<string> { "CLIENT_ID" };
            var addLanguage = "fr";

            var dodParams = new Dictionary<string, string> {{ "name", _documentAndDodName },
                { "language", addLanguage }, { "sendBy", "email"}, { "depositType", "bank_transfer" } };

            // update FR Language to active
            _apiFactory
                .ChangeContext<ILanguagesTab>()
                .PutTradingLanguagePipe(_crmUrl, "Français", addLanguage);

            // create dod body
            var documentBody = _apiFactory
                .ChangeContext<IPlatformTabApi>()
                .ComposeEmailBody(dodAttributes);

            // create dod
            _apiFactory
                .ChangeContext<IPlatformTabApi>()
                .CreateDodPipe(_crmUrl, dodParams, documentBody);       

            // Change Dod Status
            _apiFactory
                .ChangeContext<IPlatformTabApi>()
                .ChangeDodStatusPipe(_crmUrl, _documentAndDodName, _expectedDodNewStatus);
            #endregion

            #region Create Document
            var documentAttributes = new List<string> { "CLIENT_ID" };

            var documentParams = new Dictionary<string, string> {
                { "type", "custom" }, { "language", "en" }, { "name", _documentAndDodName }};

            // create document body
            documentBody = _apiFactory
                .ChangeContext<IPlatformTabApi>()
                .ComposeEmailBody(documentAttributes);

            // create document
            _apiFactory
                .ChangeContext<IPlatformTabApi>()
                .CreateDocumentPipe(_crmUrl, documentParams, documentBody);
          
            _apiFactory
               .ChangeContext<IPlatformTabApi>()
               .ChangeDocumentStatusPipe(_crmUrl, _documentAndDodName, _expectedDocumentNewStatus);
            #endregion

            // get email current status
            var baseStatus = _apiFactory
               .ChangeContext<IPlatformTabApi>()
               .GetEmailStatusByNameRequest(_crmUrl, _emailType);

            _emailNewStatus = !baseStatus.Equals(true);

            _apiFactory
               .ChangeContext<IPlatformTabApi>()
               .ChangeEmailStatusPipe(_crmUrl, _emailType, _emailNewStatus);
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

        // delete all dods in one time setup
        // create dod with FR lang - by default the status of new dod is true
        // change the dod status to false
        // delete the automation document if exist
        // create new document - by default the status of new document is true
        // change the document status to false
        // check the DOD email status
        // change the status to different from above

        [Test]
        [Description("based on jira https://airsoftltd.atlassian.net/browse/AIRV2-4655")]
        [Category(DataRep.SanityCategory)]
        [Category(DataRep.RegressionCategory)]
        [RetryMenage(DataRep.NumOfRetraiesForFallingTest)]
        public void VerifyDodDocumentEmailStatusChangeToActiveApiTest()
        {
            var actualDodStatus = _apiFactory
                .ChangeContext<IPlatformTabApi>()
                .GetDodsRequest(_crmUrl)
                .Where(p => p.Name == _documentAndDodName)
                .FirstOrDefault()
                .Active;

            var actualDocumentStatus = _apiFactory
                .ChangeContext<IPlatformTabApi>()
                .GetDocumentsRequest(_crmUrl)
                .Where(p => p.Name == _documentAndDodName)
                .FirstOrDefault()
                .Active;

            var actualEmailStatus = _apiFactory
                .ChangeContext<IPlatformTabApi>()
                .GetAutoEmailsRequest(_crmUrl)
                .Where(p => p.Type == _emailType)
                .FirstOrDefault()
                .Active;

            Assert.Multiple(() =>
            {
                Assert.True(actualDodStatus == _expectedDodNewStatus,
                    $" actual Dod status: {actualDodStatus}" +
                    $" expected Dod status: {_expectedDodNewStatus}");

                Assert.True(actualDocumentStatus == _expectedDocumentNewStatus,
                    $" actual Document status: {actualDocumentStatus}" +
                    $" expected Document status: {_expectedDocumentNewStatus}");

                Assert.True(actualEmailStatus == _emailNewStatus,
                    $" actual email status: {actualEmailStatus}" +
                    $" expected email status: {_emailNewStatus}");
            });
        }
    }
}