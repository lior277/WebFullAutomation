// Ignore Spelling: exce api req

using AirSoftAutomationFramework.Internals.DAL.ApiAccess;
using AirSoftAutomationFramework.Internals.ExtensionsMethods;
using AirSoftAutomationFramework.Internals.Factory;
using AirSoftAutomationFramework.Internals.Helpers;
using AventStack.ExtentReports;
using AventStack.ExtentReports.Reporter;
using Microsoft.VisualStudio.TestPlatform.Utilities;
using NUnit.Framework;
using NUnit.Framework.Interfaces;
using NUnit.Framework.Internal;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using Path = System.IO.Path;

namespace AirSoftAutomationFramework.Internals.DAL.Report
{
    public class ReportsGenerationClass
    {
        #region Members
        private IApplicationFactory _apiFactory = new ApplicationFactory();
        //private IApiAccess _apiAccess;
        private static string _reportPath;
        private static ExtentReports _extent;
        protected ExtentTest _extentTest;
        protected DateTime time = DateTime.Now;
        protected static string _screenShotName;
        private Status _logStatus;
        private string _output;
        //private TestStatus _status;
        private object _testDescription;
        private string _testMessage;
        private string _stackTrace;
        private static ExtentHtmlReporter _htmlReporter;
        private static string _buildName = DataRep.BuildName;
        #endregion

        public static ExtentReports ReportSetUp()
        {
            //var path = System.Reflection.Assembly.GetCallingAssembly().CodeBase;
            //var actualPath = path.Substring(0, path.LastIndexOf("bin"));
            //var projectPath = new Uri(actualPath).LocalPath;
            //Directory.CreateDirectory(projectPath.ToString() + "Reports");

            if (_extent == null)
            {
                if (RuntimeInformation.IsOSPlatform(System.Runtime.InteropServices.OSPlatform.Linux))
                {
                    _reportPath = "/tmp/report/bla"; // dont know why, the report is exported to /tmp/report (something removing the bla). 
                }
                else
                {
                    var file = @"\Jenkins\workspace\Donet\TestProject\";
                    var absolute_path = Environment.GetEnvironmentVariable("ProgramFiles(x86)") + file;
                    //var path = System.Reflection.Assembly.GetCallingAssembly().CodeBase;
                    //var actualPath = path.Substring(0, path.LastIndexOf("bin"));
                    var projectPath = new Uri(absolute_path).LocalPath;
                    var date = DateTime.Now.ToString("dd.MM.yyyy");
                    var time = DateTime.Now.ToString("HH.mm");
                    var format = "{0} on {1} At {2}";
                    var folderName = string.Format(format, "Report", date, time);
                    _reportPath = Path.Combine(projectPath + "Reports", folderName, "Screenshots");
                }

                Directory.CreateDirectory(_reportPath);
                _htmlReporter = new ExtentHtmlReporter(_reportPath);
                _htmlReporter.Config.EnableTimeline = true;
                _htmlReporter.Config.CSS = "css-string";
                _htmlReporter.Config.DocumentTitle = "Automation report";
                _htmlReporter.Config.Encoding = "utf-8";
                _htmlReporter.Config.JS = "js-string";
                _htmlReporter.Config.ReportName = "Automation report";

                _extent = new ExtentReports();
                _extent.AttachReporter(_htmlReporter);
                _extent.AddSystemInfo("Host Name", "Selenium 4 on kubernetes ");
                _extent.AddSystemInfo("Environment", "QA");
                _extent.AddSystemInfo("UserName", "TestUser");
                _extent.AddSystemInfo("BuildName", _buildName);
            }

            return _extent;
        }

        //public void DeleteFilesAndFoldersInReportDirectory(int numOfDays = -1)
        //{
        //    if (!RuntimeInformation.IsOSPlatform(System.Runtime.InteropServices.OSPlatform.Linux))
        //    {
        //        var _fileHandler = new FileHandler(_apiFactory, _apiAccess);
        //        var file = @"\Jenkins\workspace\Donet\TestProject\Reports";
        //        var absolute_path = $"{Environment.GetEnvironmentVariable("ProgramFiles(x86)")}{file}";

