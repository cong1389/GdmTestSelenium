using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
