using System;
using GenerateDocument.Common;
using GenerateDocument.Common.Extensions;
using GenerateDocument.Common.Types;
using GenerateDocument.Common.WebElements;
using GenerateDocument.Domain.Designs;
using GenerateDocument.Domain.TestSenario;
using GenerateDocument.Test.Base;

namespace GenerateDocument.Test.PageObjects.BackEnd
{
    public class AdminOptionField : PageBaseObject,IAutoSave
    {
        private  readonly ElementLocator 
            _textboxLocator = new ElementLocator(Locator.XPath, "//input[contains(@name,'{0}') or contains(@id,'{0}')]"),
            _dropboxLocator = new ElementLocator(Locator.XPath, "//select[contains(@name,'{0}') or contains(@id,'{0}')]"),
            _buttonLocator = new ElementLocator(Locator.XPath, "//*[@type='submit' and contains(@name,'{0}') or contains(@id,'{0}')]");

        public AdminOptionField(DriverContext driverContext) : base(driverContext)
        {
        }

        public void PerformToControlType(Step step, DesignModel designModel)
        {
            string controlType = step.ControlType;
            Enum.TryParse(controlType, true, out ControlTypes controlTypeValue);

            switch (controlTypeValue)
            {
                case ControlTypes.Button:
                    Driver.GetElement<Button>(_buttonLocator.Format(step.ControlId)).ClickTo();
                    break;

                case ControlTypes.Textbox:
                    Driver.GetElement<Textbox>(_textboxLocator.Format(step.ControlId)).SetValue(step.ControlValue);
                    break;

                case ControlTypes.Dropbox:
                    Driver.GetElement<Select>(_dropboxLocator.Format(step.ControlId)).SelectedByValue(step.ControlValue);
                    break;
            }
        }
    }
}