        //        _fileHandler.DeleteFilesOlderThen(absolute_path, numOfDays);
        //        _fileHandler.DeleteDirectoryOlderThen(absolute_path, numOfDays);
        //    }
        //}

        public ExtentTest BeforeTest(string browserName = null)
        {
            var testName = TestContext.CurrentContext.Test.Name;

            testName = testName.Contains("default namespace") ?// if setup is fail?
                TestContext.CurrentContext.Test.FullName :
                $"{TestContext.CurrentContext.Test.Name}" +
                $" :: {browserName?.UpperCaseFirstLetter()}";

            _extentTest = _extent.CreateTest(testName);

            //_extentTest.Log(log4net);
            return _extentTest;
        }

        public void SetTestsStatusBrowserStack(string reqString, string sessionId)
        {
            using var httpClient = new HttpClient();
            using var request = new HttpRequestMessage(new HttpMethod("PUT"), $"https://api.browserstack.com/automate/sessions/{sessionId}.json");
            var base64authorization = Convert.ToBase64String(Encoding.ASCII.GetBytes("liorhalaly1:NqBHTmxyFQ7jYumRjZyi"));
            request.Headers.TryAddWithoutValidation("Authorization", $"Basic {base64authorization}");

            request.Content = new StringContent(reqString);
            request.Content.Headers.ContentType = MediaTypeHeaderValue.Parse("application/json");

            var response = httpClient.SendAsync(request).Result;
        }

        public void ReportTearDown()
        {
            _extent.Flush();
        }

        public void AfterTest(IWebDriver driver = null)
        {
            try
            {
                _output = TestExecutionContext.CurrentContext.CurrentResult.Output;
                var status = TestContext.CurrentContext.Result.Outcome.Status;
                _testDescription = TestContext.CurrentContext.Test.Properties.Get("Description") ?? " ";
                _testMessage = TestContext.CurrentContext.Result.Message ?? " ";
                _stackTrace = TestContext.CurrentContext.Result.StackTrace;
                //stackTrace = string.IsNullOrEmpty(TestContext.CurrentContext.Result.StackTrace) ?
                //    "" : string.Format("{0}", TestContext.CurrentContext.Result.StackTrace);

                //string sessionId = null;

                //if (driver != null)
                //{
                //    //sessionId = ((RemoteWebDriver)driver).SessionId.ToString();
                //}

                switch (status)
                {
                    case TestStatus.Failed:

                        _logStatus = Status.Fail;

                        CaptureScreenShootAndLogging(driver,
                            _stackTrace, _testDescription, _testMessage);

                        break;

                    case TestStatus.Inconclusive:

                        _logStatus = Status.Fail;

                        _extentTest.Log(_logStatus, " TEST DESCRIPTION:" +
                            $" {_testDescription}\n, TEST MESSAGE: {_testMessage}\n,");

                        break;

                    case TestStatus.Skipped:
                        _logStatus = Status.Skip;
                        _extentTest.Log(_logStatus, $" TEST DESCRIPTION:" +
                            $" {_testDescription}\n, TEST MESSAGE: {_testMessage}\n,");

                        break;

                    case TestStatus.Passed:
                        _logStatus = Status.Pass;
                        //sessionId = ((RemoteWebDriver)driver).SessionId.ToString();
                        //var passed = "{\"status\":\"passed\", \"reason\":\"passed\"}";

                        if (driver != null)
                        {
                            //SetTestsStatusBrowserStack(passed, sessionId);
                        }

                        _extentTest.Log(_logStatus, $" TEST DESCRIPTION:" +
                            $" {_testDescription}\n, TEST MESSAGE: {_testMessage}");

                        break;

                    default:
                        Console.WriteLine("Value didn't match earlier.");
                        break;

                }

                //SetStatusHierarchy();

                //_test.Log(_logStatus, $" TEST DESCRIPTION:
                //{testDescription}\n, TEST MESSAGE: { testMessage}\n," +
                //    $" STACK TRACE: {stackTrace}");
                //Stopwatch stopWatch = new Stopwatch();
                //stopWatch.Start();
                //_extent.Flush();
                //stopWatch.Stop();
                //var ts = stopWatch.Elapsed;
                //var ff = ts.TotalSeconds;


            }
            catch (Exception ex)
            {
                var exceMessage = $"driver: {driver}, _output: {_output}," +
                    $" test Description, test Message: {_testMessage}, stack Trace: {_stackTrace}" +
                    $" Exception: {ex?.Message}";

                if (!ex.Message.Contains("invalid session id") && driver != null)
                {
                    CaptureScreenShootAndLogging(driver,
                        _stackTrace, _testDescription, _testMessage);
                }

                throw new Exception(exceMessage);
            }
        }

