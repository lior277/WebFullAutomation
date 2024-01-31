// Ignore Spelling: Forex

using OpenQA.Selenium;

namespace AirSoftAutomationFramework.Objects.ForexObjects.Ui.Settings
{
    public interface IOfficesTabUi
    {
        IOfficesTabUi AssignIpsPipe();
        T ChangeContext<T>(IWebDriver driver) where T : class;
        IOfficesTabUi ClickOnAssignIpsButton();
        IOfficesTabUi ClickOnEditOfficeButton();
        IOfficesTabUi ClickOnOverrideButton();
        IOfficesTabUi ClickOnSaveButton();
        IOfficesTabUi SetAllowedIpAddresses();
    }
}