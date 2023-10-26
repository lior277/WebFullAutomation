// Ignore Spelling: Api Timeline Psp Kyc Tp Dods Erp Ips Ip Chrono Recaptcha Cfd Usdt Crm

using System;
using System.Collections.Generic;
using System.Linq;

namespace AirSoftAutomationFramework.Internals.DAL.ApiAccess.ApiRoutes
{
    public static class ApiRouteAggregate
    {
        public static string PostCreateMgmLogin()
        {
            return "/api/mgm/auth/login";
        }

        public static string GetTimelineDetails()
        {
            return "/api/users/trading-platform/timeline/";
        }

        public static string PostCreateCampaign()
        {
            return "/api/users/campaigns";
        }

        public static string PostRunInactiveFeesJob()
        {
            return "/api/finance/funds-transactions/start-inactive-fees-job/";
        }

        public static string InactiveFees()
        {
            return "/api/settings/inactive-fees";
        }

        public static string GetCampaigns()
        {
            return "/api/users/campaigns/performance";
        }

        public static string GetPspInstances()
        {
            return "/api/finance/psp/instances";
        }

        public static string GetBulkTrades()
        {
            return "/api/finance/mass-trades?draw=1";
        }

        public static string PostCreateBulkTrades()
        {
            return "/api/finance/trades/mass-trade";
        }

        public static string PostCreateComment()
        {
            return "/api/users/comments";
        }

        public static string PostCreateApiKey()
        {
            return "/api/users/erp/generate-api-key";
        }

        public static string PatchAddFingerPrint()
        {
            return "/api/users/auth/allow-fingerprint";
        }

        public static string PostCreateAffiliate()
        {
            return "/api/users/erp/affiliate";
        }

        public static string PatchMassAssignSalesAgent()
        {
            return "/api/users/trading-platform/mass-assign-sales-agent";
        }

        public static string PatchAssignSaleAgent()
        {
            return "/api/users/trading-platform/assign-to-sales-agent";
        }

        public static string PatchMassAssignSalesStatus()
        {
            return "/api/users/trading-platform/mass-assign-sales-status";
        }

        public static string GetRoleByName()
        {
            return "/api/users/roles/byname/";
        }

        public static string PostCreateUser()
        {
            return "/api/users/erp";
        }

        public static string GetUserLastLoginsTimeline(string userId)
        {
            return $"/api/users/erp/last-logins-timeline/{userId}";
        }

        public static string PostCreateRole()
        {
            return "/api/users/roles";
        }

        public static string PatchConnectCampaignToClient()
        {
            return "/api/users/trading-platform/mass-assign-campaign";
        }

        public static string PostResetPassword()
        {
            return "/api/users/trading-platform/reset-password";
        }

        public static string PostCreateLoginClientShop()
        {
            return "/auth/login";
        }

        public static string PostBuyAsset()
        {
            return "/api/finance/trades/cfd/buy";
        }

        public static string GetLogout()
        {
            return "/api/users/auth/logout";
        }

        public static string GetClientById()
        {
            return "/api/users/trading-platform/user/";
        }

        public static string GetClient(string searchValue)
        {
            return $"/api/users/trading-platform/customers?start=0&search[value]={searchValue}";
        }

        public static string GetClientCard(string clientId)
        {
            return $"/api/users/trading-platform/user/{clientId}";
        }

        public static string GetClientByFilter(string filterName, string filterValue)
        {
            return $"/api/users/trading-platform/customers?start=0&filter[{filterName}]={filterValue}";
        }

        public static string GetClientByFilter()
        {
            return $"/api/users/trading-platform/customers?start=0";
        }

        public static string GetRisksByFilter()
        {
            return $"/api/finance/user-account/risk?order[0][dir]=asc";
        }

        public static string GetDepositByFilter()
        {
            return "/api/finance/funds-transactions/by-type/deposit?order[0]";
        }

        public static string DeleteClient()
        {
            return "/api/users/trading-platform/";
        }

        public static string ExportUser(string clientId)
        {
            return $"/api/users/trading-platform/export-user/{clientId}";
        }

        public static string MassDeleteClients()
        {
            return "/api/users/trading-platform/mass-delete-leads";
        }

