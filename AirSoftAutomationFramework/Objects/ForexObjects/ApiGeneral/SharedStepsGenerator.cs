// Ignore Spelling: Api Forex crm

using AirSoftAutomationFramework.Internals.DAL.ApiAccess;
using AirSoftAutomationFramework.Internals.DAL.ApiAccess.ApiRoutes;
using AirSoftAutomationFramework.Internals.DAL.ConfigurationManagement;
using AirSoftAutomationFramework.Internals.ExtensionsMethods;
using AirSoftAutomationFramework.Internals.Factory;
using AirSoftAutomationFramework.Internals.Helpers;
using AirSoftAutomationFramework.Objects.DTOs;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.AccountsApi.UsersMenuApi.UserMenuApi;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.CampaignsApi.Dashboard;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.Clients.ClientCard;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.Clients.ClientCard.FinancesTab;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.Clients.ClientPage;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.Settings;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Trade;
using AirSoftAutomationFramework.Objects.ForexObjects.Ui.UiGeneral;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using OpenQA.Selenium;
using OpenQA.Selenium.Remote;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Transactions;

namespace AirSoftAutomationFramework.Objects.ForexObjects.ApiGeneral
{
    public class SharedStepsGenerator : ISharedStepsGenerator
    {
        #region Members     
        private IUserApi _createUserApi;
        private ICreateClientApi _createClientApi;
        private IClientsApi _clientsApi;
        private IApiAccess _apiAccess;
        private Dictionary<string, string> _mailsAndUersIds = new Dictionary<string, string>();
        private string _apiKey = Config.appSettings.ApiKey;
        private string _crmUrl = Config.appSettings.CrmUrl;
        private List<string> _usersIds = new List<string>();
        private List<string> _usersMail = new List<string>();
        private List<CreateUserRequest> _usersListRequests = new List<CreateUserRequest>();
        private List<CreateUserRequest> _subUsersList = new List<CreateUserRequest>();
        private List<CreateUserRequest> _managerUsersList = new List<CreateUserRequest>();
        private List<CreateClientRequest> _clientsListRequests = new List<CreateClientRequest>();
        private List<string> _clientIds = new List<string>();
        private List<string> _clientsMail = new List<string>();
        private Dictionary<string, string> _mailsAndClientIds = new Dictionary<string, string>();
        private string _testimUrl = DataRep.TesimUrl;
        private IWebDriver _driver;
        private readonly IApplicationFactory _apiFactory;
        private IFinancesTabApi _createDepositApi;
        private string _mailPerfix = DataRep.EmailPrefix;
        #endregion Members

        #region Locator's  
        private readonly By ExportBtnExp = By.CssSelector("button[class*='buttons-excel']");
        private readonly By Error404Exp = By.CssSelector("div[class='page page-core page-404']");
        private readonly By WaitForTableToLoadExp = By.CssSelector("div[class*" +
            "='dataTables_processing panel'][style='display: none;'], app-trade-table[style='visibility: visible;']");

        #endregion Locator's 

        public SharedStepsGenerator(IClientsApi clientsApi, IFinancesTabApi createDepositApi,
            ICreateClientApi createClientApi, IUserApi createUserApi,
            IApplicationFactory apiFactory, IWebDriver driver, IApiAccess apiAccess)
        {
            _apiAccess = apiAccess;
            _clientsApi = clientsApi;
            _createDepositApi = createDepositApi;
            _apiFactory = apiFactory;
            _driver = driver;
            _createUserApi = createUserApi;
            _createClientApi = createClientApi;
        }

        public string CreateUrlWithFilter(Dictionary<string, string> filters)
        {
            var filtersNames = filters.Keys.ToList();
            var filtersValues = filters.Values.ToList();
            var sb = new StringBuilder();

            for (var i = 0; i < filtersNames.Count; i++)
            {
                if (filtersNames[i].Contains("[]"))
                {
                    var temp = filtersNames[i].Split("[").First();
                    sb.Append("&filter[").Append(temp).Append("][]=").Append(filtersValues[i]);
                }
                else
                {
                    sb.Append("&filter[").Append(filtersNames[i]).Append("]=").Append(filtersValues[i]);
                }
            }

            return sb.ToString();
        }

