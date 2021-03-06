﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GenerateDocument.Common.Types;
using OpenQA.Selenium;
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
                driver.GetElement(locator, e => e.Displayed);
                return true;
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

            wait.Until(x => x.GetElements(locator).Count == 0);
        }

        public static IWebElement WaitUntilPresentedElement(this IWebDriver driver, ElementLocator locator, double timeout)
        {
            try
            {
                var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(timeout));

                wait.Until(x => x.GetElements(locator).Count > 0);
            }
            catch (WebDriverTimeoutException)
            {
                return null;
            }

            return driver.GetElement(locator);
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

        public static bool IsAbleToAccessPage(this IWebDriver driver, string url)
        {
            driver.Navigate().GoToUrl(url);

            return driver.Url.IsContains(url);
        }

        public static void NavigateTo(this IWebDriver driver, string url)
        {
            driver.Navigate().GoToUrl(url);
        }
    }
}
