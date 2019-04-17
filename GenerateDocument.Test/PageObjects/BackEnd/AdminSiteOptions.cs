using GenerateDocument.Common.Extensions;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;

using static GenerateDocument.Test.Utilities.PageCommon;

namespace GenerateDocument.Test.PageObjects.BackEnd
{
    public class AdminSiteOptions : PageObject
    {
        public AdminSiteOptions(IWebDriver browser) : base(browser)
        {
        }

        public void Open()
        {
            Browser.NavigateTo(AdminSiteOptionsPage);
        }

        [FindsBy(How = How.Id, Using = "AdminMaster_Title")]
        private IWebElement Title { set; get; }

        [FindsBy(How = How.Id, Using = "AdminMaster_RepeaterTabs_ctl06_LinkButtonTab")]
        private IWebElement OrdersTab { set; get; }

        [FindsBy(How = How.Id, Using = "AdminMaster_RepeaterTabs_ctl07_LinkButtonTab")]
        private IWebElement SecurityTab { set; get; }

        [FindsBy(How = How.Id, Using = "AdminMaster_ContentPlaceHolderBody_btnChange")]
        private IWebElement ChangeSettingButton { set; get; }

        public string GetTitleText()
        {
            return Title.Text;
        }

        public void ClickToOrdersTab()
        {
            OrdersTab.Click();
        }

        public void ClickToSecurityTab()
        {
            SecurityTab.Click();
        }

        public void SaveSetting()
        {
            ChangeSettingButton.Click();
        }

        public bool IsChangeSettingButtonEnabled()
        {
            return ChangeSettingButton.Enabled;
        }
    }
}
