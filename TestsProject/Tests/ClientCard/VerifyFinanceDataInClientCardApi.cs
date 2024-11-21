// Ignore Spelling: Api

using System.Linq;
using AirSoftAutomationFramework.Internals.DAL.ConfigurationManagement;
using AirSoftAutomationFramework.Internals.ExtensionsMethods;
using AirSoftAutomationFramework.Internals.Factorys;
using AirSoftAutomationFramework.Internals.Helpers;
using AirSoftAutomationFramework.Internals.NunitAtributes;
using AirSoftAutomationFramework.Objects.DTOs;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.AccountsApi.UsersMenuApi.UserMenuApi;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.Clients.ClientCard.FinancesTab;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.Clients.ClientPage;
using AirSoftAutomationFramework.Objects.ForexObjects.ApiGeneral.Login;
using NUnit.Framework;
using TestsProject.TestsInternals;

namespace TestsProject.Tests.ClientCard
{
    [TestFixture]
    [Parallelizable(ParallelScope.All)]
    public class VerifyFinanceDataInClientCardApi : TestSuitBase
    {
        [SetUp]
        public void SetUp()
        {
            BeforeTest();
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

        [Test]
        [TestCaseSource(typeof(FinanceFactoryApi),
            nameof(FinanceFactoryApi.RetrieveTestsFromFinanceDataOfClientCardTable))]
        [Category(DataRep.SanityCategory)]
        [Category(DataRep.RegressionCategory)]
        [RetryMenage(DataRep.NumOfRetriesForFallingTest)]
        public void VerifyFinanceDataInClientCardApiTest(TestCase testCase)
        {
            var apiFactory = new ApplicationFactory();
            var crmUrl = Config.appSettings.CrmUrl;
            var tradingPlatformUrl =  Config.appSettings.tradingPlatformUrl;

            // create user
            var userName = TextManipulation.RandomString();

            // create user
            var userId = apiFactory
             .ChangeContext<IUserApi>()
             .PostCreateUserRequest(crmUrl, userName);

            // create ApiKey
            var currentUserApiKey = apiFactory
                .ChangeContext<IUserApi>()
                .PostCreateApiKeyRequest(crmUrl, userId);

            // create client 
            var clientName = TextManipulation.RandomString();
            var clientEmail = clientName + DataRep.EmailPrefix;

            var clientId = apiFactory
                .ChangeContext<ICreateClientApi>()
                .CreateClientRequest(crmUrl, clientName,
                apiKey: currentUserApiKey);

            var loginData = apiFactory
                .ChangeContext<ILoginApi>()
                .PostLoginToTradingPlatform(tradingPlatformUrl, clientEmail)
                .GeneralResponse;

            var testData = apiFactory
                .ChangeContext<IFinanceFactoryApi>()
                .FinanceFactoryByName(clientId, loginData, currentUserApiKey, testCase)
                .GetFinanceData();

            var actualFinanceData = testData.Item1;
            var expectedFinanceData = testCase.expected_finance_data;
            var actualAccountValuesColumn = testData.Item2;
            var expectedAccountValuesColumn = testCase.account_value_values;
            var testCaseId = testCase.case_id;

            Assert.Multiple(() =>
            {
                Assert.True(actualFinanceData.available == expectedFinanceData.available,
                  $" testCaseId: {testCaseId}, expected _available: {expectedFinanceData.available} for client id: {clientId} " +
                  $" actual _available :  {actualFinanceData.available}");

                Assert.True(actualFinanceData.balance == expectedFinanceData.balance,
                  $" testCaseId: { testCaseId},expected _balance: {expectedFinanceData.balance} for client id: {clientId} " +
                  $" actual _balance :  {actualFinanceData.balance}");

                Assert.True(actualFinanceData.bonus == expectedFinanceData.bonus,
                  $" testCaseId: { testCaseId},expected bonus: {expectedFinanceData.bonus}  for client id: {clientId} " +
                  $" actual bonus :  {actualFinanceData.bonus}");

                Assert.True(actualFinanceData.equity == expectedFinanceData.equity,
                  $" testCaseId: { testCaseId},expected _equity: {expectedFinanceData.equity}  for client id: {clientId} " +
                  $" actual _equity :  {actualFinanceData.equity}");

                Assert.True(actualFinanceData.min_margin == expectedFinanceData.min_margin,
                  $" testCaseId: { testCaseId},expected min_margin: {expectedFinanceData.min_margin}  for client id: {clientId} " +
                  $" actual min_margin :  {actualFinanceData.min_margin}");

                Assert.True(actualAccountValuesColumn.CompareListOfDouble(expectedAccountValuesColumn).Count() == 0,
                  $" testCaseId: { testCaseId}, actual Account Values Column  for client id: {clientId} : { actualAccountValuesColumn.ListToString()} " +
                  $" expected Account Values Column: { expectedAccountValuesColumn.ListToString()}");
            });
        }
    }
}