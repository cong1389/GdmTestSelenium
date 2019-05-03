using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GenerateDocument.Common.Extensions;
using OpenQA.Selenium;
using OpenQA.Selenium.Remote;

namespace GenerateDocument.Common.WebElements
{
    public class Button : RemoteWebElement
    {
        private readonly IWebElement _webElement;

        public Button(IWebElement webElement) : base(webElement.ToDriver() as RemoteWebDriver, null)
        {
            _webElement = webElement;
        }

        private IWebDriver Driver
        {
            get { return _webElement.ToDriver(); }
        }

        public void ClickTo()
        {
            Driver.ScrollToView(_webElement);
            _webElement?.Click();
        }
    }
}
