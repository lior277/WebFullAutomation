using AirSoftAutomationFramework.Internals.ExtensionsMethods;
using AirSoftAutomationFramework.Internals.Factorys;
using AirSoftAutomationFramework.Objects.ForexObjects.ApiGeneral;
using AirSoftAutomationFramework.Objects.ForexObjects.Ui.UiGeneral;
using OpenQA.Selenium;

namespace AirSoftAutomationFramework.Objects.ForexObjects.Ui.Crm.ClientsPage.ClientCard
{
    public class TimelineTabUi : ITimelineTabUi
    {
        private IWebDriver _driver;
        private readonly IApplicationFactory _apiFactory;

        public TimelineTabUi(IApplicationFactory apiFactory, IWebDriver driver)
        {
            _driver = driver;
            _apiFactory = apiFactory;
        }

        #region Locator's
        private readonly By TimelineFilterLoginActivityExp = By.CssSelector("a[title='Login activity']");
        private readonly By TimelineFilterTradingActivityExp = By.CssSelector("a[title='Trading activity']");
        private readonly By TimelineFilterAccountActivityExp = By.CssSelector("a[title='Account activity']");
        private readonly By TimelineFilterBankingActivityExp = By.CssSelector("a[title='Banking activity ']");
        private readonly By SelectNumOfLinesInTimeLineExp = By.CssSelector("div[id='timeLineTab_length'] select[class='btn']");
        private readonly By EnvelopeExp = By.XPath("//div[@class='timeline-email-content']" +
            "[contains(.,'Email first deposit has been sent to client')]/button");

        private readonly By EnvelopeBoxTitleExp = By.CssSelector("div[class='modal-header content-modal-header'] h4[class*='modal-title']");
        private readonly By EnvelopeBoxBodyExp = By.CssSelector("div[class*='list-error-service']");
        private readonly By NumOfRowsInTimelineTableExp = By.CssSelector("div[id='timeLineTab_info']");
        private readonly By WaitForProcessingTimeLineExp = By.CssSelector("div[id='timeLineTab_processing'][style='display: block;']");
        private readonly By TimelineDataTableRowsExp = By.XPath("//table[contains(@class,'search-result-timeline')]/tbody/tr/td[not(contains(@class,'dataTables_empty'))]/parent::tr");
        #endregion Locator's

        public ITimelineTabUi ClickOnEnvelope()
        {
            _driver.SearchElement(EnvelopeExp);
            _driver.ClickAndWaitForNextElement(EnvelopeExp, EnvelopeBoxTitleExp);

            return this;
        }

        public ITimelineTabUi WaitForTimeLineTableToLoad()
        {
            _driver.WaitForElementNotExist(WaitForProcessingTimeLineExp);
            _driver.WaitForAtListNumberOfElements(TimelineDataTableRowsExp, 1);

            return this;
        }

        public string GetEnvelopeBoxTitle()
        {
            return _driver.SearchElement(EnvelopeBoxTitleExp)
                .GetElementText(_driver, EnvelopeBoxTitleExp);
        }

        public string GetEnvelopeBoxBody()
        {
            return _driver.SearchElement(EnvelopeBoxBodyExp)
                .GetElementText(_driver, EnvelopeBoxBodyExp);
        }

        public ITimelineTabUi ClickOnLoginActivityFilterButton()
        {
            _driver.SearchElement(TimelineFilterLoginActivityExp)
               .ForceClick(_driver, TimelineFilterLoginActivityExp);

            return this;
        }

        public ITimelineTabUi ClickOnTradingActivityFilterButton()
        {
            _driver.SearchElement(TimelineFilterTradingActivityExp)
               .ForceClick(_driver, TimelineFilterTradingActivityExp);

            return this;
        }

        public ISearchResultsUi SetNumOfLines()
        {
            WaitForTimeLineTableToLoad();

            _driver.SearchElement(SelectNumOfLinesInTimeLineExp)
               .SelectElementFromDropDownByValue(_driver, "100");

            //_driver.WaitForAnimationToLoad(1000);

            return _apiFactory.ChangeContext<ISearchResultsUi>(_driver);
        }

        public ITimelineTabUi ClickOnAccountActivityFilterButton()
        {
            _driver.SearchElement(TimelineFilterAccountActivityExp)
               .ForceClick(_driver, TimelineFilterAccountActivityExp);

            return this;
        }

        public ITimelineTabUi ClickOnBankingActivityFilterButton()
        {
            _driver.SearchElement(TimelineFilterBankingActivityExp)
               .ForceClick(_driver, TimelineFilterBankingActivityExp);

            return this;
        }

        public T ChangeContext<T>(IWebDriver driver) where T : class
        {
            return _apiFactory.ChangeContext<T>(_driver);
        }

    }
}
