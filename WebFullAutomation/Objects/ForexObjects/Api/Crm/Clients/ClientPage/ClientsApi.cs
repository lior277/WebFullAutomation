// Ignore Spelling: Api Forex Crm ftd app apikey

using AirSoftAutomationFramework.Internals.DAL.ConfigurationManagement;
using AirSoftAutomationFramework.Internals.ExtensionsMethods;
using AirSoftAutomationFramework.Internals.Helpers;
using AirSoftAutomationFramework.Objects.DTOs;
using AirSoftAutomationFramework.Objects.ForexObjects.ApiGeneral;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using AirSoftAutomationFramework.Internals.DAL.ApiAccsess;
using AirSoftAutomationFramework.Internals.DAL.ApiAccsess.ApiRoutes;
using AirSoftAutomationFramework.Internals.Factorys;

namespace AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.Clients.ClientPage
{
    public class ClientsApi : IClientsApi
    {
        #region Members     
        private IApiAccess _apiAccess;
        private string _apiKey = Config.appSettings.ApiKey;
        private readonly IApplicationFactory _apiFactory;
        private IWebDriver _driver;
        #endregion Members

        public ClientsApi(IApplicationFactory appFactory,
            IApiAccess apiAccess, IWebDriver driver = null)
        {
            _driver = driver;
            _apiFactory = appFactory;
            _apiAccess = apiAccess;
        }

        public string PatchMassAssignSaleAgentsRequest(string url, string userId,
            List<string> clientsIds, string apiKey = null, bool checkStatusCode = true)
        {
            var ids = new StringCollection();
            ids.AddRange(clientsIds.ToArray());
            var idsArray = ids.Cast<string>()
                .ToArray();

            _apiKey = apiKey ?? _apiKey;
            var route = $"{url}{ApiRouteAggregate.PatchMassAssignSalesAgent()}?api_key={_apiKey}";

            var conectUserToClientDto = new
            {
                id = userId,
                ids = idsArray
            };
            var response = _apiAccess.ExecutePatchEntry(route, conectUserToClientDto);
            var json = response.Content.ReadAsStringAsync().Result;

            if (checkStatusCode)
            {
                _apiAccess.CheckStatusCode(route, response);
            }

            return json;
        }

        public string PatchAssignSaleAgentRequest(string url, string userId,
            List<string> clientsIds, string apiKey = null, bool checkStatusCode = true)
        {
            var ids = new StringCollection();
            ids.AddRange(clientsIds.ToArray());
            var idsArray = ids.Cast<string>()
                .ToArray();

            _apiKey = apiKey ?? _apiKey;
            var route = $"{url}{ApiRouteAggregate.PatchAssignSaleAgent()}?api_key={_apiKey}";

            var conectUserToClientDto = new
            {
                id = userId,
                ids = idsArray
            };
            var response = _apiAccess.ExecutePatchEntry(route, conectUserToClientDto);
            var json = response.Content.ReadAsStringAsync().Result;

            if (checkStatusCode)
            {
                _apiAccess.CheckStatusCode(route, response);
            }

            return json;
        }

        public string PatchAssignTreadingGroupRequest(string url, string clientsId,
            string newTreadingGroupId, string oldTreadingGroupId,
            string platformName, string apiKey = null, bool checkStatusCode = true)
        {
            var idsArray = new string[] { clientsId };

            _apiKey = apiKey ?? _apiKey;
            var route = $"{url}{ApiRouteAggregate.PatchAssignTradingGroup()}?api_key={_apiKey}";

            var assignTreadingGroupDto = new
            {
                platform = platformName,
                id = newTreadingGroupId,
                old_group_id = oldTreadingGroupId,
                ids = idsArray
            };
            var response = _apiAccess.ExecutePatchEntry(route, assignTreadingGroupDto);
            var json = response.Content.ReadAsStringAsync().Result;

            if (checkStatusCode)
            {
                _apiAccess.CheckStatusCode(route, response);
            }

            return json;
        }