        public static string DeleteMassAssignClient()
        {
            return "/api/users/trading-platform/mass-delete-leads";
        }

        public static string GetDownloadKycFile()
        {
            return "/api/users/trading-platform/private-file/";
        }

        public static string GetGlobalEventsExportData()
        {
            return "/api/users/erp/log-export/global_events";
        }

        public static string PostTransferToSA()
        {
            return "/api/finance/users-saving-accounts/deposit";
        }

        public static string PatchEditTrade()
        {
            return "/api/finance/trades/commissions-and-status/";
        }

        public static string PatchEditSwap()
        {
            return "/api/finance/trades/edit-swap/";
        }

        public static string GetSaBalanceByClientId()
        {
            return "/api/finance/users-saving-accounts/sa-balance/";
        }

        public static string GetUsersSavingAccounts(string today)
        {
            return $"/api/finance/users-saving-accounts/me?date={today}";
        }

        public static string GetSaByClientId()
        {
            return "/api/finance/users-saving-accounts/";
        }

        public static string GetAvailableWithdrawalByClientId()
        {
            return "/api/finance/user-account/available-withdrawal/";
        }

        public static string PostTransferToBalance()
        {
            return "/api/finance/users-saving-accounts/withdrawal";
        }

        public static string PostCreateDeposit()
        {
            return "/api/finance/funds-transactions/deposit";
        }

        public static string PostCreateDepositWithWooCommerceUser()
        {
            return "/api/users/trading-platform/deposit-by-email";
        }

        public static string GetDepositsByCampaignId(string campaignId)
        {
            return $"/api/users/campaigns/transactions/deposit/{campaignId}";
        }

        public static string GetCampaignsByDialer()
        {
            return "/api/users/campaigns/dialer-campaigns-basic-info";
        }

        public static string PostCreateMassAssignComment()
        {
            return "/api/users/comments/mass";
        }

        public static string PatchMassAssignRandomSaleAgents()
        {
            return "/api/users/trading-platform/mass-assign-random-sales-agent";
        }

        public static string GetCampaignsByAffiliate()
        {
            return "/api/users/campaigns/basic-info";
        }

        public static string PatchWithdrawalStatus()
        {
            return "/api/finance/funds-transactions/withdrawal-status";
        }

        public static string PatchAssignWithdrawal(string withdrawalId, string title)
        {
            return $"/api/finance/funds-transactions/assign-title-to-withdrawal/{withdrawalId}/{title}";
        }

        public static string GetFinanceData(string clientId)
        {
            return $"/api/finance/funds-transactions/by-user/{clientId}";
        }

        public static string GetClientFile(string clientId, string fileName)
        {
            return $"/api/users/trading-platform/private-file/{clientId}/{fileName}?download=true";
        }

        public static string PostCreateBonus()
        {
            return "/api/finance/funds-transactions/deposit/bonus";
        }
        public static string DeleteResetAccount()
        {
            return "/api/finance/funds-transactions/reset-account/";
        }

        public static string PostWithdrawalBonus()
        {
            return "/api/finance/funds-transactions/withdrawal-bonus";
        }

        public static string PostSplitPendingWithdrawal(string withdrawalId)
        {
            return $"/api/finance/funds-transactions/split/{withdrawalId}";
        }

        public static string PostWithdrawalDeposit()
        {
            return "/api/finance/funds-transactions/withdrawal";
        }

        public static string DeleteFinanceItem(string financeItemId)
        {
            return $"/api/finance/funds-transactions/{financeItemId}";
        }

        public static string PatchAssignFinanceToUser(string FinanceId, string userId)
        {
            return $"/api/finance/funds-transactions/assign/{FinanceId}/{userId}";
        }

        public static string PostCreatePendingWithdrawal()
        {
            return "/api/finance/funds-transactions/withdrawal";
        }

        public static string PostCancelPendingWithdrawal()
        {
            return "/api/finance/funds-transactions/me/cancel-withdrawal/";
        }

        public static string GetTransactionsFromTp()
        {
            return "/api/finance/funds-transactions/me";
        }

        public static string PostCreateTax()
        {
            return "/api/settings/taxes";
        }

        public static string PutTax(string taxId)
        {
            return $"/api/settings/taxes/type/{taxId}";
        }

