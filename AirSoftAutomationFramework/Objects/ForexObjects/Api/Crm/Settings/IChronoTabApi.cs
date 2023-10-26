using AirSoftAutomationFramework.Objects.DTOs;
using OpenQA.Selenium;

namespace AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.Settings
{
    public interface IChronoTabApi
    {
        T ChangeContext<T>(IWebDriver driver) where T : class;
        GetChronoTabResponse GetChronoTabRequest(string url, string apiKey = null);
        IChronoTabApi PatchMinAmountForBoostOptionRequest(string url, string apiKey = null);
        IChronoTabApi PatchBoostOptionsToDefaultRequest(string url, string apiKey = null);
        IChronoTabApi PutChronoSettingsRequest(string url);
    }
}