        public string PatchMassAssignRandomSaleAgentsRequest(string url, string userId,
            string[] clientsIds, string apiKey, bool checkStatusCode = true)
        {
            var ids = new StringCollection();
            ids.AddRange(clientsIds.ToArray());
            var idsArray = ids.Cast<string>().ToArray();
            var route = $"{url}{ApiRouteAggregate.PatchMassAssignRandomSaleAgents()}?api_key={apiKey}";

            var conectUserToClientDto = new
            {
                id = userId,
                ids = idsArray
            };
            var response = _apiAccess.ExecutePatchEntry(route, conectUserToClientDto);
            var json = response.Content.ReadAsStringAsync().Result;

            if (checkStatusCode)
            {
                _apiAccess.CheckStatusCode(route, response);
            }

            return json;
        }

        public string ExportClientsTablePipe(string url,
            string clientEmail, string userEmail, string apiKey = null, bool checkStatusCode = true)
        {
            _apiKey = apiKey ?? _apiKey;
            var route = $"{url}{ApiRouteAggregate.PostExportTableWithLink()}?api_key={_apiKey}";
            var today = DateTime.Now.ToString("yyyy-MM-dd");
            var columns = GetClientTableColumns(url, clientEmail);

            var exportParams = _apiFactory
                .ChangeContext<ISharedStepsGenerator>()
                .CreateExportParamsString(columns, "clients");

            var exportClientsTableDto = new
            {
                export_email = userEmail,
                export_params = exportParams,
                export_table_name = "clients",
                service_url = ApiRouteAggregate.GetClient(userEmail).Split('?').First(),
            };

            var response = _apiAccess.ExecutePostEntry(route, exportClientsTableDto);

            if (checkStatusCode)
            {
                _apiAccess.CheckStatusCode(route, response);
            }

            return response.Content.ReadAsStringAsync().Result;
        }

        public string PostCreateAttributionRoleRequest(string url,
            string attributionRoleName,
            string[] campaignId = null,
            string actualType = null,
            string[] countryNames = null,
            string[] ftdAgentIds = null,
            string[] retentionAgentIds = null,
            string retentionType = null,
            string actualSplit = null,
            int? actualVolume = null,
            string apiKey = null,
            bool checkStatusCode = true)
        {
            _apiKey = apiKey ?? _apiKey;
            var route = $"{url}" +
                $"{ApiRouteAggregate.PostCreteAttributionRole()}?api_key={_apiKey}";

            var attributionRoleDto = new
            {
                name = attributionRoleName,
                type = actualType ?? "campaign",
                country = countryNames ?? Array.Empty<string>(),
                split = actualSplit ?? "random",
                campaign_id = campaignId ?? Array.Empty<string>(),
                ftd_agent_id = ftdAgentIds ?? Array.Empty<string>(),
                retention_agent_id = retentionAgentIds ?? Array.Empty<string>(),
                retention_type = retentionType ?? "never",
                volume = actualVolume ?? default
            };

            var response = _apiAccess.ExecutePostEntry(route, attributionRoleDto);
            var json = response.Content.ReadAsStringAsync().Result;

            if (checkStatusCode)
            {
                _apiAccess.CheckStatusCode(route, response);
            }
            return json;
        }

        //public string PostImportLeadsRequest(string url, binary
        //    string attributionRoleName,
        //    string[] campaignId = null,
        //    string actualType = null,
        //    string[] countryNames = null,
        //    string[] ftdAgentIds = null,
        //    string[] retentionAgentIds = null,
        //    string retentionType = null,
        //    string actualSplit = null,
        //    int? actualVolume = null,
        //    string apiKey = null,
        //    bool checkStatusCode = true)
        //{
        //    _apiKey = apiKey ?? _apiKey;
        //    var route = $"{url}" +
        //        $"{ApiRouteAggregate.PostCreteAttributionRole()}?api_key={_apiKey}";

