// Ignore Spelling: Forex api

using AirSoftAutomationFramework.Internals.DAL.Logger;
using AirSoftAutomationFramework.Internals.ExtensionsMethods;
using AirSoftAutomationFramework.Internals.Helpers;
using AirSoftAutomationFramework.Objects.ForexObjects.ApiGeneral;
using AirSoftAutomationFramework.Objects.ForexObjects.Ui.Cfd;
using AirSoftAutomationFramework.Objects.ForexObjects.Ui.Crm.Accounts.RolesPage;
using AirSoftAutomationFramework.Objects.ForexObjects.Ui.Crm.Accounts.UsersMenuUi;
using AirSoftAutomationFramework.Objects.ForexObjects.Ui.Crm.ClientsPage;
using AirSoftAutomationFramework.Objects.ForexObjects.Ui.Crm.ClientsPage.ClientCard;
using HtmlAgilityPack;
using NUnit.Framework;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using AirSoftAutomationFramework.Internals.Factorys;

namespace AirSoftAutomationFramework.Objects.ForexObjects.Ui.UiGeneral
{
    public class SearchResultsUi : ISearchResultsUi
    {
        #region Members
        private By RowsExp;
        //private By CellsExp;
        private By TitleValuesExp;
        private IWriteToFile _writeToFile;
        private IWebDriver _driver;
        private string _tableType;
        private ISearchResultsFactory _searchResultsFactory;
        private string _recentActivitiesName = "//h5[contains(.,'{0}')]/ancestor::div[@class='media']" +
            " | //span[contains(.,'{0}')]/ancestor::div[@class='media']";

        private string _recentActivitiesFullName = "//div[@class='media']//child::span[contains(.,'{0}')]";
        private string _recentActivitiesComment = "//div[@class='media']//child::span[contains(.,'{0}')]";
        private string _rows = "//table[contains(@class,'{0}')]/tbody/tr/td[not(contains(@class,'dataTables_empty'))]/parent::tr";
        private string _rowsForTrade = "//table[contains(@class,'{0}')]/tbody/tr[@class='accordion-toggle']/td[not(contains(@class,'dataTables_empty'))]/parent::tr";
        private string _rowsTranslate = "//table[contains(@class,'{0}')]/tbody/tr";
        private string _rowTranslate = "[contains(., '{1}')]";
        private string _cell = "//table[contains(@class,'{0}')]/tbody/tr/td"; //[not(contains(@class,'ng-star-inserted'))]";
        private string _titles = "(//table[contains(@class,'{0}')])[1]/thead/tr/th"; //[text()]";
                                                                                     //"//table[contains(@class,'{0}')]/thead/tr/th[text()]";
        private string _date = DateTime.Now.ToString("dd/MM/yyyy");
        private string _mailPerfix = DataRep.EmailPrefix;

        private readonly IApplicationFactory _apiFactory;
        #endregion

        public SearchResultsUi(IWriteToFile writeToFile, ISearchResultsFactory searchResultsFactory,
            IApplicationFactory apiFactory, IWebDriver driver)
        {
            _writeToFile = writeToFile;
            _searchResultsFactory = searchResultsFactory;
            _driver = driver;
            _apiFactory = apiFactory;
        }

        #region Locator's       
        private readonly By EditUserButtonExp = By.CssSelector(
            "button[class*='edit-user']");

        private readonly By ClientsDataTableRowsExp = By.XPath(
            "//table[contains(@class,'search-result-clients')]" +
            "/tbody/tr/td[not(contains(@class,'dataTables_empty'))]/parent::tr");


        private readonly By EditGroupButtonExp = By.CssSelector(
            "button[class*='btn btn-warning']");

        private readonly By FirstDepositFlagExp = By.CssSelector(
           "i[class*='flag-checkered']");

        private readonly By TableFullNameColumnExp = By.CssSelector(
            "a[class*='search-result-full-name']");

        private readonly By TableRowExp = By.XPath(
            "//table[@id='users']/tbody/tr " +
            "| //table[@id='customersTable']/tbody/tr " +
            "| //table[contains(@id,'DataTables')]/tbody/tr"); // need check

        private readonly By TableCellsExp = By.XPath("//table[@id='users']/tbody/tr " +
           "| //table[@id='customersTable']/tbody/tr " +
            "| //table[contains(@id,'DataTables')]/tbody/tr"); // need check

        private readonly By EditRoleButtonExp = By.CssSelector("button[class*='edit-role-btn']");

        private readonly By DeleteRoleButtonExp = By.CssSelector(
            "button[class*='delete-role-btn']");

        private readonly By SelectSalesAgentlExp = By.XPath(
            "//select[contains(@class,'select-sales-agent')]");

