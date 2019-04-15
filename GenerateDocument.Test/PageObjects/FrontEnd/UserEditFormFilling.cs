using System;
using System.Collections.Generic;
using System.Linq;
using GenerateDocument.Test.Utilities;
using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;

namespace GenerateDocument.Test.PageObjects.FrontEnd
{
    public class UserEditFormFilling : PageObject
    {
        public UserEditFormFilling(IWebDriver browser) : base(browser)
        {
        }

        public override Uri Page
        {
            get
            {
                BrowserWait().Until(ExpectedConditions.UrlContains("UserEditFormFilling"));

                return base.Page;
            }
        }

        private IWebElement ShareToFacebookIcon
        {
            get
            {
                try
                {
                    return BrowserWait(5).Until(ExpectedConditions.ElementIsVisible(By.CssSelector(".share-button__facebook>img")));

                }
                catch
                {
                    return null;
                }
            }
        }

        private IWebElement ShareToTwitterIcon
        {
            get
            {
                try
                {
                    return BrowserWait(5).Until(ExpectedConditions.ElementIsVisible(By.CssSelector(".share-button__twitter>img")));

                }
                catch
                {
                    return null;
                }
            }
        }

        private IWebElement GetLinkIcon
        {
            get
            {
                try
                {
                    return BrowserWait(5).Until(ExpectedConditions.ElementIsVisible(By.ClassName("share-button__get-link--action")));

                }
                catch
                {
                    return null;
                }
            }
        }

        private IWebElement ViewDesignOptionLabel => BrowserWait().Until(ExpectedConditions.ElementIsVisible(By.LinkText("View design options")));

        private IWebElement ViewTextOptionLabel => BrowserWait(20).Until(ExpectedConditions.ElementIsVisible(By.XPath("//div[contains(text(), 'View Text options') or contains(text(), 'View text options')]")));

        private IWebElement ViewFooterOptionLabel => BrowserWait(20).Until(ExpectedConditions.ElementIsVisible(By.LinkText("View footer options*")));

        private IWebElement ViewImageOptionLabel => BrowserWait(20).Until(ExpectedConditions.ElementIsVisible(By.LinkText("View Image options")));

        private IWebElement UploadLogoButton => BrowserWait(20).Until(ExpectedConditions.ElementIsVisible(By.XPath("//span[contains(@id, '_ContentPlaceHolderStepArea_InputFields_LinkButton482')]")));

        private IWebElement UploadFirstImageButton => BrowserWait(20).Until(ExpectedConditions.ElementIsVisible(By.XPath("//span[contains(@id, '_ContentPlaceHolderStepArea_InputFields_LinkButton20826')]")));

        private IWebElement UploadSecondImageButton => BrowserWait(20).Until(ExpectedConditions.ElementIsVisible(By.XPath("//span[contains(@id, '_ContentPlaceHolderStepArea_InputFields_LinkButton20827')]")));

        private IWebElement UploadLogoTextbox => BrowserWait(20).Until(ExpectedConditions.ElementIsVisible(By.Id("FIELD_482_IMGNAME")));

        private IWebElement FirstImageTextbox => BrowserWait(20).Until(ExpectedConditions.ElementIsVisible(By.Id("FIELD_20826_IMGNAME")));

        private IWebElement SecondImageTextbox => BrowserWait(20).Until(ExpectedConditions.ElementIsVisible(By.Id("FIELD_20827_IMGNAME")));

        private IWebElement FirstSupportingLogoTextbox => BrowserWait(20).Until(ExpectedConditions.ElementIsVisible(By.Id("FIELD_841_IMGNAME")));

        private IWebElement SecondSupportingLogoTextbox => BrowserWait(20).Until(ExpectedConditions.ElementIsVisible(By.Id("FIELD_842_IMGNAME")));

        private IWebElement FooterDropdown => BrowserWait(5).Until(ExpectedConditions.ElementIsVisible(By.Id("FIELD_499")));

        private IWebElement SupportingLogosDropdown => BrowserWait(5).Until(ExpectedConditions.ElementIsVisible(By.Id("FIELD_836")));

        private IWebElement FirstSupportingLogoSection => BrowserWait(5).Until(ExpectedConditions.ElementIsVisible(By.Id("DIV_841")));

        private IWebElement SecondSupportingLogoSection => BrowserWait(5).Until(ExpectedConditions.ElementIsVisible(By.Id("DIV_842")));

        private IWebElement FirstSupportingLogoUploadButton => BrowserWait(5).Until(ExpectedConditions.ElementIsVisible(By.XPath("//div[contains(@id, 'DIV_841')]//a")));

