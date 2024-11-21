// Ignore Spelling: api crm kyc TimeLine

using AirSoftAutomationFramework.Internals.DAL.ConfigurationManagement;
using AirSoftAutomationFramework.Internals.ExtensionsMethods;
using AirSoftAutomationFramework.Internals.Helpers;
using AirSoftAutomationFramework.Objects.DTOs;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.AccountsApi.UsersMenuApi.UserMenuApi;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.Banking;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.Clients.ClientCard;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.Clients.ClientCard.FinancesTab;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.Clients.ClientPage;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.Settings;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Trade;
using AirSoftAutomationFramework.Objects.ForexObjects.ApiGeneral;
using AirSoftAutomationFramework.Objects.ForexObjects.ApiGeneral.Login;
using AirSoftAutomationFramework.Objects.ForexObjects.TradingPlatform.TradeObjects.TradeApi;
using AirSoftAutomationFramework.Objects.ForexObjects.TradingPlatform.TradeObjects.TradeApi.Profile;
using AirSoftAutomationFramework.Objects.ForexObjects.TradingPlatform.TradeObjects.TradeApi.Settings;
using AirSoftAutomationFramework.Objects.ForexObjects.Ui.Crm.ClientsPage;
using AirSoftAutomationFramework.Objects.ForexObjects.Ui.Crm.ClientsPage.ClientCard;
using AirSoftAutomationFramework.Objects.ForexObjects.Ui.Login;
using AirSoftAutomationFramework.Objects.ForexObjects.Ui.UiGeneral;
using NUnit.Framework;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using AirSoftAutomationFramework.Internals.Factorys;
using AirSoftAutomationFramework.Internals.NunitAtributes;
using AirSoftAutomationFramework.Models;
using TestsProject.TestsInternals;
using static AirSoftAutomationFramework.Objects.DTOs.TestCase;

namespace TestsProject.Tests.ClientCard.TimeLine
{
    [TestFixture(DataRep.Chrome)]
    public class VerifyAccountActivitiesInTimeline : TestSuitBase
    {
        #region Test Preparation
        public VerifyAccountActivitiesInTimeline(string browser) : base(browser)
        {
            _browserName = browser;
        }

        #region members
        private IApplicationFactory _apiFactory = new ApplicationFactory();
        private string _crmUrl = Config.appSettings.CrmUrl;
        private string _userName;
        private string _userNameForAssignDeposit;
        private string _userEmailForAssignDeposit;
        private string _clientId;
        private string _clientName;
        private string _clientEmail;
        private string _userEmail;
        private string _newClientName;
        private string _newClientEmail;
        private string _expectedFirstSavingAccountName;
        private string _expectedDefaultSavingAccountName;
        private string _expectedSecondSavingAccountName;
        private string _tradeAssetName = DataRep.AssetName;
        private string _depositId;
        private string _firstTradeIdForMargin;
        private string _secondTradeIdForMargin;
        private string _saleStatus = "Deposit";
        private string _originalPhone;
        private string _expectedStatus2 = "2";
        private string _originalPhoneTwo;
        private string _newClientPhone;
        private string _newClientPhoneInTimeLine;
        private string _originalClientPhoneInTimeLine;
        private string _userApiKey;
        private int _depositAmount = 10000;
        private IWebDriver _driver;
        private string _browserName;
        #endregion

