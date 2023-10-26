// Ignore Spelling: Api

using System.Collections.Generic;
using System.Linq;
using AirSoftAutomationFramework.Internals.DAL.ConfigurationManagement;
using AirSoftAutomationFramework.Internals.Factory;
using AirSoftAutomationFramework.Internals.Helpers;
using AirSoftAutomationFramework.Internals.NunitAttributes;
using AirSoftAutomationFramework.Objects.DTOs;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.Clients.ClientPage;
using AirSoftAutomationFramework.Objects.ForexObjects.ApiGeneral.Login;
using AirSoftAutomationFramework.Objects.ForexObjects.TradingPlatform.TradeObjects.TradeApi;
using AirSoftAutomationFramework.Objects.MgmObjects.Api;
using AirSoftAutomationFramework.Objects.MgmObjects.Api.Risk.AssetsCfd;
using NUnit.Framework;
using TestsProject.TestsInternals;
using static AirSoftAutomationFramework.Objects.DTOs.GetCfdResponse;

namespace TestsProject.Tests.Mgm_tests
{
    [TestFixture]
    public class VerifySyncAssetApi : TestSuitBase
    {
        public VerifySyncAssetApi() { }

        private IApplicationFactory _apiFactory = new ApplicationFactory();
        private string _crmUrl = Config.appSettings.CrmUrl;     
        private string _attributionRoleName;
        private string _tradingPlatformUrl = Config.appSettings.tradingPlatformUrl;
        private GetLoginResponse _mgmLoginData;
        private string _removeAddAssetName = "USD/JPY";
        private List<AssetData> _cfdData;
        private string _brandId;
        private GetLoginResponse _tragdingLoginData;

        [SetUp]
        public void SetUp()
        {
            BeforeTest();
            _attributionRoleName = TextManipulation.RandomString();

            // get MGM login data
            _mgmLoginData = _apiFactory
                .ChangeContext<IMgmCreateUserApi>()
                .PostMgmLoginCookiesRequest(DataRep.MgmUrl,
                DataRep.MgmUserName.Split('@').First());

            // create client 
            var clientName = TextManipulation.RandomString();
            var clientEmail = clientName + DataRep.EmailPrefix;

            _apiFactory
                .ChangeContext<ICreateClientApi>()
                .CreateClientRequest(_crmUrl, clientName);

            // get TP login data
            _tragdingLoginData = _apiFactory
                .ChangeContext<ILoginApi>()
                .PostLoginToTradingPlatform(_tradingPlatformUrl, clientEmail)
                .GeneralResponse;

            _brandId = _apiFactory
                .ChangeContext<IMgmDashboardApi>()
                .GetBrandsRequest(DataRep.MgmUrl, _mgmLoginData)
                .FirstOrDefault()
                .Id;

            // remove the asset if exist
            _apiFactory
                .ChangeContext<ITradePageApi>()
                .RemoveAssetPipe(_tradingPlatformUrl,
                _removeAddAssetName, _mgmLoginData, _tragdingLoginData);

            // update brands sync assets
            _apiFactory
               .ChangeContext<IAssetsCfdApi>()
               .PatchMgmFrontAssetsBrandsDeployCfdRequest(DataRep.MgmUrl, _brandId, _mgmLoginData);

            _cfdData = _apiFactory
                .ChangeContext<IAssetsCfdApi>()
                .GetCfdRequest(DataRep.MgmUrl, _mgmLoginData);     
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
            // set show_in_front to true
            var asset = _cfdData.Where(p => p.label == _removeAddAssetName)
                 .FirstOrDefault();

            asset
                .cfd
                .show_in_front = true;

            _apiFactory
                .ChangeContext<IAssetsCfdApi>()
                .PatchCfdRequest(_mgmLoginData, asset);

            // update brands sync assets
            _apiFactory
                .ChangeContext<IAssetsCfdApi>()
                .PatchMgmFrontAssetsBrandsDeployCfdRequest(DataRep.MgmUrl, _brandId, _mgmLoginData);

            var checkIfAssetsExist = _apiFactory
                .ChangeContext<ITradePageApi>()
                .GetCfdAssetsNamesRequest(_tradingPlatformUrl, _tragdingLoginData)
                .Any(p => p.label.Equals(_removeAddAssetName));

            Assert.True(checkIfAssetsExist,
                $" actual Assets Exist ON tp: {checkIfAssetsExist}" +
                $" expected Assets Exist ON tp : true ");
        }
    }
}