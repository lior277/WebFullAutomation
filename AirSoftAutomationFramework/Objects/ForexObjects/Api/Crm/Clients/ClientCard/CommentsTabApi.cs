using System;
using System.Collections.Generic;
using AirSoftAutomationFramework.Internals.DAL.ApiAccess;
using AirSoftAutomationFramework.Internals.DAL.ApiAccess.ApiRoutes;
using AirSoftAutomationFramework.Internals.DAL.ConfigurationManagement;
using AirSoftAutomationFramework.Internals.Factory;
using AirSoftAutomationFramework.Objects.DTOs;
using Newtonsoft.Json;
using OpenQA.Selenium;

namespace AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.Clients.ClientCard
{
    public class CommentsTabApi : ICommentsTabApi
    {
        #region Members     
        private IApiAccess _apiAccess;
        private string _apiKey = Config.appSettings.ApiKey;
        private readonly IApplicationFactory _apiFactory;
        private IWebDriver _driver;
        #endregion Members

        public CommentsTabApi(IWebDriver driver,
            IApplicationFactory appFactory, IApiAccess apiAccess)
        {
            _driver = driver;
            _apiFactory = appFactory;
            _apiAccess = apiAccess;
        }

        public GeneralResult<List<GetCommentResponse>> GetCommentsByClientIdRequest(string uri,
            string clientId, string apiKey = null, bool checkStatusCode = true)
        {
            _apiKey = apiKey ?? _apiKey;
            var generalResult = new GeneralResult<List<GetCommentResponse>>();
            var route = $"{uri}{ApiRouteAggregate.GetCommentsByUserId(clientId)}?api_key={_apiKey}";
            var response = _apiAccess.ExecuteGetEntry(route);
            var json = response.Content.ReadAsStringAsync().Result;

            if (checkStatusCode)
            {
                _apiAccess.CheckStatusCode(route, response);

                generalResult.GeneralResponse = JsonConvert
                    .DeserializeObject<List<GetCommentResponse>>(json);
            }
            else
            {
                generalResult.Message = json;
            }

            return generalResult;
        }

        public List<GetCommentResponse> GetDeletedCommentsByClientIdRequest(string uri,
            string clientId, string apiKey)
        {
            _apiKey = apiKey ?? _apiKey;
            var route = $"{uri}{ApiRouteAggregate.GetCommentsByUserId(clientId)}" +
                $"?includeDeleted=true&api_key={_apiKey}";

            var response = _apiAccess.ExecuteGetEntry(route);
            _apiAccess.CheckStatusCode(route, response);
            var json = response.Content.ReadAsStringAsync().Result;

            return JsonConvert.DeserializeObject<List<GetCommentResponse>>(json);
        }

        public ICommentsTabApi DeleteCommentRequest(string uri, string commentId,
            string apiKey = null)
        {
            _apiKey = apiKey ?? _apiKey;
            var route = $"{uri}{ApiRouteAggregate.DeleteComment()}{commentId}?api_key={_apiKey}";
            _apiAccess.ExecuteDeleteEntry(route);

            return this;
        }

        public string DeleteCommentRequest(string uri, string commentId,
            string apiKey, bool checkStatusCode = true)
        {
            _apiKey = apiKey ?? _apiKey;
            var route = $"{uri}{ApiRouteAggregate.DeleteComment()}{commentId}?api_key={apiKey}";
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

        public string PatchEditCommentRequest(string uri, string commentId,
            string comment, string apiKey = null, bool checkStatusCode = true)
        {
            _apiKey = apiKey ?? _apiKey;
            var route = $"{uri}{ApiRouteAggregate.PostComment()}/{commentId}?api_key={_apiKey}";

            var editCommentDto = new
            {
                new_comment = $"{comment} new comment",
                old_comment = comment
            };
            var response = _apiAccess.ExecutePatchEntry(route, editCommentDto);
            var json = response.Content.ReadAsStringAsync().Result;

            if (checkStatusCode)
            {
                _apiAccess.CheckStatusCode(route, response);
            }

            return json;
        }

        public GeneralResult<List<CommentResponse>> PostCommentRequest(string uri, string clientId,
            string actualComment = "automation comment", string apiKey = null, bool checkStatusCode = true)
        {
            var generalResult = new GeneralResult<List<CommentResponse>>();
            string json;
            _apiKey = apiKey ?? _apiKey;
            var route = $"{uri}{ApiRouteAggregate.PostComment()}?api_key={_apiKey}";

            var postCommentDto = new
            {
                user_id = clientId,
                date = DateTime.Now.ToString("MM/dd/yyyy HH:mm"),
                comment = actualComment,
            };

            var response = _apiAccess.ExecutePostEntry(route, postCommentDto);
            json = response.Content.ReadAsStringAsync().Result;

            if (checkStatusCode)
            {
                _apiAccess.CheckStatusCode(route, response);
                generalResult.GeneralResponse = JsonConvert
                    .DeserializeObject<List<CommentResponse>>(json);
            }
            else
            {
                generalResult.Message = json;
            }

            return generalResult;
        }

        public T ChangeContext<T>(IWebDriver driver) where T : class
        {
            return _apiFactory.ChangeContext<T>(_driver);
        }
    }
}
