using System;
using GenerateDocument.Common;
using GenerateDocument.Common.Extensions;
using GenerateDocument.Common.Types;
using GenerateDocument.Common.WebElements;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using GenerateDocument.Common.Helpers;
using GenerateDocument.Domain.Designs;
using GenerateDocument.Domain.TestSenario;
using GenerateDocument.Test.Base;

namespace GenerateDocument.Test.PageObjects.FrontEnd
{
    public class UserEditFormFilling : PageBaseObject, IAutoSave
    {
        private readonly ElementLocator
            _optionsLabel = new ElementLocator(Locator.XPath, "//td[@class='formFilling-form']//div[text()='{0}' or contains(text(), '{1}')]"),
            _optionsCommonLabel = new ElementLocator(Locator.XPath, "//td[@class='formFilling-form']//div[text()='{0}']//parent::a"),
            _optionsCommonId = new ElementLocator(Locator.XPath, "//div[@id='{0}']"),
            _optionsContain = new ElementLocator(Locator.XPath, "//div[@id='{0}']"),
            _dropDownLocator = new ElementLocator(Locator.XPath, "//td[@class='formFilling-form']//select[@id='{0}']"),
            _completeRequiredFields = new ElementLocator(Locator.XPath, "//div[@class='warningAreaMessageWarning']"),
            _nextStepButtonLocator = new ElementLocator(Locator.XPath, "//a[contains(@id, '_StepNextN1_TheLabelButton')]"),
            _inputLocator = new ElementLocator(Locator.XPath, "//input[@id='{0}']"),
            _textAreaLocator = new ElementLocator(Locator.XPath, "//textarea[@id='{0}']"),
            _passwordLocator = new ElementLocator(Locator.XPath, "//input[@id='{0}' and @type='password']"),
            _radioLocator = new ElementLocator(Locator.XPath, "//input[@type='radio' and @id='{0}' and @value='{1}']"),
            _checkBoxLocator = new ElementLocator(Locator.XPath, "//input[@type='checkbox' and @id='{0}']"),
            _imageContainerLocator = new ElementLocator(Locator.XPath, "//div[@id='{0}']"),
            _imageLableLocator = new ElementLocator(Locator.XPath, "//div[@id='{0}']//input[@disabled and contains(@id,'_IMGNAME')]"),
            _includeMultipleLocator = new ElementLocator(Locator.XPath, "//select[@id='{0}']"),
            _checkboxMultipleLocator = new ElementLocator(Locator.XPath, "//input[@type='checkbox' and @name='{0}']"),
            _listboxMultipleLocator = new ElementLocator(Locator.XPath, "//select[@id='{0}']");


        public UserEditFormFilling(DriverContext driverContext) : base(driverContext)
        {
        }

        public void ClickToViewTextOptions()
        {
            var groupEle = Driver.GetElement(_optionsLabel.Format("View text options", "View Text options"));
            Driver.ScrollToView(groupEle);
            groupEle.OnClickJavaScript();

            Driver.WaitUntilPresentedElement(_optionsContain.Format("div2"), BaseConfiguration.LongTimeout);
        }

        public List<IWebElement> GetAllInputFields()
        {
            try
            {
                DriverContext.BrowserWait().Until(ExpectedConditions.ElementIsVisible(By.Id("ctl00_ctl00_ContentPlaceHolderBody_StepArea1_ContentPlaceHolderStepArea_InputFields_InputFields")));

                var fields = DriverContext.BrowserWait(5).Until<List<IWebElement>>(d =>
               {
                   var xpath = "//div[contains(@id, '_ContentPlaceHolderStepArea_InputFields_InputFields')]//p//input | //div[contains(@id, '_ContentPlaceHolderStepArea_InputFields_InputFields')]//textarea";
                   var elements = d.FindElements(By.XPath(xpath)).Where(x => x.Displayed).ToList();
                   if (elements.Count > 0)
                   {
                       return elements.ToList();
                   }

                   return null;
               });

                return fields;
            }
            catch
            {
                return null;
            }
        }

        private UserEditFormFilling ExpandOptions(string groupName, string groupId)
        {
            if (string.IsNullOrEmpty(groupName) || string.IsNullOrEmpty(groupId))
            {
                return this;
            }
            var statusGroupContent = Driver.IsElementPresent(_optionsCommonId.Format(groupId), BaseConfiguration.ShortTimeout);
            if (!statusGroupContent)
            {
                var groupEle = Driver.GetElement(_optionsCommonLabel.Format(groupName));
                Driver.ScrollToView(groupEle);
                groupEle.OnClickJavaScript();
            }

            return this;
        }

