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
using GenerateDocument.Common.Helpers;
using GenerateDocument.Domain.TestSenario;

namespace GenerateDocument.Test.PageObjects.FrontEnd
{
    public class UserEditFormFilling : PageBaseObject, IAutoSave
    {
        private readonly ElementLocator
            _optionsLabel = new ElementLocator(Locator.XPath, "//td[@class='formFilling-form']//div[text()='{0}' or contains(text(), '{1}')]"),
            _optionsCommonLabel = new ElementLocator(Locator.XPath, "//td[@class='formFilling-form']//div[text()='{0}']//parent::a"),
            _optionsCommonId = new ElementLocator(Locator.XPath, "//div[@id='{0}']"),
            _optionsContain = new ElementLocator(Locator.XPath, "//div[@id='{0}']"),
            _uploadLogoButton = new ElementLocator(Locator.XPath, "//span[contains(@id, '_ContentPlaceHolderStepArea_InputFields_LinkButton482')]"),
            _uploadImageTextbox = new ElementLocator(Locator.XPath, "//input[@id='{0}']"),
            _uploadImageButton = new ElementLocator(Locator.XPath, "//div[contains(@id,'{0}')]//a"),
            _modalLarge = new ElementLocator(Locator.XPath, "//div[@class='modal modal--large']"),
            _imageEditArea = new ElementLocator(Locator.XPath, "//table[contains(@id, '_ContentPlaceHolderStepArea__UserImageEdit1__ModalPopUp1_tablePopUpArea')]"),
            _dropDownLocator = new ElementLocator(Locator.XPath, "//td[@class='formFilling-form']//select[@id='{0}']"),
            _completeRequiredFields = new ElementLocator(Locator.XPath, "//div[@class='warningAreaMessageWarning']"),
            _nextStepButtonLocator = new ElementLocator(Locator.XPath, "//a[contains(@id, '_StepNextN1_TheLabelButton')]"),
            _containerLocator = new ElementLocator(Locator.XPath, "//div[@id='{0}']//input"),
            _inputLocator = new ElementLocator(Locator.XPath, "//input[@id='{0}']"),
            _textAreaLocator = new ElementLocator(Locator.XPath, "//textarea[@id='{0}']"),
            _passwordLocator = new ElementLocator(Locator.XPath, "//input[@id='{0}' and @type='password']"),
            _radioLocator = new ElementLocator(Locator.XPath, "//input[@type='radio' and @id='{0}' and @value='{1}']"),
            _checkBoxLocator = new ElementLocator(Locator.XPath, "//input[@type='checkbox' and @id='{0}']"),
            _imageContainerLocator = new ElementLocator(Locator.XPath, "//div[@id='{0}']"),
            //_imageUploadBtnLocator = new ElementLocator(Locator.XPath, "//div[@id='{0}']//a//span[text()='Upload']"),
            //_imageClearBtnLocator = new ElementLocator(Locator.XPath, "//div[@id='{0}']//a[text()='Clear']"),
            _imageLableLocator = new ElementLocator(Locator.XPath, "//div[@id='{0}']//input[@disabled and contains(@id,'_IMGNAME')]"),
            _includeMultipleLocator = new ElementLocator(Locator.XPath, "//select[@id='{0}']"),
            _checkboxMultipleLocator = new ElementLocator(Locator.XPath, "//input[@type='checkbox' and @name='{0}']"),
            _listboxMultipleLocator = new ElementLocator(Locator.XPath, "//select[@id='{0}']");


        public UserEditFormFilling(DriverContext driverContext) : base(driverContext)
        {
        }

        private IWebElement UploadLogoTextbox => DriverContext.BrowserWait(20).Until(ExpectedConditions.ElementIsVisible(By.Id("FIELD_482_IMGNAME")));

        private IWebElement FooterDropdown => DriverContext.BrowserWait(5).Until(ExpectedConditions.ElementIsVisible(By.Id("FIELD_499")));

        private IWebElement SupportingLogosDropdown => DriverContext.BrowserWait(5).Until(ExpectedConditions.ElementIsVisible(By.Id("FIELD_836")));

        private IWebElement FirstSupportingLogoSection => DriverContext.BrowserWait(5).Until(ExpectedConditions.ElementIsVisible(By.Id("DIV_841")));

        private IWebElement SecondSupportingLogoSection => DriverContext.BrowserWait(5).Until(ExpectedConditions.ElementIsVisible(By.Id("DIV_842")));

        private IWebElement InputFile => DriverContext.BrowserWait(20).Until(ExpectedConditions.ElementExists(By.XPath("//div[contains(@id, 'html5_')]//input[@type='file']")));

