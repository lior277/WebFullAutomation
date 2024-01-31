using System.Collections.Generic;
using System.Linq;
using AirSoftAutomationFramework.Internals.ExtensionsMethods;
using AirSoftAutomationFramework.Internals.Factorys;
using OpenQA.Selenium;

namespace AirSoftAutomationFramework.Objects.ForexObjects.Ui.Crm.ClientsPage.ClientCard
{
    public class DocumentsTabUI : IDocumentsTabUI
    {
        private IWebDriver _driver;
        private string _documentSection = "//span[contains(.,'{0}')]/parent::a[@role='button']";
        private readonly IApplicationFactory _apiFactory;

        public DocumentsTabUI(IApplicationFactory apiFactory, IWebDriver driver)
        {
            _driver = driver;
            _apiFactory = apiFactory;
        }

        #region Locator's
        private readonly By EnvelopeExp = By.CssSelector("button[class='btn btn-info email-content-btn main-email-content-btn']");
        private readonly By EnvelopeBoxTitleExp = By.CssSelector("h4[class*='modal-title ng-star-inserted']");
        private readonly By EnvelopeBoxBodyExp = By.CssSelector("div[class*='list-error-service']");
        private readonly By DocumentsNameExp = By.CssSelector("div[class='panel-default panel panel-open'] td[class='sorting_1']");
        private readonly By DocumentsListInClientCardExp = By.CssSelector("div[class='panel-default panel panel-open'] table[id='filesTable'] tbody tr");
        private readonly By DocumentStatusExp = By.CssSelector("div[class='panel-default panel panel-open'] option[selected='selected']");
        private readonly By DocumentsIpExp = By.CssSelector("div[class='panel-default panel panel-open'] tbody tr td:nth-child(3)");
        private readonly By DocumentsLastModifiedExp = By.CssSelector("div[class='panel-default panel panel-open'] tbody tr td:nth-child(4)");
        private readonly By FilesLastModifiedExp = By.CssSelector("div[class='panel-default panel panel-open'] td:nth-child(3)");

        #endregion Locator's

        public IDocumentsTabUI ClickOnSectionByTitle(string documentSectionName)
        {
            var section = By.XPath(string.Format(_documentSection, documentSectionName));
            _driver.SearchElement(section)
                .ForceClick(_driver, section);

            return this;
        }

        public Dictionary<string, string> GetDocumentTitles(string documentName, string documentStatus)
        {
            var titlesData = new Dictionary<string, string>();
            _driver.WaitForExactNumberOfElements(DocumentsNameExp, 1);
            var elements = _driver.SearchElements(DocumentsNameExp);

            var actualDocumentName = elements
                .Where(p => p.GetElementText(_driver)
                .Contains(documentName))
                .FirstOrDefault()
                .GetElementText(_driver);

            titlesData.Add("actualDocumentName", actualDocumentName);

            _driver.WaitForExactNumberOfElements(DocumentsListInClientCardExp, 1);

            var actualDocumentStatus = _driver.SearchElements(DocumentsListInClientCardExp)
                .Where(p => p.GetElementText(_driver)
                .Contains(documentName))
                .FirstOrDefault()?
                .FindElement(DocumentStatusExp)
                .GetElementText(_driver);

            titlesData.Add("actualDocumentStatus", actualDocumentStatus);
            _driver.WaitForExactNumberOfElements(DocumentsListInClientCardExp, 1);

            var documentIp = _driver.SearchElements(DocumentsListInClientCardExp)
                .Where(p => p.GetElementText(_driver)
                .Contains(documentName))
                .FirstOrDefault()?
                .FindElement(DocumentsIpExp)
                .GetElementText(_driver);

            titlesData.Add("documentIp", documentIp);
            _driver.WaitForExactNumberOfElements(DocumentsListInClientCardExp, 1);

            var documentLastModified = _driver.SearchElements(DocumentsListInClientCardExp) 
                .Where(p => p.GetElementText(_driver)
                .Contains(documentName))
                .FirstOrDefault()?
                .FindElement(DocumentsLastModifiedExp)
                .GetElementText(_driver);

            titlesData.Add("documentLastModified", documentLastModified);

            return titlesData;
        }

        public string GetFileTitles(string fileName)
        {
            _driver.WaitForExactNumberOfElements(DocumentsListInClientCardExp, 1);

            var actualFileName = _driver.SearchElements(DocumentsListInClientCardExp)   
                 .Where(p => p.GetElementText(_driver)
                 .Contains(fileName))
                 .FirstOrDefault()
                 .GetElementText(_driver);

            _driver.WaitForExactNumberOfElements(DocumentsListInClientCardExp, 1);

            var element = _driver.SearchElements(DocumentsListInClientCardExp)
                .Where(p => p.GetElementText(_driver)
                .Contains(fileName))
                .FirstOrDefault();

            var fileLastModified = element
                .FindElement(FilesLastModifiedExp)
                .GetElementText(_driver);

            var newString = actualFileName
                .Replace("\r\n", "")
                .TrimEnd()
                .TrimStart();

            return newString;
        }

        public string GetEnvelopeBoxBody()
        {
            return _driver.SearchElement(EnvelopeBoxBodyExp)
                .GetElementText(_driver, EnvelopeBoxBodyExp);
        }

        public T ChangeContext<T>(IWebDriver driver) where T : class
        {
            return _apiFactory.ChangeContext<T>(_driver);
        }

    }
}