        //    var attributionRoleDto = new
        //    {
        //        name = attributionRoleName,
        //        type = actualType ?? "campaign",
        //        country = countryNames ?? Array.Empty<string>(),
        //        split = actualSplit ?? "random",
        //        campaign_id = campaignId ?? Array.Empty<string>(),
        //        ftd_agent_id = ftdAgentIds ?? Array.Empty<string>(),
        //        retention_agent_id = retentionAgentIds ?? Array.Empty<string>(),
        //        retention_type = retentionType ?? "never",
        //        volume = actualVolume ?? default
        //    };

        //    var response = _apiAccess.ExecutePostEntry(route, attributionRoleDto);
        //    var json = response.Content.ReadAsStringAsync().Result;

        //    if (checkStatusCode)
        //    {
        //        _apiAccess.CheckStatusCode(route, response);
        //    }

        //    return json;
        //}

        public IClientsApi DeleteAttributionRolesRequest(string uri, List<AttributionRoleResponse> attributionRoleResponses)
        {
            foreach (var item in attributionRoleResponses)
            {
                var route = $"{uri}{ApiRouteAggregate.PostCreteAttributionRole()}/{item._id}?api_key={_apiKey}";
                var response = _apiAccess.ExecuteDeleteEntry(route);
                _apiAccess.CheckStatusCode(route, response);
            }

            return this;
        }

        public GeneralResult<List<AttributionRoleResponse>> GetAttributionRolesRequest(string uri,
            string apiKey = null, bool checkStatusCode = true)
        {
            _apiKey = apiKey ?? _apiKey;
            var generalResult = new GeneralResult<List<AttributionRoleResponse>>();
            var route = $"{uri}{ApiRouteAggregate.PostCreteAttributionRole()}?api_key={_apiKey}";
            var response = _apiAccess.ExecuteGetEntry(route);

            var json = response
                .Content
                .ReadAsStringAsync()
                .Result;

            if (checkStatusCode)
            {
                _apiAccess.CheckStatusCode(route, response);

                generalResult.GeneralResponse = JsonConvert
                    .DeserializeObject<List<AttributionRoleResponse>>(json);
            }
            else
            {
                generalResult.Message = json;
            }

            return generalResult;
        }

        public IClientsApi PutAttributionRoleRequest(string uri,
            AttributionRoleResponse attributionRoleResponse)
        {
            var route = $"{uri}{ApiRouteAggregate.PutAttributionRole(attributionRoleResponse._id)}" +
                $"?api_key={_apiKey}";

            var response = _apiAccess.ExecutePutEntry(route, attributionRoleResponse);
            _apiAccess.CheckStatusCode(route, response);

            return this;
        }

        public string PatchMassAssignSalesStatusRequest(string url, string saleStatus,
            List<string> clientsIds, string apiKey = null, bool checkStatusCode = true)
        {
            var ids = new StringCollection();
            ids.AddRange(clientsIds.ToArray());
            var idsArray = ids.Cast<string>().ToArray();

            _apiKey = apiKey ?? _apiKey;
            var route = $"{url}{ApiRouteAggregate.PatchMassAssignSalesStatus()}?api_key={_apiKey}";

            var massAsignSalesStatusDto = new
            {
                sales_status = saleStatus,
                ids = idsArray
            };
            var response = _apiAccess.ExecutePatchEntry(route, massAsignSalesStatusDto);
            var json = response.Content.ReadAsStringAsync().Result;

            if (checkStatusCode)
            {
                _apiAccess.CheckStatusCode(route, response);
            }

            return json;
        }


