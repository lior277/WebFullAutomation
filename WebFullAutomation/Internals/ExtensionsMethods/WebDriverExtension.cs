using AirSoftAutomationFramework.Internals.Helpers;
using Microsoft.Graph.Models.CallRecords;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.DataCollection;
using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Remote;
using OpenQA.Selenium.Support.Extensions;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;
using static AirSoftAutomationFramework.Internals.Enums.EnumFactory;

namespace AirSoftAutomationFramework.Internals.ExtensionsMethods
{
    public static class WebDriverExtension
    {
        public static void WaitForStaleElementError()
        {
            Thread.Sleep(400);
        }

        public static bool CheckIfElementIsOnTop(this IWebElement element,
            IWebDriver driver, int fromSeconds = DataRep.TimeToWaitFromSeconds)
        {
            var js = (IJavaScriptExecutor)driver;

            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(fromSeconds));

            wait.IgnoreExceptionTypes(
                typeof(NoSuchElementException),
                typeof(ElementNotVisibleException),
                typeof(ElementNotSelectableException),
                typeof(InvalidSelectorException),
                typeof(NoSuchFrameException),
                typeof(ElementNotInteractableException),
                typeof(WebDriverException));

            var elements = wait.Until(d =>
            {
                return (bool)js.ExecuteScript(
                "var elm = arguments[0];" +
                "var doc = elm.ownerDocument || document;" +
                "var rect = elm.getBoundingClientRect();" +
                "return elm === doc.elementFromPoint(rect.left + " +
                "(rect.width / 2), rect.top + (rect.height / 2));"
                , element);
            });

            return true;
        }

        //public static bool SteadinessOfElementLocated(this IWebElement element,
        //    IWebDriver driver, By by, int fromSeconds = DataRep.TimeToWaitFromSeconds)
        //{
        //    var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(fromSeconds));

        //    wait.IgnoreExceptionTypes(
        //        typeof(NoSuchElementException),
        //        typeof(ElementNotVisibleException),
        //        typeof(ElementNotSelectableException),
        //        typeof(InvalidSelectorException),
        //        typeof(NoSuchFrameException),
        //        typeof(ElementNotInteractableException),
        //        typeof(WebDriverException));

        //    WebElement _element;
        //    var _location = new Point();


        //    try
        //    {
        //        var elements = wait.Until(d =>
        //        {

        //            var actualLocation = element.Location;
        //            if (actualLocation.Equals(_location) && ElementIsOnTop(_element))
        //            {
        //                return _element;
        //            }
        //            _location = location;
        //        };
        //    }
        //    catch (StaleElementReferenceException e)
        //    {
        //        _element = null;
        //    }

        //    return null;
        //}



        public static bool CheckIfElementExist(this IWebDriver driver, By by,
                    int fromSeconds = DataRep.TimeToWaitFromSeconds)
        {
            int numOfElements;
            var rounds = 40;

            while (rounds > 0)
            {
                numOfElements = driver.FindElements(by).Count;
                if (numOfElements > 0)
                {
                    return true;
                }
                rounds--;
                Thread.Sleep(100);
            }

            return false;
        }

        public static List<IWebElement> GetElementsOfElement(this IWebElement element,
          IWebDriver driver, By by, int fromSeconds = DataRep.TimeToWaitFromSeconds)
        {
            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(fromSeconds));
            wait.IgnoreExceptionTypes(
                typeof(NoSuchElementException),
                typeof(ElementNotVisibleException),
                typeof(ElementNotSelectableException),
                typeof(InvalidSelectorException),
                typeof(NoSuchFrameException),
                typeof(ElementNotInteractableException),
                typeof(WebDriverException));