        private IWebElement UploadFileSubmitLink => DriverContext.BrowserWait(20).Until(ExpectedConditions.ElementIsVisible(By.Id("il_upload_btn-upload")));

        public bool IsSupportingLogosDropdownDisplayed()
        {
            try
            {
                return SupportingLogosDropdown != null;
            }
            catch
            {
                return false;
            }
        }

        public bool IsFirstSupportingLogosSectionDisplayed()
        {
            try
            {
                return FirstSupportingLogoSection != null;
            }
            catch
            {
                return false;
            };
        }

        public bool IsSecondSupportingLogoSectionDisplayed()
        {
            try
            {
                return SecondSupportingLogoSection != null;
            }
            catch
            {
                return false;
            }
        }

        public void ClickToViewDesignOptions()
        {
            var groupEle = Driver.GetElement(_optionsCommonLabel.Format("View design options"));
            Driver.ScrollToView(groupEle);
            groupEle.Click();

            //Driver.WaitUntilPresentedElement(_optionsContain.Format("div1"), BaseConfiguration.LongTimeout);
        }

        public void ClickToViewTextOptions()
        {
            var groupEle = Driver.GetElement(_optionsLabel.Format("View text options", "View Text options"));
            Driver.ScrollToView(groupEle);
            groupEle.Click();

            Driver.WaitUntilPresentedElement(_optionsContain.Format("div2"), BaseConfiguration.LongTimeout);
        }

        public void ClickToViewFooterOptions()
        {
            var footerGroupEle = Driver.GetElement(_optionsCommonLabel.Format("View footer options*"));
            Driver.ScrollToView(footerGroupEle);
            footerGroupEle.Click();

            Driver.WaitUntilPresentedElement(_optionsContain.Format("div3"), BaseConfiguration.LongTimeout);
        }

        private void ClickToSubmitFileUpload()
        {
            var element = UploadFileSubmitLink;
            element.Click();
            DriverContext.BrowserWait().Until(ExpectedConditions.StalenessOf(element));
        }

        public void ClickToUploadFirstSupportingLogo(string path)
        {
            var uploadBtnEle = Driver.GetElement(_uploadImageButton.Format("DIV_841"));
            Driver.ScrollToView(uploadBtnEle);
            uploadBtnEle.Click();

            InputFile.SendKeys(path);
            ClickToSubmitFileUpload();

            Driver.WaitUntilElementIsNoLongerFound(_modalLarge, BaseConfiguration.LongTimeout);
        }

        public void ClickToUploadSecondSupportingLogo(string path)
        {
            var uploadBtnEle = Driver.GetElement(_uploadImageButton.Format("DIV_842"));
            Driver.ScrollToView(uploadBtnEle);
            uploadBtnEle.Click();

            InputFile.SendKeys(path);
            ClickToSubmitFileUpload();

            Driver.WaitUntilElementIsNoLongerFound(_modalLarge, BaseConfiguration.LongTimeout);

            //TODO
            //if (Driver.IsElementPresent(_imageEditArea))
            //{
            //    EditImage();
            //}
        }
        public void UploadDesignOptionFile(string path)
        {
            var uploadBtnEle = Driver.GetElement(_uploadLogoButton);
            Driver.ScrollToView(uploadBtnEle);
            uploadBtnEle.Click();

            InputFile.SendKeys(path);
            ClickToSubmitFileUpload();

            Driver.WaitUntilElementIsNoLongerFound(_modalLarge, BaseConfiguration.LongTimeout);

            if (Driver.IsElementPresent(_imageEditArea))
            {
                EditImage();
            }
        }

        public void ClickToSelectSupportingLogos()
        {
            new SelectElement(FooterDropdown).SelectByValue("logos");
        }

        public void ClickToSelectTwoSupportingLogos()
        {
            new SelectElement(SupportingLogosDropdown).SelectByValue("2");
        }

        public string GetUploadLogoValue()
        {
            Driver.ScrollToView(UploadLogoTextbox);
            return UploadLogoTextbox.GetAttribute("value");
        }

        public string GetFirstSupportingLogoValue()
        {
            var logoTextboxEle = Driver.GetElement(_uploadImageTextbox.Format("FIELD_841_IMGNAME"), e => e.Displayed);
            Driver.ScrollToView(logoTextboxEle);

            return logoTextboxEle.GetAttribute("value");
        }

        public string GetSecondSupportingLogoValue()
        {
            var logoTextboxEle = Driver.GetElement(_uploadImageTextbox.Format("FIELD_842_IMGNAME"), e => e.Displayed);
            Driver.ScrollToView(logoTextboxEle);

            return logoTextboxEle.GetAttribute("value");
        }

