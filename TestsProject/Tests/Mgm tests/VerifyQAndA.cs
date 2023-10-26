using System.Linq;
using AirSoftAutomationFramework.Internals.DAL.ConfigurationManagement;
using AirSoftAutomationFramework.Internals.Factory;
using AirSoftAutomationFramework.Internals.Helpers;
using AirSoftAutomationFramework.Internals.NunitAttributes;
using AirSoftAutomationFramework.Objects.DTOs;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.AccountsApi.UsersMenuApi.UserMenuApi;
using AirSoftAutomationFramework.Objects.ForexObjects.Ui.Login;
using AirSoftAutomationFramework.Objects.ForexObjects.Ui.QAndA;
using AirSoftAutomationFramework.Objects.ForexObjects.Ui.UiGeneral;
using AirSoftAutomationFramework.Objects.MgmObjects.Api;
using AirSoftAutomationFramework.Objects.MgmObjects.Api.QAndA;
using NUnit.Framework;
using OpenQA.Selenium;
using TestsProject.TestsInternals;

namespace TestsProject.Tests.Mgm_tests
{
    [TestFixture(DataRep.Chrome)]
    public class VerifyQAndA : TestSuitBase
    {
        public VerifyQAndA(string browser) : base(browser)
        {
            _browserName = browser;
        }

        private IApplicationFactory _apiFactory = new ApplicationFactory();
        private string _crmUrl = Config.appSettings.CrmUrl;
        private string _browserName;
        private GetLoginResponse _mgmLoginData;
        private IWebDriver _driver;

        [SetUp]
        public void SetUp()
        {
            BeforeTest();
            _driver = GetDriver();

            _mgmLoginData = _apiFactory
                .ChangeContext<IMgmCreateUserApi>()
                .PostMgmLoginCookiesRequest(DataRep.MgmUrl,
                DataRep.MgmUserName.Split('@').First());

            _apiFactory
                .ChangeContext<IQEndAApi>()
                .DeleteQEndAPipe(DataRep.MgmUrl, _mgmLoginData);          
        }

        [TearDown]
        public void TearDown()
        {
            try
            {
            }
            finally
            {
                AfterTest(_driver);
                DriverDispose(_driver);
            }
        }

        [Test]
        [Category(DataRep.MgmSanityCategory)]
        [RetryMenage(DataRep.NumOfRetraiesForFallingTest)]
        public void VerifyQAndATest()
        {
            var expectedAnswer = "answer";
            var expectedQuestion = "question";

            _apiFactory
                .ChangeContext<ILoginPageUi>(_driver)
                .LoginPipe(DataRep.MgmUserName, DataRep.MgmUrl, DataRep.Password)
                .ChangeContext<IMenus>(_driver)
                .ClickOnMenuItem<IQAndAPageUi>()
                .ClickOnAddQuestionAndAnswer()
                .SetQuestion(expectedQuestion)
                .SetAnswer(expectedAnswer)
                .ClickUploadImage()
                .UploadImageToAnswer()
                .ClickOnSave()
                .ClickOnConfirm();

            _apiFactory
                .ChangeContext<IQEndAApi>(_driver)
                .PatchBrandDeployQEndARequest(DataRep.MgmUrl,
                new string[] { DataRep.MgmQaDevAutoId }, _mgmLoginData);

            var userName = TextManipulation.RandomString();

            var userId = _apiFactory
                .ChangeContext<IUserApi>(_driver)
                .CreateUserForUiPipe(_crmUrl, userName);

            var actualAnswerImage = _apiFactory
                .ChangeContext<ILoginPageUi>(_driver)
                .LoginPipe(userName, _crmUrl)
                .ChangeContext<IMenus>(_driver)
                .ClickOnMenuItem<IQAndAPageUi>()
                .ClickOnQuestionByName(expectedQuestion)
                .VerifyAnswerText(expectedAnswer)
                .GetAnswerImage();

            Assert.Multiple(() =>
            {
                Assert.True(actualAnswerImage.Contains(".png"),
                    $" expected Answer Image: {expectedAnswer}" +
                    $" actual Answer Image Contains: .png");
            });
        }
    }
}