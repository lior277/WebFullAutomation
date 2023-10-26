using System.Collections.Generic;

namespace AirSoftAutomationFramework.Objects.ForexObjects.Ui.Crm.Campaigns.Map
{
    public interface ICampaignsMapPageUi
    {
        CampaignsMapPageUi ClickOnFilterButtonByName(string filterButtonName);
        string GetCampaignName();
        string GetCountryName();
        string GetCountryNameFromMapByContrySymble(string countrySymble);
        List<string> GetFilterDataPipe();
        List<string> GetFilterValues();
        List<string> GetiInActiveCampaignTableValues();
        ICampaignsMapPageUi VerifyCountryHighlightedBySymble(string countrySymble);
    }
}