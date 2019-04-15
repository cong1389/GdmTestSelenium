using GenerateDocument.Common;
using GenerateDocument.Common.Extensions;
using GenerateDocument.Common.Types;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;

namespace GenerateDocument.Test.PageObjects.BackEnd
{
    public class AdminLogout : PageBaseObject
    {
        private readonly ElementLocator _logoutLink = new ElementLocator(Locator.XPath, "//a[@id='AdminMaster_AnchorLogout']");

        public AdminLogout(DriverContext driverContext) : base(driverContext)
        {
        }

        public AdminLogout ClickLogOutButton()
        {
            Driver.GetElement(_logoutLink).Click();

            Driver.DeleteAllCookies();

            return this;
        }
    }
}
