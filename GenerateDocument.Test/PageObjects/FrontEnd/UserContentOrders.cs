using System;
using System.Linq;
using GenerateDocument.Common.Extensions;
using GenerateDocument.Test.Extensions;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using OpenQA.Selenium.Support.UI;

using static GenerateDocument.Test.Utilities.PageCommon;

namespace GenerateDocument.Test.PageObjects.FrontEnd
{
    public class UserContentOrders : PageObject
    {
        public UserContentOrders(IWebDriver browser) : base(browser)
        {
        }

        public readonly string OrderInprogressStaticIcon = "https://ui.rightmarket.com/icons/order-inprogress-static.svg";
        public readonly string OrderCompletedIcon = "https://ui.rightmarket.com/icons/order-completed.svg";
        public readonly string OrderDeclinedIcon = "https://ui.rightmarket.com/icons/order-declined.svg";
        public readonly string OrderWaitApprovalIcon = "https://ui.rightmarket.com/icons/order-await-approval.svg";
        public readonly string OrderInProgressIcon = "https://ui.rightmarket.com/icons/gif/inprogress-24px.gif";
        public readonly string OrderCompletedStatus = "Completed";
        public readonly string OrderInprocessStatus = "In process";

        [FindsBy(How = How.ClassName, Using = "itemTableLabelWithIcon")]
        private IWebElement NameOrderLabel { set; get; }

        [FindsBy(How = How.ClassName, Using = "itemTable-Status")]
        private IWebElement OrderStatus { get; set; }

        private IWebElement ShareToFacebookIcon
        {
            get
            {
                try
                {
                    var elements = BrowserWait(5).Until(x =>
                        x.FindElement(By.XPath("//img[@src='https://ui.rightmarket.com/icons/facebook-logo-on-square.svg']")));

                    return elements;
                }
                catch
                {
                    return null;
                }
            }
        }

        public string GetOrderCompleteStatus()
        {
            BrowserWait(120).Until(x => x.FindElement(By.ClassName("itemTable-Status")).Text.IsEquals(OrderCompletedStatus));

            return OrderStatus.Text.Trim();
        }

        public string GetOrderInprocessStatus()
        {
            BrowserWait(120).Until(x => x.FindElement(By.ClassName("itemTable-Status")).Text.IsEquals(OrderInprocessStatus));

            return OrderStatus.Text.Trim();
        }

        public string GetNameOrder()
        {
            BrowserWait(120).Until(ExpectedConditions.VisibilityOfAllElementsLocatedBy(By.ClassName("itemTableLabelWithIcon")));

            return NameOrderLabel.Text.Trim();
        }

        public void ClickToReuseDocument(string documentId)
        {
            var element = BrowserWait().Until(ExpectedConditions.ElementIsVisible(By.XPath($"//a[contains(@documentid, '{documentId}')]")));
            ScrollToView(element);
            element.Click();
        }

        public bool CheckDocumentCanReuse(string documentId)
        {
            try
            {
                return BrowserWait().Until(ExpectedConditions.ElementIsVisible(By.XPath($"//a[contains(@documentid, '{documentId}')]"))) != null;
            }
            catch
            {
                return false;
            }
        }

        public bool IsShareToFacebookIconEnabled()
        {
            return ShareToFacebookIcon.Enabled;
        }

        public void ClickToShareToFacebookIcon()
        {
            ShareToFacebookIcon.Click();
        }

        public void Open()
        {
            Browser.NavigateTo(UserContentOrdersPage);
        }
    }
}
