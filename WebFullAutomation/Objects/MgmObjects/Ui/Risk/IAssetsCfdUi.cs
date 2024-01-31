// Ignore Spelling: Cfd

using OpenQA.Selenium;

namespace AirSoftAutomationFramework.Objects.MgmObjects.Ui.Risk
{
    public interface IAssetsCfdUi
    {
        IAssetsCfdUi AddAssetPipe(string groupName, string subGroupName, string assetName);
        IAssetsCfdUi AddAssetToFrontOrderByName(string assetName);
        T ChangeContext<T>(IWebDriver driver) where T : class;
        IAssetsCfdUi ChooseAssetGroupByName(string subGroupName);
        IAssetsCfdUi ClickOnSaveButton();
        IAssetsCfdUi OpenAssetGroupByName(string assetGroupName);
        IAssetsCfdUi RemoveAssetByName(string assetName);
        IAssetsCfdUi RemoveAssetPipe(string groupName, string subGroupName, string assetName);
    }
}