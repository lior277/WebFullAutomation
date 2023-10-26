using OpenQA.Selenium;

namespace AirSoftAutomationFramework.Objects.ForexObjects.Ui.Crm.Accounts.RolesPage
{
    public interface IRoleUi
    {
        T ChangeContext<T>(IWebDriver driver) where T : class;
        IRoleUi ClickOnAutoComplate(string permissionName);
        IRoleUi ClickOnPermissionTab();
        IRolesPageUi ClickOnSaveRoleButton();
        IRolesPageUi CreateRolePipe(string roleName, string permissionName);
        string GetPermissionDisableReasonMessage();
        int GetPermissionPosition(string permissionName);
        IRoleUi HoverOnPermissionByName(string permissionName);
        IRoleUi SearchPermission(string permissionName);
        IRoleUi SetPermissionByName(string permissionName);
        IRoleUi SetRolename(string rolename);
        IRoleUi VerifyPermissionHighlighted(string permissionName);
    }
}