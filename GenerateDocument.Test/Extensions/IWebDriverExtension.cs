using System;
using System.Linq;
using GenerateDocument.Common.Extensions;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace GenerateDocument.Test.Extensions
{
    public static class IWebDriverExtension
    {
        //public static bool IsAbleToAccessPage(this IWebDriver browser, string url)
        //{
        //    browser.Navigate().NavigateTo(url);

        //    return browser.Url.IsContains(url);
        //}

        //public static bool CurrentPageIs(this IWebDriver browser, string page)
        //{
        //    return browser.Url.IsContains(page);
        //}

        public static void AcceptAlert(this IWebDriver browser)
        {
            browser.SwitchTo().Alert().Accept();
        }

        //public static void NavigateTo(this IWebDriver browser, string url)
        //{
        //    browser.Navigate().GoToUrl(url);
        //}

        //public static void SwitchToPopup(this IWebDriver browser)
        //{
        //    browser.SwitchTo().Window(browser.WindowHandles.Last());
        //}

       

       
    }
}
