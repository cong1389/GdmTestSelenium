using GenerateDocument.Common.Extensions;
using GenerateDocument.Test.Utilities;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using OpenQA.Selenium.Support.UI;

namespace GenerateDocument.Test.PageObjects.BackEnd
{
    public class AdminExtensions : PageObject
    {
        [FindsBy(How = How.Id, Using = "AdminMaster_TitleComment")]
        private IWebElement AdminMasterTitleComment { get; set; }

        private IWebElement AutosaveSwitchDropdown => BrowserWait(5).Until(ExpectedConditions.ElementIsVisible(By.Name("GDAutosaveSwitch")));

        [FindsBy(How = How.Id, Using = "AdminMaster_ContentPlaceHolderBody_btnUpdate")]
        private IWebElement UpdateButton { get; set; }

        public AdminExtensions(IWebDriver browser) : base(browser)
        {
        }

        public string GetTitle()
        {
            BrowserWait().Until(ExpectedConditions.ElementIsVisible(By.Id("AdminMaster_TitleComment")));
            var element = Browser.FindElement(By.Id("AdminMaster_TitleComment"));
            return element.Text;
        }

        public void Open()
        {
            WebDriverExtensions.NavigateTo(Browser, PageCommon.AdminExtensionsPage);
        }

        public void ClickGeneratedDocumentManagerLink()
        {
            var document = Browser.FindElement(By.LinkText("Generated Document Manager"));
            document.Click();
        }

        public void SelectAutoSaveSwitch(bool select = true)
        {
            ScrollToView(AutosaveSwitchDropdown);
            new SelectElement(AutosaveSwitchDropdown).SelectByValue(select ? "True" : "False");
        }

        public void ClickUpdateSettings()
        {
            UpdateButton.Click();
        }
    }
}
