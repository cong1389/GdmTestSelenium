using System;
using System.Collections.Generic;
using GenerateDocument.Common.Extensions;
using GenerateDocument.Test.Extensions;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;

using static GenerateDocument.Test.Utilities.PageCommon;

namespace GenerateDocument.Test.PageObjects.BackEnd
{
    public class AdminManagedConfiguration : PageObject
    {
        public AdminManagedConfiguration(IWebDriver browser) : base(browser)
        {
        }

        [FindsBy(How = How.Id, Using = "DivLinkConfiguration")]
        private IWebElement ConfigurationTab { get; set; }

        [FindsBy(How = How.ClassName, Using = "onoffswitch")]
        private IList<IWebElement> FeatureOnOffConfigurations { get; set; }

        [FindsBy(How = How.Id, Using = "AdminMaster_ContentPlaceHolderBody_ButtonSave")]
        private IWebElement ButtonSave { get; set; }

        private IWebElement ToggleRmAdminLayout
        {
            get { return Browser.FindElement(By.Id("InputMasterUserExempt")); }
        }

        public void ClickToTurnOnOrOffAllFeatures()
        {
            if (!FeatureOnOffConfigurations[1].Selected)
            {
                FeatureOnOffConfigurations[1].Click();
            }
        }

        public void ChangeLayoutForAdminSite(bool isRmLayout)
        {
            var isChecked = Convert.ToBoolean(ToggleRmAdminLayout.GetAttribute("checked"));

            if (isRmLayout && isChecked || !isRmLayout && !isChecked)
            {
                FeatureOnOffConfigurations[1].Click();
            }

        }

        public void Open()
        {
            Browser.NavigateTo(AdminManagedConfigurationPage);
        }

        public void ClickToConfigurationTab()
        {
            ConfigurationTab.Click();
        }

        public void SaveSetting()
        {
            ButtonSave.Click();
        }
    }
}
