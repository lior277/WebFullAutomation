// Ignore Spelling: Crm Chrono Nft Cfd Ftd Admin Psp Airsoft Chargebacks Pdf Testim Api Ip Ips Retraies Tesim

using Microsoft.EntityFrameworkCore;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;

namespace AirSoftAutomationFramework.Internals.Helpers
{
    public static class DataRep
    {
        // **copy my sql database:
        //Scaffold-DbContext "server=kube-prod1.cchhahczwlx4
        //.eu-west-1.rds.amazonaws.com;database=qa-automation01
        //;user=qa-auto01;Password=F6RqUku6246hAZUW" Pomelo
        //.EntityFrameworkCore.MySql -OutputDir C:\Users\Lior\
        //Source\Repos\airsoftautomationframework\WebFullAutomation\Models -f

        //** after coping the db replace in context
        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    var sqlDbConnectionString = Config.appSettings.SqlConnectionString;
        //    if (!optionsBuilder.IsConfigured)
        //    {
        //        optionsBuilder.UseMySql(sqlDbConnectionString,
        //            new MySqlServerVersion(new Version(8, 0, 11)));
        //    }
        //}

        #region Menus
        public static string TradeMenuItem = "TradeMenuItem";
        public static string CrmOpenTradesMenuItem = "CrmOpenTradesMenuItem";
        public static string TradingOpenTradesMenuItem = "TradingOpenTradesMenuItem";
        public static string TradingCloseTradeMenuItem = "TradingCloseTradeMenuItem";
        public static string CrmCloseTradeMenuItem = "CrmCloseTradeMenuItem";
        public static string MyProfileMenuItem = "MyProfileMenuItem";
        public static string DocumentsMenuItem = "DocumentsMenuItem";
        public static string ChronoOpenTradesMenuItem = "ChronoOpenTradesMenuItem";
        public static string ChronoCloseTradeMenuItem = "ChronoCloseTradesMenuItem";
        public static string TradingPendingTradeMenuItem = "TradingPendingTradeMenuItem";
        public static string CrmPendingTradeMenuItem = "CrmPendingTradeMenuItem";
        public static string FavoritesMenuItem = "FavoritesMenuItem";
        public static string LiveTvMenuItem = "LiveTvMenuItem";
        public static string RiskMenuItem = "RiskMenuItem";
        public static string CrmSettingsMenuItem = "CrmSettingsMenuItem";
        public static string SecurityTabUiMenuItem = "SecurityTabUiMenuItem";
        #endregion

        #region Trade messages
        public static string ApprovedSignatureMessage = "File uploaded.";
        public static string BlockedUserMessage = "The account is blocked. Contact the support team for more information";
        public static string CloseTradeMessage = "Trade was closed successfully";
        public static string PendingTradeOpenedMessage = "An order has been placed, BUY order for {0}, Amount of {1} units. Trade id is";
        public static string BuyCfdOrderActivatedMessage = "An order has been placed, BUY order for {0}, Amount of {1} units. Trade id is";
        public static string BuyNftOrderExecutedMessage = "An order has been executed, BUY for {0}, Amount of 1 units. Trade id is";
        public static string SellOrderActivatedMessage = "An order has been placed, SELL order for {0}, Amount of {1} units. Trade id is";
        public static string ChronoOrderActivatedMessage = "BUY chrono order activated, for";
        public static string ChronoBlockTradeMessage = "Minimum available required";
        #endregion

        #region Dashboard messages
        public static string LoginComment = "has logged into the erp and is currently online";
        public static string RegisterComment = "registered from API";
        public static string AddComment = "Added a comment";
        #endregion

        #region Filters
        public static string CampaignsFilter = "campaigns-filter";
        public static string FtdFilter = "ftd-filter";
        public static string DepositFilter = "deposit-filter-filter";
        public static string AssetsFilter = "assets-filter";
        public static string SalesAgentsFilter = "sales-agents-filter";
        public static string TradeTypeFilter = "trade-type-filter";
        public static string TypeFilter = "type-filter";
        public static string TodayFilter = "day";     
        #endregion

