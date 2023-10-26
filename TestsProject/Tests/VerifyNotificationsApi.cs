// Ignore Spelling: Api

using AirSoftAutomationFramework.Internals.DAL.ConfigurationManagement;
using AirSoftAutomationFramework.Internals.ExtensionsMethods;
using AirSoftAutomationFramework.Internals.Helpers;
using AirSoftAutomationFramework.Internals.NunitAttributes;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using AirSoftAutomationFramework.Objects.DTOs;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.Clients.ClientCard;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.Clients.ClientCard.FinancesTab;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.Clients.ClientPage;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.Settings;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Trade;
using AirSoftAutomationFramework.Objects.ForexObjects.ApiGeneral;
using AirSoftAutomationFramework.Objects.ForexObjects.TradingPlatform.TradeObjects.TradeApi;
using AirSoftAutomationFramework.Objects.ForexObjects.TradingPlatform.TradeObjects.TradeApi.Profile;
using AirSoftAutomationFramework.Objects.ForexObjects.TradingPlatform.TradeObjects.TradeApi.Settings;
using TestsProject.TestsInternals;
using static AirSoftAutomationFramework.Objects.DTOs.TestCase;
using System.Threading;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.AccountsApi.UsersMenuApi.UserMenuApi;
using AirSoftAutomationFramework.Objects.ForexObjects.ApiGeneral.Login;
using ConsoleApp;
using AirSoftAutomationFramework.Internals.Factory;

namespace TestsProject.Tests
{
    [TestFixture]
    public class VerifyNotificationsApi : TestSuitBase
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
            var tradingPlatformUrl
                =  Config.appSettings.tradingPlatformUrl;

            var _tradeAmount = 3;
            var depositAmount = 100000;

            #region delete attribution role 
            var attributionRoles = _apiFactory
                .ChangeContext<IClientsApi>()
                .GetAttributionRolesRequest(_crmUrl)
                .GeneralResponse;

            _apiFactory
               .ChangeContext<IClientsApi>()
               .DeleteAttributionRolesRequest(_crmUrl, attributionRoles);
            #endregion

            // create user
            _userName = TextManipulation.RandomString();
            var country = "french southern and antarctic lands";
            _userEmail = _userName + DataRep.EmailPrefix;

            _userId = _apiFactory
             .ChangeContext<IUserApi>()
             .PostCreateUserRequest(_crmUrl, _userName, country: country,
             role: DataRep.AdminWithUsersOnlyRoleName);

            #region create campaign
            // create campaign
            var campaignData = _apiFactory
                 .ChangeContext<ISharedStepsGenerator>()
                 .CreateAffiliateAndCampaignApi(_crmUrl);
            #endregion

            var campaignId = campaignData.Values.First();
            var campaignName = campaignData.Keys.First();

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
                .CreateClientWithCampaign(_crmUrl, clientName, campaignId: campaignId);
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

            #region connect One User To One Client notification
            // connect One User To One Client notification
            _apiFactory
                .ChangeContext<IClientsApi>()
                .PatchMassAssignSaleAgentsRequest(_crmUrl, _userId,
                new List<string> { clientId });
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
            attributionRoleName = _apiFactory
                .ChangeContext<IClientsApi>()
                .PostCreateAttributionRoleRequest(_crmUrl, _attributionRoleName, actualType: "country",
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
                .PostDepositRequest(_crmUrl, clientId, depositAmount);
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
            var tradeId = tradeDetails.TradeId;

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
                spread = -0.1,
                margin_call = null,
                swap_long = 0.04,
                swap_short = 0.04,
                swap_time = "00:00:00",
            };

            var currentRate = tradeDetails.TradeRate;
            var takeProfitRate = currentRate + +100; // to open a take Profit trade

            _apiFactory // create trade with take profit
               .ChangeContext<ITradePageApi>()
               .CreateTakeProfitApi(tradingPlatformUrl,
               _tradeAmount, loginData, takeProfitRate);

            // Create Cripto Group And Assign It To client
            var groupData = _apiFactory
                .ChangeContext<ISharedStepsGenerator>()
                .CreateTradeGroupAndAssignItToClientPipe(_crmUrl,
                tradeGroupAttributesForTakeProfit, clientId);

            // wait for the stop loss close trade 
            _apiFactory
                .ChangeContext<ITradePageApi>()
                .WaitForCfdTradeToClose(tradingPlatformUrl, tradeId, loginData);

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
                .PatchSetTradingGroupRequest(_crmUrl, bronzeGroupId, new List<string> { clientId });

            currentRate = tradeDetails.TradeRate;
            var stopLossRate = (double)(currentRate - 0.0001); // to open a stop loss trade

            // create trade with stop loss
            tradeDetails = _apiFactory
                .ChangeContext<ITradePageApi>()
                .CreateStopLossApi(tradingPlatformUrl, _tradeAmount, loginData, stopLossRate);

