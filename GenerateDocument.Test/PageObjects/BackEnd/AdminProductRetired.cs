using System.Linq;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using System.Collections.Generic;

namespace GenerateDocument.Test.PageObjects.BackEnd
{
    public class AdminProductRetired : PageObject
    {
        public AdminProductRetired(IWebDriver browser) : base(browser)
        {
        }

        [FindsBy(How = How.Id, Using = "AdminMaster_ContentPlaceHolderBody_btnUpdate")]
        private IWebElement UpdateSettingButton { get; set; }

        [FindsBy(How = How.ClassName, Using = "tip_noindent")]
        private IList<IWebElement> TipNoIndents { get; set; }

        public void ClickToUpdateSetting()
        {
            UpdateSettingButton.Click();
        }

        public string[] GetTipNoIndentsTexts()
        {
            return TipNoIndents.Select(x => x.Text).ToArray();
        }
    }
}