        private readonly By SelectCampaignlExp = By.XPath(
           "//select[contains(@class,'select-campaign')]");

        private readonly By CustomersTableProcessingExp = By.CssSelector(
            "div[class*='dataTables'] div[style='display: block;']");

        private readonly By CustomersTableFullNameExp = By.XPath(
            "//table[contains(@class,'search-result-clients')]" +
            "/tbody/tr/td/a[contains(@class, 'full-name')]");

        private readonly By RecentActivitiesDateExp = By.XPath("//span[contains(@class,'timeline-date')]");

        private readonly By LastRegistrationEntriesExp = By.XPath(
          "//table[contains(@class,'last-registration-table')]/tbody/tr");

        private readonly By LastRegistrationFullNameExp = By.CssSelector(
          "a[class*='getClient']");

        private readonly By LastRegistrationEntryValuesExp = By.XPath(
         "//table[contains(@class,'last-registration-table')]/tbody/tr/td[contains(text(),'{0}')]");

        private readonly By LastRegistrationEmailExp = By.XPath(
        "//table[contains(@class,'last-registration-table')]/tbody/tr//td[contains(text(),'{0}')]");

        private readonly By DashboardDepositTableValuesExp = By.CssSelector(
            "table[id='depositDashTable'] td");
        #endregion Locator's

        public IUserUi ClickOnEditUserButton()
        {
            _driver.SearchElement(EditUserButtonExp)
                .ForceClick(_driver, EditUserButtonExp);

            return _apiFactory.ChangeContext<IUserUi>(_driver);
        }

        public ITradeGroupCardUi ClickOnEditGroupButton()
        {
            _driver.SearchElement(EditGroupButtonExp)
                .ForceClick(_driver, EditGroupButtonExp);

            return _apiFactory.ChangeContext<ITradeGroupCardUi>(_driver);
        }

        public ISearchResultsUi VerifyEmptyTable()
        {
            _driver.WaitForExactNumberOfElements(DataRep.DataTablesEmptyExp, 1);

            return _apiFactory.ChangeContext<ISearchResultsUi>(_driver);
        }

        public IRoleUi ClickOnEditRoleButton()
        {
            _driver.WaitForExactNumberOfElements(TableRowExp, 1, fromSeconds: 60);
            _driver.ClickOnElementWithAnimation(EditRoleButtonExp, 60);

            return _apiFactory.ChangeContext<IRoleUi>(_driver);
        }

        public IRolesPageUi ClickOnDeleteRoleButton()
        {
            _driver.WaitForExactNumberOfElements(TableRowExp, 1);
            _driver.ClickOnElementWithAnimation(DeleteRoleButtonExp);
            //_driver.WaitForAnimationToLoad(100);
            _driver.SearchElement(DataRep.ConfirmExp)
                .ForceClick(_driver, DataRep.ConfirmExp);

            return _apiFactory.ChangeContext<IRolesPageUi>(_driver);
        }

        public IClientCardUi ClickOnClientFullName(string clientName = null)
        {
            _apiFactory
               .ChangeContext<ISharedStepsGenerator>(_driver)
               .WaitForTableToLoad();

            //_driver.WaitForNumberOfElements(TableRowExp, 1);

            _apiFactory
                .ChangeContext<ISharedStepsGenerator>(_driver)
                .WaitForTableToLoad();

            _driver.SearchElement(TableFullNameColumnExp, 60)
                .ForceClick(_driver, TableFullNameColumnExp);

            return _apiFactory.ChangeContext<IClientCardUi>(_driver);
        }

        public IClientsPageUi SelectSalesAgent(string agentName)
        {
            _driver.WaitForExactNumberOfElements(TableRowExp, 1);

            _driver.SearchElement(SelectSalesAgentlExp)
                .SelectElementFromDropDownByText(_driver, SelectSalesAgentlExp, agentName);

            return _apiFactory.ChangeContext<IClientsPageUi>(_driver);
        }

        public IClientsPageUi ConnectClientToCampaign(string campaignName)
        {
            _apiFactory
            .ChangeContext<ISharedStepsGenerator>(_driver)
            .WaitForTableToLoad();

            _driver.WaitForAtListNumberOfElements(ClientsDataTableRowsExp, 1);

            _driver.SearchElement(SelectCampaignlExp)
                .SelectElementFromDropDownByText(_driver, SelectCampaignlExp, campaignName);


            //_driver.SearchElement(SelectCampaignlExp)
            //    .ConnectClientToCampaign(_driver, SelectCampaignlExp, campaignName, 60);

            return _apiFactory.ChangeContext<IClientsPageUi>(_driver);
        }

