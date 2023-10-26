using System.Linq;
using AirSoftAutomationFramework.Internals.DAL.ConfigurationManagement;
using AirSoftAutomationFramework.Internals.Factory;
using AirSoftAutomationFramework.Internals.Helpers;

using AirSoftAutomationFramework.Objects.DTOs;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.AccountsApi.UsersMenuApi.UserMenuApi;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.Clients.ClientCard.FinancesTab;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.Clients.ClientPage;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.Settings;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Trade;
using AirSoftAutomationFramework.Objects.ForexObjects.TradingPlatform.TradeObjects.TradeApi;
using AirSoftAutomationFramework.Objects.MgmObjects.Api;
using NUnit.Framework;
using TestsProject.TestsInternals;

namespace TestsProject.Tests.Mgm_tests.Dashboard
{
    [TestFixture]
    public class VerifyUpdateBrand : TestSuitBase
    {      

        private IApplicationFactory _apiFactory = new ApplicationFactory();
        private GetLoginResponse _mgmLoginData;

        [SetUp]
        public void SetUp()
        {
            #region PreCondition  
            BeforeTest();

            _mgmLoginData = _apiFactory
                .ChangeContext<IMgmCreateUserApi>()
                .PostMgmLoginCookiesRequest(DataRep.MgmUrl,
                DataRep.MgmUserName.Split('@').First());
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
        [Category(DataRep.MgmSanityCategory)]
        public void VerifyUpdateBrandTest()
        {
            var actualBrandsActivities = _apiFactory
                .ChangeContext<IMgmDashboardApi>()
                .PostUpdateBrandsRequest(DataRep.MgmUrl, DataRep.MgmQaDevAutoId, _mgmLoginData);         
        }
    }
}