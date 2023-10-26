using System.Collections.Generic;
using System.Linq;
using AirSoftAutomationFramework.Internals.DAL.ApiAccess;
using AirSoftAutomationFramework.Internals.DAL.ApiAccess.ApiRoutes;
using AirSoftAutomationFramework.Internals.DAL.ConfigurationManagement;
using AirSoftAutomationFramework.Internals.ExtensionsMethods;
using AirSoftAutomationFramework.Internals.Factory;
using AirSoftAutomationFramework.Objects.DTOs;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using OpenQA.Selenium;

namespace AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.Settings
{
    public class LanguagesTab : ILanguagesTab
    {
        #region Members  
        private string _apiKey = Config.appSettings.ApiKey;
        private readonly IApplicationFactory _apiFactory;
        private IApiAccess _apiAccess;
        private IWebDriver _driver;
        #endregion Members

        public LanguagesTab(IApplicationFactory apiFactory, IWebDriver driver, IApiAccess apiAccess)
        {
            _apiFactory = apiFactory;
            _driver = driver;
            _apiAccess = apiAccess;
        }

        public TradingTranslation GenerateTradingTranslations(TradingTranslation
            tradindPlatformTranslation)
        {
            var props = tradindPlatformTranslation.GetType().GetProperties();

            foreach (var prop in props)
            {
                var newName = prop.Name.ToLower().Replace("_", " ").UpperCaseFirstLetter();
                prop.SetValue(tradindPlatformTranslation, newName, null);
            }

            return tradindPlatformTranslation;
        }

        public Translations GenerateErpTranslations(Translations erpTranslation)
        {
            var props = erpTranslation.GetType().GetProperties();

            foreach (var prop in props)
            {
                var newName = prop.Name.ToLower().Replace("_", " ").UpperCaseFirstLetter();
                prop.SetValue(erpTranslation, newName, null);
            }

            return erpTranslation;
        }

        public ErpLanguage GetLanguageByCode(string url, string languageCode)
        {
            var route = $"{url}{ApiRouteAggregate.GetErpLanguageByLangCode(languageCode)}" +
              $"?api_key={_apiKey}";

            var response = _apiAccess.ExecuteGetEntry(route);

            var json = response.Content.ReadAsStringAsync().Result;
            _apiAccess.CheckStatusCode(route, response);

            return JsonConvert.DeserializeObject<ErpLanguage>(json);
        }

        public ErpLanguage GetErpLanguagePipe(string url, string languageCode,
            string apiKey = null)
        {
            _apiKey = apiKey ?? _apiKey;
            var route = $"{url}{ApiRouteAggregate.GetTradingPlatformLanguageData("erp")}" +
              $"?api_key={_apiKey}";

            var response = _apiAccess.ExecuteGetEntry(route);
            _apiAccess.CheckStatusCode(route, response);
            var json = response.Content.ReadAsStringAsync().Result;

            var languageId = JsonConvert.DeserializeObject<TradingLanguage>(json)
              .languageData
              .Where(p => p.code == languageCode)
              .FirstOrDefault()
              ._id;

            route = $"{url}{ApiRouteAggregate.GetLanguageById(languageId)}" +
              $"?api_key={_apiKey}";

            response = _apiAccess.ExecuteGetEntry(route);
            _apiAccess.CheckStatusCode(route, response);
            json = response.Content.ReadAsStringAsync().Result;
            var language = JsonConvert.DeserializeObject<ErpLanguage>(json);
            var translation = GenerateErpTranslations(language.translations);
            language.translations = translation;

            return language;
        }

        private TradingLanguage GetTradingLanguagePipe(string url,
            string languageCode)
        {
            var route = $"{url}{ApiRouteAggregate.GetTradingPlatformLanguageData("trading-platform")}" +
                $"?api_key={_apiKey}";

            var response = _apiAccess.ExecuteGetEntry(route);
            _apiAccess.CheckStatusCode(route, response);
            var json = response.Content.ReadAsStringAsync().Result;

            var languageId = JsonConvert.DeserializeObject<TradingLanguage>(json)
                .languageData
                .Where(p => p.code == languageCode)
                .FirstOrDefault()
                ._id;

            route = $"{url}{ApiRouteAggregate.GetLanguageById(languageId)}" +
                $"?api_key={_apiKey}";

            response = _apiAccess.ExecuteGetEntry(route);
            _apiAccess.CheckStatusCode(route, response);
            json = response.Content.ReadAsStringAsync().Result;
            var language = JsonConvert.DeserializeObject<TradingLanguage>(json);
            var tradingTranslation = GenerateTradingTranslations(language.tradingTranslations);
            language.tradingTranslations = tradingTranslation;

            return language;
        }

