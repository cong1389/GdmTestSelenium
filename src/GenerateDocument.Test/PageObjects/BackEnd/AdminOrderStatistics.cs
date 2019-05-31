using GenerateDocument.Common.Extensions;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using static GenerateDocument.Test.Utilities.PageCommon;

namespace GenerateDocument.Test.PageObjects.BackEnd
{
    public class AdminOrderStatistics : PageObject
    {
        public AdminOrderStatistics(IWebDriver browser) : base(browser)
        {
        }

        [FindsBy(How = How.ClassName, Using = "titleExtra")]
        private IWebElement Title { set; get; }

        public string GetTitleText()
        {
            return Title.Text;
        }

        public void Open()
        {
            Browser.NavigateTo(AdminOrderStatisticsPage);
        }
    }
}