        public static string PutSuspiciousPnl()
        {
            return "/api/settings/config/suspicious_profit";
        }

        public static string PostCreateTaxInstance()
        {
            return "/api/settings/taxes/instance";
        }

        public static string PostSellAsset()
        {
            return "/api/finance/trades/cfd/sell";
        }

        public static string RetrieveClosedTradesByAgent()
        {
            return "/api/finance/trades/erp/cfd";
        }

        public static string Convert()
        {
            return "/api/finance/funds-transactions/convert";
        }

        public static string GetDepositPage()
        {
            return "/api/finance/psp/deposit-page";
        }

        public static string PatchKycFile()
        {
            return "/api/users/trading-platform/me/kyc-file";
        }

        public static string GetCompanyDodFile()
        {
            return "/api/settings/config/company-dod-file?dod=true&download=true";
        }

        public static string PostImportLead()
        {
            return "/api/users/trading-platform/import";
        }


        public static string PostCreatePayment()
        {
            return "/api/finance/funds-transactions/create-payment";
        }

        public static string PostPaymentNotification()
        {
            return "/api/finance/psp/notify/";
        }

        public static string PostSendCustomPspBankDetails()
        {
            return "/api/finance/psp/send-custom-psp-details/bank";
        }

        public static string PostSendCustomPspWalletDetails()
        {
            return "/api/finance/psp/send-custom-psp-details/wallet";
        }

        public static string PutMaximumDeposit()
        {
            return "/api/settings/config";
        }

        public static string PutEmailByIdTemplate(string emailId)
        {
            return $"/api/settings/system-emails/{emailId}";
        }

        public static string GetAutoEmails()
        {
            return "/api/settings/system-emails/auto";
        }

        public static string SaveEmail()
        {
            return "/api/settings/system-emails";
        }

        public static string GetDocuments()
        {
            return "/api/settings/system-documents/all-by-type/custom";
        }

        public static string GetChatMessages()
        {
            return "/api/settings/chat-messages";
        }

        public static string GetCustomEmails()
        {
            return "/api/settings/system-emails/custom";
        }

        public static string GetDods()
        {
            return "/api/settings/system-documents/all-by-type/dod";
        }

        public static string GetTermsAndConditions()
        {
            return "/api/settings/system-documents/all-by-type/terms_and_conditions";
        }


        public static string PostDocument()
        {
            return "/api/settings/system-documents";
        }

        public static string PostCreateDialer()
        {
            return "/api/settings/dialers";
        }

        public static string PostCreateTrunk()
        {
            return "/api/settings/trunks";
        }

        public static string DeleteDocument(string documentId)
        {
            return $"/api/settings/system-documents/{documentId}";
        }

        public static string PatchDocumentStatus(string documentId)
        {
            return $"/api/settings/system-documents/active/{documentId}";
        }

        public static string PatchEmailStatus(string documentId)
        {
            return $"/api/settings/system-emails/active/{documentId}";
        }

        public static string PatchDodStatus(string documentId, string isActive)
        {
            return $"/api/settings/system-documents/enable-digital-signature/{documentId}?is_active={isActive}";
        }

        public static string PutTermAndCondition()
        {
            return "/api/settings/config/terms-and-conditions";
        }

        public static string PutSettingsBySection(string sectionName)
        {
            return $"/api/settings/config/{sectionName}";
        }

        public static string AvailableWithdrawalRequest()
        {
            return "/api/finance/user-account/me/available-withdrawal";
        }

        public static string GetGroupRestrictions()
        {
            return "/api/settings/config/group_restrictions";
        }

        public static string PutRiskRestrictions()
        {
            return "/api/settings/config/risk-restrictions";
        }

        public static string PutGroupNames()
        {
            return "/api/settings/config/group-names";
        }

        public static string GroupRestrictions()
        {
            return "/api/settings/config/group-restrictions";
        }

        public static string GetGroupNames()
        {
            return "/api/settings/config/group_names";
        }

        public static string PutMarginCall()
        {
            return "/api/settings/config/margin_call";
        }

        public static string PutLanguageByClientId(string clientId)
        {
            return $"/api/users/trading-platform/set-language/{clientId}";
        }

        public static string PutLanguageById(string langId)
        {
            return $"/api/settings/brand-languages/{langId}";
        }

