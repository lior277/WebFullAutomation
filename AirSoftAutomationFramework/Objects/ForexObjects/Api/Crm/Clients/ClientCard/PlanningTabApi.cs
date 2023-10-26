using System;
using System.Collections.Generic;
using AirSoftAutomationFramework.Internals.DAL.ApiAccess;
using AirSoftAutomationFramework.Internals.DAL.ApiAccess.ApiRoutes;
using AirSoftAutomationFramework.Internals.DAL.ConfigurationManagement;
using OpenQA.Selenium;
using System.Linq;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.AccountsApi.UsersMenuApi;
using AirSoftAutomationFramework.Internals.Factory;

namespace AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.Clients.ClientCard
{
    public class PlanningTabApi : IPlanningTabApi
    {
        #region Members     
        private IApiAccess _apiAccess;
        private string _apiKey = Config.appSettings.ApiKey;
        private readonly IApplicationFactory _apiFactory;
        private IWebDriver _driver;
        #endregion Members

        public PlanningTabApi(IWebDriver driver, IApplicationFactory appFactory, IApiAccess apiAccess)
        {
            _driver = driver;
            _apiFactory = appFactory;
            _apiAccess = apiAccess;
        }

        public string PostCreateAddCommentRequest(string url,
            string clientId, string apiKey = null, bool checkStatusCode = true)
        {
            _apiKey = apiKey ?? _apiKey;

            var timezone = _apiFactory
                .ChangeContext<IUsersApi>(_driver)
                .GetUserByIdRequest(url, clientId)
                .GeneralResponse
                .user
                .gmt_timezone;

            string json;

            timezone = timezone.Split(':').First();
            var hours = Convert.ToInt32(timezone);

            var plannedDate = DateTime.UtcNow.AddHours(hours)
                .AddMinutes(11).ToString("MM/dd/yyyy HH:mm");//11

            var route = $"{url}{ApiRouteAggregate.PostCreateComment()}?api_key={_apiKey}";

            var createCommentDto = new
            {
                user_id = clientId,
                date = plannedDate,
                comment = "Planning"
            };
            var response = _apiAccess.ExecutePostEntry(route, createCommentDto);
            json = response.Content.ReadAsStringAsync().Result;

            if (checkStatusCode)
            {
                _apiAccess.CheckStatusCode(route, response);
            }

            return json;
        }

        public string PostAddMassAssignCommentRequest(string url, List<string> clientsIds,
            string expectedComment, string apiKey = null)
        {
            _apiKey = apiKey ?? _apiKey;
            var route = $"{url}{ApiRouteAggregate.PostCreateMassAssignComment()}?api_key={_apiKey}";

            var ConnectCampaignToClientDto = new
            {
                comment = expectedComment,
                ids = clientsIds,
            };
            var response = _apiAccess.ExecutePostEntry(route, ConnectCampaignToClientDto);
            _apiAccess.CheckStatusCode(route, response);

            return response
               .Content
               .ReadAsStringAsync()
               .Result;
        }

        public T ChangeContext<T>(IWebDriver driver) where T : class
        {
            return _apiFactory.ChangeContext<T>(_driver);
        }
    }
}
