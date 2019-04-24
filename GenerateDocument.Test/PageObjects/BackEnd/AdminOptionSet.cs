using OpenQA.Selenium;

namespace GenerateDocument.Test.PageObjects.BackEnd
{
    public class AdminOptionSet : PageObject
    {
        public AdminOptionSet(IWebDriver browser) : base(browser)
        {
        }

        public void ClickToFooterLink()
        {
            var footerLink = Browser.FindElement(By.Id("AdminMaster_ContentPlaceHolderBody__XF_DND_OptionSet1_grid_ctl31_hlEditField"));
            ScrollToView(footerLink);
            footerLink.Click();
        }

        public void ClickToGoback()
        {
            var gobackLink = Browser.FindElement(By.Id("AdminMaster_ContentPlaceHolderBody_GoBack"));
            ScrollToView(gobackLink);
            gobackLink.Click();
        }
    }
}