        public List<string> GetTradeTableColumns(string url, string tradeType)
        {
            var route = $"{url}{ApiRouteAggregate.PostExportTradesTables(tradeType)}?api_key={_apiKey}";
            var response = _apiAccess.ExecuteGetEntry(route);

            var json = response
                .Content
                .ReadAsStringAsync()
                .Result;

            var columnNames = new List<string>();
            var jToken = JToken.Parse(json);
            var dataToken = jToken.SelectToken("data").FirstOrDefault();
            json = JsonConvert.SerializeObject(dataToken);
            var dictionary = JsonConvert.DeserializeObject<Dictionary<string, object>>(json);
            dictionary.ForEach(p => columnNames.Add(p.Key));

            return columnNames;
        }

        public List<string> GetCrmTradeTablesColumns(string url, string clientName, string tradeType)
        {
            var route = $"{url}{ApiRouteAggregate.PostExportCrmTradeTables(tradeType)}" +
               $"?start=0&order[0][column]=full_name&search[value]={clientName}&api_key={_apiKey}";

            var response = _apiAccess.ExecuteGetEntry(route);
            _apiAccess.CheckStatusCode(route, response);

            var json = response
                .Content
                .ReadAsStringAsync()
                .Result;

            var columnNames = new List<string>();
            var jToken = JToken.Parse(json);
            var dataToken = jToken.SelectToken("data").FirstOrDefault();
            json = JsonConvert.SerializeObject(dataToken);
            var dictionary = JsonConvert.DeserializeObject<Dictionary<string, object>>(json);
            dictionary.ForEach(p => columnNames.Add(p.Key));

            return columnNames;
        }

        public ISharedStepsGenerator PatchResetDevBrandRequest()
        {
            var route = ApiRouteAggregate.PatchResetDevBrand();

            var patchResetDevBrand = new
            {
                @namespace = "amit-brand"
            };
            var response = _apiAccess.ExecutePatchEntry(route, patchResetDevBrand);
            _apiAccess.CheckStatusCode(route, response);

            return this;
        }

        public string GetError404()
        {
            return _driver.SearchElement(Error404Exp)
                .GetElementText(_driver, Error404Exp);
        }

        public ISharedStepsGenerator PatchResetDevBrandPipe(string mgmUrl, string brandUrl,
            IWebDriver driver = null)
        {
            PatchResetDevBrandRequest();
            Thread.Sleep(3000);

            //var devBrandApiKey = _apiFactory
            //    .ChangeContext<ILoginPageUi>(_driver)
            //    .LoginPipe(DataRep.MgmUserName, mgmUrl, DataRep.MgmPassword)
            //    .ChangeContext<IMgmDashboardUi>(_driver)
            //    .ClickOnFastLoginBrandBtn()
            //    .ChangeContext<ISharedStepsGenerator>(_driver)
            //    .NavigateToPageByName(brandUrl, "/accounts/users")
            //    .ChangeContext<IUsersPageUi>(_driver)
            //    .SearchUser(DataRep.MgmSuperAdminUserName)
            //    .ClickOnEditUserButton()
            //    .ClickOnSendApiKeyBtn()
            //    .GetApiKey();

            //SetTheNewApiKey(devBrandApiKey);

            return this;
        }

        public List<string> GetBankingTableColumns(string url,
            string clientEmail, string transactionsType)
        {
            var route = $"{url}{ApiRouteAggregate.GetTransactions(transactionsType)}" +
                $"?start=0&search[value]={clientEmail}&api_key={_apiKey}";

            var response = _apiAccess.ExecuteGetEntry(route);
            _apiAccess.CheckStatusCode(route, response);

            var json = response
                .Content
                .ReadAsStringAsync()
                .Result;

            var columnNames = new List<string>();
            var jToken = JToken.Parse(json);
            var dataToken = jToken.SelectToken("data").FirstOrDefault();
            json = JsonConvert.SerializeObject(dataToken);
            var dictionary = JsonConvert.DeserializeObject<Dictionary<string, object>>(json);
            dictionary.ForEach(p => columnNames.Add(p.Key));

            return columnNames;
        }

