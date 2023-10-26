// Ignore Spelling: admin Timeline

using AirSoftAutomationFramework.Internals.ExtensionsMethods;
using AirSoftAutomationFramework.Internals.Factory;
using AirSoftAutomationFramework.Internals.Helpers;
using AirSoftAutomationFramework.Objects.ForexObjects.Ui.Crm.ClientsPage.ClientCard.FinancesTab;
using OpenQA.Selenium;

namespace AirSoftAutomationFramework.Objects.ForexObjects.Ui.Crm.ClientsPage.ClientCard
{
    public class ClientCardUi : IClientCardUi
    {
        private IWebDriver _driver;
        private readonly IApplicationFactory _apiFactory;

        #region Locator's      
        private readonly By CloseClientCardBtnExp = By.CssSelector(
            "div[class*='boxIn'] button[class='close pull-right']");

        private readonly By FastLoginBtnExp = By.CssSelector("div[class='header-buttons'] button[class*='fast-login']");
        private readonly By BlockedBtnExp = By.CssSelector("label[btnradio='Blocked']");
        private readonly By ClientCardOpenExp = By.CssSelector("div[id='customerModal'][style='display: block;']");
        private readonly By ChatDisableBtnExp = By.CssSelector("button[class*='cant-push-chat-button']");
        private readonly By SetCurrencyExp = By.CssSelector("select[class*='set-currency']");
        private readonly By OkBtnExp = By.CssSelector("div[class*='error-confirm-button'] button[class='btn btn-success']");
        private readonly By TimelineTabExp = By.CssSelector("a[id='timeline-tab-link']");
        private readonly By ExportEmailExp = By.CssSelector("input[name='export_user_email']");
        private readonly By SendEmailBtnExp = By.CssSelector("div[class='modal-content modalStyle'] button[class*='confirm-action']");
        private readonly By DocumentsTabExp = By.CssSelector("a[id='documents-tab-link']");
        private readonly By CaptureTabExp = By.CssSelector("a[id='capture-tab-link']");
        private readonly By FileTabExp = By.CssSelector("a[id='files-tab-link']");
        private readonly By FinanceTabExp = By.CssSelector("a[id='finance-tab-link']");
        private readonly By TradeTabExp = By.CssSelector("a[id='trade-tab-link']");
        private readonly By SavingAccountTabExp = By.CssSelector("a[id='sa-tab-link']");
        private readonly By CommentsTabExp = By.CssSelector("a[id='comments-tab-link']");
        private readonly By BlockPopUpTitleExp = By.XPath("//h4[contains(.,'Client is blocked')]");
        private readonly By InformationTabExp = By.CssSelector("a[id='client-info-tab-link']");
        private readonly By TimeLineLoaderExp = By.CssSelector("div[class='modal-header error-modal-header']");
        private readonly By PhoneIconBtnExp = By.CssSelector("i[class*='phone icon-client']");
        private readonly By ExportBtnExp = By.CssSelector("button[class*='export']");
        public static By SaveCustomerExp = By.CssSelector("button[class='btn btn-success save-customer']");
        public static By SuccessSaveCustomerExp = By.CssSelector("i[class*='saveCompleted']");

        private readonly By SelectTrunkExp = By.XPath(
            "//select[contains(@class,'choose-sales-agent')]");
        #endregion Locator's

        public ClientCardUi(IApplicationFactory apiFactory, IWebDriver driver)
        {
            _driver = driver;
            _apiFactory = apiFactory;
        }

        public IClientCardUi VerifySetCurrency(bool exist)
        {
            if (exist)
            {
                _driver.SearchElement(SetCurrencyExp);
            }
            else
            {
                _driver.WaitForElementNotExist(SetCurrencyExp, 40);
            }

            return this;
        }

        public ITimelineTabUi ClickOnTimelineTab()
        {
            _driver.SearchElement(TimelineTabExp)
               .ForceClick(_driver, TimelineTabExp);

            return _apiFactory.ChangeContext<ITimelineTabUi>(_driver);
        }

        public IClientCardUi SetEmailForExport(string adminEmail)
        {
            _driver.SearchElement(ExportEmailExp)
               .SendsKeysAuto(_driver, ExportEmailExp, adminEmail);

            return this;
        }

        public IClientCardUi ClickOnSendBtn()
        {
            _driver.SearchElement(SendEmailBtnExp)
               .ForceClick(_driver, SendEmailBtnExp);

            return this;
        }

        public IDocumentsTabUI ClickOnDocumentsTab()
        {
            _driver.SearchElement(DocumentsTabExp)
               .ForceClick(_driver, DocumentsTabExp);

            return _apiFactory.ChangeContext<IDocumentsTabUI>(_driver);
        }

