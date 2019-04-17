using GenerateDocument.Common.Extensions;
using OpenQA.Selenium;
using OpenQA.Selenium.Remote;
using OpenQA.Selenium.Support.UI;
using System;

namespace GenerateDocument.Common.WebElements
{
    public class Select : RemoteWebElement
    {
        private readonly IWebElement webElement;

        public Select(IWebElement webElement) : base(webElement.ToDriver() as RemoteWebDriver, null)
        {
            this.webElement = webElement;
        }

        public SelectElement Selected()
        {
            return new SelectElement(this.webElement);
        }

        public void SelectedByValue(string value)
        {
            SelectedByValue(value, BaseConfiguration.LongTimeout);
        }

        public void SelectedByValue(string value, double timeout)
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

        public IWebElement WaitUntilDropdownIsPopulated(double timeout)
        {
            var selecteElement = new SelectElement(webElement);
            var isPopulated = false;
            try
            {
                new WebDriverWait(webElement.ToDriver(), TimeSpan.FromSeconds(timeout)).Until(x =>
                {
                    var size = selecteElement.Options.Count;
                    if (size > 1)
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

            return webElement;
        }
    }
}
