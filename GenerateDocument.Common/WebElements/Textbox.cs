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
    public class Textbox : RemoteWebElement
    {
        private readonly IWebElement _webElement;

        public Textbox(IWebElement webElement) : base(webElement.ToDriver() as RemoteWebDriver, null)
        {
            _webElement = webElement;
        }

        private IWebDriver Driver
        {
            get { return _webElement.ToDriver(); }
        }

        public void SetValue(string text)
        {
            Driver.ScrollToView(_webElement);

            _webElement.Clear();
            _webElement.SendKeys(text);
            _webElement.SendKeys(Keys.Tab);
        }

        public string GetValue()
        {
            Driver.ScrollToView(_webElement);
            return _webElement.GetAttribute("value");
        }
    }
}
