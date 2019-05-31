﻿using GenerateDocument.Common;
using GenerateDocument.Common.Extensions;
using GenerateDocument.Common.Types;
using GenerateDocument.Domain.Designs;
using GenerateDocument.Domain.TestSenario;
using GenerateDocument.Test.Base;
using log4net;
using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;
using System;
using System.Linq;
using System.Reflection;
using System.Threading;
using GenerateDocument.Common.WebElements;
using static GenerateDocument.Test.Utilities.PageCommon;

namespace GenerateDocument.Test.PageObjects.NewApp
{
    public class MyDesign : PageBaseObject, IAutoSave
    {
        private static readonly ILog logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        private readonly ElementLocator
            _browseTemplateCatalog = new ElementLocator(Locator.XPath, "//button[@ng-click='browseTemplateCatalog()']"),
            _dataDocumentName = new ElementLocator(Locator.XPath, "//div[@data-documentname='{0}']"),
            _dataThumbnailBox = new ElementLocator(Locator.XPath, "//div[@data-documentname='{0}']//div[@class='thumbnail']"),
            _dataMenuGoTo = new ElementLocator(Locator.XPath, "//div[@data-documentname='{0}']//div[@class='thumbnail']//div[@class='content-wrapper']//ul//li//a[@name='go']"),
            _designStatus = new ElementLocator(Locator.XPath, "//div[@data-documentname='{0}']//div[@class='thumbnail']//div[@class='content-wrapper']//div[contains(@class, 'design-info')]//span"),
            _designNamesLocator = new ElementLocator(Locator.XPath, "//div[@class='thumbnail']//div[@class='caption']//div[@class='caption-container']//h4"),
            _deletedLableLocator = new ElementLocator(Locator.XPath, "//div[@data-documentname='{0}']//div[@class='thumbnail']//div[@class='content-wrapper']//label"),
            _kitLable = new ElementLocator(Locator.XPath, "//div[@data-documentname='{0}']//div[@class='thumbnail']//div[@class='caption']//div[@class='caption-container']//span[contains(@class, 'kit-label')]"),

            _menusLocator = new ElementLocator(Locator.XPath, "//div[@data-documentname='{0}']"),
            _textBoxLocator = new ElementLocator(Locator.XPath, "//input[@id='txtDesignSearch' or @name='txtDesignSearch']"),
            _actionMenuLocator = new ElementLocator(Locator.XPath, "//div[@data-documentname='{0}']//li//a[@name='{1}']"),

            _labelLocator = new ElementLocator(Locator.XPath, "//div[@data-documentname='{0}']//*[@id='{1}']"),
            _buttonLocator = new ElementLocator(Locator.XPath, "//button[@name='{0}' or @id='{0}']");


        public MyDesign(DriverContext driverContext) : base(driverContext)
        {
        }

        public MyDesign NavigateTo()
        {
            Driver.NavigateTo(MyDesignPage);

            Driver.WaitUntilPresentedUrl(PageTypes.Designs.ToString());

            return this;
        }

        public MyDesign CreateDesign()
        {
            Driver.ScrollToTop();
            Driver.GetElement(_browseTemplateCatalog).OnClickJavaScript();

            return this;
        }

        public void ClickToEditDesign(string documentName)
        {
            DriverContext.BrowserWait().Until(ExpectedConditions.ElementIsVisible(By.Id("products")));

            var element = DriverContext.BrowserWait(35).Until(ExpectedConditions.ElementToBeClickable(By.XPath($"//div[@data-documentname='{documentName}']//div[@class='thumbnail']")));

            var actions = new Actions(DriverContext.Driver);
            actions.MoveToElement(element).Perform();

            DriverContext.BrowserWait().Until(ExpectedConditions.ElementToBeClickable(By.XPath($"//div[@data-documentname='{documentName}']//div[@class='thumbnail']//div[@class='content-wrapper']//ul//li//a[@name='edit']"))).Click();

            DriverContext.BrowserWait().Until(ExpectedConditions.UrlContains("UserEditFormFilling"));
        }