        private IWebElement SecondSupportingLogoUploadButton => BrowserWait(5).Until(ExpectedConditions.ElementIsVisible(By.XPath("//div[contains(@id, 'DIV_842')]//a")));

        private IWebElement PrintMethodDropdown => BrowserWait(5).Until(ExpectedConditions.ElementIsVisible(By.Id("FIELD_51490")));

        private IWebElement InputFile => BrowserWait(20).Until(ExpectedConditions.ElementExists(By.XPath("//div[contains(@id, 'html5_')]//input[@type='file']")));

        private IWebElement UploadFileSubmitLink => BrowserWait(20).Until(ExpectedConditions.ElementIsVisible(By.Id("il_upload_btn-upload")));

        private IWebElement SocialNetworkSharingPopupMessage => BrowserWait(5).Until(ExpectedConditions.ElementIsVisible(By.CssSelector(".panel-body>p")));

        private IWebElement SocialNetworkSharingGoToHomeButton => BrowserWait(5).Until(ExpectedConditions.ElementIsVisible(By.CssSelector("button[data-ng-click='homePage()']")));

        private IWebElement SocialNetworkSharingReturnToTemplateButton => BrowserWait(5).Until(ExpectedConditions.ElementIsVisible(By.CssSelector("button[data-ng-click='cancel()']")));

        private IWebElement GetLinkTextbox => BrowserWait().Until(ExpectedConditions.ElementIsVisible(By.Id("link_share")));

        private IWebElement GetLinkCopyToClipboardButton => BrowserWait().Until(ExpectedConditions.ElementIsVisible(By.CssSelector(".wrap-text__button>img")));

        private IWebElement GetLinkCopyStatus => BrowserWait().Until(ExpectedConditions.ElementIsVisible(By.CssSelector(".share-link__copy-status>label")));

        public bool IsGetLinkTextboxNotNull()
        {
            return GetLinkTextbox != null;
        }

        public bool IsShareToFacebookIconNull()
        {
            return ShareToFacebookIcon == null;
        }

        public bool IsShareToTwitterIconNull()
        {
            return ShareToTwitterIcon == null;
        }

        public bool IsGetLinkIconNull()
        {
            return GetLinkIcon == null;
        }

        public bool IsShareToFacebookIconEnabled()
        {
            return ShareToFacebookIcon.Enabled;
        }

        public bool IsShareToTwitterIconEnabled()
        {
            return ShareToTwitterIcon.Enabled;
        }

        public bool IsGetLinkIconEnabled()
        {
            return GetLinkIcon.Enabled;
        }

        public bool IsSocialNetworkSharingGoToHomeButtonNotNull()
        {
            return SocialNetworkSharingGoToHomeButton != null;
        }

        public bool IsSocialNetworkSharingReturnToTemplateButtonNotNull()
        {
            return SocialNetworkSharingReturnToTemplateButton != null;
        }

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

        public bool IsLocalPrinterAsCurrentOption()
        {
            ScrollToView(PrintMethodDropdown);

            return new SelectElement(PrintMethodDropdown).SelectedOption.GetAttribute("value") == "Output";
        }

        public void ClickToShareToFacebookIcon()
        {
            ShareToFacebookIcon.Click();
        }

        public void ClickToShareToTwitterIcon()
        {
            ShareToTwitterIcon.Click();
        }

        public void ClickToGetLinkIcon()
        {
            GetLinkIcon.Click();
        }

        public void ClickToGetLinkCopyToClipboardButton()
        {
            GetLinkCopyToClipboardButton.Click();
        }

        public void ClickToSocialNetworkSharingGoToHomeButton()
        {
            SocialNetworkSharingGoToHomeButton.Click();
        }

        public void ClickToNextStep()
        {
            var element = BrowserWait().Until(ExpectedConditions.ElementIsVisible(By.XPath("//a[contains(@id, '_StepNextN1_TheLabelButton')]")));
            ScrollToView(element);
            element.Click();
            BrowserWait(30).Until(ExpectedConditions.StalenessOf(element));
        }

        public void ClickToViewDesignOptions(bool clickToOpen = true)
        {
            ScrollToView(ViewDesignOptionLabel);

            if (clickToOpen && !GetDesignOptionContent().Displayed)
            {
                ViewDesignOptionLabel.Click();
                BrowserWait().Until(x => GetDesignOptionContent().Displayed);
            }
            else if (!clickToOpen && GetDesignOptionContent().Displayed)
            {
                ViewDesignOptionLabel.Click();
                BrowserWait().Until(x => !GetDesignOptionContent().Displayed);
            }
        }

