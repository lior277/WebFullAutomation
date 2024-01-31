using OpenQA.Selenium;

namespace AirSoftAutomationFramework.Objects.ForexObjects.Ui.Crm.Banking
{
    public interface IBonusesPageUi
    {
        IBonusesPageUi CheckIfIdColumnExist();
        T ChangeContext<T>(IWebDriver driver) where T : class;
    }
}