using AirSoftAutomationFramework.Internals.DAL.ConfigurationManagement;
using AirSoftAutomationFramework.Internals.Helpers;
using AirSoftAutomationFramework.Objects.DTOs;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.Settings;
using NUnit.Framework;
using System;
using System.Xml.Linq;
using AirSoftAutomationFramework.Internals.Factorys;
using AirSoftAutomationFramework.Internals.NunitAtributes;
using TestsProject.TestsInternals;

namespace TestsProject.Tests.Settings.OfficeTab
{
    [TestFixture]
    public class VerifyLeaderBoardAccess : TestSuitBase
    {
        private IApplicationFactory _apiFactory = new ApplicationFactory();
        private string _crmUrl = Config.appSettings.CrmUrl;
        private string _officeName;
        private GetOfficeResponse _officeData;

        [SetUp]
        public void SetUp()
        {
            BeforeTest();
            #region PreCondition   
            _officeName = TextManipulation.RandomString();
            var pbxName = TextManipulation.RandomString();

            // create and get office
            _officeData = _apiFactory
                .ChangeContext<IOfficeTabApi>()
                .PostCreateOfficeRequest(_crmUrl, _officeName, pbxName)
                .GetOfficesByName(_crmUrl, _officeName);

            // update the office
            _apiFactory
                .ChangeContext<IOfficeTabApi>()
                .PutOfficeRequest(_crmUrl, _officeData);
            #endregion
        }

        [TearDown]
        public void TearDown()
        {
            try
            {
                // delete  office
                _apiFactory
                   .ChangeContext<IOfficeTabApi>()
                   .DeleteOfficeByIdRequest(_crmUrl, _officeData._id);
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

        // based on ticket https://airsoftltd.atlassian.net/browse/AIRV2-5247
        [Test]
        [Category(DataRep.SanityCategory)]
        [Category(DataRep.RegressionCategory)]
        [RetryMenage(DataRep.NumOfRetriesForFallingTest)]
        public void VerifyMakeCallWithTrunkClientCardTest()
        {
            var actualHasLeaderBoardNotChecked = _apiFactory
                .ChangeContext<IOfficeTabApi>()
                .GetLeaderBoardRequest(_crmUrl, _officeData._id);

            _officeData.sales_dashboard.active = true;
            _officeData.allowed_ip_addresses = Array.Empty<string>();

            // update the office
            _apiFactory
                .ChangeContext<IOfficeTabApi>()
                .PutOfficeRequest(_crmUrl, _officeData);

            var actualHasLeaderBoardNoIp = _apiFactory
                .ChangeContext<IOfficeTabApi>()
                .GetLeaderBoardRequest(_crmUrl, _officeData._id);

            Assert.Multiple(() =>
            {
                Assert.True(actualHasLeaderBoardNotChecked == "false",
                    $" expected Has LeaderBoard Not Checked: false," +
                    $" actual Has LeaderBoard Not Checked: {actualHasLeaderBoardNotChecked}");

                Assert.True(actualHasLeaderBoardNoIp == "false",
                    $" expected Has LeaderBoard No Ip: false," +
                    $" actual Has LeaderBoard No Ip: {actualHasLeaderBoardNoIp}");
            });
        }
    }
}