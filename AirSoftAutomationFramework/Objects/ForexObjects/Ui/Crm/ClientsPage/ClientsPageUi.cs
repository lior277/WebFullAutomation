using AirSoftAutomationFramework.Internals.DAL.ConfigurationManagement;
using AirSoftAutomationFramework.Internals.ExtensionsMethods;
using AirSoftAutomationFramework.Internals.Factory;
using AirSoftAutomationFramework.Internals.Helpers;
using AirSoftAutomationFramework.Objects.ForexObjects.ApiGeneral;
using AirSoftAutomationFramework.Objects.ForexObjects.TradingPlatform.TradeObjects.Ui;
using AirSoftAutomationFramework.Objects.ForexObjects.Ui.UiGeneral;
using AirSoftAutomationFramework.Objects.MgmObjects.Ui.Risk;
using OpenQA.Selenium;
using System.Linq;
using System.Threading;

namespace AirSoftAutomationFramework.Objects.ForexObjects.Ui.Crm.ClientsPage
{
    public class ClientsPageUi : IClientsPageUi
    {
        #region Members
        private IWebDriver _driver;
        private IHandleFiltersUi _handleFilters;
        private readonly IApplicationFactory _apiFactory;
        private string _mailPerfix = DataRep.EmailPrefix;
        private string _editFreeText = "div[class*='edit-office'] input[id='{0}']";
        private string _actionName = "//a[text()='{0}']";
        private string _rowInClientTable = "div[title='{0}']";
        private string _selectElement = "//div[@id='sales-agent-multiselect-container']//label[contains(.,'{0}')]";
        #endregion

        public ClientsPageUi(IHandleFiltersUi handleFilters,
            IApplicationFactory apiFactory, IWebDriver driver)
        {
            _handleFilters = handleFilters;
            _driver = driver;
            _apiFactory = apiFactory;
        }

