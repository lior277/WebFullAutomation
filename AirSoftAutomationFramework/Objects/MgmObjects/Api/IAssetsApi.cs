// Ignore Spelling: Api

using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using AirSoftAutomationFramework.Objects.DTOs;

namespace AirSoftAutomationFramework.Objects.MgmObjects.Api
{
    public interface IAssetsApi
    {
        T ChangeContext<T>(IWebDriver driver) where T : class;
        IAssetsApi CreateAssetPipe(string url, GetLoginResponse loginData);
        IAssetsApi DeleteAssetsRequest(string url, string assetId, GetLoginResponse loginData);
        List<GetAssestResponse> GetAssetsRequest(string url, GetLoginResponse loginData);
        IAssetsApi PostCreateAssetRequest(string uri, GetLoginResponse loginData);
    }
}