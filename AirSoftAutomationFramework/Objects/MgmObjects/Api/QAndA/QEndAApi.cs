// Ignore Spelling: api

using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using AirSoftAutomationFramework.Internals.DAL.ApiAccess;
using AirSoftAutomationFramework.Internals.DAL.ApiAccess.ApiRoutes;
using AirSoftAutomationFramework.Internals.DAL.ConfigurationManagement;
using AirSoftAutomationFramework.Internals.Factory;
using AirSoftAutomationFramework.Internals.Helpers;
using AirSoftAutomationFramework.Objects.DTOs;
using Newtonsoft.Json;
using OpenQA.Selenium;

namespace AirSoftAutomationFramework.Objects.MgmObjects.Api.QAndA
{
    public class QEndAApi : IQEndAApi
    {
        #region Members       
        private readonly IApplicationFactory _apiFactory;
        private IWebDriver _driver;
        private IApiAccess _apiAccess;
        private string _crmUrl = Config.appSettings.CrmUrl;
        private string _apiKey = Config.appSettings.ApiKey;
        #endregion Members

        public QEndAApi(IApplicationFactory apiFactory, IWebDriver driver, IApiAccess apiAccess)
        {
            _apiFactory = apiFactory;
            _driver = driver;
            _apiAccess = apiAccess;
        }

        public IQEndAApi PostCreateQEndARequest(string url, string answer, string question,
            GetLoginResponse loginData)
        {
            var route = ApiRouteAggregate.PostCreateQEndA();
            route = url + route;
            var actualAnsware =  answer ?? "answer";

            var qEndA = new
            {
                answer = actualAnsware,
                question = question ?? "question",
            };

           var response = _apiAccess.ExecutePostEntry(route, qEndA, loginData);
            _apiAccess.CheckStatusCode(route, response);

            return this;
        }

        public string PostCreateImageRequest(string url, string fileName, 
            GetLoginResponse loginData)
        {
            var route = ApiRouteAggregate.PostCreateQEndA();
            route = $"{url}{route}/image";

            var imageContent = _apiFactory
                .ChangeContext<IFileHandler>(_driver)
                .ConvertToBytesArray(fileName);

            var form = new MultipartFormDataContent();
            //imageContent.Headers.ContentType = MediaTypeHeaderValue.Parse("image/png");
            form.Add(imageContent, Path.GetFileName(fileName));
            var response = _apiAccess.ExecutePostEntry(route, form, loginData);
            _apiAccess.CheckStatusCode(route, response);

            return response.Content
                .ReadAsStringAsync()
                .Result;
        }

        public IQEndAApi PatchBrandDeployQEndARequest(string url, string[] brandIds,
            GetLoginResponse loginData)
        {
            var route = ApiRouteAggregate.PatchBrandDeployQEndA();
            route = url + route;

            var patchBrandDeploy = new
            {
                ids = brandIds
            };
            var response = _apiAccess.ExecutePatchEntry(route, patchBrandDeploy, loginData);
            _apiAccess.CheckStatusCode(route, response);
            
            return this;
        }

        public List<GeneralDto> GetQEndARequest(string url,
            GetLoginResponse loginData)
        {
            var route = ApiRouteAggregate.PostCreateQEndA();
            route = url + route;

            var response = _apiAccess.ExecuteGetEntry(route, loginData);
            _apiAccess.CheckStatusCode(route, response);

            var json = response
                .Content
                .ReadAsStringAsync()
                .Result;

            return JsonConvert.DeserializeObject<List<GeneralDto>>(json);
        }

        public IQEndAApi DeleteQEndARequest(string url, string QEndAId,
            GetLoginResponse loginData)
        {
            var route = ApiRouteAggregate.PostCreateQEndA();
            route = $"{url}{route}/{QEndAId}";
            var response = _apiAccess.ExecuteDeleteEntry(route);
            _apiAccess.CheckStatusCode(route, response);

            return this;
        }

        public IQEndAApi DeleteQEndARequest(string url, List<string> QEndAIds,
            GetLoginResponse loginData)
        {
            foreach (var QEndAId in QEndAIds)
            {
                DeleteQEndARequest(url, QEndAId, loginData);
            }

            return this;
        }

        public IQEndAApi DeleteQEndAPipe(string url,
            GetLoginResponse loginData)
        {
            var qAndAIds = new List<string>();

            GetQEndARequest(url, loginData)
                .ForEach(p => qAndAIds?.Add(p.Id));

            DeleteQEndARequest(url, qAndAIds, loginData);

            return this;
        }

        public T ChangeContext<T>(IWebDriver driver) where T : class
        {
            return _apiFactory.ChangeContext<T>(_driver);
        }

    }
}
