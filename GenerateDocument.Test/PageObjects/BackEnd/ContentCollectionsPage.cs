using GenerateDocument.Common.Extensions;
using GenerateDocument.Test.Extensions;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;

using static GenerateDocument.Test.Utilities.PageCommon;

namespace GenerateDocument.Test.PageObjects.BackEnd
{
    public class ContentCollectionsPage : PageObject
    {
        public ContentCollectionsPage(IWebDriver browser) : base(browser)
        {
        }

        [FindsBy(How = How.ClassName, Using = "AdminOptFldImageListDiv")]
        private IWebElement AdminOptFldImageListDiv { get; set; }

        public string GetAdminOptFldImageListDivText()
        {
            return AdminOptFldImageListDiv.Text;
        }

        public void Open()
        {
            Browser.NavigateTo(AdminContentCollectionsPage);
        }
    }
}
