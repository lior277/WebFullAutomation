using AirSoftAutomationFramework.CrmObjects.Ui;
using AirSoftAutomationFramework.CRMObjects.Accounts;
using AirSoftAutomationFramework.Internals;

namespace AirSoftAutomationFramework.Objects.SharedObjectsForCbdAndForex.Ui.Accounts.UsersPage
{
    public interface IEditUserUi : IApplicationFactory
    {
        IUCliensUi ClickOnSaveButton();
        string GetEmail();
        ICreateUserUi SaveAndCreateAnotherUserButton();
        IEditUserUi SetAccountType(string accountType = null);
        IEditUserUi SetActive(bool active);
        IEditUserUi SetAllowedIpAddresses(string[] allowedIpAddresses = null);
        IEditUserUi SetApiKey(string setApiKey = null);
        IEditUserUi SetCountry(string country = null);
        IEditUserUi SetEmail(string email = null);
        IEditUserUi SetExtention(string extention = null);
        IEditUserUi SetFirstName(string firstName = null);
        IEditUserUi SetGmtTimezone(string gmtTimezone = null);
        IEditUserUi SetLastName(string lastName = null);
        IEditUserUi SetOffice(string office = null);
        IEditUserUi SetPhone(string phone = null);
        IEditUserUi SetRole(string role = null);
        IEditUserUi SetSalaryType(string salaryType = null);
        IEditUserUi SetSubUsers(string subUsers = null);
        IEditUserUi SetUserName(string userName);
    }
}