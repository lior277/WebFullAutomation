// Ignore Spelling: Api

using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using AirSoftAutomationFramework.Objects.DTOs;

namespace AirSoftAutomationFramework.Objects.MgmObjects.Api
{
    public interface IAssetsGroupApi
    {
        T ChangeContext<T>(IWebDriver driver) where T : class;
        GetCfdResponse GetCfdRequest(string url, GetLoginResponse loginData);
    }
}