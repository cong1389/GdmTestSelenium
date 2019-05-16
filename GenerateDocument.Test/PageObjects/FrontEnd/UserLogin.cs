using GenerateDocument.Common;
using GenerateDocument.Common.Extensions;
using GenerateDocument.Common.Types;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;
using System.Reflection;
using GenerateDocument.Common.WebElements;
using GenerateDocument.Domain.TestSenario;
using GenerateDocument.Test.Base;
using log4net;
using static GenerateDocument.Test.Utilities.PageCommon;

namespace GenerateDocument.Test.PageObjects.FrontEnd
{
    public class UserLogin : PageBaseObject, IAutoSave
    {
        private readonly ElementLocator
            _cookieconsent = new ElementLocator(Locator.Id, "cookieconsent:desc"),
            _gotItLink = new ElementLocator(Locator.LinkText, "Got it!"),
            _userNameLocator = new ElementLocator(Locator.XPath, "//input[@name='Username']"),
            _passWordLocator = new ElementLocator(Locator.XPath, "//input[@name='Password']"),
            _loginButton = new ElementLocator(Locator.XPath, "//div[@id='btnLogin_div']//a[@class='siteButton']"),

            _textboxLocator = new ElementLocator(Locator.XPath, "//input[@name='{0}']"),
            _loginButtonLocator = new ElementLocator(Locator.XPath, "//div[@id='btnLogin_div']//a[text()='{0}']");

        public UserLogin(DriverContext driverContext) : base(driverContext)
        {
        }

        public UserLogin LoginSystem(string userName, string password)
        {
            CookieConfirmation();

            Driver.GetElement<Textbox>(_userNameLocator).SetValue(userName);
            Driver.GetElement<Textbox>(_passWordLocator).SetValue(password);
            Driver.GetElement<Button>(_loginButton).ClickTo();

            return this;
        }

        public UserLogin NavigateTo()
        {
            Driver.NavigateTo(UserLoginPage);
            Driver.IsUrlEndsWith(PageTypes.Login.ToString());

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
                Driver.WaitUntilElementIsNoLongerFound(_cookieconsent, BaseConfiguration.ShortTimeout);
            }
        }

        private bool NeedToLogin()
        {
            return Driver.IsUrlEndsWith("login.aspx") || Driver.IsUrlEndsWith("restrictlogin");
        }

        public void PerformToControlType(Step step)
        {
            string text;
            string controlType = step.ControlType;
            Enum.TryParse(controlType, true, out ControlTypes controlTypeValue);

            switch (controlTypeValue)
            {
                case ControlTypes.Browser:
                    NavigateTo();
                    break;

                case ControlTypes.Hyperlink:
                    CookieConfirmation();
                    break;

                case ControlTypes.Textbox:
                    text = string.IsNullOrEmpty(step.ControlValue) ? ProjectBaseConfiguration.UserId : step.ControlValue;
                    Driver.GetElement<Textbox>(_textboxLocator.Format(step.ControlId)).SetValue(text);
                    break;

                case ControlTypes.Password:
                    text = string.IsNullOrEmpty(step.ControlValue) ? ProjectBaseConfiguration.UserPassword : step.ControlValue;
                    Driver.GetElement<Textbox>(_textboxLocator.Format(step.ControlId)).SetValue(text);
                    break;

                case ControlTypes.Button:
                    Driver.GetElement<Button>(_loginButtonLocator.Format(step.ControlId)).ClickTo();
                    break;
            }
        }
    }
}
