using OpenQA.Selenium;

namespace AirSoftAutomationFramework.Objects.ForexObjects.Ui.Crm.ClientsPage
{
    public interface IAttributionRulePageUi
    {
        T ChangeContext<T>(IWebDriver driver) where T : class;
        IAttributionRulePageUi ChooseRetentionType(string RetentionTypeValue);
        IAttributionRulePageUi ChooseSplit(string splitValue);
        ICreateClientUi ClickOnSaveButton();
        string GetDuplicateAttributionMessage();
        IAttributionRulePageUi SelectCampaignPipe(string campaignName);
        IAttributionRulePageUi SelectCountryPipe(string countryName, bool needToCloseDialog = true);
        IAttributionRulePageUi SelectFtdSellerPipe(string ftdSellerId);
        IAttributionRulePageUi SelectRetentionSellerPipe(string retentionSellerId);
        IAttributionRulePageUi SelectType(string typeValue);
        IAttributionRulePageUi SetName(string name);
    }
}