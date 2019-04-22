using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using GenerateDocument.Common.Types;
using OpenQA.Selenium;
using OpenQA.Selenium.Html5;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Internal;
using OpenQA.Selenium.Support.UI;

namespace GenerateDocument.Common.Extensions
{
    public static class WebDriverExtensions
    {
        public static bool IsElementPresent(this IWebDriver driver, ElementLocator locator)
        {
            try
            {
                var isDisplayed = driver.GetElement(locator, e => e.Displayed);
                return isDisplayed != null;
            }
            catch (NoSuchElementException)
            {
                return false;
            }
            catch (WebDriverTimeoutException)
            {
                return false;
            }
        }

        public static void WaitUntilElementIsNoLongerFound(this IWebDriver driver, ElementLocator locator, double timeout)
        {
            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(timeout));
            wait.IgnoreExceptionTypes(typeof(StaleElementReferenceException), typeof(NoSuchElementException));
            wait.Until(x => x.GetElements(locator)?.Count == 0);
        }

        public static IWebElement WaitUntilPresentedElement(this IWebDriver driver, ElementLocator locator, double timeout)
        {
            return WaitUntilPresentedElement(driver, locator, e => e.Displayed && e.Enabled, timeout);
        }

        public static IWebElement WaitUntilPresentedElement(this IWebDriver driver, ElementLocator locator)
        {
            return WaitUntilPresentedElement(driver, locator, e => e.Displayed && e.Enabled, BaseConfiguration.LongTimeout);
        }

        public static IWebElement WaitUntilPresentedElement(this IWebDriver driver, ElementLocator locator, Func<IWebElement, bool> condition, double timeout)
        {
            try
            {
                var isDisplayed = new WebDriverWait(driver, TimeSpan.FromSeconds(timeout)).Until(x =>
                  {
                      return x.GetElements(locator, condition)?.Count > 0;
                  });

                return isDisplayed ? driver.GetElement(locator) : null;
            }
            catch (NoSuchElementException)
            {
                return null;
            }
            catch (WebDriverTimeoutException)
            {
                return null;
            }
        }

        public static bool WaitUntilPresentedUrl(this IWebDriver driver, string url)
        {
            return WaitUntilPresentedUrl(driver, url, BaseConfiguration.LongTimeout);
        }

        public static bool WaitUntilPresentedUrl(this IWebDriver driver, string url, double timeout)
        {
            try
            {
                var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(timeout));

                new WebDriverWait(driver, TimeSpan.FromSeconds(BaseConfiguration.LongTimeout)).Until(x =>
                {
                    return x.Url.IsContains(url);
                });
            }
            catch (WebDriverTimeoutException ex)
            {
                throw new Exception($"The loading page {url} has issue {ex.Message} ");
            }

            return false;
        }

        public static Actions Actions(this IWebDriver driver)
        {
            return new Actions(driver);
        }

        public static void ScrollToView(this IWebDriver driver, IWebElement element)
        {
            var js = driver as IJavaScriptExecutor;
            js.ExecuteScript("arguments[0].scrollIntoView(false);", element);
        }

        public static void ScrollToTop(this IWebDriver driver)
        {
            var js = driver as IJavaScriptExecutor;
            js.ExecuteScript("window.scrollTo(0, 0);");
        }

        public static void ScrollToBottom(this IWebDriver driver)
        {
            var js = driver as IJavaScriptExecutor;
            js.ExecuteScript("window.scrollTo(0, document.body.scrollHeight - 150)");
        }

        public static bool IsUrlEndsWith(this IWebDriver driver, string page)
        {
            return driver.Url.IsContains(page);
        }

        public static void DeleteAllCookies(this IWebDriver driver)
        {
            driver.Manage().Cookies.DeleteAllCookies();
        }

        public static bool CheckExistedCookie(this IWebDriver driver, string cookieName)
        {
            var cookie = driver.Manage().Cookies.GetCookieNamed(cookieName);
            return cookie != null;
        }

        public static void GetCookie(this IWebDriver driver, string cookieName)
        {
            driver.Manage().Cookies.GetCookieNamed(cookieName);
        }

        public static void AddCookie(this IWebDriver driver, string cookieName, string cookieValue)
        {
            var cookie = new Cookie(cookieName, cookieValue);
            driver.Manage().Cookies.AddCookie(cookie);
        }

        public static IJavaScriptExecutor JavaScripts(this IWebDriver driver)
        {
            return (IJavaScriptExecutor)driver;
        }

        public static string GetSessionStorage(this IWebDriver driver, string itemName)
        {
            var jsExecutor = (IJavaScriptExecutor)driver;
            var name = $"return window.sessionStorage.getItem('{itemName}');";
            return jsExecutor.ExecuteScript(name)?.ToString();
        }

        public static void SetSessionStorage(this IWebDriver driver, string itemName, string itemValue)
        {
            ((IJavaScriptExecutor)driver).ExecuteScript($"return window.sessionStorage.setItem('{itemName}', {itemValue});");
        }

        public static void ClearAllSessionStorage(this IWebDriver driver)
        {
            var jsExecutor = (IJavaScriptExecutor)driver;
            jsExecutor.ExecuteScript($"return window.sessionStorage.clear();");
        }

        public static bool IsAbleToAccessPage(this IWebDriver driver, string url)
        {
            driver.Navigate().GoToUrl(url);

            return driver.Url.IsContains(url);
        }

        public static void NavigateTo(this IWebDriver driver, string url)
        {
            driver.Navigate().GoToUrl(url);
        }

        public static void RefreshPage(this IWebDriver driver)
        {
            driver.Navigate().Refresh();
        }

        public static void SwitchToParent(this IWebDriver driver)
        {
            driver.SwitchTo().Window(driver.WindowHandles.First());
        }
    }
}
