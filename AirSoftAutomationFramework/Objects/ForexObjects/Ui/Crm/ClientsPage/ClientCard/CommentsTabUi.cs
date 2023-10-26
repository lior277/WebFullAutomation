using AirSoftAutomationFramework.Internals.ExtensionsMethods;
using AirSoftAutomationFramework.Internals.Factory;
using AirSoftAutomationFramework.Internals.Helpers;
using OpenQA.Selenium;

namespace AirSoftAutomationFramework.Objects.ForexObjects.Ui.Crm.ClientsPage.ClientCard
{
    public class CommentsTabUi : ICommentsTabUi
    {
        private string _planingTime;
        private string _comment;
        private IWebDriver _driver;
        private readonly IApplicationFactory _apiFactory;

        public CommentsTabUi(IApplicationFactory apiFactory, IWebDriver driver)
        {
            _driver = driver;
            _apiFactory = apiFactory;
        }

        #region Locator's
        private readonly By PlaningTimeExp = By.CssSelector(
            "p-calendar[class*='leads-comments'] span[class*='ui-calendar']");

        private readonly By CommentExp = DataRep.CommentExp;
        private readonly By SaveBtnExp = By.CssSelector("button[class*='save-comment-btn']");
        private readonly By DateBtnExp = By.CssSelector("a[class*='ui-state-highlight']");
        private readonly By CloseCalExp = By.XPath("//label[contains(.,'Planning Time')]");
        #endregion Locator's

        public ICommentsTabUi SetPlaningTime(string planingTime = null)
        {
            _planingTime = planingTime ?? "04/14/2020 12:08";

            _driver.SearchElement(PlaningTimeExp)
                .ForceClick(_driver, PlaningTimeExp);

            _driver.SearchElement(DateBtnExp)
                .ForceClick(_driver, DateBtnExp);
                     
            return this;
        }

        public ICommentsTabUi SetComment(string comment = null)
        {
            _comment = comment ?? "Automation Comment";

            var element = _driver.SearchElement(CommentExp);
            element.ForceClick(_driver, CommentExp);
            element.SendsKeysAuto(_driver, CommentExp, _comment);

            return this;
        }

        public ICommentsTabUi ClickOnSaveButton()
        {
            _driver.SearchElement(CloseCalExp)
                .ForceClick(_driver, CloseCalExp);

            _driver.SearchElement(SaveBtnExp)
                .ForceClick(_driver, SaveBtnExp);

            return this;
        }

        public ICommentsTabUi CreateCommentPipe()
        {
            SetComment();
            SetPlaningTime();
            ClickOnSaveButton();

            return this;
        }

        public T ChangeContext<T>(IWebDriver driver) where T : class
        {
            return _apiFactory.ChangeContext<T>(_driver);
        }
    }
}