        public ISharedStepsGenerator SetTheNewApiKey(string newDevBrandApiKey)
        {
            Environment.SetEnvironmentVariable("apikey", newDevBrandApiKey);

            return this;
        }

        public ISharedStepsGenerator UploadFileOnGrid(By by, string filePath)
        {
            var allowsDetection = (IAllowsFileDetection)_driver;
            allowsDetection.FileDetector = new LocalFileDetector();

            // if input file is disable or hidden
            // String js = "arguments[0].style.height='auto'; arguments[0].style.visibility='visible';";

            _driver.SearchElement(by)
                .SendKeys(filePath);

            return this;
        }

        public ISharedStepsGenerator PutTableColumnVisibilityRequest(string uri,
            string tableName, string columnName, bool visibility, string apiKey)
        {
            _apiKey = apiKey ?? _apiKey;
            var route = ApiRouteAggregate.PutColumnsVisibility(tableName);
            route = $"{uri}{route}?api_key={_apiKey}";
            var json = "{\"" + columnName + "\" : " + visibility.ToString().ToLower() + "}";
            dynamic jsonResponse = JsonConvert.DeserializeObject(json);
            var response = _apiAccess.ExecutePutEntry(route, jsonResponse);
            _apiAccess.CheckStatusCode(route, response);

            return this;
        }

        public ISharedStepsGenerator PutTableColumnVisibilityRequest(string uri,
            List<string> tableNames, string columnName, bool visibility, string apiKey)
        {
            foreach (var table in tableNames)
            {
                PutTableColumnVisibilityRequest(uri, table, columnName, visibility, apiKey);
            }

            return this;
        }

        public ISharedStepsGenerator PutTableColumnVisibilityRequest(string uri,
            string tableName, List<string> columnName, bool visibility, string apiKey)
        {
            foreach (var column in columnName)
            {
                PutTableColumnVisibilityRequest(uri, tableName, column, visibility, apiKey);
            }

            return this;
        }

        public ISearchResultsUi SearchClient(string searchText, By by = null)
        {
            _driver.WaitForAtListOneElement(DataRep.DataTableRowsExp, 120);
            var element = _driver.SearchElement(DataRep.TradesSearchFiledExp, 60);
            element.SendsKeysAuto(_driver, DataRep.TradesSearchFiledExp, searchText, 60);

            return _apiFactory.ChangeContext<ISearchResultsUi>(_driver);
        }

        public Dictionary<string, string> CreateAffiliateAndCampaignApi(string crmUrl,
            string[] blockedCountries = null, string roleName = null, string campaignCode = null,
            string apiKey = null, bool checkStatusCode = true)
        {
            _apiKey = apiKey ?? _apiKey;
            var apiFactory = new ApplicationFactory();
            var campaignDetails = new Dictionary<string, string>();
            var affiliateName = TextManipulation.RandomString();
            var campaignName = TextManipulation.RandomString();

            var affiliateId = apiFactory
                .ChangeContext<ICreateAffiliateApi>(_driver)
                .CreateAffiliateApiPipe(crmUrl, affiliateName,
                roleName, apiKey: _apiKey);

            var campaignId = apiFactory
                .ChangeContext<ICampaignPageApi>(_driver)
                .PostCreateCampaignRequest(crmUrl, campaignName,
                affiliateId, leadsNum: null, userEmail: null,
                blockedCountrys: blockedCountries,
                acceptingLeadsHoursActive: false,
                acceptingLeadsHoursFrom: "12:00",
                acceptingLeadsHoursTo: "12:00",
                stopTraffic: true, sendEmail: true,
                limitCountry: null, timeFrame: null,
                campaignCode: campaignCode,
                apiKey: _apiKey, checkStatusCode);

            campaignDetails.Add(campaignName, campaignId);
            campaignDetails.Add("affiliateId", affiliateId);

            return campaignDetails;
        }

