// Ignore Spelling: api Kyc Crm Forex

using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using AirSoftAutomationFramework.Internals.DAL.ApiAccsess;
using AirSoftAutomationFramework.Internals.DAL.ApiAccsess.ApiRoutes;
using AirSoftAutomationFramework.Internals.DAL.ConfigurationManagement;
using AirSoftAutomationFramework.Internals.ExtensionsMethods;
using AirSoftAutomationFramework.Internals.Factorys;
using AirSoftAutomationFramework.Objects.DTOs;
using Newtonsoft.Json;
using OpenQA.Selenium;
using static AirSoftAutomationFramework.Objects.DTOs.GetInformationTabResponse;

namespace AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.Clients.ClientCard
{
    public class InformationTabApi : IInformationTabApi
    {
        #region Members
        private IApiAccess _apiAccess;
        private string _apiKey = Config.appSettings.ApiKey;
        private readonly IApplicationFactory _apiFactory;
        #endregion Members

        public InformationTabApi(IApplicationFactory appFactory, IApiAccess apiAccess)
        {
            _apiFactory = appFactory;
            _apiAccess = apiAccess;
        }

        public InformationTabApi DeleteKycFileRequest(string url, string clientId,
            string kycFileId, string apiKey = null)
        {
            _apiKey = apiKey ?? _apiKey;
            var route = $"{url}{ApiRouteAggregate.DeleteKycFile(clientId, kycFileId)}?api_key={_apiKey}";
            var response = _apiAccess.ExecuteDeleteEntry(route);
            _apiAccess.CheckStatusCode(route, response);

            return this;
        }

        public GeneralResult<GetInformationTabResponse> PutInformationTabRequest(string url,
            InformationTab informationTab,
            string apiKey = null, bool checkStatusCode = true)
        {
            var generalResult = new GeneralResult<GetInformationTabResponse>();

            var propertiesToRemove = new string[] { "_id", "currency_code", "created_at",
                "last_login", "last_logout", "active", "attribution_date",
                "balance", "has_deposit", "available", "bonus",
                "demo_balance", "equity", "open_pnl", "min_margin",
                "margin_usage", "pnl", "pnl_real", "pnl_bonus", "online" };

            _apiKey = apiKey ?? _apiKey;

            informationTab.mass_trade ??= "1";

            var route = url + ApiRouteAggregate.PutClientCard(informationTab._id) +
                $"?api_key={_apiKey}";

            var multipartFormDataContent = new MultipartFormDataContent();
            var listKeyValuePair = new List<KeyValuePair<string, string>>();

            var jObjectProperties = JsonConvert.SerializeObject(informationTab)
                .ConvertToJObject()
                .RemovePropertiesFromObject(propertiesToRemove);

            foreach (var item in jObjectProperties)
            {
                listKeyValuePair.Add(new KeyValuePair<string, string>
                    (item.Name, item.Value.ToString()));
            }

            foreach (var item in listKeyValuePair)
            {
                multipartFormDataContent.Add(new StringContent(item.Value), item.Key);
            }

            var response = _apiAccess.ExecutePutEntry(route, multipartFormDataContent);
            var json = response.Content.ReadAsStringAsync().Result;

            if (checkStatusCode)
            {
                _apiAccess.CheckStatusCode(route, response);

                generalResult.GeneralResponse = JsonConvert
                    .DeserializeObject<GetInformationTabResponse>(json);
            }
            else
            {
                generalResult.Message = json;
            }

            return generalResult;
        }

        public GeneralResult<GetInformationTabResponse> GetInformationTabRequest(string url,
            string clientId, string apiKey = null, bool checkStatusCode = true)
        {
            _apiKey = apiKey ?? _apiKey;
            var generalResult = new GeneralResult<GetInformationTabResponse>();
            var route = $"{url}{ApiRouteAggregate.GetClientById()}{clientId}?api_key={_apiKey}";
            var response = _apiAccess.ExecuteGetEntry(route);
            var json = response.Content.ReadAsStringAsync().Result;

            if (checkStatusCode)
            {
                _apiAccess.CheckStatusCode(route, response);

                generalResult.GeneralResponse = JsonConvert
                    .DeserializeObject<GetInformationTabResponse>(json);
            }
            else
            {
                generalResult.Message = json;
            }

            return generalResult;
        }

        public HttpResponseMessage PatchSetTradingGroupRequest(string url,
            string groupId, List<string> clientsIds, string apiKey = null)
        {
            _apiKey = apiKey ?? _apiKey;
            var route = $"{url}{ApiRouteAggregate.MassPatchSetTradingGroup()}?api_key={_apiKey}";
            //var hhgroupId = Regex.Replace(groupId, @"\\", "");
            var PatchSetTradingGroup = new
            {
                id = groupId,
                ids = clientsIds,
                platform = "cfd"
            };
            var response = _apiAccess.ExecutePatchEntry(route, PatchSetTradingGroup);
            _apiAccess.CheckStatusCode(route, response);

            return response;
        }

        public IInformationTabApi PatchCilentStatusRequest(string url, string clientId,
           bool clientStatus, string apiKey = null)
        {
            _apiKey = apiKey ?? _apiKey;
            var route = $"{url}{ApiRouteAggregate.PatchClientStatus(clientId)}?api_key={_apiKey}";

            var patchCilentStatus = new
            {
                status = clientStatus
            };
            var response = _apiAccess.ExecutePatchEntry(route, patchCilentStatus);
            _apiAccess.CheckStatusCode(route, response);

            return this;
        }

        public T ChangeContext<T>(IWebDriver driver) where T : class
        {
            Debug.Assert(_apiFactory != null, nameof(_apiFactory) + " != null");
            return _apiFactory.ChangeContext<T>(driver);
        }
    }
}
