using AirSoftAutomationFramework.CRMObjects.Accounts;
using AirSoftAutomationFramework.Internals;

namespace AirSoftAutomationFramework.Objects.SharedObjectsForCbdAndForex.Ui.Accounts.UsersPage
{
    public interface ICreateUserUi : IApplicationFactory
    {
        IUCliensUi CreateDefaultUserUiPipe(string userName, string password = null);
        IUCliensUi ClickOnSaveButton();
        ICreateUserUi SaveAndCreateAnotherUserButton();
        ICreateUserUi SetAccountType(string accountType = null);
        ICreateUserUi SetActive(bool active);
        ICreateUserUi AddAsChild(bool addAsChild);
        ICreateUserUi SetYourStatus(bool? active = null, bool? addAsChild = null);
        ICreateUserUi SetAllowedIpAddresses(string[] allowedIpAddresses = null);
        ICreateUserUi SetCountry(string country = null);
        ICreateUserUi SetEmail(string email = null);
        ICreateUserUi SetExtention(string extention = null);
        ICreateUserUi SetFirstName(string firstName = null);
        ICreateUserUi SetGmtTimezone(string gmtTimezone = null);
        ICreateUserUi SetLastName(string lastName = null);
        ICreateUserUi SetOffice(string office = null);
        ICreateUserUi SetPassword(string password = null);
        ICreateUserUi SetPhone(string phone = null);
        ICreateUserUi SetRole(string role = null);
        ICreateUserUi SetSalaryType(string salaryType = null);
        ICreateUserUi SetSubUsers(string subUsers = null);
        ICreateUserUi SetUserName(string userName);
    }
}