        public string PatchMassAssignBannerMessageRequest(string url, string bannerId,
            string[] clientsIds, string apiKey, bool checkStatusCode = true)
        {
            var ids = new StringCollection();
            ids.AddRange(clientsIds.ToArray());
            var idsArray = ids.Cast<string>().ToArray();

            _apiKey = apiKey ?? _apiKey;
            var route = $"{url}{ApiRouteAggregate.PatchMassAssignSalesStatus()}?api_key={apiKey}";

            var patchMassAssignBannerMessage = new
            {
                id = bannerId,
                ids = idsArray,
                platform = "cfd"
            };
            var response = _apiAccess.ExecutePatchEntry(route, patchMassAssignBannerMessage);
            var json = response.Content.ReadAsStringAsync().Result;

            if (checkStatusCode)
            {
                _apiAccess.CheckStatusCode(route, response);
            }

            return json;
        }

        public string PatchMassAssignComplianceStatusRequest(string url, string activationStatus,
           List<string> clientsIds, string apiKey = null, bool checkStatusCode = true)
        {
            var ids = new StringCollection();
            ids.AddRange(clientsIds.ToArray());
            var idsArray = ids.Cast<string>().ToArray();

            _apiKey = apiKey ?? _apiKey;
            var route = $"{url}{ApiRouteAggregate.PatchMassAssignComplianceStatus()}?api_key={_apiKey}";

            var massAsignComplianceStatusDto = new
            {
                activation_status = activationStatus,
                ids = idsArray
            };

            var response = _apiAccess.ExecutePatchEntry(route, massAsignComplianceStatusDto);
            var json = response.Content.ReadAsStringAsync().Result;

            if (checkStatusCode)
            {
                _apiAccess.CheckStatusCode(route, response);
            }

            return json;
        }


        public string DeleteMassAssignCommentsRequest(string url, string[] clientsIds,
            string apiKey, bool checkStatusCode = true)
        {
            var route = $"{url}{ApiRouteAggregate.PostCreateMassAssignComment()}?api_key={apiKey}";

            var deleteMassAssignComments = new
            {
                ids = clientsIds
            };

            var response = _apiAccess
                 .ExecuteDeleteEntry(route, deleteMassAssignComments);

            var json = response
                .Content
                .ReadAsStringAsync()
                .Result;

            if (checkStatusCode)
            {
                _apiAccess.CheckStatusCode(route, response);
            }

            return json;
        }

        public string PostMassAssignCommentRequest(string url, string[] clientsIds,
            string comment, string apiKey = null, bool checkStatusCode = true)
        {
            _apiKey = apiKey ?? _apiKey;
            var route = $"{url}{ApiRouteAggregate.PostCreateMassAssignComment()}?api_key={apiKey}";

            var postCommentDto = new
            {
                comment = comment,
                ids = clientsIds
            };
            var response = _apiAccess.ExecutePostEntry(route, postCommentDto);

            var json = response.Content.ReadAsStringAsync().Result;

            if (checkStatusCode)
            {
                _apiAccess.CheckStatusCode(route, response);
            }

            return json;
        }

        public GeneralResult<GeneralDto> PatchMassAssignCampaign(string url,
            List<string> clientsIds, string campaignId,
            string apiKey = null, bool checkStatusCode = true)
        {
            var generalResult = new GeneralResult<GeneralDto>();
            _apiKey = apiKey ?? _apiKey;
            var route = $"{url}{ApiRouteAggregate.PatchConnectCampaignToClient()}?api_key={_apiKey}";

            var ConnectCampaignToClientDto = new CreateConnectionCampaignClientRequest
            {
                id = campaignId,
                ids = clientsIds
            };

            var response = _apiAccess.ExecutePatchEntry(route, ConnectCampaignToClientDto);
            var json = response.Content.ReadAsStringAsync().Result;

            if (checkStatusCode)
            {
                _apiAccess.CheckStatusCode(route, response);

                generalResult.GeneralResponse = JsonConvert
                    .DeserializeObject<GeneralDto>(json);
            }
            else
            {
                generalResult.Message = json;
            }

            return generalResult;
        }

