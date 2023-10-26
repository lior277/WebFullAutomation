// Ignore Spelling: Cfd Api

using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using AirSoftAutomationFramework.Objects.DTOs;

namespace AirSoftAutomationFramework.Objects.MgmObjects.Api.Risk.AssetsCfd
{
    public interface IAssetsCfdApi
    {
        T ChangeContext<T>(IWebDriver driver) where T : class;
        List<GetCfdResponse.AssetData> GetCfdRequest(string url, GetLoginResponse loginData);
        IAssetsCfdApi PatchCfdRequest(GetLoginResponse loginData, GetCfdResponse.AssetData cfdAssetData);
        IAssetsCfdApi PatchMgmFrontAssetsBrandsDeployCfdRequest(string url, string brandId, GetLoginResponse loginData);
    }
}