        private bool CheckIfLanguageExist(string url, string languageCode,
            string platform)
        {
            var route = $"{url}{ApiRouteAggregate.GetTradingPlatformLanguageData(platform)}" +
                $"?api_key={_apiKey}";

            var response = _apiAccess.ExecuteGetEntry(route);
            _apiAccess.CheckStatusCode(route, response);
            var json = response.Content.ReadAsStringAsync().Result;

            return JsonConvert.DeserializeObject<TradingLanguage>(json)
                .languageData.Any(p => p.code == languageCode);
        }

        public ILanguagesTab DeleteBrandLanguageByIdRequest(string url,
            string languageId)
        {
            var route = $"{url}{ApiRouteAggregate.PutLanguageById(languageId)}" +
                $"?api_key={_apiKey}";

            var response = _apiAccess.ExecuteDeleteEntry(route);
            _apiAccess.CheckStatusCode(route, response);

            return this;
        }

        private ILanguagesTab PostCreateErpLanguageRequest(string url,
            string languageName = "Italiano", string languageCode = "it")
        {
            var route = ApiRouteAggregate.PostCreateLanguage();
            route = $"{url}{route}?api_key={_apiKey}";
            var language = GetErpLanguagePipe(url, "en");

            var createTradeRequestDto = new ErpLanguage
            {
                name = languageName,
                code = languageCode,
                platform = language.platform,
                translations = GenerateErpTranslations(language.translations),
                active = true,
                @default = false
            };
            var json = JsonConvert.SerializeObject(createTradeRequestDto);
            var dto = JObject.Parse(json);
            dto.Property("_id").Remove();
            var response = _apiAccess.ExecutePostEntry(route, dto);
            _apiAccess.CheckStatusCode(route, response);

            return this;
        }

        private ILanguagesTab PostCreateTradingLanguageRequest(string url,
           string languageName = "Italiano", string languageCode = "it")
        {
            var route = ApiRouteAggregate.PostCreateLanguage();
            route = $"{url}{route}?api_key={_apiKey}";
            var language = GetTradingLanguagePipe(url, "en");//the default;

            var createTradingPlatformLanguageDto = new TradingLanguage
            {
                name = languageName,
                code = languageCode,
                platform = language.platform,
                tradingTranslations = GenerateTradingTranslations(
                    language.tradingTranslations),

                active = true,
                @default = false
            };
            var json = JsonConvert.SerializeObject(createTradingPlatformLanguageDto);
            var dto = JObject.Parse(json);
            dto.Property("_id").Remove();
            dto.Property("data").Remove();
            var response = _apiAccess.ExecutePostEntry(route, dto);
            _apiAccess.CheckStatusCode(route, response);

            return this;
        }

        public ErpLanguage CreateErpLanguagePipe(string url,
          string languageName = "Italiano", string languageCode = "it",
          bool activeStatus = true, bool defaultLanguage = false)
        {
            var erpLanguageExist = CheckIfLanguageExist(url, languageCode, "erp");
            ErpLanguage erpLanguage;

            if (erpLanguageExist)
            {
                erpLanguage = GetErpLanguagePipe(url, languageCode);
                DeleteBrandLanguageByIdRequest(url, erpLanguage._id);
            }

            PostCreateErpLanguageRequest(url, languageName, languageCode);

            return GetErpLanguagePipe(url, languageCode);
        }

