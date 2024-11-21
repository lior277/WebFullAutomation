// Ignore Spelling: Api

using AirSoftAutomationFramework.Internals.DAL.ConfigurationManagement;
using AirSoftAutomationFramework.Internals.Helpers;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.AccountsApi.UsersMenuApi;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.AccountsApi.UsersMenuApi.UserMenuApi;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.Clients.ClientCard;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.Clients.ClientPage;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.Settings;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using AirSoftAutomationFramework.Internals.Factorys;
using AirSoftAutomationFramework.Internals.NunitAtributes;
using TestsProject.TestsInternals;

namespace TestsProject.Tests.Dialer
{
    [TestFixture]
    public class VerifyDialerMassAssignApi : TestSuitBase
    {
        #region Test Preparation       
        private IApplicationFactory _apiFactory = new ApplicationFactory();
        private string _crmUrl = Config.appSettings.CrmUrl;
        private string _dialerApiKey;
        private List<string> _clientsIds;
        private string _userIdAirsoftOffice;
        private string _clientEmail;
        private string _clientIdForUserOfficeAirsoft;
        private string _clientIdUnasign;
        private string _dialerId;
        private string _comment = "Test comment";
        private string _salesStatus = "Call Back";

        [SetUp]
        public void SetUp()
        {
            BeforeTest();
            #region PreCondition
            // create dialer with single office
            var userName = TextManipulation.RandomString();

            _dialerId = _apiFactory
                .ChangeContext<IUserApi>()
                .PostCreateUserRequest(_crmUrl, userName,
                 role: DataRep.AdminWithDialerRole);

            // get dialer ApiKey
            _dialerApiKey = _apiFactory
                .ChangeContext<IUserApi>()
                .PostCreateApiKeyRequest(_crmUrl, _dialerId);

            // create admin user that belong to different office then the client 
            userName = TextManipulation.RandomString();

            _userIdAirsoftOffice = _apiFactory
                .ChangeContext<IUserApi>()
                .PostCreateUserRequest(_crmUrl, userName);

            // get airsoft User Data
            var airsoftUserData = _apiFactory
                .ChangeContext<IUsersApi>()
                .GetActiveUsersRequest(_crmUrl)
                .userData
                .Where(p => p._id.Equals(_userIdAirsoftOffice))
                .FirstOrDefault();

            // get airsoft office id
            var airsoftOfficeId = _apiFactory
                .ChangeContext<IOfficeTabApi>()
                .GetOfficesRequest(_crmUrl)
                .Where(p => p.city.Equals(DataRep.AirsoftOfficeName))
                .FirstOrDefault()
                ._id;

            airsoftUserData.office = airsoftOfficeId;

            // get admin airsoft user
            var user = _apiFactory
                .ChangeContext<IUsersApi>()
                .GetUserByIdRequest(_crmUrl, _userIdAirsoftOffice)
                .GeneralResponse;

            user.user.office = airsoftOfficeId;

            // assign new office to user
            _apiFactory
                .ChangeContext<IUserApi>()
                .PutEditUserOfficeRequest(_crmUrl, user);

            // create client for user office airsoft
            var clientName = TextManipulation.RandomString();
            _clientEmail = clientName + DataRep.EmailPrefix;

            _clientIdForUserOfficeAirsoft = _apiFactory
                .ChangeContext<ICreateClientApi>()
                .CreateClientRequest(_crmUrl, clientName);

            _clientsIds = new List<string> { _clientIdForUserOfficeAirsoft };

            // connect admin airsoft user to unassign Client 
            _apiFactory
                .ChangeContext<IClientsApi>()
                .PatchMassAssignSaleAgentsRequest(_crmUrl, _userIdAirsoftOffice, _clientsIds);

            // create client that is unassigned
            clientName = TextManipulation.RandomString();

            _clientIdUnasign = _apiFactory
                .ChangeContext<ICreateClientApi>()
                .CreateClientRequest(_crmUrl, clientName);

            _clientsIds.Add(_clientIdUnasign);

            // Add Mass Assign Comment
            _apiFactory
               .ChangeContext<IPlanningTabApi>()
               .PostAddMassAssignCommentRequest(_crmUrl, _clientsIds,
               _comment, _dialerApiKey);

            // add Mass Assign Sales Status
            _apiFactory
               .ChangeContext<IClientsApi>()
               .PatchMassAssignSalesStatusRequest(_crmUrl, _salesStatus,
               _clientsIds, _dialerApiKey);

            // add Mass Assign Sales agent
            _apiFactory
               .ChangeContext<IClientsApi>()
               .PatchMassAssignSaleAgentsRequest(_crmUrl,
               _dialerId, _clientsIds, _dialerApiKey);
            #endregion
        }
        #endregion

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

