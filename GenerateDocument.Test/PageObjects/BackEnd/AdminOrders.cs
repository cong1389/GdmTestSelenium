using GenerateDocument.Common.Extensions;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;

using static GenerateDocument.Test.Utilities.PageCommon;

namespace GenerateDocument.Test.PageObjects.BackEnd
{
    public class AdminOrders : PageObject
    {
        public AdminOrders(IWebDriver browser) : base(browser)
        {
        }

        [FindsBy(How = How.Id, Using = "AdminMaster_Title")]
        private IWebElement Title { set; get; }

        public string GetTitleText()
        {
            return Title.Text;
        }

        public void Open()
        {
            Browser.NavigateTo(AdminOrdersPage);
        }
    }
}
