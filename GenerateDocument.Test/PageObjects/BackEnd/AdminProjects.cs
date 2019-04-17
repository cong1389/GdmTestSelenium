using GenerateDocument.Common.Extensions;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;

using static GenerateDocument.Test.Utilities.PageCommon;

namespace GenerateDocument.Test.PageObjects.BackEnd
{
    public class AdminProjects : PageObject
    {
        public AdminProjects(IWebDriver browser) : base(browser)
        {
        }

        [FindsBy(How = How.ClassName, Using = "tip")]
        private IWebElement TipTitle { set; get; }

        [FindsBy(How = How.CssSelector, Using = "#AdminMaster_ContentPlaceHolderBody_buttonBar_btnAddNew_spnActive .adminButtonLink")]
        private IWebElement UploadButton { set; get; }

        [FindsBy(How = How.Id, Using = "mngdButtonUpload")]
        private IWebElement UploadBrowserButton { set; get; }

        [FindsBy(How = How.ClassName, Using = "LinkText")]
        private IWebElement LinkTextUploadContent { set; get; }

        public string GetTipTitleText()
        {
            return TipTitle.Text;
        }

        public string GetUploadButtonText()
        {
            return UploadButton.Text;
        }

        public void ClickToUploadButton()
        {
            UploadButton.Click();
        }

        public bool IsUploadBrowserButtonDisplayed()
        {
            return UploadBrowserButton.Displayed;
        }

        public string GetTextOfLinkTextUploadContent()
        {
            return LinkTextUploadContent.Text;
        }

        public void ClickToLinkTextUploadContent()
        {
            LinkTextUploadContent.Click();
        }

        public void Open()
        {
            Browser.NavigateTo(AdminProjectsPage);
        }
    }
}
