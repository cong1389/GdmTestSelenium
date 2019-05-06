using GenerateDocument.Common.Extensions;
using GenerateDocument.Common.Types;
using OpenQA.Selenium;
using OpenQA.Selenium.Remote;
using OpenQA.Selenium.Support.UI;
using System;
using System.Linq;

namespace GenerateDocument.Common.WebElements
{
    public class MultipleSelectListbox : RemoteWebElement
    {
        private readonly IWebElement _webElement;

        private readonly string _id;

        private readonly ElementLocator _listboxLocator = new ElementLocator(Locator.XPath, "//select[@id='{0}']");

        public MultipleSelectListbox(IWebElement webElement) : base(webElement.ToDriver() as RemoteWebDriver, null)
        {
            _webElement = webElement;
            _id = webElement.GetAttribute("id");
        }

        private IWebDriver Driver => _webElement.ToDriver();
        
        private IWebElement WaitUntilDropdownIsPopulated(IWebElement listboxEle, double timeout)
        {
            var selecteElement = new SelectElement(listboxEle);
            var isPopulated = false;
            try
            {
                new WebDriverWait(listboxEle.ToDriver(), TimeSpan.FromSeconds(timeout)).Until(x =>
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

            return listboxEle;
        }

        public void SelectedItems(string value)
        {
            var items = value.Split(',');

            if (!items.Any())
            {
                return;
            }

            var selectEle = Driver.GetElement(_listboxLocator.Format(_id));
            Driver.ScrollToView(selectEle);

            var selectElePopulated = WaitUntilDropdownIsPopulated(selectEle, BaseConfiguration.LongTimeout);
            var selectElement = new SelectElement(selectElePopulated);

            foreach (var item in items)
            {
                selectElement.SelectByValue(item);
            }
        }
    }
}
