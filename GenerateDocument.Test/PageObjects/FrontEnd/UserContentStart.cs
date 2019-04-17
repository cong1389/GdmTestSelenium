using GenerateDocument.Common.Extensions;
using GenerateDocument.Test.Utilities;
using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.PageObjects;
using System.Linq;
using static GenerateDocument.Test.Utilities.PageCommon;

namespace GenerateDocument.Test.PageObjects.FrontEnd
{
    public class UserContentStart : PageObject
    {
        public UserContentStart(IWebDriver browser) : base(browser)
        {
        }

        [FindsBy(How = How.LinkText, Using = "Advanced Flyer")]
        private IWebElement AdvanceFlyerLink { get; set; }

        [FindsBy(How = How.Id, Using = "txtSearch")]
        private IWebElement SearchBox { get; set; }

        [FindsBy(How = How.Id, Using = "btnSearch")]
        private IWebElement SearchButton { get; set; }

        [FindsBy(How = How.Id, Using = "il_autosavestartdoc_title")]
        private IWebElement AutoSaveModalTitle { get; set; }

        [FindsBy(How = How.Id, Using = "il_autosavestartdoc_btn-ok")]
        private IWebElement AutoSaveModalCompleteExistingDocumentButton { get; set; }

        [FindsBy(How = How.Id, Using = "il_autosavestartdoc_btn-startnewdocument")]
        private IWebElement AutoSaveModalStartNewDocumentButton { get; set; }

        [FindsBy(How = How.ClassName, Using = "")]
        private IWebElement AreaTitle { get; set; }

        public ProductInfo SearchDocument(string input)
        {
            var productInfo = new ProductInfo();

            if (string.IsNullOrEmpty(input))
                return productInfo;

            SearchBox.SendKeys(input);
            SearchButton.Click();

            var result = Browser.FindElement(By.XPath($"//div[contains(@class, 'catalogItemFooter')]//a[contains(@title, '{input}')]"));
            string href = result?.GetAttribute("href");

            int hash = href.LastIndexOf('=') + 1;
            string productId = href.Substring(hash, href.Length - hash);

            productInfo.Id = productId;
            productInfo.Name = result?.Text;

            return productInfo;
        }

        public void ClickToCreateDesign(string productName)
        {
            var element = Browser.FindElement(By.LinkText(productName));
            var actions = new Actions(Browser);
            actions.MoveToElement(element);
            element.Click();
        }

        public void SelectDocument(string id)
        {
            var document = Browser.FindElement(By.CssSelector($"[href*='CreateUserDocument.aspx?product={id}']"));

            document.Click();
        }

        public bool IsInprogressDocument()
        {
            try
            {
                return AutoSaveModalTitle.Displayed;
            }
            catch
            {
                return false;
            }
        }

        public void SelectCompleteExistingDocument()
        {
            AutoSaveModalCompleteExistingDocumentButton.Click();
        }

        public void AgreeWithPrivacy()
        {
            var element = Browser.FindElements(By.ClassName("gdpr-consent-modal"));
            if (element.Any())
            {
                Browser.FindElement(By.Id("gdprConsentAcknowledge")).Click();
                Browser.FindElement(By.Id("gdprConsentContinue")).Click();
            }
        }

        public void NavigateTo()
        {
            Browser.NavigateTo(UserContentStartPage);
        }
    }
}
