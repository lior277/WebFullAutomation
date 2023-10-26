using AirSoftAutomationFramework.Internals.DAL.ConfigurationManagement;
using AirSoftAutomationFramework.Internals.Factory;
using AirSoftAutomationFramework.Internals.Helpers;
using AirSoftAutomationFramework.Internals.NunitAttributes;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.Clients.ClientCard;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.Clients.ClientCard.FinancesTab;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.Clients.ClientPage;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.Settings;
using AirSoftAutomationFramework.Objects.ForexObjects.ApiGeneral.Login;
using AirSoftAutomationFramework.Objects.ForexObjects.TradingPlatform.TradeObjects.TradeApi.Profile;
using NUnit.Framework;
using TestsProject.TestsInternals;

namespace TestsProject.Tests.ClientCard
{
    [TestFixture]
    public class VerifyUploadAndDownloadFilesApi : TestSuitBase
    {
        #region members
        private IApplicationFactory _apiFactory = new ApplicationFactory();
        private string _crmUrl = Config.appSettings.CrmUrl;
        private string _kycUrl;
        private string _mainLogoUrl;
        #endregion

        #region Test Preparation           
        [SetUp]
        public void SetUp()
        {
            BeforeTest();
            #region PreCondition
            var  tradingPlatformUrl =  Config.appSettings.tradingPlatformUrl;
            var kycType = "kyc_proof_of_identity";

            // create client 
            var clientName = TextManipulation.RandomString();
            var clientEmail = clientName + DataRep.EmailPrefix;

            var clientId = _apiFactory
                .ChangeContext<ICreateClientApi>()
                .CreateClientRequest(_crmUrl, clientName);

            _apiFactory
                 .ChangeContext<IFinancesTabApi>()
                 .PostBonusRequest(_crmUrl, clientId);

            // get login Data for trading Platform
            var loginData = _apiFactory
                 .ChangeContext<ILoginApi>()
                 .PostLoginToTradingPlatform(tradingPlatformUrl, clientEmail)
                 .GeneralResponse;

            // upload kyc file to trading 
            _apiFactory
                .ChangeContext<IProfilePageApi>()
                .PatchKycFileRequest(tradingPlatformUrl,
                DataRep.FileNameToUpload, kycType, loginData);

            // login search client and click on download button
            var GetClientByIdResponse = _apiFactory
                .ChangeContext<IClientsApi>()
                .GetClientByIdRequest(_crmUrl, clientId)
                .GeneralResponse;

            _kycUrl = GetClientByIdResponse.user.kyc_proof_of_identity;

            // upload main logo
           _mainLogoUrl = _apiFactory
               .ChangeContext<IGeneralTabApi>()
               .PutEditCompanyInformationRequest(_crmUrl)
               .GetEditCompanyInformationRequest(_crmUrl)
               .main_logo;
            #endregion
        }
        #endregion

        [TearDown]
        public void TearDown()
        {
            try
            {
            }
            finally
            {
                AfterTest();
            }
        }

        // upload KYC POI file from platform
        // upload main logo file from settings
        // verify that on client card that KYC POI file can be downloaded
        // verify that on settings main logo file can be open
        [Test]
        [Category(DataRep.SanityCategory)]
        [Category(DataRep.RegressionCategory)]
        [RetryMenage(DataRep.NumOfRetraiesForFallingTest)]
        public void VerifyUploadAndDownloadFilesApiTest()
        {
            var expectedKycFileType = "kyc_proof_of_identity";
            var expectedMainLogoContentType = "image/png";

            var actualKycFileTypeFromClientCard = _apiFactory
                .ChangeContext<IClientsApi>()
                .GetDownloadKycFileRequest(_crmUrl, _kycUrl);

            var actualFileContentTypeOfMainLogo = _apiFactory
               .ChangeContext<IGeneralTabApi>()
               .GetDownloadMainLogoFileRequest(_crmUrl, _mainLogoUrl)
               .Content
               .Headers
               .ContentType
               .ToString();

            Assert.Multiple(() =>
            {
                Assert.True(actualKycFileTypeFromClientCard.Contains(expectedKycFileType),
                    $"actual Kyc File Type From Client Card : {actualKycFileTypeFromClientCard}" +
                    $"expected Kyc File Type From Client Card not contains : {expectedKycFileType}");

                Assert.True(actualFileContentTypeOfMainLogo.Contains(expectedMainLogoContentType),
                    $"actual File Content Type Of Main Logo :{actualFileContentTypeOfMainLogo}" +
                    $"expected actual File Content Type Of Main Logo: {expectedMainLogoContentType}");
            });
        }
    }
}