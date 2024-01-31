using AirSoftAutomationFramework.Objects.DTOs;
using OpenQA.Selenium;
using System.IO;

namespace AirSoftAutomationFramework.Objects.ForexObjects.TradingPlatform.TradeObjects.TradeApi.Profile
{
    public interface IProfilePageApi
    {
        T ChangeContext<T>(IWebDriver driver) where T : class;
        GetClientProfileResponse GetClientProfileRequest(string url, GetLoginResponse loginData);
        Stream GetGeneralDtoRequest(string url, GetLoginResponse loginData);
        IProfilePageApi PatchClientProfileRequest(string url, GetClientProfileResponse getClientProfileResponse, GetLoginResponse loginData);
        IProfilePageApi PatchKycFileRequest(string url, string fileName, string fileContentName, GetLoginResponse loginData);
    }
}