        [SetUp]
        public void SetUp()
        {
            BeforeTest(_browserName);
            var tradingPlatformUrl = Config.appSettings.tradingPlatformUrl;

            var kycType = "kyc_proof_of_identity";
            var kycName = "Privacy Policy";           
            var tradeAmount = 2;
            var maintenance = 10;
            var marginCall = 1;
            var dbContext = new QaAutomation01Context();
            IDictionary<string, string> _groupData;
            _driver = GetDriver();

            #region create dod bank transfer
            var documentParams = new List<string> { "DEPOSIT_DATE" };

            var dodParams = new Dictionary<string, string> {{ "name", "SignDodOnPlatformBankTransfer" },
                { "language", "es" }, { "sendBy", "trading-platform"},
                { "depositType", "bank_transfer" } };

            // create document body
            var documentBody = _apiFactory
                .ChangeContext<IPlatformTabApi>(_driver)
                .ComposeEmailBody(documentParams);

            // create document
            _apiFactory
                .ChangeContext<IPlatformTabApi>(_driver)
                .CreateDodPipe(_crmUrl, dodParams, documentBody);
            #endregion

            // create user
            _userName = TextManipulation.RandomString();
            _userEmail = _userName + DataRep.EmailPrefix;

            var userId = _apiFactory
                .ChangeContext<IUserApi>(_driver)
                .CreateUserForUiPipe(_crmUrl, _userName);

            // get ApiKey
            _userApiKey = _apiFactory
                .ChangeContext<IUserApi>(_driver)
                .PostCreateApiKeyRequest(_crmUrl, userId);

            // create first Saving Account
            _expectedFirstSavingAccountName = _apiFactory
                .ChangeContext<ISalesTabApi>(_driver)
                .PostCreateSavingAccountRequest(_crmUrl, apiKey: _userApiKey);

            // get default saving account name
            _expectedDefaultSavingAccountName = _apiFactory
                .ChangeContext<ISalesTabApi>(_driver)
                .GetSavingAccountsRequest(_crmUrl, _userApiKey)
                .SavingAccountData
                .Where(p => p.Default.Equals(true))
                .FirstOrDefault()
                .Name;

            // get first SA id
            var firstSavingAccountId = _apiFactory
                .ChangeContext<ISalesTabApi>(_driver)
                .GetSavingAccountsRequest(_crmUrl, _userApiKey)
                .SavingAccountData
                .Where(p => p.Name == _expectedFirstSavingAccountName)
                .FirstOrDefault()
                .Id;

            // create second Saving Account
            _expectedSecondSavingAccountName = _apiFactory
                .ChangeContext<ISalesTabApi>(_driver)
                .PostCreateSavingAccountRequest(_crmUrl, apiKey: _userApiKey);

            // get second SA id
            var secondSavingAccountId = _apiFactory
                .ChangeContext<ISalesTabApi>(_driver)
                .GetSavingAccountsRequest(_crmUrl, _userApiKey)
                .SavingAccountData
                .Where(p => p.Name == _expectedSecondSavingAccountName)
                .FirstOrDefault()
                .Id;

            #region Create client
            // create client
            _clientName = TextManipulation.RandomString();
            _clientEmail = _clientName + DataRep.EmailPrefix;

            _clientId = _apiFactory
                .ChangeContext<ICreateClientApi>(_driver)
                .CreateClientRequest(_crmUrl, _clientName, apiKey: _userApiKey);

            var clientsIds = new List<string> { _clientId };
            #endregion

            // get default login Attempts 
            var loginAtTemps = _apiFactory
                .ChangeContext<ISecurityTubApi>(_driver)
                .GetLoginSectionRequest(_crmUrl)
                .attempts;

            // make client blocked
            for (var i = 0; i < loginAtTemps; i++)
            {
                _apiFactory
                     .ChangeContext<ILoginApi>(_driver)
                     .PostLoginToTradingPlatform(tradingPlatformUrl,
                     _clientEmail, "wrongPassword", false); // wrong password
            }

            // release blocked client
            _apiFactory
                .ChangeContext<ISecurityTubApi>()
                .PatchReleaseBlockUserRequest(_crmUrl, _clientId, _userApiKey);

            // get login Data for trading Platform
            var loginData = _apiFactory
                .ChangeContext<ILoginApi>(_driver)
                .PostLoginToTradingPlatform(tradingPlatformUrl, _clientEmail)
                .GeneralResponse;

            // get TP client profile
            var clientProfileData = _apiFactory
                .ChangeContext<IProfilePageApi>(_driver)
                .GetClientProfileRequest(_crmUrl, loginData);

            clientProfileData.country = "andorra";
            clientProfileData.currency_code = "EUR";

            // update TP client profile
            _apiFactory
                .ChangeContext<IProfilePageApi>(_driver)
                .PatchClientProfileRequest(_crmUrl, clientProfileData, loginData);

            // get TP client profile
            clientProfileData = _apiFactory
                .ChangeContext<IProfilePageApi>(_driver)
                .GetClientProfileRequest(_crmUrl, loginData);

            clientProfileData.currency_code = DataRep.DefaultUSDCurrencyName;

            #region Set client currency
            // Set client currency to eur
            _apiFactory
                .ChangeContext<IClientCardApi>(_driver)
                .PatchSetClientCurrencyRequest(_crmUrl, _clientId, "EUR");

            // Set client currency to usd
            _apiFactory
                .ChangeContext<IClientCardApi>(_driver)
                .PatchSetClientCurrencyRequest(_crmUrl, _clientId,
                DataRep.DefaultUSDCurrencyName, _userApiKey);
            #endregion

            #region Create deposit
            // create deposit
            _apiFactory
                .ChangeContext<IFinancesTabApi>(_driver)
                .PostDepositRequest(_crmUrl, clientsIds,
                _depositAmount, apiKey: _userApiKey);

            // deposit id
            _depositId =
             (from s in dbContext.FundsTransactions
              where (s.UserId == _clientId && s.Amount ==
              _depositAmount && s.Type == "deposit")
              select s.Id)
              .First()
              .ToString();
            #endregion

            #region Create user for Assign Deposit
            // create user for Assign Deposit
            _userNameForAssignDeposit = TextManipulation.RandomString();
            _userEmailForAssignDeposit = _userName + DataRep.EmailPrefix;

            // create user
            var userIdForAssignDeposit = _apiFactory
             .ChangeContext<IUserApi>()
             .PostCreateUserRequest(_crmUrl, _userNameForAssignDeposit);

            // Assign Deposit To new User
            _apiFactory
                .ChangeContext<IDepositsPageApi>(_driver)
                .PatchAssignDepositToUserRequest(_crmUrl,
                _depositId, userIdForAssignDeposit, _userApiKey);
            #endregion

            #region Password
            // set client password
            _apiFactory
                .ChangeContext<IClientCardApi>(_driver)
                .PatchSetClientPasswordRequest(_crmUrl,
                _clientId, _clientName, apiKey: _userApiKey);

            // Reset Password
            _apiFactory
                .ChangeContext<IClientCardApi>(_driver)
                .PostResetPasswordRequest(_crmUrl, _clientId);

            // forgot Password
            _apiFactory
                .ChangeContext<IClientCardApi>(_driver)
                .PostForgotPasswordRequest(_crmUrl, _clientEmail, loginData);
            #endregion           

            #region Connect One User To One Client 
            // connect One User To One Client 
            _apiFactory
                .ChangeContext<IClientsApi>(_driver)
                .PatchMassAssignSaleAgentsRequest(_crmUrl, userIdForAssignDeposit,
                clientsIds, apiKey: _userApiKey);
            #endregion

            #region Comment
            // create Comment
            _apiFactory
                .ChangeContext<ICommentsTabApi>(_driver)
                .PostCommentRequest(_crmUrl, _clientId, apiKey: _userApiKey);

            // get Comment id
            var comment = _apiFactory
                .ChangeContext<ICommentsTabApi>(_driver)
                .GetCommentsByClientIdRequest(_crmUrl, _clientId, apiKey: _userApiKey);

            var commentId = comment.GeneralResponse.FirstOrDefault().Id;
            var commentBody = comment.GeneralResponse.FirstOrDefault().Comment;

            // edit Comment
            _apiFactory
                .ChangeContext<ICommentsTabApi>(_driver)
                .PatchEditCommentRequest(_crmUrl, commentId,
                commentBody, apiKey: _userApiKey);

            // delete Comment
            _apiFactory
                .ChangeContext<ICommentsTabApi>(_driver)
                .DeleteCommentRequest(_crmUrl, commentId, apiKey: _userApiKey);
            #endregion           

            #region Kyc file  
            // upload kyc file from trading 
            _apiFactory
                .ChangeContext<IProfilePageApi>(_driver)
                .PatchKycFileRequest(tradingPlatformUrl,
                DataRep.FileNameToUpload, kycType, loginData);

            // change document status in settings to true
            _apiFactory
                .ChangeContext<IPlatformTabApi>(_driver)
                .ChangeDocumentStatusPipe(_crmUrl, kycName);

            // sign on privacy policy doc in tp
            _apiFactory
                .ChangeContext<ITradePageApi>(_driver)
                .PatchSignKycFileRequest(tradingPlatformUrl, kycName, loginData);

            // get transactions
            var transactions = _apiFactory
                .ChangeContext<IHistoryPageApi>(_driver)
                .GetTransactionsRequest(tradingPlatformUrl, loginData)
                .transactionsList
                .FirstOrDefault();

            var transactionId = transactions.id;
            var transactionDodId = transactions.dod_id;

            // sign on deposit dod in tp
            _apiFactory
                .ChangeContext<IHistoryPageApi>(_driver)
                .PatchSignDodFileRequest(tradingPlatformUrl,
                transactionId, transactionDodId, loginData);

            // download kyc file from client card
            var GetClientByIdResponse = _apiFactory
                .ChangeContext<IClientsApi>(_driver)
                .GetClientByIdRequest(_crmUrl, _clientId)
                .GeneralResponse;

            var _kycUrl = GetClientByIdResponse.user.kyc_proof_of_identity;

            // delete kyc file from client card
            _apiFactory
                .ChangeContext<IInformationTabApi>(_driver)
                .DeleteKycFileRequest(_crmUrl, _clientId, kycType, _userApiKey);
            #endregion

            #region  export clients table
            // enter the Email For Export in Super Admin Tub
            var brandRegulation = _apiFactory
                .ChangeContext<ISuperAdminTubApi>()
                .GetBrandRegulationRequest(_crmUrl);

            DataRep.EmailListForExport.Add(_userEmail);

            brandRegulation.export_data_email_url = DataRep
                .EmailListForExport.ToArray();

            _apiFactory
                .ChangeContext<ISuperAdminTubApi>()
                .PutRegulationsRequest(_crmUrl, brandRegulation);

            // export clients table
            _apiFactory
               .ChangeContext<IClientsApi>(_driver)
               .ExportClientsTablePipe(_crmUrl, _clientEmail, _userEmail, _userApiKey);
            #endregion

            #region Planning timeline
            // planning notification with comment    
            _apiFactory
               .ChangeContext<IPlanningTabApi>(_driver)
               .PostCreateAddCommentRequest(_crmUrl, userId, _userApiKey);
            #endregion

            #region Change banner
            // get banner
            var bannerId = _apiFactory
                .ChangeContext<IPlatformTabApi>(_driver)
                .GetBannersRequest(_crmUrl)
                .Where(p => p.Name == DataRep.AutomationBannerName)
                .FirstOrDefault()?
                .Id;

            if (bannerId != null)
            {
                _apiFactory
                    .ChangeContext<IPlatformTabApi>(_driver)
                    .DeleteBannerRequest(_crmUrl, bannerId);
            }

            // create banner
            _apiFactory
                .ChangeContext<IPlatformTabApi>(_driver)
                .PostCreateBannerRequest(_crmUrl, DataRep.AutomationBannerName, _userApiKey);

            // get banner
            bannerId = _apiFactory
                .ChangeContext<IPlatformTabApi>(_driver)
                .GetBannersRequest(_crmUrl)
                .Where(p => p.Name == DataRep.AutomationBannerName)
                .FirstOrDefault()?
                .Id;

            // change banner
            _apiFactory
                .ChangeContext<IBannerTabApi>(_driver)
                .PutBannerRequest(_crmUrl, _clientId, bannerId, _userApiKey);
            #endregion

            #region Send direct email
            _apiFactory
                .ChangeContext<IClientCardApi>(_driver)
                .PostSendDirectEmailRequest(_crmUrl, _clientId);
            #endregion

            #region Set client status
            // block client
            _apiFactory
                .ChangeContext<IInformationTabApi>(_driver)
                .PatchCilentStatusRequest(_crmUrl, _clientId, false);

            // unblock client
            _apiFactory
                .ChangeContext<IInformationTabApi>(_driver)
                .PatchCilentStatusRequest(_crmUrl, _clientId, true);
            #endregion          

            // get automation account type id
            var accountTypeId = _apiFactory
                .ChangeContext<ISalesTabApi>(_driver)
                .GetAccountTypesRequest(_crmUrl)
                .AccountTypeData
                .Where(p => p.AccountTypeName == "Default")
                .FirstOrDefault()
                .AccountTypeId;

            #region Update information tab
            // new email for updating time line 
            var emailExtension = _clientEmail.Split('@').Last();
            _newClientEmail = $"{_clientEmail.Split('@').First()}automation@{emailExtension}";
            _newClientName = _newClientEmail.Split('@').First();

            var informationTabResponse = _apiFactory
                .ChangeContext<IInformationTabApi>(_driver)
                .GetInformationTabRequest(_crmUrl, _clientId)
                .GeneralResponse
                .informationTab;

            _originalPhone = informationTabResponse.phone;
            _originalPhoneTwo = informationTabResponse.phone_2;

            // the phone number is hidden in time line
#pragma warning disable IDE0057 // Use range operator
            _originalClientPhoneInTimeLine = $"{_originalPhone[..2]}" +
                $"xxxxxxxx{_originalPhone.Substring(_originalPhone.Length - 3)}";
#pragma warning restore IDE0057 // Use range operator

            _newClientPhone = "12345678";

            // the phone number is hidden in time line
#pragma warning disable IDE0057 // Use range operator
            _newClientPhoneInTimeLine = $"{_newClientPhone[..2]}" +
                $"xxxxxxxx{_newClientPhone.Substring(_newClientPhone.Length - 3)}";
#pragma warning restore IDE0057 // Use range operator

            informationTabResponse.sales_status2 = _expectedStatus2;
            informationTabResponse.last_name = _newClientName;
            informationTabResponse.first_name = _newClientName;
            informationTabResponse.email = _newClientEmail;
            informationTabResponse.account_type_id = accountTypeId;
            informationTabResponse.sales_status = _saleStatus;
            informationTabResponse.saving_account_id = firstSavingAccountId;
            informationTabResponse.country = "albania";
            informationTabResponse.phone = _newClientPhone;
            informationTabResponse.phone_2 = _newClientPhone;

            // new first name, new last name, new sale Status, new all kyc, change campaign
            _apiFactory
                .ChangeContext<IInformationTabApi>(_driver)
                .PutInformationTabRequest(_crmUrl, informationTabResponse,
                apiKey: _userApiKey);

            informationTabResponse.saving_account_id = secondSavingAccountId;

            // update Saving Account
            _apiFactory
                .ChangeContext<IInformationTabApi>(_driver)
                .PutInformationTabRequest(_crmUrl, informationTabResponse,
                apiKey: _userApiKey);

            informationTabResponse.saving_account_id = "null";

            // update Saving Account
            _apiFactory
               .ChangeContext<IInformationTabApi>(_driver)
               .PutInformationTabRequest(_crmUrl, informationTabResponse,
                apiKey: _userApiKey);
            #endregion

            #region create margin call
            var tradeGroupForMarginCallAttributes = new Default_Attr
            {
                commision = 0,
                leverage = 1,
                maintenance = maintenance,
                minimum_amount = 1,
                minimum_step = 1,
                spread = 0,
                margin_call = marginCall,
                swap_long = 0.04,
                swap_short = 0.04,
                swap_time = "00:00:00",
            };

            // create trade to retrieve the current rate
            var tradeDetails = _apiFactory
                 .ChangeContext<ITradePageApi>(_driver)
                 .PostBuyAssetRequest(tradingPlatformUrl, tradeAmount, loginData)
                 .GeneralResponse;

            var openPrice = tradeDetails.TradeRate;
            _firstTradeIdForMargin = tradeDetails.TradeId;

            var userBalance =
                (from s in dbContext.UserAccounts
                 where s.UserId == _clientId
                 select new ExpectedFinanceData
                 {
                     balance = (int)s.Balance
                     .MathRoundFromGeneric(0, MidpointRounding.ToPositiveInfinity),
                 }).FirstOrDefault()
                 .balance;

            // Calculate Trade Amount For Margin Call
            tradeAmount = _apiFactory
                .ChangeContext<ITradePageApi>(_driver)
                .CalculateTradeAmountForMarginCall(openPrice, tradeAmount,
                maintenance, (double)userBalance, marginCall);

            // create trade for margin call
            tradeDetails = _apiFactory
                .ChangeContext<ITradePageApi>(_driver)
                .PostBuyAssetRequest(tradingPlatformUrl, tradeAmount, loginData)
                .GeneralResponse;

            _secondTradeIdForMargin = tradeDetails.TradeId;

            // Create trade Group And Assign It To client
            _groupData = _apiFactory
                .ChangeContext<ISharedStepsGenerator>(_driver)
                .CreateTradeGroupAndAssignItToClientPipe(_crmUrl,
                tradeGroupForMarginCallAttributes, _clientId);

            // Start Margin Call
            _apiFactory
                .ChangeContext<ITradePageApi>(_driver)
                .GetStartMarginCallRequest(_crmUrl);

            // delete the trade group for the time-line
            _apiFactory
                .ChangeContext<ITradeGroupApi>()
                .DeleteTradeGroupRequest(_crmUrl, _groupData.Keys.First());

            // delete user for timeLine
            _apiFactory
                .ChangeContext<IUserApi>(_driver)
                .PatchDeleteOrRestoreUserRequest(_crmUrl,
                userIdForAssignDeposit, _userApiKey);
            #endregion

            // login
            _apiFactory
                .ChangeContext<ILoginPageUi>(_driver)
                .LoginPipe(_userName, _crmUrl);
            #endregion

        }

