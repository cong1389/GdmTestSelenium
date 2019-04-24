using GenerateDocument.Common;
using GenerateDocument.Common.Extensions;
using GenerateDocument.Common.Types;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace GenerateDocument.Test.PageObjects.FrontEnd
{
    public class UserEditFinish : PageBaseObject
    {
        private readonly ElementLocator
            _finishDesignButton = new ElementLocator(Locator.XPath, "//a[contains(@id,'UserAddToCartButtons2_GoToDesignsButton')]"),
            _nextStepButtonLocator = new ElementLocator(Locator.XPath, "//a[contains(@id, '_StepNextN1_TheLabelButton')]");

        public UserEditFinish(DriverContext driverContext) : base(driverContext)
        {
        }

        private IWebElement OrderNameTextBox => DriverContext.BrowserWait().Until(ExpectedConditions.ElementIsVisible(By.Id("ctl00_ctl00_ContentPlaceHolderBody_StepArea1_ContentPlaceHolderStepArea_DocDescription_box")));

        public UserEditFinish ClickToNextStep()
        {
            var ele = Driver.WaitUntilPresentedElement(_nextStepButtonLocator, BaseConfiguration.MiddleTimeout);
            if (ele == null)
            {
                return this;
            }

            ele.Click();
            Driver.WaitUntilElementIsNoLongerFound(_nextStepButtonLocator, BaseConfiguration.LongTimeout);

            return this;
        }

        public UserEditFinish ClickToFinishDesign()
        {
            var buttonEle = Driver.GetElement(_finishDesignButton);
            Driver.ScrollToView(buttonEle);
            buttonEle?.Click();

            Driver.WaitUntilPresentedUrl("onedesign", BaseConfiguration.LongTimeout);

            return this;
        }

        public UserEditFinish EnterOrderName(string name)
        {
            OrderNameTextBox.Clear();
            OrderNameTextBox.SendKeys(name);
            OrderNameTextBox.SendKeys(Keys.Tab);

            return this;
        }

        public string GetErrorMessage()
        {
            return DriverContext.BrowserWait().Until(ExpectedConditions.ElementIsVisible(By.Id("descriptionErrorMessage"))).Text;
        }
    }
}
