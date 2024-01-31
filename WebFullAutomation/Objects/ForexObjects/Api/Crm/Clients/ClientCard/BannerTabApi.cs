// Ignore Spelling: api

using AirSoftAutomationFramework.Internals.DAL.ApiAccsess;
using AirSoftAutomationFramework.Internals.DAL.ApiAccsess.ApiRoutes;
using AirSoftAutomationFramework.Internals.DAL.ConfigurationManagement;
using AirSoftAutomationFramework.Internals.Factorys;
using AirSoftAutomationFramework.Objects.DTOs;
using Newtonsoft.Json;
using OpenQA.Selenium;

namespace AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.Clients.ClientCard
{
    public class BannerTabApi : IBannerTabApi
    {
        #region Members
        private IApiAccess _apiAccess;
        private string _apiKey = Config.appSettings.ApiKey;
        private readonly IApplicationFactory _apiFactory;
        private IWebDriver _driver;
        #endregion Members

        public BannerTabApi(IApplicationFactory appFactory, IWebDriver driver, IApiAccess apiAccess)
        {
            _apiFactory = appFactory;
            _driver = driver;
            _apiAccess = apiAccess;
        }

        public string PutBannerRequest(string url, string clientId,
            string bannerId, string apiKey = null, bool checkStatusCode = true)
        {
            _apiKey = apiKey ?? _apiKey;
            var route = $"{url}{ApiRouteAggregate.PutBanners()}?api_key={_apiKey}";

            var PutBannerDto = new
            {
                feedMessageId = bannerId,
                userId = clientId
            };
            var response = _apiAccess.ExecutePutEntry(route, PutBannerDto);
            var json = response.Content.ReadAsStringAsync().Result;

            if (checkStatusCode)
            {
                _apiAccess.CheckStatusCode(route, response);
            }

            return json;
        }

        public GeneralResult<GeneralDto> GetClientBannerRequest(string url, string clientId,
            bool checkStatusCode = true)
        {
            var generalResult = new GeneralResult<GeneralDto>();
            var route = $"{url}{ApiRouteAggregate.GetClientBanner()}{clientId}?api_key={_apiKey}";
            var response = _apiAccess.ExecuteGetEntry(route);

            var json = response
              .Content
              .ReadAsStringAsync()
              .Result;

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
    }
}
