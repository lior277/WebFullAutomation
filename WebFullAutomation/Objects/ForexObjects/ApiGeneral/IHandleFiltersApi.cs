using System.Collections.Generic;
using AirSoftAutomationFramework.Objects.DTOs;
using OpenQA.Selenium;

namespace AirSoftAutomationFramework.Objects.ForexObjects.ApiGeneral
{
    public interface IHandleFiltersApi
    {
        T ChangeContext<T>(IWebDriver driver) where T : class;
        IHandleFiltersApi DeleteFilterRequest(string url, string filterName, string componentName);
        IHandleFiltersApi DeleteFilterRequest(string url, List<string> filtersNames,
            string componentName);
        Dictionary<string, SaleStatusValues> GetSalesStatusTextFilter(string url);

        List<GetFiltersReponse> GetFiltersRequest(string url, string componentName);
        IHandleFiltersApi VerifyFilterCreated(string url,
            string filtername, string componentName);
        string PostCreateFilterRequest(string url, string filterName,
            bool @default, string id, string itemName, string lable,
            string componentName, string time_filter = "month", bool checkStatusCode = true);
    }
}