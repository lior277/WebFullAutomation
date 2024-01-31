// Ignore Spelling: admin Timeline

using AirSoftAutomationFramework.Objects.ForexObjects.Ui.Crm.ClientsPage.ClientCard.FinancesTab;
using OpenQA.Selenium;

namespace AirSoftAutomationFramework.Objects.ForexObjects.Ui.Crm.ClientsPage.ClientCard
{
    public interface IClientCardUi
    {
        T ChangeContext<T>(IWebDriver driver) where T : class;
        IClientCardUi ClickOnBlockedBtn();
        IClientCardUi ClickOnCaptureTab();
        IClientsPageUi ClickOnCloseBtn();
        IClientCardUi ClickOnOkButton();
        ICommentsTabUi ClickOnCommentTab();
        IDocumentsTabUI ClickOnDocumentsTab();
        IClientCardUi ClickOnExportButton();
        IClientCardUi ClickOnFastLoginBtn();
        IClientCardUi ClickOnFileTab();
        IFinanceTabUi ClickOnFinanceTab();
        IInformationTabUi ClickOnInformationTab();
        IClientCardUi ClickOnPhoneIconButton();
        IClientCardUi ClickOnSave();
        ISavingAccountsTabUI ClickOnSavingAccountTab();
        IClientCardUi ClickOnSendBtn();
        ITimelineTabUi ClickOnTimelineTab();
        ITradeTabUi ClickOnTradeTab();
        string GetBlockPopUpTitle();
        bool GetFastLoginBtnStatus();
        IClientCardUi SetEmailForExport(string adminEmail);
        IClientCardUi VerifyChatButtonDisable();
        IClientCardUi VerifyPhoneCallIcon();
        IClientCardUi VerifySetCurrency(bool exist);
    }
}