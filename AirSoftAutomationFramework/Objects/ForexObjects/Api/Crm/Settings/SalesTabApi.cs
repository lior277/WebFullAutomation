using System;
using System.Collections.Generic;
using System.Linq;
using AirSoftAutomationFramework.Internals.DAL.ApiAccess;
using AirSoftAutomationFramework.Internals.DAL.ApiAccess.ApiRoutes;
using AirSoftAutomationFramework.Internals.DAL.ConfigurationManagement;
using AirSoftAutomationFramework.Internals.Factory;
using AirSoftAutomationFramework.Internals.Helpers;
using AirSoftAutomationFramework.Objects.DTOs;
using Newtonsoft.Json;
using OpenQA.Selenium;

namespace AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.Settings
{
    public class SalesTabApi : ISalesTabApi
    {
        #region Members  
        private string _apiKey = Config.appSettings.ApiKey;
        private readonly IApplicationFactory _apiFactory;
        private IApiAccess _apiAccess;
        private IWebDriver _driver;
        #endregion Members

        public SalesTabApi(IApplicationFactory apiFactory, IWebDriver driver,
            IApiAccess apiAccess)
        {
            _apiFactory = apiFactory;
            _driver = driver;
            _apiAccess = apiAccess;
        }

        public string PostCreateSavingAccountRequest(string url, bool isDefault = false, string apiKey = null)
        {
            _apiKey = apiKey ?? _apiKey;
            var route = $"{url}{ApiRouteAggregate.PostCreateSavingAccount()}?api_key={_apiKey}";
            object postCreateSavingAccountDto;

            var savingAccountName = TextManipulation.RandomString();

            postCreateSavingAccountDto = new
            {
                name = savingAccountName,
                platform_name = "",
                percent_per_month = 1,
                client_transfer = true,
                daily = false,
                @default = isDefault
            };
            var response = _apiAccess.ExecutePostEntry(route, postCreateSavingAccountDto);
            _apiAccess.CheckStatusCode(route, response);

            return savingAccountName;
        }

        public string PostSalasAgentSalaryRequest(string url, int depositAmount, string apiKey = null)
        {
            var route = $"{url}{ApiRouteAggregate.PostSalasAgentSalary()}?api_key={_apiKey}";

            var salasAgentSalaryName = DataRep.SalasAgentSalaryName;
            var createSalaryRequest = new CreateSalaryRequest();
            createSalaryRequest.name = salasAgentSalaryName;

            var currentMonth = DateTime.Now.ToString("MMMM").ToLower();
            var createSalaryType = createSalaryRequest.GetType();
            var properties = createSalaryType.GetProperties();

            foreach (var prop in properties)
            {
                if (prop.PropertyType == typeof(string))
                {
                    continue;
                }

                var monthType = prop.PropertyType;
                var monthObj = Activator.CreateInstance(monthType);

                // DepositCount properties         
                var depositCountProp = monthType.GetProperty("deposit_count");
                var depositCountFullName = depositCountProp.PropertyType.FullName;
                var depositCountType = Type.GetType(depositCountFullName);
                var depositCountObj = (Deposit_Count)Activator.CreateInstance(depositCountType);

                // DepositSum properties
                var depositSumProp = monthType.GetProperty("deposit_sum");
                var depositSumFullName = depositSumProp.PropertyType.FullName;
                var depositSumType = Type.GetType(depositSumFullName);
                var depositSumObj = (Deposit_Sum)Activator.CreateInstance(depositSumType);

                // Conversion properties
                var conversionProp = monthType.GetProperty("conversion");
                var conversionFullName = conversionProp.PropertyType.FullName;
                var conversionType = Type.GetType(conversionFullName);
                var conversionObj = (Conversion)Activator.CreateInstance(conversionType);

                if (prop.Name.Equals(currentMonth))
                {
                    depositCountObj.fail = 0;
                    depositCountObj.success = 0;
                    depositCountObj.target = 1;

                    depositSumObj.fail = 0;
                    depositSumObj.success = 0;
                    depositSumObj.target = depositAmount;

                    conversionObj.fail = 0;
                    conversionObj.success = 0;
                    conversionObj.target = 100;
                }
                else
                {
                    depositCountObj.fail = 0;
                    depositCountObj.success = 0;
                    depositCountObj.target = 0;

                    depositSumObj.fail = 0;
                    depositSumObj.success = 0;
                    depositSumObj.target = 0;

                    conversionObj.fail = 0;
                    conversionObj.success = 0;
                    conversionObj.target = 0;
                }

                depositCountProp.SetValue(monthObj, depositCountObj);
                depositSumProp.SetValue(monthObj, depositSumObj);
                conversionProp.SetValue(monthObj, conversionObj);
                prop.SetValue(createSalaryRequest, monthObj);

            }
            var response = _apiAccess.ExecutePostEntry(route, createSalaryRequest);
            _apiAccess.CheckStatusCode(route, response);

            return salasAgentSalaryName;
        }