        #region Crm configurations
        public static string AdminWithUsersOnlyRoleName = "adminwithusersonly";
        public static string SalasAgentSalaryName = "AutomationSalary";
        public static string AutomationDepositRangeGroupName = "Automation deposit range group";
        public static string AdminWithDialerRole = "adminwithdialer";
        public static string AdminUsersOnlyWithViewTradesOnly = "adminusersunlywithviewtradesonly";
        public static string AdminRole = "admin";
        public static string AgentRole = "agent";
        public static string ApiKeyOfWooCommerceUser;
        public static string OpenTradesTableName = "open_trades";
        public static string BonusTableName = "bonuses";
        public static string ClientsTableName = "clients";
        public static string DepositsTableName = "deposit";
        public static string RiskTableName = "risk";
        public static string WithdrawalsTableName = "withdrawals";
        public static string ChargebacksTableName = "chargebacks";
        public static string PendingTradesTableName = "pending_trades";
        public static string CloseTradesTableName = "close_trades";
        public static string EmailPrefix = "@auto.local";
        public static string DefaultPspTransactionId = "505";
        public static string UserDefaultPhone = "22254445";
        public static string UserDefaultCountry = "afghanistan";
        public static string ClientFreeText = "free_text";
        public static string UserDefaultExtension = "5558";
        public static string AirsoftOfficeName = "Airsoft";
        public static string MainOfficeName = "Main Office";
        public static string PbxNameAutomation = "for dialer test";
        public static string PbxType = "Voicespin";
        public static string AutomationTrunkName = "Automation trunk";
        public static string AutomationBannerName = "Automation Banner";
        public static string AffiliateWithNoPermissionRole = "affiliate with no permission role";
        public static string AffiliateWithAllPermissionRole = "affiliate with all permission role";
        public static string FileNameToUpload = "FileToUpload.png";
        public static string PdfFileNameToUpload = "PdfForAutomation.pdf";
        public static string TestimEmailPrefix = "@testim.airsoftltd.com";
        public static string Password = "Automation8!";
        public static string AccountTypeNameForAutomation = "AccountTypeNameForAutomation";
        public static string ProgressFilterConversion = "Conversion";
        public static string ProgressFilterTotalDeposit = "Total Deposit";
        public static string AirsoftSandboxPspName = "airsoft-sandbox";
        public static string BankTransferPspName = "bank transfer";
        public static string AirsoftSandboxPspApiKey = "9429771322F9A";
        public static string LastDigits = "5054";
        public static string AirsoftSandboxPspMerchantId = "Airsoftest";
        public static string AirsoftSandboxPspPrivateApiSecret = "ZYxkCxuik5";
        public static string TableNameForFinanceDataTest = "finance_data_of_client_card";      
        public static string TestimDbConnectionString = "mongodb+srv://testim:g63Hp7UreWgyJDrY@dev01-de-cwsxo.mongodb.net/testim";
        #endregion

        #region Shared crm locator's
        public static By DataTablesEmptyExp = By.CssSelector("td[class='dataTables_empty']");
        public static By ProgressBarExp = By.CssSelector("bar[style*='width']");
        public static By DataTablesInfoExp = By.XPath("//div[contains(@id,'_info')]//ancestor::div[@class='dataTables_info' and contains(.,1)]");
        public static By CardButtonsForAnimationExp = By.CssSelector("div[class*='asset-info-animated-in']");
        public static By ChronoPlatformMenuItemExp = By.CssSelector("div[class*='deposit_box orange-background']");
        public static By AllowedIpAddressExp = By.CssSelector("p-chips[id='allowed_ip_addresses'] input");
        public static By TradesSearchFiledExp = By.CssSelector("div[id='openTradesTable_filter'] input[type='search']");
        public static By DataTableRowsExp = By.CssSelector("div[class='dataTables_scrollBody'] table tr[class='odd']");
        public static By ProfileFirstNameExp = By.CssSelector("span[class='profile_name']");
        public static By SignaturePadExp = By.CssSelector("canvas[id='signature-pad']");
        public static By SaveSignatureBtnExp = By.CssSelector("button[id='save-signiture']");
        public static By ChooseClientCheckBoxExp = By.CssSelector("label[class*='customer-check'] div[class='input-indicator']");

        #endregion

        #region Dev mgm data
        public static string ApiRoutePostCreateMgmApiKey = "/api/mgm/users/generate-api-key";
        public static string AssetNameForCreateAsset = "test";
        public static string MgmUrl = "https://mgm-automation.airsoftltd.com";
        public static string MgmQaDevAutoId = "5f47f034f4f361bf19fe8fbd";
        public static string MgmUserName = "lior@airsoftltd.com";
        public static string MgmPassword = "Automation6!";
        public static string MgmSuperAdminUserName = "support@airsoftltd.com";
        #endregion