        #region Locator's   
        private readonly By FastLoginBtnExp = By.CssSelector("table button[class*='fast-login']");
        private readonly By CustomersTableProcessingExp = By.CssSelector("div[id='customersTable_processing'][style='display: block;']");
        private readonly By ClientTableIdColumnExp = By.XPath("//th[text()='ID']");// By.CssSelector("table thead th[aria-label='ID']:not([style*='padding-top'])");
        private readonly By CustomersTableShowingCounterExp = By.CssSelector("div[id*='customersTable_info']");
        private readonly By CustomersTableTotalCounterExp = By.CssSelector("p[class*='clients-entries']");
        private readonly By EditFreeTextBtnExp = By.CssSelector("a[class*='freeTxtbtn']");
        private readonly By FreeTextIsDisableExp = By.CssSelector("fieldset[disabled]");
        private readonly By SaveBtnExp = By.CssSelector("div[class*='edit-office'] button[class='btn btn-success']");
        private readonly By UploadLeadErrorMessageExp = By.CssSelector("h4[class='alert alert-danger message-error']");
        private readonly By EventsButtonExp = By.CssSelector("button[class*='leads-events']");
        private readonly By ColumnsVisibilityBtnExp = By.CssSelector("button[class*='buttons-collection buttons-colvis']");
        private readonly By ImportButtonExp = By.CssSelector("button[class*='import-lead-open button-clients']");
        private readonly By CommentExp = By.CssSelector("textarea[name='commentText']");
        private readonly By MouthFilterExp = By.CssSelector("label[btnradio='month']");
        private readonly By PrevMouthFilterExp = By.CssSelector("label[btnradio='prvMonth']");
        private readonly By UploadInputExp = By.CssSelector("div[class*='import-content'] input[id='excel']");
        private readonly By MassEventRandomAssighSalesAgentExp = By.CssSelector("angular2-multiselect[id='randomAssign']");
        private readonly By MassEventSearchSalesAgentExp = By.CssSelector("angular2-multiselect[id='randomAssign'] input[class*='c-input']");
        private readonly By MassEventWithSelectSalesAgentAndCampaignTradeExp = By.CssSelector("div[class='modal-content modalStyle'] select");
        private readonly By MassEventWithSelectSalesStatus1Exp = By.CssSelector("div[class='modal-content'] select[name='choosedStatus']");
        private readonly By MassEventWithSelectSalesStatus2Exp = By.CssSelector("div[class='modal-content'] select[name='choosedStatus2']");
        private readonly By Status2CheckBoxExp = By.CssSelector("label[for='customers_show_status_2'] span");
        //private readonly By SelectStatusExp = By.XPath("//select[@name='choosedStatus']");
        private readonly By ClientsTableRowExp = By.CssSelector("table[id='customersTable'] tbody tr");
        private readonly By ConfirmCommentExp = By.CssSelector("button[class*= 'create-comment-confirm']");
        private readonly By PreviewConfirmationExp = By.CssSelector("button[class= 'btn btn-success custom-success confirm-action-btn ng-star-inserted']");
        private readonly By ConfirmDeleteCommentExp = By.XPath("//button[contains(., 'Yes')]");
        private readonly By ConfirmSalesAgentAndCampaignAndGroupExp = By.CssSelector("div[class='custom-button custom-modal-body'] button[class*='btn btn-success custom-success']");
        private readonly By ConfirmChangeGroupExp = By.CssSelector("div[class='modal-body custom-modal-body custom-button'] button[class*='btn btn-success custom-success confirm-action-btn']");
        private readonly By ConfirmSChangeStatusExp = By.CssSelector("select[name='choosedStatus2'] + div button[class*='change-sales-status-dropdown-confirm']");
        private readonly By CommentsButtonExp = By.CssSelector("i[class*='comments']");
        private readonly By ClientsCheckBoxExp = By.CssSelector("input[class*='headLeadsCheck']");
        private readonly By ClientsDataTableRowsExp = By.XPath("//table[contains(@class,'search-result-clients')]/tbody/tr/td[not(contains(@class,'dataTables_empty'))]/parent::tr");
        private readonly By ClientsTableSearchFiledExp = By.CssSelector("input[type='search'][aria-controls='customersTable']");
        private readonly By SelectCampaignlExp = By.CssSelector("select[class='select-map-statictics select-campaign']");
        private readonly By SelectSalesAgentlExp = By.CssSelector("select[class='select-sales-agent']");
        private readonly By SelectStatusExp = By.CssSelector("select[class='select-status-text']");
        private readonly By SelectTradeGroupExp = By.CssSelector("select[class='select-trade-group']");
        private readonly By FileUploadBtnExp = By.CssSelector("div[class*='import-content'] button[class*='import-button']");
        private readonly By ImportClientPopupExp = By.CssSelector("div[class*='import-content']");
        private readonly By PhoneIconBtnExp = By.CssSelector("a[title='Call Client']");
        private readonly By ActionAlertPopupExp = By.CssSelector("div[class='model fade in action-alert-auto']");
        private readonly By NumOfRowsInClientsTableExp = By.CssSelector("div[id='customersTable_info']");
        private readonly By WaitForClientsTableExp = By.CssSelector("div[id = 'customersTable_info']");
        private readonly By EditFreeTextFieldsClosedExp = By.CssSelector("div[class*='fade create-office-modal'][style='display: none;']");      
        private readonly By FiltersMenueBtnExp = By.CssSelector("button[class='filters-button']");
        private readonly By CreateAttributionRuleBtnExp = By.CssSelector("button[class*='new_attribution_rule']");
        #endregion Locator's     

        public ICreateClientUi ClickOnCreateClientButton()
        {
            //// wait for 26 is the num of clients per page
            //_driver.WaitForNumberOfElements(CommentsButtonExp, 26);
            //_driver.WaitForAtListNumberOfElements(ClientsDataTableRowsExp, 1);

            _driver.SearchElement(DataRep.CreateClientButtonExp)
                .ForceClick(_driver, DataRep.CreateClientButtonExp);

            return _apiFactory.ChangeContext<ICreateClientUi>(_driver);
        }

        public IAttributionRulePageUi ClickOnCreateAttributionRuleButton()
        {
            _driver.SearchElement(CreateAttributionRuleBtnExp)
                .ForceClick(_driver, CreateAttributionRuleBtnExp);

            return _apiFactory.ChangeContext<IAttributionRulePageUi>(_driver);
        }

        public IClientsPageUi WaitForClientsTableToLoad()
        {
            _apiFactory
                .ChangeContext<ISharedStepsGenerator>(_driver)
                .WaitForTableToLoad();

            return this;
        }

        public IClientsPageUi VerifyMessages(string message)
        {
            var alertExp = By.XPath(string.Format(DataRep.AlertOnFront, message));

            var text = _driver.SearchElement(alertExp)
                .GetElementText(_driver, alertExp);

            text.StringContains(message);

            return this;
        }

        public int WaitForNumOfRowsInClientsTable(int expectedNumOfRows)
        {
            return _driver
                .WaitForExactNumberOfElements(ClientsDataTableRowsExp, expectedNumOfRows, 70);
        }

        public IClientsPageUi CheckIfIdColumnExist()
        {
            _driver.SearchElement(ClientTableIdColumnExp);

            return this;
        }

