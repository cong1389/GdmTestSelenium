using GenerateDocument.Common;
using GenerateDocument.Common.Extensions;
using GenerateDocument.Common.Types;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System.Collections.Generic;
using System.Linq;
using GenerateDocument.Common.WebElements;
using System.IO;
using System;

namespace GenerateDocument.Test.PageObjects.FrontEnd
{
    public class UserEditFormFilling : PageBaseObject
    {
        private readonly ElementLocator
            _optionsLabel = new ElementLocator(Locator.XPath,
                "//td[@class='formFilling-form']//div[text()='{0}' or contains(text(), '{1}')]"),
            _optionsCommonLabel = new ElementLocator(Locator.XPath,
                "//td[@class='formFilling-form']//div[text()='{0}']//parent::a"),
            _optionsCommonId = new ElementLocator(Locator.XPath, "//div[@id='{0}']"),
            _optionsContain = new ElementLocator(Locator.XPath, "//div[@id='{0}']"),
            _uploadLogoButton = new ElementLocator(Locator.XPath,
                "//span[contains(@id, '_ContentPlaceHolderStepArea_InputFields_LinkButton482')]"),
            _uploadImageTextbox = new ElementLocator(Locator.XPath, "//input[@id='{0}']"),
            _uploadImageButton = new ElementLocator(Locator.XPath, "//div[contains(@id,'{0}')]//a"),
            _modalLarge = new ElementLocator(Locator.XPath, "//div[@class='modal modal--large']"),
            _chooseFileButtonOfModal = new ElementLocator(Locator.XPath, "//x-upload-file//input[@type='file']"),
            _uploadButtonOfModal = new ElementLocator(Locator.XPath, "il_upload_btn-upload"),
            _textAreaField = new ElementLocator(Locator.XPath,
                "//div[contains(@id, '_ContentPlaceHolderStepArea_InputFields_InputFields')]//textarea"),
            _imageEditArea = new ElementLocator(Locator.XPath,
                "//table[contains(@id, '_ContentPlaceHolderStepArea__UserImageEdit1__ModalPopUp1_tablePopUpArea')]"),
            _dropDownLocator = new ElementLocator(Locator.XPath, "//td[@class='formFilling-form']//select[@id='{0}']"),
            _completeRequiredFields = new ElementLocator(Locator.XPath, "//div[@class='warningAreaMessageWarning']"),
            _nextStepButtonLocator =
                new ElementLocator(Locator.XPath, "//a[contains(@id, '_StepNextN1_TheLabelButton')]"),
            _containerLocator = new ElementLocator(Locator.XPath, "//div[@id='{0}']//input"),
            _inputLocator = new ElementLocator(Locator.XPath, "//*[@id='{0}']"),
            _radioLocator = new ElementLocator(Locator.XPath, "//input[@type='radio' and @id='{0}' and @value='{1}']"),
            _checkBoxLocator = new ElementLocator(Locator.XPath, "//input[@type='radio' and @id='{0}'"),
            _imageUploadBtnLocator = new ElementLocator(Locator.XPath, "//div[@id='{0}']//a//span[text()='Upload']"),
            _imageLableLocator = new ElementLocator(Locator.XPath, "//div[@id='{0}']//input[@disabled and contains(@id,'_IMGNAME')]"),
            _chooseImageLocator = new ElementLocator(Locator.XPath, "//div[contains(@id, 'html5_')]//input[@type='file']"),
            _submitUploadBtnLocator = new ElementLocator(Locator.Id, "il_upload_btn-upload");

        public UserEditFormFilling(DriverContext driverContext) : base(driverContext)
        {
        }

        private IWebElement ViewImageOptionLabel => DriverContext.BrowserWait(20).Until(ExpectedConditions.ElementIsVisible(By.LinkText("View Image options")));

        private IWebElement UploadLogoButton => DriverContext.BrowserWait(20).Until(ExpectedConditions.ElementIsVisible(By.XPath("//span[contains(@id, '_ContentPlaceHolderStepArea_InputFields_LinkButton482')]")));

        private IWebElement UploadFirstImageButton => DriverContext.BrowserWait(20).Until(ExpectedConditions.ElementIsVisible(By.XPath("//span[contains(@id, '_ContentPlaceHolderStepArea_InputFields_LinkButton20826')]")));

        private IWebElement UploadSecondImageButton => DriverContext.BrowserWait(20).Until(ExpectedConditions.ElementIsVisible(By.XPath("//span[contains(@id, '_ContentPlaceHolderStepArea_InputFields_LinkButton20827')]")));

        private IWebElement UploadLogoTextbox => DriverContext.BrowserWait(20).Until(ExpectedConditions.ElementIsVisible(By.Id("FIELD_482_IMGNAME")));

        private IWebElement FirstImageTextbox => DriverContext.BrowserWait(20).Until(ExpectedConditions.ElementIsVisible(By.Id("FIELD_20826_IMGNAME")));

        private IWebElement SecondImageTextbox => DriverContext.BrowserWait(20).Until(ExpectedConditions.ElementIsVisible(By.Id("FIELD_20827_IMGNAME")));

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

        public void ClickToViewDesignOptions(bool clickToOpen = true)
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

