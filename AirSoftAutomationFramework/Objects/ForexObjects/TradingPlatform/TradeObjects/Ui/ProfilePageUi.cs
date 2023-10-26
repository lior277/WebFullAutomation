using AirSoftAutomationFramework.Internals.ExtensionsMethods;
using AirSoftAutomationFramework.Internals.Factory;
using AirSoftAutomationFramework.Internals.Helpers;
using AirSoftAutomationFramework.Objects.ForexObjects.ApiGeneral;
using OpenQA.Selenium;
using System.IO;
using System.Linq;
using System.Reflection;

namespace AirSoftAutomationFramework.Objects.ForexObjects.TradingPlatform.TradeObjects.Ui
{
    public class ProfilePageUi : IProfilePageUi
    {
        private IWebDriver _driver;
        private readonly IApplicationFactory _apiFactory;
        private string kycSection = "div[class*='{0}']";

        private string _filePath = Path.Combine(Path.GetDirectoryName(Assembly
            .GetExecutingAssembly().Location), DataRep.FileNameToUpload);

        public ProfilePageUi(IApplicationFactory apiFactory, IWebDriver driver)
        {
            _driver = driver;
            _apiFactory = apiFactory;
        }

        #region Locator's      
        private readonly By ProofOfIdentitySectionExp = By.CssSelector("div[class='kyc-row-title proof-of-identity']");
        private readonly By KycPendingStatusExp = By.CssSelector("span[class='icon-small kyc-status-img spr spr-small_pending']");
        private readonly By FirstNamePencilBtnExp = By.CssSelector("i[class='spr spr-pencil zoom-out-icon ng-star-inserted']");
        private readonly By FirstNameInputExp = By.CssSelector("input[class*='custom-input profile-data-label']");
        private readonly By UploadKycFileExp = By.CssSelector("div[class='kyc-row-drop-file'] input[class='file-input']");
        private readonly By SaveProfileBtnExp = By.CssSelector("button[class*='personal_info_submit']");
        private readonly By ProfileNameExp = By.CssSelector("span[class='profile_name']");
        private readonly By SectionIsOpenExp = By.CssSelector("div[class='kyc-row-inner-content ng-star-inserted']");
        private readonly By ImageLoaderExp = By.CssSelector("div[class*='lds-dual-ring upload-image-loader']");
        #endregion Locator's

        public bool CheckIfProofOfIdentitySectionExist(int numOfElement)
        {
            return _driver.WaitForExactNumberOfElements(ProofOfIdentitySectionExp, numOfElement)
                == numOfElement;
        }

        public IProfilePageUi ClickOnKycSection(string sectionName)
        {
            var KycSectionExp = By.CssSelector(string.Format(kycSection, sectionName));

            _driver.SearchElement(KycSectionExp)
                .ForceClick(_driver, KycSectionExp);

            return this;
        }

        public IProfilePageUi ClickOnFirstNamePencil()
        {
            _driver.WaitForExactNumberOfElements(FirstNamePencilBtnExp, 2);

            _driver.SearchElements(FirstNamePencilBtnExp)
                .First()
                .ClickWithJavaScript(_driver);

            return this;
        }

        public IProfilePageUi ClickOnSaveProfilebutton()
        {
            _driver.SearchElement(SaveProfileBtnExp)
                .ForceClick(_driver, SaveProfileBtnExp);

            return this;
        }

        public string GetProfileName(string firstName)
        {
            _driver.SearchElement(DataRep.ProfileFirstNameExp)
                .WaitForElementTextToChange(_driver, DataRep.ProfileFirstNameExp, firstName);

            return _driver.SearchElement(DataRep.ProfileFirstNameExp)
                .GetElementText(_driver, DataRep.ProfileFirstNameExp);
        }

        public IProfilePageUi SetFirstName(string firstName)
        {
            _driver.WaitForExactNumberOfElements(FirstNameInputExp, 2);

            _driver.SearchElements(FirstNameInputExp)
                .First()
                .SendsKeysAuto(_driver, FirstNameInputExp, firstName);

            return this;
        }

        public IProfilePageUi UploadKycFile()
        {
            _apiFactory
                .ChangeContext<ISharedStepsGenerator>(_driver)
                .UploadFileOnGrid(UploadKycFileExp, _filePath);

            _driver.WaitForElementNotExist(ImageLoaderExp, 120);

            return this;
        }

        public IProfilePageUi VerifyMessages(string message)
        {
            var alertExp = By.XPath(string.Format(DataRep.AlertOnFront, message));

            _driver.SearchElement(alertExp)
                .GetElementText(_driver, alertExp)
                .StringContains(message);

            return this;
        }

        public IProfilePageUi VerifyPendingStatus()
        {
            _driver.SearchElement(KycPendingStatusExp);

            return this;
        }

        public T ChangeContext<T>(IWebDriver driver) where T : class
        {
            return _apiFactory.ChangeContext<T>(_driver);
        }
    }
}
