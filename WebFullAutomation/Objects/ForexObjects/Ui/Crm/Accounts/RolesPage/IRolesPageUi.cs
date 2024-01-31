using AirSoftAutomationFramework.Objects.ForexObjects.Ui.UiGeneral;
using OpenQA.Selenium;

namespace AirSoftAutomationFramework.Objects.ForexObjects.Ui.Crm.Accounts.RolesPage
{
    public interface IRolesPageUi
    {
        T ChangeContext<T>(IWebDriver driver) where T : class;
        IRoleUi ClickOnCreateRoleButton();
        ISearchResultsUi SearchRole(string searchText, bool refresh = false);
    }
}