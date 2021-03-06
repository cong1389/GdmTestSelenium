﻿using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using GenerateDocument.Test.Extensions;

using static GenerateDocument.Test.Utilities.PageCommon;
using GenerateDocument.Common.Extensions;

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