        public static string PostCreateLanguage()
        {
            return "/api/settings/brand-languages";
        }

        public static string GetLanguageById(string langId)
        {
            return $"/api/settings/brand-languages/{langId}";
        }

        public static string GetErpLanguageByLangCode(string langCode)
        {
            return $"/api/settings/brand-languages/erp/{langCode}";
        }

        public static string GetDefaultTradingPlatformTranslation()
        {
            return "/assets/languages/default.json";
        }

        public static string GetTradingPlatformLanguageData(string platform)
        {
            return $"/api/settings/brand-languages/by-platform/{platform}";
        }

        public static string GetBaseCurrencies()
        {
            return "/api/settings/config/base-currencies";
        }

        public static string GetCreateDepositCurrencies()
        {
            return "/api/users/erp/filter-fields?currencies=true";
        }

        public static string GetSalesStatusTextFilter()
        {
            return $"/api/users/erp/filter-fields/?sales_status_text=true";
        }

        public static string GetCurrencies()
        {
            return "/api/settings/config/currencies";
        }

        public static string GetLoginAttempts()
        {
            return "/api/settings/config/login";
        }

        public static string GetBlockUsers()
        {
            return "/api/users/erp/blocked-users";
        }

        public static string GetBlockIps()
        {
            return "/api/users/blocked-ips";
        }

        public static string ReleaseBlockUser()
        {
            return "/api/users/erp/release/";
        }

        public static string ReleaseIpBlockUser()
        {
            return "/api/users/blocked-ips/";
        }

        public static string GetClientCurrencies()
        {
            return "/api/users/erp/create-form-fields";
        }

        public static string PutSalesStatus()
        {
            return "/api/settings/config/sales_status_text";
        }

        public static string PutSalesStatus2()
        {
            return "/api/settings/config/sales_status_text2";
        }

        public static string PatchCloseTrade(string tradeId)
        {
            return $"/api/finance/trades/close/cfd/{tradeId}";
        }

        public static string PatchEditBulkTrade(string bulkTradeId)
        {
            return $"/api/finance/trades/mass-trade-edit/{bulkTradeId}";
        }

        public static string GetMassTradeById(string MassTradeId)
        {
            return $"/api/finance/mass-trade-reports/{MassTradeId}";
        }

        public static string PatchCloseBulkTrade(string bulkTradeId)
        {
            return $"/api/finance/trades/mass-trade-close/{bulkTradeId}";
        }

        public static string PatchCloseChronoTrade(string tradeId)
        {
            return $"/api/finance/trades/chrono/close/cfd/{tradeId}";
        }

        public static string GetSalesPerformance()
        {
            return "/api/finance/funds-transactions/performance-by-goals";
        }

        public static string PatchMassCloseTrade()
        {
            return "/api/finance/trades/mass-close/cfd";
        }

        public static string PatchEditTradeById(int tradeId)
        {
            return $"/api/finance/trades/commissions-and-status/{tradeId}";
        }

        public static string PostSendDirectEmail()
        {
            return "/api/users/trading-platform/send-email";
        }

        public static string GetTrades(string clientId)
        {
            return $"/api/finance/trades/user/cfd/{clientId}";
        }

        public static string DeleteTrade(string tradeId)
        {
            return $"/api/finance/trades/cfd/{tradeId}";
        }

        public static string PatchReOpenTrade()
        {
            return "/api/finance/trades/reopen-trades-by-ids/cfd";
        }

        public static string DeleteTaxInstance(string taxId, string taxInstanceId)
        {
            return $"/api/settings/taxes/instance/{taxId}/{taxInstanceId}";
        }

        public static string GetTaxInstance()
        {
            return "/api/settings/taxes/instance";
        }

        public static string PatchPendingTrade(string tradeId)
        {
            return $"/api/finance/trades/pending/{tradeId}";
        }

        public static string PostForgotPasswordTp()
        {
            return "/api/users/trading-platform/forgot-password";
        }

        public static string PostForgotPasswordErp()
        {
            return "/api/users/erp/forgot-password";
        }

        public static string PostMakeCall()
        {
            return "/api/users/erp/make-call/";
        }

        public static string GetBanners()
        {
            return "/api/settings/system-feeds";
        }