        public void DoSort(string optionName)
        {
            DriverContext.BrowserWait().Until(ExpectedConditions.ElementIsVisible(By.XPath("//div[@class='dropdown']")));

            //we should not use actions.MoveToEelement in this case because the dropdown location is incorrect since more data loaded by paging
            DriverContext.Driver.ScrollToTop();

            DriverContext.Driver.FindElement(By.XPath("//div[@class='dropdown']//button[contains(@class, 'dropdown')]")).Click();

            DriverContext.BrowserWait().Until(ExpectedConditions.ElementIsVisible(By.XPath("//div[@class='dropdown open']")));

            DriverContext.Driver.FindElement(By.LinkText(optionName)).Click();
        }

        public string GetDesignStatus(string designName)
        {
            DriverContext.Driver.RefreshPage();

            Driver.WaitUntilPresentedElement(_dataDocumentName.Format(designName));
            var designStatusEle = Driver.WaitUntilPresentedElement(_designStatus.Format(designName), e => e.Displayed, BaseConfiguration.LongTimeout);

            return designStatusEle == null ? string.Empty : designStatusEle.Text;
        }

        public string GetDesignType(string designName)
        {
            Driver.WaitUntilPresentedElement(_dataDocumentName.Format(designName), BaseConfiguration.LongTimeout);
            var element = Driver.WaitUntilPresentedElement(_kitLable.Format(designName), BaseConfiguration.LongTimeout);

            return element.Text;
        }

        public string GetRetiredOrDeletedLabel(string designName)
        {
            Driver.WaitUntilPresentedElement(_dataDocumentName.Format(designName), BaseConfiguration.LongTimeout);
            var element = Driver.WaitUntilPresentedElement(_deletedLableLocator.Format(designName), BaseConfiguration.LongTimeout);
            return element.Text;
        }

        public string GetSelectedSortingOption()
        {
            DriverContext.BrowserWait().Until(ExpectedConditions.ElementIsVisible(By.XPath("//div[@class='dropdown']")));

            return DriverContext.Driver.FindElement(By.XPath("//div[@class='dropdown']//button[contains(@class, 'dropdown')]")).Text;
        }

        public string GetNoDesignsMessage()
        {
            return DriverContext.BrowserWait().Until(ExpectedConditions.ElementIsVisible(By.XPath("//div[contains(@class, 'no-designs-wrapper')]//h4"))).Text;
        }

        public string[] GetDesignNames()
        {
            Driver.WaitForAngular(BaseConfiguration.LongTimeout);

            var eles = Driver.GetElements(_designNamesLocator, 1);
            return eles.Select(x => x.Text).ToArray();
        }

        public string[] GetDesignNamesByStatus(string status)
        {
            var designs = DriverContext.Driver.FindElements(By.XPath($"//div[@id='products']//div[@data-sfdocumentstatus='{status}']"));
            if (designs.Any())
            {
                return designs.Select(y => y.GetAttribute("data-documentname")).ToArray();
            }

            return new string[] { };
        }

        public int CountDesignsByStatus(int expectedNumber, string status)
        {
            DriverContext.BrowserWait().Until(ExpectedConditions.ElementIsVisible(By.Id("products")));

            var designsByStatusCount = 0;
            var tryingTime = 0;
            do
            {
                designsByStatusCount = DriverContext.Driver.FindElements(By.XPath("//div[@class='thumbnail']//div[@class='content-wrapper']//div[contains(@class, 'design-info')]//span")).Where(x => x.Text.IsEquals(status)).Count();

                if (designsByStatusCount == expectedNumber)
                {
                    break;
                }

                tryingTime++;

                Thread.Sleep(1000);
            } while (tryingTime < 5);

            return designsByStatusCount;
        }

        public int CountDesignsByStatus(string status)
        {
            DriverContext.BrowserWait().Until(ExpectedConditions.ElementIsVisible(By.Id("products")));

            return DriverContext.Driver.FindElements(By.XPath("//div[@class='thumbnail']//div[@class='content-wrapper']//div[contains(@class, 'design-info')]//span")).Where(x => x.Text.IsEquals(status)).Count();
        }

