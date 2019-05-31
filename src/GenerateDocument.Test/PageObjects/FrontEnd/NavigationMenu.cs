using System.Linq;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace GenerateDocument.Test.PageObjects.FrontEnd
{
    public class NavigationMenu : PageObject
    {
        private string xPathIconReminder = "//a[contains(@id,'mnuIconBell_anchor')]";
        private string classNameListProduct = "nav-bell__list-product";

        public NavigationMenu(IWebDriver browser) : base(browser)
        {
        }

        public int CountItemInViewReminder()
        {
            SelectViewReminder();

            var ul = BrowserWait(30).Until(ExpectedConditions.ElementIsVisible(By.ClassName(classNameListProduct)));
            var li = ul.FindElements(By.TagName("li"));

            return li.Any() ? li.Count : 0;
        }

        public void SelectItemInViewReminder(string productName)
        {
            SelectViewReminder();

            var ul = BrowserWait(30).Until(ExpectedConditions.ElementIsVisible(By.ClassName(classNameListProduct)));
            
            var anchors = ul.FindElements(By.TagName("a"));
            var anchor = anchors.FirstOrDefault(x => x.Text.Replace(",","") == productName);
	        if (anchor != null)
	        {
		        string script = anchor.GetAttribute("onclick");

		        var js = (IJavaScriptExecutor)Browser;
		        js.ExecuteScript(script);
	        }
        }

        private void SelectViewReminder()
        {
            var result = BrowserWait(30).Until(ExpectedConditions.ElementIsVisible(By.XPath(xPathIconReminder)));
            result?.Click();
        }
    }
}
