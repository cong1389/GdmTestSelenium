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
        private readonly ElementLocator 
            _nextStepButtonLocator = new ElementLocator(Locator.XPath, "//a[contains(@id, '_StepNextN1_TheLabelButton')]");

        public UserEditPrinting(DriverContext driverContext) : base(driverContext)
        {
        }

        [FindsBy(How = How.Id, Using = "LinkButtonContinueAnyway")]
        private IWebElement ContinueAnywayWithoutRequiredFieldButton { get; set; }
        
        public UserEditPrinting ClickToNextStep()
        {
            var nextButtonEle = Driver.GetElement(_nextStepButtonLocator);
            Driver.ScrollToView(nextButtonEle);
            nextButtonEle?.Click();

            return this;
        }
    }
}
