using System.Collections.Generic;
using System.Linq;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace GenerateDocument.Test.PageObjects.FrontEnd
{
    public class UserCheckoutPayment : PageObject
    {
        public UserCheckoutPayment(IWebDriver browser) : base(browser)
        {
        }

        public List<IWebElement> GetAllTextboxs()
        {
            return BrowserWait(20).Until<List<IWebElement>>((d) =>
            {
                var elements = d.FindElements(By.XPath("//div[contains(@id, 'StepArea_InputFields_InputFields')]//input[contains(@id, 'FIELD')]")).Where(x => x.Displayed).ToList();

                return elements ?? new List<IWebElement>();
            });
        }

        public void ClickToNextStep()
        {
            var element = BrowserWait().Until(ExpectedConditions.ElementIsVisible(By.Id("StepArea_StepNextN1_TheLabelButton")));
            ScrollToView(element);

            element.Click();
        }
    }
}
