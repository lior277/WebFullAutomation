using AirSoftAutomationFramework.Objects.ForexObjects.Ui.UiGeneral;
using OpenQA.Selenium;
using System.Collections.Generic;

namespace AirSoftAutomationFramework.Objects.ForexObjects.Ui.Crm.Campaigns.Dashboard
{
    public interface ICampaignsPageUi
    {
        T ChangeContext<T>(IWebDriver driver) where T : class;
        ICreateAffiliateUi ClickCreateAffiliateButton();
        ICreateCampaignUi ClickCreateCampaignButton();
        List<string> GetCampaignsTitlesFromListBarsDiagram();
        ISearchResultsUi SearchActiveCampaign(string campaignName);
        ISearchResultsUi SearchInActiveCampaign(string campaignName);
        ICampaignsPageUi VerifyDonutDiagramExist();
        ICampaignsPageUi VerifyListBarsDiagramExist();
        ICampaignsPageUi WaitForCampaignTableToLoad();
    }
}