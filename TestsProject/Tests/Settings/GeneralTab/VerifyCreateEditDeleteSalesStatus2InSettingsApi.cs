using AirSoftAutomationFramework.Internals.DAL.ConfigurationManagement;
using AirSoftAutomationFramework.Internals.Helpers;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.Clients.ClientCard;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.Clients.ClientPage;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.Settings;
using AirSoftAutomationFramework.Objects.ForexObjects.ApiGeneral;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using AirSoftAutomationFramework.Internals.Factorys;
using AirSoftAutomationFramework.Internals.NunitAtributes;
using TestsProject.TestsInternals;

namespace TestsProject.Tests.Settings.GeneralTab
{
    [TestFixture]
    public class VerifyCreateEditDeleteSalesStatus2InSettingsApi : TestSuitBase
    {
        #region Test Preparation
        private IApplicationFactory _apiFactory = new ApplicationFactory();
        private string _crmUrl = Config.appSettings.CrmUrl;
        private string _clientId;  
        private Dictionary<string, string> _salesStatusText;

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
               .GetSalesStatus2Request(_crmUrl);
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
        [RetryMenage(DataRep.NumOfRetraiesForFallingTest)]
        public void VerifyCreateEditDeleteSalesStatus2InSettingsApiTest()
        {
            // add status name
            var newSalesStatusName = TextManipulation.RandomString();
            var expctedEditSalesStatus = newSalesStatusName + "Automation";

            // create new sales status 
            _apiFactory
                .ChangeContext<IGeneralTabApi>()
                .PutSalesStatus2Request(_crmUrl, _salesStatusText,
                newSalesStatusName, null);

            _salesStatusText = _apiFactory
                .ChangeContext<IGeneralTabApi>()
                .GetSalesStatus2Request(_crmUrl);

            var actualNewSaleStatus = _salesStatusText
                .Where(s => s.Key == newSalesStatusName)
                .FirstOrDefault();    

            // edit sales status
             _apiFactory
                .ChangeContext<IGeneralTabApi>()
                .PutSalesStatus2Request(_crmUrl, _salesStatusText,
                 expctedEditSalesStatus, newSalesStatusName);

            _salesStatusText = _apiFactory
                .ChangeContext<IGeneralTabApi>()
                .GetSalesStatus2Request(_crmUrl);

            var actualEditSaleStatus = _salesStatusText
              .Where(s => s.Key == expctedEditSalesStatus)
              .FirstOrDefault();

            // delete sales status
            _apiFactory
                .ChangeContext<IGeneralTabApi>()
                .PutSalesStatus2Request(_crmUrl, _salesStatusText,
                 oldStatusName: expctedEditSalesStatus);

            _salesStatusText = _apiFactory
                .ChangeContext<IGeneralTabApi>()
                .GetSalesStatus2Request(_crmUrl);

            var actualDeleteSaleStatus = _salesStatusText
              .Where(s => s.Key == expctedEditSalesStatus)
              .FirstOrDefault();

            Assert.Multiple(() =>
            {
                Assert.True(actualNewSaleStatus.Key == newSalesStatusName,
                    $" expected New Sales Status: {newSalesStatusName}" +
                    $" actual New Sales Status: {actualNewSaleStatus.Key}");

                Assert.True(actualEditSaleStatus.Key == expctedEditSalesStatus,
                    $" expected edit Status: {expctedEditSalesStatus}" +
                    $" actual edit Status: {actualEditSaleStatus.Key}");

                Assert.True(actualDeleteSaleStatus.Key == null,
                    $" expected delete Sales Status : null" +
                    $" actual delete Sales Status : {actualDeleteSaleStatus.Key}");
            });
        }
    }
}