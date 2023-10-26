// Ignore Spelling: Api

using AirSoftAutomationFramework.Internals.DAL.ConfigurationManagement;
using AirSoftAutomationFramework.Internals.Factory;
using AirSoftAutomationFramework.Internals.Helpers;
using AirSoftAutomationFramework.Internals.NunitAttributes;
using AirSoftAutomationFramework.Objects.DTOs;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.Clients.ClientCard;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.Clients.ClientPage;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.Settings;
using AirSoftAutomationFramework.Objects.ForexObjects.ApiGeneral;
using DocumentFormat.OpenXml.Drawing.Charts;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using TestsProject.TestsInternals;

namespace TestsProject.Tests.Settings.GeneralTab
{
    [TestFixture]
    public class VerifyDeleteAndEditSalesStatusInSettingsApi : TestSuitBase
    {
        #region Test Preparation
        private IApplicationFactory _apiFactory = new ApplicationFactory();
        private string _crmUrl = Config.appSettings.CrmUrl;
        private string _oldSalesStatusName;
        private string _clientId;  
        private Dictionary<string, SaleStatusValues> _salesStatusText;

        [SetUp]
        public void SetUp()
        {            
            #region PreCondition
            BeforeTest();

            // create client 
            var clientName = TextManipulation.RandomString();
            var clientEmail = clientName + DataRep.EmailPrefix;

            _clientId = _apiFactory
                .ChangeContext<ICreateClientApi>()
                .CreateClientRequest(_crmUrl, clientName);

            // get sales statuses text
            _salesStatusText = _apiFactory
               .ChangeContext<IGeneralTabApi>()
               .GetSalesStatusRequest(_crmUrl);

            // add status name
            _oldSalesStatusName = TextManipulation.RandomString();

            // create new sales status
            _apiFactory
               .ChangeContext<IGeneralTabApi>()
               .PutSalesStatusRequest(_crmUrl, _salesStatusText,
               _oldSalesStatusName);

            // get client card information tab
            var informationTab = _apiFactory
               .ChangeContext<IInformationTabApi>()
               .GetInformationTabRequest(_crmUrl, _clientId)
               .GeneralResponse
               .informationTab;

            informationTab.sales_status = _oldSalesStatusName;
            informationTab.saving_account_id = "null";
            informationTab.sales_agent = "null";

            // update client card information tab with the new sales status
            _apiFactory
                .ChangeContext<IInformationTabApi>()
                .PutInformationTabRequest(_crmUrl, informationTab);
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

        // based on bug https://airsoftltd.atlassian.net/browse/AIRV2-5372
        [Test]
        [Category(DataRep.SanityCategory)]
        [Category(DataRep.RegressionCategory)]
        [RetryMenage(DataRep.NumOfRetraiesForFallingTest)]
        public void VerifyDeleteAndEditSalesStatusInSettingsApiTest()
        {
            var expectedNewSalesStatus = _oldSalesStatusName + "Automation";
            var expectedErrorWhenDeleteAssignSalesStatus = "This sale status cannot be delete due to assigned clients";

            // get sales status
            _salesStatusText = _apiFactory
               .ChangeContext<IGeneralTabApi>()
               .GetSalesStatusRequest(_crmUrl);

            // update the old sales status with new name
            _apiFactory
                .ChangeContext<IGeneralTabApi>()
                .PutSalesStatusRequest(_crmUrl, _salesStatusText,
                expectedNewSalesStatus, _oldSalesStatusName);

            // get sales status value from client card 
            var actualNewSalesStatus = _apiFactory
                .ChangeContext<IInformationTabApi>()
                .GetInformationTabRequest(_crmUrl, _clientId)
                .GeneralResponse
                .informationTab
                .sales_status;

            // delete sales status
            var actualErrorWhenDeleteAssignSalesStatus = _apiFactory
                .ChangeContext<IGeneralTabApi>()
                .PutSalesStatusRequest(_crmUrl, _salesStatusText,
                oldStatusName: expectedNewSalesStatus, checkStatusCode: false);

            // check if the new sales status exist in sales status filter on clients
            var actualNewSalesStatusExist = _apiFactory
                .ChangeContext<IHandleFiltersApi>()
                .GetSalesStatusTextFilter(_crmUrl)
                .Any(p => p.Key == expectedNewSalesStatus);

            Assert.Multiple(() =>
            {
                Assert.True(actualNewSalesStatus == expectedNewSalesStatus,
                    $" expected New Sales Status: {expectedNewSalesStatus}" +
                    $" actual New Sales Status: {actualNewSalesStatus}");

                Assert.True(actualErrorWhenDeleteAssignSalesStatus == expectedErrorWhenDeleteAssignSalesStatus,
                    $" expected Delete Assign Sales Status: {expectedErrorWhenDeleteAssignSalesStatus}" +
                    $" actual Delete Assign Sales Status: {actualErrorWhenDeleteAssignSalesStatus}");

                Assert.True(actualNewSalesStatusExist,
                    $" expected New Sales Status filter value Exist sales on clients: true" +
                    $" actual New Sales Status filter value Exist sales on clients: {actualNewSalesStatusExist}");
            });
        }
    }
}