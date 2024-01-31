// Ignore Spelling: Api Forex Crm ftd app apikey

using AirSoftAutomationFramework.Objects.DTOs;
using OpenQA.Selenium;
using System.Collections.Generic;

namespace AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.Clients.ClientPage
{
    public interface IClientsApi
    {
        T ChangeContext<T>(IWebDriver driver) where T : class;
        List<ImportClientRequest> CreateClientDataForImport(int numOfClients);
        IClientsApi DeleteAttributionRolesRequest(string uri, List<AttributionRoleResponse> attributionRoleResponses);
        string DeleteClientRequest(string url, string clientId, string apiKey, bool checkStatusCode = true);
        string DeleteMassAssignCommentsRequest(string url, string[] clientsIds, string apiKey, bool checkStatusCode = true);
        string ExportClientsTablePipe(string url, string clientEmail, string userEmail, string apiKey = null, bool checkStatusCode = true);
        GetAllClientsResponse GetAllClientsRequest(string uri);
        List<Leads> GetAllLeadsRequest(string uri, string clientId, string apiKey);
        GeneralResult<List<AttributionRoleResponse>> GetAttributionRolesRequest(string uri, string apiKey = null, bool checkStatusCode = true);
        GeneralResult<GetClientsRespose> GetClientByFilterRequest(string url, Dictionary<string, string> filtersValues, string apiKey = null, bool checkStatusCode = true);
        GeneralResult<GetClientsRespose> GetClientByFilterRequest(string url, string filterName, string filterValue, string apiKey = null, bool checkStatusCode = true);
        GeneralResult<GetClientCardResponse> GetClientByIdRequest(string url, string clientId, string apiKey = null, bool checkStatusCode = true);
        GeneralResult<GetClientsRespose> GetClientRequest(string url, string searchValue, string apiKey = null, bool checkStatusCode = true);
        List<string> GetClientTableColumns(string url, string clientEmail);
        string GetDownloadKycFileRequest(string url, string kycUrl);
        string MassDeleteClientsRequest(string url, string[] clientsIds, string apiKey, bool checkStatusCode = true);
        string MassPatchAssignTradingGroupRequest(string url, string groupId, string[] clientsIds, string apiKey, bool checkStatusCode = true);
        string PatchAssignSaleAgentRequest(string url, string userId, List<string> clientsIds, string apiKey = null, bool checkStatusCode = true);
        string PatchAssignTreadingGroupRequest(string url, string clientsId, string newTreadingGroupId, string oldTreadingGroupId, string platformName, string apiKey = null, bool checkStatusCode = true);
        string PatchMassAssignBannerMessageRequest(string url, string bannerId, string[] clientsIds, string apiKey, bool checkStatusCode = true);
        GeneralResult<GeneralDto> PatchMassAssignCampaign(string url, List<string> clientsIds, string campaignId, string apiKey = null, bool checkStatusCode = true);
        string PatchMassAssignComplianceStatusRequest(string url, string activationStatus, List<string> clientsIds, string apiKey = null, bool checkStatusCode = true);
        string PatchMassAssignRandomSaleAgentsRequest(string url, string userId, string[] clientsIds, string apiKey, bool checkStatusCode = true);
        string PatchMassAssignSaleAgentsRequest(string url, string userId, List<string> clientsIds, string apiKey = null, bool checkStatusCode = true);
        string PatchMassAssignSalesStatusRequest(string url, string saleStatus, List<string> clientsIds, string apiKey = null, bool checkStatusCode = true);
        string PostCreateAttributionRoleRequest(string url, string attributionRoleName, string[] campaignId = null, string actualType = null, string[] countryNames = null, string[] ftdAgentIds = null, string[] retentionAgentIds = null, string retentionType = null, string actualSplit = null, int? actualVolume = null, string apiKey = null, bool checkStatusCode = true);
        IClientsApi PostImportLeadsRequest(string url, string fileName, string filePath, string campaignId, string apikey = null);
        string PostMassAssignCommentRequest(string url, string[] clientsIds, string comment, string apiKey = null, bool checkStatusCode = true);
        IClientsApi PutAttributionRoleRequest(string uri, AttributionRoleResponse attributionRoleResponse);
        IClientsApi WaitForClientToBeLogin(string url, string searchValue, string apiKey = null);
        IClientsApi WaitForClientToCreate(string url, List<string> SearchValues);
        IClientsApi WaitForClientToCreate(string url, string SearchValue);
        IClientsApi WaitForImportClient(string url, List<string> clientNames);
    }
}