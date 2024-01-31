using System.Collections.Generic;
using System.Linq;
using System.Text;
using AirSoftAutomationFramework.Internals.DAL.ApiAccsess;
using AirSoftAutomationFramework.Internals.DAL.ApiAccsess.ApiRoutes;
using AirSoftAutomationFramework.Internals.DAL.ConfigurationManagement;
using AirSoftAutomationFramework.Internals.ExtensionsMethods;
using AirSoftAutomationFramework.Internals.Factorys;
using AirSoftAutomationFramework.Objects.DTOs;
using Newtonsoft.Json;
using OpenQA.Selenium;

namespace AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.Settings
{
    public class ChatTabApi : IChatTabApi
    {
        #region Members  
        private string _apiKey = Config.appSettings.ApiKey;
        private readonly IApplicationFactory _apiFactory;
        private IApiAccess _apiAccess;
        private IWebDriver _driver;
        #endregion Members

        public ChatTabApi(IApplicationFactory apiFactory, IWebDriver driver, IApiAccess apiAccess)
        {
            _apiFactory = apiFactory;
            _driver = driver;
            _apiAccess = apiAccess;
        }

        public string PatchEnableChatForUserRequest(string url,
           string userId, string apiKey = null, bool checkStatusCode = true)
        {
            _apiKey = apiKey ?? _apiKey;
            var route = ApiRouteAggregate.PatchEnableChatForUser();
            route = $"{url}{route}/{userId}?api_key={_apiKey}";

            var enableChatDto = new
            {
                chat_enabled = true,
            };
            var response = _apiAccess.ExecutePatchEntry(route, enableChatDto);
            var json = response.Content.ReadAsStringAsync().Result;

            if (checkStatusCode)
            {
                _apiAccess.CheckStatusCode(route, response);
            }

            return json;
        }

        public string PutChatMessageRequest(string url,
            GetChatMessagesResponse getChatMessagesResponse,
            string apiKey = null, bool checkStatusCode = true)
        {
            _apiKey = apiKey ?? _apiKey;
            var route = ApiRouteAggregate.GetChatMessages();
            route = $"{url}{route}/{getChatMessagesResponse._id}?api_key={_apiKey}";

            var chat = new
            {
                active = true,
                content = getChatMessagesResponse.content,
                language = "en",
                type = getChatMessagesResponse.type
            };

            var response = _apiAccess.ExecutePutEntry(route, chat);
            var json = response.Content.ReadAsStringAsync().Result;

            if (checkStatusCode)
            {
                _apiAccess.CheckStatusCode(route, response);
            }

            return json;
        }

        public string DeleteChatMessageRequest(string url, string chatMessagesId,
            string apiKey = null, bool checkStatusCode = true)
        {
            _apiKey = apiKey ?? _apiKey;
            var route = ApiRouteAggregate.GetChatMessages();
            route = $"{url}{route}/{chatMessagesId}?api_key={_apiKey}";
            var response = _apiAccess.ExecuteDeleteEntry(route);
            var json = response.Content.ReadAsStringAsync().Result;

            if (checkStatusCode)
            {
                _apiAccess.CheckStatusCode(route, response);
            }

            return json;
        }

        public IChatTabApi PostCreateChatMessageRequest(string url,
            string chatType, bool active = true, string apiKey = null)
        {
            _apiKey = apiKey ?? _apiKey;
            var route = $"{url}{ApiRouteAggregate.GetChatMessages()}?api_key={_apiKey}";
            string base64BannerBody = null;

            var chatBodyContent = new List<string>() { "COMPANY_NAME", "CLIENT_NAME",
                "CLIENT_FIRST_NAME", "CLIENT_LAST_NAME", "SELLER_FIRST_NAME",
                "SELLER_LAST_NAME", "SELLER_EMAIL", "SELLER_PHONE", "SELLER_PHONE_EXTENSION" };

            var builder = new StringBuilder();

            foreach (var item in chatBodyContent)
            {
                var value = "{" + item + "}";
                builder.Append(value);
            }

            base64BannerBody = builder
                .ToString()
                .TrimEnd(',')
                .EncodeBase64();

            var chat = new
            {
                active = true,
                content = base64BannerBody,
                language = "en",
                type = chatType
            };

            var response = _apiAccess.ExecutePostEntry(route, chat);
            _apiAccess.CheckStatusCode(route, response);

            return this;
        }

        public IChatTabApi PostCreateChatMessageRequestPipe(string url,
            string chatType, string apiKey = null)
        {
            var chatId = GetChatMessagesRequest(url)
                .Where(p => p.type == chatType)?
                .FirstOrDefault()?
                ._id;

            if (chatId != null)
            {
                DeleteChatMessageRequest(url, chatId);
            }

            PostCreateChatMessageRequest(url, chatType, apiKey: apiKey);

            return this;
        }

        public List<GetChatMessagesResponse> GetChatMessagesRequest(string url,
        string apiKey = null)
        {
            _apiKey = apiKey ?? _apiKey;
            var route = ApiRouteAggregate.GetChatMessages();
            route = $"{url}{route}?api_key={_apiKey}";
            var response = _apiAccess.ExecuteGetEntry(route);
            _apiAccess.CheckStatusCode(route, response);
            var json = response.Content.ReadAsStringAsync().Result;

            return JsonConvert.DeserializeObject<List<GetChatMessagesResponse>>(json);
        }

        public T ChangeContext<T>(IWebDriver driver) where T : class
        {
            return _apiFactory.ChangeContext<T>(_driver);
        }
    }
}
