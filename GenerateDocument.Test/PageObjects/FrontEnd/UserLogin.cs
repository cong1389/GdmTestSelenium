using GenerateDocument.Common;
using GenerateDocument.Common.Extensions;
using GenerateDocument.Common.Types;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;
using static GenerateDocument.Test.Utilities.PageCommon;

namespace GenerateDocument.Test.PageObjects.FrontEnd
{
    public class UserLogin : PageBaseObject
    {
        private readonly ElementLocator
            _cookieconsent = new ElementLocator(Locator.Id, "cookieconsent:desc"),
            _gotItLink = new ElementLocator(Locator.LinkText, "Got it!"),
            _userName = new ElementLocator(Locator.XPath, "//input[@name='Username']"),
            _passWord = new ElementLocator(Locator.XPath, "//input[@name='Password']"),
            _loginButton = new ElementLocator(Locator.XPath, "//div[@id='btnLogin_div']//a[@class='siteButton']"),
            _cookiePolicyButton = new ElementLocator(Locator.ClassName, "cc-dismiss");

        public UserLogin(DriverContext driverContext) : base(driverContext)
        {
        }

        public UserLogin LoginSystem(string userName, string password)
        {
            CookieConfirmation();

            Driver.GetElement(_userName).Clear();
            Driver.GetElement(_userName).SendKeys(userName);

            Driver.GetElement(_passWord).Clear();
            Driver.GetElement(_passWord).SendKeys(password);

            Driver.GetElement(_loginButton).Click();

            return this;
        }

        //public UserLogin LoginSystem()
        //{
        //    if (NeedToLogin())
        //    {
        //        NavigateTo();
        //        LoginSystem(UserId, UserPassword);
        //    }

        //    return this;
        //}

        public UserLogin NavigateTo()
        {
            Driver.NavigateTo(UserLoginPage);

            return this;
        }

        public bool CheckAfterLoginPage(string page)
        {
            var wait = new WebDriverWait(Driver, TimeSpan.FromSeconds(BaseConfiguration.LongTimeout));

            try
            {
                wait.Until(x =>
                {
                    return x.IsUrlEndsWith(page);
                });
            }
            catch (WebDriverTimeoutException)
            {
                return false;
            }

            return true;
        }

        private void CookieConfirmation()
        {
            if (Driver.IsElementPresent(_cookieconsent))
            {
                Driver.GetElement(_gotItLink).Click();
                Driver.WaitUntilElementIsNoLongerFound(_cookieconsent,BaseConfiguration.ShortTimeout);
            }
        }

        private bool NeedToLogin()
        {
            return Driver.IsUrlEndsWith("login.aspx") || Driver.IsUrlEndsWith("restrictlogin");
        }
    }
}