        public static string PutBanners()
        {
            return "/api/users/trading-platform/user-feed-message";
        }

        public static string GetClientBanner()
        {
            return "/api/users/trading-platform/user-feed-message-id/";
        }

        public static string PatchResetDevBrand()
        {
            return "https://kube-dev01-deploy.airsoftltd.com/mysql/reset-database#";
        }

        public static string PutBannerInSettings(string bannerId)
        {
            return $"/api/settings/system-banners/{bannerId}";
        }

        public static string PostCreateBanners()
        {
            return "/api/settings/system-banners";
        }

        public static string PostRegisterClient()
        {
            return "/api/users/trading-platform/register";
        }

        public static string GetBoxesStatistics()
        {
            return "/api/finance/funds-transactions/boxes-statistics";
        }

        public static string GetUserTimeLine()
        {
            return "/api/users/trading-platform/users-timeline";
        }

        public static string GetNotification()
        {
            return "/api/notification/notification";
        }

        public static string PostCreateClient()
        {
            return "/api/users/trading-platform/create-client";
        }

        public static string PostCreateLoginLink()
        {
            return "/api/users/erp/login-link";
        }

        public static string PostCreteAttributionRole()
        {
            return "/api/users/registration-rules";
        }

        public static string PatchAssignTradingGroup()
        {
            return "/api/users/trading-platform/assign-to-trading-group";
        }

        public static string PutAttributionRole(string attributionRoleId)
        {
            return $"/api/users/registration-rules/{attributionRoleId}";
        }

        public static string PostPixel(string referenceId)
        {
            return $"/pixel/params?referance_id={referenceId}";
        }

        public static string SendPixel()
        {
            return "/pixel/send-params";
        }

        public static string GetPixel()
        {
            return "/pixel/get-params";
        }

        public static string GetAllLeads(string clientId)
        {
            return $"/api/users/trading-platform/all-leads?clients_ids={clientId}";
        }

        public static string GetClients()
        {
            return "/api/users/erp/columns-visibility/clients";
        }

        public static string GetCustomers()
        {
            return "/api/users/trading-platform/customers";
        }

        public static string PostCreateFilter(string componentName)
        {
            return $"/api/users/erp/user-filter/{componentName}";
        }

        public static string DeleteFilterFilter(string componentName)
        {
            return $"/api/users/erp/delete-user-filter/{componentName}";
        }

        public static string PatchMassAssignComplianceStatus()
        {
            return "/api/users/trading-platform/mass-assign-compliance-status";
        }

        public static string ImportClients()
        {
            return "/api/users/trading-platform/import";
        }

        public static string PostExportTableWithLink()
        {
            return "/api/users/erp/send-export-link-to-admin-email";
        }

        public static string PostExportClientCard(string clientId)
        {
            return $"/api/users/erp/send-single-export-link-to-admin-email/{clientId}";
        }

        public static string GetExportCampaignTable()
        {
            return "/api/users/erp/log-export/campaigns";
        }

        public static string GetCommentsByUserId(string userId)
        {
            return $"/api/users/comments/by-user/{userId}";
        }

        public static string PostComment()
        {
            return "/api/users/comments";
        }

        public static string GetActiveCampaigns()
        {
            return "/api/users/campaigns/performance";
        }

        public static string GetTransactions(string transactionsType)
        {
            return $"/api/finance/funds-transactions/by-type/{transactionsType}";
        }

        public static string GetLastRegistration()
        {
            return "/api/users/trading-platform/last-registration";
        }

        public static string DeleteComment()
        {
            return "/api/users/comments/";
        }

        public static string GetOrdersByClientIdId(string clientId)
        {
            return $"/api/core/store/orders/user/{clientId}";
        }

        public static string MassPatchSetTradingGroup()
        {
            return "/api/users/trading-platform/mass-assign-trade-group";
        }

        public static string PatchLastDepositDate()
        {
            return "/api/users/trading-platform/last-deposit-date/";
        }

        public static string PostLogin()
        {
            return "/api/users/auth/login";
        }

        public static string PatchRefreshClient(string clientId)
        {
            return $"/api/finance/user-account/refresh-user-state/{clientId}";
        }

        public static string PatchReSetClientPassword(string clientId)
        {
            return $"/api/users/trading-platform/set-user-password/{clientId}";
        }

