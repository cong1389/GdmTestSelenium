using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using OpenQA.Selenium.Support.UI;
using System;

namespace GenerateDocument.Test.PageObjects
{
    public abstract class PageObject
    {
        public readonly IWebDriver Browser;

        public PageObject(IWebDriver browser)
        {
            Browser = browser;

            PageFactory.InitElements(Browser, this);
        }

        public WebDriverWait BrowserWait(int? timeoutInSecond = null) => new WebDriverWait(Browser, TimeSpan.FromSeconds(timeoutInSecond ?? ProjectBaseConfiguration.TimeoutInSecond));

        public void ScrollToView(IWebElement element)
        {
            var js = (IJavaScriptExecutor)Browser;
            js.ExecuteScript("arguments[0].scrollIntoView(false);", element);
        }

        public void ScrollToTop()
        {
            var js = (IJavaScriptExecutor)Browser;
            js.ExecuteScript("window.scrollTo(0, 0);");
        }

        public void ScrollToBottom()
        {
            ((IJavaScriptExecutor)Browser).ExecuteScript("window.scrollTo(0, document.body.scrollHeight - 150)");
        }

        public virtual Uri Page
        {
            get
            {
                return new Uri(Browser.Url.ToString());
            }
        }
    }
}
