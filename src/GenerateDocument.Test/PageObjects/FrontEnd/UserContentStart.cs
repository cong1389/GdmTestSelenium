using System;
using GenerateDocument.Common;
using GenerateDocument.Common.Extensions;
using GenerateDocument.Common.Types;
using GenerateDocument.Common.WebElements;
using GenerateDocument.Test.Utilities;
using OpenQA.Selenium;
using System.Linq;
using System.Reflection;
using GenerateDocument.Domain.Designs;
using GenerateDocument.Domain.TestSenario;
using GenerateDocument.Test.Base;
using log4net;
using static GenerateDocument.Test.Utilities.PageCommon;

namespace GenerateDocument.Test.PageObjects.FrontEnd
{
    public class UserContentStart : PageBaseObject, IAutoSave
    {
        private static readonly ILog logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        private readonly ElementLocator
            _searchBoxLocator = new ElementLocator(Locator.XPath, "//div//input[@id='txtSearch']"),
            _selectProductHrefLocator = new ElementLocator(Locator.XPath, "//table[@class='catalogThumbnailArea']//a[@href='CreateUserDocument.aspx?product={0}']"),
            _searchButtonLocator = new ElementLocator(Locator.Id, "btnSearch");

        public UserContentStart(DriverContext driverContext) : base(driverContext)
        {
        }

        public ProductInfo SearchDocument(string input)
        {
            var productInfo = new ProductInfo();

            if (string.IsNullOrEmpty(input))
                return productInfo;

            Driver.GetElement<Textbox>(_searchBoxLocator).SetValue(input);
            Driver.GetElement<Button>(_searchButtonLocator).ClickTo();

            var result = Driver.FindElement(By.XPath($"//div[contains(@class, 'catalogItemFooter')]//a[contains(@title, '{input}')]"));
            string href = result?.GetAttribute("href");

            int hash = href.LastIndexOf('=') + 1;
            string productId = href.Substring(hash, href.Length - hash);

            productInfo.Id = productId;
            productInfo.Name = result?.Text;

            return productInfo;
        }

        public void ClickToCreateDesign(string productName)
        {
            logger.Info($"Creating design: {productName}");

            var element = Driver.FindElement(By.LinkText(productName));
            Driver.Actions().MoveToElement(element);
            element.Click();
        }

        public void SelectDocument(string id)
        {
            Driver.GetElement<Button>(_selectProductHrefLocator.Format(id)).ClickTo();
        }

        public void AgreeWithPrivacy()
        {
            var element = Driver.FindElements(By.ClassName("gdpr-consent-modal"));
            if (element.Any())
            {
                Driver.FindElement(By.Id("gdprConsentAcknowledge")).Click();
                Driver.FindElement(By.Id("gdprConsentContinue")).Click();
            }
        }

        public void NavigateTo()
        {
            Driver.NavigateTo(UserContentStartPage);

            Driver.WaitUntilPresentedUrl(PageTypes.UserContentStart.ToString());
        }

        public void PerformToControlType(Step step, DesignModel designModel)
        {
            Enum.TryParse(step.ControlType, true, out ControlTypes controlTypeValue);

            switch (controlTypeValue)
            {
                case ControlTypes.Browser:
                    NavigateTo();
                    break;

                case ControlTypes.Hyperlink:
                    ClickToCreateDesign(step.ControlValue);
                    break;
            }
        }
    }
}
