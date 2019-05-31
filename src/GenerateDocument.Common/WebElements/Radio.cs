using GenerateDocument.Common.Extensions;
using OpenQA.Selenium;
using OpenQA.Selenium.Remote;
using OpenQA.Selenium.Support.UI;
using System;

namespace GenerateDocument.Common.WebElements
{
    public class Radio : RemoteWebElement
    {
        private readonly IWebElement _webElement;

        public Radio(IWebElement webElement) : base(webElement.ToDriver() as RemoteWebDriver, null)
        {
            _webElement = webElement;
        }

        public void TickRadio()
        {
            if (!_webElement.Selected)
            {
                _webElement.Click();
            }
        }
    }
}
