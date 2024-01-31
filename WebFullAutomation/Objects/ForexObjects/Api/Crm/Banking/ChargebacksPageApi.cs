// Ignore Spelling: Api

using System;
using AirSoftAutomationFramework.Internals.DAL.ApiAccsess;
using AirSoftAutomationFramework.Internals.DAL.ApiAccsess.ApiRoutes;
using AirSoftAutomationFramework.Internals.DAL.ConfigurationManagement;
using AirSoftAutomationFramework.Internals.Factorys;
using AirSoftAutomationFramework.Objects.ForexObjects.ApiGeneral;
using OpenQA.Selenium;

namespace AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.Banking
{
    public class ChargeBacksPageApi : IChargebacksPageApi
    {
        #region Members       
        private readonly IApplicationFactory _apiFactory;
        private IApiAccess _apiAccess;
        private IWebDriver _driver;
        private string _apiKey = Config.appSettings.ApiKey;
        #endregion Members

        public ChargeBacksPageApi(IApplicationFactory apiFactory, IWebDriver driver, IApiAccess apiAccess)
        {
            _apiFactory = apiFactory;
            _driver = driver;
            _apiAccess = apiAccess;
        }

        public IChargebacksPageApi PatchAssignChargebackToUserRequest(string url,
            string chargebackId, string userId, string apiKey = null)
        {
            var route = ApiRouteAggregate.PatchAssignFinanceToUser(chargebackId, userId);
            _apiKey = apiKey ?? _apiKey;
            route = $"{url}{route}?api_key={_apiKey}";
            var response = _apiAccess.ExecutePatchEntry(route);
            _apiAccess.CheckStatusCode(route, response);

            return this;
        }

        public IChargebacksPageApi PostExportChargebacksTableRequest(string url,
            string clientEmail, string userEmail, string apiKey = null)
        {
            _apiKey = apiKey ?? _apiKey;
            var route = $"{url}{ApiRouteAggregate.PostExportTableWithLink()}?api_key={_apiKey}";
            var routeForExport = ApiRouteAggregate.PostExportBankingTables("chargeback");
            var tableName = "chargebacks";

            var columns = _apiFactory
              .ChangeContext<ISharedStepsGenerator>()
              .GetBankingTableColumns(url, clientEmail, "chargeback");

            var exportParams = _apiFactory
                .ChangeContext<ISharedStepsGenerator>()
                .CreateExportParamsString(columns, tableName);

            var exportChargebacksTableDto = new
            {
                export_email = userEmail,
                export_params = exportParams,
                export_table_name = tableName,
                service_url = routeForExport
            };
            var response = _apiAccess.ExecutePostEntry(route, exportChargebacksTableDto);
            _apiAccess.CheckStatusCode(route, response);

            return this;
        }

        public string GetChargebackDataFromBankingRequest(string url,
            string apiKey, bool checkStatusCode = true)
        {
            _apiKey = apiKey ?? _apiKey;
            var route = $"{url}{ApiRouteAggregate.GetDataFromBankingByType("chargeback")}&api_key={apiKey}";
            var response = _apiAccess.ExecuteGetEntry(route);
            _apiAccess.CheckStatusCode(route, response);

            return response
                .Content
                .ReadAsStringAsync()
                .Result;
        }

        public T ChangeContext<T>(IWebDriver driver) where T : class
        {
            return _apiFactory.ChangeContext<T>(_driver);
        }
    }
}