        private bool CheckDesignExists(string designName)
        {
            return DriverContext.Driver.FindElements(By.XPath($"//div[@data-documentname='{designName}']//div[@class='thumbnail']//div[@class='caption']//div[@class='caption-container']//h4")).Any(x => x.Text.IsEquals(designName));
        }

        public bool CheckDesignCloneable(string designName)
        {
            DriverContext.BrowserWait().Until(ExpectedConditions.ElementIsVisible(By.Id("products")));

            var element = DriverContext.BrowserWait().Until(ExpectedConditions.ElementToBeClickable(
                By.XPath($"//div[@data-documentname='{designName}']//div[@class='thumbnail']")));

            var actions = new Actions(DriverContext.Driver);
            actions.MoveToElement(element).Perform();

            return !DriverContext.Driver
                .FindElements(By.XPath(
                    $"//div[@data-documentname='{designName}']//div[@class='thumbnail']//div[@class='content-wrapper']//ul//li[@class='inactive']//a[@name='copy']"))
                .Any();
        }

        public bool CheckDesignEditable(string designName)
        {
            DriverContext.BrowserWait().Until(ExpectedConditions.ElementIsVisible(By.Id("products")));

            var element = DriverContext.BrowserWait().Until(ExpectedConditions.ElementToBeClickable(
                By.XPath($"//div[@data-documentname='{designName}']//div[@class='thumbnail']")));

            var actions = new Actions(DriverContext.Driver);
            actions.MoveToElement(element).Perform();

            return DriverContext.BrowserWait().Until(ExpectedConditions.ElementIsVisible(By.XPath($"//div[@data-documentname='{designName}']//div[@class='thumbnail']//div[@class='content-wrapper']//ul//li[@class='inactive']//a[@name='edit']"))) == null;
        }

        public bool CheckDesignCanGoToActionPage(string designName)
        {
            DriverContext.BrowserWait().Until(ExpectedConditions.ElementIsVisible(By.Id("products")));

            var element = DriverContext.BrowserWait().Until(ExpectedConditions.ElementToBeClickable(
                By.XPath($"//div[@data-documentname='{designName}']//div[@class='thumbnail']")));

            var actions = new Actions(DriverContext.Driver);
            actions.MoveToElement(element).Perform();

            return DriverContext.BrowserWait().Until(ExpectedConditions.ElementIsVisible(By.XPath($"//div[@data-documentname='{designName}']//div[@class='thumbnail']//div[@class='content-wrapper']//ul//li[@class='inactive']//a[@name='go']"))) == null;
        }

        public bool DoCopyDesign(string designName, string copiedDesignName)
        {
            DriverContext.BrowserWait().Until(ExpectedConditions.ElementToBeClickable(By.XPath($"//div[@data-documentname='{designName}']//div[@class='thumbnail']//div[@class='content-wrapper']//ul//li//a[@name='copy']"))).OnClickJavaScript();

            DriverContext.BrowserWait().Until(ExpectedConditions.ElementIsVisible(By.ClassName("modal-open")));

            var textField = DriverContext.BrowserWait().Until(ExpectedConditions.ElementToBeClickable(By.Name("designName")));
            textField.Clear();
            textField.SendKeys(copiedDesignName);

            DriverContext.BrowserWait().Until(ExpectedConditions.ElementToBeClickable(By.XPath("//button[@ng-click='cloneDesign()']"))).OnClickJavaScript();

            DriverContext.BrowserWait().Until(ExpectedConditions.InvisibilityOfElementLocated(By.ClassName("modal-open")));

            return CheckDesignExists(copiedDesignName);
        }

        public void GoToActions(string documentName)
        {
            Driver.IsElementPresent(_dataThumbnailBox.Format(documentName));
            {
                var thumbnailBoxEle = Driver.WaitUntilPresentedElement(_dataThumbnailBox.Format(documentName), BaseConfiguration.LongTimeout);
                Driver.Actions().MoveToElement(thumbnailBoxEle).Perform();

                Driver.GetElement(_dataMenuGoTo.Format(documentName)).Click();
            }
        }

