using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GenerateDocument.Common.Extensions;
using GenerateDocument.Common.Types;
using OpenQA.Selenium;
using OpenQA.Selenium.Remote;

namespace GenerateDocument.Common.WebElements
{
    public class ImageUpload : RemoteWebElement
    {
        private readonly IWebElement _webElement;

        private readonly ElementLocator
            _modalLarge = new ElementLocator(Locator.XPath, "//div[@class='modal modal--large']"),
            _chooseImageLocator = new ElementLocator(Locator.XPath, "//div[contains(@id, 'html5_')]//input[@type='file']"),
            _submitUploadBtnLocator = new ElementLocator(Locator.Id, "il_upload_btn-upload");

        public ImageUpload(IWebElement webElement) : base(webElement.ToDriver() as RemoteWebDriver, null)
        {
            _webElement = webElement;
        }

        private IWebDriver Driver
        {
            get { return _webElement.ToDriver(); }
        }

        public void Upload(string imagePath)
        {
            //Open modal upload image
            Driver.ScrollToView(_webElement);
            _webElement.Click();

            //choose file
            var ele = Driver.GetElement(_chooseImageLocator, e => e.Enabled);
            ele.SendKeys(imagePath);

            //Click submit upload
            Driver.GetElement(_submitUploadBtnLocator).Click();

            //Waiting closed modal
            Driver.WaitUntilElementIsNoLongerFound(_modalLarge, BaseConfiguration.LongTimeout);
        }

        public string GetImageName()
        {
            Driver.ScrollToView(_webElement);
            return _webElement.GetAttribute("value");
        }
    }
}
