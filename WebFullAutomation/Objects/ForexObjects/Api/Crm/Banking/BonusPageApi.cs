// Ignore Spelling: Api Forex Crm

using System.Collections.Generic;
using AirSoftAutomationFramework.Internals.DAL.ApiAccsess;
using AirSoftAutomationFramework.Internals.DAL.ApiAccsess.ApiRoutes;
using AirSoftAutomationFramework.Internals.DAL.ConfigurationManagement;
using AirSoftAutomationFramework.Internals.Factorys;
using AirSoftAutomationFramework.Objects.ForexObjects.ApiGeneral;
using OpenQA.Selenium;

namespace AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.Banking
{
    public class BonusPageApi : IBonusPageApi
    {
        #region Members       
        private readonly IApplicationFactory _apiFactory;
        private IApiAccess _apiAccess;
        private IWebDriver _driver;
        private string _apiKey = Config.appSettings.ApiKey;
        #endregion Members

        public BonusPageApi(IApplicationFactory apiFactory, IWebDriver driver, IApiAccess apiAccess)
        {
            _apiFactory = apiFactory;
            _driver = driver;
            _apiAccess = apiAccess;
        }

        public string ExportBonusTablePipe(string url, string clientEmail,
            string userEmail, string apiKey = null, bool checkStatusCode = true)
        {
            _apiKey = apiKey ?? _apiKey;
            var route = $"{url}{ApiRouteAggregate.PostExportTableWithLink()}?api_key={_apiKey}";
            var routeForExport = ApiRouteAggregate.PostExportBankingTables("deposit_bonus");
            var tableName = "bonuses";

            var columns = _apiFactory
                .ChangeContext<ISharedStepsGenerator>()
                .GetBankingTableColumns(url, clientEmail, "deposit_bonus");

            var exportParams = _apiFactory
                .ChangeContext<ISharedStepsGenerator>()
                .CreateExportParamsString(columns, tableName);

            var exportBonusTableDto = new
            {
                export_email = userEmail,
                export_params = exportParams,
                export_table_name = tableName,
                service_url = routeForExport
            };

            var response = _apiAccess.ExecutePostEntry(route, exportBonusTableDto);

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
