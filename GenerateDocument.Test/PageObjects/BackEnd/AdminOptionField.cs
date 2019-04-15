using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace GenerateDocument.Test.PageObjects.BackEnd
{
    public class AdminOptionField : PageObject
    {
        public AdminOptionField(IWebDriver browser) : base(browser)
        {
        }



        public void ClickToSelectLogoAndSupportingLogos()
        {
            var container = Browser.FindElement(By.ClassName("list"));

            ScrollToView(container);

            var link = container.FindElement(By.XPath("//*[@id=\"AdminMaster_ContentPlaceHolderBody__XF_DND_OptionValueTable1_ListView1_ctrl0_item\"]/div[1]"));

            //ScrollToView(link);

            //key: Logo and supporting logos
            //value: logos

            link.Click();
        }

        public void ClickToDeleteOptionItem()
        {
            var button = Browser.FindElement(By.XPath("//img[contains(@src, 'Delete.gif')]/ancestor::div[@class='adminButtonLabel adminButtonNoLabel']"));

            button.Click();
        }

        public void ClickToUpdateSetting()
        {
            var buttonUpdate = Browser.FindElement(By.Id("AdminMaster_ContentPlaceHolderBody_btnUpdate1"));
            ScrollToView(buttonUpdate);
            buttonUpdate.Click();
        }

        public void ClickToAddNewOptionItem()
        {
            var button = Browser.FindElement(By.XPath("//img[contains(@src, 'AddNew.gif')]/ancestor::div[@class='adminButtonLabel adminButtonNoLabel']"));

            button.Click();
        }

        public void InsertSelectLogoAndSupportingLogosOption()
        {
            var container = Browser.FindElement(By.ClassName("list"));

            ScrollToView(container);

            var wrapper = container.FindElement(By.Id("dojoUnique1"));

            var textboxKey = wrapper.FindElement(By.Name("TextBoxDisplayText"));
            textboxKey.Clear();
            textboxKey.SendKeys("Logo and supporting logos");

            var textboxValue = wrapper.FindElement(By.Name("TextBoxValue"));
            textboxValue.Clear();
            textboxValue.SendKeys("logos");
        }
    }
}
