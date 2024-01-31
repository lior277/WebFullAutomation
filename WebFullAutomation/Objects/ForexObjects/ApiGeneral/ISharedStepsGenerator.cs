// Ignore Spelling: Api Forex crm

using AirSoftAutomationFramework.Objects.DTOs;
using AirSoftAutomationFramework.Objects.ForexObjects.Ui.UiGeneral;
using OpenQA.Selenium;
using System.Collections.Generic;

namespace AirSoftAutomationFramework.Objects.ForexObjects.ApiGeneral
{
    public interface ISharedStepsGenerator
    {
        T ChangeContext<T>(IWebDriver driver) where T : class;
        Dictionary<string, string> CreateAffiliateAndCampaignApi(string crmUrl, string[] blockedCountries = null, string roleName = null, string campaignCode = null, string apiKey = null, bool checkStatusCode = true);
        string CreateExportParamsString(List<string> tableColumns, string tableName);
        IDictionary<string, string> CreateTradeGroupAndAssignItToClientPipe(string crmUrl, Default_Attr groupAttributes, string clientId, string apiKey = null);
        string CreateUrlWithFilter(Dictionary<string, string> filters);
        List<string> GetBankingTableColumns(string url, string clientEmail, string transactionsType);
        List<string> GetCrmTradeTablesColumns(string url, string clientName, string tradeType);
        string GetError404();
        string GetExportLinkFromExportEmailBody(string url, string userEmail, string filter = null);
        List<string> GetTradeTableColumns(string url, string tradeType);
        ISharedStepsGenerator NavigateToPageByName(string url, string pageName, bool checkUrl = true);
        ISharedStepsGenerator PatchResetDevBrandPipe(string mgmUrl, string brandUrl, IWebDriver driver = null);
        ISharedStepsGenerator PatchResetDevBrandRequest();
        ISharedStepsGenerator PutTableColumnVisibilityRequest(string uri, List<string> tableNames, string columnName, bool visibility, string apiKey);
        ISharedStepsGenerator PutTableColumnVisibilityRequest(string uri, string tableName, List<string> columnName, bool visibility, string apiKey);
        ISharedStepsGenerator PutTableColumnVisibilityRequest(string uri, string tableName, string columnName, bool visibility, string apiKey);
        ISearchResultsUi SearchClient(string searchText, By by = null);
        ISharedStepsGenerator SetTheNewApiKey(string newDevBrandApiKey);
        ISharedStepsGenerator UpdateExportTableEmailTemplate(string url);
        ISharedStepsGenerator UploadFileOnGrid(By by, string filePath);
        ISharedStepsGenerator VerifyDashboardPage();
        ISharedStepsGenerator VerifyExportTableButtonNotExist();
        ISharedStepsGenerator WaitForTableToLoad();
    }
}