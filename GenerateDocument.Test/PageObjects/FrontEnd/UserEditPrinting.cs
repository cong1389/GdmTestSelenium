using GenerateDocument.Common;
using GenerateDocument.Common.Extensions;
using GenerateDocument.Common.Types;

namespace GenerateDocument.Test.PageObjects.FrontEnd
{
    public class UserEditPrinting : PageBaseObject
    {
        private readonly ElementLocator 
            _nextStepButtonLocator = new ElementLocator(Locator.XPath, "//a[contains(@id, '_StepNextN1_TheLabelButton')]");

        public UserEditPrinting(DriverContext driverContext) : base(driverContext)
        {
        }
        
        public UserEditPrinting ClickToNextStep()
        {
            if (!Driver.IsElementPresent(_nextStepButtonLocator))
            {
                return this;
            }
            var nextButtonEle = Driver.GetElement(_nextStepButtonLocator);
            Driver.ScrollToView(nextButtonEle);
            nextButtonEle?.Click();

            return this;
        }
    }
}
