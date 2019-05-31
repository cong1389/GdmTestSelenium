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
    public class MultipleSelectCheckbox : RemoteWebElement
    {
        private readonly IWebElement _webElement;

        private readonly string _id;

        private readonly ElementLocator _checkboxMultipleLocator = new ElementLocator(Locator.XPath, "//input[@type='checkbox' and @id='{0}']");

        public MultipleSelectCheckbox(IWebElement webElement) : base(webElement.ToDriver() as RemoteWebDriver, null)
        {
            _webElement = webElement;
            _id = webElement.GetAttribute("id");
        }

        private IWebDriver Driver => _webElement.ToDriver();

        public void MultipleTick(string value)
        {
            TickOrUnTickMulitpleCheckbox(value, true);
        }

        public void MultipleUnTick(string value)
        {
            TickOrUnTickMulitpleCheckbox(value, false);
        }

        private void TickOrUnTickMulitpleCheckbox(string value, bool neddToChecked)
        {
            var values = value.Split(',');

            if (!values.Any())
            {
                return;
            }

            var eles = Driver.GetElements(_checkboxMultipleLocator.Format(_id));
            foreach (var ele in eles)
            {
                if (ele.Selected && !neddToChecked || !ele.Selected && neddToChecked)
                {
                    Driver.ScrollToView(ele);

                    var matchValue = values.Any(x => ele.GetAttribute("value").Equals(x));
                    if (matchValue)
                    {
                        ele.Click();
                    }
                }
            }
        }
    }
}
