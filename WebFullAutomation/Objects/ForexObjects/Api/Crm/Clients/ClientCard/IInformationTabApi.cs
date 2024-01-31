using System.Collections.Generic;
using System.Net.Http;
using AirSoftAutomationFramework.Objects.DTOs;
using OpenQA.Selenium;

namespace AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.Clients.ClientCard
{
    public interface IInformationTabApi
    {
        T ChangeContext<T>(IWebDriver driver) where T : class;
        InformationTabApi DeleteKycFileRequest(string url, string clientId, string kycFileId, string apiKey = null);
        GeneralResult<GetInformationTabResponse> GetInformationTabRequest(string url, string clientId,
            string apiKey = null, bool checkStatusCode = true);
        IInformationTabApi PatchCilentStatusRequest(string url, string clientId, bool clientStatus, string apiKey = null);
        HttpResponseMessage PatchSetTradingGroupRequest(string url, string groupId, List<string> clientsIds, string apiKey = null);
        GeneralResult<GetInformationTabResponse> PutInformationTabRequest(string url, GetInformationTabResponse.InformationTab informationTab, string apiKey = null, bool checkStatusCode = true);
    }
}