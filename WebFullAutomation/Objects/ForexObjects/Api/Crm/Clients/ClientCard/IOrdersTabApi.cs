using System.Collections.Generic;
using AirSoftAutomationFramework.Objects.DTOs;
using OpenQA.Selenium;

namespace AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.Clients.ClientCard
{
    public interface IOrdersTabApi
    {
        T ChangeContext<T>(IWebDriver driver) where T : class;
        List<GetOrdersByClientIdResponse> GetOrdersByClientIdRequest(string uri, string clientId);
    }
}