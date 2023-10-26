using System;
using System.Collections.Generic;
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
    public class DepositsPageApi : IDepositsPageApi
    {
        #region Members       
        private readonly IApplicationFactory _apiFactory;
        private IApiAccess _apiAccess;
        private IWebDriver _driver;
        private string _apiKey = Config.appSettings.ApiKey;
        #endregion Members

        public DepositsPageApi(IApplicationFactory apiFactory,
            IWebDriver driver, IApiAccess apiAccess)
        {
            _apiFactory = apiFactory;
            _driver = driver;
            _apiAccess = apiAccess;
        }

        public GeneralResult<SearchResultOnBanking> GetDepositByFiltersRequest(string url,
          Dictionary<string, string> filtersValues, string apiKey = null,
          bool checkStatusCode = true)
        {
            var generalResult = new GeneralResult<SearchResultOnBanking>();
            _apiKey = apiKey ?? _apiKey;

            var filters = _apiFactory
                .ChangeContext<ISharedStepsGenerator>()
                .CreateUrlWithFilter(filtersValues);

            var route = $"{url}{ApiRouteAggregate.GetDepositByFilter()}{filters}&api_key={_apiKey}";
            var response = _apiAccess.ExecuteGetEntry(route);

            var json = response
                .Content
                .ReadAsStringAsync()
                .Result;

            if (checkStatusCode)
            {
                _apiAccess.CheckStatusCode(route, response);

                generalResult.GeneralResponse = JsonConvert
                    .DeserializeObject<SearchResultOnBanking>(json);
            }
            else
            {
                generalResult.Message = json;
            }

            return generalResult;
        }

        public GeneralResult<SearchResultOnBanking> GetDepositDataFromBankingRequest(string url,
            string filterName, string filterValue, string apiKey = null, bool checkStatusCode = true)
        {
            var httpResult = new GeneralResult<SearchResultOnBanking>();
            _apiKey = apiKey ?? _apiKey;
            string temp = null;

            if (filterValue == null)
            {
                temp = $"/api/finance/funds-transactions/by-type/deposit?order[0][column]=" +
                    $"user_id&order[0][dir]=desc&start=0&length=10000&filter[{filterName}]={filterValue}";
            }
            else
            {
                temp = $"/api/finance/funds-transactions/by-type/deposit?order[0][column]=" +
                    $"user_id&order[0][dir]=desc&start=0&length=10000&filter[{filterName}][]={filterValue}";
            }

            var route = $"{url}{temp}&api_key={_apiKey}";
            var response = _apiAccess.ExecuteGetEntry(route);

            var json = response
               .Content
               .ReadAsStringAsync()
               .Result;

            if (checkStatusCode)
            {
                _apiAccess.CheckStatusCode(route, response);

                httpResult.GeneralResponse = JsonConvert
                    .DeserializeObject<SearchResultOnBanking>(json);
            }
            else
            {
                httpResult.Message = json;
            }

            return httpResult;
        }

        public string ExportDepositsTablePipe(string url, string clientEmail,
            string userEmail, string apiKey = null, bool checkStatusCode = true)
        {
            var tableName = "deposit";
            _apiKey = apiKey ?? _apiKey;
            var route = $"{url}{ApiRouteAggregate.PostExportTableWithLink()}?api_key={_apiKey}";
            var routeForExport = ApiRouteAggregate.PostExportBankingTables(tableName);

            var columns = _apiFactory
                .ChangeContext<ISharedStepsGenerator>()
                .GetBankingTableColumns(url, clientEmail, tableName);

            var exportParams = _apiFactory
                .ChangeContext<ISharedStepsGenerator>()
                .CreateExportParamsString(columns, tableName);

            var exportDepositsTableDto = new
            {
                export_email = userEmail,
                export_params = exportParams,
                export_table_name = tableName,
                service_url = routeForExport
            };

            var response = _apiAccess.ExecutePostEntry(route, exportDepositsTableDto);

            if (checkStatusCode)
            {
                _apiAccess.CheckStatusCode(route, response);
            }

            return response.Content.ReadAsStringAsync().Result;
        }

        public string PatchAssignDepositToUserRequest(string url, string depositId,
            string userId, string apiKey = null, bool checkStatusCode = true)
        {
            _apiKey = apiKey ?? _apiKey;
            var route = ApiRouteAggregate.PatchAssignFinanceToUser(depositId, userId);
            route = $"{url}{route}?api_key={_apiKey}";
            var response = _apiAccess.ExecutePatchEntry(route);

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
