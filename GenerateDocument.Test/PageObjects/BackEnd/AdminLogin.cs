using System;
using GenerateDocument.Common;
using GenerateDocument.Common.Extensions;
using GenerateDocument.Common.Types;
using GenerateDocument.Common.WebElements;
using GenerateDocument.Domain.Designs;
using GenerateDocument.Domain.TestSenario;
using GenerateDocument.Test.Base;
using static GenerateDocument.Test.Utilities.PageCommon;

namespace GenerateDocument.Test.PageObjects.BackEnd
{
    public class AdminLogin : PageBaseObject, IAutoSave
    {
        private readonly ElementLocator
            _cookieconsentLocator = new ElementLocator(Locator.Id, "cookieconsent:desc"),
            _gotItLinkLocator = new ElementLocator(Locator.LinkText, "Got it!"),
            _userNameLocator = new ElementLocator(Locator.XPath, "//input[@name='Username']"),
            _passWordLocator = new ElementLocator(Locator.XPath, "//input[@name='Password']"),
            _loginButton = new ElementLocator(Locator.XPath, "//input[@type='submit' and contains(@id,'ButtonLogin')]"),

            _textboxLocator = new ElementLocator(Locator.XPath, "//input[@name='{0}']"),
            _loginButtonLocator = new ElementLocator(Locator.XPath, "//input[@type='submit' and contains(@id,'{0}')]");

        public AdminLogin(DriverContext driverContext) : base(driverContext)
        {
        }

        public AdminLogin LoginSystem(string userName, string password)
        {
            Driver.IsUrlEndsWith(PageTypes.AdminLogin.ToString());

            CookieConfirmation();

            Driver.GetElement<Textbox>(_userNameLocator).SetValue(userName);
            Driver.GetElement<Textbox>(_passWordLocator).SetValue(password);
            Driver.GetElement<Button>(_loginButton).ClickTo();

            return this;
        }

        public AdminLogin NavigateTo()
        {
            Driver.NavigateTo(AdminLoginPage);
            return this;
        }

        private void CookieConfirmation()
        {
            if (Driver.IsElementPresent(_cookieconsentLocator, BaseConfiguration.ShortTimeout))
            {
                Driver.GetElement(_gotItLinkLocator).Click();
            }
        }

        public void PerformToControlType(Step step, DesignModel designModel)
        {
            string text;
            Enum.TryParse(step.ControlType, true, out ControlTypes controlTypeValue);

            switch (controlTypeValue)
            {
                case ControlTypes.Browser:
                    NavigateTo();
                    break;

                case ControlTypes.Hyperlink:
                    CookieConfirmation();
                    break;

                case ControlTypes.Textbox:
                    text = string.IsNullOrEmpty(step.ControlValue) ? AdminAccount.UserName : step.ControlValue;
                    Driver.GetElement<Textbox>(_textboxLocator.Format(step.ControlId)).SetValue(text);
                    break;

                case ControlTypes.Password:
                    text = string.IsNullOrEmpty(step.ControlValue) ? AdminAccount.Password : step.ControlValue;
                    Driver.GetElement<Textbox>(_textboxLocator.Format(step.ControlId)).SetValue(text);
                    break;

                case ControlTypes.Button:
                    Driver.GetElement<Button>(_loginButtonLocator.Format(step.ControlId)).ClickTo();
                    break;
            }
        }
    }
}