        public static string PatchSetClientCurrency(string clientId)
        {
            return $"/api/users/trading-platform/currency/{clientId}";
        }

        public static string PatchClientStatus(string clientId)
        {
            return $"/api/users/trading-platform/active/{clientId}";
        }

        public static string GetFastLogin()
        {
            return "/api/auth/trading-platform/fast-login";
        }

        public static string ClientProfileOnTradingPlatform()
        {
            return "/api/users/trading-platform/me";
        }

        public static string GetChronoTab()
        {
            return "/api/settings/config/global-settings?chrono=true";
        }

        public static string GetPspList()
        {
            return "/api/finance/psp/list";
        }

        public static string PostCreateSavingAccount()
        {
            return "/api/settings/saving-account";
        }

        public static string PostSalasAgentSalary()
        {
            return "/api/settings/salary/salary";
        }

        public static string PostCreateAccountTypes()
        {
            return "/api/settings/account-types";
        }

        public static string PostCreateDepositRange()
        {
            return "/api/settings/deposit-range";
        }

        public static string PutAccountTypes(string accountTypeId)
        {
            return $"/api/settings/account-types/{accountTypeId}";
        }

        public static string PutRecaptcha()
        {
            return "/api/settings/config/recaptcha";
        }

        public static string GetSalasAgentSalary()
        {
            return "/api/settings/salary";
        }

        public static string DeleteSalasAgentSalary(string salaryId)
        {
            return $"/api/settings/salary/{salaryId}";
        }

        public static string PutEditSavingAccount(string savingAccountId)
        {
            return $"/api/settings/saving-account/{savingAccountId}";
        }

        public static string GetSavingAccount()
        {
            return "/api/settings/saving-account";
        }

        public static string GetDataFromBankingByTypeAndFilter(string byType, string filterName, string filterValue)
        {
            return $"/api/finance/funds-transactions/by-type/{byType}?order[0][column]=" +
                $"user_id&order[0][dir]=desc&start=0&length=10000&filter[{filterName}][]={filterValue}";
        }

        public static string GetDataFromBankingByType(string byType)
        {
            var startDate = DateTime.Now.AddDays(-30).ToString("yyyy-MM-dd");
            var endDate = DateTime.Now.AddDays(1).ToString("yyyy-MM-dd");

            return $"/api/finance/funds-transactions/by-type/{byType}?order[0][column]=order_id&order[0]" +
                $"[dir]=desc&length=25&filter[start_date]=" +
                $"{startDate} 00:00:00&filter[end_date]={endDate} 23:59:59";
        }

        public static string GetAgentProfileInfo(string userId)
        {
            var startDate = DateTime.Now.AddDays(-30).ToString("yyyy-MM-dd");
            var endDate = DateTime.Now.AddDays(1).ToString("yyyy-MM-dd");

            return $"/api/users/erp/get-agent-profile-info?agent_id={userId}" +
                $"&start_date={startDate} 00:00:00&end_date={endDate} 23:59:59&filter=month";
        }

        public static string PostExportBankingTables(string byType)
        {
            return $"/api/finance/funds-transactions/by-type/{byType}";
        }

        public static string PostExportTradesTables(string byType)
        {
            return $"/api/finance/trades/erp/cfd/{byType}";
        }

        public static string PostExportCrmTradeTables(string byType)
        {
            return $"/api/finance/user-account/{byType}";
        }

        public static string GetSalesStatus()
        {
            return "/api/settings/config/sales_status_text";
        }

        public static string GetSalesStatusFromApiDoc()
        {
            return "/api/settings/config/sales-status";
        }

        public static string GetSalesStatus2()
        {
            return "/api/settings/config/sales_status_text2";
        }

        public static string GetMinimumDeposit()
        {
            return "/api/settings/config/minimum_deposit";
        }

        public static string GetMaximumDeposit()
        {
            return "/api/settings/config/maximum_deposit";
        }

        public static string GetSetting()
        {
            return "/api/settings/";
        }

        public static string GetMailByEmailAddress()
        {
            return "/mails/get";
        }

        public static string GetRoles()
        {
            return "/api/users/roles?noAffiliate=true";
        }

        public static string GetAffiliateRoles()
        {
            return "/api/users/roles";
        }

