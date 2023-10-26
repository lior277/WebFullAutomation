using OpenQA.Selenium;

namespace AirSoftAutomationFramework.Objects.ForexObjects.Ui.Crm.ClientsPage.ClientCard.FinancesTab
{
    public interface IDepositUi
    {
        T ChangeContext<T>(IWebDriver driver) where T : class;
        IFinanceTabUi CreateDeposit(string depositAmount = "10");
        IFinanceTabUi SaveDeposit();
        IDepositUi SetDepositAmount(string depositAmount = null);
        IDepositUi SetDepositDetails(string TypeDepositDetails = null);
        IDepositUi SetDepositMethod(string typeDepositMethod = null);
        IDepositUi SetTransactionCurrency(string typeTransactionCurrency = null);
    }
}