        public string CreateSalasAgentSalaryPipe(string url, int depositAmount, string apiKey = null)
        {
            var route = $"{url}{ApiRouteAggregate.PostSalasAgentSalary()}?api_key={_apiKey}";

            var automationSalaryExist = GetSalasAgentSalaryRequest(url)
                .Any(p => p.Name == DataRep.SalasAgentSalaryName);

            if (!automationSalaryExist)
            {
                PostSalasAgentSalaryRequest(url, depositAmount);
            }

            return GetSalasAgentSalaryRequest(url)
               .Where(p => p.Name == DataRep.SalasAgentSalaryName)
               .FirstOrDefault()
               .Id;
        }

        public ISalesTabApi DeleteSalasAgentSalaryRequest(string url,
            string salasAgentSalaryId, string apiKey = null)
        {
            var route = $"{url}{ApiRouteAggregate.DeleteSalasAgentSalary(salasAgentSalaryId)}?api_key={_apiKey}";
            var response = _apiAccess.ExecuteDeleteEntry(route);
            _apiAccess.CheckStatusCode(route, response);

            return this;
        }

        public List<GeneralDto> GetSalasAgentSalaryRequest(string url)
        {
            var route = $"{url}{ApiRouteAggregate.GetSalasAgentSalary()}?api_key={_apiKey}";
            var response = _apiAccess.ExecuteGetEntry(route);
            _apiAccess.CheckStatusCode(route, response);
            var json = response.Content.ReadAsStringAsync().Result;

            return JsonConvert.DeserializeObject<List<GeneralDto>>(json);
        }

        public ISalesTabApi PostCreateAccountTypeRequest(string url,
            string accountTypeName = null, string apiKey = null)
        {
            _apiKey = apiKey ?? _apiKey;
            var route = $"{url}{ApiRouteAggregate.PostCreateAccountTypes()}?api_key={_apiKey}";

            var postCreateAccountTypeDto = new
            {
                name = accountTypeName ?? DataRep.AccountTypeNameForAutomation,
                chat_enabled = true,
                initial_bonus = "1000",
                demo_initial_demo = 0,
                show_on_register = false,
                enable_demo = false,
                nft_trading = true,
                demo_reinitialize_balance_on_0 = false,
                @default = false,
                saving_account_id = (string)null,
                deposit_range_id = Array.Empty<string>(),
                trading_tabs = Array.Empty<string>(),
                chrono_trading = true
            };
            var response = _apiAccess.ExecutePostEntry(route, postCreateAccountTypeDto);
            _apiAccess.CheckStatusCode(route, response);

            return this;
        }

        public ISalesTabApi PostCreateDepositRangeRequest(string url)
        {
            var route = $"{url}{ApiRouteAggregate.PostCreateDepositRange()}?api_key={_apiKey}";

            var postDepositRangeDto = new
            {
                name = DataRep.AutomationDepositRangeGroupName,
                from_sum = 0,
                to_sum = 0,
                bonus_on_deposit = 0,
            };
            var response = _apiAccess.ExecutePostEntry(route, postDepositRangeDto);
            _apiAccess.CheckStatusCode(route, response);

            return this;
        }

        public string PostCreateDepositRangePipe(string url)
        {
            var route = $"{url}{ApiRouteAggregate.PostCreateDepositRange()}?api_key={_apiKey}";

            var automationDepositRangeExist = GetDepositRangeRequest(url)
                .depositGroupRange.Any(p => p.name == DataRep.AutomationDepositRangeGroupName);

            if (!automationDepositRangeExist)
            {
                PostCreateDepositRangeRequest(url);
            }

            return GetDepositRangeRequest(url)
                .depositGroupRange
                .Where(p => p.name == DataRep.AutomationDepositRangeGroupName)
                .FirstOrDefault()
                ._id;
        }

        public DepositGroupRangeResponse GetDepositRangeRequest(string url)
        {
            var route = $"{url}{ApiRouteAggregate.PostCreateDepositRange()}?api_key={_apiKey}";
            var response = _apiAccess.ExecuteGetEntry(route);
            _apiAccess.CheckStatusCode(route, response);
            var json = response.Content.ReadAsStringAsync().Result;

            return JsonConvert.DeserializeObject<DepositGroupRangeResponse>(json);
        }

