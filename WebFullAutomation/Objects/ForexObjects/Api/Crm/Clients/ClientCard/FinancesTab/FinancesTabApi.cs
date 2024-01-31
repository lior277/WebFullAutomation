// Ignore Spelling: Api psp Crm Forex

using AirSoftAutomationFramework.Internals.DAL.ConfigurationManagement;
using AirSoftAutomationFramework.Internals.ExtensionsMethods;
using AirSoftAutomationFramework.Internals.Helpers;
using AirSoftAutomationFramework.Objects.DTOs;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Net.Http;
using AirSoftAutomationFramework.Internals.DAL.ApiAccsess;
using AirSoftAutomationFramework.Internals.DAL.ApiAccsess.ApiRoutes;
using AirSoftAutomationFramework.Internals.Factorys;
using static AirSoftAutomationFramework.Objects.DTOs.GetFinanceDataResponse;

namespace AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.Clients.ClientCard.FinancesTab
{
    public class FinancesTabApi : IFinancesTabApi
    {
        #region Members       
        private readonly IApplicationFactory _apiFactory;
        private IApiAccess _apiAccess;
        private IWebDriver _driver;
        private string _apiKey = Config.appSettings.ApiKey;
        #endregion Members

        public FinancesTabApi(IApplicationFactory apiFactory,
            IWebDriver driver, IApiAccess apiAccess)
        {
            _apiFactory = apiFactory;
            _driver = driver;
            _apiAccess = apiAccess;
        }

        public IFinancesTabApi PutLastDepositDateRequest(string url, string clientId,
            string lastDepositDate, string apiKey = null)
        {
            _apiKey = apiKey ?? _apiKey;
            var route = $"{url}{ApiRouteAggregate.PatchLastDepositDate()}?api_key={_apiKey}";

            var putLastDepositDate = new
            {
                userId = clientId,
                lastDepositDate = lastDepositDate,
            };
            var response = _apiAccess.ExecutePutEntry(route, putLastDepositDate);
            _apiAccess.CheckStatusCode(route, response);

            return this;
        }

        public string PostDepositRequest(string url, string clientId, double actualAmount,
           string lastDigits = null, string transactionType = null,
           string actualMethod = null, string originalCurrency = null,
           string nameForMethod = null, string pspInstanceId = null,
           string pspTransactionId = null, string apiKey = null, bool checkStatusCode = true)
        {
            _apiKey = apiKey ?? _apiKey;
            var route = ApiRouteAggregate.PostCreateDeposit();
            route = $"{url}{route}?api_key={_apiKey}";

            var createDepositRequestDto = new
            {
                user_id = clientId,
                psp_instance_id = pspInstanceId,
                psp_transaction_id = pspTransactionId ?? DataRep.DefaultPspTransactionId,
                last_digits = lastDigits ?? DataRep.LastDigits,
                transaction_type = transactionType ?? "bank_transfer",
                method = actualMethod ?? "bank_transfer",
                original_currency = originalCurrency ?? DataRep.DefaultUSDCurrencyName,
                amount = actualAmount,
                name_for_method = nameForMethod ?? "Bank Transfer",
            };

            var response = _apiAccess.ExecutePostEntry(route, createDepositRequestDto);
            var json = response.Content.ReadAsStringAsync().Result;

            if (checkStatusCode)
            {
                _apiAccess.CheckStatusCode(route, response);

                return JObject.Parse(json)["insertId"].ToString();
            }

            return json;
        }

        public List<string> PostDepositRequest(string url, List<string> clientIds, int actualAmount,
            string lastDigits = null, string transactionType = null,
            string actualMethod = null, string originalCurrency = null,
            string nameForMethod = null, string pspInstanceId = null,
            string pspTransactionId = null, string apiKey = null)
        {
            var depositIds = new List<string>();

            foreach (var id in clientIds)
            {
                depositIds.Add(PostDepositRequest(url, id, actualAmount, lastDigits,
                    transactionType, actualMethod, originalCurrency,
                    nameForMethod, pspInstanceId, pspTransactionId, apiKey));
            }

            return depositIds;
        }