        public static string DeleteBanner(string bannerId)
        {
            return $"/api/settings/system-banners/{bannerId}";
        }

        public static string GetCountries()
        {
            return "/api/settings/config/countries-iso2";
        }

        public static string GetAllSalesAgentsByAffiliate()
        {
            return "/api/users/erp/all-sales-agents";
        }

        public static string PatchHideUser()
        {
            return "/api/users/erp/hide-erp-user/";
        }

        public static string MassDeleteUserUser()
        {
            return "/api/users/erp/mass-hide";
        }

        public static string GetExportUsersTable()
        {
            return "/api/users/erp/log-export/usersTable";
        }

        public static string GetAllSalesAgentsByDialer()
        {
            return "/api/users/erp/dialer-all-sales-agents";
        }

        public static string PatchBoostOptionsInChronoTab()
        {
            return "/api/settings/chrono";
        }

        public static string PostCreateOffice()
        {
            return "/api/settings/offices";
        }

        public static string GetOffices()
        {
            return "/api/settings/offices";
        }

        public static string GetTrunks()
        {
            return "/api/settings/trunks";
        }

        public static string GetLeader(string officeId)
        {
            return $"/api/settings/offices/leaderboard-access/?officeId={officeId}";
        }

        public static string GetClientRegistration()
        {
            return "/api/users/campaigns/clients-registrations";
        }

        public static string PutClientCard(string clientId)
        {
            return $"/api/users/trading-platform/{clientId}";
        }

        public static string DeleteKycFile(string clientId, string KycFileId)
        {
            return $"/api/users/trading-platform/private-file/{clientId}/{KycFileId}";
        }

        public static string PostAffiliateRole()
        {
            return "/api/users/roles/affiliate";
        }

        public static string DeleteRole()
        {
            return "/api/users/roles/";
        }

        public static string DeleteCampaign()
        {
            return "/api/users/campaigns";
        }

        public static string UpdateRole()
        {
            return "/api/users/roles/";
        }

        public static string GetTradesByStatus(string tradeStatus = "open")
        {
            return $"/api/finance/trades/me/cfd/{tradeStatus}";
        }

        public static string GetChronoTrades()
        {
            return "/api/finance/trades/me/chrono/";
        }

        public static string GetAssetByName(string assetName)
        {
            return $"/api/feed/assets/cfd?symbols[]={assetName}";
        }

        public static string GetStartMarginCall()
        {
            return "/api/finance/user-account/start-margin-call";
        }

        public static string GetTradesCount()
        {
            return "/api/finance/trades/me/trades-count/cfd";
        }

        public static string SetLanguage(string clientId)
        {
            return $"/api/users/trading-platform/set-language/{clientId}";
        }

        public static string PutSetTemplate(string clientId)
        {
            return $"/api/users/trading-platform/set-template/{clientId}";
        }

        public static string GetActivities(string date)
        {
            return $"/api/users/trading-platform/me/timeline?date={date}";
        }

        public static string GetFeedAssetsCfdMajor()
        {
            return "/api/feed/assets/cfd/Forex/Major";
        }

        public static string GetGlobalEvents(string date)
        {
            return $"/api/settings/global-events?timeObj[start_date]={date} 00:00:00&timeObj[end_date]={date} 23:59:59";
        }

        public static string GetActivitiesByName(string date, string clientId, bool fullHistory)
        {
            return $"/api/users/trading-platform/export-users-activities?" +
                $"type=all_activities&date={date}&user_id={clientId}&full_history={fullHistory}";
        }

        public static string GetAssetDetailsByName(string assetName)
        {
            return $"/api/feed/assets/cfd?symbols[]={assetName.ToUpper()}";
        }

        public static string GetCfdVsUsdt(string name = "Crypto vs USDT")
        {
            return $"/api/feed/assets/cfd/{name}";
        }

        public static string GetClosedTrades()
        {
            return "/api/finance/trades/me/cfd/close";
        }

        public static string GetCfdAssetsNames()
        {
            return "/api/feed/assets/names/cfd";
        }

        public static string GetAppleAsset()
        {
            return "/api/feed/assets/cfd?symbols[]=APPLE";
        }


        public static string GetCfd()
        {
            return "/api/mgm/front-assets/assets-management/cfd";
        }