        public TradingLanguage CreateTradingLanguagePipe(string url,
         string languageName = "Italiano", string languageCode = "it",
         bool activeStatus = true, bool defaultLanguage = false)
        {
            var tradeLanguageExist = CheckIfLanguageExist(url, languageCode, "trading-platform");
            TradingLanguage tradingLanguage;

            if (tradeLanguageExist)
            {
                tradingLanguage = GetTradingLanguagePipe(url, languageCode);
                DeleteBrandLanguageByIdRequest(url, tradingLanguage._id);
            }

            PostCreateTradingLanguageRequest(url, languageName, languageCode);

            return GetTradingLanguagePipe(url, languageCode);
        }

        private ILanguagesTab PutTradingLanguageRequest(string url,
            TradingLanguage language, bool activeStatus = true,
            bool defaultLanguage = false, Dictionary<string, string> modifiedLanguageItem = null)
        {
            var route = $"{url}{ApiRouteAggregate.PutLanguageById(language._id)}" +
           $"?api_key={_apiKey}";

            var translation = GenerateTradingTranslations(language.tradingTranslations);

            var languageDto = new TradingLanguage
            {
                name = language.name,
                code = language.code,
                platform = language.platform,
                tradingTranslations = translation,
                active = activeStatus,
                @default = defaultLanguage
            };

            if (modifiedLanguageItem != null)
            {
                languageDto.tradingTranslations.GetType().GetProperties().Where(p => p.Name
                == modifiedLanguageItem.Keys.First()).First().SetValue(translation,
                modifiedLanguageItem.Values.First(), null);
            }

            var json = JsonConvert.SerializeObject(languageDto);
            var dto = JObject.Parse(json);
            dto.Property("_id").Remove();
            dto.Property("data").Remove();
            var response = _apiAccess.ExecutePutEntry(route, dto);
            _apiAccess.CheckStatusCode(route, response);

            return this;
        }

        public ILanguagesTab PutErpLanguageRequest(string url,
        ErpLanguage language, bool activeStatus = true,
        bool defaultLanguage = false,
        Dictionary<string, string> modifiedLanguageItem = null)
        {
            var route = $"{url}{ApiRouteAggregate.PutLanguageById(language._id)}" +
           $"?api_key={_apiKey}";

            var languageDto = new ErpLanguage
            {
                name = language.name,
                code = language.code,
                platform = language.platform,
                translations = GenerateErpTranslations(language.translations),
                active = activeStatus,
                @default = defaultLanguage
            };

            if (modifiedLanguageItem != null)
            {
                languageDto.translations.GetType().GetProperties().Where(p => p.Name
                == modifiedLanguageItem.Keys.First()).First().SetValue(language.translations,
                modifiedLanguageItem.Values.First(), null);
            }

            var json = JsonConvert.SerializeObject(languageDto);
            var dto = JObject.Parse(json);
            dto.Property("_id").Remove();
            var response = _apiAccess.ExecutePutEntry(route, dto);
            _apiAccess.CheckStatusCode(route, response);

            return this;
        }

        public ILanguagesTab PutErpLanguagePipe(string url, string languageName,
            string languageCode, bool activeStatus = true, bool defaultLanguage = true,
            Dictionary<string, string> modifiedLanguageItem = null)
        {
            var erpLanguageExist = CheckIfLanguageExist(url, languageCode, "erp");

            if (!erpLanguageExist)
            {
                PostCreateErpLanguageRequest(url, languageName, languageCode);
            }

            var erpLanguage = GetErpLanguagePipe(url, languageCode);

            PutErpLanguageRequest(url, erpLanguage, activeStatus,
                modifiedLanguageItem: modifiedLanguageItem);

            return this;
        }

        public ILanguagesTab PutTradingLanguagePipe(string url, string languageName,
            string languageCode, bool activeStatus = true, bool defaultLanguage = true,
            Dictionary<string, string> modifiedLanguageItem = null)
        {
            var languageExist = CheckIfLanguageExist(url, languageCode,
                "trading-platform");

            if (!languageExist)
            {
                PostCreateTradingLanguageRequest(url, languageName, languageCode);
            }

            var tradingLanguage = GetTradingLanguagePipe(url, languageCode);

            PutTradingLanguageRequest(url, tradingLanguage, activeStatus,
                modifiedLanguageItem: modifiedLanguageItem);

            return this;
        }

        public T ChangeContext<T>(IWebDriver driver = null) where T : class
        {
            return _apiFactory.ChangeContext<T>(_driver);
        }
    }
}
