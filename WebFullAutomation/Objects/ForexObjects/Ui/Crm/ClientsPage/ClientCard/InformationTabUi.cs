using AirSoftAutomationFramework.Internals.ExtensionsMethods;
using AirSoftAutomationFramework.Internals.Factorys;
using OpenQA.Selenium;

namespace AirSoftAutomationFramework.Objects.ForexObjects.Ui.Crm.ClientsPage.ClientCard
{
    public class InformationTabUi : IInformationTabUi
    {
        private IWebDriver _driver;
        private readonly IApplicationFactory _apiFactory;

        public InformationTabUi(IApplicationFactory apiFactory, IWebDriver driver)
        {
            _driver = driver;
            _apiFactory = apiFactory;
        }

        #region Locator's
        private readonly By SelectCampaignFiledActiveExp = By.CssSelector("select[id='campaigns']");
        private readonly By ProofOfIdentityDounloadButtonExp = By.CssSelector("a[href*='kyc_proof_of_identity']");
        private readonly By SelectComplianceStatusExp = By.CssSelector("select[id='activation_status']");
        private readonly By ComplianceStatusToolTipText = By.CssSelector("div[class='compliance-status-tooltip help-block-tooltip  customer-content']");
        #endregion Locator's

        public IInformationTabUi VerifyCampaignFiledDisable()
        {
            _driver.WaitForElementNotExist(SelectCampaignFiledActiveExp, 40);

            return this;
        }

        public IInformationTabUi SelectComplianceStatus(string complianceStatu)
        {
            _driver.SearchElement(SelectComplianceStatusExp)
                .SelectElementFromDropDownByText(_driver, SelectComplianceStatusExp, complianceStatu);

            return this;
        }

        public string GetComplianceStatusToolTipText()
        {
            return _driver.SearchElement(ComplianceStatusToolTipText)
                 .GetElementText(_driver, ComplianceStatusToolTipText);
        }

        public IInformationTabUi ClickOnKycProofOfIdentityDounloadButton()
        {
            _driver.SearchElement(ProofOfIdentityDounloadButtonExp)
               .ForceClick(_driver, ProofOfIdentityDounloadButtonExp);

            return this;
        }

        public T ChangeContext<T>(IWebDriver driver) where T : class
        {
            return _apiFactory.ChangeContext<T>(_driver);
        }

    }
}
