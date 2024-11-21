// Ignore Spelling: Api

using AirSoftAutomationFramework.Internals.DAL.ConfigurationManagement;
using AirSoftAutomationFramework.Internals.Helpers;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.Clients.ClientCard.FinancesTab;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.Clients.ClientPage;
using NUnit.Framework;
using System;
using AirSoftAutomationFramework.Internals.Factorys;
using AirSoftAutomationFramework.Internals.NunitAtributes;
using TestsProject.TestsInternals;

namespace TestsProject.Tests.Emails
{
    [TestFixture]
    public class VerifyErrorMessageTradingToDeleteWithdrawalBonusApi : TestSuitBase
    {
        #region Test Preparation
        private IApplicationFactory _apiFactory = new ApplicationFactory();
        private string _crmUrl = Config.appSettings.CrmUrl;
        private string _bonusId;

        [SetUp]
        public void SetUp()
        {
            BeforeTest();
            var bonusAmount = 10000;

            // create client 
            var clientName = TextManipulation.RandomString();

            var clientId = _apiFactory
                .ChangeContext<ICreateClientApi>()
                .CreateClientRequest(_crmUrl, clientName,
              emailPrefix: DataRep.TestimEmailPrefix);

            // create bonus
            _bonusId = _apiFactory
                .ChangeContext<IFinancesTabApi>()
                .PostBonusRequest(_crmUrl, clientId, bonusAmount)
                .GeneralResponse
                .InsertId;

            // withdrawal bonus
            _apiFactory
                .ChangeContext<IFinancesTabApi>()
                .PostWithdrawalBonusRequest(_crmUrl, clientId, bonusAmount);
        }

        [TearDown]
        public void TearDown()
        {
            try
            {
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
        [RetryMenage(DataRep.NumOfRetriesForFallingTest)]
        public void VerifyErrorMessageTradingToDeleteWithdrawalBonusApiTest()
        {
            var expectedDeleteBonusErrorMessage = "You can't delete this bonus" +
                " because it's already used in a withdrawal , please contact support" +
                " for more information";

            // withdrawal bonus
            var actualDeleteBonusErrorMessage = _apiFactory
                .ChangeContext<IFinancesTabApi>()
                .DeleteFinanceItemRequest(_crmUrl, _bonusId, checkStatusCode: false);

            Assert.Multiple(() =>
            {
                Assert.True(actualDeleteBonusErrorMessage == expectedDeleteBonusErrorMessage,
                    $" expected Delete Bonus Error Message {expectedDeleteBonusErrorMessage}" +
                    $" actual Delete Bonus Error Message: {actualDeleteBonusErrorMessage}");
            });
        }
    }
}