        public void ClickToViewFooterOptions(bool clickToOpen = true)
        {
            var footerGroupEle = Driver.GetElement(_optionsCommonLabel.Format("View footer options*"));
            Driver.ScrollToView(footerGroupEle);
            footerGroupEle.Click();

            Driver.WaitUntilPresentedElement(_optionsContain.Format("div3"), BaseConfiguration.LongTimeout);
        }

        public void ClickToViewImageOptions(bool clickToOpen = true)
        {
            Driver.ScrollToView(ViewImageOptionLabel);
            if (clickToOpen && !GetImageOptionContent().Displayed)
            {
                ViewImageOptionLabel.Click();
                DriverContext.BrowserWait().Until(x => GetImageOptionContent().Displayed);
            }
            else if (!clickToOpen && GetImageOptionContent().Displayed)
            {
                ViewImageOptionLabel.Click();
                DriverContext.BrowserWait().Until(x => !GetImageOptionContent().Displayed);
            }
        }

        public void ClickToUploadFirstImageOptionFile(string path)
        {
            Driver.ScrollToView(UploadFirstImageButton);
            UploadFirstImageButton.Click();

            InputFile.SendKeys(path);
            ClickToSubmitFileUpload();

            if (Driver.IsElementPresent(_imageEditArea))
            {
                EditImage();
            }
        }

        public void ClickToUploadSecondImageOptionFile(string path)
        {
            Driver.ScrollToView(UploadSecondImageButton);
            UploadSecondImageButton.Click();

            InputFile.SendKeys(path);
            ClickToSubmitFileUpload();

            if (Driver.IsElementPresent(_imageEditArea))
            {
                EditImage();
            }
        }

        public void ClickToSubmitFileUpload()
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

        public string GetFirstUploadImageValue()
        {
            Driver.ScrollToView(FirstImageTextbox);
            return FirstImageTextbox.GetAttribute("value");
        }

        public string GetSecondUploadImageValue()
        {
            Driver.ScrollToView(SecondImageTextbox);

            return SecondImageTextbox.GetAttribute("value");
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

        public List<IWebElement> GetDesignOptionLayoutRadios()
        {
            return DriverContext.BrowserWait(20).Until(d =>
            {
                var elements = d.FindElements(By.Id("FIELD_51484")).ToList();
                return elements.Count > 0 ? elements.ToList() : null;
            });
        }

        private IWebElement GetImageOptionContent()
        {
            return DriverContext.BrowserWait().Until(ExpectedConditions.ElementExists(By.Id("div4")));
        }

        public void SetSelectDesignOptionLayout(List<IWebElement> radiobuttons, int index)
        {
            var selectElement = radiobuttons[index];
            Driver.ScrollToView(selectElement);
            selectElement.Click();
        }

        public bool CheckSelectDesignOptionLayout(List<IWebElement> radiobuttons, int index)
        {
            var selectElement = radiobuttons[index];
            Driver.ScrollToView(selectElement);

            return selectElement.Selected;
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

        public UserEditFormFilling TickRadio(string ctrId, string value)
        {
            Driver.GetElement<Radio>(_radioLocator.Format(ctrId, value)).TickRadio();

            return this;
        }

        public UserEditFormFilling TickOrUnTickCheckBox(string ctrId, bool isCheck)
        {
            if (isCheck)
            {
                Driver.GetElement<CheckBox>(_checkBoxLocator.Format(ctrId)).TickCheckBox();
            }
            else
            {
                Driver.GetElement<CheckBox>(_checkBoxLocator.Format(ctrId)).UnTickCheckBox();
            }

            return this;
        }

        public string GetSelectedValue(string ctrId)
        {
            Select drpEle = Driver.GetElement<Select>(_dropDownLocator.Format(ctrId));
            return drpEle.Selected().SelectedOption.Text;
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

        public void EnteringValueInputTextInOptions(string controlId, string text)
        {
            var inputEle = Driver.GetElement(_inputLocator.Format(controlId));
            Driver.ScrollToView(inputEle);
            inputEle.Clear();
            inputEle.SendKeys(text);
            inputEle.SendKeys(Keys.Tab);
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
            var nextButtonEle = Driver.GetElement(_nextStepButtonLocator);
            Driver.ScrollToView(nextButtonEle);
            nextButtonEle?.Click();

            //Check required fields
            if (Driver.IsUrlEndsWith("usereditformfilling") && Driver.IsElementPresent(_completeRequiredFields))
            {
                var requiredFieldsEle = Driver.GetElement(_completeRequiredFields);
                requiredFieldsEle?.Click();
            }

            return this;
        }

        public UserEditFormFilling UploadImageControl(string containerId, string fileName)
        {
            var imagePath = Path.Combine(ProjectBaseConfiguration.Contents, fileName);
            Driver.GetElement<ImageUpload>(_imageUploadBtnLocator.Format(containerId)).Upload(imagePath);

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
                Driver.GetElement(headerGroupLocator.Format(id)).OnClickJavaScript(); ;
            }

            return Driver.GetElement<ImageUpload>(_imageLableLocator.Format(containerId), e => e.Displayed).GetImageName();

            //var inputLableEle = Driver.GetElement(_imageLableLocator.Format(containerId), e => e.Displayed);
            //Driver.ScrollToView(inputLableEle);

            //return inputLableEle.GetAttribute("value");
        }

    }
}
