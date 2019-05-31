using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GenerateDocument.Common.Extensions;
using GenerateDocument.Common.Types;
using OpenQA.Selenium;
using OpenQA.Selenium.Remote;
using OpenQA.Selenium.Support.UI;

namespace GenerateDocument.Common.WebElements
{
    public class MultipleSelectInclude : RemoteWebElement
    {
        private readonly IWebElement _webElement;

        private readonly string _id;

        private readonly ElementLocator
            _includeListboxLocator = new ElementLocator(Locator.XPath, "//select[@name='{0}_INCLUDE']"),
            _excludeListboxLocator = new ElementLocator(Locator.XPath, "//select[@name='{0}_EXCLUDE']"),
            _addButtonLocator = new ElementLocator(Locator.XPath, "//select[@name='{0}_EXCLUDE']//following-sibling::div//a[text()='< Add']"),
            _removeButtonLocator = new ElementLocator(Locator.XPath, "//select[@name='{0}_INCLUDE']//following-sibling::div//a[text()='Remove >']");

        public MultipleSelectInclude(IWebElement webElement) : base(webElement.ToDriver() as RemoteWebDriver, null)
        {
            _webElement = webElement;
            _id = webElement.GetAttribute("id");
        }

        private IWebDriver Driver => _webElement.ToDriver();

        private void SelectedByValue(IWebElement listboxEle, string item)
        {
            var ele = WaitUntilDropdownIsPopulated(listboxEle, BaseConfiguration.LongTimeout);
            var selectElement = new SelectElement(ele);
            try
            {
                selectElement.SelectByValue(item);
            }
            catch (NoSuchElementException)
            {

            }
        }

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

        public void IncludeItems(string value)
        {
            var items = value.Split(',');

            if (!items.Any())
            {
                return;
            }

            var selectEle = Driver.GetElement(_excludeListboxLocator.Format(_id));
            Driver.ScrollToView(selectEle);

            foreach (var item in items)
            {
                SelectedByValue(selectEle, item);
                Driver.GetElement(_addButtonLocator.Format(_id)).Click();
            }
        }

        public void ExcludeItems(string value)
        {
            var items = value.Split(',');

            if (!items.Any())
            {
                return;
            }

            var selectEle = Driver.GetElement(_includeListboxLocator.Format(_id));
            Driver.ScrollToView(selectEle);

            foreach (var item in items)
            {
                SelectedByValue(selectEle, item);
                Driver.GetElement(_removeButtonLocator.Format(_id)).Click();
            }
        }

    }
}
