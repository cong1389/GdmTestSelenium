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
    public class Listbox : RemoteWebElement
    {
        private readonly IWebElement _webElement;

        public Listbox(IWebElement webElement) : base(webElement.ToDriver() as RemoteWebDriver, null)
        {
            _webElement = webElement;
        }
    }
}