        public bool CheckDeleteDocumentAction(string documentName)
        {
            DriverContext.BrowserWait().Until(ExpectedConditions.ElementIsVisible(By.Id("products")));

            var element = DriverContext.BrowserWait(35).Until(ExpectedConditions.ElementToBeClickable(
                By.XPath($"//div[contains(@data-documentname, '{documentName}')]//div[@class='thumbnail']")));

            var actions = new Actions(DriverContext.Driver);
            actions.MoveToElement(element).Perform();

            DriverContext.BrowserWait().Until(ExpectedConditions.ElementToBeClickable(By.XPath($"//div[contains(@data-documentname, '{documentName}')]//div[@class='thumbnail']//div[@class='content-wrapper']//ul//li//a[@name='delete']"))).Click();

            DriverContext.BrowserWait().Until(ExpectedConditions.ElementIsVisible(By.ClassName("modal-open")));

            DriverContext.BrowserWait().Until(ExpectedConditions.ElementToBeClickable(By.XPath("//button[@ng-click='deleteDesign()']"))).Click();

            DriverContext.BrowserWait().Until(ExpectedConditions.InvisibilityOfElementLocated(By.ClassName("modal-open")));

            return !CheckDesignExists(documentName);
        }

        public bool DoPaging()
        {
            DriverContext.BrowserWait().Until(ExpectedConditions.ElementIsVisible(By.Id("products")));

            var elements = DriverContext.Driver.FindElements(By.Id("paging"));

            if (elements.Any() && elements.FirstOrDefault().Displayed)
            {
                var button = DriverContext.Driver.FindElement(By.XPath("//div[@id='paging']//div/button"));
                var actions = new Actions(DriverContext.Driver);
                actions.MoveToElement(button);

                button.Click();

                DriverContext.BrowserWait().Until(ExpectedConditions.InvisibilityOfElementLocated(By.XPath("//div[@id='paging']//div//button[contains(@class, 'pressed')]")));

                Thread.Sleep(2000);

                return true;
            }

            return false;
        }

        public void DoSearchAndWaitResults(string designName)
        {
            DriverContext.BrowserWait().Until(ExpectedConditions.ElementIsVisible(By.Id("products")));

            DriverContext.Driver.ScrollToTop();

            var txtSearch = DriverContext.Driver.FindElement(By.Id("txtDesignSearch"));
            txtSearch.Clear();
            txtSearch.SendKeys(designName);

            DriverContext.Driver.FindElement(By.Id("doSearching")).Click();
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
                    Driver.IsElementPresent(_dataThumbnailBox.Format(step.Argument.ProductDescription));
                    {
                        logger.Info($"ProductDescription: {step.Argument.ProductDescription}; element value: {_dataThumbnailBox.Format(step.Argument.ProductDescription).Value}");

                        var thumbnailBoxEle = Driver.WaitUntilPresentedElement(_dataThumbnailBox.Format(step.Argument.ProductDescription), BaseConfiguration.LongTimeout);
                        Driver.Actions().MoveToElement(thumbnailBoxEle).Perform();

                        Driver.GetElement(_actionMenuLocator.Format(step.Argument.ProductDescription, step.ControlId)).OnClickJavaScript();

                        if (step.ControlId.Equals(ActionTypes.Edit.ToString(), StringComparison.OrdinalIgnoreCase))
                        {
                            Driver.WaitUntilPresentedUrl(PageTypes.UserEditFormFilling.ToString());
                        }
                    }
                    break;

                case ControlTypes.Textbox:
                    Driver.GetElement<Textbox>(_textBoxLocator.Format(step.ControlId)).SetValue(step.ControlValue);
                    break;

                case ControlTypes.Button:
                    Driver.GetElement<Button>(_buttonLocator.Format(step.ControlId)).ClickTo();
                    break;

                case ControlTypes.Label:
                    Driver.IsElementPresent(_dataThumbnailBox.Format(step.Argument.ProductDescription));
                    {
                        var result = Driver.GetElement<Label>(_labelLocator.Format(step.Argument.ProductDescription, step.ControlId)).GetValue();
                        Console.WriteLine($"get designname: {result}");
                    }
                    break;
            }
        }
    }
}