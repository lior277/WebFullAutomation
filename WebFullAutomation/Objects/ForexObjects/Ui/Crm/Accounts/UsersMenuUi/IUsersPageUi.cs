using AirSoftAutomationFramework.Objects.ForexObjects.Ui.UiGeneral;
using OpenQA.Selenium;

namespace AirSoftAutomationFramework.Objects.ForexObjects.Ui.Crm.Accounts.UsersMenuUi
{
    public interface IUsersPageUi
    {
        IUserUi ClickOnCreateUserButton();
        ISearchResultsUi SearchUser(string searchText);
        IUserUi ClickOnEditUserButton();
        T ChangeContext<T>(IWebDriver driver) where T : class;
        //IGridSearchResultUsersUi SearchUser(string searchText);
    }
}