        public void ClickToViewTextOptions(bool clickToOpen = true)
        {
            ScrollToView(ViewTextOptionLabel);
            if (clickToOpen && !GetTextOptionContent().Displayed)
            {
                ViewTextOptionLabel.Click();
                //var actions = new Actions(_browser);
                //actions.MoveToElement(GetTextOptionContent()).Perform();

                BrowserWait().Until(x => GetTextOptionContent().Displayed);

                //ScrollToView(GetTextOptionContent());
            }
            else if (!clickToOpen && GetTextOptionContent().Displayed)
            {
                ViewTextOptionLabel.Click();
                //var actions = new Actions(_browser);
                //actions.MoveToElement(GetTextOptionContent()).Perform();
                BrowserWait().Until(x => !GetTextOptionContent().Displayed);
            }
        }

        public void ClickToViewFooterOptions(bool clickToOpen = true)
        {
            ScrollToView(ViewFooterOptionLabel);
            if (clickToOpen && !GetFooterOptionContent().Displayed)
            {
                ViewFooterOptionLabel.Click();

                BrowserWait().Until(x => GetFooterOptionContent().Displayed);
            }
            else if (!clickToOpen && GetFooterOptionContent().Displayed)
            {
                ViewFooterOptionLabel.Click();

                BrowserWait().Until(x => !GetFooterOptionContent().Displayed);
            }
        }

        public void ClickToViewImageOptions(bool clickToOpen = true)
        {
            ScrollToView(ViewImageOptionLabel);
            if (clickToOpen && !GetImageOptionContent().Displayed)
            {
                ViewImageOptionLabel.Click();
                BrowserWait().Until(x => GetImageOptionContent().Displayed);
            }
            else if (!clickToOpen && GetImageOptionContent().Displayed)
            {
                ViewImageOptionLabel.Click();
                BrowserWait().Until(x => !GetImageOptionContent().Displayed);
            }
        }

        public void ClickToUploadDesignOptionFile()
        {
            ScrollToView(UploadLogoButton);
            UploadLogoButton.Click();
        }

        public void ClickToUploadFirstImageOptionFile(string path)
        {
            ScrollToView(UploadFirstImageButton);
            UploadFirstImageButton.Click();

            InputFile.SendKeys(path);
            ClickToSubmitFileUpload();

            if (IsImageEditorDisplayed())
            {
                EditImage();
            }
        }

        public void ClickToUploadSecondImageOptionFile(string path)
        {
            ScrollToView(UploadSecondImageButton);
            UploadSecondImageButton.Click();

            InputFile.SendKeys(path);
            ClickToSubmitFileUpload();

            if (IsImageEditorDisplayed())
            {
                EditImage();
            }
        }

        public void ClickToSubmitFileUpload()
        {
            var element = UploadFileSubmitLink;
            element.Click();
            BrowserWait().Until(ExpectedConditions.StalenessOf(element));
        }

        public void ClickToUploadFirstSupportingLogo(string path)
        {
            ScrollToView(FirstSupportingLogoUploadButton);
            FirstSupportingLogoUploadButton.Click();

            InputFile.SendKeys(path);
            ClickToSubmitFileUpload();

            if (IsImageEditorDisplayed())
            {
                EditImage();
            }
        }

        public void ClickToUploadSecondSupportingLogo(string path)
        {
            ScrollToView(SecondSupportingLogoUploadButton);
            SecondSupportingLogoUploadButton.Click();

            InputFile.SendKeys(path);
            ClickToSubmitFileUpload();

            if (IsImageEditorDisplayed())
            {
                EditImage();
            }
        }

