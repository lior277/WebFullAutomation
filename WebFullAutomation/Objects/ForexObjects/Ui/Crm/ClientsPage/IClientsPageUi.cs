using AirSoftAutomationFramework.Objects.ForexObjects.Ui.UiGeneral;
using OpenQA.Selenium;

namespace AirSoftAutomationFramework.Objects.ForexObjects.Ui.Crm.ClientsPage
{
    public interface IClientsPageUi
    {
        void AddComment(string comment);
        IClientsPageUi ChangeCampaign(string campaignName);
        T ChangeContext<T>(IWebDriver driver) where T : class;
        IClientsPageUi ChangeStatusAndStatus2(string status1, string status2);
        IClientsPageUi ChangeTradeGroup(string group);
        bool CheckIfColumnsVisibilityButtonExist();
        bool CheckIfEventsButtonExist();
        int CheckIfFreeTextIsDisable();
        IClientsPageUi CheckIfIdColumnExist();
        bool CheckIfImportButtonExist();
        IClientsPageUi ClickOnConfimationCallPoUp();
        IAttributionRulePageUi ClickOnCreateAttributionRuleButton();
        ICreateClientUi ClickOnCreateClientButton();
        IClientsPageUi ClickOnEditFreeTextBtn();
        IClientsPageUi ClickOnFastLoginBtn();
        IClientsPageUi ClickOnImportButton();
        IClientsPageUi ClickOnPhoneIconButton();
        IClientsPageUi ClickOnSaveFreeTextButton();
        IClientsPageUi ClickOnUploadFileButton();
        IClientsPageUi ClientAssignmentPipe(string salesAgentName, string salesStatus = "New");
        IClientsPageUi DeleteComment();
        string GetErrorUploadMessage();
        string GetFreeText(string freeTextFiled);
        IClientsPageUi PerformMassAsignPipe(string comment, string salesAgent, string campaign, string status1, string status2, string tradeGroup);
        ISearchResultsUi SearchClientByEmail(string clientMail);
        IClientsPageUi SelectCampaign(string campaignName);
        IClientsPageUi SelectSalesAgent(string salesAgentName);
        IClientsPageUi SelectStatus(string statusName);
        IClientsPageUi SelectTradeGroup(string tradeGroupName);
        IClientsPageUi SetFreeText(string freeTextFiled, string text);
        ISearchResultsUi SetTableColumnVisibility(string columnNAme);
        ISearchResultsUi SetTimeFilter(string time);
        IClientsPageUi UploadCsvFile(string filePath);
        IClientsPageUi VerifyMessages(string message);
        IClientsPageUi VerifyNoPhoneCallButton();
        IClientsPageUi VerifyPhoneCallAnimation();
        IClientsPageUi WaitForClientsTableToLoad();
        int WaitForNumOfRowsInClientsTable(int expectedNumOfRows);
        IClientsPageUi WaitForTableToLoad();
    }
}