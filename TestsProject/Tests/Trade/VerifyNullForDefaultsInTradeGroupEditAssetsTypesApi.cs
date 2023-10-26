// Ignore Spelling: Api

using AirSoftAutomationFramework.Internals.DAL.ConfigurationManagement;

using AirSoftAutomationFramework.Internals.Helpers;
using AirSoftAutomationFramework.Internals.NunitAttributes;
using NUnit.Framework;
using TestsProject.TestsInternals;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Trade;
using System.Collections.Generic;
using AirSoftAutomationFramework.Objects.DTOs;
using AirSoftAutomationFramework.Objects.ForexObjects.Ui.Cfd;
using AirSoftAutomationFramework.Objects.ForexObjects.Ui.UiGeneral;
using OpenQA.Selenium;
using AirSoftAutomationFramework.Objects.ForexObjects.Ui.Login;
using MongoDB.Driver.Linq;
using System.Linq;
using AirSoftAutomationFramework.Internals.ExtensionsMethods;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.AccountsApi.UsersMenuApi.UserMenuApi;
using System;
using AirSoftAutomationFramework.Internals.Factory;

namespace TestsProject.Tests.Trade
{
    [TestFixture(DataRep.Chrome)]
    public class VerifyNullForDefaultsInTradeGroupEditAssetsTypesApi : TestSuitBase
    {
        public VerifyNullForDefaultsInTradeGroupEditAssetsTypesApi(string browser) : base(browser)
        {
            _browserName = browser;
        }

        #region Test Preparation
        private IApplicationFactory _apiFactory = new ApplicationFactory();
        private string _crmUrl = Config.appSettings.CrmUrl;
        private IWebDriver _driver;
        private string _browserName;
        private string _groupId;
        private string cryptoGroupName = TextManipulation.RandomString();

        [SetUp]
        public void SetUp()
        {
            #region PreCondition
            BeforeTest(_browserName);
            _driver = GetDriver();

            // create user
            var userName = TextManipulation.RandomString();

            // create user
            _apiFactory
                .ChangeContext<IUserApi>(_driver)
                .CreateUserForUiPipe(_crmUrl, userName);

            // login
            _apiFactory
                .ChangeContext<ILoginPageUi>(_driver)
                .LoginPipe(userName);

            // set trade group default Attributes
            var defaultAttr = new Default_Attr
            {
                commision = 0,
                leverage = 1,
                maintenance = 1,
                minimum_amount = 1,
                minimum_step = 1,
                spread = 0,
                margin_call = 1,
                swap_long = 0.04,
                swap_short = 0.04,
                swap_time = "00:00:00",
            };

            // set trade group forex Attributes
            var forexAttr = new Forex
            {
                commision = 1,
                leverage = 1,
                maintenance = 1,
                minimum_amount = 1,
                minimum_step = 1,
                spread = 1,
                margin_call = 1,
                swap_long = 1,
                swap_short = 1,
                swap_time = "00:00:00",
            };

            var tradeGroupAssetList = new List<object>() 
            {
                { defaultAttr }, { forexAttr }
            };

            // create trade group  
            _groupId = _apiFactory
                .ChangeContext<ITradeGroupApi>()
                .PostCreateTradeGroupRequest(_crmUrl,
                tradeGroupAssetList, cryptoGroupName);
            #endregion
        }

        [TearDown]
        public void TearDown()
        {
            try
            {
                _apiFactory
                    .ChangeContext<ITradeGroupApi>(_driver)
                    .DeleteTradeGroupRequest(_crmUrl, _groupId);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                AfterTest(_driver);
                DriverDispose(_driver);
            }
        }
        #endregion


        [Test]
        [Description("based on jira https://airsoftltd.atlassian.net/browse/AIRV2-5163")]
        [Category(DataRep.SanityCategory)]
        [Category(DataRep.RegressionCategory)]
        [RetryMenage(DataRep.NumOfRetraiesForFallingTest)]
        public void VerifyNullForDefaultsInTradeGroupEditAssetsTypesApiTest()
        {
            var expectedradeGroupForexValues = new List<string>();

            // assign default values to forex
            _apiFactory
                .ChangeContext<IMenus>(_driver)
                .ClickOnMenuItem<ITradeGroupsUi>()
                .SearchTradeGroup(_groupId)
                .ClickOnEditGroupButton()
                .ClickOnCryptoGroupTabByName("Assets types setting")
                .ClickOnEditAssetType("forex")
                .ClickOnDefaultCheckbox()
                .ClickOnOkBtn()
                .ClickOnCryptoGroupTabByName("General")
                .ClickOnSaveBtn();

            var tradeGroup = _apiFactory
                .ChangeContext<ITradeGroupApi>()
                .GetTradeGroupsRequest(_crmUrl)
                .GeneralResponse
                .Where(p => p._id == _groupId)
                .FirstOrDefault()
                .forex;

            // get the default values from forex
            tradeGroup
                .GetType()
                .GetProperties()
                .ForEach(p => expectedradeGroupForexValues.Add(p.GetValue(tradeGroup)?
                .ToString()));

            Assert.True(expectedradeGroupForexValues.All(p => p == null),
                $" actual trade Group Forex Values: {expectedradeGroupForexValues.ListToString()}" +
                $" expected trade Group Forex Values: null");
        }
    }
}