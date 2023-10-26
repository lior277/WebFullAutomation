using System.Collections.Generic;
using OpenQA.Selenium;

namespace AirSoftAutomationFramework.Objects.ForexObjects.Ui.Crm.ClientsPage.ClientCard
{
    public interface IDocumentsTabUI
    {
        T ChangeContext<T>(IWebDriver driver) where T : class;
        IDocumentsTabUI ClickOnSectionByTitle(string documentSectionName);
        Dictionary<string, string> GetDocumentTitles(string documentName, string documentStatus);
        string GetEnvelopeBoxBody();
        string GetFileTitles(string fileName);
    }
}