        public IFinancesTabApi PostCreateDepositWithWooCommerceUserRequest(
            string url, string clientName, int depositAmount, string apiKey,
            string payment_method = "bacs", string currency = "USD",
            int id = 1572, string product = "Beginner courses")
        {
            var route = ApiRouteAggregate.PostCreateDepositWithWooCommerceUser();
            route = $"{url}{route}?api_key={apiKey}";
            var email = clientName + DataRep.EmailPrefix;

            var PostCreateDepositWithWooCommerceUser = new
            {
                email = email,
                payment_method = payment_method,
                currency = currency,
                total = depositAmount,
                id = id,
                product = product,
            };

            var response = _apiAccess.ExecutePostEntry(route, PostCreateDepositWithWooCommerceUser);
            var json = response.Content.ReadAsStringAsync().Result;
            _apiAccess.CheckStatusCode(route, response);

            return this;
        }

        public GeneralResult<GeneralDto> PostBonusRequest(string url, string clientId,
            int actualAmount = 1000, string actualMethod = null,
            string apiKey = null, bool checkStatusCode = true)
        {
            _apiKey = apiKey ?? _apiKey;
            var httpResult = new GeneralResult<GeneralDto>();
            var route = ApiRouteAggregate.PostCreateBonus();
            route = $"{url}{route}?api_key={_apiKey}";

            var createBonusRequestDto = new
            {
                user_id = clientId,
                method = actualMethod ?? "Bonus",
                amount = actualAmount,
            };

            var response = _apiAccess.ExecutePostEntry(route, createBonusRequestDto);
            var json = response.Content.ReadAsStringAsync().Result;

            if (checkStatusCode)
            {
                _apiAccess.CheckStatusCode(route, response);
                httpResult.GeneralResponse = JsonConvert.DeserializeObject<GeneralDto>(json);
            }
            else
            {
                httpResult.Message = json;
            }

            return httpResult;
        }

        public IFinancesTabApi PostBonusRequest(string url, List<string> clientIds,
            int actualAmount = 1000, string actualMethod = null,
            string apiKey = null, bool checkStatusCode = true)
        {
            foreach (var clientId in clientIds)
            {
                PostBonusRequest(url, clientId, actualAmount,
                    actualMethod = null, apiKey = null, checkStatusCode = true);
            }

            return this;
        }

        public GeneralResult<GetAvailableWithdrawalByUserIdResponse>
            GetAvailableWithdrawalByClientIdRequest(string url,
           string clientId, string apiKey = null, bool checkStatusCode = true)
        {
            _apiKey = apiKey ?? _apiKey;
            var httpResult = new GeneralResult<GetAvailableWithdrawalByUserIdResponse>();
            var route = ApiRouteAggregate.GetAvailableWithdrawalByClientId();
            route = $"{url}{route}{clientId}?api_key={_apiKey}";
            var response = _apiAccess.ExecuteGetEntry(route);

            var json = response
                .Content
                .ReadAsStringAsync()
                .Result;

            if (checkStatusCode)
            {
                _apiAccess.CheckStatusCode(route, response);

                httpResult.GeneralResponse =
                    JsonConvert.DeserializeObject<GetAvailableWithdrawalByUserIdResponse>(json);
            }
            else
            {
                httpResult.Message = json;
            }

            return httpResult;
        }

        public HttpResponseMessage DeleteResetAccountRequest(string url, string clientId)
        {
            var route = ApiRouteAggregate.DeleteResetAccount();
            route = $"{url}{route}{clientId}?api_key={_apiKey}";
            var response = _apiAccess.ExecuteDeleteEntry(route);
            _apiAccess.CheckStatusCode(route, response);

            return response;
        }

