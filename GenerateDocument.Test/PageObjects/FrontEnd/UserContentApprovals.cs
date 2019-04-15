using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Cryptography.X509Certificates;
using System.Threading;
using GenerateDocument.Common.Extensions;
using GenerateDocument.Test.Extensions;
using GenerateDocument.Test.Utilities;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using static GenerateDocument.Test.Utilities.PageCommon;

namespace GenerateDocument.Test.PageObjects.FrontEnd
{
    public class UserContentApprovals : PageObject
    {
        private UserContentApprovalsReview _userContentApprovalsReview;

        public UserContentApprovals(IWebDriver browser) : base(browser)
        {
            _userContentApprovalsReview = new UserContentApprovalsReview(browser);
        }

        public void NavigateTo()
        {
            Browser.NavigateTo(UserContentApprovalsPage);
        }

        public void ClickToReviewItem()
        {
            BrowserWait().Until(ExpectedConditions.ElementIsVisible(By.XPath("//div[@id='orderListTable']//table//tr[last()]//a[@class='siteLink']"))).Click();
        }

        public void ReviewDesign(string designName)
        {
            BrowserWait().Until(ExpectedConditions.UrlContains("usercontentapprovals"));
            BrowserWait().Until(ExpectedConditions.ElementToBeClickable(By.Id("orderListTable")));

            checkDesignSubmittedForApproval(designName);
        }

        void checkDesignSubmittedForApproval(string name)
        {
            var result = false;
            var links = Browser.FindElements(By.XPath("//table[@class='itemTable']//tr//td//a[@class='siteLink']"));
            var i = links.Count;
            while (i > 0)
            {
                Console.WriteLine($"i: {i - 1} -- CountList: {links.Count}");
                ScrollToView(links[i - 1]);
                BrowserWait().Until(ExpectedConditions.ElementToBeClickable(links[i - 1]));
             
                links[i - 1].Click();

                result = _userContentApprovalsReview.CheckDesignSubmittedForApproval(name);
                Console.WriteLine($"result compare: {result}");
                if (result)
                {
                    break;
                }

                NavigateTo();

                BrowserWait().Until(ExpectedConditions.UrlContains("usercontentapprovals"));

                //links = Browser.FindElements(By.XPath("//table[@class='itemTable']//tr//td//a[@class='siteLink']"));

                i--;
            }
        }
    }
}
