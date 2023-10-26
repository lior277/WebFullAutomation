using System.Collections.Generic;
using AirSoftAutomationFramework.Objects.DTOs;
using OpenQA.Selenium;

namespace AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.Settings
{
    public interface ILanguagesTab
    {
        T ChangeContext<T>(IWebDriver driver = null) where T : class;
        ErpLanguage CreateErpLanguagePipe(string url, string languageName = "Italiano", string languageCode = "it", bool activeStatus = true, bool defaultLanguage = false);
        TradingLanguage CreateTradingLanguagePipe(string url, string languageName = "Italiano", string languageCode = "it", bool activeStatus = true, bool defaultLanguage = false);
        ILanguagesTab DeleteBrandLanguageByIdRequest(string url, string languageId);
        Translations GenerateErpTranslations(Translations erpTranslation);
        TradingTranslation GenerateTradingTranslations(TradingTranslation tradindPlatformTranslation);
        ErpLanguage GetErpLanguagePipe(string url, string languageCode, string apiKey = null);
        ErpLanguage GetLanguageByCode(string url, string languageCode);
        ILanguagesTab PutErpLanguagePipe(string url, string languageName, string languageCode, bool activeStatus = true, bool defaultLanguage = true, Dictionary<string, string> modifiedLanguageItem = null);
        ILanguagesTab PutErpLanguageRequest(string url, ErpLanguage language, bool activeStatus = true, bool defaultLanguage = false, Dictionary<string, string> modifiedLanguageItem = null);
        ILanguagesTab PutTradingLanguagePipe(string url, string languageName, string languageCode, bool activeStatus = true, bool defaultLanguage = true, Dictionary<string, string> modifiedLanguageItem = null);
    }
}