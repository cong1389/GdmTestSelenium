﻿using GenerateDocument.Common;
using GenerateDocument.Common.Extensions;
using GenerateDocument.Common.Types;
using OpenQA.Selenium;
using System;
using System.Linq;
using GenerateDocument.Common.WebElements;

namespace GenerateDocument.Test.PageObjects
{
    public class PageCommonAction : PageBaseObject
    {
        private readonly ElementLocator
            _logoutLinkLocator = new ElementLocator(Locator.XPath, "//a[contains(@href, 'GeneratedDocumentMngExtension/Logout.aspx')]"),
            _notifyMsgLocator = new ElementLocator(Locator.ClassName, "cg-notify-message-template");

        public PageCommonAction(DriverContext driverContext) : base(driverContext)
        {
        }

        public void UserLogout()
        {
            Driver.GetElement<Button>(_logoutLinkLocator).ClickTo();
        }

        public bool GetNotifyMessage()
        {
            return Driver.IsElementPresent(_notifyMsgLocator);
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
            var cookieName = $"MSFeedbackSent{ProjectBaseConfiguration.MopinionFormId}";

            if (!Driver.Manage().Cookies.AllCookies.Any(x => x.Name.Equals(cookieName)))
            {
                var mopinionCookie = new Cookie(cookieName, "true", "/", DateTime.Now.AddDays(-1));

                Driver.Manage().Cookies.AddCookie(mopinionCookie);
            }
        }
    }
}
