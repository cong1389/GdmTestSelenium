using GenerateDocument.Common;
using GenerateDocument.Common.Extensions;
using GenerateDocument.Common.Helpers;
using GenerateDocument.Common.Types;
using GenerateDocument.Common.WebElements;
using GenerateDocument.Domain.TestSenario;
using GenerateDocument.Test.Base;
using log4net;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;
using System.Reflection;
using GenerateDocument.Domain.Designs;

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
            if (!DriverContext.Driver.IsUrlEndsWith(PageTypes.UserEditFinish.ToString()))
            {
                return this;
            }

            Logger.Info($"Perform to controlId: {_finishDesignButton.Value}");

            Driver.GetElement<Button>(_finishDesignButton).ClickTo();

            Driver.WaitUntilPresentedUrl(PageTypes.OneDesign.ToString());

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

        public void PerformToControlType(Step step, DesignModel designModel)
        {
            Enum.TryParse(step.ControlType, true, out ControlTypes controlTypeValue);

            switch (controlTypeValue)
            {
                case ControlTypes.Textbox:
                    var text = string.IsNullOrEmpty(step.ControlValue) ? NameHelper.RandomName(10) : $"{step.ControlValue}";
                    EnterOrderName(text);
                    break;

                case ControlTypes.Button:
                    ClickToFinishDesign();
                    break;


            }
        }

    }
}
