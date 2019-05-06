using GenerateDocument.Common;
using GenerateDocument.Common.Extensions;
using GenerateDocument.Common.Types;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using OpenQA.Selenium.Support.UI;
using System.Linq;
using System.Threading;
using static GenerateDocument.Test.Utilities.PageCommon;

namespace GenerateDocument.Test.PageObjects.BackEnd
{
    public class AdminProducts : PageBaseObject
    {
        private readonly AdminProductDetails _adminProductDetails;

        public AdminProducts(DriverContext driverContext) : base(driverContext)
        {
            _adminProductDetails = new AdminProductDetails(driverContext);
        }

        [FindsBy(How = How.Id, Using = "AdminMaster_ContentPlaceHolderBody_linkProjects")]
        private IWebElement LinkProject { get; set; }

        public string GetVdpIconSrc(string productName)
        {
            var cell = GetCellElementInGrid(productName);
            return cell.FindElement(By.TagName("img")).GetAttribute("src");
        }

        public string GetKitIconSrc(string productName)
        {
            var cell = GetCellElementInGrid(productName);
            return cell.FindElement(By.TagName("img")).GetAttribute("src");
        }

        private IWebElement GetCellElementInGrid(string productName)
        {
            DriverContext.BrowserWait().Until(ExpectedConditions.ElementIsVisible(By.XPath("//*[@id=\"AdminMaster_ContentPlaceHolderBody_grid\"]")));

            var rows = Driver.FindElements(By.XPath("//table[@class='EnhancedDataGrid']//tr"));

            for (int i = 3; i < rows.Count; i++)
            {
                var tds = rows[i].FindElements(By.TagName("td"));
                if (tds[2].Text.StartsWith(productName))
                {
                    return tds[2];
                }

            }

            return null;
        }

        public string GetLinkProjectText()
        {
            return LinkProject.Text;
        }

        public void ClickToAdvanceFlyer()
        {
            var xPath = $"//a[contains(text(),'[AUTOTEST]Advanced Flyer')]";

            Driver.FindElement(By.XPath(xPath)).Click();
        }

        public void GoToAdminProductDetails(string productName)
        {
            var xPath = $"//a[contains(text(),'{productName}')]";

            Driver.FindElement(By.XPath(xPath)).Click();
        }

        private void SelectProductToEnableActions(string productName)
        {
            DriverContext.BrowserWait().Until(ExpectedConditions.ElementIsVisible(By.ClassName("EnhancedDataGrid")));

            Driver.FindElement(By.XPath($"//a[text()='{productName}' or text()='{productName}*']//ancestor::tr[1]//td[1]//input[@type='checkbox']")).Click();
        }

        private void ClickToDuplicateAction()
        {
            Driver.FindElement(By.Id("AdminMaster_ContentPlaceHolderBody_buttonBar_btnDuplicate_spnActive")).Click();
        }

        private void ClickToRetireAction()
        {
            Driver.FindElement(By.Id("AdminMaster_ContentPlaceHolderBody_buttonBar_btnRetract_spnActive")).Click();
        }

        private void ClickToDeleteAction()
        {
            Driver.FindElement(By.Id("AdminMaster_ContentPlaceHolderBody_buttonBar_btnDelete_spnActive")).Click();
        }

        public void CloneToNewProduct(string productName, string namePrefix)
        {
            SelectProductToEnableActions(productName);

            ClickToDuplicateAction();

            var defaultClonedProduct = $"Copy of {productName}";

            var link = DriverContext.BrowserWait().Until(ExpectedConditions.ElementIsVisible(By.XPath($"//a[contains(text(),'{defaultClonedProduct}')]")));

            link.Click();

            DriverContext.BrowserWait().Until(ExpectedConditions.UrlContains("AdminProductDetails.aspx"));

            _adminProductDetails.ClickToChangeSetting();

            var productNameField = DriverContext.BrowserWait()
                .Until(ExpectedConditions.ElementIsVisible(By.Id("AdminMaster_ContentPlaceHolderBody_tbProductName")));

            productNameField.Clear();
            productNameField.SendKeys($"{namePrefix}{productName}");

            _adminProductDetails.UpdateSettings();

            _adminProductDetails.ClickToRelease();

            _adminProductDetails.UpdateSettings(true);
        }

        public void RetireProduct(string retiredProductName)
        {
            SelectProductToEnableActions(retiredProductName);

            ClickToRetireAction();

            _adminProductDetails.UpdateSettings();
        }

        public void DeleteProduct(string productName)
        {
            SelectProductToEnableActions(productName);

            ClickToDeleteAction();

            var alert = Driver.SwitchTo().Alert();
            alert.Accept();

            var i = 0;
            do
            {
                var elements = Driver.FindElements(By.XPath($"//a[contains(text(),'{productName}')]"));
                if (!elements.Any())
                {
                    break;
                }

                Thread.Sleep(2000);
                i++;
            } while (i < 3);
        }

        public void Open()
        {
            Driver.NavigateTo(AdminProductsPage);
        }
    }
}
