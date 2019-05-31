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
        private string _id;

        private readonly ElementLocator
            _modalOverlayLocator = new ElementLocator(Locator.XPath, "//div[contains(@class,'modal-overlay-fallback')]"),
            _acceptScropedLocator = new ElementLocator(Locator.XPath, "//div[contains(@class,'modal-overlay-fallback')]//button[text()='OK']"),
            _uploadBtnLocator = new ElementLocator(Locator.XPath, "//div[@id='{0}']//a//span[text()='Upload']"),
            _clearBtnLocator = new ElementLocator(Locator.XPath, "//div[@id='{0}']//a[text()='Clear']"),
            _resetBtnLocator = new ElementLocator(Locator.XPath, "//div[@id='{0}']//a[text()='Reset']"),
            _modalLarge = new ElementLocator(Locator.XPath, "//div[@class='modal modal--large']"),
            _chooseImageLocator = new ElementLocator(Locator.XPath, "//div[contains(@id, 'html5_')]//input[@type='file']"),
            _chooseNewImageLocator = new ElementLocator(Locator.XPath, "//button[@id='il_re_cropper_upload_btn-library']"),
            _submitUploadBtnLocator = new ElementLocator(Locator.Id, "il_upload_btn-upload");

        public ImageUpload(IWebElement webElement) : base(webElement.ToDriver() as RemoteWebDriver, null)
        {
            _webElement = webElement;
            _id = _webElement.GetAttribute("id");
        }

        private IWebDriver Driver => _webElement.ToDriver();

        public void Upload(string imagePath)
        {
            //Open modal upload image
            var buttonEle = Driver.GetElement(_uploadBtnLocator.Format(_id));
            Driver.ScrollToView(buttonEle);
            buttonEle.Click();

            //waiting modal overlay to hidden
            Driver.WaitUntilElementIsNoLongerFound(_modalOverlayLocator, BaseConfiguration.ShortTimeout);

            //Check display upload new image button
          
            var isDisplayNewImageBtn = Driver.IsElementPresent(_chooseNewImageLocator, BaseConfiguration.LongTimeout);
            if (isDisplayNewImageBtn)
            {
                Driver.GetElement<Button>(_chooseNewImageLocator).ClickTo();
            }

            //choose file
            var ele = Driver.GetElement(_chooseImageLocator, e => e.Enabled);
            ele.SendKeys(imagePath);

            //Click submit upload
            Driver.GetElement(_submitUploadBtnLocator).Click();

            //Scroping image
            var isAccepScrop = Driver.IsElementPresent(_acceptScropedLocator, BaseConfiguration.ShortTimeout);
            if (isAccepScrop)
            {
                Driver.GetElement(_acceptScropedLocator).Click();
            }

            //Waiting modal close
            Driver.WaitUntilElementIsNoLongerFound(_modalLarge, BaseConfiguration.LongTimeout);
        }

        public string GetImageName()
        {
            Driver.ScrollToView(_webElement);
            return _webElement.GetAttribute("value");
        }

        public void ClearImage()
        {
            var buttonEle = Driver.GetElement(_clearBtnLocator.Format(_id));
            Driver.ScrollToView(buttonEle);
            buttonEle.Click();
        }

        public void ResetImage()
        {
            var buttonEle = Driver.GetElement(_resetBtnLocator.Format(_id));
            Driver.ScrollToView(buttonEle);
            buttonEle.Click();
        }
    }
}
