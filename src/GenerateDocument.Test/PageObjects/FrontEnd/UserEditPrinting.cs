﻿using System;
using System.Reflection;
using GenerateDocument.Common;
using GenerateDocument.Common.Extensions;
using GenerateDocument.Common.Helpers;
using GenerateDocument.Common.Types;
using GenerateDocument.Common.WebElements;
using GenerateDocument.Domain.Designs;
using GenerateDocument.Domain.TestSenario;
using GenerateDocument.Test.Base;
using log4net;

namespace GenerateDocument.Test.PageObjects.FrontEnd
{
    public class UserEditPrinting : PageBaseObject, IAutoSave
    {
        private static readonly ILog Logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        private readonly ElementLocator
            _nextStepButtonLocator = new ElementLocator(Locator.XPath, "//a[contains(@id, '_StepNextN1_TheLabelButton')]"),
            _inputLocator = new ElementLocator(Locator.XPath, "//*[@id='{0}']");

        public UserEditPrinting(DriverContext driverContext) : base(driverContext)
        {
        }

        public UserEditPrinting ClickToNextStep()
        {
            if (!DriverContext.Driver.IsUrlEndsWith(PageTypes.UserEditPrinting.ToString()))
            {
                return this;
            }

            Logger.Info($"Perform to next button with id: {_nextStepButtonLocator.Value}");

            Driver.GetElement<Button>(_nextStepButtonLocator).ClickTo();
            
            Driver.WaitUntilPresentedUrl(PageTypes.UserEditFinish.ToString());

            return this;
        }

        private void EnteringValueInputText(string controlId, string text)
        {
            Logger.Info($"Perform to controlId: {controlId}, value is {text}");

            Driver.GetElement<Textbox>(_inputLocator.Format(controlId)).SetValue(text);
        }

        public void PerformToControlType(Step step, DesignModel designModel)
        {
            string text;

            string controlType = step.ControlType;
            Enum.TryParse(controlType, true, out ControlTypes controlTypeValue);

            switch (controlTypeValue)
            {
                case ControlTypes.Textbox:
                    text = string.IsNullOrEmpty(step.ControlValue) ? NameHelper.RandomName(10) : step.ControlValue;
                    EnteringValueInputText(step.ControlId, text);
                    break;

                case ControlTypes.TextArea:
                    text = string.IsNullOrEmpty(step.ControlValue) ? NameHelper.RandomName(100) : step.ControlValue;
                    EnteringValueInputText(step.ControlId, text);
                    break;

                case ControlTypes.Button:
                    ClickToNextStep();
                    break;


            }
        }

    }
}