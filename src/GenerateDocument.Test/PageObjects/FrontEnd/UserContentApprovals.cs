using GenerateDocument.Common.Extensions;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;
using GenerateDocument.Common;
using static GenerateDocument.Test.Utilities.PageCommon;

namespace GenerateDocument.Test.PageObjects.FrontEnd
{
    public class UserContentApprovals : PageBaseObject
    {
        private readonly UserContentApprovalsReview _userContentApprovalsReview;

        public UserContentApprovals(DriverContext driverContext) : base(driverContext)
        {
            _userContentApprovalsReview = new UserContentApprovalsReview(driverContext);
        }

        public void NavigateTo()
        {
            Driver.NavigateTo(UserContentApprovalsPage);
        }

        public void ReviewDesign(string designName)
        {
            DriverContext.BrowserWait().Until(ExpectedConditions.UrlContains("usercontentapprovals"));
            DriverContext.BrowserWait().Until(ExpectedConditions.ElementToBeClickable(By.Id("orderListTable")));

            checkDesignSubmittedForApproval(designName);
        }

        void checkDesignSubmittedForApproval(string name)
        {
            var result = false;
            var links = Driver.FindElements(By.XPath("//table[@class='itemTable']//tr//td//a[@class='siteLink']"));
            var i = links.Count;
            while (i > 0)
            {
                Console.WriteLine($"i: {i - 1} -- CountList: {links.Count}");
                Driver.ScrollToView(links[i - 1]);
                DriverContext.BrowserWait().Until(ExpectedConditions.ElementToBeClickable(links[i - 1]));

                links[i - 1].Click();

                result = _userContentApprovalsReview.CheckDesignSubmittedForApproval(name);
                Console.WriteLine($"result compare: {result}");
                if (result)
                {
                    break;
                }

                NavigateTo();

                DriverContext.BrowserWait().Until(ExpectedConditions.UrlContains("usercontentapprovals"));

                i--;
            }
        }
    }
}
