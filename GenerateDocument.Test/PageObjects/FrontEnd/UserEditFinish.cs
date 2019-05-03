using System.Reflection;
using GenerateDocument.Common;
using GenerateDocument.Common.Extensions;
using GenerateDocument.Common.Helpers;
using GenerateDocument.Common.Types;
using GenerateDocument.Common.WebElements;
using GenerateDocument.Domain.TestSenario;
using log4net;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace GenerateDocument.Test.PageObjects.FrontEnd
{
    public class UserEditFinish : PageBaseObject, IAutoSave
    {
        private static readonly ILog Logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        private readonly ElementLocator
            _inputDescriptionLocator = new ElementLocator(Locator.XPath, "//input[contains(@id,'DocDescription_box')]"),
            _finishDesignButton = new ElementLocator(Locator.XPath, "//a[contains(@id,'UserAddToCartButtons2_GoToDesignsButton')]");

        public UserEditFinish(DriverContext driverContext) : base(driverContext)
        {

        }

        public UserEditFinish ClickToFinishDesign()
        {
            Logger.Info($"Perform to controlId: {_finishDesignButton.Value}");

            Driver.GetElement<Button>(_finishDesignButton).ClickTo();

            Driver.WaitUntilPresentedUrl("onedesign");

            return this;
        }

        public UserEditFinish EnterOrderName(string text)
        {
            Logger.Info($"Perform to controlId: {_inputDescriptionLocator.Value}, value is {text}");

            Driver.GetElement<Textbox>(_inputDescriptionLocator).SetValue(text);

            return this;
        }

        public string GetErrorMessage()
        {
            return DriverContext.BrowserWait().Until(ExpectedConditions.ElementIsVisible(By.Id("descriptionErrorMessage"))).Text;
        }

        public void PerformToControlType(Step step)
        {
            switch (step.ControlType)
            {
                case "textbox":
                    var text = string.IsNullOrEmpty(step.ControlValue) ? NameHelper.RandomName(10) : step.ControlValue;
                    EnterOrderName(text);
                    break;

                case "button":
                    ClickToFinishDesign();
                    break;


            }
        }
    }
}
