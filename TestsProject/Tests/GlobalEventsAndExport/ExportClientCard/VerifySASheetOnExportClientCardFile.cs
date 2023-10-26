using System;
using System.IO;
using AirSoftAutomationFramework.Internals.DAL.ConfigurationManagement;
using AirSoftAutomationFramework.Internals.Factory;
using AirSoftAutomationFramework.Internals.Helpers;
using AirSoftAutomationFramework.Internals.NunitAttributes;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.AccountsApi.UsersMenuApi.UserMenuApi;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.Clients.ClientCard;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.Clients.ClientCard.FinancesTab;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.Clients.ClientCard.SATab;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.Clients.ClientPage;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.Settings;
using AirSoftAutomationFramework.Objects.ForexObjects.ApiGeneral;
using NUnit.Framework;
using TestsProject.TestsInternals;

namespace TestsProject.Tests.GlobalEventsAndExport.ExportClientCard
{
    [TestFixture]
    public class VerifySASheetOnExportClientCardFile : TestSuitBase
    {

        #region Test Preparation
        private IApplicationFactory _apiFactory = new ApplicationFactory();
        private string _crmUrl = Config.appSettings.CrmUrl;
        private string _currentUserApiKey;
        private string _userId;
        private int _depositAmount = 10000;
        private string _userName;
        private string _clientEmail;
        private string _clientName;
        private string _clientId;
        private Stream _clientCardStream;
        private int _transferToSavingAccountAmount = 1;

        [SetUp]
        public void SetUp()
        {
            BeforeTest();

            // create user
            _userName = TextManipulation.RandomString();
            var testimEmailPerfix = DataRep.TestimEmailPrefix;
            var userEmail = _userName + testimEmailPerfix;

            // create user
            _userId = _apiFactory
             .ChangeContext<IUserApi>()
             .PostCreateUserRequest(_crmUrl, _userName,
             emailPrefix: testimEmailPerfix,
              role: DataRep.AdminWithUsersOnlyRoleName);

            #region create ApiKey
            // create ApiKey
            _currentUserApiKey = _apiFactory
                .ChangeContext<IUserApi>()
                .PostCreateApiKeyRequest(_crmUrl, _userId);
            #endregion

            // create client 
            _clientName = TextManipulation.RandomString();
            _clientEmail = _clientName + DataRep.EmailPrefix;

            _clientId = _apiFactory
                .ChangeContext<ICreateClientApi>()
                .CreateClientRequest(_crmUrl, _clientName,
                apiKey: _currentUserApiKey);

            // create deposit 
            _apiFactory
                .ChangeContext<IFinancesTabApi>()
                .PostDepositRequest(_crmUrl, _clientId, _depositAmount,
                apiKey: _currentUserApiKey);

            // Transfer from balance to Saving Account
            _apiFactory
                .ChangeContext<ISATabApi>()
                .PostTransferToSavingAccountRequest(_crmUrl,
                _clientId, _transferToSavingAccountAmount,
                apiKey: _currentUserApiKey);

            // enter the Email For Export in Super Admin Tub
            var brandRegulation = _apiFactory
               .ChangeContext<ISuperAdminTubApi>()
               .GetBrandRegulationRequest(_crmUrl);

            DataRep.EmailListForExport.Add(userEmail);

            brandRegulation.export_data_email_url = DataRep
                .EmailListForExport.ToArray();

            _apiFactory
                .ChangeContext<ISuperAdminTubApi>()
                .PutRegulationsRequest(_crmUrl, brandRegulation);

            #region  export Client Card
            // export comments tab
            _apiFactory
                .ChangeContext<IClientCardApi>()
                .ExportClientCardPipe(_crmUrl, _clientId,
                userEmail, _currentUserApiKey);

            // extract the export link from mail body
            var exportLink = _apiFactory
               .ChangeContext<ISharedStepsGenerator>()
               .GetExportLinkFromExportEmailBody(_crmUrl, userEmail, "export_link");

            // get the csv file 
            _clientCardStream = _apiFactory
                .ChangeContext<IClientCardApi>()
                .GetExportClientCardRequest(_crmUrl, exportLink, _currentUserApiKey)
                .Stream;
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
        public void VerifySASheetOnExportClientCardFileTest()
        {
            var expectedDate = DateTime.Now.ToString("dd/MM/yyyy");

            var dataSet = _apiFactory
                .ChangeContext<IFileHandler>()
                .ConvertXlsxStreamToDataSet(_clientCardStream);

            // SA table data
            var savingAccountSheet = _apiFactory
                .ChangeContext<IFileHandler>()
                .ReadDataFromDataSet(dataSet, "SA");

            Assert.Multiple(() =>
            {
                Assert.True(savingAccountSheet["Id"] != null,
                    $" actual  Id : {savingAccountSheet["Id"]}" +
                    $" expected  id not null");

                Assert.True(savingAccountSheet["Date| GMT"].Contains(expectedDate),
                    $" actual Date | GMT : {savingAccountSheet["Date| GMT"]}" +
                    $" expected Date | GMT: {expectedDate}");

                Assert.True(savingAccountSheet["SA Balance"] == string.Empty,
                    $" actual SA Balance : {savingAccountSheet["SA Balance"]}" +
                    $" expected SA Balance: empty");

                Assert.True(savingAccountSheet["SA Name"] != null,
                    $" actual SA Name : {savingAccountSheet["SA Name"]}" +
                    $" expected SA Name not null");

                Assert.True(savingAccountSheet["Type"] == "deposit",
                    $" actual Type : {savingAccountSheet["Type"]}" +
                    $" expected Type: deposit");

                Assert.True(savingAccountSheet["SA %"] == "1",
                    $" actual SA % : {savingAccountSheet["SA %"]}" +
                    $" expected SA %: 1");

                Assert.True(savingAccountSheet["Transaction amount"] == $"{_transferToSavingAccountAmount}$",
                    $" actual Transaction amount : {savingAccountSheet["Transaction amount"]}" +
                    $" expected Transaction amount: {_transferToSavingAccountAmount}");         
            });
        }
    }
}