            var elements = wait.Until(d =>
            {
                IList<IWebElement> elementsToSearch;

                try
                {
                    elementsToSearch = driver.FindElements(by);

                    if (elementsToSearch.Count < 2)
                    {
                        return null;
                    }

                    return elementsToSearch;
                }
                catch (StaleElementReferenceException)
                {
                    WaitForStaleElementError();

                    return null;
                }
            });
            return elements.ToList();
        }

        public static void WaitForElementToBeEnable(this IWebDriver driver,
            By by, int fromSeconds = DataRep.TimeToWaitFromSeconds)
        {
            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(fromSeconds));

            wait.Until(d =>
            {
                try
                {
                    var elementEnabled = driver.FindElement(by);
                    return elementEnabled.Enabled;
                }
                catch
                {
                    return false;
                }
            });
        }

        public static void WaitForUrlToContain(this IWebDriver driver,
            string urlContains, int fromSeconds = DataRep.TimeToWaitFromSeconds)
        {
            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(fromSeconds));
            wait.IgnoreExceptionTypes(
                typeof(NoSuchElementException),
                typeof(ElementNotVisibleException),
                typeof(ElementNotInteractableException));

            var actualUrl = wait.Until(d =>
            {
                try
                {
                    if (!driver.Url.Contains(urlContains))
                    {
                        return false;
                    }

                    return true;
                }
                catch (Exception)
                {
                    return false;
                }
            });
        }

        public static void GetElementWitJavaScript(this IWebDriver driver, By by)
        {
            _ = driver.ExecuteJavaScript<IWebElement>("return $('.my-class')[0]");

            var elementToPass = driver.FindElement(by);
            _ = driver.ExecuteJavaScript<IReadOnlyCollection<IWebElement>>(
                        "return $(arguments[0]).children('.my-class').toArray()", elementToPass);
        }

        public static void GetSwitchToAlert(this IWebDriver driver)
        {
            var a = driver.SwitchTo().Alert();
            a.Accept();
            a.Dismiss();
        }

        public static void WaitForUrlToChange(this IWebDriver driver, string url,
            int fromSeconds = DataRep.TimeToWaitFromSeconds)
        {
            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(fromSeconds));
            wait.IgnoreExceptionTypes(
                typeof(NoSuchElementException),
                typeof(ElementNotVisibleException),
                typeof(ElementNotSelectableException),
                typeof(InvalidSelectorException),
                typeof(NoSuchFrameException),
                typeof(ElementNotInteractableException),
                typeof(WebDriverException));

            wait.Until(d =>
            {

                var currrentUrl = driver.Url;

                if (!currrentUrl.Contains(url))
                {
                    return false;
                }

                return true;

            });
        }

        public static void WaitForNumberOfWindows(this IWebDriver driver, int expectedNumOfWindows,
            int fromSeconds = DataRep.TimeToWaitFromSeconds)
        {
            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(fromSeconds));
            wait.Until(wd => wd.WindowHandles.Count == 2);
        }

        public static void OpenNewWindowAndSwitch(this IWebDriver driver,
            By by, string targetUrl, int fromSeconds = DataRep.TimeToWaitFromSeconds)
        {
            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(fromSeconds));

            wait.Until(d =>
            {
                driver.SearchElement(by)
                .ForceClick(driver, by);

                driver.WaitForNumberOfWindows(2);
                var windows = driver.WindowHandles;

                //if (lastOrFirstWindow == TabToSwitch.First)
                //{
                //    driver.SwitchTo().Window(windows.First());
                //    WaitForUrlToContain(driver, targetUrl);
                //}
                //else
                //{
                driver.SwitchTo().Window(windows.Last());
                WaitForUrlToContain(driver, targetUrl);
                //}

                return true;
            });
        }

        public static void SwitchToExistingWindow(this IWebDriver driver,
            TabToSwitch tabToSwitch, int fromSeconds = DataRep.TimeToWaitFromSeconds)
        {
            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(fromSeconds));

            wait.Until(d =>
            {
                var windows = driver.WindowHandles;

                if (tabToSwitch == TabToSwitch.First)
                {
                    driver.SwitchTo().Window(windows.First());
                }
                else
                {
                    driver.SwitchTo().Window(windows.Last());
                }

                return true;
            });
        }

        //public static void GetAddCookie(this IWebDriver driver, string key, string value)
        //{
        //    var cookie = new Cookie(key, value);
        //    driver.Manage().Cookies.AddCookie(cookie);
        //}

        public static void GetAllCookies(this IWebDriver driver)
        {
            _ = driver.Manage().Cookies.AllCookies;
        }

        public static void GetDeleteCookieByName(this IWebDriver driver, string cookieName)
        {
            driver.Manage().Cookies.DeleteCookieNamed(cookieName);
        }

        public static void GetDeleteAllCookies(this IWebDriver driver)
        {
            driver.Manage().Cookies.DeleteAllCookies();
        }

        public static void GetFullScreenShot(this IWebDriver driver, string pathToSaveImage)
        {
            var screenshot = ((ITakesScreenshot)driver).GetScreenshot();
            screenshot.SaveAsFile(pathToSaveImage, ScreenshotImageFormat.Png);
        }

        public static void WaitForPageLoad(this IWebDriver driver, string expectedPageName,
            int fromSeconds = DataRep.TimeToWaitFromSeconds)
        {
            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(fromSeconds));

            wait.Until(d =>
            {
                try
                {
                    var actualUrl = driver.Url;

                    if (actualUrl.Contains(expectedPageName))
                    {
                        return true;
                    }

                    return false;
                }
                catch (StaleElementReferenceException)
                {
                    Thread.Sleep(500);

                    return false;
                }
            });
        }

        //public static bool CheckIfElementExist(this IWebElement element,
        //    IWebDriver driver, int fromSeconds = 20)
        //{
        //    var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(fromSeconds));

        //    wait.IgnoreExceptionTypes(
        //        typeof(NoSuchElementException),
        //        typeof(ElementNotVisibleException),
        //        typeof(ElementNotSelectableException),
        //        typeof(InvalidSelectorException),
        //        typeof(NoSuchFrameException),
        //        typeof(ElementNotInteractableException),
        //        typeof(WebDriverException));

        //    var visible = wait.Until(d =>
        //    {
        //        try
        //        {
        //            var isVisible = driver.IsVisibleInViewPort(element);

        //            if (!isVisible)
        //            {
        //                return true;
        //            }

        //            return false;
        //        }
        //        catch (StaleElementReferenceException)
        //        {
        //            Thread.Sleep(500);

        //            return false;
        //        }
        //    });

        //    return visible;
        //}

        public static void WaitForElementNotExist(this IWebDriver driver,
            By by, double fromSeconds = DataRep.TimeToWaitFromSeconds)
        {
            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(fromSeconds));

            wait.IgnoreExceptionTypes(
                typeof(NoSuchElementException),
                typeof(ElementNotVisibleException),
                typeof(ElementNotSelectableException),
                typeof(InvalidSelectorException),
                typeof(NoSuchFrameException),
                typeof(ElementNotInteractableException),
                typeof(WebDriverException));

            try
            {
                wait.Until(d =>
                {
                    if (driver == null)
                    {
                        return false;
                    }

                    try
                    {
                        driver?.FindElement(by);

                        return false;
                    }
                    catch (StaleElementReferenceException)
                    {
                        WaitForStaleElementError();

                        return false;
                    }
                    catch (NoSuchElementException)
                    {
                        return true;
                    }
                });
            }
            catch (Exception ex)
            {
                var exceMessage = ($" search parameters: {by?.ToString()?.ToBold()}," +
                    $" element is, Exception: {ex?.Message}, driver: {driver} ");

                var newException = new Exception(exceMessage);

                throw newException;
            }
        }

        public static void WaitForDataTableToLoad(this IWebDriver driver,
            By by, int fromSeconds = DataRep.TimeToWaitFromSeconds)
        {
            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(fromSeconds));

            wait.IgnoreExceptionTypes(
                typeof(ElementNotInteractableException),
                typeof(ElementNotVisibleException),
                typeof(ElementNotSelectableException),
                typeof(InvalidSelectorException),
                typeof(NoSuchFrameException),
                typeof(WebDriverException));

            try
            {
                wait.Until(d =>
                {
                    try
                    {
                        var element = driver.FindElement(by);

                        return true;
                    }
                    catch (StaleElementReferenceException)
                    {
                        WaitForStaleElementError();

                        return false;
                    }
                    catch (NoSuchElementException)
                    {
                        return false;
                    }
                });
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static void NavigateToPageByName(this IWebDriver driver,
            string url, string pageName, bool checkUrl = true,
            int fromSeconds = DataRep.TimeToWaitFromSeconds)
        {
            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(fromSeconds));
            string pageAddress = null;

            wait.IgnoreExceptionTypes(
                typeof(ElementNotInteractableException),
                typeof(ElementNotVisibleException),
                typeof(ElementNotSelectableException),
                typeof(InvalidSelectorException),
                typeof(NoSuchFrameException),
                typeof(WebDriverException));
            try
            {
                wait.Until(d =>
                {
                    try
                    {
                        var pageAddress = driver.Url;

                        if (pageAddress == null)
                        {
                            return false;
                        }

                        //Thread.Sleep(1500);

                        // dashboard is the starting point for each and other page
                        if (!pageAddress.Contains("dashboard"))
                        {
                            pageAddress = url + "/dashboard";
                            driver.Navigate().GoToUrl(pageAddress);

                            return false;
                        }

                        driver.Navigate().GoToUrl(url + pageName);

                        if (!checkUrl)
                        {
                            return true;
                        }

                        pageAddress = driver.Url;

                        if (pageAddress == null)
                        {
                            return false;
                        }

                        if (!pageAddress.Contains(pageName))
                        {
                            return false;
                        }

                        return true;
                    }
                    catch (Exception ex)
                    {
                        var sessionIdProperty = typeof(RemoteWebDriver)
                        .GetProperty("SessionId", BindingFlags.Instance | BindingFlags.NonPublic);
                        OpenQA.Selenium.SessionId sessionId = null;

                        if (sessionIdProperty != null)
                        {
                            sessionId = sessionIdProperty?.GetValue(driver, null)
                            as OpenQA.Selenium.SessionId;
                        }

                        var exceMessage = $"page name: {pageName}, actual url: {pageAddress}," +
                        $" Exception: {ex?.Message}, session id: {sessionId}";

                        var newException = new Exception(exceMessage);

                        throw newException;
                    }
                });
            }
            catch (Exception ex)
            {
                if (pageName != null)
                {
                    var exceMessage = $"page name: {pageName}, actual url: {driver?.Url},Exception: {ex?.Message}";
                    var newException = new Exception(exceMessage);

                    throw newException;
                }
            }
        }

        public static void WaitForAtListOneElement(this IWebDriver driver,
            By by, int fromSeconds = DataRep.TimeToWaitFromSeconds)
        {
            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(fromSeconds));
            wait.IgnoreExceptionTypes(
                typeof(NoSuchElementException),
                typeof(ElementNotVisibleException),
                typeof(ElementNotSelectableException),
                typeof(InvalidSelectorException),
                typeof(NoSuchFrameException),
                typeof(ElementNotInteractableException),
                typeof(WebDriverException));

            wait.Until(d =>
            {
                try
                {
                    var numOfRows = driver.FindElements(by).Count;

                    if (numOfRows < 1)
                    {
                        //Thread.Sleep(200);

                        return false;
                    }
                    else
                    {
                        return true;
                    }
                }
                catch (StaleElementReferenceException)
                {
                    WaitForStaleElementError();

                    return false;
                }
            });
        }

        public static void DrawSignatureOnCanvas(this IWebElement element,
            IWebDriver driver, By by, int fromSeconds = DataRep.TimeToWaitFromSeconds)
        {
            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(fromSeconds));
            wait.IgnoreExceptionTypes(
                typeof(NoSuchElementException),
                typeof(ElementNotVisibleException),
                typeof(ElementNotSelectableException),
                typeof(InvalidSelectorException),
                typeof(NoSuchFrameException),
                typeof(ElementNotInteractableException),
                typeof(WebDriverException));

            wait.Until(d =>
            {
                driver.ExecuteJavaScript(
                       "arguments[0]" + ".scrollIntoView(false);", element);
                try
                {
                    var builder = new Actions(driver);
                    builder.ClickAndHold(element).Perform();
                    builder.MoveByOffset(150, 50).Perform();
                    builder.MoveToElement(element).Perform();

                    ((WebDriver)driver).ResetInputState();

                    return true;
                }
                catch (StaleElementReferenceException)
                {
                    WaitForStaleElementError();

                    return false;
                }
            });
        }

        public static IList<IWebElement> SearchElements(this IWebDriver driver,
            By by, double fromSeconds = DataRep.TimeToWaitFromSeconds)
        {
            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(fromSeconds));

            wait.IgnoreExceptionTypes(
                typeof(NoSuchElementException),
                typeof(ElementNotVisibleException),
                typeof(ElementNotSelectableException),
                typeof(InvalidSelectorException),
                typeof(NoSuchFrameException),
                typeof(ElementNotInteractableException),
                typeof(WebDriverException));

            IList<IWebElement> elementsToSearch = null;

            try
            {
                return wait.Until(d =>
                {
                    try
                    {
                        elementsToSearch = driver.FindElements(by);

                        //if (elementsToSearch.Count == 0)
                        //{
                        //    return null;
                        //}

                        return elementsToSearch;
                    }
                    catch (StaleElementReferenceException)
                    {
                        WaitForStaleElementError();

                        return null;
                    }
                });
            }
            catch (Exception ex)
            {
                var exceMessage = $"elements To Search: {elementsToSearch.ToList()}, Exception: {ex?.Message}";
                var newException = new Exception(exceMessage);

                throw newException;
            }
        }

        public static IList<IWebElement> CheckIfElementsExist(this IWebDriver driver,
            By by, int fromSeconds = DataRep.TimeToWaitFromSeconds)
        {
            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(fromSeconds));
            wait.IgnoreExceptionTypes(
                typeof(NoSuchElementException),
                typeof(ElementNotVisibleException),
                typeof(ElementNotSelectableException),
                typeof(InvalidSelectorException),
                typeof(NoSuchFrameException),
                typeof(ElementNotInteractableException),
                typeof(WebDriverException));

            //driver.WaitForPageToLoad();

            IList<IWebElement> elementsToSearch = null;

            var elements = wait.Until(d =>
            {
                try
                {
                    elementsToSearch = driver.FindElements(by);

                    if (elementsToSearch.Count() == 0)
                    {
                        Thread.Sleep(10);

                        return null;
                    }
                    else
                    {
                        return elementsToSearch;
                    }
                }
                catch (StaleElementReferenceException)
                {
                    WaitForStaleElementError();

                    return null;
                }
            });

            return elements.ToList();
        }

        public static string GetElementText(this IWebElement firstSearch, IWebDriver driver,
            By by = null, int fromSeconds = DataRep.TimeToWaitFromSecondsForElementBased)
        {
            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(fromSeconds));

            wait.IgnoreExceptionTypes(
                typeof(NoSuchElementException),
                typeof(ElementNotVisibleException),
                typeof(ElementNotSelectableException),
                typeof(InvalidSelectorException),
                typeof(NoSuchFrameException),
                typeof(ElementNotInteractableException),
                typeof(WebDriverException));

            var elementText = "";
            IWebElement webElement;
            var newString = "";

            //try
            //{
            var actualElementText = wait.Until(d =>
            {
                try
                {
                    if (by != null)
                    {
                        webElement = driver.FindElement(by);
                    }
                    else
                    {
                        webElement = firstSearch;
                    }

                    newString = ((IJavaScriptExecutor)driver)
                        .ExecuteScript("return arguments[0].innerText;", webElement)?
                        .ToString();

                    //newString = webElement.Text;

                    if (string.IsNullOrEmpty(newString))
                    {
                        newString = ((IJavaScriptExecutor)driver)
                        .ExecuteScript("return arguments[0].value;", webElement)?
                        .ToString();

                        return newString ?? "";
                    }

                    if (newString != null && !string.IsNullOrEmpty(newString))
                    {
                        var temp = newString.Trim().RemoveNewLine();
                        newString = temp;
                    }

                    return newString;
                }
                catch (StaleElementReferenceException)
                {
                    //Console.WriteLine("Stale Element");
                    WaitForStaleElementError();

                    return null;
                }
            });

            if (elementText == null)
            {
                //Console.WriteLine($"element Text is null: {elementText}");
            }

            return newString ?? "";
        }

        public static int WaitForExactNumberOfElements(this IWebDriver driver,
            By by, int expectedNumOfElements,
            int fromSeconds = DataRep.TimeToWaitFromSeconds, string additionalData = null)
        {
            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(fromSeconds));
            wait.IgnoreExceptionTypes(
                typeof(NoSuchElementException),
                typeof(ElementNotVisibleException),
                typeof(ElementNotSelectableException),
                typeof(InvalidSelectorException),
                typeof(NoSuchFrameException),
                typeof(ElementNotInteractableException),
                typeof(WebDriverException));

            var actualNumOfElements = 0;
            List<IWebElement> elements = null;

            try
            {
                var element = wait.Until(d =>
                {
                    try
                    {
                        elements = driver.FindElements(by).ToList();
                        actualNumOfElements = elements.Count();

                        if (actualNumOfElements == expectedNumOfElements)
                        {
                            return elements;
                        }
                        else
                        {
                            return null;
                        }
                    }
                    catch (StaleElementReferenceException)
                    {
                        WaitForStaleElementError();

                        return null;
                    }
                });

                return actualNumOfElements;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static List<IWebElement> WaitForAtListNumberOfElements(this IWebDriver driver,
           By by, int expectedNumOfElements, double fromSeconds = 70, string additionalData = null)
        {
            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(fromSeconds));
            wait.IgnoreExceptionTypes(
                typeof(NoSuchElementException),
                typeof(ElementNotVisibleException),
                typeof(ElementNotSelectableException),
                typeof(InvalidSelectorException),
                typeof(NoSuchFrameException),
                typeof(ElementNotInteractableException),
                typeof(WebDriverException));

            var actualNumOfElements = 0;
            var elements = new List<IWebElement>();

            try
            {
                var element = wait.Until(d =>
                {
                    try
                    {
                        elements = driver.FindElements(by).ToList();
                        actualNumOfElements = elements.Count;

                        if (actualNumOfElements == 0)
                        {
                            return false;
                        }

                        if (actualNumOfElements > expectedNumOfElements)
                        {
                            return true;
                        }

                        if (actualNumOfElements == expectedNumOfElements)
                        {
                            return true;
                        }

                        return true;
                    }
                    catch (StaleElementReferenceException)
                    {
                        WaitForStaleElementError();

                        return false;
                    }
                });
            }
            catch (Exception ex)
            {
                var exceMessage = $" actual Num Of Elements: {actualNumOfElements}" +
                    $" expected Num Of Elements: {expectedNumOfElements}, Exception: {ex?.Message}";

                throw new Exception(exceMessage);
            }

            return elements;
        }

        private static void ElementIfVisible(this IWebElement element, IWebDriver driver)
        {
            var js = (IJavaScriptExecutor)driver;
            var displayed = element.Displayed ? element : null;

            if (displayed == null)
            {
                js.ExecuteScript("arguments[0].scrollIntoView(false);", element);
                Thread.Sleep(500);
            }

            if (displayed == null)
            {
                js.ExecuteScript("arguments[0].scrollIntoView(true);", element);
                Thread.Sleep(500);
            }
        }

        private static void ScrollToElement(this IWebElement element, IWebDriver driver, By by,
            int fromSeconds = DataRep.TimeToWaitFromSeconds)
        {
            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(fromSeconds));

            wait.IgnoreExceptionTypes(
                typeof(ElementNotVisibleException),
                typeof(ElementNotSelectableException),
                typeof(InvalidSelectorException),
                typeof(NoSuchFrameException),
                typeof(WebDriverException));

            IWebElement elementToFind = null;

            wait.Until(d =>
            {
                try
                {
                    elementToFind = driver.FindElement(by);

                    if (elementToFind.Enabled)
                    {
                        var actions = new Actions(driver);
                        actions.MoveToElement(element).Perform();

                        return true;
                    }

                    return true;
                }
                catch (StaleElementReferenceException)
                {
                    WaitForStaleElementError();

                    return false;
                }
                catch (MoveTargetOutOfBoundsException)
                {
                    return true;
                }
                catch (ElementNotInteractableException)
                {
                    return false;
                }
            });
        }

        public static IWebElement ScrollDown(this IWebDriver driver, By by,
           int fromSeconds = DataRep.TimeToWaitFromSeconds, string log = null)
        {
            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(fromSeconds));

            wait.IgnoreExceptionTypes(
                typeof(ElementNotVisibleException),
                typeof(ElementNotSelectableException),
                typeof(InvalidSelectorException),
                typeof(NoSuchFrameException),
                typeof(ElementNotInteractableException),
                typeof(WebDriverException));

            IWebElement elementToFind = null;

            try
            {
                var js = (IJavaScriptExecutor)driver;

                return wait.Until(d =>
                {
                    try
                    {
                        elementToFind = driver.FindElement(by);

                        //var isVisible = driver.IsVisibleInViewPort(by);

                        if (elementToFind == null)
                        {
                            return null;
                        }

                        if (elementToFind.Enabled)
                        {
                            js.ExecuteScript("arguments[0].scrollIntoView(false);", elementToFind);
                            Thread.Sleep(500);

                            return elementToFind; // isVisible ? elementToFind : null;
                        }

                        return null;
                    }
                    catch (StaleElementReferenceException)
                    {
                        Thread.Sleep(500);

                        return null;
                    }
                });
            }
            catch (Exception ex)
            {
                var exceMessage = ($" search message: {log}," +
                    $"  search parameters: {by}, Exception: {ex?.Message} ");

                throw new Exception(exceMessage);
            }
        }

        public static IWebElement ScrollUp(this IWebDriver driver, By by,
          int fromSeconds = DataRep.TimeToWaitFromSeconds, string log = null)
        {
            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(fromSeconds));

            wait.IgnoreExceptionTypes(
                typeof(ElementNotVisibleException),
                typeof(ElementNotSelectableException),
                typeof(InvalidSelectorException),
                typeof(NoSuchFrameException),
                typeof(ElementNotInteractableException),
                typeof(WebDriverException));

            IWebElement elementToFind = null;

            try
            {
                var js = (IJavaScriptExecutor)driver;

                return wait.Until(d =>
                {
                    try
                    {
                        elementToFind = driver.FindElement(by);

                        //var isVisible = driver.IsVisibleInViewPort(by);

                        if (elementToFind == null)
                        {
                            return null;
                        }

                        if (elementToFind.Enabled)
                        {
                            js.ExecuteScript("arguments[0].scrollIntoView(true);", elementToFind);

                            return elementToFind; // isVisible ? elementToFind : null;
                        }

                        return null;
                    }
                    catch (StaleElementReferenceException)
                    {
                        WaitForStaleElementError();

                        return null;
                    }
                });
            }
            catch (Exception ex)
            {
                var exceMessage = ($" search message: {log}," +
                    $"  search parameters: {by}, Exception: {ex?.Message} ");

                throw new Exception(exceMessage);
            }
        }

        public static IWebElement SearchElement(this IWebDriver driver, By by,
            int fromSeconds = DataRep.TimeToWaitFromSeconds, string log = null)
        {
            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(fromSeconds));

            wait.IgnoreExceptionTypes(
                typeof(ElementNotVisibleException),
                typeof(ElementNotSelectableException),
                typeof(InvalidSelectorException),
                typeof(NoSuchFrameException),
                typeof(ElementNotInteractableException),
                typeof(WebDriverException));

            IWebElement elementToFind = null;

            try
            {
                var js = (IJavaScriptExecutor)driver;

                return wait.Until(d =>
                {
                    try
                    {
                        if (driver == null)
                        {
                            return null;
                        }

                        elementToFind = driver.FindElement(by);
                        //elementToFind.CheckIfElementIsOnTop(driver);
                        //isVisible = driver.IsVisibleInViewPort(by);

                        if (elementToFind == null)
                        {
                            return null;
                        }

                        if (!elementToFind.Enabled)
                        {
                            return null;
                        }

                        return elementToFind;
                    }
                    catch (StaleElementReferenceException)
                    {
                        WaitForStaleElementError();

                        return null;
                    }
                    catch (NoSuchElementException)
                    {
                        return null;
                    }
                });
            }
            catch (Exception ex)
            {
                var exceMessage = ($" search message: {log?.ToBold()}, Search Element method" +
                    $" search parameters: {by?.ToString()?.ToBold()} ," +
                    $" Exception: {ex?.Message} ");

                var newException = new Exception(exceMessage);

                throw newException;
            }
        }

        public static IWebElement WaitElementToStopMoving(this IWebElement element, IWebDriver driver, By by,
            int fromSeconds = DataRep.TimeToWaitFromSeconds, string log = null)
        {
            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(fromSeconds));

            wait.IgnoreExceptionTypes(
                typeof(ElementNotVisibleException),
                typeof(ElementNotSelectableException),
                typeof(InvalidSelectorException),
                typeof(NoSuchFrameException),
                typeof(ElementNotInteractableException),
                typeof(WebDriverException));

            try
            {
                wait.Until(d =>
                {
                    var element = driver.FindElement(by);
                    var initialLocation = element.Location;
                    var initialX = initialLocation.X;
                    var initialY = initialLocation.Y;
                    Thread.Sleep(500);
                    var finalLocation = element.Location;
                    var finalX = finalLocation.X;
                    var finalY = finalLocation.Y;

                    if (initialX == finalX && initialY == finalY)
                    {
                        return element;
                    }

                    return null;
                });
            }
            catch (StaleElementReferenceException)
            {
                WaitForStaleElementError();

                return null;
            }

            return element;
        }

        public static void ForceClick(this IWebElement element, IWebDriver driver,
            By by = null, int fromSeconds = DataRep.TimeToWaitFromSecondsForElementBased)
        {
            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(fromSeconds));
            var actions = new Actions(driver);

            wait.IgnoreExceptionTypes(
                typeof(NoSuchElementException),
                typeof(ElementNotSelectableException),
                typeof(InvalidSelectorException),
                typeof(NoSuchFrameException),
                typeof(ElementNotVisibleException),
                typeof(ElementClickInterceptedException),
                typeof(WebDriverException));

            try
            {
                var js = (IJavaScriptExecutor)driver;

                wait.Until(d =>
                {
                    try
                    {
                        if (driver == null)
                        {
                            return false;
                        }

                        if (by != null)
                        {
                            element = driver.FindElement(by);
                        }

                        if (element.Enabled)
                        {
                            element.Click();
                        }

                        return true;
                    }
                    catch (StaleElementReferenceException)
                    {
                        WaitForStaleElementError();

                        return false;
                    }
                    catch (ElementNotInteractableException)
                    {
                        if (by != null)
                        {
                            driver.ScrollElementToView(by);
                        }

                        //actions.MoveToElement(element).Build().Perform();

                        //driver.ExecuteJavaScript(
                        //    "arguments[0]" + ".scrollIntoView(false);", element);

                        return false;
                    }
                    catch (MoveTargetOutOfBoundsException)
                    {
                        return true;
                    }
                });
            }
            catch (Exception ex)
            {
                var exceMessage = $" css selector: {by}, Exception: {ex?.Message}";

                throw new Exception(exceMessage);
            }
        }

        public static IWebElement MoveToElementAndClick(this IWebDriver driver, By by, int fromSeconds =
            DataRep.TimeToWaitFromSecondsForElementBased)
        {
            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(fromSeconds));

            wait.IgnoreExceptionTypes(
                typeof(NoSuchElementException),
                typeof(ElementNotVisibleException),
                typeof(ElementNotSelectableException),
                typeof(InvalidSelectorException),
                typeof(NoSuchFrameException),
                typeof(ElementNotInteractableException),
                typeof(WebDriverException));

            var actions = new Actions(driver);
            IWebElement elementToMove = null;

            wait.Until(d =>
            {
                try
                {
                    elementToMove = driver.FindElement(by);

                    actions
                    .Click(elementToMove)
                    .Build()
                    .Perform();
                    //elementToMove.Click();
                    //var isVisible = driver.IsVisibleInViewPort(elementToMove);

                    //if (!isVisible)
                    //{
                    //    driver.ExecuteJavaScript(
                    //        "arguments[0]" + ".scrollIntoView(false);", elementToMove);
                    //}

                    //actions.MoveToElement(elementToMove);
                    //actions.Click().Perform();

                    return elementToMove;
                }
                catch (StaleElementReferenceException)
                {
                    WaitForStaleElementError();

                    return null;
                }
            });

            return elementToMove;
        }

        public static IWebElement SearchHiddenElement(this IWebDriver driver,
            By by, int fromSeconds = DataRep.TimeToWaitFromSeconds)
        {
            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(fromSeconds));

            wait.IgnoreExceptionTypes(
                typeof(NoSuchElementException),
                typeof(ElementNotVisibleException),
                typeof(ElementNotSelectableException),
                typeof(InvalidSelectorException),
                typeof(NoSuchFrameException),
                typeof(ElementNotInteractableException),
                typeof(WebDriverException));

            IWebElement elementToFind;

            var element = wait.Until(d =>
             {
                 try
                 {
                     elementToFind = driver.FindElement(by);

                     driver.ExecuteJavaScript(
                         "arguments[0]" + ".scrollIntoView(false);", elementToFind);

                     return elementToFind;
                 }
                 catch (StaleElementReferenceException)
                 {
                     WaitForStaleElementError();

                     return null;
                 }
             });

            return element;
        }

        public static void ForceClickWithRetry(this IWebElement element,
            IWebDriver driver, By by, int fromSeconds =
            DataRep.TimeToWaitFromSecondsForElementBased)
        {
            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(fromSeconds));

            wait.IgnoreExceptionTypes(
                typeof(NoSuchElementException),
                typeof(ElementNotVisibleException),
                typeof(ElementNotSelectableException),
                typeof(InvalidSelectorException),
                typeof(NoSuchFrameException),
                typeof(ElementNotInteractableException),
                typeof(WebDriverException));

            wait.Until(d =>
            {
                try
                {
                    var numOfElements = driver.FindElements(by).Count();
                    if (numOfElements != 0)
                    {
                        element.Click();
                    }

                    numOfElements = driver.FindElements(by).Count();
                    if (numOfElements == 0)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                catch (StaleElementReferenceException)
                {
                    WaitForStaleElementError();

                    return true;
                }
            });
        }

        public static void ClickWithJavaScript(this IWebElement element,
            IWebDriver driver, int fromSeconds = DataRep.TimeToWaitFromSeconds)
        {
            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(fromSeconds));

            wait.IgnoreExceptionTypes(
                typeof(NoSuchElementException),
                typeof(ElementNotVisibleException),
                typeof(ElementNotInteractableException),
                typeof(ElementNotSelectableException),
                typeof(InvalidSelectorException),
                typeof(NoSuchFrameException),
                typeof(WebDriverException));

            wait.Until(d =>
            {
                try
                {
                    driver.ExecuteJavaScript("arguments[0].click();", element);

                    return true;
                }
                catch (StaleElementReferenceException)
                {
                    WaitForStaleElementError();

                    return true;
                }
            });
        }

        public static void RetryClickTillElementNotDisplayJavaScript(this
            IWebElement element, IWebDriver driver, By by,
            int fromSeconds = DataRep.TimeToWaitFromSecondsForElementBased)
        {
            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(fromSeconds));

            wait.IgnoreExceptionTypes(
                //typeof(NoSuchElementException),
                typeof(ElementNotVisibleException),
                typeof(ElementNotInteractableException),
                typeof(ElementNotSelectableException),
                typeof(InvalidSelectorException),
                typeof(NoSuchFrameException),
                typeof(WebDriverException));

            //if (!element.Displayed)
            //{
            //    driver.ExecuteJavaScript("arguments[0]" +
            //        ".scrollIntoView(false);", element);
            //}
            wait.Until(d =>
            {
                try
                {
                    var actualElement = driver.FindElement(by);
                    driver.ExecuteJavaScript("arguments[0].click();", actualElement);

                    return false;
                }
                catch (StaleElementReferenceException)
                {
                    WaitForStaleElementError();

                    return false;
                }
                catch (NoSuchElementException)
                {
                    return true;
                }
            });
        }

        public static void RetryClickTillElementNotDisplay(this IWebDriver
            driver, By by, int fromSeconds = DataRep.TimeToWaitFromSecondsForElementBased)
        {
            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(fromSeconds));
            IWebElement elenent;

            wait.IgnoreExceptionTypes(
                typeof(ElementNotVisibleException),
                typeof(ElementNotSelectableException),
                typeof(InvalidSelectorException),
                typeof(NoSuchFrameException),
                typeof(ElementClickInterceptedException),
                typeof(WebDriverException));

            wait.Until(d =>
            {
                try
                {
                    elenent = driver.FindElement(by);
                    elenent.Click();
                    Thread.Sleep(500);

                    return false;
                }
                catch (StaleElementReferenceException)
                {
                    WaitForStaleElementError();

                    return false;
                }
                catch (NoSuchElementException)
                {
                    return true;
                }
                catch (ElementNotInteractableException)
                {
                    return false;
                }
            });
        }

        public static void ClickAndWaitForNextElement(this IWebDriver driver,
            By firstElement, By nextElement, int fromSeconds =
            DataRep.TimeToWaitFromSeconds)
        {
            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(fromSeconds));

            wait.IgnoreExceptionTypes(
                typeof(ElementNotSelectableException),
                typeof(InvalidSelectorException),
                typeof(ElementNotVisibleException),
                typeof(NoSuchFrameException),
                typeof(WebDriverException));

            wait.Until(d =>
            {
                try
                {
                    driver.FindElement(firstElement).Click();
                    Thread.Sleep(500);
                    driver.FindElement(nextElement);

                    return true;
                }
                catch (StaleElementReferenceException)
                {
                    WaitForStaleElementError();

                    return false;
                }
                catch (NoSuchElementException)
                {
                    return false;
                }
                catch (ElementClickInterceptedException)
                {
                    return false;
                }
                catch (ElementNotInteractableException)
                {
                    return false;
                }
            });
        }

        public static void ClickAndWaitForElementNotExist(this IWebElement webElement,
            IWebDriver driver, int fromSeconds = DataRep.TimeToWaitFromSeconds)
        {
            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(fromSeconds));

            wait.IgnoreExceptionTypes(
                typeof(ElementNotVisibleException),
                typeof(ElementNotSelectableException),
                typeof(InvalidSelectorException),
                typeof(NoSuchFrameException),
                typeof(ElementClickInterceptedException),
                typeof(WebDriverException));

            try
            {
                wait.Until(d =>
                {
                    try
                    {
                        webElement.Click();
                        Thread.Sleep(100);

                        return false;
                    }
                    catch (StaleElementReferenceException)
                    {
                        return true;
                    }
                    catch (NoSuchElementException)
                    {
                        return true;
                    }
                    catch (ElementNotInteractableException)
                    {
                        return true;
                    }
                });
            }
            catch (Exception ex)
            {
                var exceMessage = $"Wait For Element Not Exist, Exception: {ex?.Message}";
                throw new Exception(exceMessage);
            }
        }

        public static void ClickUntilElementNotExist(this IWebElement element,
            IWebDriver driver, int fromSeconds = DataRep.TimeToWaitFromSeconds)
        {
            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(fromSeconds));

            wait.IgnoreExceptionTypes(
                typeof(ElementNotVisibleException),
                typeof(ElementNotInteractableException),
                typeof(ElementNotSelectableException),
                typeof(NoSuchElementException),
                typeof(InvalidSelectorException),
                typeof(NoSuchFrameException),
                typeof(WebDriverException));

            wait.Until(d =>
            {
                try
                {
                    element.Click();

                    return false;
                }
                catch (StaleElementReferenceException)
                {
                    WaitForStaleElementError();

                    return true;
                }
                catch (ElementClickInterceptedException)
                {
                    return false;
                }
            });
        }

        public static void ClickOnElementWithAnimation(this IWebDriver driver,
            By by, int fromSeconds = DataRep.TimeToWaitFromSeconds)
        {
            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(fromSeconds));
            wait.IgnoreExceptionTypes(typeof(NoSuchElementException),
                typeof(ElementNotVisibleException),
                typeof(ElementNotInteractableException));

            var builder = new Actions(driver);

            var text = wait.Until(d =>
            {
                try
                {
                    var element = driver.FindElement(by);
                    //driver.WaitForAnimationToLoad();

                    builder
                    .Click(element)
                    .Build()
                    .Perform();

                    return true;
                }
                catch (StaleElementReferenceException)
                {
                    WaitForStaleElementError();
                    return false;
                }
            });
        }

        public static void ConnectClientToCampaign(this IWebElement element,
            IWebDriver driver, By by, string input, int fromSeconds =
            DataRep.TimeToWaitFromSecondsForElementBased)
        {
            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(fromSeconds));
            var path = by?.ToString().Split(':')[1].TrimStart();
            var optionExp = By.XPath(string.Format("{0}/option[contains(.,'{1}')]", path, input));

            wait.IgnoreExceptionTypes(
                typeof(ElementClickInterceptedException),
                typeof(ElementNotVisibleException),
                typeof(ElementNotInteractableException),
                typeof(NoSuchElementException));

            wait.Until(d =>
            {
                try
                {
                    driver.FindElement(optionExp)
                    .Click();

                    var optionElement = driver.FindElement(optionExp);
                    driver.WaitForAnimationToLoad(10);

                    driver.ExecuteJavaScript(
                       "arguments[0]" + ".scrollIntoView(false);", element);

                    optionElement.Click();

                    return true;
                }
                catch (StaleElementReferenceException)
                {
                    WaitForStaleElementError();

                    return false;
                }
            });
        }

        public static void SelectElementFromDropDownByText(this IWebElement element,
            IWebDriver driver, By by, string input, int fromSeconds =
            DataRep.TimeToWaitFromSecondsForElementBased)
        {
            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(fromSeconds));

            wait.IgnoreExceptionTypes(
                typeof(ElementNotVisibleException),
                typeof(ElementNotSelectableException),
                typeof(InvalidSelectorException),
                typeof(NoSuchFrameException),
                typeof(WebDriverException));

            wait.Until(d =>
            {
                try
                {
                    if (driver == null)
                    {
                        return false;
                    }

                    element = driver.FindElement(by);

                    if (element != null)
                    {
                        var select = new SelectElement(element);
                        select?.SelectByText(input);
                    }

                    return true;
                }
                catch (StaleElementReferenceException)
                {
                    WaitForStaleElementError();

                    return false;
                }
                catch (NoSuchElementException)
                {
                    return false;
                }
                catch (ElementNotInteractableException)
                {
                    return false;
                }
            });
        }

        public static void SelectElementFromDropDownByValue(this IWebElement element,
            IWebDriver driver, string input, int fromSeconds =
            DataRep.TimeToWaitFromSecondsForElementBased)
        {
            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(fromSeconds));

            wait.IgnoreExceptionTypes(
                typeof(NoSuchElementException),
                typeof(ElementNotVisibleException),
                typeof(ElementNotInteractableException),
                typeof(ElementNotSelectableException),
                typeof(InvalidSelectorException),
                typeof(NoSuchFrameException),
                typeof(WebDriverException));

            wait.Until(d =>
            {
                try
                {
                    var select = new SelectElement(element);
                    select.SelectByValue(input);

                    return true;
                }
                catch (StaleElementReferenceException)
                {
                    WaitForStaleElementError();

                    return false;
                }
            });
        }

        public static void ClickOnElementWithSenKeys(this IWebDriver driver, By by,
            int fromSeconds = DataRep.TimeToWaitFromSeconds)
        {
            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(fromSeconds));

            wait.IgnoreExceptionTypes(
                typeof(NoSuchElementException),
                typeof(ElementNotVisibleException),
                typeof(ElementNotSelectableException),
                typeof(InvalidSelectorException),
                typeof(NoSuchFrameException),
                typeof(ElementNotInteractableException),
                typeof(InvalidElementStateException),
                typeof(WebDriverException));

            wait.Until(d =>
            {
                try
                {
                    var element = driver.FindElement(by);
                    element.SendKeys(Keys.Enter);

                    return true;
                }
                catch (StaleElementReferenceException)
                {
                    WaitForStaleElementError();

                    return false;
                }
            });
        }

        public static void SendsKeysAuto(this IWebElement element,
            IWebDriver driver, By by, string input,
            int fromSeconds = DataRep.TimeToWaitFromSeconds)
        {
            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(fromSeconds));

            wait.IgnoreExceptionTypes(
                typeof(NoSuchElementException),
                typeof(ElementNotVisibleException),
                typeof(ElementNotSelectableException),
                typeof(InvalidSelectorException),
                typeof(NoSuchFrameException),
                typeof(InvalidElementStateException),
                typeof(WebDriverException));

            wait.Until(d =>
            {
                try
                {
                    if (driver == null)
                    {
                        return false;
                    }

                    var element = driver.FindElement(by);

                    if (element == null)
                    {
                        return false;
                    }

                    var elementText = element.GetElementText(driver, by);

                    if (!input.Equals(elementText))
                    {
                        element.Clear();
                        Thread.Sleep(15);
                        elementText = element.GetElementText(driver, by);
                        element.SendKeys(input);
                        Thread.Sleep(15);
                        elementText = element.GetElementText(driver);

                        return false;
                    }
                    else
                    {
                        return true;
                    }
                }
                catch (StaleElementReferenceException)
                {
                    WaitForStaleElementError();
                    Console.WriteLine("SendsKeys Auto, Stale Element Reference Exception");

                    return false;
                }
                catch (ElementNotInteractableException)
                {
                    Console.WriteLine("Element Not Intractable Exception");

                    return false;
                }
            });

        }

        public static void SendsKeysCharByChar(this IWebElement element,
            IWebDriver driver, By by, string input,
            int fromSeconds = DataRep.TimeToWaitFromSeconds)
        {
            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(fromSeconds));

            wait.IgnoreExceptionTypes(
                typeof(NoSuchElementException),
                typeof(ElementNotVisibleException),
                typeof(ElementNotSelectableException),
                typeof(InvalidSelectorException),
                typeof(NoSuchFrameException),
                typeof(InvalidElementStateException),
                typeof(WebDriverException));

            try
            {
                wait.Until(d =>
                {
                    try
                    {
                        if (driver == null)
                        {
                            return false;
                        }

                        if (element == null)
                        {
                            return false;
                        }

                        var elementNew = driver?.FindElement(by);

                        if (elementNew != null)
                        {
                            foreach (var inputItem in input)
                            {
                                Thread.Sleep(1);
                                elementNew.SendKeys(inputItem.ToString());
                                Thread.Sleep(10);
                            }
                        }
                        else
                        {
                            return false;
                        }

                        //Thread.Sleep(1000);
                        var elementText = elementNew?.GetElementText(driver, by);

                        if (elementText != input)
                        {
                            element.Clear();
                            Thread.Sleep(30);

                            //if (elementNew.GetElementText(driver, by) != "")
                            //{
                            //    element.Clear();
                            //    Thread.Sleep(7);
                            //}

                            return false;
                        }

                        return true;
                    }
                    catch (StaleElementReferenceException)
                    {
                        WaitForStaleElementError();

                        return false;
                    }
                    catch (ElementNotInteractableException)
                    {

                        return false;
                    }
                });
            }
            catch (Exception ex)
            {
                if (input != null)
                {
                    var exceMessage = ($" Search Element method" +
                        $"  search parameters: {by}, input: {input} element is, Exception: {ex?.Message} ");

                    var newException = new Exception(exceMessage);

                    throw newException;
                }
            }
        }

        public static void SetTextUsingJavaScript(this IWebElement element,
            IWebDriver driver, string input, int fromSeconds =
            DataRep.TimeToWaitFromSecondsForElementBased)
        {
            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(fromSeconds));

            wait.IgnoreExceptionTypes(
                typeof(NoSuchElementException),
                typeof(ElementNotVisibleException),
                typeof(ElementNotSelectableException),
                typeof(InvalidSelectorException),
                typeof(NoSuchFrameException),
                typeof(ElementNotInteractableException),
                typeof(WebDriverException));

            wait.Until(d =>
            {
                try
                {
                    var displayed = element.Displayed;
                    element.Clear();
                    driver.ExecuteJavaScript("arguments[0].focus();", element);
                    driver.ExecuteJavaScript($"arguments[0].value='{input}';", element);
                    element.Click();
                    Thread.Sleep(100);
                    element.SendKeys(Keys.Enter);
                    var elmentInnerText = element.GetElementText(driver);

                    if (!elmentInnerText.Contains(input, StringComparison.OrdinalIgnoreCase))
                    {
                        return false;
                    }
                    else
                    {
                        return true;
                    }
                }
                catch (StaleElementReferenceException)
                {
                    WaitForStaleElementError();

                    return false;
                }
            });
        }

        public static void WaitForJQueryToLoad(this IWebDriver driver, int fromSeconds = 150)
        {
            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(fromSeconds));

            wait.Until(wd =>
                ((IJavaScriptExecutor)wd).ExecuteScript("return jQuery.active==0").Equals(true));
        }

        public static string WaitForElementTextToChange(this IWebElement element, IWebDriver driver,
            By by, string expectedText, int fromSeconds = DataRep.TimeToWaitFromSeconds)
        {
            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(fromSeconds));
            string actualElementText = null;

            wait.IgnoreExceptionTypes(
                typeof(NoSuchElementException),
                typeof(ElementNotVisibleException),
                typeof(ElementNotSelectableException),
                typeof(InvalidSelectorException),
                typeof(NoSuchFrameException),
                typeof(ElementNotInteractableException),
                typeof(WebDriverException));

            try
            {
                var elementText = wait.Until(d =>
                {
                    try
                    {
                        var element = driver.FindElement(by);
                        actualElementText = element.GetElementText(driver);

                        if (actualElementText != expectedText)
                        {
                            Console.WriteLine($"actualElementText: {actualElementText} + expectedText {expectedText}");
                            return null;
                        }

                        return actualElementText;
                    }
                    catch (StaleElementReferenceException)
                    {
                        WaitForStaleElementError();

                        return null;
                    }
                });

                return actualElementText;
            }
            catch (Exception ex)
            {
                var exceMessage = ($" expected Text: {expectedText}," +
                    $" actualElementText: {actualElementText}. exception: {ex.Message}");

                throw new Exception(exceMessage);
            }
        }

        public static IWebElement SwitchToIFrameWithClick(
            this IWebElement element, IWebDriver driver, By by,
            int fromSeconds = DataRep.TimeToWaitFromSeconds)
        {
            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(fromSeconds));

            wait.IgnoreExceptionTypes(
                typeof(NoSuchElementException),
                typeof(ElementNotVisibleException),
                typeof(ElementNotSelectableException),
                typeof(InvalidSelectorException),
                typeof(NoSuchFrameException),
                typeof(ElementNotInteractableException),
                typeof(WebDriverException));

            var frameElement = wait.Until(d =>
            {
                try
                {
                    var frame = driver.SwitchTo()
                    .Frame(element)
                    .FindElement(by);

                    frame.ForceClick(driver, by);
                    driver.SwitchTo().DefaultContent();

                    return element;
                }
                catch (StaleElementReferenceException)
                {
                    WaitForStaleElementError();

                    return null;
                }
            });

            return element;
        }

        public static void PrintOut(this IWebDriver driver, string stringToPrint, int timeToWait = 1000)
        {
            driver.ExecuteJavaScript("alert(arguments[0]);", stringToPrint);
            //Thread.Sleep(TimeSpan.FromSeconds(20));
        }

        public static bool CheckIfElementNotEnable(this IWebDriver driver, By by,
            int fromSeconds = DataRep.TimeToWaitFromSeconds, string log = null)
        {
            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(fromSeconds));

            wait.IgnoreExceptionTypes(
                typeof(ElementNotVisibleException),
                typeof(ElementNotSelectableException),
                typeof(InvalidSelectorException),
                typeof(NoSuchFrameException),
                typeof(ElementNotInteractableException),
                typeof(WebDriverException),
                typeof(NullReferenceException));

            IWebElement elementToFind = null;

            try
            {
                return wait.Until(d =>
                {
                    try
                    {
                        elementToFind = driver.FindElement(by);
                        //var isVisible = driver.IsVisibleInViewPort(by);

                        if (!elementToFind.Enabled)
                        {
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }
                    catch (StaleElementReferenceException)
                    {
                        WaitForStaleElementError();

                        return false;
                    }
                    catch (NoSuchElementException)
                    {
                        return false;
                    }
                });
            }
            catch (Exception ex)
            {
                var exceMessage = ($" search message: {log}," +
                    $"  search parameters: {by}, Exception: {ex?.Message} ");

                throw new Exception(exceMessage);
            }
        }

        public static void ScrollElementToView(this IWebDriver driver, By by)
        {
            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(60));
            wait.IgnoreExceptionTypes(
                typeof(ElementNotVisibleException),
                typeof(ElementNotSelectableException),
                typeof(InvalidSelectorException),
                typeof(NoSuchFrameException),
                typeof(ElementNotInteractableException),
                typeof(WebDriverException));

            wait.Until(d =>
            {
                try
                {
                    var element = driver.FindElement(by);

                    driver.ExecuteJavaScript(
                        "arguments[0]" + ".scrollIntoView(alignToTop = false);", element);

                    Thread.Sleep(500);

                    return true;

                }
                catch (StaleElementReferenceException)
                {
                    WaitForStaleElementError();

                    return false;
                }
            });
        }

        public static bool IsVisibleInViewPort(this IWebDriver driver, By by,
            int fromSeconds = DataRep.TimeToWaitFromSeconds)
        {
            var elemenNew = driver.FindElement(by);

            var ff = driver.ExecuteJavaScript<bool>(
            "var element = arguments[0]; " +
                "var rect = element.getBoundingClientRect();" +
                "console.log('rect.top' + rect.top);" +
                "console.log('rect.left' + rect.left); " +
                "return ( " +
                //"rect.top >= 0 && " +
                //"rect.left >= 0 && " +
                "rect.bottom <= (window.innerHeight || document.documentElement.clientHeight) && " +
                "rect.right <= (window.innerWidth || document.documentElement.clientWidth) " +
                ");"
            , elemenNew);

            //var dd = driver.ExecuteJavaScript<bool>(
            //    "var elem = arguments[0],                 " +
            //    "  box = elem.getBoundingClientRect(),    " +
            //    "  cx = box.left + box.width / 2,         " +
            //    "  cy = box.top + box.height / 2,   " +
            //    "  e = document.elementFromPoint(cx, cy);" +
            //    "console.log('cx' + cx);" +
            //    "console.log('cy' + cy);" +
            //    "console.log('e' + e);" +
            //    "if(e !== null)" +
            //    "  return true;" +
            //    "return false;", elemenNew);

            return ff;
        }

        public static void WaitForAnimationToLoad(this IWebDriver driver,
            int fromMilliseconds = 500)
        {
            Thread.Sleep(fromMilliseconds);
        }

        public static void WaitForAnimationToFinish(this IWebDriver driver,
            By animationElement, int fromSeconds = DataRep.TimeToWaitFromSeconds)
        {
            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(fromSeconds));

            wait.IgnoreExceptionTypes(
                typeof(ElementNotVisibleException),
                typeof(ElementNotSelectableException),
                typeof(InvalidSelectorException),
                typeof(NoSuchFrameException),
                typeof(ElementNotInteractableException),
                typeof(ElementClickInterceptedException),
                typeof(WebDriverException));

            //var builder = new Actions(driver);

            var text = wait.Until(d =>
            {
                try
                {
                    var element = driver.FindElement(animationElement);
                    var value = element.GetCssValue("animationDuration").Split('s').First();

                    // firefox issue Value syntax is different
                    if (value == "")
                    {
                        value = element.GetCssValue("transition-duration").Split('s').First();
                    }

                    var animationDuration = (Convert.ToDouble(value) * 1000) + 500;
                    var t = Convert.ToInt32(animationDuration);
                    Thread.Sleep(Convert.ToInt32(animationDuration));

                    return true;
                }
                catch (StaleElementReferenceException)
                {
                    WaitForStaleElementError();

                    return false;
                }
                catch (NoSuchElementException)
                {
                    return false;
                }
            });
        }

        public static void HoverOnElement(this IWebElement element, IWebDriver driver,
           int fromSeconds = DataRep.TimeToWaitFromSeconds)
        {
            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(fromSeconds));
            wait.IgnoreExceptionTypes(
                typeof(ElementNotVisibleException),
                typeof(ElementNotSelectableException),
                typeof(InvalidSelectorException),
                typeof(NoSuchFrameException),
                typeof(ElementNotInteractableException),
                typeof(WebDriverException));

            wait.Until(d =>
            {
                try
                {
                    var builder = new Actions(driver);
                    builder.MoveToElement(element);
                    builder.Perform();

                    return true;
                }
                catch (StaleElementReferenceException)
                {
                    WaitForStaleElementError();

                    return false;
                }
            });
        }
    }
}
