// Ignore Spelling: Api

using System.Collections.Generic;
using AirSoftAutomationFramework.Objects.DTOs;
using OpenQA.Selenium;

namespace AirSoftAutomationFramework.Objects.MgmObjects.Api.QAndA
{
    public interface IQEndAApi
    {
        T ChangeContext<T>(IWebDriver driver) where T : class;
        IQEndAApi DeleteQEndAPipe(string url, GetLoginResponse loginData);
        IQEndAApi DeleteQEndARequest(string url, List<string> QEndAIds, GetLoginResponse loginData);
        IQEndAApi DeleteQEndARequest(string url, string QEndAId, GetLoginResponse loginData);
        List<GeneralDto> GetQEndARequest(string url, GetLoginResponse loginData);
        IQEndAApi PostCreateQEndARequest(string url, string answer, string question, GetLoginResponse loginData);
        string PostCreateImageRequest(string url, string fileName,
            GetLoginResponse loginData);

        IQEndAApi PatchBrandDeployQEndARequest(string url, string[] brandIds,
            GetLoginResponse loginData);
    }
}