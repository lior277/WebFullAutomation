// Ignore Spelling: Api

using System;
using System.Collections.Generic;
using System.Linq;
using AirSoftAutomationFramework.Internals.DAL.ConfigurationManagement;
using AirSoftAutomationFramework.Internals.ExtensionsMethods;
using AirSoftAutomationFramework.Internals.Factorys;
using AirSoftAutomationFramework.Internals.Helpers;
using AirSoftAutomationFramework.Internals.NunitAtributes;
using AirSoftAutomationFramework.Models;
using AirSoftAutomationFramework.Objects.DTOs;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.AccountsApi.Roles;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.AccountsApi.UsersMenuApi.UserMenuApi;
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
using NUnit.Framework;
using TestsProject.TestsInternals;
using static AirSoftAutomationFramework.Objects.DTOs.TestCase;

namespace TestsProject.Tests.RolePage.Notifications
{
    [TestFixture]
    public class VerifyNotificationsPermissionsApi : TestSuitBase
    {
        #region Test Preparation

        private IApplicationFactory _apiFactory = new ApplicationFactory();
        private string _crmUrl = Config.appSettings.CrmUrl;
        private string _userName;
        private string _userEmail;
        private string _userId;
        private string _clientEmail;
        private int _depositAmountForMargin = 10000;
        private string _currentUserApiKey;
        private string _attributionRoleName;
        private List<string> _tradeGroupsIdsListForDelete = new List<string>();
        private QaAutomation01Context _dbContext = new QaAutomation01Context();

