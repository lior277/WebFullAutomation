using AirSoftAutomationFramework.CrmObjects.Ui.Accounts.RolesPage;
using OpenQA.Selenium;

namespace AirSoftAutomationFramework.Objects.SharedObjectsForCbdAndForex.Ui.Accounts.RolesPage
{
    public interface IGridSearchResultRolesUi
    {
        T ChangeContext<T>(IWebDriver driver) where T : class;
        ICreateRoleUi ClickOnEditRoleButton();
    }
}