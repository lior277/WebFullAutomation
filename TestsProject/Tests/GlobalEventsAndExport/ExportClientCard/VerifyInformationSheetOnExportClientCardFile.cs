using AirSoftAutomationFramework.Internals.DAL.ConfigurationManagement;
using AirSoftAutomationFramework.Internals.ExtensionsMethods;
using AirSoftAutomationFramework.Internals.Helpers;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.AccountsApi.UsersMenuApi.UserMenuApi;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.Clients.ClientCard;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.Clients.ClientCard.FinancesTab;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.Clients.ClientCard.SATab;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.Clients.ClientPage;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.Settings;
using AirSoftAutomationFramework.Objects.ForexObjects.ApiGeneral;
using AirSoftAutomationFramework.Objects.ForexObjects.ApiGeneral.Login;
using AirSoftAutomationFramework.Objects.ForexObjects.TradingPlatform.TradeObjects.TradeApi;
using NUnit.Framework;
using System;
using System.IO;
using AirSoftAutomationFramework.Internals.Factorys;
using AirSoftAutomationFramework.Internals.NunitAtributes;
using TestsProject.TestsInternals;

namespace TestsProject.Tests.GlobalEventsAndExport.ExportClientCard
{
    [TestFixture]
    public class VerifyInformationSheetOnExportClientCardFile : TestSuitBase
    {

        #region Test Preparation
        private IApplicationFactory _apiFactory = new ApplicationFactory();
        private string _crmUrl = Config.appSettings.CrmUrl;
        private string _currentUserApiKey;
        private string _userId;
        private int _tradeAmount = 3;
        private int _depositAmount = 10000;
        private string _userName;
        private string _clientEmail;
        private string _expectedStatus2 = "2";
        private string _clientName;
        private string _clientId;
        private string _expectedCurrency = DataRep.DefaultUSDCurrencyName;
        private Stream _clientCardStream;
        private int _transferToSavingAccountAmount = 1;
        private string _cryptoGroupName;

