using AirSoftAutomationFramework.Internals.DAL.ConfigurationManagement;
using AirSoftAutomationFramework.Internals.ExtensionsMethods;
using AirSoftAutomationFramework.Internals.Helpers;
using AirSoftAutomationFramework.Objects.DTOs;
using AirSoftAutomationFramework.Objects.ForexObjects.ApiGeneral;
using Newtonsoft.Json;
using OpenQA.Selenium;
using System.Collections.Generic;
using System.Linq;
using AirSoftAutomationFramework.Internals.DAL.ApiAccsess;
using AirSoftAutomationFramework.Internals.DAL.ApiAccsess.ApiRoutes;
using AirSoftAutomationFramework.Internals.Factorys;

namespace AirSoftAutomationFramework.Objects.ForexObjects.Api.Trade
{
    public class RiskPageApi : IRiskPageApi
    {
        #region Members       
        private readonly IApplicationFactory _apiFactory;
        private IApiAccess _apiAccess;
        private IWebDriver _driver;
        private string _apiKey = Config.appSettings.ApiKey;
        #endregion Members

        public RiskPageApi(IApplicationFactory apiFactory, IWebDriver driver, IApiAccess apiAccess)
        {
            _apiFactory = apiFactory;
            _driver = driver;
            _apiAccess = apiAccess;
        }

        public string PostExportRisksTablePipeRequest(string url, string clientEmail,
            string userEmail, string apiKey = null, bool checkStatusCode = true)
        {
            var tableName = DataRep.RiskTableName;
            _apiKey = apiKey ?? _apiKey;
            var route = $"{url}{ApiRouteAggregate.PostExportTableWithLink()}?api_key={_apiKey}";
            var routeForExport = ApiRouteAggregate.PostExportCrmTradeTables(tableName);
            var clientName = clientEmail.Split('@').First(); 

            var columns = _apiFactory
                .ChangeContext<ISharedStepsGenerator>(_driver)
                .GetCrmTradeTablesColumns(url, clientName, DataRep.RiskTableName);

            var exportParams = _apiFactory
                .ChangeContext<ISharedStepsGenerator>()
                .CreateExportParamsString(columns, tableName);

            var exportRiskTableDto = new
            {
                export_email = userEmail,
                export_params = exportParams,
                export_table_name = tableName,
                service_url = routeForExport
            };

            var response = _apiAccess.ExecutePostEntry(route, exportRiskTableDto);

            if (checkStatusCode)
            {
                _apiAccess.CheckStatusCode(route, response);
            }

            return response.Content.ReadAsStringAsync().Result;
        }

        public string PostExportRisksTableRequest(string url, string clientEmail,
            string userEmail, string apiKey = null, bool checkStatusCode = true)
        {
            var tableName = DataRep.RiskTableName;
            _apiKey = apiKey ?? _apiKey;
            var route = $"{url}{ApiRouteAggregate.PostExportTableWithLink()}?api_key={_apiKey}";
            var routeForExport = ApiRouteAggregate.PostExportCrmTradeTables(tableName);

            var columns = _apiFactory
                .ChangeContext<ISharedStepsGenerator>(_driver)
                .GetCrmTradeTablesColumns(url, clientEmail, DataRep.RiskTableName);

            var exportParams = _apiFactory
                .ChangeContext<ISharedStepsGenerator>()
                .CreateExportParamsString(columns, tableName);

            var exportRiskTableDto = new
            {
                export_email = userEmail,
                export_params = exportParams,
                export_table_name = tableName,
                service_url = routeForExport
            };

            var response = _apiAccess.ExecutePostEntry(route, exportRiskTableDto);

            if (checkStatusCode)
            {
                _apiAccess.CheckStatusCode(route, response);
            }

            return response.Content.ReadAsStringAsync().Result;
        }

        public GeneralResult<GetRiskFilterResult> GetRisksByFilterRequest(string url,
           Dictionary<string, string> filtersValues, string apiKey = null,
           bool checkStatusCode = true)
        {
            var generalResult = new GeneralResult<GetRiskFilterResult>();

            _apiKey = apiKey ?? _apiKey;

            var filters = _apiFactory
              .ChangeContext<ISharedStepsGenerator>()
              .CreateUrlWithFilter(filtersValues);

            var route = $"{url}{ApiRouteAggregate.GetRisksByFilter()}{filters}&api_key={_apiKey}";
            var response = _apiAccess.ExecuteGetEntry(route);

            var json = response
                .Content
                .ReadAsStringAsync()
                .Result;

            if (checkStatusCode)
            {
                _apiAccess.CheckStatusCode(route, response);

                generalResult.GeneralResponse = JsonConvert
                    .DeserializeObject<GetRiskFilterResult>(json);
            }
            else
            {
                generalResult.Message = json;
            }

            return generalResult;
        }

        public string GetRisksRequest(string url, string apiKey, bool checkStatusCode = true)
        {
            var route = ApiRouteAggregate.GetRisks();
            route = $"{url}{route}&api_key={apiKey}";
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

        public T ChangeContext<T>(IWebDriver driver) where T : class
        {
            return _apiFactory.ChangeContext<T>(_driver);
        }
    }
}