        public ISearchResultsUi SetTimeFilter(string time)
        {
            _handleFilters.SetTimeFilter(time);

            return _apiFactory.ChangeContext<ISearchResultsUi>(_driver);
        }

        public ISearchResultsUi SetTableColumnVisibility(string columnNAme)
        {
            _handleFilters.SetTableColumnVisibility(columnNAme);

            return _apiFactory.ChangeContext<ISearchResultsUi>(_driver);
        }

        public IClientsPageUi WaitForTableToLoad()
        {
            _driver.SearchElement(WaitForClientsTableExp, 70);

            return this;
        }

        public ISearchResultsUi SearchClientByEmail(string clientMail)
        {
            var crmUrl = Config.appSettings.CrmUrl;

            if (!clientMail.Contains('@'))
            {
                clientMail += _mailPerfix;
            }

            //if (!_driver.Url.Contains("/crm/clients"))
            //{
            //    _driver.NavigateToPageByName(crmUrl, "/crm/clients");
            //}

            //for (var i = 0; i < 50; i++)
            //{
            //    if (!_driver.Url.Contains("/crm/clients"))
            //    {
            //        Thread.Sleep(100);

            //        continue;
            //    }

            //    break;
            //}

            var rowInClientTableExp = By.CssSelector(string.Format(_rowInClientTable, clientMail.ToLower()));
            _driver.SearchElement(rowInClientTableExp, 60);
            _driver.SearchElement(ClientsTableSearchFiledExp, 60);
            var element = _driver.SearchElement(ClientsTableSearchFiledExp, 60);
            var tempClientMail = clientMail.UpperCaseFirstLetter();
            element.SendsKeysCharByChar(_driver, ClientsTableSearchFiledExp, tempClientMail, 60);

            return _apiFactory.ChangeContext<ISearchResultsUi>(_driver);
        }

        public IClientsPageUi ClickOnFastLoginBtn()
        {
            _driver.OpenNewWindowAndSwitch(FastLoginBtnExp, "trade");

            return this;
        }

        public IClientsPageUi ClickOnEditFreeTextBtn()
        {
            _driver.SearchElement(EditFreeTextBtnExp)
                .ForceClick(_driver, EditFreeTextBtnExp);

            return this;
        }

        public IClientsPageUi SetFreeText(string freeTextFiled, string text)
        {
            var editFreeTextExp = By.CssSelector(string.Format(_editFreeText, freeTextFiled));

            _driver.SearchElement(editFreeTextExp)
                .SendsKeysAuto(_driver, editFreeTextExp, text);

            return this;
        }

        public IClientsPageUi ClickOnSaveFreeTextButton()
        {
            _driver.SearchElement(SaveBtnExp)
                .ForceClick(_driver, SaveBtnExp);

            _driver.SearchElement(EditFreeTextFieldsClosedExp);

            return this;
        }

        public string GetFreeText(string freeTextFiled)
        {
            var editFreeTextExp = By.CssSelector(string.Format(_editFreeText, freeTextFiled));
            var freeText = _driver.SearchElement(editFreeTextExp)
                .GetElementText(_driver, editFreeTextExp);

            freeText ??= _driver.SearchElement(editFreeTextExp)
                    .GetElementText(_driver, editFreeTextExp);

            return freeText;
        }

        public int CheckIfFreeTextIsDisable()
        {
            return _driver.SearchElements(FreeTextIsDisableExp).Count;
        }

        private IClientsPageUi EventsHandlingPipe(string actionName)
        {
            //_driver.WaitForAnimationToLoad(); // for firefox
            ClickOnClientCheckbox();
            ClickOnEventButton();
            ClickOnAction(actionName);

            return this;
        }

        private IClientsPageUi ClickOnEventButton()
        {
            _driver.SearchElement(EventsButtonExp)
                .ForceClick(_driver, EventsButtonExp);

            return this;
        }

        public bool CheckIfEventsButtonExist()
        {
            _driver.WaitForElementNotExist(EventsButtonExp);

            return false;
        }

        public bool CheckIfColumnsVisibilityButtonExist()
        {
            _driver.WaitForElementNotExist(ColumnsVisibilityBtnExp);

            return false;
        }

        public IClientsPageUi ClickOnImportButton()
        {
            _driver.SearchElement(ImportButtonExp)
                .ForceClick(_driver, ImportButtonExp);

            _driver.WaitForAnimationToLoad(300);

            return this;
        }

        public bool CheckIfImportButtonExist()
        {
            _driver.WaitForElementNotExist(ImportButtonExp);

            return false;
        }

        public IClientsPageUi ClickOnPhoneIconButton()
        {
            _driver.SearchElement(PhoneIconBtnExp)
                .ForceClick(_driver, PhoneIconBtnExp);

            return this;
        }

