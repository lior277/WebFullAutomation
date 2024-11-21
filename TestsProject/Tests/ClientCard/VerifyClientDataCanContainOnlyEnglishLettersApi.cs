// Ignore Spelling: Api

using AirSoftAutomationFramework.Internals.DAL.ConfigurationManagement;
using AirSoftAutomationFramework.Internals.Factorys;
using AirSoftAutomationFramework.Internals.Helpers;
using AirSoftAutomationFramework.Internals.NunitAtributes;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.AccountsApi.UsersMenuApi.UserMenuApi;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.Clients.ClientCard;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.Clients.ClientPage;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.Settings;
using NUnit.Framework;
using TestsProject.TestsInternals;
using static AirSoftAutomationFramework.Objects.DTOs.GetInformationTabResponse;

namespace TestsProject.Tests.ClientCard
{
    [TestFixture]
    public class VerifyClientDataCanContainOnlyEnglishLettersApi : TestSuitBase
    {
        #region Test Preparation

        #region members
        private IApplicationFactory _apiFactory = new ApplicationFactory();
        private string _crmUrl = Config.appSettings.CrmUrl;
        private string _actualIllegalCommentResponse;
        private string _actualIllegalEditClientResponse;
        private string _newClientName;
        private string _currentUserApiKey;
        private InformationTab _informationTabResponse;
        #endregion

        [SetUp]
        public void SetUp()
        {
            BeforeTest();
            #region PreCondition
            var comment = "=comment";

            // create user
            var userName = TextManipulation.RandomString();

            // create user
            var userId = _apiFactory
             .ChangeContext<IUserApi>()
             .PostCreateUserRequest(_crmUrl, userName);

            // create ApiKey
            _currentUserApiKey = _apiFactory
                .ChangeContext<IUserApi>()
                .PostCreateApiKeyRequest(_crmUrl, userId);

            #region Create client
            // create client 
            var clientName = TextManipulation.RandomString();

            var clientId = _apiFactory
                .ChangeContext<ICreateClientApi>()
                .CreateClientRequest(_crmUrl, clientName,
                apiKey: _currentUserApiKey);
            #endregion

            #region Comment
            // create Comment
            _actualIllegalCommentResponse =  _apiFactory
                .ChangeContext<ICommentsTabApi>()
                .PostCommentRequest(_crmUrl, clientId, comment, _currentUserApiKey, false)
                .Message;

            #endregion           
            
            #region Update information tab
            // new email for updating time line 
            _newClientName = $"={clientName}";

            // Create Automation Account type
            _apiFactory
               .ChangeContext<ISalesTabApi>()
               .CreateAutomationAccountTypePipe(_crmUrl);

            _informationTabResponse = _apiFactory
                .ChangeContext<IInformationTabApi>()
                .GetInformationTabRequest(_crmUrl, clientId)
                .GeneralResponse
                .informationTab;

            _informationTabResponse.first_name = _newClientName;
            _informationTabResponse.last_name = _newClientName;
            _informationTabResponse.saving_account_id = "null";
            _informationTabResponse.sales_agent = "null";
            #endregion
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

        // create comment with lIllegal characters
        // edit client with first and last name with lIllegal characters
        // expected proper message
        [Test]
        [Category(DataRep.SanityCategory)]
        [Category(DataRep.RegressionCategory)]
        [RetryMenage(DataRep.NumOfRetriesForFallingTest)]
        public void VerifyClientDataCanContainOnlyEnglishLettersApiTest()
        {
            var expectedIllegalCommentResponse = "Comment can start only with english letters";
            var expectedIllegalFirstNameResponse = "first_name can contain only english letters and numbers. Maximum length is 20 characters";
            var expectedIllegalLastNameResponse = "last_name can contain only english letters and numbers. Maximum length is 30 characters";

            // new first name, new last name, new sale Status, new all kyc, change campaign
            _actualIllegalEditClientResponse = _apiFactory
                .ChangeContext<IInformationTabApi>()
                .PutInformationTabRequest(_crmUrl, _informationTabResponse,
                _currentUserApiKey, false)
                .Message;

            Assert.Multiple(() =>
            {
                Assert.True(_actualIllegalCommentResponse.Contains(expectedIllegalCommentResponse),
                    $" expected lIllegal Comment Response: {expectedIllegalCommentResponse}," +
                    $" actual lIllegal Comment Response: {_actualIllegalCommentResponse}");

                Assert.True(_actualIllegalEditClientResponse.Contains(expectedIllegalFirstNameResponse),
                    $" expected Illegal Edit Client first name Response: {expectedIllegalFirstNameResponse}," +
                    $" actual Illegal Edit Client first name Response: {_actualIllegalEditClientResponse}");

                Assert.True(_actualIllegalEditClientResponse.Contains(expectedIllegalLastNameResponse),
                    $" expected Illegal Edit Client last name Response: {expectedIllegalLastNameResponse}," +
                    $" actual Illegal Edit Client last name Response: {_actualIllegalEditClientResponse}");
            });
        }
    }
}