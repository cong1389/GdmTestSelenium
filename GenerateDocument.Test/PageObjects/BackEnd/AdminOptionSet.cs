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

            _buttonLocator = new ElementLocator(Locator.XPath, "//*[@type='submit' and contains(@name,'{0}') or contains(@id,'{0}')]"),
            _dragAndDropImageLocator = new ElementLocator(Locator.XPath, "//*[contains(text(),'{0}')]//ancestor::tr[1]//img[@title='Drag And Drop']");

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
            Enum.TryParse(step.ControlType, true, out ControlTypes controlTypeValue);

            switch (controlTypeValue)
            {
                case ControlTypes.Button:
                    TakeActionButtonTypes(step);
                    break;

                case ControlTypes.Image:
                    Driver.GetElement<Button>(_dragAndDropImageLocator.Format(step.ControlId)).ClickTo();
                    break;
            }
        }

        private void TakeActionButtonTypes(Step step)
        {
            Enum.TryParse(step.Action, true, out ActionTypes actionResult);

            switch (actionResult)
            {
                case ActionTypes.Click:
                    Driver.GetElement<Button>(_buttonLocator.Format(step.ControlId)).ClickTo();
                    break;

                case ActionTypes.Delete:
                    Driver.GetElement<Button>(_buttonLocator.Format(step.ControlId)).DeleteTo();
                    break;
            }
        }
    }
}