        public void UploadDesignOptionFile(string path)
        {
            ClickToUploadDesignOptionFile();
            InputFile.SendKeys(path);
            ClickToSubmitFileUpload();

            if (IsImageEditorDisplayed())
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

        public void ClickToSelectLocalPrinter()
        {
            ScrollToView(PrintMethodDropdown);

            new SelectElement(PrintMethodDropdown).SelectByValue("Output");
        }

        public void ClickToSelectHomePrinter()
        {
            ScrollToView(PrintMethodDropdown);

            new SelectElement(PrintMethodDropdown).SelectByValue("OutputDownload");
        }

        public string GetGetLinkCopyStatusText()
        {
            return GetLinkCopyStatus.Text;
        }

        public string GetSocialNetworkSharingPopupMessageText()
        {
            return SocialNetworkSharingPopupMessage?.Text;
        }

        public string GetUploadLogoValue()
        {
            ScrollToView(UploadLogoTextbox);
            return UploadLogoTextbox.GetAttribute("value");
        }

        public string GetFirstUploadImageValue()
        {
            ScrollToView(FirstImageTextbox);
            return FirstImageTextbox.GetAttribute("value");
        }

        public string GetSecondUploadImageValue()
        {
            ScrollToView(SecondImageTextbox);
            return SecondImageTextbox.GetAttribute("value");
        }

        public string GetFirstSupportingLogoValue()
        {
            ScrollToView(FirstSupportingLogoTextbox);
            return FirstSupportingLogoTextbox.GetAttribute("value");
        }

        public string GetSecondSupportingLogoValue()
        {
            ScrollToView(SecondSupportingLogoTextbox);
            return SecondSupportingLogoTextbox.GetAttribute("value");
        }

        public string GetDocumentId()
        {
            var element = Browser.FindElement(By.Id("btnProofInternal"));

            return new string(element.GetAttribute("href").Where(Char.IsDigit).ToArray());
        }

        public List<IWebElement> GetAllDisplayedTextareas()
        {
            return BrowserWait().Until<List<IWebElement>>((d) =>
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
                BrowserWait().Until(ExpectedConditions.ElementIsVisible(By.Id("ctl00_ctl00_ContentPlaceHolderBody_StepArea1_ContentPlaceHolderStepArea_InputFields_InputFields")));

                var fields =  BrowserWait(5).Until<List<IWebElement>>(d =>
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

        public List<string> GetValueOfAllDisplayedTextareas()
        {
            var values = new List<string>();
            var elements = Browser.FindElements(By.XPath("//div[contains(@id, '_ContentPlaceHolderStepArea_InputFields_InputFields')]//textarea"));
            foreach (var item in elements)
            {
                ScrollToView(item);
                values.Add(item.GetAttribute("value"));
            }

            return values;
        }

        public string[] GetFooterDropdownValues()
        {
            var ddl = new SelectElement(FooterDropdown);
            return ddl.Options.Select(x => x.GetAttribute("value")).ToArray();
        }

        public void FillTextFirstInput()
        {
            var element = BrowserWait(2).Until(ExpectedConditions.ElementIsVisible(By.XPath("//div[contains(@id, '_ContentPlaceHolderStepArea_InputFields_InputFields')]//p//input")));

            ScrollToView(element);
            var textValue = TestUtil.RandomName(10);
            element.Clear();
            element.SendKeys(textValue);
        }

        private void EditImage()
        {
            var imageEditorButton = BrowserWait().Until(ExpectedConditions.ElementIsVisible(
                By.XPath("//div[contains(@id, '_ContentPlaceHolderStepArea__UserImageEdit1__ModalPopUp1_ButtonSave')]")));
            imageEditorButton.Click();
            BrowserWait(30).Until(ExpectedConditions.StalenessOf(imageEditorButton));
        }

        private bool IsImageEditorDisplayed()
        {
            try
            {
                return BrowserWait().Until(ExpectedConditions.ElementIsVisible(
                           By.XPath("//table[contains(@id, '_ContentPlaceHolderStepArea__UserImageEdit1__ModalPopUp1_tablePopUpArea')]"))) != null;
            }
            catch
            {
                return false;
            }
        }

        public List<IWebElement> GetDesignOptionLayoutRadios()
        {
            return BrowserWait(20).Until(d =>
            {
                var elements = d.FindElements(By.Id("FIELD_51484")).ToList();
                return elements.Count > 0 ? elements.ToList() : null;
            });
        }

        private IWebElement GetDesignOptionContent()
        {
            return BrowserWait().Until(ExpectedConditions.ElementExists(By.Id("div1")));
        }

        private IWebElement GetTextOptionContent()
        {
            return BrowserWait().Until(ExpectedConditions.ElementExists(By.Id("div2")));
        }

        private IWebElement GetFooterOptionContent()
        {
            return BrowserWait().Until(ExpectedConditions.ElementExists(By.Id("div3")));
        }

        private IWebElement GetImageOptionContent()
        {
            return BrowserWait().Until(ExpectedConditions.ElementExists(By.Id("div4")));
        }

        public void SetSelectDesignOptionLayout(List<IWebElement> radiobuttons, int index)
        {
            var selectElement = radiobuttons[index];
            ScrollToView(selectElement);
            selectElement.Click();
        }

        public bool CheckSelectDesignOptionLayout(List<IWebElement> radiobuttons, int index)
        {
            var selectElement = radiobuttons[index];
            ScrollToView(selectElement);

            return selectElement.Selected;
        }
    }
}
