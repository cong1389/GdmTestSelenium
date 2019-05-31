using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using OpenQA.Selenium.Support.UI;
using System;
using GenerateDocument.Common;

namespace GenerateDocument.Test.PageObjects
{
    public abstract class PageObject
    {
        public readonly IWebDriver Browser;

        protected PageObject(IWebDriver browser)
        {
            Browser = browser;

            PageFactory.InitElements(Browser, this);
        }

        public WebDriverWait BrowserWait(int? timeoutInSecond = null) => new WebDriverWait(Browser,
            TimeSpan.FromSeconds(timeoutInSecond ?? BaseConfiguration.LongTimeout));

    }
}
