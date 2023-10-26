using System.Collections.Generic;
using OpenQA.Selenium;

namespace AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.Clients.ClientCard
{
    public interface IPlanningTabApi
    {
        T ChangeContext<T>(IWebDriver driver) where T : class;
        string PostAddMassAssignCommentRequest(string url, List<string> clientsIds, string expectedComment, string apiKey = null);
        string PostCreateAddCommentRequest(string url, string clientId, string apiKey = null, bool checkStatusCode = true);
    }
}