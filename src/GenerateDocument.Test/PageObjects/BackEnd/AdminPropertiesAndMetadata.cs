using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;

namespace GenerateDocument.Test.PageObjects.BackEnd
{
    public class AdminPropertiesAndMetadata : PageObject
    {
        public AdminPropertiesAndMetadata(IWebDriver browser) : base(browser)
        {
        }

        [FindsBy(How = How.Id, Using = "AdminMaster_ContentPlaceHolderBody_hlMetadata")]
        private IWebElement MetaDataLink { get; set; }

        [FindsBy(How = How.Id, Using = "FIELD_51481")]
        private IWebElement ShareFB { get; set; }

        [FindsBy(How = How.Id, Using = "FIELD_51482")]
        private IWebElement ShareTwitter { get; set; }

        [FindsBy(How = How.Id, Using = "FIELD_51483")]
        private IWebElement ShareLink { get; set; }

        [FindsBy(How = How.Id, Using = "AdminMaster_ContentPlaceHolderBody_btnUpdate2")]
        private IWebElement UpdateButton { get; set; }

        public void EnableToSocialNetworkSharing()
        {
            if (!ShareFB.Selected)
            {
                ShareFB.Click();
            }
            if (!ShareTwitter.Selected)
            {
                ShareTwitter.Click();
            }
            if (!ShareLink.Selected)
            {
                ShareLink.Click();
            }
        }

        public void DisableToSocialNetworkSharing()
        {
            if (ShareFB.Selected)
            {
                ShareFB.Click();
            }
            if (ShareTwitter.Selected)
            {
                ShareTwitter.Click();
            }
            if (ShareLink.Selected)
            {
                ShareLink.Click();
            }
        }

        public void ClickToMetaDataLink()
        {
            MetaDataLink.Click();
        }

        public void ClickToUpdate()
        {
            UpdateButton.Click();
        }
    }
}
