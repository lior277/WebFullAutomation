using System;
using System.Collections.Generic;
using AirSoftAutomationFramework.Internals.DAL.ApiAccess;
using AirSoftAutomationFramework.Internals.DAL.ApiAccess.ApiRoutes;
using AirSoftAutomationFramework.Internals.DAL.ConfigurationManagement;
using AirSoftAutomationFramework.Internals.Factory;
using AirSoftAutomationFramework.Objects.ForexObjects.ApiGeneral;
using AirSoftAutomationFramework.Objects.ForexObjects.TradingPlatform.TradeObjects.TradeApi;
using OpenQA.Selenium;

namespace AirSoftAutomationFramework.Objects.ForexObjects.Api.Trade
{
    public class OpenTradesPageApi : IOpenTradesPageApi
    {
        #region Members       
        private IApiAccess _apiAccess;
        private string _apiKey = Config.appSettings.ApiKey;
        private readonly IApplicationFactory _apiFactory;
        private IWebDriver _driver;
        #endregion Members

        public OpenTradesPageApi(IApplicationFactory appFactory,
          IWebDriver driver, IApiAccess apiAccess)
        {
            _apiFactory = appFactory;
            _driver = driver;
            _apiAccess = apiAccess;
        }

        public IOpenTradesPageApi ExportOpenTradeTablePipe(string url,
            string userEmail, string apiKey = null)
        {
            _apiKey = apiKey ?? _apiKey;
            var route = $"{url}{ApiRouteAggregate.PostExportTableWithLink()}?api_key={_apiKey}";
            var routeForExport = ApiRouteAggregate.PostExportTradesTables("open");
            var today = DateTime.Now.ToString("yyyy-MM-dd");

            var columns = _apiFactory
                .ChangeContext<ISharedStepsGenerator>(_driver)
                .GetTradeTableColumns(url, "open");

            var exportParams = _apiFactory
                .ChangeContext<ISharedStepsGenerator>()
                .CreateExportParamsString(columns, "open_trades");

            var exportOpenTradesTableDto = new
            {
                export_email = userEmail,
                export_params = exportParams,
                export_table_name = "open_trades",
                service_url = routeForExport
            };

            var response = _apiAccess.ExecutePostEntry(route, exportOpenTradesTableDto);
            _apiAccess.CheckStatusCode(route, response);

            return this;
        }

        public string GetOpenTradesRequest(string url, string apiKey,
            bool checkStatusCode = true)
        {
            var route = ApiRouteAggregate.GetTradesByStatus("open");
            route = $"{url}{route}?api_key={apiKey}";
            var response = _apiAccess.ExecuteGetEntry(route);

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

        public IOpenTradesPageApi PatchCloseTradeRequest(string url, string tradeId,
            string apiKey = null)
        {
            _apiKey = apiKey ?? _apiKey;
            var route = ApiRouteAggregate.PatchCloseTrade(tradeId);
            route = $"{url}{route}?api_key={_apiKey}";
            var response = _apiAccess.ExecutePatchEntry(route);
            _apiAccess.CheckStatusCode(route, response);

            return this;
        }

        public IOpenTradesPageApi PatchCloseTradeRequest(string url, List<string> tradeIds,
           string apiKey = null)
        {
            foreach (var id in tradeIds)
            {
                PatchCloseTradeRequest(url, id, apiKey);
            }

            return this;
        }

        public T ChangeContext<T>(IWebDriver driver) where T : class
        {
            return _apiFactory.ChangeContext<T>(_driver);
        }
    }
}
