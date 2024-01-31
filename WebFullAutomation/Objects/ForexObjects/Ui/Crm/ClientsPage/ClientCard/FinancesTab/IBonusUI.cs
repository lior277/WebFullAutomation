using OpenQA.Selenium;

namespace AirSoftAutomationFramework.Objects.ForexObjects.Ui.Crm.ClientsPage.ClientCard.FinancesTab
{
    public interface IBonusUi
    {
        T ChangeContext<T>(IWebDriver driver) where T : class;
        IFinanceTabUi CreateBonus(int bonusAmount = 1000);
        IFinanceTabUi SaveBonus();
        IBonusUi SetBonusAmount(int bonusAmount = 1000);
        IBonusUi SetBonusMethod(string bonusMethod = null);
    }
}