        public static string GetBrands()
        {
            return "/api/mgm/brands";
        }

        public static string PostUpdateBrands()
        {
            return "/api/mgm/brands/update";
        }

        public static string PostCreateQEndA()
        {
            return "/api/mgm/qna";
        }

        public static string GetAssets()
        {
            return "/api/mgm/assets/assets";
        }

        public static string PatchBrandDeployQEndA()
        {
            return "/api/mgm/qna/brands-deploy";
        }

        public static string PostCreateQNA()
        {
            return "/api/mgm/qna";
        }

        public static string PostCreateBroadcastMessageMgm()
        {
            return "/api/mgm/messages/";
        }

        public static string PostCreateBroadcastMessageCrm()
        {
            return "/api/users/messages/internal";
        }

        public static string PatchAllMessagesStatus()
        {
            return "/api/users/messages/force-close-mgm-messages/";
        }

        public static string GetAssetsOnFront()
        {
            return "/api/feed/assets/assets-management/cfd";
        }

        public static string GetHourlyPnl()
        {
            return "/api/finance/hourly-pnl";
        }

        public static string GetRisks()
        {
            return "/api/finance/user-account/risk?length=25";
        }

        public static string PatchMgmFrontAssetsBrandsDeployCfd()
        {
            return "/api/mgm/front-assets/brands-deploy/cfd";
        }

        public static string PatchAddToCart()
        {
            return "/user/cart";
        }

        public static string GetCheckoutDetails()
        {
            return "/checkout/details";
        }

        public static string GetProducts()
        {
            return "/api/core/store/products?draw=1[search][regex]=false&order[0][column]=_id&order[0]" +
                "[dir]=desc&start=0&length=10000&search[value]=&search[regex]=false&filter[available]=true";
        }

        public static string PatchOrderStatus()
        {
            return "/api/core/store/orders/status/";
        }

        public static string PostCheckout()
        {
            return "/checkout";
        }

        public static string PostCreateCoupons()
        {
            return "/api/core/store/coupons";
        }

        public static string GetCoupons()
        {
            return "/api/core/store/coupons?";
        }


        public static string PutCurrencies()
        {
            return "/api/settings/config/currencies";
        }

        public static string PutPlatform()
        {
            return "/api/settings/config/platform";
        }

        public static string GetGlobalSettings()
        {
            return "/api/settings/config/global-settings?regulation=true&platform=true";
        }

        public static string PutChat()
        {
            return "/api/settings/config/chat";
        }

        public static string PostCreateChat()
        {
            return "/api/settings/chat-messages";
        }

        public static string PatchEnableChatForUser()
        {
            return "/api/users/erp/enable-chat";
        }

        public static string PutBrandRestriction()
        {
            return "/api/settings/config/restrictions";
        }

        public static string PutRegulation()
        {
            return "/api/settings/config/regulation";
        }

        public static string PutConfigChrono()
        {
            return "/api/settings/config/chrono";
        }

        public static string GetProduct()
        {
            return "/api/core/store/products?draw=1&order[0][dir]=desc&search[regex]=false";
        }

        public static string PatchEnableProduct(string id)
        {
            return $"/api/core/store/products/{id}/enable";
        }

        public static string PostCreateBundle()
        {
            return "/api/core/store/bundles";
        }

        public static string PostCreatePsp(string pspId)
        {
            return $"/api/finance/psp/{pspId}";
        }

        public static string CfdGroup()
        {
            return "/api/settings/trade-groups/cfd";
        }

        public static string GetTradeGroupByRevisionId(string tradeGroupId, int revisionId)
        {
            return $"/api/settings/trade-groups/revision-data/cfd/{tradeGroupId}/{revisionId}";
        }

        public static string GetTradeGroupsRevisions(string tradeGroupId)
        {
            return $"/api/settings/trade-groups/revisions/cfd/{tradeGroupId}";
        }

        public static string PutColumnsVisibility(string tableName)
        {
            return $"/api/users/erp/save-columns-visibility/{tableName}";
        }

        public static string GetUsers()
        {
            return $"/api/users/erp?active=&deleted=false";
        }

        public static string GetTaxes()
        {
            return "/api/settings/taxes";
        }
    }
}