        public string PostWithdrawalBonusRequest(string url, string clientId,
            int withdrawalBonusAmount, string apiKey = null, bool checkStatusCode = true)
        {
            _apiKey = apiKey ?? _apiKey;
            var route = ApiRouteAggregate.PostWithdrawalBonus();
            route = $"{url}{route}?api_key={_apiKey}";

            var createBonusRequestDto = new
            {
                user_id = clientId,
                method = "Bonus",
                amount = withdrawalBonusAmount,
            };
            var response = _apiAccess.ExecutePostEntry(route, createBonusRequestDto);

            if (checkStatusCode)
            {
                _apiAccess.CheckStatusCode(route, response);
            }

            return response.Content.ReadAsStringAsync().Result;
        }

        public IFinancesTabApi PostSplitPendingWithdrawalRequest(string url, string clientId, string withdrawalId,
            int withdrawalSplitsAmount, string apiKey = null)
        {
            _apiKey = apiKey ?? _apiKey;
            var route = ApiRouteAggregate.PostSplitPendingWithdrawal(withdrawalId);
            route = $"{url}{route}?api_key={_apiKey}";

            var withdrawalSplitAmountDto = new
            {
                user_id = clientId,
                amount = withdrawalSplitsAmount,
            };
            var response = _apiAccess.ExecutePostEntry(route, withdrawalSplitAmountDto);
            _apiAccess.CheckStatusCode(route, response);

            return this;
        }

        public string PostWithdrawalDepositRequest(string url,
            string clientId, string userId, int withdrawalAmount,
            string currencyCode = DataRep.DefaultUSDCurrencyName, string apiKey = null, bool checkStatusCode = true)
        {
            _apiKey = apiKey ?? _apiKey;
            var route = ApiRouteAggregate.PostWithdrawalDeposit();
            route = $"{url}{route}?api_key={_apiKey}";

            var financeAccount = new
            {
                erp_user_id = userId
            };

            var userDto = new
            {
                _id = clientId,
                currency_code = currencyCode,
                sales_agent = userId,
                financeAccount = financeAccount
            };

            var postWithdrawalDeposit = new
            {
                amount = withdrawalAmount,
                user = userDto
            };

            var response = _apiAccess.ExecutePostEntry(route, postWithdrawalDeposit);
            var json = response.Content.ReadAsStringAsync().Result;

            if (checkStatusCode)
            {
                _apiAccess.CheckStatusCode(route, response);
            }

            return json;
        }

        public string PatchWithdrawalStatusRequest(string url,
            string clientId, string withdrawalId,
            string withdrawalStatus = "approved",
            string apiKey = null, bool checkStatusCode = true)
        {
            _apiKey = apiKey ?? _apiKey;
            var route = ApiRouteAggregate.PatchWithdrawalStatus();
            route = $"{url}{route}?api_key={_apiKey}";

            var createBonusRequestDto = new
            {
                id = Convert.ToInt32(withdrawalId),
                user_id = clientId,
                status = withdrawalStatus ?? "approved",
            };
            var response = _apiAccess.ExecutePatchEntry(route, createBonusRequestDto);
            var json = response.Content.ReadAsStringAsync().Result;

            if (checkStatusCode)
            {
                _apiAccess.CheckStatusCode(route, response);
            }

            return json;
        }

        public IFinancesTabApi PatchAssignWithdrawalRequest(string url, string withdrawalId,
            string assignWithdrawalTitle = "fees", string apiKey = null)
        {
            _apiKey = apiKey ?? _apiKey;
            var route = ApiRouteAggregate.PatchAssignWithdrawal(withdrawalId,
                assignWithdrawalTitle);

            route = $"{url}{route}?api_key={_apiKey}";
            var response = _apiAccess.ExecutePatchEntry(route);
            _apiAccess.CheckStatusCode(route, response);

            return this;
        }

        public GeneralResult<List<FinanceData>> GetFinanceDataRequest(string url,
            string clientId, string apiKey = null, bool checkStatusCode = true)
        {
            _apiKey = apiKey ?? _apiKey;
            var generalResult = new GeneralResult<List<FinanceData>>();
            var route = ApiRouteAggregate.GetFinanceData(clientId);
            route = $"{url}{route}?api_key={_apiKey}";
            var response = _apiAccess.ExecuteGetEntry(route);

            var json = response
                .Content
                .ReadAsStringAsync()
                .Result;

            if (checkStatusCode)
            {
                _apiAccess.CheckStatusCode(route, response);

                generalResult.GeneralResponse = JsonConvert
                    .DeserializeObject<List<FinanceData>>(json);
            }
            else
            {
                generalResult.Message = json;
            }

            return generalResult;
        }