        public ISalesTabApi PutAccountTypeRequest(string url,
            AccountTypeData accountTypeData, string apiKey = null)
        {
            _apiKey = apiKey ?? _apiKey;
            var route = $"{url}{ApiRouteAggregate.PutAccountTypes(accountTypeData.AccountTypeId)}" +
                $"?api_key={_apiKey}";

            var putCreateAccountTypeDto = new
            {
                name = accountTypeData.AccountTypeName,
                chat_enabled = accountTypeData.ChatEnabled ?? true,
                initial_bonus = accountTypeData.InitialBonus ?? 1000,
                demo_initial_demo = accountTypeData.DemoInitialDemo ?? 0,
                show_on_register = accountTypeData.ShowOnRegister ?? false,
                enable_demo = accountTypeData.EnableDemo ?? false,
                demo_reinitialize_balance_on_0 = accountTypeData.DemoReinitializeBalanceOn0 ?? false,
                @default = accountTypeData.AccountTypeDefault ?? false,
                saving_account_id = accountTypeData.SavingAccountId,
                deposit_range_id = accountTypeData.DepositRangeId ?? Array.Empty<string>(),
                trading_tabs = accountTypeData.TradingTabs ?? Array.Empty<string>(),
                chrono_trading = accountTypeData.ChronoTrading ?? true,
                nft_trading = accountTypeData.NftTrading ?? true
            };
            var response = _apiAccess.ExecutePutEntry(route, putCreateAccountTypeDto);
            _apiAccess.CheckStatusCode(route, response);

            return this;
        }

        public GetRecaptchaResponse GetRecaptchaRequest(string url)
        {
            var route = $"{url}{ApiRouteAggregate.PutRecaptcha()}?api_key={_apiKey}";
            var response = _apiAccess.ExecuteGetEntry(route);
            _apiAccess.CheckStatusCode(route, response);
            var json = response.Content.ReadAsStringAsync().Result;

            return JsonConvert.DeserializeObject<GetRecaptchaResponse>(json);
        }

        public AccountTypeData CreateAutomationAccountTypePipe(string url)
        {
            // check if the account type exist
            var accountTypeForAutomation = GetAccountTypesRequest(url)
             .AccountTypeData
             .Any(p => p.AccountTypeName == DataRep.AccountTypeNameForAutomation);

            if (!accountTypeForAutomation)
            {
                PostCreateAccountTypeRequest(url);
            }

            return GetAccountTypesRequest(url)
                .AccountTypeData
                .Where(p => p.AccountTypeName == DataRep.AccountTypeNameForAutomation)
                .FirstOrDefault();
        }

        public GetAccountTypesResponse GetAccountTypesRequest(string url)
        {
            var route = $"{url}{ApiRouteAggregate.PostCreateAccountTypes()}?api_key={_apiKey}";
            var response = _apiAccess.ExecuteGetEntry(route);
            _apiAccess.CheckStatusCode(route, response);
            var json = response.Content.ReadAsStringAsync().Result;

            return JsonConvert.DeserializeObject<GetAccountTypesResponse>(json);
        }

        public ISalesTabApi DeleteAccountTypeRequest(string url,
            string accountTypeId, string apiKey = null)
        {
            _apiKey = apiKey ?? _apiKey;
            var route = $"{url}{ApiRouteAggregate.PostCreateAccountTypes()}/{accountTypeId}?api_key={_apiKey}";
            var response = _apiAccess.ExecuteDeleteEntry(route);
            _apiAccess.CheckStatusCode(route, response);

            return this;
        }

        public ISalesTabApi DeleteSavingAccountRequest(string url,
            string savingAccountTypeId, string apiKey = null)
        {
            _apiKey = apiKey ?? _apiKey;
            var route = $"{url}{ApiRouteAggregate.PutEditSavingAccount(savingAccountTypeId)}?api_key={_apiKey}";
            var response = _apiAccess.ExecuteDeleteEntry(route);
            _apiAccess.CheckStatusCode(route, response);

            return this;
        }

        public ISalesTabApi PutEditSavingAccountRequest(string url,
            string savingAccountId, string savingAccountName, string apiKey)
        {
            _apiKey = apiKey ?? _apiKey;
            var route = $"{url}{ApiRouteAggregate.PutEditSavingAccount(savingAccountId)}?api_key={_apiKey}";

            var putCreateSavingAccountDto = new
            {
                name = savingAccountName,
                platform_name = "",
                percent_per_month = 1,
                client_transfer = false,
                daily = false,
                @default = false
            };
            var response = _apiAccess.ExecutePutEntry(route, putCreateSavingAccountDto);
            _apiAccess.CheckStatusCode(route, response);

            return this;
        }

        public GetSavingAccountResponse GetSavingAccountsRequest(string url, string apiKey = null)
        {
            _apiKey = apiKey ?? _apiKey;
            var route = $"{url}{ApiRouteAggregate.GetSavingAccount()}?api_key={_apiKey}";
            var response = _apiAccess.ExecuteGetEntry(route);
            _apiAccess.CheckStatusCode(route, response);
            var json = response.Content.ReadAsStringAsync().Result;

            return JsonConvert.DeserializeObject<GetSavingAccountResponse>(json);
        }

        public T ChangeContext<T>(IWebDriver driver) where T : class
        {
            return _apiFactory.ChangeContext<T>(_driver);
        }
    }
}
