using System;
using System.Linq;
using GenerateDocument.Common.Extensions;
using GenerateDocument.Test.Extensions;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using OpenQA.Selenium.Support.UI;

namespace GenerateDocument.Test.PageObjects.FrontEnd
{
    public class UserEditPrinting : PageObject
    {
        public UserEditPrinting(IWebDriver browser) : base(browser)
        {
        }

        [FindsBy(How = How.Id, Using = "LinkButtonContinueAnyway")]
        private IWebElement ContinueAnywayWithoutRequiredFieldButton { get; set; }

        public void ClickToNextStep()
        {
            if (!Browser.IsUrlEndsWith("usereditprinting"))
                return;

            var webDriver = BrowserWait();
            var element = webDriver.Until(ExpectedConditions.ElementToBeClickable(By.Id("ctl00_ctl00_ContentPlaceHolderBody_StepArea1_StepNextN1_TheLabelButton")));
            ScrollToView(element);
            element.Click();
            webDriver.Until(ExpectedConditions.StalenessOf(element));
        }

        public bool NeedToCompleteRequiredFields()
        {
            try
            {
                return BrowserWait(5).Until(ExpectedConditions.ElementIsVisible(By.ClassName("warningAreaMessageWarning"))) != null;
            }
            catch
            {
                return false;
            }
        }

        public void ClickToContinueAnywayWithoutRequiredFieldButton()
        {
            ContinueAnywayWithoutRequiredFieldButton.Click();
        }

        public void ClickToBypassCompleteRequiredFields()
        {
            if (NeedToCompleteRequiredFields())
            {
                ClickToContinueAnywayWithoutRequiredFieldButton();
            }
        }
    }
}
