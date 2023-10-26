using System;
using System.Collections.Generic;
using System.Threading;
using AirSoftAutomationFramework.Internals.DAL.ApiAccess;
using AirSoftAutomationFramework.Internals.DAL.ApiAccess.ApiRoutes;
using AirSoftAutomationFramework.Internals.DAL.ConfigurationManagement;
using AirSoftAutomationFramework.Internals.DAL.MongoDb;
using AirSoftAutomationFramework.Internals.ExtensionsMethods;
using AirSoftAutomationFramework.Internals.Factory;
using AirSoftAutomationFramework.Internals.Helpers;
using AirSoftAutomationFramework.Objects.DTOs;
using Microsoft.Graph.Models;
using MongoDB.Bson;
using MongoDB.Driver;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using OpenQA.Selenium;

namespace AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.Settings
{
    public class SuperAdminTubApi : ISuperAdminTubApi
    {
        #region Members       
        private readonly IApplicationFactory _apiFactory;
        private IApiAccess _apiAccess;
        private IWebDriver _driver = null;
        private IMongoDbAccess _mongoDbAccess;
        private string _apiKey = Config.appSettings.ApiKey;
        private string _crmUrl = Config.appSettings.CrmUrl;
        #endregion Members

        public SuperAdminTubApi(IApplicationFactory apiFactory,
            IApiAccess apiAccess, IMongoDbAccess mongoDbAccess)
        {
            _mongoDbAccess = mongoDbAccess;
            _apiFactory = apiFactory;
            _apiAccess = apiAccess;
        }

        public ISuperAdminTubApi PutPlatformRequest(string url)
        {
            var route = ApiRouteAggregate.PutPlatform();
            route = $"{_crmUrl}{route}?api_key={_apiKey}";

            var platformRequestDto = new
            {
                binary = false,
                cfd = true,
                nft = true,
                //nft_only = false,
                crypto = false,
                chrono = true,
                chrono_only = false,
            };
            var response = _apiAccess.ExecutePutEntry(route, platformRequestDto);
            _apiAccess.CheckStatusCode(route, response);

            return this;
        }

        public ISuperAdminTubApi PutChatRequest(string url)
        {
            var route = ApiRouteAggregate.PutChat();
            route = $"{_crmUrl}{route}?api_key={_apiKey}";

            var chatDto = new
            {
                chat_enabled = true,
                mounthly_price = 0,
                color = "black",
            };
            var response = _apiAccess.ExecutePutEntry(route, chatDto);
            _apiAccess.CheckStatusCode(route, response);

            return this;
        }

        public GeneralDto GetBrandRestrictionRequest(string url)
        {
            var route = ApiRouteAggregate.PutBrandRestriction();
            route = $"{_crmUrl}{route}?api_key={_apiKey}";

            var response = _apiAccess.ExecuteGetEntry(route);
            _apiAccess.CheckStatusCode(route, response);
            var json = response.Content.ReadAsStringAsync().Result;

            return JsonConvert.DeserializeObject<GeneralDto>(json);
        }

        public GetGlobalSettingsResponse GetGlobalSettingsRequest(string url)
        {
            var route = ApiRouteAggregate.GetGlobalSettings();
            route = $"{_crmUrl}{route}?api_key={_apiKey}";

            var response = _apiAccess.ExecuteGetEntry(route);
            _apiAccess.CheckStatusCode(route, response);
            var json = response.Content.ReadAsStringAsync().Result;

            return JsonConvert.DeserializeObject<GetGlobalSettingsResponse>(json);
        }

        public ISuperAdminTubApi PutBrandRestrictionRequest(string url, string redirectedUrl,
            int? countryId = null, string countryName = null)
        {
            var route = ApiRouteAggregate.PutBrandRestriction();
            route = $"{_crmUrl}{route}?api_key={_apiKey}";

            var country = new Countries
            {
                Id = countryId ?? 221,
                ItemName = countryName ?? "united states",
                Selected = 1
            };

            var tradingDto = new Trading
            {
                countries = new Countries[] { country },
                ips = Array.Empty<object>(),
                url = redirectedUrl
            };

            var erpDto = new Erp
            {
                countries = new Countries[] { country },
                ips = Array.Empty<object>(),
                url = redirectedUrl
            };

            var restrictDto = new
            {
                trading = tradingDto,
                erp = erpDto
            };
            var response = _apiAccess.ExecutePutEntry(route, restrictDto);
            _apiAccess.CheckStatusCode(route, response);

            return this;
        }