        public List<Leads> GetAllLeadsRequest(string uri, string clientId, string apiKey)
        {
            _apiKey = apiKey ?? _apiKey;
            var route = $"{uri}{ApiRouteAggregate.GetAllLeads(clientId)}&api_key={_apiKey}";
            var response = _apiAccess.ExecuteGetEntry(route);
            _apiAccess.CheckStatusCode(route, response);
            var json = response.Content.ReadAsStringAsync().Result;

            return JsonConvert.DeserializeObject<GetAllLeadsResponse>(json)
                .leads
                .ToList();
        }

        public GetAllClientsResponse GetAllClientsRequest(string uri)
        {
            var route = $"{uri}{ApiRouteAggregate.GetCustomers()}?draw=1&length=1000&api_key={_apiKey}";
            var response = _apiAccess.ExecuteGetEntry(route);
            _apiAccess.CheckStatusCode(route, response);
            var json = response.Content.ReadAsStringAsync().Result;

            return JsonConvert.DeserializeObject<GetAllClientsResponse>(json);
        }

        public List<string> GetClientTableColumns(string url, string clientEmail)
        {
            var route = $"{url}{ApiRouteAggregate.GetClient(clientEmail)}&api_key={_apiKey}";
            var response = _apiAccess.ExecuteGetEntry(route);

            var json = response
                .Content
                .ReadAsStringAsync()
                .Result;

            var columnNames = new List<string>();
            var jToken = JToken.Parse(json);

            var dataToken = jToken.SelectToken("data")
                .FirstOrDefault();

            json = JsonConvert.SerializeObject(dataToken);
            var dictionary = JsonConvert.DeserializeObject<Dictionary<string, object>>(json);
            dictionary.ForEach(p => columnNames.Add(p.Key));

            return columnNames;
        }

        public IClientsApi WaitForClientToCreate(string url, string SearchValue)
        {
            for (var i = 0; i < 10; i++)
            {
                var client = GetClientRequest(url, SearchValue);

                if (client == null)
                {
                    client = GetClientRequest(url, SearchValue);
                    Thread.Sleep(400);

                    continue;
                }

                break;
            }

            return this;
        }

        public IClientsApi WaitForClientToCreate(string url, List<string> SearchValues)
        {
            var names = new List<object>();

            for (var i = 0; i < 30; i++)
            {
                foreach (var name in SearchValues)
                {
                    var clientName = GetClientRequest(url, name, Config.appSettings.ApiKey)
                        .GeneralResponse.data?
                        .FirstOrDefault()?
                        .full_name?
                        .Split(' ')?
                        .First();

                    if (clientName != null && !names.Contains(clientName))
                    {
                        names.Add(clientName);
                    }
                }

                if (names.Count != SearchValues.Count)
                {
                    Thread.Sleep(1000);

                    continue;
                }

                break;
            }

            if (names?.Count != SearchValues.Count)
            {
                throw new InvalidOperationException("client not created");
            }

            return this;
        }

        public IClientsApi WaitForClientToBeLogin(string url, string searchValue,
            string apiKey = null)
        {
            var isOnline = GetClientRequest(url, searchValue, apiKey)
                .GeneralResponse
                .data
                .FirstOrDefault()
                .online;

            for (var i = 0; i < 10; i++)
            {
                if (!isOnline)
                {
                    Thread.Sleep(700);

                    continue;
                }

                break;
            }

            return this;
        }

        public GeneralResult<GetClientsRespose> GetClientRequest(string url, string searchValue,
            string apiKey = null, bool checkStatusCode = true)
        {
            var generalResult = new GeneralResult<GetClientsRespose>();
            _apiKey = apiKey ?? _apiKey;
            var route = $"{url}{ApiRouteAggregate.GetClient(searchValue)}&api_key={_apiKey}";
            var response = _apiAccess.ExecuteGetEntry(route);

            var json = response
                .Content
                .ReadAsStringAsync()
                .Result;

            if (checkStatusCode)
            {
                _apiAccess.CheckStatusCode(route, response);

                generalResult.GeneralResponse = JsonConvert
                    .DeserializeObject<GetClientsRespose>(json);
            }
            else
            {
                generalResult.Message = json;
            }

            return generalResult;
        }

