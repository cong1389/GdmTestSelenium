using GenerateDocument.Common;
using GenerateDocument.Common.Extensions;
using GenerateDocument.Common.Types;
using GenerateDocument.Common.WebElements;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using OpenQA.Selenium.Support.UI;

namespace GenerateDocument.Test.PageObjects.BackEnd
{
    public class AdminProductDetails : PageBaseObject
    {
        private readonly ElementLocator
            _changeSettingButtonLocator = new ElementLocator(Locator.Id, "AdminMaster_ContentPlaceHolderBody_btnChangeSettings"),
            _submitJobCheckBoxLocator = new ElementLocator(Locator.Id, "AdminMaster_ContentPlaceHolderBody_ckAutoSubmit"),
            _downloadRadio3Locator = new ElementLocator(Locator.Id, "AdminMaster_ContentPlaceHolderBody_radDownloadOnly"),
            _priceTableValueLocator = new ElementLocator(Locator.Id, "AdminMaster_ContentPlaceHolderBody_ddPriceTable"),
            _releaseButtonLocator = new ElementLocator(Locator.Id, "AdminMaster_ContentPlaceHolderBody_btnRelease"),
            _formFillingLinkLocator = new ElementLocator(Locator.XPath, "//div[contains(@class, 'steptype-FormFill')]//a"),
            _updateButtonLocator = new ElementLocator(Locator.Id, "AdminMaster_ContentPlaceHolderBody_btnUpdate");

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
            var lnk = Driver.GetElement(_formFillingLinkLocator);

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
    }
}