        public ISuperAdminTubApi PutRegulationsRequest(string url,
            GetRegulationResponse getRegulationResponse)
        {
            var route = ApiRouteAggregate.PutSettingsBySection("regulation");
            route = $"{url}{route}?api_key={_apiKey}";

            if (getRegulationResponse.admin_email_for_deposit == null)
            {
                DataRep.EmailListForAdminDeposit.Add("lior@airsoftltd.com");

                getRegulationResponse.admin_email_for_deposit = DataRep
                    .EmailListForAdminDeposit.ToArray();
            }

            if (getRegulationResponse.export_data_email_url == null)
            {
                DataRep.EmailListForExport.Add("lior@airsoftltd.com");

                getRegulationResponse.export_data_email_url = DataRep
                .EmailListForExport.ToArray();
            }

            var regulationsRequestDto = new
            {
                chargeback_months_limit = 3,
                edit_free_text = getRegulationResponse.edit_free_text,
                edit_free_text_2 = getRegulationResponse.edit_free_text2,
                edit_free_text_3 = getRegulationResponse.edit_free_text3,
                edit_free_text_4 = getRegulationResponse.edit_free_text4,
                edit_free_text_5 = getRegulationResponse.edit_free_text5,
                delete_bonus = getRegulationResponse.delete_bonus = true,
                edit_swap = getRegulationResponse.edit_swap = true,
                delete_trades = getRegulationResponse.delete_trades = true,
                reopen_trade = getRegulationResponse.reopen_trade = true,
                export_data = getRegulationResponse.export_data = true,
                export_data_email_url = getRegulationResponse.export_data_email_url ??
                DataRep.EmailListForExport.ToArray(),
                mass_trading = getRegulationResponse.mass_trading = true,
                admin_email_for_deposit = getRegulationResponse.admin_email_for_deposit,
                withdrawal_title = getRegulationResponse.withdrawal_title = true,
                terms_conditions = getRegulationResponse.terms_conditions = true,
                terms_conditions_url = getRegulationResponse.terms_conditions_url = ""
            };
            var response = _apiAccess.ExecutePutEntry(route, regulationsRequestDto);
            _apiAccess.CheckStatusCode(route, response);

            return this;
        }

        public ISuperAdminTubApi PutRiskRestrictionsRequest(string url,
            int maxGroupNumber = 1000, bool allowGroupTitleChange = true,
            bool allowSpreadChange = true)
        {
            var route = ApiRouteAggregate.PutRiskRestrictions();
            route = $"{url}{route}?api_key={_apiKey}";

            var riskRestrictionsDto = new
            {
                max_group_number = maxGroupNumber,
                allow_group_title_change = allowGroupTitleChange,
                allow_spread_change = allowSpreadChange,
            };
            var response = _apiAccess.ExecutePutEntry(route, riskRestrictionsDto);
            _apiAccess.CheckStatusCode(route, response);

            return this;
        }

        public ISuperAdminTubApi PutGroupNamesRequest(string url, string[] groupNames)
        {
            var route = ApiRouteAggregate.PutGroupNames();
            route = $"{url}{route}?api_key={_apiKey}";

            var groupNamesDto = new
            {
                names = groupNames
            };
            var response = _apiAccess.ExecutePutEntry(route, groupNamesDto);
            _apiAccess.CheckStatusCode(route, response);

            return this;
        }

        public GeneralDto GetGroupNamesRequest(string url)
        {
            var route = $"{url}{ApiRouteAggregate.GetGroupNames()}?api_key={_apiKey}";
            var response = _apiAccess.ExecuteGetEntry(route);
            _apiAccess.CheckStatusCode(route, response);
            var json = response.Content.ReadAsStringAsync().Result;

            return JsonConvert.DeserializeObject<GeneralDto>(json);
        }

        public ISuperAdminTubApi DeleteGroupNamesRequest(string url)
        {
            var route = ApiRouteAggregate.PutGroupNames();
            route = $"{url}{route}?api_key={_apiKey}";

            var groupNamesDto = new
            {
                names = Array.Empty<string>()
            };
            var response = _apiAccess.ExecutePutEntry(route, groupNamesDto);
            _apiAccess.CheckStatusCode(route, response);

            return this;
        }

        public ISuperAdminTubApi UpdateGroupRestrictionsTable(string ConfigName, int minus, int plus)
        {
            try
            {
                var mongoDatabase = InitializeMongoClient.ConnectToCrmMongoDb;
                var configCollection = mongoDatabase.GetCollection<BsonDocument>("config");
                var filter = Builders<BsonDocument>.Filter.Eq("config_name", "group_restrictions");
                var update = Builders<BsonDocument>.Update.Set($"{ConfigName}.minus", minus);
                configCollection.UpdateOne(filter, update);
                //Thread.Sleep(3000);
                update = Builders<BsonDocument>.Update.Set($"{ConfigName}.plus", plus);
                configCollection.UpdateOne(filter, update);
                //Thread.Sleep(3000);
            }
            catch (Exception)
            {
                throw;
            }

            return this;
        }