        public GeneralResult<GetClientCardResponse> GetClientByIdRequest(string url,
            string clientId, string apiKey = null, bool checkStatusCode = true)
        {
            var generalResult = new GeneralResult<GetClientCardResponse>();
            _apiKey = apiKey ?? _apiKey;
            var route = $"{url}{ApiRouteAggregate.GetClientCard(clientId)}?api_key={_apiKey}";
            var response = _apiAccess.ExecuteGetEntry(route);

            var json = response
                .Content
                .ReadAsStringAsync()
                .Result;

            if (checkStatusCode)
            {
                _apiAccess.CheckStatusCode(route, response);

                generalResult.GeneralResponse = JsonConvert
                    .DeserializeObject<GetClientCardResponse>(json);
            }
            else
            {
                generalResult.Message = json;
            }

            return generalResult;
        }

        public IClientsApi WaitForImportClient(string url, List<string> clientNames)
        {
            var clients = new List<GetClientsRespose.Datum>();

            for (var i = 0; i < 60; i++)
            {
                foreach (var name in clientNames)
                {
                    GetClientRequest(url, name)
                        .GeneralResponse
                        .data
                        .ForEach(p => clients.Add(p));
                }

                if (clients.Count < 2)
                {
                    Thread.Sleep(300);

                    continue;
                }

                break;
            }

            return this;
        }

        public GeneralResult<GetClientsRespose> GetClientByFilterRequest(string url,
            string filterName, string filterValue, string apiKey = null,
            bool checkStatusCode = true)
        {
            var generalResult = new GeneralResult<GetClientsRespose>();
            _apiKey = apiKey ?? _apiKey;
            var route = $"{url}{ApiRouteAggregate.GetClientByFilter(filterName, filterValue)}&api_key={_apiKey}";
            var response = _apiAccess.ExecuteGetEntry(route);

            var json = response
                .Content
                .ReadAsStringAsync()
                .Result;

            if (checkStatusCode)
            {
                _apiAccess.CheckStatusCode(route, response);

                generalResult.GeneralResponse = JsonConvert
                    .DeserializeObject<GetClientsRespose>(json);
            }
            else
            {
                generalResult.Message = json;
            }

            return generalResult;
        }

        public GeneralResult<GetClientsRespose> GetClientByFilterRequest(string url,
           Dictionary<string, string> filtersValues, string apiKey = null,
           bool checkStatusCode = true)
        {
            var generalResult = new GeneralResult<GetClientsRespose>();
            _apiKey = apiKey ?? _apiKey;

            var filters = _apiFactory
                .ChangeContext<ISharedStepsGenerator>()
                 .CreateUrlWithFilter(filtersValues);

            var route = $"{url}{ApiRouteAggregate.GetClientByFilter()}{filters}&api_key={_apiKey}";
            var response = _apiAccess.ExecuteGetEntry(route);

            var json = response
                .Content
                .ReadAsStringAsync()
                .Result;

            if (checkStatusCode)
            {
                _apiAccess.CheckStatusCode(route, response);

                generalResult.GeneralResponse = JsonConvert
                    .DeserializeObject<GetClientsRespose>(json);
            }
            else
            {
                generalResult.Message = json;
            }

            return generalResult;
        }

        public string DeleteClientRequest(string url, string clientId,
            string apiKey, bool checkStatusCode = true)
        {
            var route = $"{url}{ApiRouteAggregate.DeleteClient()}{clientId}?api_key={apiKey}";
            var response = _apiAccess.ExecuteDeleteEntry(route);

            var json = response
                 .Content
                 .ReadAsStringAsync()
                 .Result;

            if (checkStatusCode)
            {
                _apiAccess.CheckStatusCode(route, response);
            }

            return json;
        }

