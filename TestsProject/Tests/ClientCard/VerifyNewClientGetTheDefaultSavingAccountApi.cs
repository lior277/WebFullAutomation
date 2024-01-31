// Ignore Spelling: Api

using System.Linq;
using AirSoftAutomationFramework.Internals.DAL.ConfigurationManagement;
using AirSoftAutomationFramework.Internals.Factorys;
using AirSoftAutomationFramework.Internals.Helpers;
using AirSoftAutomationFramework.Internals.NunitAtributes;
using AirSoftAutomationFramework.Models;
using AirSoftAutomationFramework.Objects.DTOs;
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
    public class VerifyNewClientGetTheDefaultSavingAccountApi : TestSuitBase
    {
        #region Test Preparation

        #region members
        private IApplicationFactory _apiFactory = new ApplicationFactory();
        private string _crmUrl = Config.appSettings.CrmUrl;
        private string _userName;
        private InformationTab _informationTabData;
        private string _firstSavingAccountName;
        private string _expectedFirstSavingAccountId;
        private AccountTypeData _automationAccountType;
        private string _expectedDefaultSavingAccountId;
        private string _userApiKey;
        #endregion

        [SetUp]
        public void SetUp()
        {
            BeforeTest();
            var dbContext = new QaAutomation01Context();

            // create user
            _userName = TextManipulation.RandomString();

            var userId = _apiFactory
                .ChangeContext<IUserApi>()
                .PostCreateUserRequest(_crmUrl, _userName);

            // get ApiKey
            _userApiKey = _apiFactory
                .ChangeContext<IUserApi>()
                .PostCreateApiKeyRequest(_crmUrl, userId);

            // get default saving account name
            _expectedDefaultSavingAccountId = _apiFactory
                .ChangeContext<ISalesTabApi>()
                .GetSavingAccountsRequest(_crmUrl, _userApiKey)
                .SavingAccountData
                .Where(p => p.Default.Equals(true))
                .FirstOrDefault()
                .Id;

            // create first Saving Account
            _firstSavingAccountName = _apiFactory
                .ChangeContext<ISalesTabApi>()
                .PostCreateSavingAccountRequest(_crmUrl, apiKey: _userApiKey);

            // get first saving account id
            _expectedFirstSavingAccountId = _apiFactory
                .ChangeContext<ISalesTabApi>()
                .GetSavingAccountsRequest(_crmUrl, _userApiKey)
                .SavingAccountData
                .Where(p => p.Name == _firstSavingAccountName)
                .FirstOrDefault()
                .Id;

            // create Account type 
            _automationAccountType = _apiFactory
                .ChangeContext<ISalesTabApi>()
                .CreateAutomationAccountTypePipe(_crmUrl);

            _automationAccountType.SavingAccountId = _expectedFirstSavingAccountId;

            // update account Type with saving account
            _apiFactory
                .ChangeContext<ISalesTabApi>()
                .PutAccountTypeRequest(_crmUrl, _automationAccountType);           
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

        [Test]
        [Category(DataRep.SanityCategory)]
        [Category(DataRep.RegressionCategory)]
        [RetryMenage(DataRep.NumOfRetraiesForFallingTest)]
        public void VerifyNewClientGetTheDefaultSavingAccountApiTest()
        {
            // create client 
            var clientName = TextManipulation.RandomString();

            var clientId = _apiFactory
                .ChangeContext<ICreateClientApi>()
                .CreateClientRequest(_crmUrl, clientName, apiKey: _userApiKey);

            _informationTabData = _apiFactory
                .ChangeContext<IInformationTabApi>()
                .GetInformationTabRequest(_crmUrl, clientId)
                .GeneralResponse
                .informationTab;

            var actualDefaultSavingAccountId = _informationTabData.saving_account_id;

            _informationTabData.account_type_id = _automationAccountType.AccountTypeId;

            // update client card with the new acoount type
            _apiFactory
                .ChangeContext<IInformationTabApi>()
                .PutInformationTabRequest(_crmUrl,
                _informationTabData, _userApiKey);

            var actualFirstSavingAccountId = _apiFactory
                 .ChangeContext<IInformationTabApi>()
                 .GetInformationTabRequest(_crmUrl, clientId)
                 .GeneralResponse
                 .informationTab
                 .saving_account_id;

            Assert.Multiple(() =>
            {
                Assert.True(actualDefaultSavingAccountId == _expectedDefaultSavingAccountId,
                    $" expected Default Saving Account Id: {_expectedDefaultSavingAccountId}" +
                    $" actual : different then {actualDefaultSavingAccountId}");

                Assert.True(actualFirstSavingAccountId == _expectedFirstSavingAccountId,
                     $" expected First Saving Account Id: {_expectedFirstSavingAccountId}" +
                     $" actual : First then {actualFirstSavingAccountId}");
            });
        }
    }
}