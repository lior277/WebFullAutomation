// Ignore Spelling: Api

using System.Collections.Generic;
using System.Linq;
using AirSoftAutomationFramework.Internals.DAL.ConfigurationManagement;
using AirSoftAutomationFramework.Internals.Factory;
using AirSoftAutomationFramework.Internals.Helpers;
using AirSoftAutomationFramework.Internals.NunitAttributes;

using AirSoftAutomationFramework.Objects.DTOs;
using AirSoftAutomationFramework.Objects.MgmObjects.Api;
using ConsoleApp;
using NUnit.Framework;
using TestsProject.TestsInternals;

namespace TestsProject.Tests.Mgm_tests
{
    [TestFixture]
    public class VerifyCreateAssetApi : TestSuitBase
    {
        private IApplicationFactory _apiFactory = new ApplicationFactory();
        private string _crmUrl = Config.appSettings.CrmUrl;     
        private string _attributionRoleName;
        private List<string> _tradeGroupsIdsListForDelete = new List<string>();
        private QaAutomation01Context _dbContext = new QaAutomation01Context();
        private string _tradingPlatformUrl = 
             Config.appSettings.tradingPlatformUrl;

        private GetLoginResponse _mgmLoginData;

        [SetUp]
        public void SetUp()
        {
            BeforeTest();
            _attributionRoleName = TextManipulation.RandomString();

            _mgmLoginData = _apiFactory
                .ChangeContext<IMgmCreateUserApi>()
                .PostMgmLoginCookiesRequest(DataRep.MgmUrl,
                DataRep.MgmUserName.Split('@').First());

            // get MGM login data
            _apiFactory
                .ChangeContext<IAssetsApi>()
                .CreateAssetPipe(DataRep.MgmUrl, _mgmLoginData);
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
        [Category(DataRep.MgmSanityCategory)]
        [RetryMenage(DataRep.NumOfRetraiesForFallingTest)]
        public void VerifySyncAssetApiTest()
        {
            #region Expected data
            var expectedIconMain = "ad";
            var expectedKey= DataRep.AssetNameForCreateAsset;
            var expectedLable= DataRep.AssetNameForCreateAsset;
            var expectedType = "forex";
            var expectedIbId = 1;
            var expectedIbExchang = "ASX";
            var expectedIbSecType = "forex";
            var expectedCurrency = "AUD";
            var expectedDecimalDigits = 2;
            var expectedSymbol = DataRep.AssetNameForCreateAsset;
            var expectedAssetIsFuture = false;
            var expectedAssetIsContract = false;
            var expectedBlombergKey = "BLOOMBERG KEY";
            #endregion

            // get MGM login data
            var actualAsset = _apiFactory
                .ChangeContext<IAssetsApi>()
                .GetAssetsRequest(DataRep.MgmUrl, _mgmLoginData)
                .Where(p => p.label == DataRep.AssetNameForCreateAsset)
                .FirstOrDefault();

            Assert.Multiple(() =>
            {
                Assert.True(actualAsset.icon_main == expectedIconMain,
                    $" expected icon main: {expectedIconMain}" +
                    $" actual icon main: {actualAsset.icon_main}");

                Assert.True(actualAsset.symbol == expectedKey,
                    $" expected symbol: {expectedKey}" +
                    $" actual symbol: {actualAsset.symbol}");

                Assert.True(actualAsset.label == expectedLable,
                    $" expected label: {expectedLable}" +
                    $" actual label: {actualAsset.label}");

                Assert.True(actualAsset.category == expectedType,
                    $" expected category: {expectedType}" +
                    $" actual category: {actualAsset.category}");

                Assert.True(actualAsset.ib_id == expectedIbId,
                    $" expected ib_id: {expectedIbId}" +
                    $" actual ib_id: {actualAsset.ib_id}");

                Assert.True(actualAsset.exchange == expectedIbExchang,
                    $" expected exchange: {expectedIbExchang}" +
                    $" actual exchange: {actualAsset.exchange}");

                Assert.True(actualAsset.ib_sec_type == expectedIbSecType,
                    $" expected ib_sec_type: {expectedIbSecType}" +
                    $" actual ib_sec_type: {actualAsset.ib_sec_type}");

                Assert.True(actualAsset.currency == expectedCurrency,
                    $" expected currency: {expectedCurrency}" +
                    $" actual currency: {actualAsset.currency}");

                Assert.True(actualAsset.decimal_digits == expectedDecimalDigits,
                    $" expected decimal_digits: {expectedDecimalDigits}" +
                    $" actual decimal_digits: {actualAsset.decimal_digits}");

                Assert.True(actualAsset.asset_is_future == expectedAssetIsFuture,
                    $" expected asset_is_future: {expectedAssetIsFuture}" +
                    $" actual asset_is_future: {actualAsset.asset_is_future}");

                Assert.True(actualAsset.asset_is_contract == expectedAssetIsContract,
                    $" expected asset_is_contract: {expectedAssetIsContract}" +
                    $" actual asset_is_contract: {actualAsset.asset_is_contract}");

                Assert.True(actualAsset.bloomberg_key == expectedBlombergKey,
                    $" expected bloomberg_key: {expectedBlombergKey}" +
                    $" actual bloomberg_key: {actualAsset.bloomberg_key}");
            });
        }
    }
}