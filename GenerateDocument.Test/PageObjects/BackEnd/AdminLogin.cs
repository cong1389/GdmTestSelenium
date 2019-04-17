using GenerateDocument.Common;
using GenerateDocument.Common.Extensions;
using GenerateDocument.Common.Types;
using static GenerateDocument.Test.Utilities.PageCommon;

namespace GenerateDocument.Test.PageObjects.BackEnd
{
    public class AdminLogin : PageBaseObject
    {
        private readonly ElementLocator
            _cookieconsent = new ElementLocator(Locator.Id, "cookieconsent:desc"),
            _gotItLink = new ElementLocator(Locator.LinkText, "Got it!"),
            _userName = new ElementLocator(Locator.XPath, "//input[@name='Username']"),
            _passWord = new ElementLocator(Locator.XPath, "//input[@name='Password']"),
            _loginButton = new ElementLocator(Locator.XPath, "//*[contains(@id,'ButtonLogin')]"),
            _cookiePolicyButton = new ElementLocator(Locator.ClassName, "cc-dismiss");

        public AdminLogin(DriverContext driverContext) : base(driverContext)
        {
        }

        public AdminLogin LoginSystem(string userName, string password)
        {
            Driver.IsUrlEndsWith("adminLogin.aspx");

            CookieConfirmation();

            Driver.GetElement(_userName).Clear();
            Driver.GetElement(_userName).SendKeys(userName);

            Driver.GetElement(_passWord).Clear();
            Driver.GetElement(_passWord).SendKeys(password);

            Driver.GetElement(_loginButton).Click();

            return this;
        }

        public AdminLogin NavigateTo()
        {
            Driver.NavigateTo(AdminLoginPage);
            return this;
        }

        private void CookieConfirmation()
        {
            //var displayedCookieConfirm = Driver.GetElement(_cookieconsent);
            if (Driver.IsElementPresent(_cookieconsent))
            {
                Driver.GetElement(_gotItLink).Click();
            }
        }
    }
}
