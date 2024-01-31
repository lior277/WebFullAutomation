// Ignore Spelling: Api Crm Forex

using System.Collections.Generic;
using AirSoftAutomationFramework.Internals.DAL.ApiAccsess;
using AirSoftAutomationFramework.Internals.DAL.ApiAccsess.ApiRoutes;
using AirSoftAutomationFramework.Internals.DAL.ConfigurationManagement;
using AirSoftAutomationFramework.Internals.Factorys;
using AirSoftAutomationFramework.Objects.DTOs;
using Newtonsoft.Json;

namespace AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.CampaignsApi.Dashboard
{
    public class AffiliatesRequestsApi : IAffiliatesRequestsApi
    {
        #region Members
        private IApiAccess _apiAccess;
        private string _apiKey = Config.appSettings.ApiKey;
        private readonly IApplicationFactory _apiFactory;
        #endregion Members

        public AffiliatesRequestsApi(IApplicationFactory apiFactory, IApiAccess apiAccess)
        {
            _apiFactory = apiFactory;
            _apiAccess = apiAccess;
        }

        public List<GetClientRegistrationResponse>
            GetClientRegistrationRequest(string uri, string campaignId, string apiKey = null)
        {
            _apiKey = apiKey ?? _apiKey;

            var route = $"{uri}{ApiRouteAggregate.GetClientRegistration()}" +
                $"?api_key={_apiKey}&campaign_id={campaignId}";

            var response = _apiAccess.ExecuteGetEntry(route);
            _apiAccess.CheckStatusCode(route, response);
            var json = response.Content.ReadAsStringAsync().Result;

            return JsonConvert.DeserializeObject<List<GetClientRegistrationResponse>>(json);
        }
    }
}