        public ISearchResultsUi VerifyFirstDepositFlag()
        {
            //RowsExp = By.XPath(string.Format(_rows, _tableType));
            //_driver.WaitForNumberOfElements(RowsExp, 1);

            _driver.SearchElement(FirstDepositFlagExp, 150);

            return this;
        }

        public ISearchResultsUi VerifyRegistrationByTime()
        {
            _driver.WaitForExactNumberOfElements(TableRowExp, 1);

            _driver.SearchElement(FirstDepositFlagExp, 150);

            return this;
        }

        public List<string> GetClientsFullName()
        {
            var names = new List<string>();
            //_driver.WaitForNumberOfElements(TableRowExp, 2);
            _driver.WaitForExactNumberOfElements(CustomersTableFullNameExp, 2, 120);
            var elements = _driver.SearchElements(CustomersTableFullNameExp);

            foreach (var element in elements)
            {
                names.Add(element.Text.ToLower().Split(' ')[0]);
            }

            return names;
        }

        public IDashboardPageUi VerifyRecentActivities(string userName = null, string clientName = null, string comment = null)
        {
            IWebElement recentActivitiesElement = null;

            // verify the user or the client name
            if (userName != null)
            {
                var nameExp = By.XPath(string.Format(_recentActivitiesName, $"{userName}"));
                Thread.Sleep(500);
                recentActivitiesElement = _driver.SearchElement(nameExp);
                Thread.Sleep(500);
            }
            else
            {
                var clientNameExp = By.XPath(string.Format(_recentActivitiesFullName,
                    $"{clientName.UpperCaseFirstLetter()} {clientName.UpperCaseFirstLetter()}"));

                recentActivitiesElement = _driver.SearchElement(clientNameExp);

                recentActivitiesElement
                    .GetElementText(_driver, clientNameExp)
                    .StringContains(clientName);
            }

            var commentExp = By.XPath(string.Format(_recentActivitiesComment, $"{comment}"));
            recentActivitiesElement = _driver.SearchElement(commentExp);

            recentActivitiesElement
                .GetElementText(_driver, commentExp)
                .StringContains(comment);

            // verify the comment


            //recentActivitiesElement.FindElement(commentExp).GetElementText(_driver, commentExp)
            //    .StringContains(comment);

            //var tempDate = recentActivitiesElement.FindElement(RecentActivitiesDateExp)
            //    .GetElementText(_driver);

            return _apiFactory.ChangeContext<IDashboardPageUi>(_driver);
        }

        public ISearchResultsUi VerifyLastRegistration(string clientName, string country, string phone)
        {
            var email = clientName + _mailPerfix.ToLower();
            var path = _rowsTranslate + _rowTranslate;
            var clientNameExp = By.XPath(string.Format(path, "last-registration-table", clientName));
            var countryExp = By.XPath(string.Format(path, "last-registration-table", country));
            var phoneExp = By.XPath(string.Format(path, "last-registration-table", phone.ToLower()));
            var emailExp = By.XPath(string.Format(path, "last-registration-table", email));

            var lastRegistrationElement = _driver.SearchElement(clientNameExp);
            lastRegistrationElement.FindElement(countryExp);
            lastRegistrationElement.FindElement(phoneExp);
            lastRegistrationElement.FindElement(emailExp);

            return this;
        }

        public ISearchResultsUi VerifyDepositTableDashboard(string clientName, int amount)
        {
            var depositElements = _driver.SearchElements(DashboardDepositTableValuesExp);
            var depositElementsText = new List<string>();

            foreach (var element in depositElements)
            {
                depositElementsText.Add(element.GetElementText(_driver));
            }

            depositElementsText[0].StringContains(depositElementsText[0]);
            depositElementsText[1].StringContains(clientName.UpperCaseFirstLetter());
            depositElementsText[2].StringContains($"{amount} USD");
            depositElementsText[3].StringContains(_date);

            return this;
        }