        public string MassDeleteClientsRequest(string url, string[] clientsIds,
            string apiKey, bool checkStatusCode = true)
        {
            var deleteMassAssignClients = new
            {
                ids = clientsIds
            };

            var route = $"{url}{ApiRouteAggregate.MassDeleteClients()}?api_key={apiKey}";
            var response = _apiAccess.ExecuteDeleteEntry(route, deleteMassAssignClients);

            var json = response
                .Content
                .ReadAsStringAsync()
                .Result;

            if (checkStatusCode)
            {
                _apiAccess.CheckStatusCode(route, response);
            }

            return json;
        }

        public string MassPatchAssignTradingGroupRequest(string url,
            string groupId, string[] clientsIds, string apiKey, bool checkStatusCode = true)
        {
            var route = $"{url}{ApiRouteAggregate.MassPatchSetTradingGroup()}?api_key={apiKey}";

            var patchSetTradingGroup = new
            {
                id = groupId,
                ids = clientsIds,
                platform = "cfd"
            };
            var response = _apiAccess.ExecutePatchEntry(route, patchSetTradingGroup);
            var json = response.Content.ReadAsStringAsync().Result;

            if (checkStatusCode)
            {
                _apiAccess.CheckStatusCode(route, response);
            }

            return json;
        }

        public List<ImportClientRequest> CreateClientDataForImport(int numOfClients)
        {
            var dataForImport = new List<ImportClientRequest>();

            for (var i = 0; i < numOfClients; i++)
            {
                var importClientRequest = new ImportClientRequest();
                var clientName = TextManipulation.RandomString();

                importClientRequest.FirstName = clientName;
                importClientRequest.LastName = clientName;
                importClientRequest.EMail = clientName + DataRep.EmailPrefix;
                importClientRequest.Currency = DataRep.DefaultUSDCurrencyName;
                importClientRequest.CountryIsoCodeId = "IT";
                importClientRequest.PhoneNumber = "5465464";

                dataForImport.Add(importClientRequest);
            }

            return dataForImport;
        }

        public IClientsApi PostImportLeadsRequest(string url, string fileName, string filePath,
            string campaignId, string apikey = null)
        {
            _apiKey = apikey ?? _apiKey;
            var route = ApiRouteAggregate.PostImportLead();
            route = $"{url}{route}?api_key={_apiKey}";

            var documentFileContent = _apiFactory
                .ChangeContext<IFileHandler>(_driver)
                .ConvertToBytesArray(filePath);

            var form = new MultipartFormDataContent();
            //var stream = new StreamContent(File.Open(filePath, FileMode.Open));
            documentFileContent.Headers.ContentType = new MediaTypeHeaderValue("application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
            documentFileContent.Headers.Add("Content-Disposition", $"form-data; name=\"upload\"; filename={fileName}");
            form.Add(documentFileContent, "import", fileName);
            form.Add(new StringContent(campaignId), "\"campaign_id\"");
            var response = _apiAccess.ExecutePostEntry(route, form);
            _apiAccess.CheckStatusCode(route, response);

            return this;
        }

        public string GetDownloadKycFileRequest(string url, string kycUrl)
        {
            var route = ApiRouteAggregate.GetDownloadKycFile();
            route = $"{url}{route}{kycUrl}?download=true&api_key={_apiKey}";
            var response = _apiAccess.ExecuteGetEntry(route);
            _apiAccess.CheckStatusCode(route, response);

            return response.Content.Headers.ContentDisposition.FileName;
        }

        public T ChangeContext<T>(IWebDriver driver) where T : class
        {
            return _apiFactory.ChangeContext<T>(_driver);
        }
    }
}
