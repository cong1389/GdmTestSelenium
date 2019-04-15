using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using OpenQA.Selenium.Support.UI;

namespace GenerateDocument.Test.PageObjects.BackEnd
{
    public class AdminProductDetails : PageObject
    {
        public AdminProductDetails(IWebDriver browser) : base(browser)
        {
        }

        [FindsBy(How = How.Id, Using = "AdminMaster_ContentPlaceHolderBody_btnUpdate")]
        private IWebElement UpdateButton { get; set; }

        [FindsBy(How = How.Id, Using = "AdminMaster_ContentPlaceHolderBody_ckAutoSubmit")]
        private IWebElement SubmitJobCheckBox { get; set; }

        [FindsBy(How = How.Id, Using = "AdminMaster_ContentPlaceHolderBody_radShipOnly")]
        private IWebElement DownloadRadio1 { get; set; }

        [FindsBy(How = How.Id, Using = "AdminMaster_ContentPlaceHolderBody_radDownloadAndShip")]
        private IWebElement DownloadRadio2 { get; set; }

        [FindsBy(How = How.Id, Using = "AdminMaster_ContentPlaceHolderBody_radDownloadOnly")]
        private IWebElement DownloadRadio3 { get; set; }

        [FindsBy(How = How.Id, Using = "AdminMaster_ContentPlaceHolderBody_ddPriceTable")]
        private IWebElement PriceTableValue { get; set; }

        [FindsBy(How = How.Id, Using = "AdminMaster_ContentPlaceHolderBody_btnChangeSettings")]
        private IWebElement ChangeSettingButton { get; set; }

        [FindsBy(How = How.Id, Using = "AdminMaster_ContentPlaceHolderBody_btnRelease")]
        private IWebElement ReleaseButton { get; set; }

        [FindsBy(How = How.XPath, Using = "//div[contains(@class, 'steptype-FormFill')]//a")]
        private IWebElement FormFillingLink { get; set; }

        public void SelectSubmitJobCheckBox()
        {
            if (!SubmitJobCheckBox.Selected)
            {
                SubmitJobCheckBox.Click();
            }
        }

        public void UnSelectSubmitJobCheckBox()
        {
            if (SubmitJobCheckBox.Selected)
            {
                SubmitJobCheckBox.Click();
            }
        }

        public void ClickDownloadRadio1()
        {
            if (!DownloadRadio1.Selected)
            {
                DownloadRadio1.Click();
            }
        }

        public void ClickDownloadRadio2()
        {
            if (!DownloadRadio2.Selected)
            {
                DownloadRadio2.Click();
            }
        }

        public void ClickDownloadRadio3()
        {
            if (!DownloadRadio3.Selected)
            {
                DownloadRadio3.Click();
            }
        }

        public void SettingPriceNone()
        {
            var selector = new SelectElement(PriceTableValue);
            selector.SelectByText("(None)");
        }

        public void SettingPriceZero()
        {
            var selector = new SelectElement(PriceTableValue);
            selector.SelectByText("Price Zero");
        }

        public void SettingPriceGreaterThanZero()
        {
            var selector = new SelectElement(PriceTableValue);
            selector.SelectByText("Flyer");
        }

        public void ClickToChangeSetting()
        {
            ChangeSettingButton.Click();
        }

        public void ClickToRelease()
        {
            ReleaseButton.Click();
        }

        public void UpdateForMaintainOnly()
        {
            var element = BrowserWait()
                .Until(ExpectedConditions.ElementToBeClickable(By.Id("AdminMaster_ContentPlaceHolderBody_radLegacy")));
            if (!element.Selected)
            {
                element.Click();
            }
        }

        public void UpdateAndConvertToLatestRelease()
        {
            var element = BrowserWait()
                .Until(ExpectedConditions.ElementToBeClickable(By.Id("AdminMaster_ContentPlaceHolderBody_radConvert")));

            if (!element.Selected)
            {
                element.Click();
            }
        }

        public void ClickToFormFilling()
        {
            ScrollToView(FormFillingLink);

            FormFillingLink.Click();
        }

        public void UpdateSettings(bool ifLastStep = false)
        {
            UpdateButton.Click();

            if (ifLastStep)
            {
                BrowserWait()
                    .Until(ExpectedConditions.ElementToBeClickable(
                        By.Id("AdminMaster_ContentPlaceHolderBody_btnDone")));
            }
        }
    }
}
