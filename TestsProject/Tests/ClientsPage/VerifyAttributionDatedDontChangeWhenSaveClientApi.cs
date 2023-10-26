// Ignore Spelling: Api Dont

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using AirSoftAutomationFramework.Internals.DAL.ConfigurationManagement;
using AirSoftAutomationFramework.Internals.Factory;
using AirSoftAutomationFramework.Internals.Helpers;
using AirSoftAutomationFramework.Internals.NunitAttributes;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.AccountsApi.UsersMenuApi.UserMenuApi;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.Clients.ClientCard;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.Clients.ClientPage;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Trade;
using NUnit.Framework;
using TestsProject.TestsInternals;

namespace TestsProject.Tests.ClientsPage
{
    [TestFixture]
    public class VerifyAttributionDatedDontChangeWhenSaveClientApi : TestSuitBase
    {
        #region Test Preparation
        private IApplicationFactory _apiFactory = new ApplicationFactory();
        private string _crmUrl = Config.appSettings.CrmUrl;
        private string _clientId;
        private string _userId;
        private string _clientEmail;   
        private List<string> _tradeGroupsIdsListForDelete = new List<string>();
        private string _tradingPlatformUrl =  Config.appSettings.tradingPlatformUrl;

        [SetUp]
        public void SetUp()
        {
            BeforeTest();

            // create user
            var userName = TextManipulation.RandomString();

            _userId = _apiFactory
             .ChangeContext<IUserApi>()
             .PostCreateUserRequest(_crmUrl, userName);

            // create client 
            var clientName = TextManipulation.RandomString();
            _clientEmail = clientName + DataRep.EmailPrefix;

            _clientId = _apiFactory
                .ChangeContext<ICreateClientApi>()
                .CreateClientRequest(_crmUrl, clientName);
        }

        [TearDown]
        public void TearDown()
        {
            try
            {
                _apiFactory
                    .ChangeContext<ITradeGroupApi>()
                    .DeleteTradeGroupRequest(_crmUrl, _tradeGroupsIdsListForDelete);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
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
        public void VerifyAttributionDatedDontChangeWhenSaveClientApiTest()
        {
            var expectedMarginCallNotifiedDate = DateTime.Now.ToString("yyyy-MM-dd");

           var AttributionDateWhenCreateClient =  _apiFactory
                .ChangeContext<IClientsApi>()
                .GetClientRequest(_crmUrl, _clientId)
                .GeneralResponse
                .data
                .FirstOrDefault()
                .attribution_date;

            // connect One User To One Client notification
            _apiFactory
                .ChangeContext<IClientsApi>()
                .PatchMassAssignSaleAgentsRequest(_crmUrl, _userId,
                new List<string> { _clientId });

            var attributionDateAfterAssign = _apiFactory
                .ChangeContext<IClientsApi>()
                .GetClientRequest(_crmUrl, _clientId)
                .GeneralResponse
                .data
                .FirstOrDefault()
                .attribution_date;

            var informationTabResponse = _apiFactory
               .ChangeContext<IInformationTabApi>()
               .GetInformationTabRequest(_crmUrl, _clientId)
               .GeneralResponse
               .informationTab;

            informationTabResponse.saving_account_id = "null";

            // wait before save to see changes in time stamp
            Thread.Sleep(200);

            // save client
            _apiFactory
                .ChangeContext<IInformationTabApi>()
                .PutInformationTabRequest(_crmUrl, informationTabResponse);

            var attributionDateAfterSaveClient = _apiFactory
                .ChangeContext<IClientsApi>()
                .GetClientRequest(_crmUrl, _clientId)
                .GeneralResponse
                .data
                .FirstOrDefault()
                .attribution_date;

            Assert.Multiple(() =>
            {
                Assert.True(AttributionDateWhenCreateClient != attributionDateAfterAssign,
                    $" Attribution Date When Create Client {AttributionDateWhenCreateClient}" +
                    $" Attribution Date When After Assign: {attributionDateAfterAssign}");

                Assert.True(attributionDateAfterAssign == attributionDateAfterSaveClient,
                    $" Attribution Date When After Assign {AttributionDateWhenCreateClient}" +
                    $" Attribution Date When After Save Client,: {attributionDateAfterAssign}");
            });
        }
    }
}