        public IClientCardUi ClickOnExportButton()
        {
            _driver.SearchElement(ClientCardOpenExp);

            _driver.SearchElement(ExportBtnExp)
               .ForceClick(_driver, ExportBtnExp);

            return this;
        }

        public IClientCardUi ClickOnOkButton()
        {
            _driver.SearchElement(OkBtnExp)
               .ForceClick(_driver, OkBtnExp);

            return this;
        }

        public IClientCardUi ClickOnPhoneIconButton()
        {
            _driver.SearchElement(PhoneIconBtnExp)
                .ForceClick(_driver, PhoneIconBtnExp);

            return this;
        }

        public IClientCardUi VerifyPhoneCallIcon()
        {
            _driver.SearchElement(DataRep.PhoneCallAnimationExp);

            return this;
        }

        //public IClientCardUi ChooseTrunk(string OfficeName)
        //{
        //    _driver.SearchElement(SelectTrunkExp)
        //        .SelectElementFromDropDownByText(_driver, SelectTrunkExp, OfficeName);

        //    return this;
        //}

        public IFinanceTabUi ClickOnFinanceTab()
        {
            _driver.SearchElement(FinanceTabExp)
               .ForceClick(_driver, FinanceTabExp);

            return _apiFactory.ChangeContext<IFinanceTabUi>(_driver);
        }

        public IClientCardUi ClickOnSave()
        {
            _driver.SearchElement(SaveCustomerExp)
               .ForceClick(_driver, SaveCustomerExp);

            _driver.SearchElement(SuccessSaveCustomerExp);

            return this;
        }

        public ITradeTabUi ClickOnTradeTab()
        {
            _driver.SearchElement(TradeTabExp)
               .ForceClick(_driver, TradeTabExp);

            return _apiFactory.ChangeContext<ITradeTabUi>(_driver);
        }

        public ISavingAccountsTabUI ClickOnSavingAccountTab()
        {
            _driver.SearchElement(SavingAccountTabExp)
               .ForceClick(_driver, SavingAccountTabExp);

            return _apiFactory.ChangeContext<ISavingAccountsTabUI>(_driver);
        }

        public ICommentsTabUi ClickOnCommentTab()
        {
            _driver.SearchElement(CommentsTabExp)
               .ForceClick(_driver, CommentsTabExp);

            return _apiFactory.ChangeContext<ICommentsTabUi>(_driver);
        }

        public IInformationTabUi ClickOnInformationTab()
        {
            _driver.SearchElement(InformationTabExp)
               .ForceClick(_driver, InformationTabExp);

            return _apiFactory.ChangeContext<IInformationTabUi>(_driver);
        }

        public IClientCardUi ClickOnCaptureTab()
        {
            _driver.SearchElement(CaptureTabExp)
               .ForceClick(_driver, CaptureTabExp);

            return this;
        }

        public IClientCardUi ClickOnFileTab()
        {
            _driver.SearchElement(FileTabExp)
               .ForceClick(_driver, FileTabExp);

            return this;
        }

        public IClientCardUi VerifyChatButtonDisable()
        {
            _driver.CheckIfElementNotEnable(ChatDisableBtnExp);

            return this;
        }

        public IClientsPageUi ClickOnCloseBtn()
        {
            _driver.SearchElement(CloseClientCardBtnExp)
                .ForceClick(_driver, CloseClientCardBtnExp);

            return _apiFactory.ChangeContext<IClientsPageUi>(_driver);
        }

        public IClientCardUi ClickOnFastLoginBtn()
        {
            //var originalWindow = _driver.CurrentWindowHandle;

            //_driver.SearchElement(FastLoginBtnExp)
            //    .ForceClick(_driver, FastLoginBtnExp);

            //var wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));
            //wait.Until(wd => wd.WindowHandles.Count == 2);

            //Wait for the new window or tab
            _driver.OpenNewWindowAndSwitch(FastLoginBtnExp, "trade");

            return this;
        }

        public bool GetFastLoginBtnStatus()
        {
            return _driver.CheckIfElementNotEnable(FastLoginBtnExp);
        }

        public string GetBlockPopUpTitle()
        {
            return _driver.SearchElement(BlockPopUpTitleExp)
                .GetElementText(_driver);
        }

        public IClientCardUi ClickOnBlockedBtn()
        {
            _driver.SearchElement(BlockedBtnExp)
                .ForceClick(_driver, BlockedBtnExp);

            return this;
        }

        public T ChangeContext<T>(IWebDriver driver) where T : class
        {
            return _apiFactory.ChangeContext<T>(_driver);
        }
    }
}