        /// <summary>
        /// pre condition
        /// user admin with dialer role belong to main office
        /// user admin belong to airsoft office
        /// create client that is unassigned
        /// create client and connect him to user admin belong to airsoft office
        /// create mass  Assign Sales Agent
        /// create mass  Assign Sales status
        /// create mass  comment
        /// verify new Sales status for the unassigned client
        /// verify Sales status not  change for the airsoft office client
        /// verify new Sales Agent for the unassigned client
        /// verify Sales Agent not  change for the airsoft office client
        /// verify new comment for the unassigned client
        /// verify no comment for the airsoft office client
        /// </summary>
        [Test]
        [Description("based on jira: https://airsoftltd.atlassian.net/browse/AIRV2-5359")]
        [Category(DataRep.ApiDocCategory)]
        [Category(DataRep.SanityCategory)]
        [Category(DataRep.RegressionCategory)]
        [RetryMenage(DataRep.NumOfRetriesForFallingTest)]
        public void VerifyDialerMassAssignApiTest()
        {
            var actualClientByIdUnasignResponse = _apiFactory
                .ChangeContext<IClientsApi>()
                .GetClientByIdRequest(_crmUrl, _clientIdUnasign)
                .GeneralResponse;

            var actualCommentByClientIdUnasignResponse = _apiFactory
                .ChangeContext<ICommentsTabApi>()
                .GetCommentsByClientIdRequest(_crmUrl, _clientIdUnasign)
                .GeneralResponse
                .FirstOrDefault()
                .Comment;

            var actualClientByIdAirsofOfficeResponse = _apiFactory
                .ChangeContext<IClientsApi>()
                .GetClientByIdRequest(_crmUrl, _clientIdForUserOfficeAirsoft)
                .GeneralResponse;

            var actualCommentByClientIdAirsofOfficeResponse = _apiFactory
                .ChangeContext<ICommentsTabApi>()
                .GetCommentsByClientIdRequest(_crmUrl, _clientIdForUserOfficeAirsoft)
                .GeneralResponse
                .FirstOrDefault()?
                .Comment;

            Assert.Multiple(() =>
            {
                Assert.True(actualClientByIdUnasignResponse.user.sales_agent.Equals(_dialerId),
                    $" actual sales agent for Unassigned client: {actualClientByIdUnasignResponse.user.sales_agent}, " +
                    $" expected sales agent: {_dialerId}");

                Assert.True(actualClientByIdUnasignResponse.user.sales_status.Equals(_salesStatus),
                    $" actual sales status for Unassigned client: {actualClientByIdUnasignResponse.user.sales_status}, " +
                    $" expected sales status for Unassigned client: {_salesStatus}");

                Assert.True(actualCommentByClientIdUnasignResponse == _comment,
                    $" actual Comment for Unassigned client: {actualCommentByClientIdUnasignResponse}, " +
                    $" expected Comment for Unassigned client: {_comment}");

                Assert.True(actualClientByIdAirsofOfficeResponse.user.sales_agent.Equals(_userIdAirsoftOffice),
                    $" actual sales agent for Airsof Office client: {actualClientByIdAirsofOfficeResponse.user.sales_agent}, " +
                    $" expected sales agent for Airsof Office client: {_userIdAirsoftOffice}");

                Assert.True(actualClientByIdAirsofOfficeResponse.user.sales_status.Equals("Double Phone Number"),
                    $" actual sales status for Airsof Office client: {actualClientByIdAirsofOfficeResponse.user.sales_status}, " +
                    $" expected sales status for Airsof Office client: Double Phone Number");

                Assert.True(actualCommentByClientIdAirsofOfficeResponse == null,
                    $" actual Comment for Airsof Office client: {actualCommentByClientIdAirsofOfficeResponse}, " +
                    $" expected Comment for Airsof Office client: {_comment}");
            });
        }
    }
}