        public List<IWebElement> GetAllDisplayedTextareas()
        {
            return DriverContext.BrowserWait().Until<List<IWebElement>>((d) =>
            {
                var elements = d.FindElements(By.XPath("//div[contains(@id, '_ContentPlaceHolderStepArea_InputFields_InputFields')]//textarea")).Where(x => x.Displayed).ToList();
                if (elements.Count > 0)
                {
                    return elements.ToList();
                }

                return null;
            });
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

        private void EditImage()
        {
            var imageEditorButton = DriverContext.BrowserWait().Until(ExpectedConditions.ElementIsVisible(
                By.XPath("//div[contains(@id, '_ContentPlaceHolderStepArea__UserImageEdit1__ModalPopUp1_ButtonSave')]")));
            imageEditorButton.Click();
            DriverContext.BrowserWait(30).Until(ExpectedConditions.StalenessOf(imageEditorButton));
        }

        public UserEditFormFilling ExpandOptions(string groupName, string groupId)
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

        public UserEditFormFilling SelectByValue(string ctrId, string value)
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

        public Dictionary<string, string> EnteringValueInputsTextInOptions(string optionsContainId, string text)
        {
            var valueInputs = new Dictionary<string, string>();
            Driver.GetElements(_containerLocator.Format(optionsContainId)).ToList().ForEach(x =>
            {
                Driver.ScrollToView(x);

                x.Clear();
                x.SendKeys(text);
                x.SendKeys(Keys.Tab);

                valueInputs.Add(x.GetAttribute("id"), text);
            });

            return valueInputs;
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

        public Dictionary<string, string> GetValueInputsTextInOptions(string optionsContainId)
        {
            var valueOutputs = new Dictionary<string, string>();

            Driver.GetElements(_containerLocator.Format(optionsContainId), 3).ToList().ForEach(x =>
              {
                  Driver.ScrollToView(x);
                  valueOutputs.Add(x.GetAttribute("id"), x.GetAttribute("value"));
              });

            return valueOutputs;
        }

        public UserEditFormFilling ClickToNextStep()
        {
            Driver.GetElement<Button>(_nextStepButtonLocator).ClickTo();

            //Check required fields
            if (Driver.IsUrlEndsWith("usereditformfilling") && Driver.IsElementPresent(_completeRequiredFields))
            {
                Driver.GetElement<Button>(_completeRequiredFields).ClickTo();
            }

            if (Driver.IsUrlEndsWith("usereditprinting"))
            {
                Driver.WaitUntilPresentedUrl("usereditprinting");
            }
            else if (Driver.IsUrlEndsWith("usereditfinish"))
            {
                Driver.WaitUntilPresentedUrl("usereditfinish");
            }

            return this;
        }

        private UserEditFormFilling UploadImageControl(string containerId, string fileName)
        {
            var imagePath = Path.Combine(ProjectBaseConfiguration.Contents, fileName);
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

        public void PerformToControlType(Step step)
        {
            string text;

            string controlType = step.ControlType;
            Enum.TryParse(controlType, true, out ControlTypes controlTypeValue);

            switch (controlTypeValue)
            {
                case ControlTypes.Textbox:
                    text = string.IsNullOrEmpty(step.ControlValue) ? NameHelper.RandomName(10) : step.ControlValue;
                    SendValueToInputText(step.ControlId, text);
                    break;

                case ControlTypes.TextArea:
                    text = string.IsNullOrEmpty(step.ControlValue) ? NameHelper.RandomName(100) : step.ControlValue;
                    SendValueToTextArea(step.ControlId, text);
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
                    if (step.Action.Equals("upload"))
                    {
                        UploadImageControl(step.ControlId, step.ControlValue);
                    }
                    else if (step.Action.Equals("clear"))
                    {
                        Console.WriteLine($"Image click to reset");
                        ClearImageControl(step.ControlId);
                    }
                    else if (step.Action.Equals("reset"))
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
                    if (step.Action.Equals("add"))
                    {
                        AddItemsToListboxInclude(step.ControlId, step.ControlValue);
                    }
                    else if (step.Action.Equals("remove"))
                    {
                        RemoveItemsToListboxInclude(step.ControlId, step.ControlValue);
                    }

                    break;

                case ControlTypes.MultipleSelectCheckbox:
                    if (step.Action.Equals("checked"))
                    {
                        TickItemsToListCheckbox(step.ControlId, step.ControlValue);
                    }
                    else if (step.Action.Equals("unchecked"))
                    {
                        UnTickItemsToCheckListbox(step.ControlId, step.ControlValue);
                    }

                    break;

                case ControlTypes.MultipleSelectListbox:
                    if (step.Action.Equals("selected"))
                    {
                        SelectItemsToListbox(step.ControlId, step.ControlValue);
                    }

                    break;
            }
        }
    }
}
