using GenerateDocument.Common.Extensions;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;
using System.Linq;
using System.Threading;

namespace GenerateDocument.Test.PageObjects.FrontEnd
{
    public class UserContentApprovalsReview : PageObject
    {
        public UserContentApprovalsReview(IWebDriver browser) : base(browser)
        {
        }

        public void HandleApprovalProcess(bool isApproved)
        {
            var linkText = isApproved ? "Mark All As Approved" : "Mark All As Declined";
         
            //Browser.FindElement(By.LinkText(linkText)).Click();
            BrowserWait().Until(ExpectedConditions.ElementToBeClickable(By.LinkText(linkText))).Click();
            Thread.Sleep(2000);
            BrowserWait(20).Until(ExpectedConditions.ElementToBeClickable(By.XPath("//div[@name='LinkbuttonApprove_div']//a[@id='LinkbuttonApprove']")))
                .Click();
            //Browser.FindElement(By.Id("LinkbuttonApprove")).Click();

            var alert = Browser.SwitchTo().Alert();
            alert.Accept();

            BrowserWait().Until(ExpectedConditions.UrlContains("UserContentApprovals"));
        }

        public bool CheckDesignSubmittedForApproval(string designName)
        {
            //click to review to get output name
            BrowserWait().Until(ExpectedConditions.ElementToBeClickable(By.XPath("//p[@class='ShipmentDetailsAnchor ShipmentDocumentDetails']//a"))).Click();

            BrowserWait().Until(ExpectedConditions.ElementIsVisible(By.Id("DocDetailPopup_divPop")));

            Browser.SwitchTo().Frame(Browser.FindElement(By.XPath("//iframe[contains(@src, 'UserContentApprovalDocumentDetails.aspx')]")));

            var arrText = BrowserWait(3).Until(ExpectedConditions.ElementIsVisible(By.XPath("//*[@id='mainForm']//div[@class='ApprovalDocumentDetailLeft']//div[@class='DocumentDetailsMain']//p"))).Text;

            var designNamePart = arrText.Split(new[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries)[3];
            designNamePart = designNamePart.Split(new[] { ">" }, StringSplitOptions.RemoveEmptyEntries).Last().Trim();

            Browser.SwitchTo().DefaultContent();

            //close modal
            Browser.FindElement(By.Id("DocDetailPopup_divPopClose1")).Click();
         
            return designName.IsEquals(designNamePart);
        }
    }
}
