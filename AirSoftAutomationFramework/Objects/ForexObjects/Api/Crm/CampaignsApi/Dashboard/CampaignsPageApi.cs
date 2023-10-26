using AirSoftAutomationFramework.Internals.DAL.ApiAccess;
using AirSoftAutomationFramework.Internals.DAL.ApiAccess.ApiRoutes;
using AirSoftAutomationFramework.Internals.DAL.ConfigurationManagement;
using AirSoftAutomationFramework.Internals.Factory;
using AirSoftAutomationFramework.Objects.DTOs;
using Newtonsoft.Json;
using OpenQA.Selenium;
using System.Collections.Generic;

namespace AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.CampaignsApi.Dashboard
{
    public class CampaignsPageApi : ICampaignsPageApi
    {
        #region Members
        private IWebDriver _driver;
        private IApiAccess _apiAccess;
        private string _apiKey = Config.appSettings.ApiKey;
        private readonly IApplicationFactory _apiFactory;
        #endregion Members

        public CampaignsPageApi(IApplicationFactory apiFactory,
            IWebDriver driver, IApiAccess apiAccess)
        {
            _apiFactory = apiFactory;
            _driver = driver;
            _apiAccess = apiAccess;
        }

        public GeneralResult<GetCampaignByIdResponse> GetCampaignByIdRequest(string url,
            string campaignId, string apiKey = null, bool checkStatusCode = true)
        {
            _apiKey = apiKey ?? _apiKey;
            var route = $"{url}{ApiRouteAggregate.PostCreateCampaign()}/{campaignId}?api_key={_apiKey}";
            var generalResult = new GeneralResult<GetCampaignByIdResponse>();
            var response = _apiAccess.ExecuteGetEntry(route);

            var json = response
               .Content
               .ReadAsStringAsync()
               .Result;

            if (checkStatusCode)
            {
                _apiAccess.CheckStatusCode(route, response);

                generalResult.GeneralResponse = JsonConvert
                    .DeserializeObject<GetCampaignByIdResponse>(json);
            }
            else
            {
                generalResult.Message = json;
            }

            return generalResult;
        }

        public GeneralResult<List<GetCampaignByIdResponse>> GetCampaignsRequest(string url,
            string apiKey = null, bool checkStatusCode = true)
        {
            _apiKey = apiKey ?? _apiKey;
            var route = $"{url}{ApiRouteAggregate.GetCampaigns()}?api_key={_apiKey}";
            var generalResult = new GeneralResult<List<GetCampaignByIdResponse>>();
            var response = _apiAccess.ExecuteGetEntry(route);

            var json = response
               .Content
               .ReadAsStringAsync()
               .Result;

            if (checkStatusCode)
            {
                _apiAccess.CheckStatusCode(route, response);

                generalResult.GeneralResponse = JsonConvert
                    .DeserializeObject<List<GetCampaignByIdResponse>>(json);
            }
            else
            {
                generalResult.Message = json;
            }

            return generalResult;
        }

        public string DeleteCampaignRequest(string uri, string CampaignId,
            string apiKey, bool checkStatusCode = true)
        {
            var route = $"{uri}{ApiRouteAggregate.DeleteCampaign()}/{CampaignId}?api_key={apiKey}";
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

        public T ChangeContext<T>(IWebDriver driver) where T : class
        {
            return _apiFactory.ChangeContext<T>(_driver);
        }
    }
}
