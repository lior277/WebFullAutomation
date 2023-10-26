using AirSoftAutomationFramework.Internals.ExtensionsMethods;
using AirSoftAutomationFramework.Internals.Helpers;
using NUnit.Framework;
using OpenQA.Selenium;
using TestsProject.TestsInternals;

namespace TestsProject.Tests.TempTests
{
    [TestFixture(DataRep.Chrome)]
    public class UploadFile : TestSuitBase
    {
        #region members
        private IWebDriver _driver;
        private string _browserName;
        #endregion

        public UploadFile(string browser) : base(browser)
        {
            _browserName = browser;
        }

        private readonly By UploadFileExp = By.CssSelector("input[type='file'][id='residency_proof']");

        [SetUp]
        public void SetUp()
        {
            BeforeTest();
            _driver = GetDriver();
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
        public void UploadFileTest()
        {
            var script = "C:\\Users\\Lior\\Pictures\\Capture.PNG";

           _driver.Navigate().GoToUrl("https://qa-auto01-trade.airsoftltd.com/settings/profile");

           _driver.SearchElement(UploadFileExp)
                .SendKeys(script);
           
        }

              
    }
}