        [SetUp]
        public void SetUp()
        {
            BeforeTest();
            _attributionRoleName = TextManipulation.RandomString();
            #region PreCondition
            var  tradingPlatformUrl =  Config.appSettings.tradingPlatformUrl;
            var _tradeAmount = 3;
            var depositAmount = 10000;
            GetRoleByNameResponse _roleData;
            var roleName = TextManipulation.RandomString();

            // get role by name
            _roleData = _apiFactory
                .ChangeContext<IRolesApi>()
                .GetRoleByNameRequest(_crmUrl, DataRep.AdminRole);

            _roleData.Name = roleName;
            _roleData.UsersOnly = true;

            #region permission to remove         
            _roleData.Notifications.Remove("login");
            _roleData.Notifications.Remove("clients_import");
            _roleData.Notifications.Remove("sl_close_trade");
            _roleData.Notifications.Remove("reach_margin_limit");
            _roleData.Notifications.Remove("deposit");
            _roleData.Notifications.Remove("logout");
            _roleData.Notifications.Remove("deposit_page");
            _roleData.Notifications.Remove("open_trade");
            _roleData.Notifications.Remove("trade_order_activated");
            _roleData.Notifications.Remove("appointment");
            _roleData.Notifications.Remove("direct_register");
            _roleData.Notifications.Remove("withdrawal_request");
            _roleData.Notifications.Remove("close_trade");
            _roleData.Notifications.Remove("file_upload");
            _roleData.Notifications.Remove("campaign_or_api_register");
            _roleData.Notifications.Remove("withdrawal_processed");
            _roleData.Notifications.Remove("tp_close_trade");
            _roleData.Notifications.Remove("outgoing_call");
            _roleData.Notifications.Remove("new_lead_assignment");
            _roleData.Notifications.Remove("client_unassignement");
            _roleData.Notifications.Remove("reset_password");
            _roleData.Notifications.Remove("redirect_to_psp");
            _roleData.Notifications.Remove("margin_call");
            #endregion

            // create  role 
            _apiFactory
                .ChangeContext<IRolesApi>()
                .PostCreateRoleRequest(_crmUrl, _roleData);

            // create user
            _userName = TextManipulation.RandomString();
            _userEmail = _userName + DataRep.EmailPrefix;

            // create user
            _userId = _apiFactory
             .ChangeContext<IUserApi>()
             .PostCreateUserRequest(_crmUrl, _userName, role: roleName); 

            // create campaign
            var campaignData = _apiFactory
                 .ChangeContext<ISharedStepsGenerator>()
                 .CreateAffiliateAndCampaignApi(_crmUrl, apiKey: _currentUserApiKey);

            var  campaignId = campaignData.Values.First();
            var  campaignName = campaignData.Keys.First();

            #region create attribution role for campaign
            // create attribution role
            var attributionRoleName = _apiFactory
                .ChangeContext<IClientsApi>()
                .PostCreateAttributionRoleRequest(_crmUrl, _attributionRoleName,
                new string[] { campaignId }, ftdAgentIds: new string[] { _userId });
            #endregion

            #region create ApiKey
            // create ApiKey
            _currentUserApiKey = _apiFactory
                .ChangeContext<IUserApi>()
                .PostCreateApiKeyRequest(_crmUrl, _userId);
            #endregion

            #region create client campaign and api register notification
            // create client
            var clientName = TextManipulation.RandomString();
            _clientEmail = clientName + DataRep.EmailPrefix;

            var clientId = _apiFactory
                .ChangeContext<ICreateClientApi>()
                .CreateClientWithCampaign(_crmUrl, clientName,
                campaignId: campaignId, apiKey: _currentUserApiKey);
            #endregion

            #region delete attribution role 
            var attributionRoles = _apiFactory
                .ChangeContext<IClientsApi>()
                .GetAttributionRolesRequest(_crmUrl)
                .GeneralResponse;

            _apiFactory
               .ChangeContext<IClientsApi>()
               .DeleteAttributionRolesRequest(_crmUrl, attributionRoles);
            #endregion

            #region connect One User To One Client notification
            // connect One User To One Client notification
            _apiFactory
                .ChangeContext<IClientsApi>()
                .PatchMassAssignSaleAgentsRequest(_crmUrl, _userId,
                new List<string> { clientId }, apiKey: _currentUserApiKey);
            #endregion

            #region login data and notification
            // login notification
            var loginData = _apiFactory
                .ChangeContext<ILoginApi>()
                .PostLoginToTradingPlatform(tradingPlatformUrl, _clientEmail)
                .GeneralResponse;
            #endregion

            #region forget password notification
            // forget password notification
            _apiFactory
             .ChangeContext<IClientCardApi>()
             .PostForgotPasswordRequest(_crmUrl, _clientEmail, loginData);
            #endregion

            #region create attribution role for country
            // create attribution role
            var country = "french southern and antarctic lands";

            attributionRoleName = _apiFactory
                .ChangeContext<IClientsApi>()
                .PostCreateAttributionRoleRequest(_crmUrl, _attributionRoleName, actualType : "country",
                countryNames: new string[] { country }, ftdAgentIds: new string[] { _userId });
            #endregion            

            #region direct register notification
            // direct register notification
            _apiFactory
                .ChangeContext<ICreateClientApi>()
                .RegisterClientWithPromoCode(_crmUrl, country: country);
            #endregion

            #region delete attribution role 
            attributionRoles = _apiFactory
                .ChangeContext<IClientsApi>()
                .GetAttributionRolesRequest(_crmUrl)
                .GeneralResponse;

            _apiFactory
               .ChangeContext<IClientsApi>()
               .DeleteAttributionRolesRequest(_crmUrl, attributionRoles);
            #endregion

            #region deposit notification
            // deposit notification
            _apiFactory
                .ChangeContext<IFinancesTabApi>()
                .PostDepositRequest(_crmUrl, clientId, depositAmount, apiKey: _currentUserApiKey);
            #endregion

            #region create trade notification
            // create trade notification
            var tradeDetails = _apiFactory
                .ChangeContext<ITradePageApi>()
                .PostBuyAssetRequest(tradingPlatformUrl, _tradeAmount, loginData)
                .GeneralResponse;
            #endregion

            #region close trade notification
            // close trade notification
            var  tradeId = tradeDetails.TradeId;

            _apiFactory
              .ChangeContext<IOpenTradesPageApi>()
              .PatchCloseTradeRequest(_crmUrl, tradeId);
            #endregion

            #region take profit notification 
            var tradeGroupAttributesForTakeProfit = new Default_Attr
            {
                commision = 0,
                leverage = 5,
                maintenance = 1,
                minimum_amount = 1,
                minimum_step = 1,
                spread = -1,
                margin_call = null,
                swap_long = 0.04,
                swap_short = 0.04,
                swap_time = "00:00:00",
            };

            var currentRate = tradeDetails.TradeRate;
            var takeProfitRate = (double)(currentRate + 0.001); // to open a take Profit trade

            _apiFactory // create trade with take profit
               .ChangeContext<ITradePageApi>()
               .CreateTakeProfitApi(tradingPlatformUrl, _tradeAmount, loginData, takeProfitRate);

            // Create Cripto Group And Assign It To client
            var groupData = _apiFactory 
                .ChangeContext<ISharedStepsGenerator>()
                .CreateTradeGroupAndAssignItToClientPipe(_crmUrl, tradeGroupAttributesForTakeProfit, clientId);

            var groupId = groupData.Keys.First();
            _tradeGroupsIdsListForDelete.Add(groupId);
            #endregion

            #region stop loss notification
            var tradeGroupAttributesForStopLoss = new Default_Attr
            {
                commision = 0,
                leverage = 5,
                maintenance = 1,
                minimum_amount = 1,
                minimum_step = 1,
                spread = 0.1,
                margin_call = null,
                swap_long = 0.04,
                swap_short = 0.04,
                swap_time = "00:00:00",
            };

            var bronzeGroupId = "5c1b4817df7f1a324f6ade52";

            _apiFactory
                .ChangeContext<IInformationTabApi>()
                .PatchSetTradingGroupRequest(_crmUrl, bronzeGroupId,
                new List<string> { clientId });

            currentRate = tradeDetails.TradeRate;
            var stopLossRate = (double)(currentRate - 0.0001); // to open a take Profit trade

            _apiFactory // create trade with stop loss
               .ChangeContext<ITradePageApi>()
               .CreateStopLossApi(tradingPlatformUrl, _tradeAmount, loginData, stopLossRate);

            groupData = _apiFactory // Create Cripto Group And Assign It To client
              .ChangeContext<ISharedStepsGenerator>()
              .CreateTradeGroupAndAssignItToClientPipe(_crmUrl, tradeGroupAttributesForStopLoss, clientId);

            groupId = groupData.Keys.First();
            _tradeGroupsIdsListForDelete.Add(groupId);
            #endregion

            #region trade margin close
            var tradeGroupSreadZeroAttributes = new Default_Attr
            {
                commision = 0,
                leverage = 1000,
                maintenance = 0.1,
                minimum_amount = 1,
                minimum_step = 1,
                spread = 0,
                margin_call = null,
                swap_long = 0.04,
                swap_short = 0.04,
                swap_time = "00:00:00",
            };

            var tradeGroupSreadTenAttributes = new Default_Attr
            {
                commision = 0,
                leverage = 1,
                maintenance = 0.1,
                minimum_amount = 1,
                minimum_step = 1,
                spread = 0.2,
                margin_call = null,
                swap_long = 0.04,
                swap_short = 0.04,
                swap_time = "00:00:00",
            };

            // create client 
            clientName = TextManipulation.RandomString();
            var clientEmailForMagin = clientName + DataRep.TestimEmailPrefix;

            var clientIdForMagin = _apiFactory
                .ChangeContext<ICreateClientApi>()
                .CreateClientRequest(_crmUrl, clientName,
                emailPrefix: DataRep.TestimEmailPrefix,
                apiKey: _currentUserApiKey);

            // create deposit
            _apiFactory
                .ChangeContext<IFinancesTabApi>()
                .PostDepositRequest(_crmUrl, clientIdForMagin, _depositAmountForMargin);

            // login data
            var loginDataForMagin = _apiFactory
                .ChangeContext<ILoginApi>()
                .PostLoginToTradingPlatform(tradingPlatformUrl, clientEmailForMagin)
                .GeneralResponse;

            // create trade to retrieve the current rate
            var tradeDetailsForMagin = _apiFactory
                 .ChangeContext<ITradePageApi>()
                 .PostBuyAssetRequest(tradingPlatformUrl, _tradeAmount, loginDataForMagin)
                 .GeneralResponse;

            currentRate = tradeDetails.TradeRate;

            // Create Cripto Group And Assign It To client
            groupData = _apiFactory
                .ChangeContext<ISharedStepsGenerator>()
                .CreateTradeGroupAndAssignItToClientPipe(_crmUrl, tradeGroupSreadZeroAttributes, clientIdForMagin);

            _tradeGroupsIdsListForDelete.Add(groupData.Keys.First());

            // _balance = 100 * leverage = 1000 = 100000 - 1000
            var tradeAmountForMagin = Convert.ToInt32(90000 / currentRate);

            // create trade for client with trade group "tradeGroupSreadZeroAttributes"
            _apiFactory
                .ChangeContext<ITradePageApi>()
                .PostBuyAssetRequest(tradingPlatformUrl, tradeAmountForMagin, loginDataForMagin);

            // change the trade group in order to automaticly close the trade
            groupData = _apiFactory
              .ChangeContext<ISharedStepsGenerator>()
              .CreateTradeGroupAndAssignItToClientPipe(_crmUrl, tradeGroupSreadTenAttributes, clientIdForMagin);

            _tradeGroupsIdsListForDelete.Add(groupData.Keys.First());
            #endregion

            #region create pending withdrawal notification
            // create pending withdrawal notification
            var  withdrawalAmount = 30;

            _apiFactory
                .ChangeContext<IWithdrawalTpApi>()
                .PostPendingWithdrawalRequest(tradingPlatformUrl, loginData, withdrawalAmount);
            #endregion

            #region proceed withdrawal notification
            // get withdrawal id
            var  withdrawalId =
                (from s in _dbContext.FundsTransactions
                 where (s.UserId == clientId
                 && Math.Abs(s.Amount) == withdrawalAmount 
                 && s.Type == "withdrawal" && s.Status == "pending")
                 select s.Id).First()
                 .ToString();

            // proceed withdrawal notification
            _apiFactory
               .ChangeContext<IFinancesTabApi>()
               .PatchWithdrawalStatusRequest(_crmUrl,
               clientId, withdrawalId, apiKey: _currentUserApiKey);
            #endregion

            #region upload file notification 
            //upload file notification
            _apiFactory
             .ChangeContext<IProfilePageApi>()
             .PatchKycFileRequest(tradingPlatformUrl,
             DataRep.FileNameToUpload, "kyc_proof_of_identity", loginData);
            #endregion

            #region Planning notification
            // planning notification with comment    
            _apiFactory
               .ChangeContext<IPlanningTabApi>()
               .PostCreateAddCommentRequest(_crmUrl, _userId, _currentUserApiKey);
            #endregion

            #region deposit page notification and redirect to psp notification 
            // create psp for deposit page
            _apiFactory
              .ChangeContext<IPspTabApi>()
              .PostCreateAirsoftSandboxPspRequest(_crmUrl);

            // create deposit
            _apiFactory
               .ChangeContext<IFinancesTabApi>()
               .PostDepositRequest(_crmUrl, clientId, 200000);

            // trade deposit page notification
            _apiFactory
              .ChangeContext<IGeneralTabApi>()
              .PutMaximumDepositRequest(_crmUrl)
              .ChangeContext<ITradeDepositPageApi>()
              .GetDepositPageRequest(tradingPlatformUrl, loginData);

            _apiFactory
              .ChangeContext<ITradeDepositPageApi>()
              .PostCreatePaymentRequestPipe(tradingPlatformUrl, loginData);
            #endregion

            #region logout notification
            // logout notification
            _apiFactory
              .ChangeContext<ICreateClientApi>()
              .GetLogoutTreadingPlatformRequest(tradingPlatformUrl, loginData);
            #endregion

            #region create margin call
            var tradeAmountForMarginCall = 2;
            var maintenance = 10;
            var marginCall = 1;

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
            tradeDetails = _apiFactory
                 .ChangeContext<ITradePageApi>()
                 .PostBuyAssetRequest(tradingPlatformUrl, _tradeAmount, loginData)
                 .GeneralResponse;

            var openPrice = tradeDetails.TradeRate;

            var userBalance =
                (from s in _dbContext.UserAccounts
                 where s.UserId == clientId
                 select new ExpectedFinanceData
                 {
                     balance = (int)s.Balance
                     .MathRoundFromGeneric(0, MidpointRounding.ToPositiveInfinity),
                 }).FirstOrDefault()
                 .balance;

            // Calculate Trade Amount For Margin Call
            tradeAmountForMarginCall = _apiFactory
                .ChangeContext<ITradePageApi>()
                .CalculateTradeAmountForMarginCall(openPrice, tradeAmountForMarginCall,
                maintenance, (double)userBalance, marginCall);

            // create trade for margin call
            _apiFactory
                .ChangeContext<ITradePageApi>()
                .PostBuyAssetRequest(tradingPlatformUrl, tradeAmountForMarginCall, loginData);

            // Create Cripto Group And Assign It To client
            groupData = _apiFactory
                .ChangeContext<ISharedStepsGenerator>()
                .CreateTradeGroupAndAssignItToClientPipe(_crmUrl, tradeGroupForMarginCallAttributes, clientId);

            _tradeGroupsIdsListForDelete.Add(groupData.Keys.First());

            // Start Margin Call
            _apiFactory
                .ChangeContext<ITradePageApi>()
                .GetStartMarginCallRequest(_crmUrl);
            #endregion
            #endregion
        }
        #endregion

