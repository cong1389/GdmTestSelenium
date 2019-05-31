using GenerateDocument.Common.Extensions;
using OpenQA.Selenium;
using OpenQA.Selenium.Remote;
using OpenQA.Selenium.Support.UI;
using System;

namespace GenerateDocument.Common.WebElements
{
    public class Select : RemoteWebElement
    {
        private readonly IWebElement _webElement;

        public Select(IWebElement webElement) : base(webElement.ToDriver() as RemoteWebDriver, null)
        {
            _webElement = webElement;
        }

        public void SelectedByValue(string value)
        {
            SelectedByValue(value, BaseConfiguration.LongTimeout);
        }

        private void SelectedByValue(string value, double timeout)
        {
            var ele = WaitUntilDropdownIsPopulated(timeout);
            SelectElement selectElement = new SelectElement(ele);
            try
            {
                selectElement.SelectByValue(value);
            }
            catch (NoSuchElementException)
            {

            }
        }

        private IWebElement WaitUntilDropdownIsPopulated(double timeout)
        {
            var selecteElement = new SelectElement(_webElement);
            var isPopulated = false;
            try
            {
                new WebDriverWait(_webElement.ToDriver(), TimeSpan.FromSeconds(timeout)).Until(x =>
                {
                    var size = selecteElement.Options.Count;
                    if (size > 0)
                    {
                        isPopulated = true;
                    }

                    return isPopulated;
                });
            }
            catch (NoSuchElementException)
            {
                throw new Exception("Option is null");
            }

            return _webElement;
        }
    }
}
