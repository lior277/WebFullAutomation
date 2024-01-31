using OpenQA.Selenium;
using System.Collections.Generic;

namespace AirSoftAutomationFramework.Objects.ForexObjects.Ui.Crm.ClientsPage.ClientCard
{
    public interface ISavingAccountsTabUI
    {
        T ChangeContext<T>(IWebDriver driver) where T : class;
        List<string> GetSavingAccountTitlesText();
    }
}