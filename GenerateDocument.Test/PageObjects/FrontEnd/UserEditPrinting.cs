using System;
using System.Linq;
using GenerateDocument.Common;
using GenerateDocument.Common.Extensions;
using GenerateDocument.Common.Types;
using GenerateDocument.Test.Extensions;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using OpenQA.Selenium.Support.UI;

namespace GenerateDocument.Test.PageObjects.FrontEnd
{
    public class UserEditPrinting : PageBaseObject
    {
        private readonly ElementLocator completeRequiredFields = new ElementLocator(Locator.XPath, "//div[@class='warningAreaMessageWarning']");

        public UserEditPrinting(DriverContext driverContext) : base(driverContext)
        {
        }

        [FindsBy(How = How.Id, Using = "LinkButtonContinueAnyway")]
        private IWebElement ContinueAnywayWithoutRequiredFieldButton { get; set; }

        public void ClickToNextStep()
        {
            if (!Driver.IsUrlEndsWith("usereditprinting"))
                return;

            var webDriver = DriverContext.BrowserWait();
            var element = webDriver.Until(ExpectedConditions.ElementToBeClickable(By.Id("ctl00_ctl00_ContentPlaceHolderBody_StepArea1_StepNextN1_TheLabelButton")));
            Driver.ScrollToView(element);
            element.Click();
            webDriver.Until(ExpectedConditions.StalenessOf(element));
        }

        public bool NeedToCompleteRequiredFields()
        {
            try
            {
                return DriverContext.BrowserWait(5).Until(ExpectedConditions.ElementIsVisible(By.ClassName("warningAreaMessageWarning"))) != null;
            }
            catch
            {
                return false;
            }
        }

       

        public void ClickToBypassCompleteRequiredFields()
        {
            var requiredFieldsEle = Driver.WaitUntilPresentedElement(completeRequiredFields, BaseConfiguration.LongTimeout);
            requiredFieldsEle?.Click();
        }
    }
}
