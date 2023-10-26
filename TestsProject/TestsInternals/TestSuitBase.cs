using AirSoftAutomationFramework.Internals.DAL.Report;
using AirSoftAutomationFramework.Internals.Helpers;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Remote;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace TestsProject.TestsInternals
{
    public class TestSuitBase : ReportsGenerationClass
    {
        private string _remoteWebDriverUrl =
            Environment.GetEnvironmentVariable("RemoteWebDriverUrl") ?? string.Empty;

        private string _browserName;
        private string _resolution;

        public TestSuitBase(string browserName = null, string resolution = "1920x1080")
        {
            _browserName = browserName;
            _resolution = resolution;
        }

        public IWebDriver GetDriver()
        {
            IWebDriver driver = null;
            var chromeOptions = new ChromeOptions();

            try
            {
                //the command to run for proxy : BrowserStackLocal.exe --key NqBHTmxyFQ7jYumRjZyi --force-local
                //var remoteDriverSource = Config.GetValue(nameof(Config.RemoteDriverSource));
                //new DriverManager().SetUpDriver(new ChromeConfig());
                //new DriverManager().SetUpDriver(new FirefoxConfig());

                #region Chrome Options  
                //capabilities.AddArgument("--no-sandbox");
                //capabilities.AddArgument("--disable-dev-shm-usage");
                //capabilities.AddArgument("--ignore-ssl-errors");
                //capabilities.AddAdditionalCapability("resolution", "1920x1080", true);
                //capabilities.AddAdditionalCapability("build", DataRep.BuildName, true);
                //capabilities.AddAdditionalCapability("acceptInsecureCerts", true, true);
                //capabilities.AddAdditionalCapability("result", TestContext.CurrentContext.Result, true);
                //capabilities.AddAdditionalCapability("timezone", _timeZone, true);

                #region Selenium 4
                //chromeOptions.AddArgument("--headless=new");
                chromeOptions.AddArgument("--start-maximized");
                chromeOptions.AddArgument("--no-sandbox");
                chromeOptions.AddArgument("--disable-dev-shm-usage");
                chromeOptions.AddArgument("--ignore-ssl-errors");
                chromeOptions.AddArgument("--disable-browser-side-navigation");
                chromeOptions.AddArgument("--enable-features=NetworkService,NetworkServiceInProcess");
                chromeOptions.AddArgument("--ignore-certificate-errors");

                var cloudOptions = new Dictionary<string, object>();
                cloudOptions.Add("resolution", "1920x1080");
                cloudOptions.Add("buildName", DataRep.BuildName);
                cloudOptions.Add("ACCEPT_SSL_CERTS", true);
                cloudOptions.Add("result", TestContext.CurrentContext.Result);
                cloudOptions.Add("name", TestContext.CurrentContext.Test.MethodName);

                chromeOptions.AddAdditionalOption("cloud:options", cloudOptions);
                //chromeOptions.BrowserVersion = "latest";
                //chromeOptions.AddAdditionalOption("recordVideo", true);         
                #endregion

                //if (remoteDriverSource.Equals("browserstack"))
                {
                    //chromeOptions.AddAdditionalOption("os", "Windows");
                    //chromeOptions.AddAdditionalOption("osversion", "10");
                    //chromeOptions.AddAdditionalOption("acceptSslCerts", "false");
                    //chromeOptions.AddAdditionalOption("local", "true");
                    //chromeOptions.AddAdditionalOption("idleTimeout", "90");
                    //chromeOptions.AddAdditionalOption("wsLocalSupport", "true");
                    //chromeOptions.AddAdditionalOption("consoleLogs", "info");
                    //chromeOptions.AddAdditionalOption("debug", "true");
                    //chromeOptions.AddAdditionalOption("networkLogs", "true");
                    //chromeOptions.AddAdditionalOption("timezone", _timeZone);
                    //chromeOptions.AddArguments("ignore-certificate-errors");
                    //chromeOptions.AddAdditionalOption("bstack:options", chromeOptions);

                    //chromeOptions.AddAdditionalCapability("os", "Windows", true);
                    //chromeOptions.AddAdditionalCapability("os_version", "10", true);
                    //chromeOptions.AddArguments("ignore-certificate-errors");
                    //chromeOptions.AddAdditionalCapability("browserstack.acceptSslCerts", "false", true);
                    //chromeOptions.AddAdditionalCapability("browserstack.local", "true", true);
                    //chromeOptions.AddAdditionalCapability("browserstack.idleTimeout", "90", true);
                    //chromeOptions.AddAdditionalCapability("browserstack.wsLocalSupport", "true", true);
                    //chromeOptions.AddAdditionalCapability("browserstack.console", "info", true);
                    //chromeOptions.AddAdditionalCapability("browserstack.debug", "true", true);
                }
                #endregion

                #region firefox Options
                //firefoxOptions = new FirefoxOptions();
                //firefoxOptions.AddAdditionalCapability("resolution", "1920x1080", true);
                //firefoxOptions.AddArgument("--no-sandbox");
                //firefoxOptions.AddAdditionalCapability("build", DataRep.BuildName, true);
                //firefoxOptions.AddAdditionalCapability("result", TestContext.CurrentContext.Result, true);
                //firefoxOptions.AddAdditionalCapability("name", TestContext.CurrentContext.Test.MethodName, true);

                //if (remoteDriverSource.Equals("browserstack"))
                //{
                //firefoxOptions.AddAdditionalCapability("os", "Windows", true);
                //firefoxOptions.AddAdditionalCapability("os_version", "10", true);
                //firefoxOptions.AddArguments("ignore-certificate-errors");
                //firefoxOptions.AddAdditionalCapability("browserstack.acceptSslCerts", "false", true);
                //firefoxOptions.AddAdditionalCapability("browserstack.local", "true", true);
                //firefoxOptions.AddAdditionalCapability("browserstack.idleTimeout", "90", true);
                //firefoxOptions.AddAdditionalCapability("browserstack.wsLocalSupport", "true", true);
                //firefoxOptions.AddAdditionalCapability("browserstack.console", "info", true);
                //firefoxOptions.AddAdditionalCapability("browserstack.debug", "true", true);

                //firefoxOptions.AddAdditionalOption("os", "Windows");
                //firefoxOptions.AddAdditionalOption("osversion", "10");
                //firefoxOptions.AddArguments("ignore-certificate-errors");
                //firefoxOptions.AddAdditionalOption("local", "true");
                //firefoxOptions.AddAdditionalOption("browserstack.idleTimeout", "90");
                //firefoxOptions.AddAdditionalOption("browserstack.wsLocalSupport", "true");
                //firefoxOptions.AddAdditionalOption("consoleLogs", "info");
                //firefoxOptions.AddAdditionalOption("debug", "true");
                //firefoxOptions.AddAdditionalOption("networkLogs", "true");
                //firefoxOptions.AddAdditionalOption("timezone", _timeZone);
                //}
                #endregion

                switch (_browserName)
                {
                    case "chrome":
                        if (_remoteWebDriverUrl != string.Empty && _remoteWebDriverUrl != null)
                        {
                            CodePagesEncodingProvider.Instance.GetEncoding(437);
                            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

                            //driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(180);

                            return new RemoteWebDriver(new Uri(_remoteWebDriverUrl), chromeOptions);
                        }
                        else
                        {
                            //return new RemoteWebDriver(new Uri("https://sel.airsoftltd.com"), chromeOptions);
                            //return new RemoteWebDriver(new Uri("https://liorhalaly1:NqBHTmxyFQ7jYumRjZyi@hub-cloud.browserstack.com/wd/hub"), chromeOptions.ToCapabilities());
                            return new ChromeDriver(chromeOptions);
                        }
                    case null:
                        {
                            throw new NullReferenceException($"browser name: {_browserName} will not run");

                            //return null;
                        }

                    #region Firafox
                    //case "firefox":
                    //    if (_remoteWebDriverUrl != string.Empty)
                    //    {
                    //        CodePagesEncodingProvider.Instance.GetEncoding(437);
                    //        Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

                    //        //var driver = new RemoteWebDriver(new Uri(_remoteWebDriverUrl),
                    //        //    firefoxOptions.ToCapabilities());
                    //            //TimeSpan.FromSeconds(60));

                    //        return driver;
                    //    }
                    //else
                    //{
                    //    var geckoDriverDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
                    //    var geckoService = FirefoxDriverService.CreateDefaultService(geckoDriverDirectory);
                    //    geckoService.Host = "::1";

                    //    //return new RemoteWebDriver(new Uri("https://sel.airsoftltd.com/wd/hub"), firefoxOptions.ToCapabilities());
                    //    //return new RemoteWebDriver(new Uri("https://liorhalaly1:NqBHTmxyFQ7jYumRjZyi@hub-cloud.browserstack.com/wd/hub"), firefoxOptions.ToCapabilities());
                    //    var driver = new FirefoxDriver(geckoService, firefoxOptions);

                    //    return driver;
                    //}
                    #endregion

                    default:
                        {
                            return null;
                        }
                }
            }
            catch (Exception ex)
            {             
                var exceptionMessage = $" _remote Web Driver Url: {_remoteWebDriverUrl}," +
                    $"  _browser Name: {_browserName}, driver: {driver}, Exception: {ex.Message} ";

                throw new Exception(exceptionMessage);
            }
        }

        public void DriverDispose(IWebDriver driver = null)
        {
            try
            {
                //driver.Close();
                driver.Quit();
                Thread.Sleep(10);
                driver = null;
            }
            catch (Exception ex)
            {
                var exceMessage =  $"Message: {ex?.Message}";

                throw new Exception(exceMessage);
            }
        }
    }
}