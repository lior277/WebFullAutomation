namespace AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.Clients.ClientCard.DocumentsAndFilesTab
{
    public interface IDocumentsAndFilesTabApi
    {
        IDocumentsAndFilesTabApi VerifyFileDonloaded(string url, string clientId,
            string fileName, string apiKey);
    }
}