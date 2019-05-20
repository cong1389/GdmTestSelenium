using System;
using System.Linq;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using System.Collections.Generic;
using GenerateDocument.Common;
using GenerateDocument.Common.Extensions;
using GenerateDocument.Common.Types;
using GenerateDocument.Common.WebElements;
using GenerateDocument.Domain.Designs;
using GenerateDocument.Domain.TestSenario;
using GenerateDocument.Test.Base;

namespace GenerateDocument.Test.PageObjects.BackEnd
{
    public class AdminProductRetired : PageBaseObject, IAutoSave
    {
        private readonly ElementLocator
            _releaseOptionsLocator = new ElementLocator(Locator.ClassName, "tip_noindent"),
            _radioRelaseLocator = new ElementLocator(Locator.XPath, "//input[@type='radio' and contains(@id,'{0}') or contains(@name,'{0}')]"),
            _buttonLocator = new ElementLocator(Locator.XPath, "//*[@type='submit' and contains(@name,'{0}') or contains(@id,'{0}')]");

        public AdminProductRetired(DriverContext driverContext) : base(driverContext)
        {
        }

        [FindsBy(How = How.Id, Using = "AdminMaster_ContentPlaceHolderBody_btnUpdate")]
        private IWebElement UpdateSettingButton { get; set; }

        public void ClickToUpdateSetting()
        {
            UpdateSettingButton.Click();
        }

        public string[] GetTipNoIndentsTexts()
        {
            return Driver.GetElements(_releaseOptionsLocator).Select(x => x.Text).ToArray();
        }

        public void PerformToControlType(Step step, DesignModel designModel)
        {
            string controlType = step.ControlType;
            Enum.TryParse(controlType, true, out ControlTypes controlTypeValue);

            switch (controlTypeValue)
            {
                case ControlTypes.Radio:
                    Driver.GetElement<Radio>(_radioRelaseLocator.Format(step.ControlId)).TickRadio();
                    break;

                case ControlTypes.Button:
                    Driver.GetElement<Button>(_buttonLocator.Format(step.ControlId)).ClickTo();
                    break;
            }
        }
    }
}
