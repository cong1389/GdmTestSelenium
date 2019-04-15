using GenerateDocument.Common.Extensions;
using GenerateDocument.Test.Extensions;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using OpenQA.Selenium.Support.UI;

using static GenerateDocument.Test.Utilities.PageCommon;

namespace GenerateDocument.Test.PageObjects.BackEnd
{
    public class AdminProductContent : PageObject
    {
        public AdminProductContent(IWebDriver browser) : base(browser)
        {
        }

        [FindsBy(How = How.Id, Using = "AdminMaster_ContentPlaceHolderBody_ddlProductType")]
        private IWebElement SelectBoxProductType { get; set; }

        [FindsBy(How = How.Id, Using = "AdminMaster_ContentPlaceHolderBody_labProductType")]
        private IWebElement LabelProductType { get; set; }

        public void SelectProductType(string type)
        {
            var select = new SelectElement(SelectBoxProductType);
            select.SelectByText(type);
        }

        public string GetLabelProductTypeText()
        {
            return LabelProductType.Text;
        }

        public void Open()
        {
            Browser.NavigateTo(AdminProductContentPage);
        }
    }
}
