using OpenQA.Selenium;

namespace AirSoftAutomationFramework.Objects.ForexObjects.Ui.Crm.ClientsPage.ClientCard
{
    public interface IInformationTabUi
    {
        T ChangeContext<T>(IWebDriver driver) where T : class;
        IInformationTabUi ClickOnKycProofOfIdentityDounloadButton();
        string GetComplianceStatusToolTipText();
        IInformationTabUi SelectComplianceStatus(string complianceStatu);
        IInformationTabUi VerifyCampaignFiledDisable();
    }
}