            tradeId = tradeDetails.TradeId;

            // Create trade Group And Assign It To client
            groupData = _apiFactory 
                .ChangeContext<ISharedStepsGenerator>()
                .CreateTradeGroupAndAssignItToClientPipe(_crmUrl, tradeGroupAttributesForStopLoss, clientId);

            // wait for the stop loss close trade 
            _apiFactory
                .ChangeContext<ITradePageApi>()
                .WaitForCfdTradeToClose(tradingPlatformUrl, tradeId, loginData);

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
              .CreateTradeGroupAndAssignItToClientPipe(_crmUrl,
              tradeGroupSreadTenAttributes, clientIdForMagin);

            _tradeGroupsIdsListForDelete.Add(groupData.Keys.First());
            #endregion

            #region create pending withdrawal notification
            // create pending withdrawal notification
            var withdrawalAmount = 30;

            _apiFactory
                .ChangeContext<IWithdrawalTpApi>()
                .PostPendingWithdrawalRequest(tradingPlatformUrl, loginData, withdrawalAmount);
            #endregion

            #region proceed withdrawal notification
            // get withdrawal id
            var withdrawalId =
                (from s in _dbContext.FundsTransactions
                 where (s.UserId == clientId
                 && Math.Abs(s.Amount) == withdrawalAmount
                 && s.Type == "withdrawal" && s.Status == "pending")
                 select s.Id)
                 .FirstOrDefault()
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
                 })
                 .FirstOrDefault()
                 .balance;

            // Calculate Trade Amount For Margin Call
            tradeAmountForMarginCall = _apiFactory
                .ChangeContext<ITradePageApi>()
                .CalculateTradeAmountForMarginCall(openPrice,
                tradeAmountForMarginCall,
                maintenance, (double)userBalance, marginCall);

            // create trade for margin call
            _apiFactory
                .ChangeContext<ITradePageApi>()
                .PostBuyAssetRequest(tradingPlatformUrl, 
                tradeAmountForMarginCall, loginData);

            // Create Cripto Group And Assign It To client
            groupData = _apiFactory
                .ChangeContext<ISharedStepsGenerator>()
                .CreateTradeGroupAndAssignItToClientPipe(_crmUrl,
                tradeGroupForMarginCallAttributes, clientId);

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
        [Category(DataRep.ApiDocCategory)] // apidoc add comment
        [Category(DataRep.SanityCategory)]
        [Category(DataRep.RegressionCategory)]
        [RetryMenage(DataRep.NumOfRetraiesForFallingTest)]
        public void VerifyNotificationsApiTest()
        {
            var expectedNotificationsTyps = new List<string>() {
                "appointment", "campaign_or_api_register", "close_trade","deposit",
                "deposit","deposit","deposit_page","direct_register",
                "file_upload","login", "login","logout","margin_call",
                "new_lead_assignment","new_lead_assignment","new_lead_assignment",
                "new_lead_assignment", "open_trade","open_trade","open_trade",
                "open_trade","open_trade","open_trade","open_trade",
                "reach_margin_limit","reach_margin_limit","redirect_to_psp",
                "reset_password","sl_close_trade","tp_close_trade",
                "trade_order_activated","trade_order_activated","trade_order_activated",
                "trade_order_activated","trade_order_activated","trade_order_activated",
                "trade_order_activated","withdrawal_processed","withdrawal_request",
                "client_unassignement" };

            var actualNotificationsTyps = new List<string>();

            // waiting notification to contains appointment
            for (var i = 0; i < 100; i++)
            {
                actualNotificationsTyps = _apiFactory
                    .ChangeContext<INotificationsApi>()
                    .GetNotificationRequest(_crmUrl, _currentUserApiKey,
                    expectedNotificationsTyps.Count());

                if (!actualNotificationsTyps.Contains("appointment"))
                {
                    actualNotificationsTyps = _apiFactory
                    .ChangeContext<INotificationsApi>()
                    .GetNotificationRequest(_crmUrl, _currentUserApiKey,
                    expectedNotificationsTyps.Count());
                    Thread.Sleep(500);
                }
                else
                {
                    break;
                }
            }

            actualNotificationsTyps
              .RemoveAll(p => p.Equals("reach_margin_limit"));

            actualNotificationsTyps.Add("reach_margin_limit");
            actualNotificationsTyps.Add("reach_margin_limit");

            var actualAgainstExpected = actualNotificationsTyps
                .CompareTwoListOfString(expectedNotificationsTyps);

             Assert.True(actualAgainstExpected.Count == 0,
                $" actual not contains: {actualAgainstExpected.ListToString()}" +
                $" actual : {actualNotificationsTyps.ListToString()} " +
                $" client Email: {_clientEmail}, user Email: {_userEmail}");
        }
    }
}