// Ignore Spelling: Api Forex Crm

using AirSoftAutomationFramework.Internals.DAL.ApiAccess;
using AirSoftAutomationFramework.Internals.DAL.ApiAccess.ApiRoutes;
using AirSoftAutomationFramework.Internals.DAL.ConfigurationManagement;
using AirSoftAutomationFramework.Internals.Factory;
using AirSoftAutomationFramework.Objects.DTOs;
using AirSoftAutomationFramework.Objects.ForexObjects.ApiGeneral;
using Newtonsoft.Json;
using OpenQA.Selenium;

namespace AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.Banking
{
    public class WithdrawalsPageApi : IWithdrawalsPageApi
    {
        #region Members       
        private readonly IApplicationFactory _apiFactory;
        private IApiAccess _apiAccess;
        private string _apiKey = Config.appSettings.ApiKey;
        #endregion Members

        public WithdrawalsPageApi(IApplicationFactory apiFactory, IApiAccess apiAccess)
        {
            _apiFactory = apiFactory;
            _apiAccess = apiAccess;
        }

        public GeneralResult<SearchResultOnBanking> GetWithdrawalDataFromBankingRequest(string url,
            string filterName, string filterValue, string apiKey = null, bool checkStatusCode = true)
        {
            var generalResult = new GeneralResult<SearchResultOnBanking>();
            _apiKey = apiKey ?? _apiKey;
            var route = $"{url}{ApiRouteAggregate.GetDataFromBankingByTypeAndFilter("withdrawal", filterName, filterValue)}&api_key={_apiKey}";
            var response = _apiAccess.ExecuteGetEntry(route);

            var json = response
               .Content
               .ReadAsStringAsync()
               .Result;

            if (checkStatusCode)
            {
                _apiAccess.CheckStatusCode(route, response);
                generalResult.GeneralResponse = JsonConvert.DeserializeObject<SearchResultOnBanking>(json);
            }
            else
            {
                generalResult.Message = json;
            }

            return generalResult;
        }

        public IWithdrawalsPageApi ExportWithdrawalsTablePipe(string url, string clientEmail,
            string userEmail, string apiKey = null)
        {
            _apiKey = apiKey ?? _apiKey;
            var route = $"{url}{ApiRouteAggregate.PostExportTableWithLink()}?api_key={_apiKey}";
            var routeForExport = ApiRouteAggregate.PostExportBankingTables("withdrawal");

            var columns = _apiFactory
                .ChangeContext<ISharedStepsGenerator>()
                .GetBankingTableColumns(url, clientEmail, "withdrawal");

            var exportParams = _apiFactory
                .ChangeContext<ISharedStepsGenerator>()
                .CreateExportParamsString(columns, "withdrawals");

            var exportWithdrawalsTableDto = new
            {
                export_email = userEmail,
                export_params = exportParams,
                export_table_name = "withdrawals",
                service_url = routeForExport
            };

            var response = _apiAccess.ExecutePostEntry(route, exportWithdrawalsTableDto);
            _apiAccess.CheckStatusCode(route, response);

            return this;
        }

        public T ChangeContext<T>(IWebDriver driver) where T : class
        {
            return _apiFactory.ChangeContext<T>(driver);
        }
    }
}