        public ISuperAdminTubApi PutGroupRestrictionsRequest(string url, object group = null,
            int actualMinus = -1, int actualPlus = 1)
        {
            var route = ApiRouteAggregate.GroupRestrictions();
            route = $"{url}{route}?api_key={_apiKey}";
            var groupRestrictions = new GroupRestrictionsGeneral();

            if (group != null)
            {
                var groupName = group.GetType().Name;

                var ff = groupRestrictions
                     .GetType()
                     .GetProperty(groupName.LowerCaseFirstLetter());

                ff.SetValue(groupRestrictions, group);
            }

            groupRestrictions.groupRestrictionsCash ??= new GroupRestrictionsCash
            {
                minus = actualMinus,
                plus = actualPlus
            };

            groupRestrictions.groupRestrictionsCmdty ??= new GroupRestrictionsCmdty
            {
                minus = actualMinus,
                plus = actualPlus
            };

            groupRestrictions.groupRestrictionsCommodities ??= new GroupRestrictionsCommodities
            {
                minus = actualMinus,
                plus = actualPlus
            };

            groupRestrictions.groupRestrictionsCrypto ??= new GroupRestrictionsCrypto
            {
                minus = actualMinus,
                plus = actualPlus
            };

            groupRestrictions.groupRestrictionsDefaultAttr ??= new GroupRestrictionsDefaultAttr
            {
                minus = actualMinus,
                plus = actualPlus
            };

            groupRestrictions.groupRestrictionsForex ??= new GroupRestrictionsForex
            {
                minus = actualMinus,
                plus = actualPlus
            };

            groupRestrictions.groupRestrictionsInd ??= new GroupRestrictionsInd
            {
                minus = actualMinus,
                plus = actualPlus
            };

            groupRestrictions.groupRestrictionsIndices ??= new GroupRestrictionsIndices
            {
                minus = actualMinus,
                plus = actualPlus
            };

            groupRestrictions.groupRestrictionsStk ??= new GroupRestrictionsStk
            {
                minus = actualMinus,
                plus = actualPlus
            };

            groupRestrictions.groupRestrictionsStock ??= new GroupRestrictionsStock
            {
                minus = actualMinus,
                plus = actualPlus
            };

            groupRestrictions.groupRestrictionsNft ??= new GroupRestrictionsNft
            {
                minus = actualMinus,
                plus = actualPlus
            };

            var json = JsonConvert.SerializeObject(groupRestrictions);
            var dto = JObject.Parse(json);
            dto.Property("_id").Remove();
            var response = _apiAccess.ExecutePutEntry(route, dto);
            //Thread.Sleep(3000);
            _apiAccess.CheckStatusCode(route, response);

            return this;
        }

        public GroupRestrictionsForex GetRestrictionsRequest(string uri)
        {
            var route = ApiRouteAggregate.GetGroupRestrictions();
            route = $"{uri}{route}?api_key={_apiKey}";
            var response = _apiAccess.ExecuteGetEntry(route);
            _apiAccess.CheckStatusCode(route, response);
            var json = response.Content.ReadAsStringAsync().Result;

            return JsonConvert.DeserializeObject<GroupRestrictionsForex>(json);
        }

        public GetRegulationResponse GetBrandRegulationRequest(string url)
        {
            var route = $"{url}{ApiRouteAggregate.PutSettingsBySection("global-settings?regulation=true&platform=true")}&api_key={_apiKey}";
            var response = _apiAccess.ExecuteGetEntry(route);
            _apiAccess.CheckStatusCode(route, response);
            var json = response.Content.ReadAsStringAsync().Result;

            return JsonConvert.DeserializeObject<GetRegulationResponse>(json);
        }

        public GetRegulationResponse GetRegulationRequest(string url)
        {
            var route = $"{url}{ApiRouteAggregate.PutRegulation()}?api_key={_apiKey}";
            var response = _apiAccess.ExecuteGetEntry(route);
            _apiAccess.CheckStatusCode(route, response);
            var json = response.Content.ReadAsStringAsync().Result;

            return JsonConvert.DeserializeObject<GetRegulationResponse>(json);
        }

        public GetCurrenciesResponse GetCurrenciesRequest(string url)
        {
            var route = $"{url}{ApiRouteAggregate.GetCurrencies()}?api_key={_apiKey}";
            var response = _apiAccess.ExecuteGetEntry(route);
            _apiAccess.CheckStatusCode(route, response);
            var json = response.Content.ReadAsStringAsync().Result;

            return JsonConvert.DeserializeObject<GetCurrenciesResponse>(json);
        }

        public ISuperAdminTubApi PutCurrenciesRequest(string url)
        {
            var route = ApiRouteAggregate.PutCurrencies();
            route = $"{url}{route}?api_key={_apiKey}";

            var usd = new UsdCurrencie
            {
                max_initial_bonus = 100000,
                max_bonus_after_deposit = 1000,
                Default = true
            };

            var eur = new EurCurrencie
            {
                max_initial_bonus = 100000,
                max_bonus_after_deposit = 1000,
                Default = false
            };

            var cad = new CadCurrencie
            {
                max_initial_bonus = 100000,
                max_bonus_after_deposit = 1000,
                Default = false
            };

            var currenciesDto = new PutCurrencies
            {
                USD = usd,
                EUR = eur,
                CAD = cad
            };

            var putCurrencies = new PutCurrenciesRequest
            {
                currencies = currenciesDto
            };
            var response = _apiAccess.ExecutePutEntry(route, putCurrencies);
            _apiAccess.CheckStatusCode(route, response);

            return this;
        }

        public T ChangeContext<T>(IWebDriver driver = null) where T : class
        {
            return _apiFactory.ChangeContext<T>(_driver);
        }
    }
}
