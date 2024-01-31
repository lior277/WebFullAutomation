using AirSoftAutomationFramework.Internals.DAL.ConfigurationManagement;
using AirSoftAutomationFramework.Internals.DAL.MongoDb;
using AirSoftAutomationFramework.Objects.DTOs;
using DocumentFormat.OpenXml.Drawing;
using MongoDB.Driver.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using OpenQA.Selenium;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using AirSoftAutomationFramework.Internals.DAL.ApiAccsess;
using AirSoftAutomationFramework.Internals.DAL.ApiAccsess.ApiRoutes;
using AirSoftAutomationFramework.Internals.Factorys;

namespace AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.Settings
{
    public class SecurityTubApi : ISecurityTubApi
    {
        #region Members       
        private readonly IApplicationFactory _apiFactory;
        private IApiAccess _apiAccess;
        private IWebDriver _driver = null;
        private IMongoDbAccess _mongoDbAccess;
        private string _apiKey = Config.appSettings.ApiKey;
        #endregion Members

        public SecurityTubApi(IApplicationFactory apiFactory,
            IApiAccess apiAccess, IMongoDbAccess mongoDbAccess)
        {
            _mongoDbAccess = mongoDbAccess;
            _apiFactory = apiFactory;
            _apiAccess = apiAccess;
        }

        public GetLoginSectionInSecurity GetLoginSectionRequest(string url)
        {
            var route = $"{url}{ApiRouteAggregate.GetLoginAttempts()}?api_key={_apiKey}";
            var response = _apiAccess.ExecuteGetEntry(route);
            _apiAccess.CheckStatusCode(route, response);
            var json = response.Content.ReadAsStringAsync().Result;

            return JsonConvert.DeserializeObject<GetLoginSectionInSecurity>(json);
        }

        public SecurityTubApi PutRecaptchaRequest(string url)
        {
            var route = $"{url}{ApiRouteAggregate.PutRecaptcha()}?api_key={_apiKey}";

            var putRecaptchaDto = new
            {
                recaptcha_enabled = true,
                recaptcha_site_key = "recaptcha_site_key",
            };
            var response = _apiAccess.ExecutePutEntry(route, putRecaptchaDto);
            _apiAccess.CheckStatusCode(route, response);

            return this;
        }

        public List<GetBlockUsersResponse> GetBlockUsersRequest(string url)
        {
            var route = $"{url}{ApiRouteAggregate.GetBlockUsers()}?api_key={_apiKey}";
            var response = _apiAccess.ExecuteGetEntry(route);
            _apiAccess.CheckStatusCode(route, response);
            var json = response.Content.ReadAsStringAsync().Result;

            return JsonConvert.DeserializeObject<List<GetBlockUsersResponse>>(json);
        }

        public ISecurityTubApi WaitForUserToGetBlocked(string url, string userId)
        {
            for (var i = 0; i < 10; i++)
            {
                var blockedUser = GetBlockUsersRequest(url)
                       .Where(p => p._id == userId)
                       .FirstOrDefault();

                if (blockedUser == null)
                {
                    Thread.Sleep(300);

                    continue;
                }

                break;                
            }

            return this;
        }

        public List<GetBlockedIpsResponse> GetBlockIpsRequest(string url)
        {
            var route = $"{url}{ApiRouteAggregate.GetBlockIps()}?api_key={_apiKey}";
            var response = _apiAccess.ExecuteGetEntry(route);
            _apiAccess.CheckStatusCode(route, response);
            var json = response.Content.ReadAsStringAsync().Result;

            return JsonConvert.DeserializeObject<List<GetBlockedIpsResponse>>(json);
        }

        public ISecurityTubApi PutLoginSectionRequest(string url,
            GetLoginSectionInSecurity getLoginSectionInSecurity)
        {
            var route = $"{url}{ApiRouteAggregate.GetLoginAttempts()}?api_key={_apiKey}";
            var json = JsonConvert.SerializeObject(getLoginSectionInSecurity);
            var dto = JObject.Parse(json);
            dto.Property("_id").Remove();
            var response = _apiAccess.ExecutePutEntry(route, dto);
            json = response.Content.ReadAsStringAsync().Result;
            _apiAccess.CheckStatusCode(route, response);

            return this;
        }

        public ISecurityTubApi PatchReleaseBlockUserRequest(string url,
            string userId, string apiKey = null)
        {
            _apiKey = apiKey ?? _apiKey;
            var route = $"{url}{ApiRouteAggregate.ReleaseBlockUser()}{userId}?api_key={_apiKey}";
            var response = _apiAccess.ExecutePatchEntry(route);
            _apiAccess.CheckStatusCode(route, response);

            return this;
        }

        public ISecurityTubApi DeleteReleaseBlockIpUserRequest(string url, string userId)
        {
            var route = $"{url}{ApiRouteAggregate.ReleaseIpBlockUser()}{userId}?api_key={_apiKey}";
            var response = _apiAccess.ExecuteDeleteEntry(route);
            _apiAccess.CheckStatusCode(route, response);

            return this;
        }

        public T ChangeContext<T>(IWebDriver driver = null) where T : class
        {
            return _apiFactory.ChangeContext<T>(_driver);
        }
    }
}
