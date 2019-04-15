using System.Linq;
using System.Threading;
using GenerateDocument.Common.Extensions;
using GenerateDocument.Test.Extensions;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using OpenQA.Selenium.Support.UI;
using static GenerateDocument.Test.Utilities.PageCommon;
using GenerateDocument.Common;
using GenerateDocument.Common.Types;

namespace GenerateDocument.Test.PageObjects.BackEnd
{
    public class AdminProducts : PageObject
    {
        private AdminProductDetails _adminProductDetails;

        private readonly ElementLocator eleProductNameInGrid = new ElementLocator(Locator.XPath, "//table[@class='EnhancedDataGrid']//tr//td//a[text()='{0}");

        public AdminProducts(IWebDriver browser) : base(browser)
        {
            _adminProductDetails = new AdminProductDetails(browser);
        }

        [FindsBy(How = How.Id, Using = "AdminMaster_ContentPlaceHolderBody_linkProjects")]
        private IWebElement LinkProject { get; set; }

        [FindsBy(How = How.Id, Using = "AdminMaster_ContentPlaceHolderBody_grid_ctl11_imgProdIcon")]
        private IWebElement KitIcon { get; set; }

        [FindsBy(How = How.Id, Using = "AdminMaster_ContentPlaceHolderBody_grid_ctl03_imgProdIcon")]
        private IWebElement VdpIcon { get; set; }

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
            BrowserWait().Until(ExpectedConditions.ElementIsVisible(By.XPath("//*[@id=\"AdminMaster_ContentPlaceHolderBody_grid\"]")));

            var rows = Browser.FindElements(By.XPath("//table[@class='EnhancedDataGrid']//tr"));

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

            Browser.FindElement(By.XPath(xPath)).Click();
        }

        public void ClickToA4Poster()
        {
            var xPath = $"//a[contains(text(),'A4 Poster')]";

            Browser.FindElement(By.XPath(xPath)).Click();
        }

        public void GoToAdminProductDetails(string productName)
        {
            var xPath = $"//a[contains(text(),'{productName}')]";

            Browser.FindElement(By.XPath(xPath)).Click();
        }

        public void SelectProductToEnableActions(string productName)
        {
            BrowserWait().Until(ExpectedConditions.ElementIsVisible(By.ClassName("EnhancedDataGrid")));

            Browser.FindElement(By.XPath($"//a[text()='{productName}' or text()='{productName}*']//ancestor::tr[1]//td[1]//input[@type='checkbox']")).Click();

            //var tableRows = Browser.FindElements(By.XPath("//table[@class='EnhancedDataGrid']//tr"));

            //for (var i = 3; i < tableRows.Count; i++)
            //{
            //    var cells = tableRows[i].FindElements(By.TagName("td"));
            //    if (cells[2].Text.IsEquals(productName))
            //    {
            //        cells[0].Click();

            //        break;
            //    }
            //}
        }

        public void ClickToDuplicateAction()
        {
            Browser.FindElement(By.Id("AdminMaster_ContentPlaceHolderBody_buttonBar_btnDuplicate_spnActive")).Click();
        }

        public void ClickToRetireAction()
        {
            Browser.FindElement(By.Id("AdminMaster_ContentPlaceHolderBody_buttonBar_btnRetract_spnActive")).Click();
        }

        public void ClickToDeleteAction()
        {
            Browser.FindElement(By.Id("AdminMaster_ContentPlaceHolderBody_buttonBar_btnDelete_spnActive")).Click();
        }

        public void CloneToNewProduct(string productName, string namePrefix)
        {
            SelectProductToEnableActions(productName);

            ClickToDuplicateAction();

            var defaultClonedProduct = $"Copy of {productName}";

            var link = BrowserWait().Until(ExpectedConditions.ElementIsVisible(By.XPath($"//a[contains(text(),'{defaultClonedProduct}')]")));

            link.Click();

            BrowserWait().Until(ExpectedConditions.UrlContains("AdminProductDetails.aspx"));

            _adminProductDetails.ClickToChangeSetting();

            var productNameField = BrowserWait()
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

            var alert = Browser.SwitchTo().Alert();
            alert.Accept();

            var i = 0;
            do
            {
                var elements = Browser.FindElements(By.XPath($"//a[contains(text(),'{productName}')]"));
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
            Browser.NavigateTo(AdminProductsPage);
        }
    }
}
