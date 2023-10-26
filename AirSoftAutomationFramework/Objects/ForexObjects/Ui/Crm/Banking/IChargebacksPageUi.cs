using OpenQA.Selenium;

namespace AirSoftAutomationFramework.Objects.ForexObjects.Ui.Crm.Banking
{
    public interface IChargebacksPageUi
    {
        T ChangeContext<T>(IWebDriver driver) where T : class;
        IChargebacksPageUi CheckIfIdColumnExist();
        IChargebacksPageUi SearchChargeback(int ChargebackId);
        IChargebacksPageUi SelectPsp(string pspName);
        IChargebacksPageUi SelectAssignTo(string assignTo);
        IChargebacksPageUi VerifyMessages(string message);
    }
}