        #region General configuration  
        public static string OfficeIp = "212.116.171.54"; //"147.235.229.98"; 

        public static string[] UserAllowedIps = { "141.226.27.66", "82.166.207.154", "176.231.50.131", OfficeIp, // office
            "34.255.130.185", // for Dev selenium run
            "94.230.92.150", "83.130.81.37", "176.231.3.46",  // home "141.226.27.66", "87.71.139.122", 
            "99.80.144.236", "34.247.140.168", "18.202.101.240", "87.71.136.54" };// jenkines
      

        public static string BuildName =
            $"{Environment.GetEnvironmentVariable("systemType")}-{TextManipulation.RandomString()}" +
            $"-{DateTime.Now:dd/MM/yyyy - H:mm:ss}";

        public static string UsersTable = "users";
        public static string SitesTable = "sites";
        public static string PspLogsTable = "psp_logs";
        public static string PspTable = "psp";
        public static List<string> EmailListForExport = new List<string>();
        public static List<string> EmailListForAdminDeposit = new List<string>();
        public const int NumOfRetraiesForFallingTest = 2;
        public const string AssetName = "ETHUSD";
        public const string DefaultUSDCurrencyName = "USD";
        public const string Platform = "cfd";
        public const string AssetNameShort = "ETH";
        public const string AssetNftSymbol = "Doodles";
        public const string AssetNftLongSymbol = "DOODLES-OFFICIAL";
        public const string Chrome = "chrome";
        public const string SanityCategory = "sanity";
        public const string MgmSanityCategory = "mgm-sanity";
        public const string RegressionCategory = "regression";
        public const string ApiDocCategory = "apidoc";
        public const string TestCategory = "test";
        public const string ExportEmailTemplateSubject = "Export table";
        public const string TesimUrl = "http://testim.airsoftltd.com";
        public const string TradeUrl = "trade.airsoftltd.com/trade";
        public const int TimeToWaitFromSeconds = 30;
        public const int TimeToWaitFromSecondsForElementBased = 30;

        public static List<string> EmailGlobalVariables = new List<string> {
            "CLIENT_ID", "CLIENT_NAME",
            "CLIENT_FIRST_NAME", "CLIENT_LAST_NAME", "CLIENT_CURRENCY_SYMBOL", "CLIENT_EMAIL",
            "CLIENT_PHONE", "CLIENT_BALANCE", "COMPANY_LOGO", "DATE" };
        #endregion

        #region Create client & client card locator's
        public static By FirstNameExp = By.CssSelector("input[class*='first-name']");
        public static By LastNameExp = By.CssSelector("input[class*='last-name']");
        public static By EmailExp = By.CssSelector("input[class*='email']");
        public static By PhoneExp = By.CssSelector("input[class*='phone']");
        public static By CountryExp = By.XPath("//select[contains(@class,'country')]");
        public static By GmtTimeZoneExp = By.XPath("//select[contains(@class,'gmt-timezone')]");
        public static By CurrencyCodeExp = By.XPath("//select[contains(@class,'currency')]");
        public static By SaveExp = By.CssSelector("button[class*='save-btn']");
        public static By PasswordExp = By.CssSelector("input[type='password']");
        public static By UserNamedExp = By.CssSelector("input[name='username']");      
        public static By CreateClientButtonExp = By.CssSelector("button[class*='create-new-client-btn']");
        public static By PhoneCallAnimationExp = By.CssSelector("div[class*='modal fade phone-animation'][style='display: block;'");
        #endregion

        #region Trade locators
        public static By BuyAndSellButtonsForAnimationExp = By.CssSelector("div[class*='card_buttons");
        #endregion

        #region General locators
        public static By CommentExp = By.CssSelector("textarea[id='comment']");
        public static string WorkingHoursFrom = "12:00";
        public static string WorkingHoursTo = "12:01";
        #endregion

        #region Alert buttons
        public static By ConfirmExp = By.CssSelector("div[class*='action-alert'] button[class*='confirm-action-btn']");
        public static string AlertOnFront = "//div[contains(@class,'ajs-message') and contains(., '{0}')]";
        #endregion
    }
}
