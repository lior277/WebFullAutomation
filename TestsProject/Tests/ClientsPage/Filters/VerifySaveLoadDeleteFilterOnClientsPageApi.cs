// Ignore Spelling: Api

using AirSoftAutomationFramework.Internals.DAL.ConfigurationManagement;
using AirSoftAutomationFramework.Internals.Factory;
using AirSoftAutomationFramework.Internals.Helpers;
using AirSoftAutomationFramework.Internals.NunitAttributes;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.AccountsApi.UsersMenuApi.UserMenuApi;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.Clients.ClientPage;
using AirSoftAutomationFramework.Objects.ForexObjects.ApiGeneral;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using TestsProject.TestsInternals;

namespace TestsProject.Tests.ClientsPage.Filters
{
    [NonParallelizable]
    [TestFixture]
    public class VerifySaveLoadDeleteFilterOnClientsPageApi : TestSuitBase
    {
        private IApplicationFactory _apiFactory = new ApplicationFactory();
        private string _crmUrl = Config.appSettings.CrmUrl;
        private string _clientName;
        private string _secondFilterName;
        private string _userId;
        private string _filterCommponent;
        private string _userName;
        private string _clientId;

        [SetUp]
        public void SetUp()
        {
            BeforeTest();
            #region PreCondition                  

            #region create user
            // create user
            _userName = TextManipulation.RandomString();

            // create user
            _userId = _apiFactory
             .ChangeContext<IUserApi>()
             .PostCreateUserRequest(_crmUrl, _userName);

            // create client 
            _clientName = TextManipulation.RandomString();

            _clientId = _apiFactory
                .ChangeContext<ICreateClientApi>()
                .CreateClientRequest(_crmUrl, _clientName);

            // connect  User To  Client 
            _apiFactory
                .ChangeContext<IClientsApi>()
                .PatchMassAssignSaleAgentsRequest(_crmUrl, _userId,
                new List<string> { _clientId });
            #endregion
        }
        #endregion

