using AirSoftAutomationFramework.Objects.DTOs;
using Newtonsoft.Json;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using AirSoftAutomationFramework.Internals.DAL.ApiAccsess;
using AirSoftAutomationFramework.Internals.DAL.ApiAccsess.ApiRoutes;
using AirSoftAutomationFramework.Internals.Factorys;

namespace AirSoftAutomationFramework.Objects.ForexObjects.ApiGeneral
{
    public class NotificationsApi : INotificationsApi
    {
        #region Members     
        private IApiAccess _apiAccess;
        private readonly IApplicationFactory _apiFactory;
        private IWebDriver _driver;
        #endregion Members

        public NotificationsApi(IWebDriver driver,
            IApplicationFactory appFactory, IApiAccess apiAccess)
        {
            _driver = driver;
            _apiFactory = appFactory;
            _apiAccess = apiAccess;
        }

        public List<string> GetNotificationRequest(string url, string apiKey,
            int expectedNumOfNotification)
        {
            var types = new List<string>();
            var route = $"{url}{ApiRouteAggregate.GetNotification()}?api_key={apiKey}";
            var notifications = new List<GetNotificationResponse>();

            for (var i = 0; i < 160; i++)
            {
                var response = _apiAccess.ExecuteGetEntry(route);
                _apiAccess.CheckStatusCode(route, response);
                var json = response.Content.ReadAsStringAsync().Result;

                notifications = JsonConvert.
                    DeserializeObject<List<GetNotificationResponse>>(json);

                if (notifications.Count() < expectedNumOfNotification)
                {
                    Thread.Sleep(TimeSpan.FromSeconds(1));

                    continue;
                }

                break;
            }

            notifications.ForEach(p => types.Add(p.type));

            return types.OrderBy(i => i)
                .ToList();
        }

        public T ChangeContext<T>(IWebDriver driver) where T : class
        {
            return _apiFactory.ChangeContext<T>(_driver);
        }
    }
}
