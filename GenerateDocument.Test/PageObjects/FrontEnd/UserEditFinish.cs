using System;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.Extensions;
using OpenQA.Selenium.Support.PageObjects;
using OpenQA.Selenium.Support.UI;

namespace GenerateDocument.Test.PageObjects.FrontEnd
{
    public class UserEditFinish : PageObject
    {
        public UserEditFinish(IWebDriver browser) : base(browser)
        {
        }

        private IWebElement Id => BrowserWait().Until(ExpectedConditions.ElementIsVisible(By.CssSelector("#ctl00_ctl00_ContentPlaceHolderBody_StepArea1_ContentPlaceHolderStepArea_DocID_text>span")));

        private IWebElement AddToDraftButton => BrowserWait().Until(ExpectedConditions.ElementIsVisible(By.Id("ctl00_ctl00_ContentPlaceHolderBody_StepArea1_ContentPlaceHolderStepArea__UserAddToCartButtons2_btnSave")));

        private IWebElement AddToBasketButton => BrowserWait().Until(ExpectedConditions.ElementIsVisible(By.Id("ctl00_ctl00_ContentPlaceHolderBody_StepArea1_ContentPlaceHolderStepArea__UserAddToCartButtons2_btnSave")));

        private IWebElement SaveBasketButton => BrowserWait().Until(ExpectedConditions.ElementIsVisible(By.Id("ctl00_ctl00_ContentPlaceHolderBody_StepArea1_ContentPlaceHolderStepArea__UserAddToCartButtons2_btnSaveBasket")));

        private IWebElement GenerateDocumentButton => BrowserWait().Until(ExpectedConditions.ElementToBeClickable(By.Id("ctl00_ctl00_ContentPlaceHolderBody_StepArea1_ContentPlaceHolderStepArea__UserAddToCartButtons2_GoToDesignsButton")));

        private IWebElement SaveDraftButton => BrowserWait().Until(ExpectedConditions.ElementIsVisible(By.Id("ctl00_ctl00_ContentPlaceHolderBody_StepArea1_ContentPlaceHolderStepArea__UserAddToCartButtons2_btnSaveDraft")));

        private IWebElement OrderNameTextBox => BrowserWait().Until(ExpectedConditions.ElementIsVisible(By.Id("ctl00_ctl00_ContentPlaceHolderBody_StepArea1_ContentPlaceHolderStepArea_DocDescription_box")));

        private IWebElement WarningExistsDocument => BrowserWait().Until(ExpectedConditions.ElementIsVisible(By.Id("descriptionErrorMessage")));
        
        public void ClickToCreateDraft()
        {
            ScrollToView(AddToDraftButton);
            AddToDraftButton.Click();
            BrowserWait(30).Until(ExpectedConditions.StalenessOf(AddToDraftButton));
        }

        public void ClickToCreateBasket()
        {
            ScrollToView(AddToBasketButton);
            AddToBasketButton.Click();
        }

        public void ClickToSaveDraft()
        {
            ScrollToView(SaveDraftButton);
            SaveDraftButton.Click();
        }

        public void ClickToSaveBasket()
        {
            ScrollToView(SaveBasketButton);
            SaveBasketButton.Click();
        }

        public void ClickToGenerateDocument()
        {
            ScrollToView(GenerateDocumentButton);
            GenerateDocumentButton.Click();

            BrowserWait().Until(ExpectedConditions.UrlContains("onedesign"));
        }

        public void ClickToGenerateDocument(string name)
        {
            EnterOrderName(name);
            ScrollToView(GenerateDocumentButton);
            GenerateDocumentButton.Click();

            BrowserWait().Until(ExpectedConditions.UrlContains("onedesign"));
        }

        public void EnterOrderName(string name)
        {
            OrderNameTextBox.Clear();
            OrderNameTextBox.SendKeys(name);
            OrderNameTextBox.SendKeys(Keys.Tab);
        }

        public string GetCurrentOrderName()
        {
            return OrderNameTextBox.GetAttribute("value");
        }

        public string GetWarningExistsDocumentMessage()
        {
            return WarningExistsDocument.Text;
        }

        public string GetDocumentId()
        {
            return Id.Text;
        }

        public string GetNameButtonAddToBasket()
        {
            return AddToBasketButton.Text;
        }

        public string GetErrorMessage()
        {
            return BrowserWait().Until(ExpectedConditions.ElementIsVisible(By.Id("descriptionErrorMessage"))).Text;
        }
    }
}