        public IClientsPageUi ClickOnConfimationCallPoUp()
        {
            Thread.Sleep(100);
            _driver.SearchElement(ConfirmChangeGroupExp)
                .ForceClick(_driver, ConfirmChangeGroupExp);

            return this;
        }

        public IClientsPageUi VerifyPhoneCallAnimation()
        {
            _driver.SearchElement(DataRep.PhoneCallAnimationExp);

            return this;
        }

        public IClientsPageUi VerifyNoPhoneCallButton()
        {
            _driver.WaitForElementNotExist(PhoneIconBtnExp);

            return this;
        }

        public IClientsPageUi UploadCsvFile(string filePath)
        {
            _apiFactory
                .ChangeContext<ISharedStepsGenerator>(_driver)
                .UploadFileOnGrid(UploadInputExp, filePath);

            return this;
        }

        public IClientsPageUi SelectCampaign(string campaignName)
        {
            _driver.SearchElement(SelectCampaignlExp)
                .SelectElementFromDropDownByText(_driver, SelectCampaignlExp, campaignName);

            return this;
        }

        public IClientsPageUi SelectTradeGroup(string tradeGroupName)
        {
            _driver.SearchElement(SelectTradeGroupExp)
                .SelectElementFromDropDownByText(_driver, SelectTradeGroupExp, tradeGroupName);

            return this;
        }

        public IClientsPageUi SelectSalesAgent(string salesAgentName)
        {
            _driver.SearchElement(SelectSalesAgentlExp)
                .SelectElementFromDropDownByText(_driver, SelectSalesAgentlExp, salesAgentName);

            return this;
        }

        public IClientsPageUi SelectStatus(string statusName)
        {
            _driver.SearchElement(SelectStatusExp)
                .SelectElementFromDropDownByText(_driver, SelectStatusExp, statusName);

            return this;
        }

        public IClientsPageUi ClickOnUploadFileButton()
        {
            _driver.SearchElement(FileUploadBtnExp)
                .ForceClick(_driver, FileUploadBtnExp);

            _driver.WaitForElementNotExist(ImportClientPopupExp);

            return this;
        }

        private IClientsPageUi ClickOnClientCheckbox()
        {
            _driver.WaitForExactNumberOfElements(DataRep.ChooseClientCheckBoxExp, 2);

            _driver.SearchElements(DataRep.ChooseClientCheckBoxExp)
                .First()
                .ForceClick(_driver, DataRep.ChooseClientCheckBoxExp);

            return this;
        }

        public string GetErrorUploadMessage()
        {
            return _driver.SearchElement(UploadLeadErrorMessageExp)
               .GetElementText(_driver);
        }

        private IClientsPageUi ClickOnAction(string actionName)
        {
            var actionNameExp = By.XPath(string.Format(_actionName,
              $" {actionName} "));

            _driver.SearchElement(actionNameExp)
                .ForceClick(_driver, actionNameExp);

            return this;
        }

        private void ClickOnOkButton(By by)
        {
            _driver.SearchElement(by)
                .WaitElementToStopMoving(_driver, by)
                .ForceClick(_driver, by);
            //element.ForceClick(_driver, by);
        }

        private void ClickOnConfirmButton(By by)
        {
            _driver.SearchElement(by)
                .WaitElementToStopMoving(_driver, by)
                .ForceClick(_driver, by);
        }

        public void AddComment(string comment)
        {
            EventsHandlingPipe("Add comment");

            _driver.SearchElement(CommentExp)
               .SendsKeysAuto(_driver, CommentExp, comment);

            ClickOnOkButton(ConfirmCommentExp);
            //Thread.Sleep(500);
            ClickOnConfirmButton(PreviewConfirmationExp);
            //Thread.Sleep(500);
        }

        public IClientsPageUi DeleteComment()
        {
            EventsHandlingPipe("Delete comment");
            _driver.SearchElement(ConfirmDeleteCommentExp)
                .WaitElementToStopMoving(_driver, ConfirmDeleteCommentExp)
                .ForceClick(_driver, ConfirmDeleteCommentExp);
            //element.ForceClickWithRetry(_driver, ConfirmDeleteCommentExp);
            //_driver.Navigate().Refresh();

            return this;
        }