        //public IFinancesTabApi DeleteFinanceItemRequest(string url, string financeItemId,
        //    string apiKey = null)
        //{
        //    _apiKey = apiKey ?? _apiKey;
        //    var route = ApiRouteAggregate.DeleteFinanceItem(financeItemId);
        //    route = $"{url}{route}?api_key={_apiKey}";
        //    var response = _apiAccess.ExecuteDeleteEntry(route);
        //    _apiAccess.CheckStatusCode(route, response);

        //    return this;
        //}

        public string DeleteFinanceItemRequest(string url, string financeItemId,
            string apiKey = null, bool checkStatusCode = true)
        {
            _apiKey = apiKey ?? _apiKey;
            var route = ApiRouteAggregate.DeleteFinanceItem(financeItemId);
            route = $"{url}{route}?api_key={_apiKey}";
            var response = _apiAccess.ExecuteDeleteEntry(route);

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

        public IFinancesTabApi DeleteWithdrawalRequest(string url, string withdrawalId,
            string apiKey = null)
        {
            _apiKey = apiKey ?? _apiKey;
            var route = ApiRouteAggregate.DeleteFinanceItem(withdrawalId);
            route = $"{url}{route}?api_key={_apiKey}";

            var deleteWithdrawal = new
            {
                delete_reason = "4",
                is_chargeback = false
            };
            var response = _apiAccess.ExecuteDeleteEntry(route, deleteWithdrawal);
            _apiAccess.CheckStatusCode(route, response);

            return this;
        }

        public int DeleteChargeBackDepositRequest(string url, string clientId,
            int depositAmount, string depositId, string apiKey = null)
        {
            var route = ApiRouteAggregate.DeleteFinanceItem(depositId);
            _apiKey = apiKey ?? _apiKey;
            route = $"{url}{route}?api_key={_apiKey}";

            var DeleteChargebackDeposit = new
            {
                user_id = clientId,
                amount = depositAmount,
                is_chargeback = true
            };

            var response = _apiAccess.ExecuteDeleteEntry(route, DeleteChargebackDeposit);
            _apiAccess.CheckStatusCode(route, response);
            var json = response.Content.ReadAsStringAsync().Result;

            return Convert.ToInt32(JArray.Parse(json)[2][0]["chargeback_id"].ToString());
        }

        public string DeleteChargeBackDepositRequest(string url,
            string clientId, int depositAmount, string depositId,
            string apiKey, bool checkStatusCode = true)
        {
            var route = ApiRouteAggregate.DeleteFinanceItem(depositId);
            route = $"{url}{route}?api_key={apiKey}";

            var DeleteChargebackDeposit = new
            {
                user_id = clientId,
                amount = depositAmount,
                is_chargeback = true
            };

            var response = _apiAccess.ExecuteDeleteEntry(route, DeleteChargebackDeposit);

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

        public List<string> GetCreateDepositCurrenciesRequest(string url)
        {
            var currencies = new List<string>();
            var route = $"{url}{ApiRouteAggregate.GetCreateDepositCurrencies()}&api_key={_apiKey}";
            var response = _apiAccess.ExecuteGetEntry(route);
            _apiAccess.CheckStatusCode(route, response);
            var json = response.Content.ReadAsStringAsync().Result;

            JsonConvert.DeserializeObject<GetBaseCurrenciesResponse>(json)
                .GetType()
                .GetProperties()
                .ForEach(p => currencies.Add(p.Name));

            return currencies;
        }

        public T ChangeContext<T>(IWebDriver driver) where T : class
        {
            return _apiFactory.ChangeContext<T>(_driver);
        }
    }
}
