// Ignore Spelling: Forex Api Crm

using AirSoftAutomationFramework.Objects.DTOs;
using OpenQA.Selenium;
using System.Collections.Generic;

namespace AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.Settings
{
    public interface IOfficeTabApi
    {
        T ChangeContext<T>(IWebDriver driver) where T : class;
        IOfficeTabApi DeleteDialer(string url, string dialerId);
        IOfficeTabApi DeleteOfficeByIdRequest(string url, string officeId, string apiKey = null);
        IOfficeTabApi DeleteTrunkByIdRequest(string url, string trunkId);
        List<GetDialersResponse> GetDialers(string url);
        bool CheckIfAutomationTrunkExist(string url);
        string GetLeaderBoardRequest(string url, string officeId);
        GetOfficeResponse GetOfficesByName(string url, string officeByName = "Main Office", string apiKey = null);
        List<GetOfficeResponse> GetOfficesRequest(string url);
        List<GetTrunksRequest.TrunkData> GetTrunkRequest(string url);
        GetTrunkResponse GetTrunks(string url, string trunkName);
        IOfficeTabApi PostCreateDialerRequest(string url, string officeId);
        IOfficeTabApi PostCreateOfficeRequest(string url, string officeCity, string pbxName = null, string apiKey = null);
        IOfficeTabApi PostCreateTrunkPipe(string url, string trunkName = null, string officeId = null, string pbxName = null, string trunkNumber = "5");
        IOfficeTabApi PutAssignIpsToRequest(string url);
        IOfficeTabApi PutOfficeRequest(string url, GetOfficeResponse getOfficeResponses, string apiKey = null);
        IOfficeTabApi PutOfficeRequest(string url, List<GetOfficeResponse> getOfficeResponses);
    }
}