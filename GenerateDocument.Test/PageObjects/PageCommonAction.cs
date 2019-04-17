using GenerateDocument.Common;
using GenerateDocument.Common.Extensions;
using GenerateDocument.Common.Types;
using GenerateDocument.Test.WrapperFactory;
using OpenQA.Selenium;
using System;
using System.Linq;

namespace GenerateDocument.Test.PageObjects
{
    public class PageCommonAction : PageObject
    {
        private readonly ElementLocator notifyMsg = new ElementLocator(Locator.ClassName, "cg-notify-message-template");

        public PageCommonAction(IWebDriver browser) : base(browser)
        {
        }

        public void UserLogout()
        {
            var logoutLink = Browser.FindElement(By.XPath("//a[contains(@href, 'GeneratedDocumentMngExtension/Logout.aspx')]"));
            ScrollToView(logoutLink);
            logoutLink.Click();
        }

        public bool GetNotifyMessage
        {
            get
            {
                return Browser.IsElementPresent(notifyMsg);
            }
        }

        /// <summary>
        /// This method adds an Mopinion MSFeedbackSent cookie
        /// The UI has Mopinion feedback tool integrated. Everytime the user downloads a output a feedback form is displayed if the 
        /// user has not given feedback. Mopinion creates a browser cookie to record feedback sent.
        /// We don't want to send fake feedback in test because it will spoil data gather from live customers so we will just
        /// pretend that the feedback has already been sent.
        /// </summary>
        public void CreateMopinionCookie()
        {
            var cookieName = $"MSFeedbackSent{ConfigInfo.MopinionFormId}";
            if (!Browser.Manage().Cookies.AllCookies.Any(x => x.Name.Equals(cookieName)))
            {
                var mopinionCookie = new Cookie(cookieName, "true", "/", DateTime.Now.AddDays(-1));

                Browser.Manage().Cookies.AddCookie(mopinionCookie);
            }
        }
    }
}
