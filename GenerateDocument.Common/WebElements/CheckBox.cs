using GenerateDocument.Common.Extensions;
using OpenQA.Selenium;
using OpenQA.Selenium.Remote;

namespace GenerateDocument.Common.WebElements
{
    public class CheckBox : RemoteWebElement
    {
        private readonly IWebElement _webElement;

        public CheckBox(IWebElement webElement) : base(webElement.ToDriver() as RemoteWebDriver, null)
        {
            _webElement = webElement;
        }

        public void TickCheckBox()
        {
            if (!_webElement.Selected)
            {
                _webElement.Click();
            }
        }

        public void UnTickCheckBox()
        {
            if (_webElement.Selected)
            {
                _webElement.Click();
            }
        }
    }
}