        [TearDown]
        public void TearDown()
        {
            try
            {
                // delete first filter
                _apiFactory
                   .ChangeContext<IHandleFiltersApi>()
                   .DeleteFilterRequest(_crmUrl, _secondFilterName, _filterCommponent);
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

        [Test]
        [Description("https://airsoftltd.atlassian.net/browse/AIRV2-4806")]
        [Category(DataRep.SanityCategory)]
        [Category(DataRep.RegressionCategory)]
        [RetryMenage(DataRep.NumOfRetraiesForFallingTest)]
        public void VerifySaveLoadDeleteFilterOnClientsPageApiTest()
        {
            var firstFilterName = $"{_clientName}FirstFilter";
            _secondFilterName = $"{_clientName}SecondFilter";
            var expectedLable = "sales_agents";
            _filterCommponent = "clients";

            var expectedDuplicateFilterNameMessage = "The filter could not be" +
                " saved due to a duplicate filter name";

            var expectedFirstFilterDefaultValue = false;
            var expectedSecondFilterDefaultValue = true;
            var expectedFirstFilterTimeValue = "month";

            // create first filter
            _apiFactory
                .ChangeContext<IHandleFiltersApi>()
                .PostCreateFilterRequest(_crmUrl, firstFilterName, true, _userId,
                _userName, expectedLable, _filterCommponent);

            Thread.Sleep(2000);

            // Verify first Filter Created
            _apiFactory
                .ChangeContext<IHandleFiltersApi>()
                .VerifyFilterCreated(_crmUrl, firstFilterName, _filterCommponent);

            // create filter with the same name for duplicate error
            var actualDuplicateFilterNameMessage = _apiFactory
                .ChangeContext<IHandleFiltersApi>()
                .PostCreateFilterRequest(_crmUrl, firstFilterName, true, _userId,
                _userName, expectedLable, _filterCommponent, checkStatusCode: false);

            // create second filter with default
            _apiFactory
                .ChangeContext<IHandleFiltersApi>()
                .PostCreateFilterRequest(_crmUrl, _secondFilterName, true, _userId,
                _userName, expectedLable, _filterCommponent);

            // Verify second Filter Created
            _apiFactory
                .ChangeContext<IHandleFiltersApi>()
                .VerifyFilterCreated(_crmUrl, _secondFilterName, _filterCommponent);

            #region delete the second filter again because the bug with the default
            // delete second filter
            _apiFactory
               .ChangeContext<IHandleFiltersApi>()
               .DeleteFilterRequest(_crmUrl, _secondFilterName, _filterCommponent);

            // create second filter with default
            _apiFactory
                .ChangeContext<IHandleFiltersApi>()
                .PostCreateFilterRequest(_crmUrl, _secondFilterName, true, _userId,
                _userName, expectedLable, _filterCommponent);

            // Verify second Filter Created
            _apiFactory
                .ChangeContext<IHandleFiltersApi>()
                .VerifyFilterCreated(_crmUrl, _secondFilterName, _filterCommponent);

            Thread.Sleep(3000);
            #endregion
            // get clients
            var clients = _apiFactory
                .ChangeContext<IHandleFiltersApi>()
                .GetFiltersRequest(_crmUrl, _filterCommponent)
                .FirstOrDefault()
                .Filters
                .Clients;

            // get first filter default value
            var actualFirstFilterDefaultValue = clients
                .Where(p => p.Name == firstFilterName)
                .FirstOrDefault()?
                .Default;

            // get first filter label value
            var actualFirstFilterLableValue = clients
                .Where(p => p.Name == firstFilterName)
                .FirstOrDefault()
                .ClientsData
                .FirstOrDefault()?
                .Label;

            // get second filter default value
            var actualSecondFilterDefaultValue = clients
                .Where(p => p.Name == _secondFilterName)
                .FirstOrDefault()?
                .Default;

            // get first filter time
            var actualFirstFilterTimeValue = clients
                .Where(p => p.Name == firstFilterName)
                .FirstOrDefault()
                .TimeFilter;

            // delete first filter
            _apiFactory
               .ChangeContext<IHandleFiltersApi>()
               .DeleteFilterRequest(_crmUrl, firstFilterName, _filterCommponent);

            // get first filter label value
            clients = _apiFactory
                .ChangeContext<IHandleFiltersApi>()
                .GetFiltersRequest(_crmUrl, _filterCommponent)
                .FirstOrDefault()
                .Filters
                .Clients;

            // get first filter 
            var actualFirstFilterOccurrence = clients
                .Where(p => p.Name == firstFilterName)
                .Count();

            Assert.Multiple(() =>
            {
                Assert.True(actualFirstFilterDefaultValue == expectedFirstFilterDefaultValue,
                    $" expected First Filter Default Value: {expectedFirstFilterDefaultValue}" +
                    $" actual First Filter Default Value: {actualFirstFilterDefaultValue}");

                Assert.True(actualSecondFilterDefaultValue == expectedSecondFilterDefaultValue,
                    $" expected Second Filter Default Value: {expectedSecondFilterDefaultValue}" +
                    $" actual Second Filter Default Value: {actualSecondFilterDefaultValue}");

                Assert.True(actualDuplicateFilterNameMessage.Contains(expectedDuplicateFilterNameMessage),
                    $" expected Duplicate Filter Name Message: {expectedDuplicateFilterNameMessage}" +
                    $" actual Second Duplicate Filter Name Message: {actualDuplicateFilterNameMessage}");

                Assert.True(actualFirstFilterTimeValue == expectedFirstFilterTimeValue,
                    $" expected First Filter Time Value: {expectedFirstFilterTimeValue}" +
                    $" actual First Filter Time Value: {actualSecondFilterDefaultValue}");

                Assert.True(actualFirstFilterOccurrence == 0,
                    $" expected First Filte Occurrence after delete: 0" +
                    $" actual First Filte Occurrence after delete: {actualFirstFilterOccurrence}");

                Assert.True(actualFirstFilterLableValue == expectedLable,
                   $" expected First Filter Lable Value: {expectedLable}" +
                   $" actual First Filter Lable Value: {actualFirstFilterLableValue}");
            });
        }
    }
}