        public string GetExportLinkFromExportEmailBody(string url,
            string userEmail, string filter = null)
        {
            var apiFactory = new ApplicationFactory();
            UpdateExportTableEmailTemplate(url);

            return apiFactory
                .ChangeContext<IPlatformTabApi>(_driver)
                .FilterEmailByBodyPipe(_testimUrl, userEmail, filter)
                .Body
                .Split("export_link=")
                .Last();
        }

        public ISharedStepsGenerator UpdateExportTableEmailTemplate(string url)
        {
            var apiFactory = new ApplicationFactory();
            var emailBodyParams = new List<string> { "EXPORT_LINK" };

            var emailsParams = new Dictionary<string, string> {  { "id", "5db7022798bfcfab4dacbc86" },
                { "type", "export_table" }, { "language", "en" }, { "subject", DataRep.ExportEmailTemplateSubject }};

            // update the Export table template
            apiFactory
               .ChangeContext<IPlatformTabApi>(_driver)
               .UpdateEmailTemplatePipe(url, emailsParams, emailBodyParams);

            return this;
        }

        public IDictionary<string, string> CreateTradeGroupAndAssignItToClientPipe(string crmUrl,
            Default_Attr groupAttributes, string clientId, string apiKey = null)
        {
            _apiKey = apiKey ?? _apiKey;
            var apiFactory = new ApplicationFactory();
            var groupDetails = new Dictionary<string, string>();
            var groupName = TextManipulation.RandomString();

            var groupId = apiFactory
               .ChangeContext<ITradeGroupApi>(_driver)
               .PostCreateTradeGroupRequest(crmUrl, new List<object> { groupAttributes },
               groupName, apiKey: _apiKey).Trim('"');

            groupId ??= apiFactory
               .ChangeContext<ITradeGroupApi>(_driver)
               .PostCreateTradeGroupRequest(crmUrl, new List<object> { groupAttributes },
               groupName, apiKey: _apiKey).Trim('"');

            apiFactory
                .ChangeContext<IInformationTabApi>(_driver)
                .PatchSetTradingGroupRequest(crmUrl, groupId,
                new List<string> { clientId }, _apiKey);

            groupDetails.Add(groupId, groupName);

            return groupDetails;
        }

        public ISharedStepsGenerator WaitForTableToLoad()
        {
            _driver.WaitForDataTableToLoad(WaitForTableToLoadExp);

            return this;
        }

        public string CreateExportParamsString(List<string> tableColumns, string tableName)
        {
            var today = DateTime.Now.ToString("yyyy-MM-dd");
            var sb = new StringBuilder();

            var start = $"draw=1&order[0][dir]=asc&start=0&length=" +
                $"10000000&search[value]=&search[regex]=false" +
                $"&export=1&table={tableName}";

            foreach (var column in tableColumns)
            {
                sb.Append("&projection[]=").Append(column);
            }
            var ft = sb.ToString();

            var exportParams = start + ft;

            if (tableName == "risk")
            {
                exportParams = (start + ft).EncodeBase64();
            }

            return exportParams;
        }

        public ISharedStepsGenerator NavigateToPageByName(string url,
            string pageName, bool checkUrl = true)
        {

            _driver.NavigateToPageByName(url, pageName, checkUrl);

            return this;
        }

        public ISharedStepsGenerator VerifyExportTableButtonNotExist()
        {
            _driver.WaitForElementNotExist(ExportBtnExp);

            return this;
        }

        public ISharedStepsGenerator VerifyDashboardPage()
        {
            _driver.WaitForUrlToContain("dashboard");

            return this;
        }

        public T ChangeContext<T>(IWebDriver driver) where T : class
        {
            return _apiFactory.ChangeContext<T>(_driver);
        }
    }
}