        public IList<T> GetSearchResultDetails<T>(string tableName = null,
            string additionalData = null, int expectedNumOfRows = 1,
            bool checkNumOfRows = true, bool cellsAndTitlesShouldBeEquals = true)
        {
            #region Locator's    
            _tableType = typeof(T).Name.StringAddCharBetweenWords('-');

            if (tableName == "trade")
            {
                RowsExp = By.XPath(string.Format(_rowsForTrade, _tableType));
            }
            else
            {
                RowsExp = By.XPath(string.Format(_rows, _tableType));
            }

            var cellXpathExp = By.XPath(string.Format(_cell, _tableType));
            var cellXpath = string.Format(_cell, _tableType);
            TitleValuesExp = By.XPath(string.Format(_titles, _tableType));
            #endregion

            var rows = new List<IWebElement>();
            var titles = new List<IWebElement>();
            var cells = new List<IWebElement>();
            var htmlDocument = new HtmlDocument();
            var source = _driver.PageSource;
            htmlDocument.LoadHtml(source);

            var cellsTemp = htmlDocument.DocumentNode
                .SelectNodes(cellXpath)?
                .ToList();

            for (var i = 0; i < 15; i++)
            {
                if (cellsTemp == null)
                {
                    source = _driver.PageSource;
                    htmlDocument.LoadHtml(source);

                    cellsTemp = htmlDocument.DocumentNode
                        .SelectNodes(cellXpath)?
                        .ToList();

                    Thread.Sleep(500);

                    continue;
                }

                break;
            }

            if (cellsTemp == null)
            {
                var exceMessage = $" there are no rows in {tableName} table";

                throw new NullReferenceException(exceMessage);
            }
            if (checkNumOfRows)
            {
                _driver.WaitForExactNumberOfElements(RowsExp, expectedNumOfRows,
                    30, additionalData);

                rows = _driver.SearchElements(RowsExp).ToList();

                if (cellsAndTitlesShouldBeEquals)
                {
                    for (var i = 0; i < 4; i++)
                    {
                        cells = _driver.SearchElements(cellXpathExp).ToList();
                        titles = _driver.SearchElements(TitleValuesExp).ToList();

                        if (titles.Count != cells.Count)
                        {
                            if (titles.Count == cells.Count - 1)
                            {
                                break;
                            }
                        }
                        else
                        {
                            break;
                        }

                        Thread.Sleep(500); 
                    }

                    if (titles.Count != cells.Count)
                    {
                        var exceMessage = $" num of rows: {rows.Count}, num of cells:" +
                            $" {cells.Count}, num of titles: {titles.Count}";

                        var exception = new Exception(exceMessage);

                        throw exception;
                    }
                }
            }
            else
            {
                rows = _driver.WaitForAtListNumberOfElements(RowsExp, expectedNumOfRows);
            }

            rows = _driver.SearchElements(RowsExp).ToList();
            titles = _driver.SearchElements(TitleValuesExp).ToList();
            var instaceList = new List<T>();
            string titleText = null;
            var cellsCount = 0;

            try
            {
                for (var i = 0; i < rows.Count; i++)
                {
                    instaceList.Add(_searchResultsFactory.InstanceFactory<T>());

                    var properties = instaceList[i]
                        .GetType()
                        .GetProperties();

                    for (var j = 0; j < titles.Count(); j++)
                    {
                        var tempTitleText = _driver.SearchElements(TitleValuesExp)
                            .ElementAt(new Index(j))
                            .GetElementText(_driver);

                        titleText = Regex.Replace(tempTitleText, @"\s+", "")?
                            .Trim(':')?
                            .Replace("|", "")?
                            .ToLower();

                        var property = properties
                            .Where(p => p.Name.Equals(titleText))
                            .FirstOrDefault();      

                        var cellsText = new List<string>();

                        cellsTemp.ForEach(p => cellsText
                        .Add(p.InnerText.RemoveNewLine().RemoveMultipleSpaces().Trim()));

                        #region 
                        //cellsText.ForEach(p => cellsTextNew.Add(p.RemoveMultipleSpaces()));

                        //cellText = _driver.SearchElements(CellsExp)
                        //    .ElementAt(new Index(cellsCount))
                        //    .GetElementText(_driver, fromSeconds: 20)
                        //    .Trim();

                        // Console.WriteLine($"Cell Text: {cellText}");

                        //.Where(p => p.Equals(cellsCount))
                        //.FirstOrDefault();

                        //cellText = _driver.SearchElements(CellsExp)
                        //    .Where(p => p.Equals(cellsCount))
                        //    .FirstOrDefault()
                        //    .GetElementText(_driver)
                        //    .Trim();

                        //cellText = cells[cellsCount]
                        //    .GetElementText(_driver)
                        //    .Trim();
                        #endregion

                        property?.SetValue(instaceList[i], 
                            cellsText.ElementAt(new Index(cellsCount)));

                        cellsCount++;                      
                    }
                }
            }
            catch (Exception ex)
            {
                var exceMessage = $"  title Text: {titleText}" +
                    $" num of rows: {rows.Count}, num of cells:" +
                    $" {cells.Count}, num of titles: {titles.Count}, Exception: {ex.Message}";

                var exception = new Exception(exceMessage);

                //throw exception;
            }

            if (instaceList == null)
            {
                Console.WriteLine($"instance List is null");
            }

            return instaceList;
        }

        public T ChangeContext<T>(IWebDriver driver) where T : class
        {
            return _apiFactory.ChangeContext<T>(_driver);
        }
    }
}