        private UserEditFormFilling SelectByValue(string ctrId, string value)
        {
            Driver.GetElement<Select>(_dropDownLocator.Format(ctrId)).SelectedByValue(value);

            return this;
        }

        private void TickItemsToListCheckbox(string ctrId, string values)
        {
            Driver.GetElement<MultipleSelectCheckbox>(_checkboxMultipleLocator.Format(ctrId)).MultipleTick(values);
        }

        private void UnTickItemsToCheckListbox(string ctrId, string values)
        {
            Driver.GetElement<MultipleSelectCheckbox>(_checkboxMultipleLocator.Format(ctrId)).MultipleUnTick(values);
        }

        private void AddItemsToListboxInclude(string ctrId, string values)
        {
            Driver.GetElement<MultipleSelectInclude>(_includeMultipleLocator.Format(ctrId), e => e.Enabled).IncludeItems(values);
        }

        private void RemoveItemsToListboxInclude(string ctrId, string values)
        {
            Driver.GetElement<MultipleSelectInclude>(_includeMultipleLocator.Format(ctrId), e => e.Enabled).ExcludeItems(values);
        }

        private void SelectItemsToListbox(string ctrId, string values)
        {
            Driver.GetElement<MultipleSelectListbox>(_listboxMultipleLocator.Format(ctrId)).SelectedItems(values);
        }

        private UserEditFormFilling TickRadio(string ctrId, string value)
        {
            Driver.GetElement<Radio>(_radioLocator.Format(ctrId, value)).TickRadio();

            return this;
        }

        private UserEditFormFilling TickOrUnTickCheckBox(string ctrId, bool isCheck)
        {
            if (isCheck)
            {
                Driver.GetElement<Checkbox>(_checkBoxLocator.Format(ctrId)).TickCheckBox();
            }
            else
            {
                Driver.GetElement<Checkbox>(_checkBoxLocator.Format(ctrId)).UnTickCheckBox();
            }

            return this;
        }

        private void SendValueToInputText(string controlId, string text)
        {
            Driver.GetElement<Textbox>(_inputLocator.Format(controlId)).SetValue(text);
        }

        private void SendValueToTextArea(string controlId, string text)
        {
            Driver.GetElement<Textbox>(_textAreaLocator.Format(controlId)).SetValue(text);
        }

        private void SendValueToPassword(string controlId, string text)
        {
            Driver.GetElement<Textbox>(_passwordLocator.Format(controlId)).SetValue(text);
        }

        public UserEditFormFilling ClickToNextStep()
        {
            Driver.GetElement<Button>(_nextStepButtonLocator).ClickTo();

            //Check required fields
            if (Driver.IsUrlEndsWith(PageTypes.UserEditFormFilling.ToString()) && Driver.IsElementPresent(_completeRequiredFields, BaseConfiguration.ShortTimeout))
            {
                Driver.GetElement<Button>(_completeRequiredFields).ClickTo();
            }

            if (Driver.IsUrlEndsWith(PageTypes.UserEditPrinting.ToString()))
            {
                Driver.WaitUntilPresentedUrl(PageTypes.UserEditPrinting.ToString());
            }
            else if (Driver.IsUrlEndsWith(PageTypes.UserEditFinish.ToString()))
            {
                Driver.WaitUntilPresentedUrl(PageTypes.UserEditFinish.ToString());
            }

            return this;
        }

        private UserEditFormFilling UploadImageControl(string containerId, string fileName)
        {
            var imagePath = Path.Combine(ProjectBaseConfiguration.ContentsFolder, fileName);
            Driver.GetElement<ImageUpload>(_imageContainerLocator.Format(containerId)).Upload(imagePath);

            return this;
        }

        private UserEditFormFilling ClearImageControl(string containerId)
        {
            Driver.GetElement<ImageUpload>(_imageContainerLocator.Format(containerId)).ClearImage();

            return this;
        }

        private UserEditFormFilling ResetImageControl(string containerId)
        {
            Driver.GetElement<ImageUpload>(_imageContainerLocator.Format(containerId)).ResetImage();

            return this;
        }

        public string GetImageNameAfterUploaded(string containerId)
        {
            //Expand group options
            var containerLocator = new ElementLocator(Locator.XPath, "//div[@id='{0}']//ancestor::div[@groupname]");
            var statusGroupContent = Driver.IsElementPresent(containerLocator.Format(containerId), BaseConfiguration.ShortTimeout);
            if (!statusGroupContent)
            {
                var groupContainerEle = Driver.GetElement(containerLocator.Format(containerId), e => e.Enabled);
                Driver.ScrollToView(groupContainerEle);

                var id = groupContainerEle.GetAttribute("id");
                var headerGroupLocator = new ElementLocator(Locator.XPath, "//a[contains(@onclick,'{0}')]");
                Driver.GetElement(headerGroupLocator.Format(id)).OnClickJavaScript();
            }

            return Driver.GetElement<ImageUpload>(_imageLableLocator.Format(containerId), e => e.Displayed).GetImageName();
        }

