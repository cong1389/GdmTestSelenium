using GenerateDocument.Common.Extensions;
using GenerateDocument.Test.Extensions;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;

using static GenerateDocument.Test.Utilities.PageCommon;

namespace GenerateDocument.Test.PageObjects.BackEnd
{
    public class AdminGlobalLibrary : PageObject
    {
        public AdminGlobalLibrary(IWebDriver browser) : base(browser)
        {
        }

        public void Open()
        {
            Browser.NavigateTo(AdminGlobalLibraryPage);
        }

        [FindsBy(How = How.Id, Using = "AdminMaster_Title")]
        private IWebElement Title { set; get; }

        public string GetTitleText()
        {
            return Title.Text;
        }
    }
}
