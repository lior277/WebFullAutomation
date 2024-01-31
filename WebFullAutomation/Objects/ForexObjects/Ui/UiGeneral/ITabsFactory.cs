// Ignore Spelling: Forex

namespace AirSoftAutomationFramework.Objects.ForexObjects.Ui.UiGeneral
{
    public interface ITabsFactory
    {
        T ClickOnTab<T>(string tabName) where T : class;
    }
}