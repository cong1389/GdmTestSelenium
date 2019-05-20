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
    public class AdminOptionSet : PageBaseObject, IAutoSave
    {
        private readonly ElementLocator
            _footerLinkLocator = new ElementLocator(Locator.Id,
                "AdminMaster_ContentPlaceHolderBody__XF_DND_OptionSet1_grid_ctl31_hlEditField"),
            _goBackLocator = new ElementLocator(Locator.Id, "AdminMaster_ContentPlaceHolderBody_GoBack"),

            _buttonLocator = new ElementLocator(Locator.XPath, "//*[@type='submit' and contains(@name,'{0}') or contains(@id,'{0}')]");

        public AdminOptionSet(DriverContext driverContext) : base(driverContext)
        {
        }

        public void ClickToFooterLink()
        {
            Driver.GetElement<Button>(_footerLinkLocator).ClickTo();
        }

        public void ClickToGoback()
        {
            Driver.GetElement<Button>(_goBackLocator).ClickTo();
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
            }
        }
    }
}