        [TearDown]
        public void TearDown()
        {
            try
            {

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

        [Test]
        [Category(DataRep.SanityCategory)]
        [Category(DataRep.RegressionCategory)]
        [RetryMenage(DataRep.NumOfRetriesForFallingTest)]
        public void VerifyAccountActivitiesInTimelineTest()
        {
            var clientNameWithCapital = _clientName.UpperCaseFirstLetter();

            #region Expected data
            var expectedDate = DateTime.Now.ToString("ddd MMM dd yyy");
            var expectedTimelineData = new List<string>();

            expectedTimelineData.Add("Email margin call trade has been sent to client by System");
            expectedTimelineData.Add($"{clientNameWithCapital} {clientNameWithCapital} Signed Privacy Policy Document");
            expectedTimelineData.Add($"{clientNameWithCapital} {clientNameWithCapital} changed dod status from Waiting to Pending");
            expectedTimelineData.Add($"{clientNameWithCapital} {clientNameWithCapital} Signed SignDodOnPlatformBankTransfer Document type of bank transfer");
            expectedTimelineData.Add($"{_clientName} has been blocked by the system");
            expectedTimelineData.Add($"{_userName} has activated the user");

            expectedTimelineData.Add($"{_newClientName.UpperCaseFirstLetter()} " +
                $"{_newClientName.UpperCaseFirstLetter()} opened trade asset " +
                $"{_tradeAssetName}, trade id: {_secondTradeIdForMargin}");

            expectedTimelineData.Add($"{_newClientName} {_newClientName} " +
                $"has automatically changed to Unassigned Sales agent due to delete entity");

            expectedTimelineData.Add($"{_newClientName} {_newClientName} " +
                $"has automatically Assigned to Default Trading group due to delete entity");

            expectedTimelineData.Add($"{_newClientName.UpperCaseFirstLetter()}" +
                $" {_newClientName.UpperCaseFirstLetter()} opened trade asset" +
                $" {_tradeAssetName}, trade id: {_firstTradeIdForMargin}");

            expectedTimelineData.Add($"{_userName} changed saving account from " +
                $"{_expectedSecondSavingAccountName} to unassigned");

            expectedTimelineData.Add($"{_userName} proceeded with an export of client information");

            expectedTimelineData.Add($"{_userName} changed saving account from" +
                $" {_expectedFirstSavingAccountName} to {_expectedSecondSavingAccountName}");

            expectedTimelineData.Add($"{_userName} changed saving account from" +
                $" {_expectedDefaultSavingAccountName} to {_expectedFirstSavingAccountName}");

            expectedTimelineData.Add($"{_userName} changed sales status2 to 2");

            expectedTimelineData.Add($"{_userName} changed client information:" +
                $" First Name: from {_clientName.ToLower()} to {_newClientName.ToLower()}" +
                $"Last name: from {_clientName.ToLower()} to {_newClientName.ToLower()}" +
                $"Phone: from {_originalClientPhoneInTimeLine} to {_newClientPhoneInTimeLine}" +
                $"Phone #2 : from {_originalClientPhoneInTimeLine} to {_newClientPhoneInTimeLine}" +
                $"Email: from {_clientEmail} to {_newClientEmail}" +
                $"Country: from Andorra to Albania");

            expectedTimelineData.Add($"{_userName} changed client information:" +
               $" Currency: from EUR to {DataRep.DefaultUSDCurrencyName}");

            expectedTimelineData.Add($"{_userName} changed sales status from Double Phone Number to Deposit");
            expectedTimelineData.Add($"{_userName} changed feed message from null to {DataRep.AutomationBannerName}");
            expectedTimelineData.Add($"{_userName} deleted client file");

            expectedTimelineData.Add($"{clientNameWithCapital}" +
                $" {clientNameWithCapital} changed kyc proof of identity status from Waiting to Pending");

            expectedTimelineData.Add($"{_userName} Delete comment, comment id"); // the comment Id is missing
            expectedTimelineData.Add($"{_userName} edited a comment created on the {DateTime.Now:dd/MM/yyyy}");
            expectedTimelineData.Add($"{_userName} Added a comment, comment id");
            expectedTimelineData.Add($"{_userName} changed sales agent from {_userName} to {_userNameForAssignDeposit}");

            expectedTimelineData.Add($"Reset password link has been sent to client by {_clientName}" +
                $" {_clientName}");

            expectedTimelineData.Add("Email forget password has been sent to client by System");
            expectedTimelineData.Add("Email reset password has been sent to client by System");
            expectedTimelineData.Add("Email export table has been sent to client by System");
            expectedTimelineData.Add($"{_userName} Set user password");

            expectedTimelineData.Add($"{_userName} assigned deposit from {_userName.ToLower()}" +
                $" to {_userNameForAssignDeposit.ToLower()} ID: {_depositId}");

            expectedTimelineData.Add("Email first deposit has been sent to client by System");
            expectedTimelineData.Add($"{_userName} Added a Deposit of {_depositAmount}.00 {DataRep.DefaultUSDCurrencyName}");

            expectedTimelineData.Add($"{_clientName.UpperCaseFirstLetter()}" +
                $" {_clientName.UpperCaseFirstLetter()} changed client information: Currency: from {DataRep.DefaultUSDCurrencyName}" +
                $" to EURCountry: from {DataRep.UserDefaultCountry.UpperCaseFirstLetter()} to Andorra");

            expectedTimelineData.Add($"{_clientName.UpperCaseFirstLetter()}" +
                $" {_clientName.UpperCaseFirstLetter()} has logged into the trading-platform and is currently online");

            expectedTimelineData.Add($"{_userName} registered from API");
            #endregion

            _apiFactory
                 .ChangeContext<IMenus>(_driver)
                 .ClickOnMenuItem<IClientsPageUi>()
                 .SearchClientByEmail(_newClientName)
                 .ClickOnClientFullName()
                 .ClickOnExportButton()
                 .SetEmailForExport(_userEmail)
                 .ClickOnSendBtn()
                 .ClickOnOkButton(); 

            var timeLineDetails = _apiFactory
                 .ChangeContext<IClientCardUi>(_driver)
                 .ClickOnTimelineTab()
                 .SetNumOfLines()
                 .GetSearchResultDetails<SearchResultTimeline>(_newClientName,
                 checkNumOfRows: false)
                 .ToList();

            var actualTimeLineActions = new List<string>();

            foreach (var timeLine in timeLineDetails)
            {
                switch (timeLine)
                {
                    case var a when timeLine.action.Contains("edited a comment"):
                        var withoutDate = timeLine.action.Split('-').First().Trim();
                        actualTimeLineActions.Add(withoutDate);

                        continue;

                    case var a when timeLine.action.Contains("Added a commen"):
                        var withoutId = timeLine.action.Split(':').First().Trim();
                        actualTimeLineActions.Add(withoutId);

                        continue;

                    case var a when timeLine.action.Contains("Delete comment"):
                        withoutId = timeLine.action.Split(':').First().Trim();
                        actualTimeLineActions.Add(withoutId);

                        continue;

                    default:
                        actualTimeLineActions.Add(timeLine.action);

                        continue;
                }
            }

            actualTimeLineActions
                .RemoveAll(p => p.Equals("Email admin deposit has been sent to client by System"));

            actualTimeLineActions
                .RemoveAll(p => p.Equals("Email remind about deposit for new clients has been sent to client by System"));

            actualTimeLineActions
                .RemoveAll(p => p.Equals("Email export table has been sent to client by System"));

            actualTimeLineActions.Add("Email export table has been sent to client by System");

            actualTimeLineActions
                .RemoveAll(p => p.Equals("Email margin call trade has been sent to client by System"));

            actualTimeLineActions.Add("Email margin call trade has been sent to client by System");

            var actualDate = timeLineDetails.Select(d => d.date)
                .ToList()
                .All(p => p.Contains(expectedDate));

            var actualAgainstExpected = actualTimeLineActions
                .CompareTwoListOfString(expectedTimelineData);

            Assert.Multiple(() =>
            {
                Assert.True(actualDate,
                    $" expected: {expectedDate}" +
                    $" actual : different then {actualTimeLineActions.ListToString()}");

                Assert.True(actualAgainstExpected.Count == 0,
                   $" Actual  Against expected list: {actualAgainstExpected.ListToString()}" +
                   $" expected Time line Data: {expectedTimelineData.ListToString()}" +
                   $" actual Time line Data: {actualTimeLineActions.ListToString()}");
            });
        }
    }
}