        [TearDown]
        public void TearDown()
        {
            try
            {
                _apiFactory
                    .ChangeContext<ITradeGroupApi>()
                    .DeleteTradeGroupRequest(_crmUrl, _tradeGroupsIdsListForDelete);

                // delete attribution role
                var AttributionRoleByName = _apiFactory
                   .ChangeContext<IClientsApi>()
                   .GetAttributionRolesRequest(_crmUrl)
                   .GeneralResponse
                   .Where(p => p.name == _attributionRoleName)
                   .ToList();

                _apiFactory
                   .ChangeContext<IClientsApi>()
                   .DeleteAttributionRolesRequest(_crmUrl, AttributionRoleByName);
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

        [Test]
        [Category(DataRep.SanityCategory)]
        [Category(DataRep.RegressionCategory)]
        [RetryMenage(DataRep.NumOfRetraiesForFallingTest)]
        public void VerifyNotificationsPermissionsApiTest()
        {          
            var actualNotificationsTyps = _apiFactory
                .ChangeContext<INotificationsApi>()
                .GetNotificationRequest(_crmUrl, _currentUserApiKey, 0);

            // expected 0 notification
            Assert.True(actualNotificationsTyps.Count == 0,
                $" actual num of contains: {actualNotificationsTyps.Count}" +
                $" expected num of contains : 0 " +
                $" client Email: {_clientEmail}, user Emai: {_userEmail}");
        }
    }
}