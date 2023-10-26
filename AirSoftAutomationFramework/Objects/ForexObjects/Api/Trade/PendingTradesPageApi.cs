using AirSoftAutomationFramework.Internals.DAL.ApiAccess;
using AirSoftAutomationFramework.Internals.DAL.ApiAccess.ApiRoutes;
using AirSoftAutomationFramework.Internals.DAL.ConfigurationManagement;
using AirSoftAutomationFramework.Internals.Factory;
using AirSoftAutomationFramework.Objects.ForexObjects.ApiGeneral;
using AirSoftAutomationFramework.Objects.ForexObjects.TradingPlatform.TradeObjects.TradeApi;
using OpenQA.Selenium;

namespace AirSoftAutomationFramework.Objects.ForexObjects.Api.Trade
{
    public class PendingTradesPageApi : IPendingTradesPageApi
    {
        #region Members       
        private IApiAccess _apiAccess;
        private string _apiKey = Config.appSettings.ApiKey;
        private readonly IApplicationFactory _apiFactory;
        private IWebDriver _driver;
        #endregion Members

        public PendingTradesPageApi(IApplicationFactory appFactory,
          IWebDriver driver, IApiAccess apiAccess)
        {
            _apiFactory = appFactory;
            _driver = driver;
            _apiAccess = apiAccess;
        }

        public string GetPendingTradesRequest(string url, string apiKey, bool checkStatusCode = true)
        {
            var route = ApiRouteAggregate.GetTradesByStatus("pending");
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

        public IPendingTradesPageApi ExportPendingTradeTablePipe(string url,
            string userEmail, string apiKey = null)
        {
            var tableName = "pending";
            _apiKey = apiKey ?? _apiKey;
            var route = $"{url}{ApiRouteAggregate.PostExportTableWithLink()}?api_key={_apiKey}";
            var routeForExport = ApiRouteAggregate.PostExportTradesTables(tableName);

            var columns = _apiFactory
                .ChangeContext<ISharedStepsGenerator>(_driver)
                .GetTradeTableColumns(url, tableName);

            var exportParams = _apiFactory
                .ChangeContext<ISharedStepsGenerator>()
                .CreateExportParamsString(columns, "pending_trades");

            var exportDepositsTableDto = new
            {
                export_email = userEmail,
                export_params = exportParams,
                export_table_name = "pending_trades",
                service_url = routeForExport
            };
            var response = _apiAccess.ExecutePostEntry(route, exportDepositsTableDto);
            _apiAccess.CheckStatusCode(route, response);

            return this;
        }

        public T ChangeContext<T>(IWebDriver driver) where T : class
        {
            return _apiFactory.ChangeContext<T>(_driver);
        }
    }
}
