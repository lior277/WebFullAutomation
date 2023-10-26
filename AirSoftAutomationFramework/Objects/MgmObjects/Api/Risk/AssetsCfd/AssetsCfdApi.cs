// Ignore Spelling: Cfd Api

using AirSoftAutomationFramework.Internals.DAL.ApiAccess;
using AirSoftAutomationFramework.Internals.DAL.ApiAccess.ApiRoutes;
using AirSoftAutomationFramework.Internals.DAL.ConfigurationManagement;
using AirSoftAutomationFramework.Internals.Helpers;
using Newtonsoft.Json;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using AirSoftAutomationFramework.Objects.DTOs;
using static AirSoftAutomationFramework.Objects.DTOs.GetCfdResponse;
using AirSoftAutomationFramework.Internals.Factory;

namespace AirSoftAutomationFramework.Objects.MgmObjects.Api.Risk.AssetsCfd
{
    public class AssetsCfdApi : IAssetsCfdApi
    {
        #region Members       
        private readonly IApplicationFactory _apiFactory;
        private IWebDriver _driver;
        private IApiAccess _apiAccess;
        private string _crmUrl = Config.appSettings.CrmUrl;
        private string _apiKey = Config.appSettings.ApiKey;
        #endregion Members

        public AssetsCfdApi(IApplicationFactory apiFactory, IWebDriver driver, IApiAccess apiAccess)
        {
            _apiFactory = apiFactory;
            _driver = driver;
            _apiAccess = apiAccess;
        }

        public List<AssetData> GetCfdRequest(string url,
            GetLoginResponse loginData)
        {
            var route = ApiRouteAggregate.GetCfd();
            route = url + route;
            var response = _apiAccess.ExecuteGetEntry(route, loginData);
            _apiAccess.CheckStatusCode(route, response);
            var json = response.Content.ReadAsStringAsync().Result;

            return JsonConvert.DeserializeObject<List<AssetData>>(json);
        }

        public IAssetsCfdApi PatchCfdRequest(
            GetLoginResponse loginData, AssetData cfdAssetData)
        {
            var route = ApiRouteAggregate.GetCfd();
            route = $"{DataRep.MgmUrl}{route}";

            var asset = new Asset
            {
                _id = cfdAssetData._id,
                label = cfdAssetData.label,
                category = cfdAssetData.category,
                sub_category = cfdAssetData.cfd.sub_category,
                show_in_front = cfdAssetData.cfd.show_in_front,
                order = cfdAssetData.cfd.order
            };

            var cfdRequest = new PatchCfdRequest
            {
                assets = new List<Asset> { asset }
            };

            var response = _apiAccess.ExecutePatchEntry(route, cfdRequest, loginData);
            _apiAccess.CheckStatusCode(route, response);

            return this;
        }

        public IAssetsCfdApi PatchMgmFrontAssetsBrandsDeployCfdRequest(string url, string brandId,
            GetLoginResponse loginData)
        {
            var route = ApiRouteAggregate.PatchMgmFrontAssetsBrandsDeployCfd();
            route = url + route;

            var MgmFrontAssetsBrandsDeployCfd = new
            {
                ids = new string[] { brandId },
            };

            var response = _apiAccess.ExecutePatchEntry(route,
                MgmFrontAssetsBrandsDeployCfd, loginData);

            _apiAccess.CheckStatusCode(route, response);

            return this;
        }

        public T ChangeContext<T>(IWebDriver driver) where T : class
        {
            return _apiFactory.ChangeContext<T>(_driver);
        }

    }
}