        private string Capture(IWebDriver driver, string screenShotName)
        {
            try
            {
                if (driver != null)
                {
                    var ts = (ITakesScreenshot)driver;
                    var screenShot = ts.GetScreenshot();

                    try
                    {
                        screenShot.SaveAsFile(Path.Combine(_reportPath, screenShotName),
                            ScreenshotImageFormat.Png);
                    }
                    catch (Exception ex)
                    {
                        throw new Exception(ex.Message);
                    }   
                }
            }
            catch (Exception ex)
            {
                _extentTest.Log(_logStatus, $"TEST MESSAGE: {_testMessage?.ToBold()}");

                var exceMessage = $"Exception from capture: {ex?.Message}";

                throw new Exception(exceMessage);
            }

            return _reportPath;
        }

        private void CaptureScreenShootAndLogging(IWebDriver driver, string stackTrace,
            object testDescription, string testMessage, ExtentTest extentTest = null)
        {
            _extentTest = extentTest ?? _extentTest;
            //_logStatus = Status.Fail;

            //try
            //{
                if (driver != null)
                {
                    _screenShotName = "Screenshot_" + DateTime
                        .Now.ToString("yyyy-MM-dd HH-mm-ss.fff") + ".png";

                    var screenShotPath = Capture(driver, _screenShotName);

                    if (screenShotPath != null)
                    {
                        _extentTest.Log(_logStatus, " TEST DESCRIPTION:" +
                        $" {testDescription}\n, TEST MESSAGE: {testMessage?.ToBold()}\n \n," +
                        $" STACK TRACE: {stackTrace}\n, Snapshot below:" +
                        $" {_extentTest.AddScreenCaptureFromPath(Path.Combine(screenShotPath, _screenShotName))}\n");
                    }
                    else
                    {
                        _extentTest.Log(_logStatus, " TEST DESCRIPTION:" +
                        $" {testDescription}\n, TEST MESSAGE: {testMessage?.ToBold()}\n \n," +
                        $" STACK TRACE: {stackTrace}\n, Snapshot below:\n");
                    }                
                }
                else
                {
                    _extentTest.Log(_logStatus, $" TEST DESCRIPTION: {testDescription}\n, TEST MESSAGE:" +
                        $" {testMessage?.ToBold()}\n, STACK TRACE: {stackTrace}");
                }
            //}
            //catch (Exception ex)
            //{
            //    _extentTest.Log(_logStatus, $" TEST DESCRIPTION: {testDescription}\n, TEST MESSAGE:" +
            //            $" {testMessage?.ToBold()}\n, STACK TRACE: {stackTrace}, from CaptureScreenShootAndLogging");

            //    var exceMessage = $"exception: {ex?.Message}";

            //    throw new Exception(exceMessage);
            //}
        }

        private void SetStatusHierarchy()
        {
            _extent.Config.StatusConfigurator.StatusHierarchy = new List<Status>
            {
                    Status.Fatal,
                    Status.Fail,
                    Status.Error,
                    Status.Warning,
                    Status.Skip,
                    Status.Pass,
                    Status.Debug,
                    Status.Info
            };
        }
    }
}