        public void PerformToControlType(Step step, DesignModel designModel)
        {
            string text, sfId;

            Enum.TryParse(step.ControlType, true, out ControlTypes controlTypeValue);

            switch (controlTypeValue)
            {
                case ControlTypes.Textbox:
                    text = string.IsNullOrEmpty(step.ControlValue) ? NameHelper.RandomName(10) : step.ControlValue;

                    sfId = GetSfFieldId(step.ControlId);
                    SendValueToInputText(sfId, text);
                    break;

                case ControlTypes.TextArea:
                    text = string.IsNullOrEmpty(step.ControlValue) ? NameHelper.RandomName(100) : step.ControlValue;

                    sfId = GetSfFieldId(step.ControlId);
                    SendValueToTextArea(sfId, text);
                    break;

                case ControlTypes.Password:
                    text = string.IsNullOrEmpty(step.ControlValue) ? NameHelper.RandomName(10) : step.ControlValue;
                    SendValueToPassword(step.ControlId, text);
                    break;

                case ControlTypes.Dropbox:
                case ControlTypes.Listbox:
                    SelectByValue(step.ControlId, step.ControlValue);
                    break;

                case ControlTypes.Radio:
                    TickRadio(step.ControlId, step.ControlValue);
                    break;

                case ControlTypes.Checkbox:
                    TickOrUnTickCheckBox(step.ControlId, Boolean.Parse(step.ControlValue));
                    break;

                case ControlTypes.Image:
                    if (step.Action.Equals(ActionTypes.Upload.ToString(), StringComparison.OrdinalIgnoreCase))
                    {
                        UploadImageControl(step.ControlId, step.ControlValue);
                    }
                    else if (step.Action.Equals(ActionTypes.Clear.ToString(), StringComparison.OrdinalIgnoreCase))
                    {
                        ClearImageControl(step.ControlId);
                    }
                    else if (step.Action.Equals(ActionTypes.Reset.ToString(), StringComparison.OrdinalIgnoreCase))
                    {
                        ResetImageControl(step.ControlId);
                    }
                    break;

                case ControlTypes.Button:
                    ClickToNextStep();
                    break;

                case ControlTypes.Container:
                    ExpandOptions(step.ControlValue, step.ControlId);
                    break;

                case ControlTypes.MultipleSelectInclude:
                    if (step.Action.Equals(ActionTypes.Add.ToString(), StringComparison.OrdinalIgnoreCase))
                    {
                        AddItemsToListboxInclude(step.ControlId, step.ControlValue);
                    }
                    else if (step.Action.Equals(ActionTypes.Remove.ToString(), StringComparison.OrdinalIgnoreCase))
                    {
                        RemoveItemsToListboxInclude(step.ControlId, step.ControlValue);
                    }

                    break;

                case ControlTypes.MultipleSelectCheckbox:
                    if (step.Action.Equals(ActionTypes.Tick.ToString(), StringComparison.OrdinalIgnoreCase))
                    {
                        TickItemsToListCheckbox(step.ControlId, step.ControlValue);
                    }
                    else if (step.Action.Equals(ActionTypes.UnTick.ToString(), StringComparison.OrdinalIgnoreCase))
                    {
                        UnTickItemsToCheckListbox(step.ControlId, step.ControlValue);
                    }

                    break;

                case ControlTypes.MultipleSelectListbox:
                    if (step.Action.Equals(ActionTypes.Add.ToString(), StringComparison.OrdinalIgnoreCase))
                    {
                        SelectItemsToListbox(step.ControlId, step.ControlValue);
                    }
                    break;
            }
        }

        private string GetSfFieldId(string controlId)
        {
            var pattern = @"{GetSfFieldId\(([A-Za-z0-9\-]+)\)}";

            var match = Regex.Match(controlId, pattern, RegexOptions.IgnoreCase);
            if (!match.Success)
            {
                return controlId;
            }

            var fieldName = Regex.Replace(controlId, @"{GetSfFieldId\(", "");
            fieldName = Regex.Replace(fieldName, "\\)\\}", "");

            var jsExecutor = (IJavaScriptExecutor)Driver;
            var sfId = jsExecutor.ExecuteScript($"return FieldIDs['{fieldName}'];")?.ToString();
            if (string.IsNullOrEmpty(sfId))
            {
                return sfId;
            }

            return $"FIELD_{sfId}";
        }
    }
}
