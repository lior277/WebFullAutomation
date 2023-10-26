// Ignore Spelling: Api

using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using AirSoftAutomationFramework.Objects.DTOs;

namespace AirSoftAutomationFramework.Objects.MgmObjects.Api
{
    public interface IMgmCreateUserApi
    {
        T ChangeContext<T>(IWebDriver driver) where T : class;
        GetLoginResponse PostMgmLoginCookiesRequest(string uri, string userName);
        string PostCreateMgmApiKeyRequest(string uri, string userId, GetLoginResponse loginData);
    }
}