// Ignore Spelling: api

using AirSoftAutomationFramework.Internals.DAL.ConfigurationManagement;
using AirSoftAutomationFramework.Internals.Helpers;
using Newtonsoft.Json;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using AirSoftAutomationFramework.Internals.DAL.ApiAccsess;
using AirSoftAutomationFramework.Internals.DAL.ApiAccsess.ApiRoutes;
using AirSoftAutomationFramework.Internals.Factorys;
using AirSoftAutomationFramework.Objects.DTOs;

namespace AirSoftAutomationFramework.Objects.MgmObjects.Api
{
    public class AssetsApi : IAssetsApi
    {
        #region Members       
        private readonly IApplicationFactory _apiFactory;
        private IWebDriver _driver;
        private IApiAccess _apiAccess;
        private string _crmUrl = Config.appSettings.CrmUrl;
        private string _apiKey = Config.appSettings.ApiKey;
        #endregion Members

        public AssetsApi(IApplicationFactory apiFactory,
            IWebDriver driver, IApiAccess apiAccess)
        {
            _apiFactory = apiFactory;
            _driver = driver;
            _apiAccess = apiAccess;
        }

        public IAssetsApi PostCreateAssetRequest(string uri,
            GetLoginResponse loginData)
        {
            var route = ApiRouteAggregate.GetAssets();
            route = $"{uri}{route}";

            var cfd = new Cfd
            {
                include = true,
            };

            var crypto = new CreateAssetCrypto
            {
                include = false,
            };

            var chrono = new CreateAssetChrono
            {
                include = false,
            };

            var asset = new CreateAssetRequest
            {
                symbol = DataRep.AssetNameForCreateAsset,
                label = DataRep.AssetNameForCreateAsset,
                category = "forex",
                expiry_date = null,
                roll_over_date = null,
                dividend_date = null,
                description = "DESCRIPTION",
                ib_id = 1,
                ib_sec_type = "forex",
                asset_is_future = false,
                exchange = "ASX",
                currency = "AUD",
                decimal_digits = 2,
                yahoo_symbol = "YAHOO SYMBOL",
                bloomberg_key = "BLOOMBERG KEY",
                source = "IB",
                ms_symbol = "MS SYMBOL",
                ms_exchange = "MS EXCHANGE",
                ms_type = "MS TYPE",
                ka_exchange = "krkn",
                ka_type = "spot",
                ka_symbol = "KAIKO SYMBOL",
                icon_main = "ad",
                icon_secondary = null,
                cfd = cfd,
                crypto = crypto,
                chrono = chrono,
            };

            var response = _apiAccess.ExecutePostEntry(route, asset, loginData);
            _apiAccess.CheckStatusCode(route, response);

            return this;
        }

        public IAssetsApi DeleteAssetsRequest(string url, string assetId,
            GetLoginResponse loginData)
        {
            var route = ApiRouteAggregate.GetAssets();
            route = $"{url}{route}/{assetId}";
            var response = _apiAccess.ExecuteDeleteEntry(route, loginData);
            _apiAccess.CheckStatusCode(route, response);

            return this;
        }


        public List<GetAssestResponse> GetAssetsRequest(string url,
            GetLoginResponse loginData)
        {
            var route = ApiRouteAggregate.GetAssets();
            route = url + route;
            var response = _apiAccess.ExecuteGetEntry(route, loginData);
            _apiAccess.CheckStatusCode(route, response);
            var json = response.Content.ReadAsStringAsync().Result;

            return JsonConvert.DeserializeObject<List<GetAssestResponse>>(json);
        }

        public IAssetsApi CreateAssetPipe(string url,
            GetLoginResponse loginData)
        {
            var assetId = GetAssetsRequest(url, loginData)
                .Where(p => p.label == DataRep.AssetNameForCreateAsset)
                .FirstOrDefault()?
                ._id;

            if (assetId != null)
            {
                DeleteAssetsRequest(url, assetId, loginData);               
            }

            PostCreateAssetRequest(url, loginData);

            return this;
        }

        public T ChangeContext<T>(IWebDriver driver) where T : class
        {
            return _apiFactory.ChangeContext<T>(_driver);
        }

    }
}
