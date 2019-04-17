using GenerateDocument.Common.Extensions;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using System.Collections.Generic;
using System.Linq;
using static GenerateDocument.Test.Utilities.PageCommon;

namespace GenerateDocument.Test.PageObjects.BackEnd
{
    public class AdminUserAccounts : PageObject
    {
        public AdminUserAccounts(IWebDriver browser) : base(browser)
        {
        }

        [FindsBy(How = How.Id, Using = "AdminMaster_Title")]
        private IWebElement Title { set; get; }


        [FindsBy(How = How.ClassName, Using = "LoginCvcRow")]
        private IList<IWebElement> LoginCvcRows { set; get; }

        public List<string> GetAccountNames()
        {
            return LoginCvcRows.Select(x => x.Text).ToList();
        }

        public void Open()
        {
            Browser.NavigateTo(AdminUserAccountsPage);
        }

        public void OpenAdminGroup()
        {
            Browser.NavigateTo(AdminUserAccountAdminGroupPage);
        }

        public string GetTitleText()
        {
            return Title.Text;
        }
    }
}
