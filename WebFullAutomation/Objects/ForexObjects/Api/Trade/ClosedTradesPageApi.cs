using AirSoftAutomationFramework.Internals.DAL.ConfigurationManagement;
using AirSoftAutomationFramework.Internals.Helpers;
using AirSoftAutomationFramework.Objects.ForexObjects.ApiGeneral;
using AirSoftAutomationFramework.Objects.ForexObjects.TradingPlatform.TradeObjects.TradeApi;
using OpenQA.Selenium;
using System;
using AirSoftAutomationFramework.Internals.DAL.ApiAccsess;
using AirSoftAutomationFramework.Internals.DAL.ApiAccsess.ApiRoutes;
using AirSoftAutomationFramework.Internals.Factorys;

namespace AirSoftAutomationFramework.Objects.ForexObjects.Api.Trade
{
    public class ClosedTradesPageApi : IClosedTradesPageApi
    {
        #region Members       
        private IApiAccess _apiAccess;
        private readonly IApplicationFactory _apiFactory;
        private string _apiKey = Config.appSettings.ApiKey;
        private IWebDriver _driver;
        #endregion Members

        public ClosedTradesPageApi(IApplicationFactory appFactory,
          IWebDriver driver, IApiAccess apiAccess)
        {
            _apiFactory = appFactory;
            _driver = driver;
            _apiAccess = apiAccess;
        }

        public string GetClosedTradesRequest(string url, string apiKey, bool checkStatusCode = true)
        {
            var route = ApiRouteAggregate.GetTradesByStatus("close");
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

        public string ExportCloseTradeTablePipe(string url,
            string userEmail, string apiKey = null, bool checkStatusCode = true)
        {
            _apiKey = apiKey ?? _apiKey;
            var route = $"{url}{ApiRouteAggregate.PostExportTableWithLink()}?api_key={_apiKey}";
            var routeForExport = ApiRouteAggregate.PostExportTradesTables("close");
            var today = DateTime.Now.ToString("yyyy-MM-dd");

            var columns = _apiFactory
                .ChangeContext<ISharedStepsGenerator>(_driver)
                .GetTradeTableColumns(url, "close");

            var exportParams = _apiFactory
                .ChangeContext<ISharedStepsGenerator>()
                .CreateExportParamsString(columns, "close_trades");

            var exportTradeTableDto = new
            {
                export_email = userEmail,
                export_params = exportParams,
                export_table_name = DataRep.CloseTradesTableName,
                service_url = routeForExport
            };

            var response = _apiAccess.ExecutePostEntry(route, exportTradeTableDto);

            if (checkStatusCode)
            {
                _apiAccess.CheckStatusCode(route, response);
            }

            return response.Content.ReadAsStringAsync().Result;
        }


        public T ChangeContext<T>(IWebDriver driver) where T : class
        {
            return _apiFactory.ChangeContext<T>(_driver);
        }
    }
}
