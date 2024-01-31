// Ignore Spelling: Crm Api

using System;
using System.Linq;
using AirSoftAutomationFramework.Internals.DAL.ConfigurationManagement;
using AirSoftAutomationFramework.Internals.Factorys;
using AirSoftAutomationFramework.Internals.Helpers;
using AirSoftAutomationFramework.Internals.NunitAtributes;
using AirSoftAutomationFramework.Objects.DTOs;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.Clients.ClientPage;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Trade;
using Microsoft.Graph.Models;
using Newtonsoft.Json;
using NUnit.Framework;
using TestsProject.TestsInternals;

namespace TestsProject.Tests.CrmTrade.BulkTradingPage
{
    [TestFixture]
    public class VerifyOpenBulkTradeWhenNotEnoughMoneyMessageApi : TestSuitBase
    {
        #region Test Preparation

        private IApplicationFactory _apiFactory = new ApplicationFactory();
        private string _crmUrl = Config.appSettings.CrmUrl;
        private string _clientId;
        private string _clientName;
        private string _bulkTradeId;   

        [SetUp]
        public void SetUp()
        {
            BeforeTest();
            #region PreCondition
            // create client 
            _clientName = TextManipulation.RandomString();

            _clientId = _apiFactory
                .ChangeContext<ICreateClientApi>()
                .CreateClientRequest(_crmUrl, _clientName);

            // create bulk trades 
            _bulkTradeId = _apiFactory
                .ChangeContext<IBulkTradePageApi>()
                .PostCreateBulkTradeRequest(_crmUrl, new string[] { _clientId });
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
        public void VerifyOpenBulkTradeWhenNotEnoughMoneyMessageApiTest()
        {
            var expectedNotEnoughMoneyError = "not enough money";

            var actualNotEnoughMoneyError = _apiFactory
                .ChangeContext<IBulkTradePageApi>()
                .GetMassTradeByIdRequest(_crmUrl, _bulkTradeId)
                .FirstOrDefault();

            Assert.Multiple(() =>
            {
                Assert.True(actualNotEnoughMoneyError?.error == expectedNotEnoughMoneyError,
                    $" expected Not Enough Money Error: {expectedNotEnoughMoneyError}" +
                    $" actual Not Enough Money Error: {actualNotEnoughMoneyError?.error}");

                Assert.True(actualNotEnoughMoneyError.full_name == $"{_clientName} {_clientName}",
                    $" expected full name: {_clientName} {_clientName}" +
                    $" actual full name: {actualNotEnoughMoneyError.full_name}");

                Assert.True(actualNotEnoughMoneyError.user_id == _clientId,
                    $" expected user id: {_clientId}" +
                    $" actual user id: {actualNotEnoughMoneyError.user_id}");
            });
        }
    }
}