        [SetUp]
        public void SetUp()
        {
            BeforeTest();
            _cryptoGroupName = TextManipulation.RandomString();
            var tradingPlatformUrl = Config.appSettings.tradingPlatformUrl;

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

            var informationTabResponse = _apiFactory
             .ChangeContext<IInformationTabApi>()
             .GetInformationTabRequest(_crmUrl, _clientId)
             .GeneralResponse
             .informationTab;

            informationTabResponse.sales_status2 = _expectedStatus2;

            // update sales status 2
            _apiFactory
                .ChangeContext<IInformationTabApi>()
                .PutInformationTabRequest(_crmUrl, informationTabResponse,
                apiKey: _currentUserApiKey);

            // login notification
            var loginData = _apiFactory
                .ChangeContext<ILoginApi>()
                .PostLoginToTradingPlatform(tradingPlatformUrl, _clientEmail)
                .GeneralResponse;

            // create deposit 
            _apiFactory
                .ChangeContext<IFinancesTabApi>()
                .PostDepositRequest(_crmUrl, _clientId, _depositAmount,
                apiKey: _currentUserApiKey);

            // create comment    
            _apiFactory
                .ChangeContext<ICommentsTabApi>()
                .PostCommentRequest(_crmUrl, _clientId,
                apiKey: _currentUserApiKey);

            // create trade notification
            _apiFactory
                .ChangeContext<ITradePageApi>()
                .PostBuyAssetRequest(tradingPlatformUrl, _tradeAmount, loginData);

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
        public void VerifyInformationSheetOnExportClientCardFileTest()
        {
            var expectedDate = DateTime.Now.ToString("yyyy-MM-dd");

            var dataSet = _apiFactory
                .ChangeContext<IFileHandler>()
                .ConvertXlsxStreamToDataSet(_clientCardStream);

            // time line table data
            var informationSheet = _apiFactory
                .ChangeContext<IFileHandler>()
                .ReadDataFromDataSet(dataSet, "Information");

            Assert.Multiple(() =>
            {
                Assert.True(informationSheet["Id"] == _clientId,
                    $" actual  client Id : {informationSheet["Id"]}" +
                    $" expected  client Id: {_clientId}");

                Assert.True(informationSheet["First name"] == _clientName.UpperCaseFirstLetter(),
                    $" actual   _client Name: {informationSheet["First name"]}" +
                    $" expected  _client Name : {_clientName}");

                Assert.True(informationSheet["Last name"] == _clientName.UpperCaseFirstLetter(),
                    $" actual  Last name : {informationSheet["Last name"]}" +
                    $" expected Last name : {_clientName}");

                Assert.True(informationSheet["Email"] == _clientEmail,
                    $" actual  Email : {informationSheet["Email"]}" +
                    $" expected Email : {_clientEmail}");

                Assert.True(informationSheet["Phone"] != null,
                    $" actual Phone  : {informationSheet["Phone"]}" +
                    $" expected Phone : not null");

                Assert.True(informationSheet["Phone 2"] != null,
                    $" actual Phone 2  : {informationSheet["Phone 2"]}" +
                    $" expected Phone 2 : not null");

                Assert.True(informationSheet["Country"] == "Afghanistan",
                    $" actual  Country : {informationSheet["Country"]}" +
                    $" expected Country : Afghanistan");

                Assert.True(informationSheet["Gmt timezone"] != null,
                    $" actual  Gmt timezone : {informationSheet["Gmt timezone"]}" +
                    $" expected Gmt timezone : not null");

                Assert.True(informationSheet["Currency code"] == _expectedCurrency,
                    $" actual  Currency code : {informationSheet["Currency code"]}" +
                    $" expected Currency code : {_expectedCurrency}");

                Assert.True(informationSheet["Note"] == "",
                    $" actual  Note : {informationSheet["Note"]}" +
                    $" expected  Note: ");

                Assert.True(informationSheet["Created at"].Contains(expectedDate),
                    $" actual  Created at : {informationSheet["Created at"]}" +
                    $" expected Created at : {expectedDate}");

                Assert.True(informationSheet["Sales status"] == "Double Phone Number",
                    $" actual Sales status  : {informationSheet["Sales status"]}" +
                    $" expected Sales status : Double Phone Number");

                Assert.True(informationSheet["Bulk trade"] == "1",
                    $" actual Sales status  : {informationSheet["Bulk trade"]}" +
                    $" expected Sales status : 1");

                Assert.True(informationSheet["Sales status2"] == _expectedStatus2,
                    $" actual Sales status 2  : {informationSheet["Sales status2"]}" +
                    $" expected Sales status 2 : {_expectedStatus2}");

                Assert.True(informationSheet["Last login"].Contains(expectedDate),
                    $" actual Last login  : {informationSheet["Last login"]}" +
                    $" expected Last login : {expectedDate}");

                Assert.True(informationSheet["Last logout"].Contains(expectedDate),
                    $" actual Last logout  : {informationSheet["Last logout"]}" +
                    $" expected Last logout : {expectedDate}");

                Assert.True(informationSheet["Sales agent"] == _userName,
                    $" actual Sales agent  : {informationSheet["Sales agent"]}" +
                    $" expected  Sales agent: {_userName}");

                Assert.True(informationSheet["Active"] == "True",
                    $" actual Active  : {informationSheet["Active"]}" +
                    $" expected Active : True");

                Assert.True(informationSheet["Attribution date"] == "N/A",
                    $" actual  Attribution date : {informationSheet["Attribution date"]}" +
                    $" expected Attribution date : N/A");

                Assert.True(informationSheet["Compliance status"] == "Active",
                    $" actual  Compliance status : {informationSheet["Compliance status"]}" +
                    $" expected Compliance status : Active");

                Assert.True(informationSheet["General KYC"] == "Pending",
                    $" actual General KYC  : {informationSheet["General KYC"]}" +
                    $" expected General KYC : Pending");

                Assert.True(informationSheet["KYC POI"] == "Waiting",
                    $" actual  KYC POI : {informationSheet["KYC POI"]}" +
                    $" expected KYC POI : Waiting");

                Assert.True(informationSheet["KYC POR"] == "Waiting",
                    $" actual  KYC POR : {informationSheet["KYC POR"]}" +
                    $" expected KYC POR : Waiting");

                Assert.True(informationSheet["KYC CC FRONT"] == "Waiting",
                    $" actual  KYC CC FRONT : {informationSheet["KYC CC FRONT"]}" +
                    $" expected KYC CC FRONT : Waiting");

                Assert.True(informationSheet["KYC CC BACK"] == "Waiting",
                    $" actual  KYC CC BACK : {informationSheet["KYC CC BACK"]}" +
                    $" expected KYC CC BACK : Waiting");

                Assert.True(informationSheet["DOD"] == "Waiting",
                    $" actual DOD : {informationSheet["DOD"]}" +
                    $" expected DOD : Waiting");

                Assert.True(informationSheet["KYC POR"] == "Waiting",
                    $" actual  KYC POR : {informationSheet["KYC POR"]}" +
                    $" expected KYC POR : Waiting");

                Assert.True(informationSheet["Balance"] != null,
                    $" actual Balance  : {informationSheet["Balance"]}" +
                    $" expected Balance : not null");

                Assert.True(informationSheet["Available"] != null,
                    $" actual  Available : {informationSheet["Available"]}" +
                    $" expected Available : not null");

                Assert.True(informationSheet["Bonus"] == "0",
                    $" actual  Bonus : {informationSheet["Bonus"]}" +
                    $" expected Bonus : 0");

                Assert.True(informationSheet["Equity"] != null,
                    $" actual  Equity : {informationSheet["Equity"]}" +
                    $" expected Equity : not null");

                Assert.True(informationSheet["Open pnl"] != null,
                    $" actual Open pnl  : {informationSheet["Open pnl"]}" +
                    $" expected Open pnl :not null ");

                Assert.True(informationSheet["Open pnl"] != null,
                    $" actual Open pnl  : {informationSheet["Open pnl"]}" +
                    $" expected Open pnl :not null ");

                Assert.True(informationSheet["Min margin"] != null,
                    $" actual Min margin  : {informationSheet["Min margin"]}" +
                    $" expected Min margin :not null ");

                Assert.True(informationSheet["Margin usage"] != null,
                    $" actual Margin usage  : {informationSheet["Margin usage"]}" +
                    $" expected Margin usage :not null ");

                Assert.True(informationSheet["Pnl"] != null,
                    $" actual Pnl  : {informationSheet["Pnl"]}" +
                    $" expected Pnl :not null ");

                Assert.True(informationSheet["Pnl real"] != null,
                    $" actual  Pnl real : {informationSheet["Pnl real"]}" +
                    $" expected Pnl real : not null");

                Assert.True(informationSheet["Pnl bonus"] == "0.00",
                    $" actual  Pnl bonus : {informationSheet["Pnl bonus"]}" +
                    $" expected Pnl bonus : 0");

                Assert.True(informationSheet["Online"] == "False",
                    $" actual Online  : {informationSheet["Online"]}" +
                    $" expected Online : False");

                Assert.True(informationSheet["Saving account"] != null,
                    $" actual  Saving account : {informationSheet["Saving account"]}" +
                    $" expected Saving account : not null");

                Assert.True(informationSheet["Account type"] == "Default",
                    $" actual  Account type : {informationSheet["Saving account"]}" +
                    $" expected Account type : Default");

                Assert.True(informationSheet["Trading group"] == "Default",
                    $" actual  Trading group : {informationSheet["Trading group"]}" +
                    $" expected Trading group : Default");

                Assert.True(informationSheet["Campaign"] == "N/A",
                    $" actual  Campaign : {informationSheet["Campaign"]}" +
                    $" expected Campaign : N/A");

            });
        }
    }
}