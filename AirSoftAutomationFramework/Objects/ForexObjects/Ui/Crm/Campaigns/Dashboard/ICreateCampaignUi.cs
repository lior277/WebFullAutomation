using OpenQA.Selenium;

namespace AirSoftAutomationFramework.Objects.ForexObjects.Ui.Crm.Campaigns.Dashboard
{
    public interface ICreateCampaignUi
    {
        T ChangeContext<T>(IWebDriver driver) where T : class;
        ICampaignsPageUi ClickOnSaveButton();
        ICampaignsPageUi CreateCampaignUiPipeCbd(string affiliateName, string campaignName);
        ICampaignsPageUi CreateCampaignUiPipe(string affiliateName, string campaignName);
        ICreateCampaignUi SetAffiliateName(string affiliateName);
        ICreateCampaignUi SetCampaignName(string campaigntName);
        ICreateCampaignUi SetCurrency(string currency = null);
        ICreateCampaignUi SetDeal(string deal = null);
    }
}