        public IClientsPageUi ClientAssignmentPipe(string salesAgentName, string salesStatus = "New")
        {
            EventsHandlingPipe("Client assignment");

            // click to open sales agent select
            _driver.SearchElement(MassEventWithSelectSalesAgentAndCampaignTradeExp)
                .ForceClick(_driver, MassEventRandomAssighSalesAgentExp);

            // search sales agent
            _driver.SearchElement(MassEventSearchSalesAgentExp)
                .SendsKeysAuto(_driver, MassEventSearchSalesAgentExp, salesAgentName);

            // select the sales agent
            SelectElementMultiSelect(salesAgentName);

            // click to close sales agent select
            _driver.SearchElement(MassEventWithSelectSalesAgentAndCampaignTradeExp)
                .ForceClick(_driver, MassEventRandomAssighSalesAgentExp);

            _driver.SearchElement(MassEventWithSelectSalesAgentAndCampaignTradeExp)
                .SelectElementFromDropDownByText(_driver,
                MassEventWithSelectSalesAgentAndCampaignTradeExp, salesStatus);

            ClickOnOkButton(ConfirmSalesAgentAndCampaignAndGroupExp);
            //Thread.Sleep(500);
            ClickOnConfirmButton(PreviewConfirmationExp);
            _driver.WaitForElementNotExist(ActionAlertPopupExp);

            return this;
        }

        private IClientsPageUi SelectElementMultiSelect(string value)
        {
            var SelectElementExp = By.XPath(string.Format(_selectElement, value));

            _driver.SearchElement(SelectElementExp)
                .ForceClick(_driver, SelectElementExp);

            return this;
        }

        public IClientsPageUi ChangeCampaign(string campaignName)
        {
            EventsHandlingPipe("Change campaign");

            _driver.SearchElement(MassEventWithSelectSalesAgentAndCampaignTradeExp)
                .SelectElementFromDropDownByText(_driver,
                MassEventWithSelectSalesAgentAndCampaignTradeExp, campaignName);

            ClickOnOkButton(ConfirmSalesAgentAndCampaignAndGroupExp);
            //Thread.Sleep(500);
            //_driver.RetryClickTillElementNotDisplay(PreviewConfirmationExp);
            //_driver.Navigate().Refresh();
            _driver.SearchElement(PreviewConfirmationExp)
                .WaitElementToStopMoving(_driver, PreviewConfirmationExp)
                .ClickUntilElementNotExist(_driver);

            return this;
        }

        public IClientsPageUi ChangeStatusAndStatus2(string status1, string status2)
        {
            EventsHandlingPipe("Change status");

            // change status 1
            _driver.SearchElement(MassEventWithSelectSalesStatus1Exp)
               .SelectElementFromDropDownByText(_driver, MassEventWithSelectSalesStatus1Exp, status1);

            // check status 2
            _driver.SearchElement(Status2CheckBoxExp)
                .ForceClick(_driver, Status2CheckBoxExp);

            // change status 2
            _driver.SearchElement(MassEventWithSelectSalesStatus2Exp)
                .SelectElementFromDropDownByText(_driver, MassEventWithSelectSalesStatus2Exp, status2);

            ClickOnOkButton(ConfirmSChangeStatusExp);
            //Thread.Sleep(500);
            ClickOnConfirmButton(PreviewConfirmationExp);

            //_driver.SearchElement(PreviewConfirmationExp)
            //   .ClickUntilElementNotExist(_driver);

            return this;
        }

        public IClientsPageUi ChangeTradeGroup(string group)
        {
            EventsHandlingPipe("Change trading group");

            _driver.SearchElement(MassEventWithSelectSalesAgentAndCampaignTradeExp)
                .SelectElementFromDropDownByText(_driver,
                MassEventWithSelectSalesAgentAndCampaignTradeExp, group);

            ClickOnOkButton(ConfirmSalesAgentAndCampaignAndGroupExp);
            //var element = _driver.SearchElement(PreviewConfirmationExp);           
            Thread.Sleep(500);
            ClickOnConfirmButton(PreviewConfirmationExp);
            //var element = _driver.SearchElement(PreviewConfirmationExp)
            //    .WaitElementToStopMoving(_driver, PreviewConfirmationExp)
            //    .ForceClick(_driver, PreviewConfirmationExp);
            //_driver.RetryClickTillElementNotDisplay(ConfirmDeleteCommentExp);

            return this;
        }

        public IClientsPageUi PerformMassAsignPipe(string comment, string salesAgent,
            string campaign, string status1, string status2, string tradeGroup)
        {
            AddComment(comment);
            DeleteComment();
            ClientAssignmentPipe(salesAgent);
            ChangeCampaign(campaign);
            ChangeStatusAndStatus2(status1, status2);
            ChangeTradeGroup(tradeGroup);

            return this;
        }

        public T ChangeContext<T>(IWebDriver driver) where T : class
        {
            return _apiFactory.ChangeContext<T>(_driver);
        }
    }
}
