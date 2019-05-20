using System;
using GenerateDocument.Common;
using GenerateDocument.Common.Extensions;
using GenerateDocument.Common.Types;
using GenerateDocument.Common.WebElements;
using GenerateDocument.Domain.Designs;
using GenerateDocument.Domain.TestSenario;
using GenerateDocument.Test.Base;
using GenerateDocument.Test.PageObjects.FrontEnd;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace GenerateDocument.Test.PageObjects.BackEnd
{
    public class AdminProductDetails : PageBaseObject, IAutoSave
    {
        private readonly ElementLocator
            _changeSettingButtonLocator = new ElementLocator(Locator.Id, "AdminMaster_ContentPlaceHolderBody_btnChangeSettings"),
            _submitJobCheckBoxLocator = new ElementLocator(Locator.Id, "AdminMaster_ContentPlaceHolderBody_ckAutoSubmit"),
            _downloadRadio3Locator = new ElementLocator(Locator.Id, "AdminMaster_ContentPlaceHolderBody_radDownloadOnly"),
            _priceTableValueLocator = new ElementLocator(Locator.Id, "AdminMaster_ContentPlaceHolderBody_ddPriceTable"),
            _releaseButtonLocator = new ElementLocator(Locator.Id, "AdminMaster_ContentPlaceHolderBody_btnRelease"),
            _customiseLinkLocator = new ElementLocator(Locator.XPath, "//div[contains(@class, 'steptype-FormFill')]//a"),
            _updateButtonLocator = new ElementLocator(Locator.Id, "AdminMaster_ContentPlaceHolderBody_btnUpdate"),
            _hyperlinkLocator = new ElementLocator(Locator.XPath, "//a[text()='{0}' or @href='{0}' or @id='{0}']"),

            _textboxLocator = new ElementLocator(Locator.XPath, "//input[contains(@name,'{0}') or contains(@id,'{0}')]"),
            _textAreaLocator = new ElementLocator(Locator.XPath, "//textarea[contains(@name,'{0}') or contains(@id,'{0}')]"),
            _buttonLocator = new ElementLocator(Locator.XPath, "//*[@type='submit' and contains(@name,'{0}') or contains(@id,'{0}')]");

        public AdminProductDetails(DriverContext driverContext) : base(driverContext)
        {
        }

        public void SelectSubmitJobCheckBox()
        {
            var submitEle = Driver.GetElement(_submitJobCheckBoxLocator);
            if (!submitEle.Selected)
            {
                submitEle.Click();
            }
        }

        public void ClickDownloadRadio3()
        {
            var downloadRadio3Ele = Driver.GetElement(_downloadRadio3Locator);

            if (!downloadRadio3Ele.Selected)
            {
                downloadRadio3Ele.Click();
            }
        }

        public void SettingPriceNone()
        {
            Driver.GetElement<Select>(_priceTableValueLocator).SelectedByValue("(None)");
        }

        public void ClickToChangeSetting()
        {
            Driver.GetElement(_changeSettingButtonLocator).Click();
        }

        public void ClickToRelease()
        {
            Driver.GetElement(_releaseButtonLocator).Click();
        }

        public void UpdateForMaintainOnly()
        {
            var element = DriverContext.BrowserWait()
                .Until(ExpectedConditions.ElementToBeClickable(By.Id("AdminMaster_ContentPlaceHolderBody_radLegacy")));
            if (!element.Selected)
            {
                element.Click();
            }
        }

        public void UpdateAndConvertToLatestRelease()
        {
            var element = DriverContext.BrowserWait()
                .Until(ExpectedConditions.ElementToBeClickable(By.Id("AdminMaster_ContentPlaceHolderBody_radConvert")));

            if (!element.Selected)
            {
                element.Click();
            }
        }

        public void ClickToFormFilling()
        {
            var lnk = Driver.GetElement(_customiseLinkLocator);

            Driver.ScrollToView(lnk);

            lnk.Click();
        }

        public void UpdateSettings(bool ifLastStep = false)
        {
            Driver.GetElement(_updateButtonLocator).Click();

            if (ifLastStep)
            {
                DriverContext.BrowserWait()
                    .Until(ExpectedConditions.ElementToBeClickable(
                        By.Id("AdminMaster_ContentPlaceHolderBody_btnDone")));
            }
        }

        public void PerformToControlType(Step step, DesignModel designModel)
        {
            string controlType = step.ControlType;
            Enum.TryParse(controlType, true, out ControlTypes controlTypeValue);

            switch (controlTypeValue)
            {
                case ControlTypes.Hyperlink:
                    Driver.GetElement<Button>(_hyperlinkLocator.Format(step.ControlId)).ClickTo();
                    break;

                case ControlTypes.Button:
                    Driver.GetElement<Button>(_buttonLocator.Format(step.ControlId)).ClickTo();
                    break;

                case ControlTypes.Textbox:
                    Driver.GetElement<Textbox>(_textboxLocator.Format(step.ControlId)).SetValue(step.ControlValue);
                    break;

                case ControlTypes.TextArea:
                    Driver.GetElement<Textbox>(_textAreaLocator.Format(step.ControlId)).SetValue(step.ControlValue);
                    break;
            }
        }
    }
}
