using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace GenerateDocument.Test.PageObjects.FrontEnd
{
    public class UserCheckoutSubmit : PageObject
    {
        public UserCheckoutSubmit(IWebDriver browser) : base(browser)
        {
        }

        public void ClickToConfirmOrder()
        {
            var element = BrowserWait().Until(ExpectedConditions.ElementIsVisible(By.Id("StepArea_btnSubmit1")));
            element.Click();

            BrowserWait().Until(ExpectedConditions.StalenessOf(element));
        }
    }
}
