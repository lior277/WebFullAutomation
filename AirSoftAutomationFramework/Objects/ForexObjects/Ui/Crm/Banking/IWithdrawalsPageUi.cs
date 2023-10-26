using OpenQA.Selenium;

namespace AirSoftAutomationFramework.Objects.ForexObjects.Ui.Crm.Banking
{
    public interface IWithdrawalsPageUi
    {
        T ChangeContext<T>(IWebDriver driver) where T : class;
        IWithdrawalsPageUi CheckIfIdColumnExist();
        IWithdrawalsPageUi SearchWithdrawal(int withdrawalId);
        IWithdrawalsPageUi SelectAssignTo(string assignTo);
        IWithdrawalsPageUi SelectPsp(string pspName);
        IWithdrawalsPageUi SelectTransactionStatus(string transactionStatus);
        IWithdrawalsPageUi VerifyMessages(string message);
    }
}