// Ignore Spelling: Forex api

using System.IO;
using System.Reflection;
using AirSoftAutomationFramework.Internals.ExtensionsMethods;
using AirSoftAutomationFramework.Internals.Factorys;
using AirSoftAutomationFramework.Internals.Helpers;
using AirSoftAutomationFramework.Objects.ForexObjects.ApiGeneral;
using OpenQA.Selenium;

namespace AirSoftAutomationFramework.Objects.ForexObjects.Ui.QAndA
{
    public class QAndAPageUi : IQAndAPageUi
    {
        public IWebDriver _driver;
        private readonly IApplicationFactory _apiFactory;
        private string _verifyQAndAQuestion = "//span[contains(., ' {0} ')]";
        private string _verifyQAndAAnswer = "//div[@class='qna-img' and contains(.,'{0}')]";

        private string _filePath = Path.Combine(Path.GetDirectoryName(Assembly
           .GetExecutingAssembly().Location), DataRep.FileNameToUpload);

        public QAndAPageUi(IApplicationFactory apiFactory, IWebDriver driver)
        {
            _driver = driver;
            _apiFactory = apiFactory;
        }

        #region Locator's      
        private readonly By CampaignTitleExp = By.CssSelector("span[class*='sale-title']");
        private readonly By CountryAndCampaignValuesExp = By.CssSelector("div[class*='campaign-progress'] span[class*='sale-percent']");
        private readonly By InActiveCampaignTableExp = By.CssSelector("table[id='inActiveCampaignTable'] td");
        private readonly By AddQuestionExp = By.CssSelector("textarea[name='questionTableText']");
        private readonly By AddAnswerExp = By.CssSelector("div[class='modal-dialog action-alert-auto create-qna-modal'] div[class='note-editable']");
        private readonly By UploadAnswerFileExp = By.CssSelector("div[class='modal in'] input[name='files']");
        private readonly By UploadImageBtnExp = By.CssSelector("button[data-original-title='Picture']");
        private readonly By ImageExp = By.CssSelector("img[data-filename='retriever']");
        private readonly By SaveBtnExp = By.CssSelector("div[class='modal-dialog action-alert-auto create-qna-modal'] button[class*='confirm-action-btn']");
        private readonly By ConfirmBtnExp = By.CssSelector("div[class='model in content-modal-dialog action-alert'] button[class*='confirm-action-btn']");


        private readonly By AddQuestionAndAnswerBtnExp =
            By.CssSelector("button[class='btn btn-default add-group-btn']");

        #endregion Locator's

        public IQAndAPageUi ClickOnQuestionByName(string question)
        {
            var qAndAQuestionExp = By.XPath(string.Format(_verifyQAndAQuestion, question));

            _driver.SearchElement(qAndAQuestionExp)
                    .ForceClick(_driver, qAndAQuestionExp);

            return this;
        }

        public IQAndAPageUi ClickOnSave()
        {
            _driver.SearchElement(SaveBtnExp)
                .ForceClick(_driver, SaveBtnExp);

            return this;
        }

        public IQAndAPageUi ClickOnConfirm()
        {
            _driver.SearchElement(ConfirmBtnExp)
                .ClickAndWaitForElementNotExist(_driver);

            return this;
        }

        public IQAndAPageUi SetQuestion(string question)
        {
            _driver.SearchElement(AddQuestionExp)
                .SendsKeysAuto(_driver, AddQuestionExp, question);

            return this;
        }

        public IQAndAPageUi SetAnswer(string answer)
        {
            _driver.SearchElement(AddAnswerExp)
                .SendKeys(answer);

            return this;
        }

        public IQAndAPageUi UploadImageToAnswer()
        {
            _apiFactory
                .ChangeContext<ISharedStepsGenerator>(_driver)
                .UploadFileOnGrid(UploadAnswerFileExp, _filePath);

            _driver.SearchElement(ImageExp);

            return this;
        }

        public IQAndAPageUi ClickOnAddQuestionAndAnswer()
        {
            _driver.SearchElement(AddQuestionAndAnswerBtnExp)
                .ForceClick(_driver, AddQuestionAndAnswerBtnExp);

            return this;
        }

        public IQAndAPageUi ClickUploadImage()
        {
            _driver.SearchElement(UploadImageBtnExp)
                .ForceClick(_driver, UploadImageBtnExp);

            return this;
        }

        public IQAndAPageUi VerifyAnswerText(string answer)
        {
            var qAndAAnswerExp = By.XPath(string.Format(_verifyQAndAAnswer, answer));

            _driver.SearchElement(qAndAAnswerExp)
                .GetElementText(_driver, qAndAAnswerExp);

            return this;
        }

        public string GetAnswerImage()
        {
            var hh = _driver.SearchElement(By.TagName("img"))
                .GetAttribute("src");

            return _driver.SearchElement(By.TagName("img"))
                .GetAttribute("src");
        }

        public T ChangeContext<T>(IWebDriver driver) where T : class
        {
            return _apiFactory.ChangeContext<T>(_driver);
        }
    }
}
