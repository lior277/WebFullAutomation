using System;
using AirSoftAutomationFramework.Internals.ExtensionsMethods;
using AirSoftAutomationFramework.Internals.Factorys;
using AirSoftAutomationFramework.Internals.Helpers;
using OpenQA.Selenium;

namespace AirSoftAutomationFramework.Objects.ForexObjects.Ui.Crm.Accounts.RolesPage
{
    public class RoleUi : IRoleUi
    {
        private readonly IApplicationFactory _apiFactory;
        private IWebDriver _driver;
        private string _setPermission = "input[name='{0}']+span";
        private string SearchPermissionAutoComplate = "//a[contains(.,'{0}')]";

        public RoleUi(IApplicationFactory apiFactory, IWebDriver driver)
        {
            _apiFactory = apiFactory;
            _driver = driver;
        }

        #region Locator's       
        private readonly By RoleNameExp = By.CssSelector("input[class*='create-role-role-name']");
        private readonly By PermissionHighlightedExp = By.CssSelector("h4[class*='highlighted']");
        private readonly By SearchPermissionExp = By.CssSelector("input[name='searchValue']");
        private readonly By SaveRoleBtnExp = By.CssSelector("button[class*='save-role-btn']");
        private readonly By PermissionsTabExp = By.XPath("//a[contains(.,'Permissions')]");
        private readonly By PermissionDisableReasonMessageExp = By
            .CssSelector("div[class*='help-block-tooltip help-block-right']");
        #endregion Locator's 

        public IRoleUi SetRolename(string rolename)
        {
            var element = _driver.SearchElement(RoleNameExp);
            element.SendsKeysAuto(_driver, RoleNameExp, rolename, 60);
            element.SendKeys(" ");

            return this;
        }

        public IRoleUi ClickOnPermissionTab()
        {
            _driver.SearchElement(PermissionsTabExp)
                .ForceClick(_driver, PermissionsTabExp);

            return this;
        }

        public IRoleUi SearchPermission(string permissionName)
        {
            _driver.SearchElement(SearchPermissionExp)
                .SendsKeysAuto(_driver, SearchPermissionExp, permissionName);

            return this;
        }

        public IRoleUi ClickOnAutoComplate(string permissionName)
        {
            var SearchPermissionAutoComplateExp =
                By.XPath(string.Format(SearchPermissionAutoComplate, permissionName));

            _driver.SearchElement(SearchPermissionAutoComplateExp)
                .ForceClick(_driver, SearchPermissionAutoComplateExp);

            return this;
        }

        public int GetPermissionPosition(string permissionName)
        {
            return Convert.ToInt32(((IJavaScriptExecutor)_driver)
                .ExecuteScript(" return document.querySelector('div.tab-content').scrollTop"));
        }

        public IRoleUi VerifyPermissionHighlighted(string permissionName)
        {
            _driver.SearchElement(PermissionHighlightedExp);

            return this;
        }

        public IRoleUi HoverOnPermissionByName(string permissionName)
        {
            var setPermissionByNameExp = By.CssSelector(string.Format(_setPermission, permissionName));

            _driver.SearchElement(setPermissionByNameExp)
                .HoverOnElement(_driver);

            return this;
        }

        public string GetPermissionDisableReasonMessage()
        {
            return _driver.SearchElement(PermissionDisableReasonMessageExp)
                .GetElementText(_driver, PermissionDisableReasonMessageExp);
        }

        public IRoleUi SetPermissionByName(string permissionName)
        {
            var setPermissionByNameExp = By
                .CssSelector(string.Format(_setPermission, permissionName));

            _driver.SearchElement(setPermissionByNameExp)
                .ForceClick(_driver, setPermissionByNameExp);

            return this;
        }

        public IRolesPageUi ClickOnSaveRoleButton()
        {
            _driver.SearchElement(SaveRoleBtnExp)
                .ForceClick(_driver, SaveRoleBtnExp);

            _driver.SearchElement(DataRep.ConfirmExp)
                .ForceClick(_driver, DataRep.ConfirmExp);

            return ChangeContext<IRolesPageUi>(_driver);
        }

        public IRolesPageUi CreateRolePipe(string roleName, string permissionName)
        {
            SetRolename(roleName);
            ClickOnPermissionTab();
            SetPermissionByName(permissionName);
            ClickOnSaveRoleButton();

            return ChangeContext<IRolesPageUi>(_driver);
        }

        public T ChangeContext<T>(IWebDriver driver) where T : class
        {
            return _apiFactory.ChangeContext<T>(_driver);
        }
    }
}
